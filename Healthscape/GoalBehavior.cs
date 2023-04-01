using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoalBehavior : MonoBehaviour
{
    private LevelGenerator lg;

    public static int currentLevel = 1;

    private void Start()
    {
        lg = GameObject.FindGameObjectWithTag("LevelGenerator").GetComponent<LevelGenerator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (lg.endLevel != currentLevel)
            {
                currentLevel++;
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
            else
            {
                Debug.Log("You win");
            }
        }
    }
}
