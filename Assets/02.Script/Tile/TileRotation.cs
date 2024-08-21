using EnumTypes;
using EventLibrary;
using UnityEngine;

public class TileRotation : MonoBehaviour
{
    private int _rotateValue = 0;

    private void Awake()
    {
        EventManager<TileEvent>.StartListening<int>(TileEvent.GetTileRotateValue, GetSelectTileRotate);
    }

    private void OnDestroy()
    {
        EventManager<TileEvent>.StopListening<int>(TileEvent.GetTileRotateValue, GetSelectTileRotate);
    }

    private void GetSelectTileRotate(int rotateValue)
    {
        _rotateValue = rotateValue;
    }

    public void OnClickLeftRotate()
    {
        // 현재 선택된 타일을 PuzzleMapData.Instance._selectTile로 참조
        if (PuzzleMapData.Instance._selectTile != null)
        {
            _rotateValue = (_rotateValue + 3) % 4; // 왼쪽으로 90도 회전 (270도 회전)
            EventManager<TileEvent>.TriggerEvent(TileEvent.RotationSelectTileNodeInfo, _rotateValue);
        }
    }

    public void OnClickRightRotate()
    {
        // 현재 선택된 타일을 PuzzleMapData.Instance._selectTile로 참조
        if (PuzzleMapData.Instance._selectTile != null)
        {
            _rotateValue = (_rotateValue + 1) % 4; // 오른쪽으로 90도 회전
            EventManager<TileEvent>.TriggerEvent(TileEvent.RotationSelectTileNodeInfo, _rotateValue);
        }
    }
}