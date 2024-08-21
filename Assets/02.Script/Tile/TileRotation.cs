using EnumTypes;
using EventLibrary;
using UnityEngine;

public class TileRotation : MonoBehaviour
{
    private int _rotateValue = 0;
    private bool _isGimmickTile;

    private void Awake()
    {
        EventManager<TileEvent>.StartListening<int, int>(TileEvent.GetTileRotateValue, GetSelectTileRotate);
    }

    private void OnDestroy()
    {
        EventManager<TileEvent>.StopListening<int, int>(TileEvent.GetTileRotateValue, GetSelectTileRotate);
    }

    private void GetSelectTileRotate(int roadRotateValue, int gimmickRotateValue)
    {
        _rotateValue = _isGimmickTile ? gimmickRotateValue : roadRotateValue;
        _isGimmickTile = (gimmickRotateValue != -1); // 기믹 타일인지 여부를 판단
    }

    public void OnClickLeftRotate()
    {
        // 현재 선택된 타일을 PuzzleMapData.Instance._selectTile로 참조
        if (PuzzleMapData.Instance._selectTile != null)
        {
            _rotateValue = (_rotateValue + 3) % 4; // 왼쪽으로 90도 회전 (270도 회전)
            EventManager<TileEvent>.TriggerEvent<int, bool>(TileEvent.RotationSelectTileNodeInfo, _rotateValue, _isGimmickTile);
        }
    }

    public void OnClickRightRotate()
    {
        // 현재 선택된 타일을 PuzzleMapData.Instance._selectTile로 참조
        if (PuzzleMapData.Instance._selectTile != null)
        {
            _rotateValue = (_rotateValue + 1) % 4; // 오른쪽으로 90도 회전
            EventManager<TileEvent>.TriggerEvent<int, bool>(TileEvent.RotationSelectTileNodeInfo, _rotateValue, _isGimmickTile);
        }
    }
}