//*****************************************************************************
// File Name :         WhipIconBehavior.cs
// Author :            Kyle Manning
// Creation Date :     April 2, 2021
//
// Brief Description : Has a custom cursor follow the mouse on screen. For the
//                     primary cursor movement is restricted to a radius of
//                     3.5 units around the player, changing sprites to show
//                     when the whip can be attached to a surface or not. For
//                     the secondary cursor, it appears when the mouse is more
//                     than 3.5 units from the player to let them know where
//                     the mouse is, disappearing when it re-enters the radius.
//******************************************************************************
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhipIconBehavior : MonoBehaviour
{
    public bool isSecondary;
    public GameController gc;
    public Transform player;
    private SpriteRenderer sr;
    public LayerMask mask;
    public Sprite s1;
    public Sprite s2;

    /// <summary>
    /// Assigns the sprite renderer component to its variable
    /// </summary>
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    /// <summary>
    /// While the game is not paused, FollowMouse() is called every frame
    /// </summary>
    void Update()
    {
        if (!gc.isPaused)
        {
            FollowMouse();
        }
    }

    /// <summary>
    /// Makes the cursor object follow the mouse. If it is not marked as the secondary cursor,
    /// movement is limited to a 3.5 unit radius around the player where it changes sprites
    /// depending on what the mouse is over. If it is marked as the secondary, it follows the
    /// mouse outside the radius and disappears when it enters that 3.5 unit radius
    /// </summary>
    private void FollowMouse()
    {
        Vector3 targetPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        targetPos.z = 0;
        transform.position = targetPos;

        if (!isSecondary)
        {
            float radius = 3.5f;
            Vector3 centerPos = player.position;
            float distance = Vector3.Distance(transform.position, centerPos);

            if (distance > radius)
            {
                Vector3 fromOriginToObj = transform.position - centerPos;
                fromOriginToObj *= radius / distance;
                transform.position = centerPos + fromOriginToObj;
            }

            Collider2D hit = Physics2D.OverlapCircle(targetPos, 0.1f, mask);

            if (Vector3.Distance(player.position, transform.position) < 3.5f && hit != null)
            {
                sr.sprite = s1;
            }
            else
            {
                sr.sprite = s2;
            }
        }
        else
        {
            if (Vector3.Distance(player.position, transform.position) > 3.5f)
            {
                sr.enabled = true;
            }
            else
            {
                sr.enabled = false;
            }
        }
    }
}
