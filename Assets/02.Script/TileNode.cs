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
    private Image background;
    private Image image_Road;
    private Image image_Gimmick;
    private RectTransform rectTransform;

    public int instanID;

    private void Awake()
    {
        background = transform.GetChild(0).GetComponent<Image>();
        image_Road = transform.GetChild(1).GetComponent<Image>();
        image_Gimmick = transform.GetChild(2).GetComponent<Image>();
        rectTransform = GetComponent<RectTransform>();

        instanID = GetInstanceID();
    }

    private void Start()
    {
        RectTransform imageBackGroundRectTransform = background.GetComponent<RectTransform>();
        imageBackGroundRectTransform.sizeDelta = new Vector2(120, 120);
        background.enabled = false;

        RectTransform imageRoadRectTransform = image_Road.GetComponent<RectTransform>();
        imageRoadRectTransform.sizeDelta = new Vector2(120, 120);
        image_Road.enabled = false;

        RectTransform iageGimmickRectTransform = image_Gimmick.GetComponent<RectTransform>();
        iageGimmickRectTransform.sizeDelta = rectTransform.sizeDelta - new Vector2(10, 10);
        image_Gimmick.enabled = false;
    }

    public void OnClickThisNode()
    {
        EventManager<TileEvent>.TriggerEvent(TileEvent.SelectTileNode, this);
    }

    public void ChangedTileInfo(Tile tileInfo)
    {
        tile.sprite = tileInfo.sprite;
        tile.type = tileInfo.type;  
        tile.rotateValue = 0;
        tile.tileShape = tileInfo.tileShape;

        background.enabled = true;  

        if (tileInfo.type != TileType.Wall)
        {
            image_Road.enabled = true;
            image_Road.sprite = tileInfo.sprite;
        }
        else
        {
            image_Road.enabled = true;
            image_Gimmick.sprite = tileInfo.sprite;
        }             
    }
}
