/*****************************************************************************
// File Name :         OrbCharging.cs
// Author :            Kyle Manning
// Contact :           khmanning@mail.bradley.edu
// Creation Date :     2023-01-26
//
// Updated by :        Kyle Manning 02/08/2023
//
// Brief Description : Handles objective progression in the castle level
//                     by increasing the orb fragment charge amount based on
//                     the amount of players within the fragment's radius.
*****************************************************************************/
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class OrbCharging : NetworkBehaviour
{
    Coroutine chargingRoutine;

    // Total charge and the amount the charge should change by
    [Header("Objective Modifiers")]
    [Tooltip("How many points a player adds to the charge per second")]
    public float pointsPerPlayer = 1;
    [Tooltip("How many points an enemy subtracts from the charge per second")]
    public float pointsPerEnemy = 1f;
    [Tooltip("How many enemies need to be near the orb for it to start losing charge")]
    public int enemyThreshold = 6;
    public float charge = 0;
    public float change = 0;
    [Tooltip("Value that triggers objective completion")]
    public int fullCharge = 100;
    [Tooltip("Value that triggers a game over")]
    public int lossCharge = -100;
    [Tooltip("Radius which a player or enemy must be within to effect the charge")]
    public float orbRadius = 15f;

    // Objective stage currently in progress
    private int stage = 1;

    // UI elements for displaying charge progress
    [Header("Charge Bar UI")]
    public Slider chargeBar;
    public Slider negativeChargeBar;
    public Image barColor;
    public Sprite positiveBar;
    public Sprite negativeBar;

    [Header("Orb Radius")]
    public GameObject radiusVisual;
    public Material radiusColor;
    public Color neutral;
    public Color positive;
    public Color negative;

    [Header("Dynamic Modifiers")]
    public float enemyValue1P = 0.25f;
    public float enemyValue2P = 0.5f;
    public float enemyValue3P = 0.75f;
    public float enemyValue4P = 1f;
    public float playerValue1P = 4f;
    public float playerValue2P = 2f;
    public float playerValue3P = 1.5f;
    public float playerValue4P = 1f;

    [Header("Lose Screen")]
    public GameObject loseScreen;

    private Collider[] previous;

    //Gabe: UI
    private GameObject objectivePointer;

    public GameObject playersReadyText;

    private void Start()
    {
        // Updates the size of the orb radius visual in game
        Vector3 updatedRadius = radiusVisual.GetComponent<Transform>().localScale;
        updatedRadius.x = 100 * orbRadius;
        updatedRadius.y = 100 * orbRadius;
        radiusVisual.GetComponent<Transform>().localScale = updatedRadius;

        radiusColor.SetColor("_MainColor", neutral);

        if (Tutorial.tutorial != null && Tutorial.tutorial.IsTutorial)
        {
            pointsPerPlayer = 20;
            TriggerTutChargeStart();
        }
        else
        {
            objectivePointer = GameObject.Find("Pointers Controller");
        }

        SetupPoints();
        Debug.Log("PPP:" + pointsPerPlayer);
    }

    public void SetupPoints()
    {
        if (IsServer)
        {
            switch (NetworkManager.Singleton.ConnectedClientsList.Count)
            {
                case 1:
                    pointsPerEnemy = enemyValue1P;
                    pointsPerPlayer = playerValue1P;
                    break;
                case 2:
                    pointsPerEnemy = enemyValue2P;
                    pointsPerPlayer = playerValue2P;
                    break;
                case 3:
                    pointsPerEnemy = enemyValue3P;
                    pointsPerPlayer = playerValue3P;
                    break;
                case 4:
                    pointsPerEnemy = enemyValue4P;
                    pointsPerPlayer = playerValue4P;
                    break;
            }
        }
    }

    /// <summary>
    /// Public method called in OrbMovement to trigger next objective starts
    /// </summary>
    public void TriggerChargeStart()
    {
        if (IsHost && chargingRoutine == null)
        {
            chargingRoutine = StartCoroutine(Charging());
        }
        previous = new Collider[1];
        InvokeRepeating("EnemyOutline", 0.5f, 0.5f);
    }

    private void TriggerTutChargeStart()
    {
        chargingRoutine = StartCoroutine(Charging());
    }

    /// <summary>
    /// Updates the charge bar UI and checks to see if the loss state occurred
    /// </summary>
    [ClientRpc]
    public void ChangeChargeClientRpc(float nCharge)
    {
        charge = nCharge;

        playersReadyText.SetActive(false);

        // Caps charge value at fullCharge
        if (charge >= fullCharge)
        {
            charge = fullCharge;
            ReadyUp.instance.ready = false;
        }

        // Checks to see if the charge is at or below the loss threshold, and triggers a game over if true
        if (charge <= lossCharge)
        {
            charge = lossCharge;

            loseScreen.SetActive(true);

            // Makes the cursor visible and prevents the player from accessing the pause menu
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
            PauseMenuBehavior.PauseMenu.CanPause = false;
        }

        // Sets the value of the charge bar
        if (charge == 0)
        {
            chargeBar.value = 0;
            negativeChargeBar.value = 0;
        }
        else if (charge > 0)
        {
            negativeChargeBar.value = 0;
            chargeBar.value = charge / fullCharge;
        }
        else if(charge < 0)
        {
            chargeBar.value = 0;
            negativeChargeBar.value = charge / lossCharge;
        }
    }

    /// <summary>
    /// Changes the color of the charge bar when called
    /// </summary>
    /// <param name="i">Used as a binary to determine color change</param>
    [ClientRpc]
    public void BarColorChangeClientRpc(int i)
    {
        // Changes the bar to the positive color
        if (i == 0)
        {
            barColor.sprite = positiveBar;
        }
        // Changes the bar to the negative color
        else
        {
            barColor.sprite = negativeBar;
        }
    }

    /// <summary>
    /// Changes the color of the radius to reflect whether the players are charging the orb, enemies are decharging
    /// the orb, or if no change to the orb's charge is being made
    /// </summary>
    /// <param name="i">value of change</param>
    [ClientRpc]
    public void RadiusColorChangeClientRpc(float i)
    {
        if (i > 0)
        {
            radiusColor.SetColor("_MainColor", positive);
        }
        else if (i < 0)
        {
            radiusColor.SetColor("_MainColor", negative);
        }
        else
        {
            radiusColor.SetColor("_MainColor", neutral);
        }
    }

    public void EnemyOutline()
    {
        Collider[] current = Physics.OverlapSphere(transform.position, orbRadius);

        foreach (Collider col in current)
        {
            if (col != null && col.gameObject.tag == "Enemy")
            {
                MeshOutline[] outlines = col.gameObject.GetComponentsInChildren<MeshOutline>();

                foreach (MeshOutline outline in outlines)
                {
                    outline.enabled = true;
                }
            }
        }

        foreach (Collider col in previous)
        {
            if (col != null &&  col.gameObject.tag == "Enemy")
            {
                bool inRadius = false;

                for (int i = 0; i < current.Length; i++)
                {
                    if (col == current[i])
                    {
                        inRadius = true;
                        break;
                    }
                }

                if (!inRadius)
                {
                    MeshOutline[] outlines = col.gameObject.GetComponentsInChildren<MeshOutline>();

                    foreach (MeshOutline outline in outlines)
                    {
                        outline.enabled = false;
                    }
                }
            }
        }

        previous = current;
    }

    /// <summary>
    /// Adjusts the orbs charge by a per second change, and when the threshold is
    /// reached triggers a world change
    /// </summary>
    /// <returns></returns>
    IEnumerator Charging()
    {
        // Adjusts the charge until it reaches 100 or -100
        while (charge < fullCharge && charge > lossCharge)
        {
            // Stores the value of the previous charge
            var previous = charge;

            change = 0;

            var playerCount = 0;
            var enemyCount = 0;
            // Gets all colliders within range of the orb
            Collider[] cols = Physics.OverlapSphere(transform.position, orbRadius);
            foreach (Collider col in cols)
            {
                // Increments playerCount if the collider's object is tagged as a player
                if (col.gameObject.tag == "Player")
                {
                    playerCount++;
                }
                // Increments enemyCount if the collider's object is tagged an an enemy
                else if (col.gameObject.tag == "Enemy")
                {
                    enemyCount++;
                }
            }

            if (Tutorial.tutorial != null && Tutorial.tutorial.IsTutorial)
            {
                if (enemyCount > 0)
                    change = -(int)(enemyCount * pointsPerEnemy);
                else
                    change = playerCount * pointsPerPlayer;
            }
            else
            {
                if (charge < 0)
                {
                    change = (float)(playerCount * (pointsPerPlayer * 2)) - (enemyCount * pointsPerEnemy);
                }
                else
                {
                    change = (float)(playerCount * pointsPerPlayer) - (enemyCount * pointsPerEnemy);
                }
            }

            RadiusColorChangeClientRpc(change);

            // Adds the change to the current charge to get the new charge
            charge += change;

            // Changes the color of the charge bar when the charge value switches from negative to positive
            // and vice versa
            if (previous <= 0 && charge > 0)
            {
                BarColorChangeClientRpc(0);
            }
            if (previous >= 0 && charge < 0)
            {
                BarColorChangeClientRpc(1);
            }

            if (Tutorial.tutorial != null && Tutorial.tutorial.IsTutorial && charge < -80)
                charge = -80;

            // Updates charge bar UI and lose condition checking
            ChangeChargeClientRpc(charge);

            yield return new WaitForSeconds(1f);
        }

        // Initiates a world change event depending on which orb the script is on
        if (charge >= fullCharge)
        {

            SetOpc();

            if (Tutorial.tutorial != null && Tutorial.tutorial.IsTutorial)
            {
                Tutorial.tutorial.TutorialEnd();
                chargingRoutine = null;
                yield break;
            }

            switch (stage)
            {
                case 1:
                    GlobalValues.gameController.worldChange1.Invoke();
                    stage++;
                    charge = 0;
                    break;
                case 2:
                    GlobalValues.gameController.worldChange2.Invoke();
                    stage++;
                    charge = 0;
                    break;
                case 3:
                    GlobalValues.gameController.worldChange3.Invoke();
                    stage++;
                    charge = 0;
                    break;
                case 4:
                    GlobalValues.gameController.worldChange4.Invoke();
                    break;
            }
        }

        chargingRoutine = null;
    }

    private void SetOpc()
    {
        if (!(Tutorial.tutorial != null && Tutorial.tutorial.IsTutorial))
            objectivePointer.GetComponent<ObjectivePointerController>().SetPA(true, stage);
    }
}
