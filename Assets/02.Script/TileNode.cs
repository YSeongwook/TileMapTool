using EnumTypes;
using EventLibrary;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum TileType
{
    None,
    Wall,
    Path,
    StartPoint,
    EndPoint
}

public struct Tile
{
    public Sprite sprite;
    public TileType type;
    public int rotateValue;
    public int tileShape;
}

public class TileNode : MonoBehaviour
{
    private Tile tile;
    private Image image_Road;
    private Image image_Gimmick;
    private RectTransform rectTransform;

    public int instanID;

    private void Awake()
    {
        image_Road = transform.GetChild(0).GetComponent<Image>();
        image_Gimmick = transform.GetChild(1).GetComponent<Image>();
        rectTransform = GetComponent<RectTransform>();

        instanID = GetInstanceID();
    }

    private void Start()
    {
        RectTransform imageRoadRectTransform = image_Road.GetComponent<RectTransform>();
        imageRoadRectTransform.sizeDelta = rectTransform.sizeDelta - new Vector2(20, 20);

        RectTransform iageGimmickRectTransform = image_Gimmick.GetComponent<RectTransform>();
        iageGimmickRectTransform.sizeDelta = rectTransform.sizeDelta;
    }

    public void OnClickThisNode()
    {
        EventManager<TileEvent>.TriggerEvent(TileEvent.SelectTileNode, this);
    }
}
