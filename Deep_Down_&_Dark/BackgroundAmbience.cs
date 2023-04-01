//*****************************************************************************
// File Name :         BackgroundAmbience.cs
// Author :            Kyle Manning
// Creation Date :     April 18, 2021
//
// Brief Description : Raises and lowers the volume of the two ambient sounds
//                     playing in the background. As the player travels up,
//                     the wind rises in volume while the ambience lowers in
//                     volume, and vice versa.
//******************************************************************************
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundAmbience : MonoBehaviour
{
    public GameObject player;
    AudioSource ambience;
    AudioSource wind;

    /// <summary>
    /// Creates an array of audio sources and assigns each to an audio source
    /// variable for use elsewhere
    /// </summary>
    void Start()
    {
        AudioSource[] sounds = GetComponents<AudioSource>();
        ambience = sounds[0];
        wind = sounds[1];
    }

    /// <summary>
    /// Calls AmbienceChanger() every frame to update sound volumes when conditions
    /// are met
    /// </summary>
    void Update()
    {
        AmbienceChanger();
    }

    /// <summary>
    /// Observes the y-value of the player's current position and changes the volume
    /// of the two audio sources as needed
    /// </summary>
    void AmbienceChanger()
    {
        if (player.transform.position.y <= 5 )
        {
            ambience.volume = 1f;
            wind.volume = 0f;
        }
        else if (player.transform.position.y > 5 && player.transform.position.y <= 15)
        {
            ambience.volume = 0.925f;
            wind.volume = 0.025f;
        }
        else if (player.transform.position.y > 15 && player.transform.position.y <= 25)
        {
            ambience.volume = 0.85f;
            wind.volume = 0.05f;
        }
        else if (player.transform.position.y > 25 && player.transform.position.y <= 35)
        {
            ambience.volume = 0.775f;
            wind.volume = 0.075f;
        }
        else if (player.transform.position.y > 35 && player.transform.position.y <= 45)
        {
            ambience.volume = 0.7f;
            wind.volume = 0.01f;
        }
        else if (player.transform.position.y > 45 && player.transform.position.y <= 55)
        {
            ambience.volume = 0.625f;
            wind.volume = 0.125f;
        }
        else if (player.transform.position.y > 55 && player.transform.position.y <= 65)
        {
            ambience.volume = 0.55f;
            wind.volume = 0.15f;
        }
        else if (player.transform.position.y > 65 && player.transform.position.y <= 75)
        {
            ambience.volume = 0.475f;
            wind.volume = 0.175f;
        }
        else if (player.transform.position.y > 75 && player.transform.position.y <= 85)
        {
            ambience.volume = 0.4f;
            wind.volume = 0.2f;
        }
        else if (player.transform.position.y > 85 && player.transform.position.y <= 95)
        {
            ambience.volume = 0.325f;
            wind.volume = 0.225f;
        }
        else if (player.transform.position.y > 95 && player.transform.position.y <= 105)
        {
            ambience.volume = 0.25f;
            wind.volume = 0.25f;
        }
        else if (player.transform.position.y > 105 && player.transform.position.y <= 115)
        {
            ambience.volume = 0.175f;
            wind.volume = 0.275f;
        }
        else if (player.transform.position.y > 115)
        {
            ambience.volume = 0.1f;
            wind.volume = 0.3f;
        }
    }
}
