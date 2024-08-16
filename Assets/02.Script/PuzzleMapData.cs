using EnumTypes;
using EventLibrary;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PuzzleMapData : Singleton<PuzzleMapData>
{
    private TileNode SelectTile;
    private List<Tile> TileNodes = new List<Tile>();

    protected new void Awake()
    {
        base.Awake();

        SelectTile = null;
        EventManager<TileEvent>.StartListening<TileNode>(TileEvent.SelectTileNode, OnClickSelectTile);
        EventManager<TileEvent>.StartListening<Tile>(TileEvent.ChangedSelectTileNodeInfo, ChangedSelectTileInfo);
    }

    private void OnDestroy()
    {
        EventManager<TileEvent>.StopListening<TileNode>(TileEvent.SelectTileNode, OnClickSelectTile);
        EventManager<TileEvent>.StopListening<Tile>(TileEvent.ChangedSelectTileNodeInfo, ChangedSelectTileInfo);
    }

    private void OnClickSelectTile(TileNode tileNode)
    {
        SelectTile = tileNode;
    }

    private void ChangedSelectTileInfo(Tile tileInfo)
    {
        SelectTile.ChangedTileInfo(tileInfo);
    }
}
