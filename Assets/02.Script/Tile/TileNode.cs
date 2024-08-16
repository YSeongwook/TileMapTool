using EnumTypes;
using EventLibrary;
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
    public Sprite Sprite;
    public TileType Type;
    public int RotateValue;
    public int TileShape;
}

public class TileNode : MonoBehaviour
{
    public int instanID;
    
    private Tile _tile;
    private Image _background;
    private Image _imageRoad;
    private Image _imageGimmick;
    private RectTransform _rectTransform;

    private void Awake()
    {
        _background = transform.GetChild(0).GetComponent<Image>();
        _imageRoad = transform.GetChild(1).GetComponent<Image>();
        _imageGimmick = transform.GetChild(2).GetComponent<Image>();
        _rectTransform = GetComponent<RectTransform>();

        instanID = GetInstanceID();
    }

    private void Start()
    {
        RectTransform imageBackGroundRectTransform = _background.GetComponent<RectTransform>();
        imageBackGroundRectTransform.sizeDelta = new Vector2(120, 120);
        _background.enabled = false;

        RectTransform imageRoadRectTransform = _imageRoad.GetComponent<RectTransform>();
        imageRoadRectTransform.sizeDelta = new Vector2(120, 120);
        _imageRoad.enabled = false;

        RectTransform iageGimmickRectTransform = _imageGimmick.GetComponent<RectTransform>();
        iageGimmickRectTransform.sizeDelta = _rectTransform.sizeDelta - new Vector2(10, 10);
        _imageGimmick.enabled = false;
    }

    public void OnClickThisNode()
    {
        EventManager<TileEvent>.TriggerEvent(TileEvent.SelectTileNode, this);
    }

    public void ChangedTileInfo(Tile tileInfo)
    {
        _tile.Sprite = tileInfo.Sprite;
        _tile.Type = tileInfo.Type;  
        _tile.RotateValue = 0;
        _tile.TileShape = tileInfo.TileShape;

        _background.enabled = true;  

        if (tileInfo.Type != TileType.Wall)
        {
            _imageRoad.enabled = true;
            _imageRoad.sprite = tileInfo.Sprite;
        }
        else
        {
            _imageRoad.enabled = true;
            _imageGimmick.sprite = tileInfo.Sprite;
        }             
    }
}
