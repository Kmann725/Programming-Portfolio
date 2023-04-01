using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerBehavior : MonoBehaviour
{
    public float sanity = 100f;
    public Slider sanMeter;
    private AudioSource[] wNoise;
    private bool wNPlaying = false;
    private bool correct;
    public bool draining = true;
    private bool pulsing = false;
    public GameObject door;
    public GameObject keyPad;
    public GameObject meterPulse;

    // Start is called before the first frame update
    void Start()
    {
        wNoise = GetComponents<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (draining && !pulsing)
        {
            StartCoroutine("Pulsing");
            pulsing = true;
        }

        if (draining)
        {
            sanity -= 1f * Time.deltaTime;
            sanMeter.value = sanity;
        }

        if(keyPad.activeSelf)
        {
            Time.timeScale = 0.0f;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Time.timeScale = 1.0f;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        if (SceneManager.GetActiveScene().buildIndex == 3)
        {
            print("correct scene");
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                print("quit");
                Application.Quit();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Lantern")
        {
            draining = false;
        }

        if(other.gameObject.tag == "Skeleton")
        {
            SceneManager.LoadScene(4);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == "Lantern")
        {
            draining = true;
        }
    }

    IEnumerator Pulsing()
    {
        Color tempCol = meterPulse.GetComponent<Image>().color;
        wNoise[1].Play();

        while (draining)
        {
            for (float i = 0; i <= 1; i += 0.1f)
            {
                tempCol.a = i;
                meterPulse.GetComponent<Image>().color = tempCol;
                yield return new WaitForSeconds(0.1f);
            }
            for (float i = 1; i >= 0; i -= 0.1f)
            {
                tempCol.a = i;
                meterPulse.GetComponent<Image>().color = tempCol;
                yield return new WaitForSeconds(0.1f);
            }
        }

        pulsing = false;
        wNoise[1].Stop();
    }
}