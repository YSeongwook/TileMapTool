using UnityEngine;

namespace EnumTypes
{
    public enum Layers
    {
        Default,
        TransparentFX,
        IgnoreRaycast,
        Reserved1,
        Water,
        UI,
        Reserved2,
        Reserved3,
        Player,
        Enemy,
    }

    public enum UIEvents
    {
        SavePopUp,
        ErrorPopUP,
    }

    public enum DataEvents
    {
        TileSave
    }

    public enum GoldEvent
    {
        OnGetGold,
        OnUseERC,
        OnGetERC
    }

    public enum TileEvent
    {
        SelectTileNode,
        ChangedSelectTileInfo,
        RotationSelectTileNodeInfo,
        GetTileRotateValue,
        DeleteTIleAttribute,
    }

    public enum JsonEvent
    {
        SaveData,
        JsonSaveData,
        LoadData,
        JsonLoadData
    }
    
    public enum DeleteTileAttributeList 
    {
        Gimmick,
        Road,
        All
    }
    
    public class EnumTypes : MonoBehaviour
    {
    }
}