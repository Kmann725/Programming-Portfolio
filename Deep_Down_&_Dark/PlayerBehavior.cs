//*****************************************************************************
// File Name :         PlayerBehavior.cs
// Author :            Kyle Manning
// Creation Date :     February 28, 2021
//
// Brief Description : Handles the player's actions in game. The player can
//                     move left and right, jump, use a whip by clicking the
//                     left mouse button to move around. Pressing c will send
//                     the player back to their checkpoint. The player also
//                     spawns particle effects when they land on the ground.
//
// Adam Field - added spawnLocation, the method changeSpawnLocation(),
//              the method GoToSpawnpoint(), and added jumpforce control
//              added screen shake to the player landing. 
//
// Andrew Lao - added sound effects to the player and whip
//*****************************************************************************
using System.Collections;
using UnityEngine.UI;
using UnityEngine;

public class PlayerBehavior : MonoBehaviour
{
    public ScreenShakeController screenShakeController;

    Vector3 startLocation = new Vector3(-5, -3.6f);
    Vector3 spawnLocation;

    public GameController gc;
    public GameObject landing;
    public Text whipIncreaseNotifier;
    private DistanceJoint2D joint;
    private Rigidbody2D rb2d;
    private SpriteRenderer sr;
    private LineRenderer lr;
    public LayerMask mask;
    [Tooltip("Movement speed on ground")]
    public float groundSpeed = 4f;
    [Tooltip("Movement speed while swinging on whip")]
    public float airSpeed = 150f;
    [Tooltip("Jumping power")]
    public float jumpForce = 225f;
    [Tooltip("Length of the whip")]
    public float distance = 5f;
    [Tooltip("How many times whip can be used without touching the ground")]
    public float whipCount = 2;
    public bool canMove = true;
    public bool canJump = true;
    public bool hitByEnemy = false;
    public bool inUse = false;
    Vector3 targetPos;

    public AudioClip jumping;
    public AudioClip falling;

    AudioSource walking;
    AudioSource death;
    AudioSource whipIn;
    AudioSource whipSwinging;

    bool isWalking = false;

    public Animator animator;

    public PolygonCollider2D collider;

    /// <summary>
    /// Assigns the components for variables used throughout the script
    /// </summary>
    private void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        joint = GetComponent<DistanceJoint2D>();
        lr = GetComponent<LineRenderer>();
        sr = GetComponent<SpriteRenderer>();
        walking = GetComponent<AudioSource>();
        collider = GetComponent<PolygonCollider2D>();

        AudioSource[] src = GetComponents<AudioSource>();
        walking = src[0];
        death = src[1];
        whipIn = src[2];
        whipSwinging = src[3];

        ChangeSpawnLocation(startLocation);

