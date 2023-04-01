using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactables : MonoBehaviour
{
    public GameObject display;
    public GameObject highlight;
    private bool inUse = false;
    private bool used = false;
    private string selObj;
    private AudioSource[] aud;
    private GameObject player;
    private PlayerBehavior pb;

    // Start is called before the first frame update
    void Start()
    {
        aud = GetComponents<AudioSource>();
        player = GameObject.Find("FirstPersonCharacter");
        pb = GameObject.Find("FPSController").GetComponent<PlayerBehavior>();
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));

        if (Vector3.Distance(player.transform.position, gameObject.transform.position) <= 2f)
        {
            if (Physics.Raycast(ray, out hit) && hit.transform.gameObject == gameObject)
            {
                highlight.SetActive(true);
            }
            else
            {
                highlight.SetActive(false);
            }

            if (Input.GetMouseButtonDown(0) && !inUse)
            {
                if (Physics.Raycast(ray, out hit))
                {
                    print("test1");
                    selObj = hit.transform.tag;
                    if (hit.transform.name == "Keypad2" && hit.transform.gameObject == gameObject)
                    {
                        display.SetActive(true);
                        inUse = true;
                        Cursor.visible = true;
                        Time.timeScale = 0.0f;
                    }
                    else if (hit.transform.tag == "Paper" && hit.transform.gameObject == gameObject)
                    {
                        aud[0].Play();
                        display.SetActive(true);
                        inUse = true;
                        if (!used)
                        {
                            if (pb.sanity + 10 > 100)
                            {
                                pb.sanity = 100;
                            }
                            else
                            {
                                pb.sanity += 10;
                            }
                            used = true;
                        }
                        Time.timeScale = 0.0f;
                    }
                    else if (hit.transform.tag == "Door" && hit.transform.gameObject == gameObject)
                    {
                        aud[0].Play();
                    }
                }
            }
            else if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (selObj.Equals("Paper") && inUse)
                {
                    aud[0].Play();
                }
                display.SetActive(false);
                inUse = false;
                Cursor.visible = false;
                Time.timeScale = 1.0f;
            }
        }
        else
        {
            highlight.SetActive(false);
        }
    }
}