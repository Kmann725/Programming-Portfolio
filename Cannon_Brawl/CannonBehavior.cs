/*******************************************************************************
// File Name :      CannonBehavior.cs
// Author :         Avery Macke, Kyle Manning, Jason Czech
// Creation Date :  19 January 2022
// 
// Description :    Allows for various cannon functionality; initializes cannon
                    variables & components.
*******************************************************************************/
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.SceneManagement;

public class CannonBehavior : MonoBehaviour
{


    static Touch[] touchArray = new Touch[6];


    #region Cannon Variables
    /// <summary>
    /// Enum for each player
    /// </summary>
    public enum Player {One, Two};

    [Header("General Variables")]

    [Tooltip("Player one or two")]
    public Player player;

    [Tooltip("Ship Stats ScriptableObject")]
    public ShipStats shipStatsMain;

    [Tooltip("Ship Audio ScriptableObject")]
    public ShipAudio shipAudio;

    [Tooltip("Power Up Stats ScriptableObject")]
    public PowerUpStats powerUpStats;

    [Header("Cannon Variables")]

    [Tooltip("List of cannonball types")]
    public List<GameObject> cannonballList;

    /// <summary>
    /// Cannoball from list to fire
    /// </summary>
    private GameObject cannonballToFire;

    [Tooltip("Cannonball spawn point")]
    public GameObject cannonballSpawn;

    [Tooltip("Multishot left and right spawn points")]
    public GameObject multiSpawnLeft, multiSpawnRight;

    [Tooltip("Cannon firing particle system")]
    public GameObject cannonFirePS;

    [Tooltip("Scale of trajectory line renderer")]
    public float lineRenderScale = 20f;

    [Header("Input Variables")]

    [Tooltip("Distance input must be to interact with cananon")]
    public float cannonInputRadius;

    [Tooltip("Distance input must be before cannon rotates or fires")]
    public float cannonActionRadius = 100f;

    [Header("Cannon Rotation")]

    [Tooltip("Maximum cannon rotation")]
    public float maxRotation;

    [Tooltip("Minimum cannon rotation")]
    public float minRotation;

    /// <summary>
    /// Reference to GameManager script
    /// </summary>
    private GameManager gm;

    /// <summary>
    /// Reference to childed cannon GameObject
    /// </summary>
    private GameObject cannon;

    /// <summary>
    /// Reference to cannon's CircleCollider2D component
    /// </summary>
    private CircleCollider2D cc2d;

    /// <summary>
    /// Reference to cannon's AudioSource component;
    /// </summary>
    private AudioSource audio;

    /// <summary>
    /// Rotation offset; cannon faces away from mouse
    /// </summary>
    private float rotateOffset = -270.0f;

    /// <summary>
    /// Radius in which touch inputs on cannons are registered                  // check this one
    /// </summary>
    static float touchRadius;

    /// <summary>
    /// Identifier for which Touch has touched the cannon
    /// </summary>
    public int fingerId = -1;

    /// <summary>
    /// Cannon's inital rotation at start
    /// </summary>
    private Quaternion initRotation;

    /// <summary>
    /// Cannon rate of fire
    /// </summary>
    private float rateOfFire;

    /// <summary>
    /// Cannon rotations peed
    /// </summary>
    private float rotationSpeed;

    /// <summary>
    /// Reference to LineRenderer component
    /// </summary>
    private LineRenderer lr;

    /// <summary>
    /// Cannonball's attack power; determines how much damage is dealt
    /// </summary>
    [HideInInspector]
    public float attack;

    /// <summary>
    /// Cannonball speed
    /// </summary>
    [HideInInspector]
    public float cannonballSpeed;

    /// <summary>
    /// Determines whether cannon can fire/be interacted with
    /// </summary>
    [HideInInspector]
    public bool cannonActive = true;

    #endregion

    /// <summary>
    /// Called at start; initializes variables
    /// </summary>
    void Awake()
    {
        InitializeVariables();
    }

