//*****************************************************************************
// File Name :         CameraAdjuster.cs
// Author :            Kyle Manning
// Creation Date :     April 12, 2021
//
// Brief Description : Allows the game to properly adjust for other aspect
//                     ratios. It gets the camera's aspect ratio and adjusts
//                     the camera's orthographic size, turns on letterboxing,
//                     and changes ui size as necessary.
//******************************************************************************
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class CameraAdjuster : MonoBehaviour
{
    public GameObject letterBoxing;
    public Text counter;
    public Text notifier;
    public Image picture;

    /// <summary>
    /// Gets the main camera's aspect ratio and, if it's lower than 1.7f, activates
    /// letterboxing, changes the orthographic size to fit the game properly in the
    /// screen, and changes the positioning of UI elements as needed
    /// </summary>
    void Start()
    {
        if (Camera.main.aspect < 1.7f)
        {
            Vector3 counterPos;
            Vector3 notifierPos;
            Vector3 picturePos;

            letterBoxing.SetActive(true);

            if (Camera.main.aspect > 1.55f)
            {
                Camera.main.orthographicSize = 5.5f;

                counterPos = new Vector3(-39.59f, -127, 0);
                notifierPos = new Vector3(-48.8f, -185, 0);
                picturePos = new Vector3(-118.9f, -106, 0);

                counter.rectTransform.anchoredPosition = counterPos;
                notifier.rectTransform.anchoredPosition = notifierPos;
                picture.rectTransform.anchoredPosition = picturePos;
            }
            else if (Camera.main.aspect > 1.3f)
            {
                Camera.main.orthographicSize = 6.65f;

                counterPos = new Vector3(-39.59f, -260, 0);
                notifierPos = new Vector3(-48.8f, -318, 0);
                picturePos = new Vector3(-118.9f, -238, 0);

                counter.rectTransform.anchoredPosition = counterPos;
                notifier.rectTransform.anchoredPosition = notifierPos;
                picture.rectTransform.anchoredPosition = picturePos;
            }
            else if (Camera.main.aspect > 1.2f)
            {
                Camera.main.orthographicSize = 7.1f;

                counterPos = new Vector3(-39.59f, -298, 0);
                notifierPos = new Vector3(-48.8f, -355, 0);
                picturePos = new Vector3(-118.9f, -278, 0);

                counter.rectTransform.anchoredPosition = counterPos;
                notifier.rectTransform.anchoredPosition = notifierPos;
                picture.rectTransform.anchoredPosition = picturePos;
            }
        }
    }
}
