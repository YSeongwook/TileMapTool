using EnumTypes;
using EventLibrary;
using UnityEngine;

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
        // EventManager<TileEvent>.TriggerEvent(TileEvent.DeleteTIleAttribute, DeleteTileAttributeList.All);
        EventManager<DeleteTileAttributeList>.TriggerEvent(DeleteTileAttributeList.All, MapGenerator.Instance.PreviousSize);
    }
}
