using EnumTypes;
using EventLibrary;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DeleteTileAttributeList 
{
    Gimmick,
    Road,
    All
}

public class DeleteTileAttribute : MonoBehaviour
{
    public void OnClickDeleteGimmickTile()
    {
        EventManager<TileEvent>.TriggerEvent(TileEvent.DeleteTIleAttribute, DeleteTileAttributeList.Gimmick);
    }

    public void OnClickDeleteRoadTIle()
    {
        EventManager<TileEvent>.TriggerEvent(TileEvent.DeleteTIleAttribute, DeleteTileAttributeList.Road);
    }
    public void OnClickDeleteTIleAll()
    {
        EventManager<TileEvent>.TriggerEvent(TileEvent.DeleteTIleAttribute, DeleteTileAttributeList.All);
    }
}
