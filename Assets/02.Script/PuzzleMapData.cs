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
        EventManager<TileEvent>.StartListening<Tile>(TileEvent.ChangedSelectTileInfo, ChangedSelectTileInfo);
        EventManager<TileEvent>.StartListening<int, bool>(TileEvent.RotationSelectTileNodeInfo, RotationSelectTile);
        EventManager<TileEvent>.StartListening<DeleteTileAttributeList>(TileEvent.DeleteTIleAttribute, DeleteGimmickTile);
    }

    private void OnDestroy()
    {
        EventManager<TileEvent>.StopListening<TileNode>(TileEvent.SelectTileNode, OnClickSelectTile);
        EventManager<TileEvent>.StopListening<Tile>(TileEvent.ChangedSelectTileInfo, ChangedSelectTileInfo);
        EventManager<TileEvent>.StopListening<int, bool>(TileEvent.RotationSelectTileNodeInfo, RotationSelectTile);
        EventManager<TileEvent>.StopListening<DeleteTileAttributeList>(TileEvent.DeleteTIleAttribute, DeleteGimmickTile);
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

    private void RotationSelectTile(int Rotate, bool isGimmick)
    {
        if (isGimmick)
            _selectTile.ChangedGimmickTileRotate(Rotate);
        else
           _selectTile.ChangedTileRotate(Rotate);
    }

    private void DeleteGimmickTile(DeleteTileAttributeList deleteType)
    {
        _selectTile.DeleteTileTypes(deleteType);
    }
}