    /// <summary>
    /// Called once per frame; manages multi-touch inputs, cannon rotation
    /// </summary>
    void Update()
    { 

        if (fingerId != -1)
        {
            foreach (CannonBehavior cb in FindObjectsOfType<CannonBehavior>())
            {
                if (cb != this && cb.fingerId == fingerId)
                {
                    print("umm problem lmao");
                }
            }
        }

        // if the cannon does not have a touch input associated with it
        if (fingerId == -1)
        {
            // loops through every registered touch input
            for (int i = 0; i < Input.touchCount; i++)
            {
                var t = Input.GetTouch(i);

                // gets the position of the touch input in world space
                Vector2 touchPos = Camera.main.ScreenToWorldPoint(t.position);

                // checks if the touch input has just began and if it is within a specified distance of the cannon
                if (Vector2.Distance(touchPos, transform.position) <= 0.35f && t.phase == TouchPhase.Began)
                {
                    // assigns the id of the touch input to fingerId, so cannon will follow this touch input
                    fingerId = t.fingerId;
                    break;
                }
            }
        }
        // if the cannon has a touch input associated with it
        else
        {
            // touch to be used for rotation, firing, and line render
            Touch touchInput = new Touch();
            // iterates through each currently registered touch input
            var valid = false;
            for (int i = 0; i < Input.touchCount; i++)
            {
                // temporary variable storing
                var t = Input.GetTouch(i);
                // if id saved to cannon matches touch input id
                if (t.fingerId == fingerId)
                {
                    valid = true;
                    touchInput = t;
                    break;
                }
            }

            if (valid)
            {
                Vector2 touchPos = Camera.main.ScreenToWorldPoint(touchInput.position);

                // Called when touch is moved/dragged
                if (touchInput.phase == TouchPhase.Moved /*&& Vector2.Distance(touchPos,
                transform.position) >= 1f*/ && CheckCanFireTouch())
                {
                    RotateFromTouch(touchInput);
                    DrawTrajectoryTouch(touchInput);
                }
                // Called when touch is released
                else if (touchInput.phase == TouchPhase.Ended /*&& Vector2.Distance(touchPos,
                transform.position) >= 0.15f*/ && CheckCanFireTouch())
                {
                    Fire();
                    print(touchInput.fingerId);
                    //StartCoroutine(UpdateFingerIDs());
                }

            }
            else
            {
                fingerId = -1;
            }
        }
    }

    IEnumerator UpdateFingerIDs()
    {
        yield return new WaitForEndOfFrame();

        foreach (CannonBehavior cb in FindObjectsOfType<CannonBehavior>())
        {
            if (cb.fingerId > fingerId)
            {
                cb.fingerId--;
            }
        }

        fingerId = -1;
    }

    /// <summary>
    /// Initializes variables from CannonInitialize script
    /// </summary>
    private void InitializeVariables()
    {
        cc2d = GetComponent<CircleCollider2D>();

        cc2d.radius = cannonInputRadius;
        touchRadius = cannonInputRadius;

        initRotation = transform.rotation;

        cannon = transform.Find("Cannon").gameObject;

        cannonballToFire = cannonballList[0];

        rateOfFire = shipStatsMain.cannonRateOfFire;
        rotationSpeed = shipStatsMain.cannonRotationSpeed;
        cannonballSpeed = shipStatsMain.cannonballSpeed;
        attack = shipStatsMain.shipAttack;

        lr = GetComponent<LineRenderer>();
        lr.enabled = false;

        gm = FindObjectOfType<GameManager>();

        audio = GetComponent<AudioSource>();
    }

    #region Mouse Input
    /// <summary>
    /// Called when player clicks on cannon and continues holding mouse down;
    /// rotates cannon in opposite direction of mouse.
    /// </summary>
    private void OnMouseDrag()
    {
        if (FindMouseDist() > cannonActionRadius && CheckCanFireMouse())
        {
            RotateFromMouse();
            DrawTrajectoryMouse();
        }
    }

