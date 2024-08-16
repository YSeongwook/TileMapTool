using EnumTypes;
using EventLibrary;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleMapData : Singleton<PuzzleMapData>
{
    private TileNode SelectTile;
    private List<Tile> TileNodes = new List<Tile>();

    protected new void Awake()
    {
        base.Awake();

        EventManager<TileEvent>.StartListening<TileNode>(TileEvent.SelectTileNode, OnClickSelectTile);
    }

    private void OnDestroy()
    {
        EventManager<TileEvent>.StopListening<TileNode>(TileEvent.SelectTileNode, OnClickSelectTile);
    }

    private void OnClickSelectTile(TileNode tileNode)
    {
        SelectTile = tileNode;
        Debug.Log(SelectTile.instanID);
    }
    
}
