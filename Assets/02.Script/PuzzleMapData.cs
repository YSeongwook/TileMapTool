using System.Collections.Generic;
using EnumTypes;
using EventLibrary;
using UnityEngine.UI;

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
        EventManager<TileEvent>.StartListening<int>(TileEvent.RotationSelectTileNodeInfo, RotationSelectTile);
    }

    private void OnDestroy()
    {
        EventManager<TileEvent>.StopListening<TileNode>(TileEvent.SelectTileNode, OnClickSelectTile);
        EventManager<TileEvent>.StopListening<Tile>(TileEvent.ChangedSelectTileNodeInfo, ChangedSelectTileInfo);
        EventManager<TileEvent>.StopListening<int>(TileEvent.RotationSelectTileNodeInfo, RotationSelectTile);

    }

    private void OnClickSelectTile(TileNode tileNode)
    {
        if (_selectTile == tileNode) return;
        if(_selectTile != null)
        {
            _selectTile.DisEnableOutLine();
        }

        _selectTile = tileNode;
    }

    private void ChangedSelectTileInfo(Tile tileInfo)
    {
        if (_selectTile == null) return;

        _selectTile.ChangedTileInfo(tileInfo);
    }

    private void RotationSelectTile(int Rotate)
    {
        _selectTile.ChangedTileRotate(Rotate);
    }

    public int GetSelectTileRotate()
    {
        return _selectTile.GetTileRotateValue();
    }
}