    /// <summary>
    /// Called when mouse is released; fires cannonball and resets cannon
    /// rotation
    /// </summary>
    private void OnMouseUp()
    {
        if (CheckCanFireMouse() && FindMouseDist() >= cannonActionRadius)
        {
            Fire();
        }
    }

    #endregion
    #region Rotation

    /// <summary>
    /// Rotate cannon in opposite direction of touch
    /// </summary>
    private void RotateFromTouch(Touch touchInput)
    {
        Vector3 touchPos = touchInput.position;
        Vector3 objectPos = Camera.main.WorldToScreenPoint(transform.position);
        Vector3 direction = touchPos - objectPos;

        Rotate(direction);

        //float angle = (Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);

        //Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle +
        //                                                        rotateOffset));

        //transform.rotation = Quaternion.RotateTowards(transform.rotation,
        //                     targetRotation, rotationSpeed * Time.deltaTime);
    }

    /// <summary>
    /// Rotates cannon in the opposite direction of the mouse
    /// </summary>
   private void RotateFromMouse()
    {
        Vector3 mousePos = Input.mousePosition;
        Vector3 objectPos = Camera.main.WorldToScreenPoint(transform.position);
        Vector3 direction = mousePos - objectPos;

        Rotate(direction);
    }

    /// <summary>
    /// Rotates cannon in opposite direction of input
    /// </summary>
    /// <param name="direction">Direction from input to object</param>
    private void Rotate(Vector3 direction)                                      // Get clamp working right
    {
        float angle = (Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);

        //angle = Mathf.Clamp(angle, minRotation, maxRotation);

        Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle +
                                                                rotateOffset));

        transform.rotation = Quaternion.RotateTowards(transform.rotation,
                             targetRotation, rotationSpeed * Time.deltaTime);
    }
    
    /// <summary>
    /// Smoothly rotates cannon back to initial rotation
    /// </summary>
    /// <returns></returns>
    IEnumerator ResetRotation()
    {
        // Difference between current and initial rotation
        float rotDiff = transform.rotation.z - initRotation.z;

        while (rotDiff > 0.1 || rotDiff < -0.1)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation,
                             initRotation, 100f * Time.deltaTime);

            rotDiff = transform.rotation.z - initRotation.z;

            yield return null;
        }

        transform.rotation = initRotation;

        StopCoroutine(ResetRotation());
    }
    #endregion

    /// <summary>
    /// Finds and returns distance between cannon and mouse
    /// </summary>
    /// <returns>Distance between cannon and mouse</returns>
    private float FindMouseDist()
    {
        Vector3 mousePos = Input.mousePosition;
        Vector3 objectPos = Camera.main.WorldToScreenPoint(transform.position);
        float dist = Vector3.Distance(mousePos, objectPos);

        return dist;
    }

    #region Firing
    /// <summary>
    /// Instantiates cannonball, sets cannon to inactive
    /// </summary>
    private void Fire()
    {
        cannonActive = false;
        cc2d.enabled = false;

        cannon.GetComponent<SpriteRenderer>().color = Color.grey;               // other way to make inactive cannons clear?

        lr.enabled = false;

        GameObject cannonballToFire = this.cannonballToFire;                        // Will be able to fire different cannonball types if we get to upgrades

        GameObject psToSpawn = Instantiate(cannonFirePS, cannonballSpawn.transform);
        psToSpawn.transform.parent = null;

        Instantiate(cannonballToFire, cannonballSpawn.transform.position,
            cannonballSpawn.transform.rotation, transform);

        // Multishot
        if (cannonballToFire == cannonballList[2])
        {
            Instantiate(cannonballToFire, cannonballSpawn.transform.position,
            multiSpawnLeft.transform.rotation, transform);

            Instantiate(cannonballToFire, cannonballSpawn.transform.position,
            multiSpawnRight.transform.rotation, transform);
        }

        audio.PlayOneShot(shipAudio.cannonFire);

        Invoke("Reload", rateOfFire);
        StartCoroutine(ResetRotation());

        SetCannonball(0);
    }

    /// <summary>
    /// Allows cannon to be fired again
    /// </summary>
    private void Reload()
    {
        cannonActive = true;
        cc2d.enabled = true;

        ResetRotation();
        cannon.GetComponent<SpriteRenderer>().color = Color.white;
    }

    #endregion

    #region Power-Ups

    /// <summary>
    /// Sets cannonball to be fired
    /// </summary>
    /// <param name="cannonballIndex">Index of cannonball to fire</param>
    public void SetCannonball(int cannonballIndex)
    {
        cannonballToFire = cannonballList[cannonballIndex];
    }

    /// <summary>
    /// Reduces reload speed
    /// </summary>
    public void ReduceReload()
    {
        Reload();
        StartCoroutine(ResetRotation());

        SetCannonball(0);

        StartCoroutine(ReduceReloadRoutine());
    }

    /// <summary>
    /// Reduces reload speed
    /// </summary>
    IEnumerator ReduceReloadRoutine()
    {
        rateOfFire *= powerUpStats.cooldownMod;

        yield return new WaitForSeconds(powerUpStats.cooldownDuration);

        rateOfFire = shipStatsMain.cannonRateOfFire;
    }


    #endregion

    #region Trajectory

    /// <summary>
    /// Draws trajectory of cannonball using LineRenderer; touch countrols
    /// </summary>
    private void DrawTrajectoryTouch(Touch touchInput)                          // Change from line renderer to something else
    {
        lr.enabled = true;
        //Vector3 touchPos = touchInput.position;
        Vector3 touchPos = Camera.main.ScreenToWorldPoint(touchInput.position);

        lr.SetPosition(0, transform.position);

        //Vector3 trajectoryPos = ((transform.position - Camera.main.ScreenToWorldPoint(touchPos)).normalized)
        //                          * lineRenderScale;

        Vector3 trajectoryPos = ((transform.position - touchPos).normalized)
                                 * lineRenderScale;

        trajectoryPos += transform.position;

        trajectoryPos = new Vector3(trajectoryPos.x, trajectoryPos.y, -1.05f);

        lr.SetPosition(1, trajectoryPos);
    }

    /// <summary>
    /// Draws trajectory of cannonball using LineRenderer; mouse controls
    /// </summary>
    private void DrawTrajectoryMouse()                                          // Change from line renderer to something else
    {
        lr.enabled = true;

        lr.SetPosition(0, transform.position);

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 trajectoryPos = ((transform.position - mousePos).normalized)
                                  * lineRenderScale;

        trajectoryPos += transform.position;

        //Locks the Z axis of the trajectoryPos. Makes the line longer but fixes distortion.
        trajectoryPos = new Vector3(trajectoryPos.x, trajectoryPos.y, -1.05f);

        lr.SetPosition(1, trajectoryPos);
    }

    #endregion
    #region Check Can Fire

    /// <summary>
    /// Checks if playercana fire with touch input
    /// </summary>
    /// <returns></returns>
    private bool CheckCanFireTouch()
    {
        return cannonActive && gm.touchInputActive && !gm.gameOver &&
               SceneManager.GetActiveScene().name != "ShipCustom";
    }

    /// <summary>
    /// Checks if player can fire with mouse input
    /// </summary>
    /// <returns></returns>
    private bool CheckCanFireMouse()
    {
        return cannonActive && !gm.touchInputActive && !gm.gameOver &&
               SceneManager.GetActiveScene().name != "ShipCustom";
    }

    #endregion

    /// <summary>
    /// Allows for cannon collider to be enabled or disabled
    /// </summary>
    /// <param name="enabled">Sets cannon collider's enabled status</param>
    public void SetCollider(bool enabled)
    {
        cc2d.enabled = enabled;
    }
}
