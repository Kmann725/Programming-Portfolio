//*****************************************************************************
// File Name :         TabletBehavior.cs
// Author :            Kyle Manning
// Creation Date :     March 23, 2021
//
// Brief Description : Allows the tablets in game to function as tutorials.
//                     When the player enters the tablet's collider it pops
//                     up a prompt message, which will disappear when the
//                     player exits the collider. When the prompted button is
//                     pressed, the player's movement is stopped and a tutorial
//                     message pops up on screen. When the button is pressed
//                     again, the message will disappear and the player regains
//                     movement.
//******************************************************************************

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabletBehavior : MonoBehaviour
{
    public GameObject buttonPrompt;
    public GameObject popUp;
    public PlayerBehavior pb;
    private bool isInteractable = false;

    /// <summary>
    /// When the player is within interaction range, the player can view the tutorial
    /// message and make it disappear by pressing 'e'. Viewing the message will prevent
    /// the player from moving
    /// </summary>
    private void Update()
    {
        if(isInteractable && Input.GetKeyDown(KeyCode.E))
        {
            if (!popUp.GetComponent<SpriteRenderer>().enabled)
            {
                popUp.GetComponent<SpriteRenderer>().enabled = true;
                pb.canJump = false;
                pb.canMove = false;
            }
            else
            {
                popUp.GetComponent<SpriteRenderer>().enabled = false;
                pb.canJump = true;
                pb.canMove = true;
            }
        }
    }

    /// <summary>
    /// Upon entering the tablet's collider, the button prompt for interaction
    /// is made visible
    /// </summary>
    /// <param name="collision">the collider the object is interacting
    ///                         with</param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            buttonPrompt.GetComponent<SpriteRenderer>().enabled = true;
            isInteractable = true;
        }
    }

    /// <summary>
    /// Upon exiting the tablet's collider, the button prompt for interaction
    /// disappears
    /// </summary>
    /// <param name="collision">the collider the object is interacting
    ///                         with</param>
    private void OnTriggerExit2D(Collider2D collision)
    {
        buttonPrompt.GetComponent<SpriteRenderer>().enabled = false;
        isInteractable = false;
    }
}
