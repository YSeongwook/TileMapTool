using EnumTypes;
using EventLibrary;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileRotation : MonoBehaviour
{
    private int RotateValue = 0;

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
        this.RotateValue = rotateValue;
    }

    public void OnClickRightRotate()
    {
        RotateValue = (RotateValue + 1) % 4;
        EventManager<TileEvent>.TriggerEvent(TileEvent.RotationSelectTileNodeInfo, RotateValue);
    }

    public void OnClickLeftRotate()
    {
        RotateValue = (RotateValue + 3) % 4;
        EventManager<TileEvent>.TriggerEvent(TileEvent.RotationSelectTileNodeInfo, RotateValue);
    }
}
