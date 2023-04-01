//*****************************************************************************
// File Name :         ScreenController.cs
// Author :            Kyle Manning
// Creation Date :     April 18, 2021
//
// Brief Description : Assigns each screen a position which is compared to the
//                     player's position. When the player gets 15 units above
//                     or below, the screen tells the game controller it's
//                     despawned and destroys the screen.
//******************************************************************************
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenController : MonoBehaviour
{
    GameObject player;
    GameController gc;
    Vector3 position;

    /// <summary>
    /// Assigns position with a location based on which object the script
    /// is on
    /// </summary>
    void Start()
    {
        player = GameObject.Find("Player");
        gc = FindObjectOfType<GameController>();

        if (gameObject.name.Contains("Screen 1"))
        {
            position = new Vector3(0, 0, 0);
        }
        if (gameObject.name.Contains("Screen 2"))
        {
            position = new Vector3(0, 10, 0);
        }
        if (gameObject.name.Contains("Screen 3"))
        {
            position = new Vector3(0, 20, 0);
        }
        if (gameObject.name.Contains("Screen 4"))
        {
            position = new Vector3(0, 30, 0);
        }
        if (gameObject.name.Contains("Screen 5"))
        {
            position = new Vector3(0, 40, 0);
        }
        if (gameObject.name.Contains("Screen 6"))
        {
            position = new Vector3(0, 50, 0);
        }
        if (gameObject.name.Contains("Screen 7"))
        {
            position = new Vector3(0, 60, 0);
        }
        if (gameObject.name.Contains("Screen 8"))
        {
            position = new Vector3(0, 70, 0);
        }
        if (gameObject.name.Contains("Screen 9"))
        {
            position = new Vector3(0, 80, 0);
        }
        if (gameObject.name.Contains("Screen 10"))
        {
            position = new Vector3(0, 90, 0);
        }
        if (gameObject.name.Contains("Screen 11"))
        {
            position = new Vector3(0, 100, 0);
        }
        if (gameObject.name.Contains("Screen 12"))
        {
            position = new Vector3(0, 110, 0);
        }
        if (gameObject.name.Contains("Screen 13"))
        {
            position = new Vector3(0, 120, 0);
        }
    }

    /// <summary>
    /// If the player's y is more than 15 units above or below the screen's position, it tells
    /// the game controller it is despawned and destroys the screen
    /// </summary>
    void Update()
    {
        if (player.transform.position.y > position.y + 15 || player.transform.position.y < position.y - 15)
        {
            if (gameObject.name.Contains("Screen 1"))
            {
                gc.spawned1 = false;
            }
            if (gameObject.name.Contains("Screen 2"))
            {
                gc.spawned2 = false;
            }
            if (gameObject.name.Contains("Screen 3"))
            {
                gc.spawned3 = false;
            }
            if (gameObject.name.Contains("Screen 4"))
            {
                gc.spawned4 = false;
            }
            if (gameObject.name.Contains("Screen 5"))
            {
                gc.spawned5 = false;
            }
            if (gameObject.name.Contains("Screen 6"))
            {
                gc.spawned6 = false;
            }
            if (gameObject.name.Contains("Screen 7"))
            {
                gc.spawned7 = false;
            }
            if (gameObject.name.Contains("Screen 8"))
            {
                gc.spawned8 = false;
            }
            if (gameObject.name.Contains("Screen 9"))
            {
                gc.spawned9 = false;
            }
            if (gameObject.name.Contains("Screen 10"))
            {
                gc.spawned10 = false;
            }
            if (gameObject.name.Contains("Screen 11"))
            {
                gc.spawned11 = false;
            }
            if (gameObject.name.Contains("Screen 12"))
            {
                gc.spawned12 = false;
            }
            if (gameObject.name.Contains("Screen 13"))
            {
                gc.spawned13 = false;
            }

            Destroy(gameObject);
        }
    }
}
