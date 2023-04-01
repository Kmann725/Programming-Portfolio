//*****************************************************************************
// File Name :         GameController.cs
// Authors :           Adam Field, Kyle Manning
// Creation Date :     March 1, 2021
//
// Brief Description : Controls many aspects of the game. It changes the camera
//                     position based on the player's position and spawns the 
//                     game screens as the playr moves through the world. It
//                     also handles displaying the game's UI and the pause and
//                     victory menus.
//******************************************************************************
using UnityEngine.UI;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public GameObject player;
    public GameObject mainCamera;

    public Text whipCounter;
    public PlayerBehavior pb;

    float yPosOfSecondScreen = 5f;
    float yPosOfThirdScreen = 15f;
    float yPosOfFourthScreen = 25f;
    float yPosOfFifthScreen = 35f;
    float yPosOfSixthScreen = 45f;
    float yPosOfSeventhScreen = 55f;
    float yPosOfEighthScreen = 65f;
    float yPosOfNineScreen = 75f;
    float yPosOfTenthScreen = 85f;
    float yPosOfEleventhScreen = 95f;
    float yPosOfTwelveScreen = 105f;
    float yPosOfThirteenthScreen = 115f;
    float yPosOfFourteenthScreen = 125f;

    public bool isPaused = false;

    public GameObject pauseMenu;
    public GameObject victoryMenu;

    public GameObject screen1;
    public GameObject screen2;
    public GameObject screen3;
    public GameObject screen4;
    public GameObject screen5;
    public GameObject screen6;
    public GameObject screen7;
    public GameObject screen8;
    public GameObject screen9;
    public GameObject screen10;
    public GameObject screen11;
    public GameObject screen12;
    public GameObject screen13;

    [HideInInspector] 
    public bool spawned1 = false;
    [HideInInspector]
    public bool spawned2 = false;
    [HideInInspector]
    public bool spawned3 = false;
    [HideInInspector]
    public bool spawned4 = false;
    [HideInInspector]
    public bool spawned5 = false;
    [HideInInspector]
    public bool spawned6 = false;
    [HideInInspector]
    public bool spawned7 = false;
    [HideInInspector]
    public bool spawned8 = false;
    [HideInInspector]
    public bool spawned9 = false;
    [HideInInspector]
    public bool spawned10 = false;
    [HideInInspector]
    public bool spawned11 = false;
    [HideInInspector]
    public bool spawned12 = false;
    [HideInInspector]
    public bool spawned13 = false;


    /// <summary>
    /// Assigns the starting display for the whip counter UI
    /// </summary>
    void Start()
    {
        whipCounter.text = "" + pb.whipCount;
    }

    /// <summary>
    /// Handles behavior for pausing the game, updating the whip counter UI,
    /// and reaching the victory condition for the game
    /// </summary>
    void Update()
    {
        ChangeCameraPos();
        ScreenManager();

        if (Input.GetKeyDown(KeyCode.Escape) && isPaused == false)
        {
            Time.timeScale = 0;
            isPaused = true;
            Cursor.visible = true;
            pauseMenu.SetActive(isPaused);
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && isPaused == true)
        {
            Time.timeScale = 1;
            isPaused = false;
            Cursor.visible = false;
            pauseMenu.SetActive(isPaused);
        }

        whipCounter.text = "" + pb.whipCount;

        if (player.transform.position.x > 4.5f && player.transform.position.y > 115f)
        {
            victoryMenu.SetActive(true);
            Cursor.visible = true;
            Time.timeScale = 0;
            pb.canMove = false;
            pb.canJump = false;
            pb.animator.SetFloat("Speed", 0);
        }
    }

    /// <summary>
    /// Depending on what the player's y-value is, the camera is moved to its appropriate location
    /// </summary>
    private void ChangeCameraPos()
    {
        if (player.transform.position.y < yPosOfSecondScreen)
        {
            CameraPos(0);
        }
        else if (player.transform.position.y > yPosOfSecondScreen && player.transform.position.y < yPosOfThirdScreen)
        {
            CameraPos(10);
        }
        else if (player.transform.position.y > yPosOfThirdScreen && player.transform.position.y < yPosOfFourthScreen)
        {
            CameraPos(20);
        }
        else if (player.transform.position.y > yPosOfFourthScreen && player.transform.position.y < yPosOfFifthScreen)
        {
            CameraPos(30);
        }
        else if (player.transform.position.y > yPosOfFifthScreen && player.transform.position.y < yPosOfSixthScreen)
        {
            CameraPos(40);
        }
        else if (player.transform.position.y > yPosOfSixthScreen && player.transform.position.y < yPosOfSeventhScreen)
        {
            CameraPos(50);
        }
        else if (player.transform.position.y > yPosOfSeventhScreen && player.transform.position.y < yPosOfEighthScreen)
        {
            CameraPos(60);
        }
        else if (player.transform.position.y > yPosOfEighthScreen && player.transform.position.y < yPosOfNineScreen)
        {
            CameraPos(70);
        }
        else if (player.transform.position.y > yPosOfNineScreen && player.transform.position.y < yPosOfTenthScreen)
        {
            CameraPos(80);
        }
        else if (player.transform.position.y > yPosOfTenthScreen && player.transform.position.y < yPosOfEleventhScreen)
        {
            CameraPos(90);
        }
        else if (player.transform.position.y > yPosOfEleventhScreen && player.transform.position.y < yPosOfTwelveScreen)
        {
            CameraPos(100);
        }
        else if (player.transform.position.y > yPosOfTwelveScreen && player.transform.position.y < yPosOfThirteenthScreen)
        {
            CameraPos(110);
        }
        else if (player.transform.position.y > yPosOfThirteenthScreen)
        {
            CameraPos(120);
        }
        else if (player.transform.position.y > yPosOfFourteenthScreen)
        {
            CameraPos(130);
        }
    }

    void CameraPos(float newPos)
    {
        mainCamera.transform.position = new Vector3(0, newPos, -10);
    }

    /// <summary>
    /// Depending on where the player is, the screen the player is on as well as
    /// above and below are spawned in
    /// </summary>
    void ScreenManager()
    {
        Vector3 screenLocation;

        if (player.transform.position.y < 15 && !spawned1)
        {
            screenLocation = new Vector3(0.8781567f, -0.4048893f, 0);
            Instantiate(screen1, screenLocation, transform.rotation);
            spawned1 = true;
        }
        if (player.transform.position.y < 25 && !spawned2)
        {
            screenLocation = new Vector3(-0.09263059f, -0.1873588f, 0);
            Instantiate(screen2, screenLocation, transform.rotation);
            spawned2 = true;
        }
        if ((player.transform.position.y > 5 && player.transform.position.y < 35) && !spawned3)
        {
            screenLocation = new Vector3(-0.09263059f, -0.1873588f, 0);
            Instantiate(screen3, screenLocation, transform.rotation);
            spawned3 = true;
        }
        if ((player.transform.position.y > 15 && player.transform.position.y < 45) && !spawned4)
        {
            screenLocation = new Vector3(-0.09263059f, -0.1873588f, 0);
            Instantiate(screen4, screenLocation, transform.rotation);
            spawned4 = true;
        }
        if ((player.transform.position.y > 25 && player.transform.position.y < 55) && !spawned5)
        {
            screenLocation = new Vector3(-0.09263059f, -0.1873588f, 0);
            Instantiate(screen5, screenLocation, transform.rotation);
            spawned5 = true;
        }
        if ((player.transform.position.y > 35 && player.transform.position.y < 65) && !spawned6)
        {
            screenLocation = new Vector3(-0.09263059f, -0.1873588f, 0);
            Instantiate(screen6, screenLocation, transform.rotation);
            spawned6 = true;
        }
        if ((player.transform.position.y > 45 && player.transform.position.y < 75) && !spawned7)
        {
            screenLocation = new Vector3(-0.09263059f, -0.1873588f, 0);
            Instantiate(screen7, screenLocation, transform.rotation);
            spawned7 = true;
        }
        if ((player.transform.position.y > 55 && player.transform.position.y < 85) && !spawned8)
        {
            screenLocation = new Vector3(-0.09263059f, -0.1873588f, 0);
            Instantiate(screen8, screenLocation, transform.rotation);
            spawned8 = true;
        }
        if ((player.transform.position.y > 65 && player.transform.position.y < 95) && !spawned9)
        {
            screenLocation = new Vector3(-0.09263059f, -0.1873588f, 0);
            Instantiate(screen9, screenLocation, transform.rotation);
            spawned9 = true;
        }
        if ((player.transform.position.y > 75 && player.transform.position.y < 105) && !spawned10)
        {
            screenLocation = new Vector3(-0.09263059f, -0.1873588f, 0);
            Instantiate(screen10, screenLocation, transform.rotation);
            spawned10 = true;
        }
        if ((player.transform.position.y > 85 && player.transform.position.y < 115) && !spawned11)
        {
            screenLocation = new Vector3(-0.09263059f, -0.1873588f, 0);
            Instantiate(screen11, screenLocation, transform.rotation);
            spawned11 = true;
        }
        if (player.transform.position.y > 95 && !spawned12)
        {
            screenLocation = new Vector3(-0.09263059f, -0.1873588f, 0);
            Instantiate(screen12, screenLocation, transform.rotation);
            spawned12 = true;
        }
        if (player.transform.position.y > 105 && !spawned13)
        {
            screenLocation = new Vector3(-0.09263059f, -0.1873588f, 0);
            Instantiate(screen13, screenLocation, transform.rotation);
            spawned13 = true;
        }
    }
}