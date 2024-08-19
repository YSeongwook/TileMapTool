using EnumTypes;
using EventLibrary;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FileSaveAndLoad : MonoBehaviour
{
    public void OnClickSaveButton()
    {
        EventManager<TileEvent>.TriggerEvent(TileEvent.SaveData);
    }

    public void OnClickLoadButton()
    {
        EventManager<TileEvent>.TriggerEvent(TileEvent.LoadData);
    }
}
