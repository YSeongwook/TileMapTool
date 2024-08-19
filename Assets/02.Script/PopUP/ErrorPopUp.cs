using EnumTypes;
using EventLibrary;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ErrorPopUp : MonoBehaviour
{
    [SerializeField] private TMP_Text ErrorMessage;

    private void Awake()
    {
        EventManager<UIEvents>.StartListening<string>(UIEvents.ErrorPopUP, PopUp);
    }

    private void OnDestroy()
    {
        EventManager<UIEvents>.StopListening<string>(UIEvents.ErrorPopUP, PopUp);
    }

    private void Start()
    {
        gameObject.SetActive(false);
    }

    private void PopUp(string errorMessage)
    {
        gameObject.SetActive(true);
        this.ErrorMessage.text = errorMessage;
    }
}
