using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnNeighboringRooms : MonoBehaviour
{
    public bool hasUp;
    public bool hasLeft;
    public bool hasRight;
    public bool hasDown;
    public int doorCount = -1;

    [Header("Doors")]
    public GameObject upDoor;
    public GameObject leftDoor;
    public GameObject rightDoor;
    public GameObject downDoor;

    [Header("Exit")]
    public GameObject goal;

    [Header("Room Lists")]
    public GameObject[] doors4;
    public GameObject[] doors3r;
    public GameObject[] doors3l;
    public GameObject[] doors3u;
    public GameObject[] doors3d;
    public GameObject[] doors2ud;
    public GameObject[] doors2lr;
    public GameObject[] doors2ur;
    public GameObject[] doors2ul;
    public GameObject[] doors2dr;
    public GameObject[] doors2dl;
    public GameObject[] doors1u;
    public GameObject[] doors1r;
    public GameObject[] doors1l;
    public GameObject[] doors1d;

    public Vector3 previousRoom;

    private LevelGenerator lg;

    // Start is called before the first frame update
    void Start()
    {
        lg = GameObject.FindGameObjectWithTag("LevelGenerator").GetComponent<LevelGenerator>();
        lg.roomCount--;

        doors4 = Resources.LoadAll<GameObject>("4 Doors");
        doors3r = Resources.LoadAll<GameObject>("3 Doors r");
        doors3l = Resources.LoadAll<GameObject>("3 Doors l");
        doors3u = Resources.LoadAll<GameObject>("3 Doors u");
        doors3d = Resources.LoadAll<GameObject>("3 Doors d");
        doors2lr = Resources.LoadAll<GameObject>("2 Doors lr");
        doors2ur = Resources.LoadAll<GameObject>("2 Doors ur");
        doors2ud = Resources.LoadAll<GameObject>("2 Doors ud");
        doors2ul = Resources.LoadAll<GameObject>("2 Doors ul");
        doors2dr = Resources.LoadAll<GameObject>("2 Doors dr");
        doors2dl = Resources.LoadAll<GameObject>("2 Doors dl");
        doors1u = Resources.LoadAll<GameObject>("1 Door u");
        doors1r = Resources.LoadAll<GameObject>("1 Door r");
        doors1l = Resources.LoadAll<GameObject>("1 Door l");
        doors1d = Resources.LoadAll<GameObject>("1 Door d");

        if (!lg.goalSpawned && doorCount == 1 && lg.roomCount < 0)
        {
            Instantiate(goal, transform.position, transform.rotation);
            lg.goalSpawned = true;
        }

        if (lg.roomCount > 0)
        {
            if (hasUp)
            {
                SpawnUpRoom(Random.Range(1, 8));
            }
            if (hasLeft)
            {
                SpawnLeftRoom(Random.Range(1, 8));
            }
            if (hasRight)
            {
                SpawnRightRoom(Random.Range(1, 8));
            }
            if (hasDown)
            {
                SpawnDownRoom(Random.Range(1, 8));
            }
        }
        else
        {
            if (hasUp)
            {
                SpawnUpRoom(0);
            }
            if (hasLeft)
            {
                SpawnLeftRoom(0);
            }
            if (hasRight)
            {
                SpawnRightRoom(0);
            }
            if (hasDown)
            {
                SpawnDownRoom(0);
            }
        }
    }

    void SpawnUpRoom(int type)
    {
        switch (type)
        {
            case 0:
                Vector3 newRoom = transform.position;
                newRoom.y += 10;

                Collider2D room = Physics2D.OverlapBox(newRoom, new Vector2(1, 1), 0);

                if (room != null && Vector3.Distance(room.transform.position, previousRoom) > 1)
                {
                    upDoor.SetActive(true);
                }

                if (newRoom != previousRoom && room == null)
                {
                    GameObject nRoom = Instantiate(doors1d[Random.Range(0, doors1d.Length)], newRoom, transform.rotation);
                    nRoom.GetComponent<SpawnNeighboringRooms>().previousRoom = transform.position;
                }
                break;
            case 1:
                newRoom = transform.position;
                newRoom.y += 10;

                room = Physics2D.OverlapBox(newRoom, new Vector2(1, 1), 0);

                if (room != null && Vector3.Distance(room.transform.position, previousRoom) > 1)
                {
                    upDoor.SetActive(true);
                }

                if (newRoom != previousRoom && room == null)
                {
                    GameObject nRoom = Instantiate(doors2ud[Random.Range(0, doors2ud.Length)], newRoom, transform.rotation);
                    nRoom.GetComponent<SpawnNeighboringRooms>().previousRoom = transform.position;
                }
                break;
            case 2:
                newRoom = transform.position;
                newRoom.y += 10;

                room = Physics2D.OverlapBox(newRoom, new Vector2(1, 1), 0);

                if (room != null && Vector3.Distance(room.transform.position, previousRoom) > 1)
                {
                    upDoor.SetActive(true);
                }

                if (newRoom != previousRoom && room == null)
                {
                    GameObject nRoom = Instantiate(doors2dr[Random.Range(0, doors2dr.Length)], newRoom, transform.rotation);
                    nRoom.GetComponent<SpawnNeighboringRooms>().previousRoom = transform.position;
                }
                break;
            case 3:
                newRoom = transform.position;
                newRoom.y += 10;

                room = Physics2D.OverlapBox(newRoom, new Vector2(1, 1), 0);

                if (room != null && Vector3.Distance(room.transform.position, previousRoom) > 1)
                {
                    upDoor.SetActive(true);
                }

                if (newRoom != previousRoom && room == null)
                {
                    GameObject nRoom = Instantiate(doors2dl[Random.Range(0, doors2dl.Length)], newRoom, transform.rotation);
                    nRoom.GetComponent<SpawnNeighboringRooms>().previousRoom = transform.position;
                }
                break;
            case 4:
                newRoom = transform.position;
                newRoom.y += 10;

                room = Physics2D.OverlapBox(newRoom, new Vector2(1, 1), 0);

                if (room != null && Vector3.Distance(room.transform.position, previousRoom) > 1)
                {
                    upDoor.SetActive(true);
                }

                if (newRoom != previousRoom && room == null)
                {
                    GameObject nRoom = Instantiate(doors3l[Random.Range(0, doors3l.Length)], newRoom, transform.rotation);
                    nRoom.GetComponent<SpawnNeighboringRooms>().previousRoom = transform.position;
                }
                break;
            case 5:
                newRoom = transform.position;
                newRoom.y += 10;

                room = Physics2D.OverlapBox(newRoom, new Vector2(1, 1), 0);

                if (room != null && Vector3.Distance(room.transform.position, previousRoom) > 1)
                {
                    upDoor.SetActive(true);
                }

                if (newRoom != previousRoom && room == null)
                {
                    GameObject nRoom = Instantiate(doors3r[Random.Range(0, doors3r.Length)], newRoom, transform.rotation);
                    nRoom.GetComponent<SpawnNeighboringRooms>().previousRoom = transform.position;
                }
                break;
            case 6:
                newRoom = transform.position;
                newRoom.y += 10;

                room = Physics2D.OverlapBox(newRoom, new Vector2(1, 1), 0);

                if (room != null && Vector3.Distance(room.transform.position, previousRoom) > 1)
                {
                    upDoor.SetActive(true);
                }

                if (newRoom != previousRoom && room == null)
                {
                    GameObject nRoom = Instantiate(doors3u[Random.Range(0, doors3u.Length)], newRoom, transform.rotation);
                    nRoom.GetComponent<SpawnNeighboringRooms>().previousRoom = transform.position;
                }
                break;
            case 7:
                newRoom = transform.position;
                newRoom.y += 10;

                room = Physics2D.OverlapBox(newRoom, new Vector2(1, 1), 0);

                if (room != null && Vector3.Distance(room.transform.position, previousRoom) > 1)
                {
                    upDoor.SetActive(true);
                }

                if (newRoom != previousRoom && room == null)
                {
                    GameObject nRoom = Instantiate(doors4[Random.Range(0, doors4.Length)], newRoom, transform.rotation);
                    nRoom.GetComponent<SpawnNeighboringRooms>().previousRoom = transform.position;
                }
                break;
        }
    }
    
    void SpawnDownRoom(int type)
    {
        switch (type)
        {
            case 0:
                Vector3 newRoom = transform.position;
                newRoom.y -= 10;

                Collider2D room = Physics2D.OverlapBox(newRoom, new Vector2(1, 1), 0);

                if (room != null && Vector3.Distance(room.transform.position, previousRoom) > 1)
                {
                    downDoor.SetActive(true);
                }

                if (newRoom != previousRoom && room == null)
                {
                    GameObject nRoom = Instantiate(doors1u[Random.Range(0, doors1u.Length)], newRoom, transform.rotation);
                    nRoom.GetComponent<SpawnNeighboringRooms>().previousRoom = transform.position;
                }
                break;
            case 1:
                newRoom = transform.position;
                newRoom.y -= 10;

                room = Physics2D.OverlapBox(newRoom, new Vector2(1, 1), 0);

                if (room != null && Vector3.Distance(room.transform.position, previousRoom) > 1)
                {
                    downDoor.SetActive(true);
                }

                if (newRoom != previousRoom && room == null)
                {
                    GameObject nRoom = Instantiate(doors2ud[Random.Range(0, doors2ud.Length)], newRoom, transform.rotation);
                    nRoom.GetComponent<SpawnNeighboringRooms>().previousRoom = transform.position;
                }
                break;
            case 2:
                newRoom = transform.position;
                newRoom.y -= 10;

                room = Physics2D.OverlapBox(newRoom, new Vector2(1, 1), 0);

                if (room != null && Vector3.Distance(room.transform.position, previousRoom) > 1)
                {
                    downDoor.SetActive(true);
                }

                if (newRoom != previousRoom && room == null)
                {
                    GameObject nRoom = Instantiate(doors2ul[Random.Range(0, doors2ul.Length)], newRoom, transform.rotation);
                    nRoom.GetComponent<SpawnNeighboringRooms>().previousRoom = transform.position;
                }
                break;
            case 3:
                newRoom = transform.position;
                newRoom.y -= 10;

                room = Physics2D.OverlapBox(newRoom, new Vector2(1, 1), 0);

                if (room != null && Vector3.Distance(room.transform.position, previousRoom) > 1)
                {
                    downDoor.SetActive(true);
                }

                if (newRoom != previousRoom && room == null)
                {
                    GameObject nRoom = Instantiate(doors2ur[Random.Range(0, doors2ur.Length)], newRoom, transform.rotation);
                    nRoom.GetComponent<SpawnNeighboringRooms>().previousRoom = transform.position;
                }
                break;
            case 4:
                newRoom = transform.position;
                newRoom.y -= 10;

                room = Physics2D.OverlapBox(newRoom, new Vector2(1, 1), 0);

                if (room != null && Vector3.Distance(room.transform.position, previousRoom) > 1)
                {
                    downDoor.SetActive(true);
                }

                if (newRoom != previousRoom && room == null)
                {
                    GameObject nRoom = Instantiate(doors3d[Random.Range(0, doors3d.Length)], newRoom, transform.rotation);
                    nRoom.GetComponent<SpawnNeighboringRooms>().previousRoom = transform.position;
                }
                break;
            case 5:
                newRoom = transform.position;
                newRoom.y -= 10;

                room = Physics2D.OverlapBox(newRoom, new Vector2(1, 1), 0);

                if (room != null && Vector3.Distance(room.transform.position, previousRoom) > 1)
                {
                    downDoor.SetActive(true);
                }

                if (newRoom != previousRoom && room == null)
                {
                    GameObject nRoom = Instantiate(doors3l[Random.Range(0, doors3l.Length)], newRoom, transform.rotation);
                    nRoom.GetComponent<SpawnNeighboringRooms>().previousRoom = transform.position;
                }
                break;
            case 6:
                newRoom = transform.position;
                newRoom.y -= 10;

                room = Physics2D.OverlapBox(newRoom, new Vector2(1, 1), 0);

                if (room != null && Vector3.Distance(room.transform.position, previousRoom) > 1)
                {
                    downDoor.SetActive(true);
                }

                if (newRoom != previousRoom && room == null)
                {
                    GameObject nRoom = Instantiate(doors3r[Random.Range(0, doors3r.Length)], newRoom, transform.rotation);
                    nRoom.GetComponent<SpawnNeighboringRooms>().previousRoom = transform.position;
                }
                break;
            case 7:
                newRoom = transform.position;
                newRoom.y -= 10;

                room = Physics2D.OverlapBox(newRoom, new Vector2(1, 1), 0);

                if (room != null && Vector3.Distance(room.transform.position, previousRoom) > 1)
                {
                    downDoor.SetActive(true);
                }

                if (newRoom != previousRoom && room == null)
                {
                    GameObject nRoom = Instantiate(doors4[Random.Range(0, doors4.Length)], newRoom, transform.rotation);
                    nRoom.GetComponent<SpawnNeighboringRooms>().previousRoom = transform.position;
                }
                break;
        }
    }

    void SpawnLeftRoom(int type)
    {
        switch (type)
        {
            case 0:
                Vector3 newRoom = transform.position;
                newRoom.x -= 18;

                Collider2D room = Physics2D.OverlapBox(newRoom, new Vector2(1, 1), 0);

                if (room != null && Vector3.Distance(room.transform.position, previousRoom) > 1)
                {
                    leftDoor.SetActive(true);
                }

                if (newRoom != previousRoom && room == null)
                {
                    GameObject nRoom = Instantiate(doors1r[Random.Range(0, doors1r.Length)], newRoom, transform.rotation);
                    nRoom.GetComponent<SpawnNeighboringRooms>().previousRoom = transform.position;
                }
                break;
            case 1:
                newRoom = transform.position;
                newRoom.x -= 18;

                room = Physics2D.OverlapBox(newRoom, new Vector2(1, 1), 0);

                if (room != null && Vector3.Distance(room.transform.position, previousRoom) > 1)
                {
                    leftDoor.SetActive(true);
                }

                if (newRoom != previousRoom && room == null)
                {
                    GameObject nRoom = Instantiate(doors2dr[Random.Range(0, doors2dr.Length)], newRoom, transform.rotation);
                    nRoom.GetComponent<SpawnNeighboringRooms>().previousRoom = transform.position;
                }
                break;
            case 2:
                newRoom = transform.position;
                newRoom.x -= 18;

                room = Physics2D.OverlapBox(newRoom, new Vector2(1, 1), 0);

                if (room != null && Vector3.Distance(room.transform.position, previousRoom) > 1)
                {
                    leftDoor.SetActive(true);
                }

                if (newRoom != previousRoom && room == null)
                {
                    GameObject nRoom = Instantiate(doors2ur[Random.Range(0, doors2ur.Length)], newRoom, transform.rotation);
                    nRoom.GetComponent<SpawnNeighboringRooms>().previousRoom = transform.position;
                }
                break;
            case 3:
                newRoom = transform.position;
                newRoom.x -= 18;

                room = Physics2D.OverlapBox(newRoom, new Vector2(1, 1), 0);

                if (room != null && Vector3.Distance(room.transform.position, previousRoom) > 1)
                {
                    leftDoor.SetActive(true);
                }

                if (newRoom != previousRoom && room == null)
                {
                    GameObject nRoom = Instantiate(doors2lr[Random.Range(0, doors2lr.Length)], newRoom, transform.rotation);
                    nRoom.GetComponent<SpawnNeighboringRooms>().previousRoom = transform.position;
                }
                break;
            case 4:
                newRoom = transform.position;
                newRoom.x -= 18;

                room = Physics2D.OverlapBox(newRoom, new Vector2(1, 1), 0);

                if (room != null && Vector3.Distance(room.transform.position, previousRoom) > 1)
                {
                    leftDoor.SetActive(true);
                }

                if (newRoom != previousRoom && room == null)
                {
                    GameObject nRoom = Instantiate(doors3d[Random.Range(0, doors3d.Length)], newRoom, transform.rotation);
                    nRoom.GetComponent<SpawnNeighboringRooms>().previousRoom = transform.position;
                }
                break;
            case 5:
                newRoom = transform.position;
                newRoom.x -= 18;

                room = Physics2D.OverlapBox(newRoom, new Vector2(1, 1), 0);

                if (room != null && Vector3.Distance(room.transform.position, previousRoom) > 1)
                {
                    leftDoor.SetActive(true);
                }

                if (newRoom != previousRoom && room == null)
                {
                    GameObject nRoom = Instantiate(doors3l[Random.Range(0, doors3l.Length)], newRoom, transform.rotation);
                    nRoom.GetComponent<SpawnNeighboringRooms>().previousRoom = transform.position;
                }
                break;
            case 6:
                newRoom = transform.position;
                newRoom.x -= 18;

                room = Physics2D.OverlapBox(newRoom, new Vector2(1, 1), 0);

                if (room != null && Vector3.Distance(room.transform.position, previousRoom) > 1)
                {
                    leftDoor.SetActive(true);
                }

                if (newRoom != previousRoom && room == null)
                {
                    GameObject nRoom = Instantiate(doors3u[Random.Range(0, doors3u.Length)], newRoom, transform.rotation);
                    nRoom.GetComponent<SpawnNeighboringRooms>().previousRoom = transform.position;
                }
                break;
            case 7:
                newRoom = transform.position;
                newRoom.x -= 18;

                room = Physics2D.OverlapBox(newRoom, new Vector2(1, 1), 0);

                if (room != null && Vector3.Distance(room.transform.position, previousRoom) > 1)
                {
                    leftDoor.SetActive(true);
                }

                if (newRoom != previousRoom && room == null)
                {
                    GameObject nRoom = Instantiate(doors4[Random.Range(0, doors4.Length)], newRoom, transform.rotation);
                    nRoom.GetComponent<SpawnNeighboringRooms>().previousRoom = transform.position;
                }
                break;
        }
    }

    void SpawnRightRoom(int type)
    {
        switch (type)
        {
            case 0:
                Vector3 newRoom = transform.position;
                newRoom.x += 18;

                Collider2D room = Physics2D.OverlapBox(newRoom, new Vector2(1, 1), 0);

                if (room != null && Vector3.Distance(room.transform.position, previousRoom) > 1)
                {
                    rightDoor.SetActive(true);
                }

                if (newRoom != previousRoom && room == null)
                {
                    GameObject nRoom = Instantiate(doors1l[Random.Range(0, doors1l.Length)], newRoom, transform.rotation);
                    nRoom.GetComponent<SpawnNeighboringRooms>().previousRoom = transform.position;
                }
                break;
            case 1:
                newRoom = transform.position;
                newRoom.x += 18;

                room = Physics2D.OverlapBox(newRoom, new Vector2(1, 1), 0);

                if (room != null && Vector3.Distance(room.transform.position, previousRoom) > 1)
                {
                    rightDoor.SetActive(true);
                }

                if (newRoom != previousRoom && room == null)
                {
                    GameObject nRoom = Instantiate(doors2ul[Random.Range(0, doors2ul.Length)], newRoom, transform.rotation);
                    nRoom.GetComponent<SpawnNeighboringRooms>().previousRoom = transform.position;
                }
                break;
            case 2:
                newRoom = transform.position;
                newRoom.x += 18;

                room = Physics2D.OverlapBox(newRoom, new Vector2(1, 1), 0);

                if (room != null && Vector3.Distance(room.transform.position, previousRoom) > 1)
                {
                    rightDoor.SetActive(true);
                }

                if (newRoom != previousRoom && room == null)
                {
                    GameObject nRoom = Instantiate(doors2dl[Random.Range(0, doors2dl.Length)], newRoom, transform.rotation);
                    nRoom.GetComponent<SpawnNeighboringRooms>().previousRoom = transform.position;
                }
                break;
            case 3:
                newRoom = transform.position;
                newRoom.x += 18;

                room = Physics2D.OverlapBox(newRoom, new Vector2(1, 1), 0);

                if (room != null && Vector3.Distance(room.transform.position, previousRoom) > 1)
                {
                    rightDoor.SetActive(true);
                }

                if (newRoom != previousRoom && room == null)
                {
                    GameObject nRoom = Instantiate(doors2lr[Random.Range(0, doors2lr.Length)], newRoom, transform.rotation);
                    nRoom.GetComponent<SpawnNeighboringRooms>().previousRoom = transform.position;
                }
                break;
            case 4:
                newRoom = transform.position;
                newRoom.x += 18;

                room = Physics2D.OverlapBox(newRoom, new Vector2(1, 1), 0);

                if (room != null && Vector3.Distance(room.transform.position, previousRoom) > 1)
                {
                    rightDoor.SetActive(true);
                }

                if (newRoom != previousRoom && room == null)
                {
                    GameObject nRoom = Instantiate(doors3d[Random.Range(0, doors3d.Length)], newRoom, transform.rotation);
                    nRoom.GetComponent<SpawnNeighboringRooms>().previousRoom = transform.position;
                }
                break;
            case 5:
                newRoom = transform.position;
                newRoom.x += 18;

                room = Physics2D.OverlapBox(newRoom, new Vector2(1, 1), 0);

                if (room != null && Vector3.Distance(room.transform.position, previousRoom) > 1)
                {
                    rightDoor.SetActive(true);
                }

                if (newRoom != previousRoom && room == null)
                {
                    GameObject nRoom = Instantiate(doors3r[Random.Range(0, doors3r.Length)], newRoom, transform.rotation);
                    nRoom.GetComponent<SpawnNeighboringRooms>().previousRoom = transform.position;
                }
                break;
            case 6:
                newRoom = transform.position;
                newRoom.x += 18;

                room = Physics2D.OverlapBox(newRoom, new Vector2(1, 1), 0);

                if (room != null && Vector3.Distance(room.transform.position, previousRoom) > 1)
                {
                    rightDoor.SetActive(true);
                }

                if (newRoom != previousRoom && room == null)
                {
                    GameObject nRoom = Instantiate(doors3u[Random.Range(0, doors3u.Length)], newRoom, transform.rotation);
                    nRoom.GetComponent<SpawnNeighboringRooms>().previousRoom = transform.position;
                }
                break;
            case 7:
                newRoom = transform.position;
                newRoom.x += 18;

                room = Physics2D.OverlapBox(newRoom, new Vector2(1, 1), 0);

                if (room != null && Vector3.Distance(room.transform.position, previousRoom) > 1)
                {
                    rightDoor.SetActive(true);
                }

                if (newRoom != previousRoom && room == null)
                {
                    GameObject nRoom = Instantiate(doors4[Random.Range(0, doors4.Length)], newRoom, transform.rotation);
                    nRoom.GetComponent<SpawnNeighboringRooms>().previousRoom = transform.position;
                }
                break;
        }
    }
}
