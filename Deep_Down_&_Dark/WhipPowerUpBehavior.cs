//*****************************************************************************
// File Name :         WhipPowerUpBehavior.cs
// Author :            Kyle Manning
// Creation Date :     March 26, 2021
//
// Brief Description : When the player collides with the whip power up, it
//                     increases the player's whip count and shows the increase
//                     in the UI and then disappears for 5 seconds before
//                     reappearing.
//******************************************************************************
using System.Collections;
using UnityEngine.UI;
using UnityEngine;

public class WhipPowerUpBehavior : MonoBehaviour
{
    public Text increaseNotifier;
    public PlayerBehavior playerBeh;
    private SpriteRenderer sr;
    private CircleCollider2D cc2d;
    AudioSource clap;

    /// <summary>
    /// Assigns components to variables for use
    /// </summary>
    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        cc2d = GetComponent<CircleCollider2D>();
        clap = GetComponent<AudioSource>();
    }

    /// <summary>
    /// When the player goes back to a checkpoint, the power up becomes active
    /// again
    /// </summary>
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.C) || playerBeh.hitByEnemy)
        {
            sr.enabled = true;
            cc2d.enabled = true;
        }
    }

    /// <summary>
    /// When the player collides with the power up, it adds a use to the whip count,
    /// plays a sound, and runs the reappear coroutine
    /// </summary>
    /// <param name="collision">the collider the object is interacting
    ///                         with</param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Player")
        {
            playerBeh.whipCount++;
            clap.Play();
            StartCoroutine("IncreaseNotification");
            StartCoroutine("Reappear");
        }
    }

    /// <summary>
    /// Makes the power up disappear, and then has it reappear after a set
    /// time
    /// </summary>
    /// <returns>the amount of time before the power up reappears</returns>
    IEnumerator Reappear()
    {
        sr.enabled = false;
        cc2d.enabled = false;

        yield return new WaitForSeconds(5);

        sr.enabled = true;
        cc2d.enabled = true;
    }

    /// <summary>
    /// Displays a UI notifying an increase in whip count, and then
    /// turns the UI element off after a set time
    /// </summary>
    /// <returns>the amount of time before the UI element disappears</returns>
    IEnumerator IncreaseNotification()
    {
        increaseNotifier.text = "+1";
        increaseNotifier.enabled = true;
        yield return new WaitForSeconds(1.5f);
        increaseNotifier.enabled = false;
    }
}
