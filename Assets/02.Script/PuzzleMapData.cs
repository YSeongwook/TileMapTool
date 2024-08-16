using System.Collections.Generic;
using EnumTypes;
using EventLibrary;

public class PuzzleMapData : Singleton<PuzzleMapData>
{
    private TileNode _selectTile;
    private List<Tile> _tileNodes = new List<Tile>();

    protected new void Awake()
    {
        base.Awake();

        _selectTile = null;
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
        _selectTile = tileNode;
    }

    private void ChangedSelectTileInfo(Tile tileInfo)
    {
        _selectTile.ChangedTileInfo(tileInfo);
    }
}
