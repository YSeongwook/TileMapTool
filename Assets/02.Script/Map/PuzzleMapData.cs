using System.Collections.Generic;
using EnumTypes;
using EventLibrary;
using UnityEngine;


public class PuzzleMapData : Singleton<PuzzleMapData>
{
    public TileNode _selectTile {  get; private set; }
    private List<Tile> _tileNodes = new List<Tile>();
    public MapGenerator MapGenerator;
    public JsonSaveLoader JsonSaveLoader;

    protected new void Awake()
    {
        base.Awake();

        _selectTile = null;
        AddEvents();
    }

    private void OnDestroy()
    {
        RemoveEvents();
    }

    private void AddEvents()
    {
        EventManager<TileEvent>.StartListening<TileNode>(TileEvent.SelectTileNode, OnClickSelectTile);
        EventManager<TileEvent>.StartListening<Tile, Sprite>(TileEvent.ChangedSelectTileInfo, ChangedSelectTileInfo);
        EventManager<TileEvent>.StartListening<int, bool>(TileEvent.RotationSelectTileNodeInfo, RotationSelectTile);
        EventManager<TileEvent>.StartListening<DeleteTileAttributeList>(TileEvent.DeleteTIleAttribute, DeleteGimmickTile);
        EventManager<TileEvent>.StartListening(TileEvent.SaveData, SaveTileData);
        EventManager<TileEvent>.StartListening(TileEvent.LoadData, LoadTileData);
    }

    private void RemoveEvents()
    {
        EventManager<TileEvent>.StopListening<TileNode>(TileEvent.SelectTileNode, OnClickSelectTile);
        EventManager<TileEvent>.StopListening<Tile, Sprite>(TileEvent.ChangedSelectTileInfo, ChangedSelectTileInfo);
        EventManager<TileEvent>.StopListening<int, bool>(TileEvent.RotationSelectTileNodeInfo, RotationSelectTile);
        EventManager<TileEvent>.StopListening<DeleteTileAttributeList>(TileEvent.DeleteTIleAttribute, DeleteGimmickTile);
        EventManager<TileEvent>.StopListening(TileEvent.SaveData, SaveTileData);
        EventManager<TileEvent>.StopListening(TileEvent.LoadData, LoadTileData);
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

    private void ChangedSelectTileInfo(Tile tileInfo, Sprite sprite)
    {
        if (_selectTile == null) return;

        _selectTile.ChangedTileInfo(tileInfo, sprite);
    }

    private void RotationSelectTile(int rotate, bool isGimmick)
    {
        if (isGimmick)
            _selectTile.ChangedGimmickTileRotate(rotate);
        else
           _selectTile.ChangedTileRotate(rotate);
    }

    private void DeleteGimmickTile(DeleteTileAttributeList deleteType)
    {
        _selectTile.DeleteTileTypes(deleteType);
    }

    private void SaveTileData()
    {
        _tileNodes = MapGenerator.GetTileList();

        //JsonSaveLoader에게 데이터 전달
        EventManager<TileEvent>.TriggerEvent(TileEvent.JsonSaveData, _tileNodes);
    }

    private void LoadTileData()
    {
        //JsonSaveLoader에게 데이터 받아옴
        _tileNodes = JsonSaveLoader.LoadJsonFile();

        EventManager<TileEvent>.TriggerEvent(TileEvent.JsonLoadData, _tileNodes);
    }
}
