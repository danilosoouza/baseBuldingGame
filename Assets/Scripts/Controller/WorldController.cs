using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WorldController : MonoBehaviour
{
    public static WorldController Instance { get; protected set; }

    public World World { get; protected set; }

    public Sprite floorSprite;

    private void Start()
    {
        if(Instance != null)
        {
            Debug.LogError("there should never be two world controller");
        }

        Instance = this;

        World = new World();

        for (int x = 0; x < World.Width; x++)
        {
            for (int y = 0; y < World.Height; y++)
            {
                Tile tile_data = World.GetTileAt(x, y);

                GameObject tile_go = new GameObject();
                tile_go.transform.position = new Vector3(tile_data.X, tile_data.Y);
                tile_go.name = "Tile[" + x + "," + y + "]";
                tile_go.transform.SetParent(this.transform, true);

                SpriteRenderer tile_sr = tile_go.AddComponent<SpriteRenderer>();


                tile_data.RegisterTileTypeChangedCallback((tile) => { OnTileTypeChanged(tile, tile_go); });
            }
        }
        World.RandomizeTiles();
    }

    void OnTileTypeChanged(Tile tile_data, GameObject tile_go)
    {
        if(tile_data.Type == Tile.TileType.FLOOR)
        {
            tile_go.GetComponent<SpriteRenderer>().sprite = floorSprite;
        }
        else if(tile_data.Type == Tile.TileType.EMPTY)
        {
            tile_go.GetComponent<SpriteRenderer>().sprite = null;
        }
        else
        {
            Debug.LogError("OnTileTypeChanged - Unrecognized tile type");
        }
    }


}
