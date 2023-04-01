using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody2D rb;
    public float moveSpeed, dashForce, dashCooldown;
    float dashTimer;
    public Image dashFill;
    public bool canMove = true, dashing = false;

    public GameObject mainCam;
    public GameObject doorLocks;
    public GameObject winScreen;
    public GameObject loseScreen;
    public GameObject prompt;
    public Text finishTime;
    public TextMeshProUGUI floor;
    private GameObject tutorialItems;
    private Timer timer;

    private LevelGenerator lg;
    private AudioSource[] audsrc;
    public Animator anim;
    public SpriteRenderer sR;

    public static int currentLevel = 1;

    private bool atExit = false;
    public bool gameOver = false;

    public int currentEnemies = 0;
    public WeaponData[] allWeps;
    public GameObject randomWep;

    // Start is called before the first frame update
    void Start()
    {
        allWeps = Resources.LoadAll<WeaponData>("Weapons");

        floor.text = "Floor " + currentLevel;
        Time.timeScale = 1;

        rb = GetComponent<Rigidbody2D>();
        //Anim = GetComponentInChildren<Animator>();

        lg = GameObject.FindGameObjectWithTag("LevelGenerator").GetComponent<LevelGenerator>();
        audsrc = GetComponents<AudioSource>();
        timer = GameObject.FindGameObjectWithTag("Timer").GetComponent<Timer>();

        tutorialItems = GameObject.FindGameObjectWithTag("tutorial");

        doorLocks = mainCam.transform.GetChild(0).gameObject;

        if (currentLevel != 1)
        {
            Destroy(tutorialItems);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (canMove)
        {
            float x = Input.GetAxis("Horizontal");
            float y = Input.GetAxis("Vertical");

            Vector2 vel = new Vector2();
            if (x != 0)
                vel.x = x;
            if (y != 0)
                vel.y = y;
            vel = vel.normalized * moveSpeed;
            if (vel.x == 0)
                vel.x = rb.velocity.x;
            if (vel.y == 0)
                vel.y = rb.velocity.y;

            rb.velocity = vel;

            // Animation Stuff
            // Concerning the Y axis
            if (y > 0)
            {
                anim.SetFloat("moveY", 1);
                anim.SetBool("noYInput", false);
            }
            else if(y < 0)
            {
                anim.SetFloat("moveY", -1);
                anim.SetBool("noYInput", false);
            }
            else
            {
                anim.SetFloat("moveY", 0);
                anim.SetBool("noYInput", true);
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                if(Time.time > dashTimer)
                    StartCoroutine(Dash());
            }

            // Concerning the X axis

            if (x > 0)
            {
                anim.SetFloat("moveX", 1);
                anim.SetBool("noXInput", false);
                sR.flipX = false;
            }
            else if (x < 0)
            {
                anim.SetFloat("moveX", -1);
                anim.SetBool("noXInput", false);
                sR.flipX = true;
            }
            else
            {
                anim.SetFloat("moveX", 0);
                anim.SetBool("noXInput", true);  
            }

            if(Input.GetMouseButton(0))
            {
                anim.SetBool("isShooting", true);
            }
            else
            {
                anim.SetBool("isShooting", false);
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (Time.time > dashTimer)
                    StartCoroutine(Dash());
            }
        }

        dashFill.fillAmount = 1 - (dashTimer - Time.time) / dashCooldown;

        if (Input.GetKeyDown(KeyCode.E) && atExit)
        {
            if (lg.endLevel != currentLevel)
            {
                StartCoroutine(Exit());
            }
            else
            {
                winScreen.SetActive(true);
                
                if (timer.seconds < 10)
                {
                    finishTime.text = "Run completed in " + timer.minutes + ":0" + timer.seconds;
                }
                else
                {
                    finishTime.text = "Run completed in " + timer.minutes + ":" + timer.seconds;
                }

                gameOver = true;
                Time.timeScale = 0;
            }
        }

        if (gameOver && Input.GetKeyDown(KeyCode.T))
        {
            currentLevel = 1;
            Time.timeScale = 1;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Room")
        {
            Vector3 newCamPos = other.gameObject.transform.position;
            newCamPos.z = -10;
            mainCam.transform.position = newCamPos;

            currentEnemies = 0;
            
            foreach(Transform child in other.gameObject.transform)
            {
                if (child.gameObject.CompareTag("Enemy") || child.gameObject.CompareTag("EnemyProj"))
                {
                    BaseEnemy e = child.gameObject.GetComponent<BaseEnemy>();
                    if (e)
                    {
                        e.MakeReal();
                        currentEnemies++;
                    }
                }
            }

            if (currentEnemies > 0)
            {
                transform.Translate((other.transform.position - transform.position).normalized * 0.5f);
                doorLocks.SetActive(true);
            }
        }

        if(other.gameObject.tag == "RandomWeapon")
        {
            RollRandomWep();
            Destroy(other.gameObject);
        }

        if (other.gameObject.tag == "Finish")
        {
            atExit = true;
            prompt.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Finish")
        {
            atExit = false;
            prompt.SetActive(false);
        }
    }

    public void GameOver()
    {
        gameOver = true;
        loseScreen.SetActive(true);
        Time.timeScale = 0;
    }

    public void Reset()
    {
        currentLevel = 1;
        Timer.time = 0;

        PlayerController.weapons = new List<WeaponData>();
        PlayerController.weapons.Add(Resources.LoadAll<WeaponData>("Weapons")[0]);
    }

    IEnumerator Dash()
    {
        canMove = false;
        dashing = true;
        Vector2 force = dashForce * rb.velocity.normalized;
        rb.velocity = force;
        dashTimer = Time.time + dashCooldown;

        while (rb.velocity.magnitude > moveSpeed)
            yield return new WaitForEndOfFrame();

        canMove = true;
        dashing = false;
    }

    IEnumerator Exit()
    {
        audsrc[3].Play();

        yield return new WaitForSeconds(1f);

        currentLevel++;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void SpawnRandomWep()
    {
        if(PlayerController.weapons.Count < allWeps.Length)
        {
            if(Random.Range(0, 10) == 0)
            {
                Instantiate(randomWep, mainCam.transform.position + Vector3.forward * 10, Quaternion.identity);
            }
        }
    }

    public void RollRandomWep()
    {
        List<WeaponData> missingWeps = new List<WeaponData>();
        foreach(WeaponData weapon in allWeps)
        {
            if (!PlayerController.weapons.Contains(weapon))
            {
                missingWeps.Add(weapon);
            }
        }

        int r = Random.Range(0, missingWeps.Count);
        PlayerController.weapons.Add(missingWeps[r]);
    }
}
