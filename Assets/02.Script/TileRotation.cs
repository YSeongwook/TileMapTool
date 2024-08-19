using EnumTypes;
using EventLibrary;
using UnityEngine;

public class TileRotation : MonoBehaviour
{
    private int RotateValue = 0;
    private bool isGimmickTile;

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
        var rotateValue = 0;
        if (isGimmickTile)
        {
            rotateValue = 1;
        }
        else
        {
            RotateValue = (RotateValue + 1) % 4;
            rotateValue = RotateValue;
        }
        EventManager<TileEvent>.TriggerEvent(TileEvent.RotationSelectTileNodeInfo, rotateValue, isGimmickTile);
    }

    public void OnClickLeftRotate()
    {
        var rotateValue = 0;
        if (isGimmickTile)
        {
            rotateValue = -1;
        }
        else
        {
            RotateValue = (RotateValue + 3) % 4;
            rotateValue = RotateValue;
        }
        EventManager<TileEvent>.TriggerEvent(TileEvent.RotationSelectTileNodeInfo, rotateValue, isGimmickTile);
    }        
    
}
