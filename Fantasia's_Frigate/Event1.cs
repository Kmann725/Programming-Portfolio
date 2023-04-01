using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Event1 : MonoBehaviour
{
    public GameObject[] lights;
    private GameObject player;
    private PlayerBehavior pb;
    public GameObject body;
    private AudioSource aud;
    public GameObject lightSource;
    private bool played = false;
    private bool started = false;
    private bool spooked = false;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("FirstPersonCharacter");
        pb = GameObject.Find("FPSController").GetComponent<PlayerBehavior>();
        aud = body.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && Vector3.Distance(transform.position, player.transform.position) < 2)
        {
            RunEvent();
        }

        if (started && !spooked && player.transform.position.x > -9 && player.transform.position.z > -6)
        {
            Vector3 target = new Vector3(-14.23f, 0.98f, -5.2f);
            body.transform.position = Vector3.MoveTowards(body.transform.position, target, 0.2f);
            if (!played)
            {
                aud.Play();
                played = true;
            }
            if (body.transform.position == target)
            {
                spooked = true;
            }
        }
    }

    void RunEvent()
    {
        InvokeRepeating("RunFlicker", 0f, 2f);

        lightSource.GetComponent<BoxCollider>().enabled = false;

        pb.draining = true;

        pb.sanity *= 0.9f;

        Vector3 rot = new Vector3(0, 90, -50);
        Quaternion q = Quaternion.Euler(rot);
        body.transform.rotation = q;
        body.transform.localScale = new Vector3(12.5f, 16f, 1f);
        body.transform.position = new Vector3(2.447f, 0.98f, -3.85f);
        started = true;
    }

    void RunFlicker()
    {
        StartCoroutine("Flickering");
    }

    IEnumerator Flickering()
    {
        for (int i = 0; i < lights.Length; i++)
        {
            lights[i].SetActive(false);
        }
        yield return new WaitForSeconds(0.2f);
        for (int i = 0; i < lights.Length; i++)
        {
            if (i < 5)
            {
                lights[i].GetComponent<Light>().intensity = 2;
            }
            lights[i].SetActive(true);
        }
        yield return new WaitForSeconds(0.5f);
        for (int i = 0; i < lights.Length; i++)
        {
            lights[i].SetActive(false);
        }
        yield return new WaitForSeconds(0.3f);
        for (int i = 0; i < lights.Length; i++)
        {
            if (i < 5)
            {
                lights[i].GetComponent<Light>().intensity = 3;
            }
            lights[i].SetActive(true);
        }
    }
}
