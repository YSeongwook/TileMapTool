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

        EventManager<TileEvent>.StartListening<TileNode>(TileEvent.SelectTileNode, OnClickSelectTile);
    }

    private void OnDestroy()
    {
        EventManager<TileEvent>.StopListening<TileNode>(TileEvent.SelectTileNode, OnClickSelectTile);
    }

    private void OnClickSelectTile(TileNode tileNode)
    {
        _selectTile = tileNode;
    }
}
