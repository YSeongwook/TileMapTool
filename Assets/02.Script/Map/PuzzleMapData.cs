using System.Collections.Generic;
using EnumTypes;
using EventLibrary;
using UnityEngine;

public class PuzzleMapData : Singleton<PuzzleMapData>
{
    public TileNode _selectTile { get; private set; }
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
        EventManager<TileEvent>.StartListening<Tile, Sprite, Sprite>(TileEvent.ChangedSelectTileInfo, ChangedSelectTileInfo);
        EventManager<TileEvent>.StartListening<int>(TileEvent.RotationSelectTileNodeInfo, RotationSelectTile);
        EventManager<TileEvent>.StartListening<DeleteTileAttributeList>(TileEvent.DeleteTIleAttribute, DeleteGimmickTile);
        EventManager<JsonEvent>.StartListening(JsonEvent.SaveData, SaveTileData);
        EventManager<JsonEvent>.StartListening(JsonEvent.LoadData, LoadTileData);
    }

    private void RemoveEvents()
    {
        EventManager<TileEvent>.StopListening<TileNode>(TileEvent.SelectTileNode, OnClickSelectTile);
        EventManager<TileEvent>.StopListening<Tile, Sprite, Sprite>(TileEvent.ChangedSelectTileInfo, ChangedSelectTileInfo);
        EventManager<TileEvent>.StopListening<int>(TileEvent.RotationSelectTileNodeInfo, RotationSelectTile);
        EventManager<TileEvent>.StopListening<DeleteTileAttributeList>(TileEvent.DeleteTIleAttribute, DeleteGimmickTile);
        EventManager<JsonEvent>.StopListening(JsonEvent.SaveData, SaveTileData);
        EventManager<JsonEvent>.StopListening(JsonEvent.LoadData, LoadTileData);
    }

    private void OnClickSelectTile(TileNode tileNode)
    {
        if (_selectTile == tileNode) return;
        if (_selectTile != null)
        {
            _selectTile.DisEnableOutLine();
        }

        _selectTile = tileNode;
    }

    private void ChangedSelectTileInfo(Tile tileInfo, Sprite roadSprite, Sprite gimmickSprite)
    {
        if (_selectTile == null) return;

        // 현재 타일의 정보를 가져옵니다.
        Tile currentTileInfo = _selectTile.GetTileInfo;
        
        // 길 타일 스프라이트가 제공된 경우, 무조건 길 타일을 업데이트
        if (roadSprite != null)
        {
            _selectTile.ChangedTileInfo(tileInfo, roadSprite, null); // 길 타일만 변경
        }

        // 기믹 타일 스프라이트가 null이 아니면 기믹 타일만 변경
        if (gimmickSprite != null)
        {
            _selectTile.ChangedTileInfo(tileInfo, currentTileInfo.RoadShape != RoadShape.None ? roadSprite : null, gimmickSprite);
        }
    }

    private void RotationSelectTile(int rotate)
    {
        if (_selectTile == null) return;

        _selectTile.ChangedRoadTileRotate(rotate);
    }

    private void DeleteGimmickTile(DeleteTileAttributeList deleteType)
    {
        if (_selectTile == null) return;

        _selectTile.DeleteTileTypes(deleteType);
    }

    private void SaveTileData()
    {
        _tileNodes = MapGenerator.GetTileList();

        //JsonSaveLoader에게 데이터 전달
        if(_tileNodes != null) EventManager<JsonEvent>.TriggerEvent(JsonEvent.JsonSaveData, _tileNodes);
    }

    private void LoadTileData()
    {
        //JsonSaveLoader에게 데이터 받아옴
        _tileNodes = JsonSaveLoader.LoadJsonFile();

        if(_tileNodes != null) EventManager<JsonEvent>.TriggerEvent(JsonEvent.JsonLoadData, _tileNodes);
    }
}
