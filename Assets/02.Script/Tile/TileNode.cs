using EnumTypes;
using EventLibrary;
using UnityEngine;
using UnityEngine.UI;

public enum TileType
{
    None,
    Gimmick,
    Path,
    StartPoint,
    EndPoint
}

public enum GimmickType
{
    None,
    Gimmick1,
    Gimmick2,   
}

public struct Tile
{
    public Sprite Sprite;
    public TileType Type;
    public GimmickType GimmickType;
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

    private RectTransform _imageRoadRectTransform;
    private RectTransform _iageGimmickRectTransform;

    private Outline _backgroundOutline;

    private void Awake()
    {
        _background = transform.GetChild(0).GetComponent<Image>();
        _backgroundOutline = transform.GetChild(0).GetComponent<Outline>();
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

        _imageRoadRectTransform = _imageRoad.GetComponent<RectTransform>();
        _imageRoadRectTransform.sizeDelta = new Vector2(120, 120);
        _imageRoad.enabled = false;

        _iageGimmickRectTransform = _imageGimmick.GetComponent<RectTransform>();
        _iageGimmickRectTransform.sizeDelta = _rectTransform.sizeDelta - new Vector2(10, 10);
        _imageGimmick.enabled = false;

        _backgroundOutline.enabled = false;
    }

    public void OnClickThisNode()
    {
        EventManager<TileEvent>.TriggerEvent(TileEvent.SelectTileNode, this);
        EventManager<TileEvent>.TriggerEvent(TileEvent.GetTileRotateValue, _tile.RotateValue);

        _backgroundOutline.enabled = true;
    }

    public void ChangedTileInfo(Tile tileInfo)
    {
        _tile.Sprite = tileInfo.Sprite;
        _tile.Type = tileInfo.Type;
        _tile.GimmickType = tileInfo.GimmickType;
        _tile.RotateValue = 0;
        _tile.TileShape = tileInfo.TileShape;

        _background.enabled = true;  

        if (tileInfo.Type != TileType.Gimmick)
        {
            _imageRoad.enabled = true;
            _imageRoad.sprite = tileInfo.Sprite;
        }
        else
        {
            _imageGimmick.enabled = true;
            _imageGimmick.sprite = tileInfo.Sprite;
        }             
    }

    public int GetTileRotateValue()
    {
        return _tile.RotateValue;
    }

    public void ChangedTileRotate(int rotateValue)
    {
        _tile.RotateValue = rotateValue;

        float rotationAngle = (rotateValue) * -90f;

        if(_tile.GimmickType == GimmickType.None)
            _imageRoadRectTransform.rotation = Quaternion.Euler(0, 0, rotationAngle);
        else 
            _iageGimmickRectTransform.rotation = Quaternion.Euler(0,0,rotationAngle);
    }

    public void DisEnableOutLine()
    {
        _backgroundOutline.enabled = false;
    }
}
