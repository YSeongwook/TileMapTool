using EnumTypes;
using EventLibrary;
using UnityEngine;

public class TileRotation : MonoBehaviour
{
    private int _roadRotateValue = 0;
    private int _gimmickRotateValue = 0;
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
        this._roadRotateValue = roadRotateValue;
        this._gimmickRotateValue = gimmickRotateValue;
        _isGimmickTile = (gimmickRotateValue != -1); // 기믹 타일인지 여부를 판단
    }

    public void OnClickRightRotate()
    {
        if (_isGimmickTile)
        {
            _gimmickRotateValue = (_gimmickRotateValue + 1) % 4;
            _roadRotateValue = _gimmickRotateValue; // 길도 함께 회전
        }
        else
        {
            _roadRotateValue = (_roadRotateValue + 1) % 4;
        }
        EventManager<TileEvent>.TriggerEvent<int, bool>(TileEvent.RotationSelectTileNodeInfo, _roadRotateValue, _isGimmickTile);
    }

    public void OnClickLeftRotate()
    {
        if (_isGimmickTile)
        {
            _gimmickRotateValue = (_gimmickRotateValue + 3) % 4;
            _roadRotateValue = _gimmickRotateValue; // 길도 함께 회전
        }
        else
        {
            _roadRotateValue = (_roadRotateValue + 3) % 4;
        }
        EventManager<TileEvent>.TriggerEvent<int, bool>(TileEvent.RotationSelectTileNodeInfo, _roadRotateValue, _isGimmickTile);
    }
}