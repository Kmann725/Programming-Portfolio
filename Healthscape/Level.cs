using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level : MonoBehaviour
{
    private PlayerMovement pm;

    public static string PreviousLevel { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        pm = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();

        if (PreviousLevel == "Main Menu")
        {
            pm.Reset();
        }
    }

    private void OnDestroy()
    {
        PreviousLevel = SceneManager.GetActiveScene().name;
    }
}
