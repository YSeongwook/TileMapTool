using EnumTypes;
using EventLibrary;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePopUp : MonoBehaviour
{
    private void Awake()
    {
        EventManager<UIEvents>.StartListening(UIEvents.SavePopUp, PopUp);
    }

    private void Start()
    {
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        EventManager<UIEvents>.StopListening(UIEvents.SavePopUp, PopUp);
    }

    private void PopUp()
    {
        gameObject.SetActive(true);
    }

    public void OnClickSaveData()
    {
        EventManager<DataEvents>.TriggerEvent(DataEvents.TileSave);
    }
}
