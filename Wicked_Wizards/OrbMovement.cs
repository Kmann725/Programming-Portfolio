/*****************************************************************************
// File Name :         OrbMovement.cs
// Author :            Kyle Manning
// Contact :           khmanning@mail.bradley.edu
// Creation Date :     2022-11-26
//
// Updated by :        Kyle Manning 02/08/2023
//
// Brief Description : Contains the functionality for moving the orb
//                     fragments. Moves called from WorldChangeEvents.cs
*****************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbMovement : MonoBehaviour
{
    [Header("Orb Fragments")]
    public GameObject fragment2;
    public GameObject fragment3;

    [Header("")]
    [Tooltip("Object that has the orb charging script on it")]
    public OrbCharging orbCharge;

    [Header("Orb Stages")]
    public GameObject[] stage1;
    public GameObject[] stage2;
    public GameObject[] stage3;
    public GameObject[] stage4;

    [Header("Movement Waypoint Sets")]
    public Vector3[] waypoints1;
    public Vector3[] waypointsblocked1;
    public Vector3[] waypoints2;
    public Vector3[] waypointsblocked2;
    public Vector3[] waypoints3;

    [Header("Movement Speed")]
    public float moveSpeed = 1.5f;

    [Header("Invisible walls")]
    public GameObject forestInvis;
    public GameObject caveInvis;

    [Header("Enemy Spawns")]
    public GameObject forestEnemies;
    public GameObject caveEnemies;
    public GameObject mainEnemies;

    int currentMove;

    public bool ableToReady = true;

    public GameObject playersReadyText;


    /// <summary>
    /// Public method to be called by WorldChangeEvents to initiate orb movement
    /// </summary>
    public void OrbMove(int movement)
    {
        // Switch case to determine which set of fragments should be moved and activated
        switch (movement)
        {
            case 1:
                // Calls the coroutine to move the first orb fragment
                StartCoroutine(Move(1));
                // Begins the second fragment's particle system and animation and moves the base
                // hitbox and energy wave effect source to the location of the second fragment
                fragment2.GetComponentInChildren<ParticleSystem>().Play();
                StartCoroutine(FragmentActivation(2));
                break;
            case 2:
                // Calls the coroutine to move the second orb fragment
                StartCoroutine(Move(2));
                // Begins the third fragment's particle system and animation and moves the base
                // hitbox and energy wave effect source to the location of the third fragment
                fragment3.GetComponentInChildren<ParticleSystem>().Play();
                StartCoroutine(FragmentActivation(3));
                forestEnemies.SetActive(false);
                mainEnemies.SetActive(true);
                break;
            case 3:
                // Calls the coroutine to move the third orb fragment
                StartCoroutine(Move(3));
                caveEnemies.SetActive(false);
                mainEnemies.SetActive(true);
                break;
            case 4:
                break;
            case 5:
                StartCoroutine(Move(4));
                mainEnemies.SetActive(false);
                caveEnemies.SetActive(true);
                break;
            case 6:
                StartCoroutine(Move(5));
                mainEnemies.SetActive(false);
                forestEnemies.SetActive(true);
                break;
        }
    }

    /// <summary>
    /// Takes an array of Vector3 points and moves the orb towards them, switching
    /// to the next point in the array when it gets within a certain distance of the
    /// current point
    /// </summary>
    /// <param name="objective">Which of objective stage this move occurs after the completion of</param>
    /// <returns></returns>
    IEnumerator Move(int objective)
    {
        // switch case to determine which fragment movement should be done
        switch (objective)
        {
            case 1:
                foreach (Vector3 point in waypoints1)
                {
                    // Moves the orb towards the current point every frame while it is not within 0.1 units of the point
                    while (Vector3.Distance(transform.position, point) > 0.1f)
                    {
                        var step = moveSpeed * Time.deltaTime;

                        transform.position = Vector3.MoveTowards(transform.position, point, step);

                        yield return new WaitForSeconds(0.01f);
                    }
                }

                forestInvis.GetComponent<Damageable>().god = false;
                // Starts the second objective
                //orbCharge.TriggerChargeStart();

                break;
            case 2:
                foreach (Vector3 point in waypoints2)
                {
                    // Moves the orb towards the current point every frame while it is not within 0.1 units of the point
                    while (Vector3.Distance(transform.position, point) > 0.1f)
                    {
                        var step = moveSpeed * Time.deltaTime;

                        transform.position = Vector3.MoveTowards(transform.position, point, step);

                        yield return new WaitForSeconds(0.01f);
                    }
                }

                caveInvis.GetComponent<Damageable>().god = false;
                // Starts the third objective
                //orbCharge.TriggerChargeStart();

                break;
            case 3:
                foreach (Vector3 point in waypoints3)
                {
                    // Moves the orb towards the current point every frame while it is not within 0.1 units of the point
                    while (Vector3.Distance(transform.position, point) > 0.1f)
                    {
                        var step = moveSpeed * Time.deltaTime;

                        transform.position = Vector3.MoveTowards(transform.position, point, step);

                        yield return new WaitForSeconds(0.01f);
                    }
                }

                ableToReady = true;
                playersReadyText.SetActive(true);

                // Activates the final parts of the orb vfx
                foreach(GameObject part in stage4)
                {
                    part.SetActive(true);
                }

               // Starts the final objective
                //orbCharge.TriggerChargeStart();

                break;
            case 4:
                foreach (Vector3 point in waypointsblocked2)
                {
                    while (Vector3.Distance(transform.position, point) > 0.1f)
                    {
                        var step = moveSpeed * Time.deltaTime;

                        transform.position = Vector3.MoveTowards(transform.position, point, step);

                        yield return new WaitForSeconds(0.01f);
                    }
                }

                ableToReady = true;
                playersReadyText.SetActive(true);

                // Hides the fragment already present at the final point and deactivates the two combined orb chunks
                fragment3.SetActive(false);
                stage1[0].SetActive(false);
                stage2[0].SetActive(false);

                // Activates the full orb and more of the vfx
                foreach (GameObject part in stage3)
                {
                    part.SetActive(true);
                }

                break;
            case 5:
                foreach (Vector3 point in waypointsblocked1)
                {
                    // Moves the orb towards the current point every frame while it is not within 0.1 units of the point
                    while (Vector3.Distance(transform.position, point) > 0.1f)
                    {
                        var step = moveSpeed * Time.deltaTime;

                        transform.position = Vector3.MoveTowards(transform.position, point, step);

                        yield return new WaitForSeconds(0.01f);
                    }
                }

                ableToReady = true;
                playersReadyText.SetActive(true);

                // Hides the fragment already present at the final point and activates more parts of the orb
                fragment2.SetActive(false);
                foreach (GameObject part in stage2)
                {
                    part.SetActive(true);
                }

                break;

        }
    }

    /// <summary>
    /// Activates the specified fragment of the orb, making it begin animating and
    /// glowing via particle effect.
    /// </summary>
    /// <param name="fragment">Which orb fragment should now be activated</param>
    /// <returns></returns>
    IEnumerator FragmentActivation(int fragment)
    {
        // switch case to determine which orb fragment which should activated
        switch (fragment)
        {
            case 2:
                // Sets a point above the fragments current position for it to move to before it begins animating
                Vector3 activePos = fragment2.transform.position;
                activePos.y += 1;

                // Moves the fragment to the above point
                while (Vector3.Distance(fragment2.transform.position, activePos) > 0.1)
                {
                    var step = 0.5f * Time.deltaTime;

                    fragment2.transform.position = Vector3.MoveTowards(fragment2.transform.position, activePos, step);

                    yield return new WaitForSeconds(0.01f);
                }

                // Enables the fragment's animation
                fragment2.GetComponentInChildren<Animator>().enabled = true;
                break;
            case 3:
                // Sets a point above the fragments current position for it to move to before it begins animating
                activePos = fragment3.transform.position;
                activePos.y += 1;

                // Moves the fragment to the above point
                while (Vector3.Distance(fragment3.transform.position, activePos) > 0.1)
                {
                    var step = 0.5f * Time.deltaTime;

                    fragment3.transform.position = Vector3.MoveTowards(fragment3.transform.position, activePos, step);

                    yield return new WaitForSeconds(0.01f);
                }

                // Enables the fragment's animation
                fragment3.GetComponentInChildren<Animator>().enabled = true;
                break;
        }
    }
}
