using EnumTypes;
using EventLibrary;
using UnityEngine;

public class FileSaveAndLoad : MonoBehaviour
{
    public void OnClickSaveButton()
    {
        EventManager<JsonEvent>.TriggerEvent(JsonEvent.SaveData);
    }

    public void OnClickLoadButton()
    {
        EventManager<JsonEvent>.TriggerEvent(JsonEvent.LoadData);
    }
}
