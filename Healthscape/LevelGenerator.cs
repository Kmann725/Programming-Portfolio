using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public GameObject[] rooms;
    public int roomCount = 8;
    public int endLevel = 1;
    public bool goalSpawned = false;

    // Start is called before the first frame update
    void Start()
    {
        Vector3 start = new Vector3(0, 0, 0);
        roomCount -= 1;
        Instantiate(rooms[Random.Range(0, rooms.Length)], start, transform.rotation);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