        whipIn.volume = 0.51f;
        whipSwinging.volume = 0.51f;
    }

    /// <summary>
    /// If the player presses 'C', the player is sent back to the checkpoint;
    /// runs the functions for jumpting and using the whip each frame
    /// </summary>
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            GoToSpawnPoint();
            joint.enabled = false;
            lr.enabled = false;
            inUse = false;

            if (jumpForce > 225)
            {
                jumpForce /= 2;
            }
        }

        Jumping();
        Whip();

        if(Input.GetKeyDown(KeyCode.T))
        {
            transform.position = new Vector3(6.3f, 94.75f, 0f);
        }

        //move to checkpoint after death
        //if(collider.enabled == false)
        //{
        //    t += Time.deltaTime / timeToReachTarget;
        //    transform.position = Vector3.Lerp(deathStartPosition, checkpointTarget, t);

        //    if(gameObject.transform.position == checkpointTarget)
        //    {
        //        collider.enabled = true;
        //    }
        //}
        
    }

    /// <summary>
    /// While the player is allowed to move, runs the function for walking every
    /// fixed update frame
    /// </summary>
    private void FixedUpdate()
    {
        if (canMove)
        {
            Walking();
        }
    }

    /// <summary>
    /// Handles both walking and swinging on the whip, both of which are
    /// performed by using the keys tied to the horizontal input axis
    /// </summary>
    private void Walking()
    {
        float xMove = Input.GetAxis("Horizontal");

        animator.SetFloat("Speed", Mathf.Abs(xMove));

        if (xMove < 0)
        {
            sr.flipX = true;
        }
        else
        {
            sr.flipX = false;
        }

        Vector3 movement = new Vector3(xMove, 0);
        movement *= Time.deltaTime * groundSpeed;

        if (!joint.enabled)
        {
            transform.position += movement;
        }
        else
        {
            movement *= Time.deltaTime * airSpeed;
            if (rb2d.velocity.x > -5 && rb2d.velocity.x < 5)
            {
                rb2d.AddForce(Vector2.right * movement);
            }
        }
    }

    /// <summary>
    /// Plays the walking sound effect when called
    /// </summary>
    private void IsWalking()
    {
        walking.Play();
    }

    /// <summary>
    /// When the jump button is pressed and the player can jump, an upwards force
    /// is applied to the player and the jumping sound effect is played
    /// </summary>
    private void Jumping()
    {
        if((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space)) && canJump)
        {
            canJump = false;
            rb2d.AddForce(Vector2.up * jumpForce);

            Vector3 camPos = Camera.main.transform.position;

            AudioSource.PlayClipAtPoint(jumping, camPos);
        }

        //once the player had a jump power up and they jump, it will reset to their normal jump force before the powerup
        //added by adam field
        if((Input.GetKeyDown(KeyCode.Space) && jumpForce >= 400f))
        {
            jumpForce /= 2;
        }

    }

    /// <summary>
    /// Handles behavior for the whip, making it appear and attach or disappear and detach
    /// from objects when the player clicks the left mouse
    /// </summary>
    private void Whip()
    {
        if (Input.GetButtonDown("Fire1") && !gc.isPaused)
        {
            targetPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            targetPos.z = 0;

            Vector3 camPos = Camera.main.transform.position;

            if (inUse)
            {
                inUse = false;
                joint.enabled = false;
                lr.enabled = false;
                animator.SetBool("isSwinging", false);
                if (rb2d.velocity.y < 0.5)
                {
                    rb2d.AddForce(Vector2.up * jumpForce);
                }
                else
                {
                    rb2d.AddForce(Vector2.up * jumpForce);
                }
            }
            else if (whipCount > 0)
            {
                RaycastHit2D hit = Physics2D.Raycast(transform.position, targetPos - transform.position, distance, mask);

                if (hit.collider != null)
                {
                    inUse = true;
                    joint.enabled = true;
                    lr.enabled = true;
                    canJump = false;
                    animator.SetBool("isSwinging", true);
                    whipIn.Play();
                    whipSwinging.Play();

                    joint.distance = Vector2.Distance(transform.position, hit.point);
                    joint.connectedAnchor = hit.point;

                    whipCount--;
                }
            }
        }

        lr.SetPosition(0, joint.connectedAnchor);
        lr.SetPosition(1, transform.position);
    }

    /// <summary>
    /// Handles collision behavior for the player; a boxcast is used for
    /// reseting jumping and the whip count; a death sound effect is played
    /// if an enemy is hit
    /// </summary>
    /// <param name="collision">the collider the object is interacting
    ///                         with</param>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        RaycastHit2D hit = Physics2D.BoxCast(rb2d.position,
                       new Vector2(0.4f, 0.5f), 0, Vector2.down, 0.5f, mask);

        if (collision.gameObject.tag == "World" && !joint.enabled)
        {
            Vector2 collsionForce = collision.relativeVelocity;
            float playerVelocity = rb2d.velocity.y;

            if (playerVelocity <= 0 && (transform.position.y > collision.transform.position.y))
            {
                screenShakeController.StartShake(.1f, collsionForce.y / 15);
            }

            if (canJump == false && hit.transform != null)
            {
                Vector3 particleSpawn = transform.position;
                particleSpawn.y -= 0.42f;
                Quaternion particleRotation = Quaternion.Euler(new Vector3(-90, 0, 0));

                Instantiate(landing, particleSpawn, particleRotation);
                canJump = true;
                StartCoroutine("WhipCountFeedback");
                whipCount = 2;
            }
        }
        
        if (collision.gameObject.tag == "Enemy")
        {
            death.Play();
        }
    }

    /// <summary>
    /// Changes the players spawn location when called
    /// </summary>
    /// <param name="newLocation">the new spawn location</param>
    public void ChangeSpawnLocation(Vector3 newLocation)
    {
        spawnLocation = newLocation;
    }

    private Vector3 deathStartPosition;
    private float t;
    private Vector3 checkpointTarget;
    private float timeToReachTarget;
    private float speedToReachTarget;

    /// <summary>
    /// Sends the player to the checkpoint
    /// </summary>
    public void GoToSpawnPoint()
    {
        StartCoroutine("CheckpointRespawnDelay");

        //this.gameObject.transform.position = spawnLocation;

        timeToReachTarget = 5f;
        speedToReachTarget = 5f;
        deathStartPosition = this.gameObject.transform.position;
        collider.enabled = false;
        rb2d.gravityScale = 0;
        StartCoroutine(MoveOverSpeed(gameObject, spawnLocation, speedToReachTarget));

    }

    /// <summary>
    /// Moves the player to the checkpoint at a steady rate
    /// </summary>
    /// <param name="objectToMove">the object being moved</param>
    /// <param name="end">the location to move towards</param>
    /// <param name="speed">speed of movement</param>
    /// <returns></returns>
    public IEnumerator MoveOverSpeed(GameObject objectToMove, Vector3 end, float speed)
    {
        while(Vector3.Distance(objectToMove.transform.position, end) > 0.1f)
        {
            objectToMove.transform.position = Vector3.MoveTowards(objectToMove.transform.position, end, speed * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
        collider.enabled = true;
        rb2d.gravityScale = 1;
    }

    public IEnumerator MoveOverSeconds (GameObject objectToMove, Vector3 end, float seconds)
    {
        float elapsedTime = 0;
        Vector3 startingPos = objectToMove.transform.position;
        while(elapsedTime < seconds)
        {
            objectToMove.transform.position = Vector3.Lerp(startingPos, end, (elapsedTime / seconds));
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        objectToMove.transform.position = end;
    }    

    public void SetDestination(Vector3 destination, float time)
    {
        t = 0;
        deathStartPosition = transform.position;
        timeToReachTarget = time;
        checkpointTarget = destination;
    }



    private IEnumerator CheckpointRespawnDelay()
    {
        rb2d.velocity = new Vector2(0f, 0f);
        canMove = false;
        yield return new WaitForSeconds(1f);
        canMove = true;
    }

    private IEnumerator WhipCountFeedback()
    {
        if (whipCount >= 2)
        {
            yield return new WaitForSeconds(0.1f);
        }
        else if (whipCount == 1)
        {
            whipIncreaseNotifier.text = "+1";
            whipIncreaseNotifier.enabled = true;
            yield return new WaitForSeconds(1.5f);
            whipIncreaseNotifier.enabled = false;
        }
        else if (whipCount == 0)
        {
            whipIncreaseNotifier.text = "+2";
            whipIncreaseNotifier.enabled = true;
            yield return new WaitForSeconds(1.5f);
            whipIncreaseNotifier.enabled = false;
        }
    }
}
