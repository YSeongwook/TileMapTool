using EnumTypes;
using EventLibrary;
using UnityEngine;

public class TileRotation : MonoBehaviour
{
    private int RoadRotateValue = 0;
    private int GimmickRotateValue = 0;
    private bool isGimmickTile;

    private void Awake()
    {
        EventManager<TileEvent>.StartListening<int, int>(TileEvent.GetTileRotateValue, GetSelectTileRotate);
    }

    private void OnDestroy()
    {
        EventManager<TileEvent>.StopListening<int, int>(TileEvent.GetTileRotateValue, GetSelectTileRotate);
    }

    private void GetSelectTileRotate(int roadRotateValue, int GimmickRotateValue)
    {
        this.RoadRotateValue = roadRotateValue;
        this.GimmickRotateValue = GimmickRotateValue;
    }

    public void OnClickRightRotate()
    {
        var rotateValue = 0;
        if (isGimmickTile)
        {
            GimmickRotateValue = (GimmickRotateValue + 1) % 4;
            rotateValue = GimmickRotateValue;
        }
        else
        {
            RoadRotateValue = (RoadRotateValue + 1) % 4;
            rotateValue = RoadRotateValue;
        }
        EventManager<TileEvent>.TriggerEvent(TileEvent.RotationSelectTileNodeInfo, rotateValue, isGimmickTile);
    }

    public void OnClickLeftRotate()
    {
        var rotateValue = 0;
        if (isGimmickTile)
        {
            GimmickRotateValue = (GimmickRotateValue + 3) % 4;
            rotateValue = GimmickRotateValue;
        }
        else
        {
            RoadRotateValue = (RoadRotateValue + 3) % 4;
            rotateValue = RoadRotateValue;
        }
        EventManager<TileEvent>.TriggerEvent(TileEvent.RotationSelectTileNodeInfo, rotateValue, isGimmickTile);
    }        
    
}
