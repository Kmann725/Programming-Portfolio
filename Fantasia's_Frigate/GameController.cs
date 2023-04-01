using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{

    [SerializeField] private GameObject player;
    [SerializeField] private GameObject[] boxes;
    [SerializeField] private GameObject crack;
    [SerializeField] private GameObject blood;
    [SerializeField] private GameObject bloodMist;
    [SerializeField] private GameObject hands;
    public string comboInput = "";
    public string sinkInput = "";
    private PlayerBehavior pB;
    private AudioSource[] breathe;
    private bool threshold1 = false;
    private bool threshold2 = false;
    private bool threshold3 = false;

    // Start is called before the first frame update
    void Start()
    {
        pB = player.GetComponent<PlayerBehavior>();
        breathe = GetComponents<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(pB.sanity < 100 && pB.sanity > 75)
        {
            for(int i = 0; i < 3; i++)
            {
                boxes[i].SetActive(true);
            }
        }
        else if (pB.sanity < 75 && pB.sanity > 50)
        {
            for (int i = 3; i < 7; i++)
            {
                boxes[i].SetActive(true);
            }
            hands.SetActive(true);
            crack.SetActive(true);
        }
        else if (pB.sanity < 50 && pB.sanity > 25)
        {
            for (int i = 7; i < 10; i++)
            {
                boxes[i].SetActive(true);
            }
            blood.SetActive(true);
        }
        else if (pB.sanity < 25)
        {
            for (int i = 10; i < 14; i++)
            {
                boxes[i].SetActive(true);
            }
            bloodMist.SetActive(true);
        }

        if (pB.sanity < 75 && !threshold1)
        {
            breathe[1].Play();
            threshold1 = true;
        }
        else if (pB.sanity < 50 && !threshold2)
        {
            breathe[1].Play();
            threshold2 = true;
        }
        else if (pB.sanity < 25 && !threshold3)
        {
            breathe[1].Play();
            threshold3 = true;
        }
        else if (pB.sanity > 75 && threshold1)
        {
            threshold1 = false;
            hands.SetActive(false);
            crack.SetActive(false);
        }
        else if (pB.sanity > 50 && threshold2)
        {
            threshold2 = false;
            blood.SetActive(false);
        }
        else if (pB.sanity > 25 && threshold3)
        {
            threshold3 = false;
            bloodMist.SetActive(false);
        }
    }
}