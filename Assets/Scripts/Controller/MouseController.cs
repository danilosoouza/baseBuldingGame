﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseController : MonoBehaviour
{

    public GameObject mouseCursor;

    Vector3 lastFramePosition;
    Vector3 dragStartPosition;

    private void Update()
    {
        Vector3 currFramePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        currFramePosition.z = 0;

        //update the mouse cursor position
        Tile tileUnderMouse = GetTileAtWorldCoord(currFramePosition);
        if(tileUnderMouse != null)
        {
            mouseCursor.SetActive(true);
            Vector3 cursorPosition = new Vector3(tileUnderMouse.X, tileUnderMouse.Y, 0);
            mouseCursor.transform.position = cursorPosition;
        }
        else
        {
            mouseCursor.SetActive(false);
        }

        //Start drag
        if (Input.GetMouseButtonDown(0))
        {
            dragStartPosition = currFramePosition;
        }

        //end drag
        if (Input.GetMouseButtonUp(0))
        {
            int start_x = Mathf.FloorToInt(dragStartPosition.x);
            int end_x = Mathf.FloorToInt(currFramePosition.x);

            if(end_x < start_x)
            {
                int tmp = end_x;
                end_x = start_x;
                start_x = tmp;
            }

            int start_y = Mathf.FloorToInt(dragStartPosition.y);
            int end_y = Mathf.FloorToInt(currFramePosition.y);

            if (end_y < start_y)
            {
                int tmp = end_y;
                end_y = start_y;
                start_y = tmp;
            }

            for (int x = start_x; x <= end_x; x++)
            {
                for (int y = start_y; y <= end_y; y++)
                {
                    Tile t = WorldController.Instance.World.GetTileAt(x, y);
                    if(t != null)
                    {
                        t.Type = Tile.TileType.FLOOR;
                    }
                }
            }

        }


        //handle screen dragging
        if(Input.GetMouseButton(1) || Input.GetMouseButton(2))
        {
            Vector3 diff = lastFramePosition - currFramePosition;

            Camera.main.transform.Translate(diff);
        }

        lastFramePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        lastFramePosition.z = 0;
    }

    Tile GetTileAtWorldCoord(Vector3 coord)
    {
        int x = Mathf.FloorToInt(coord.x);
        int y = Mathf.FloorToInt(coord.y);

        return WorldController.Instance.World.GetTileAt(x, y);
    }



}
