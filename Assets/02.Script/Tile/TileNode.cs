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
    public TileType Type;
    public GimmickType GimmickType;
    public int RotateRoadValue;
    public int RotateGimmickValue;
    public int RoadTileShape;
    public int GimmickTileShape;
}

public class TileNode : MonoBehaviour
{
    private Tile _tile;
    public Tile GetTileInfo {  get { return _tile; } }

    private Image _background;
    private Image _imageRoad;
    private Image _imageGimmick;
    private RectTransform _rectTransform;
    private RectTransform _imageRoadRectTransform;
    private RectTransform _imageGimmickRectTransform;
    private Outline _backgroundOutline;
    private bool _isLoad;

    private void Awake()
    {
        _background = transform.GetChild(0).GetComponent<Image>();
        _backgroundOutline = transform.GetChild(0).GetComponent<Outline>();
        _imageRoad = transform.GetChild(1).GetComponent<Image>();
        _imageGimmick = transform.GetChild(2).GetComponent<Image>();
        _rectTransform = GetComponent<RectTransform>();

        _imageRoadRectTransform = _imageRoad.GetComponent<RectTransform>();
        _imageGimmickRectTransform = _imageGimmick.GetComponent<RectTransform>();
    }

    private void Start()
    {
        RectTransform imageBackGroundRectTransform = _background.GetComponent<RectTransform>();
        imageBackGroundRectTransform.sizeDelta = new Vector2(120, 120);

        _imageRoadRectTransform.sizeDelta = new Vector2(120, 120);

        _imageGimmickRectTransform.sizeDelta = _rectTransform.sizeDelta - new Vector2(10, 10);

        _backgroundOutline.enabled = false;

        if (_isLoad)
        {
            _isLoad = false;
            return;
        }

        _background.enabled = false;
        _imageRoad.enabled = false;
        _imageGimmick.enabled = false;

        _tile.GimmickTileShape = -1;
        _tile.RoadTileShape = -1;
    }

    public void OnClickThisNode()
    {
        EventManager<TileEvent>.TriggerEvent(TileEvent.SelectTileNode, this);
        EventManager<TileEvent>.TriggerEvent(TileEvent.GetTileRotateValue, _tile.RotateRoadValue, _tile.RotateGimmickValue);

        _backgroundOutline.enabled = true;
    }

    public void ChangedTileInfo(Tile tileInfo, Sprite sprite)
    {
        _tile.Type = tileInfo.Type;
        _tile.GimmickType = tileInfo.GimmickType;
        _tile.RotateRoadValue = tileInfo.RotateRoadValue;
        _tile.RotateGimmickValue = tileInfo.RotateGimmickValue;
        _tile.RoadTileShape = tileInfo.RoadTileShape;
        _tile.GimmickTileShape = tileInfo.GimmickTileShape;

        Debug.Log(_tile.RotateRoadValue);

        _background.enabled = true;  

        if (tileInfo.Type != TileType.Gimmick)
        {
            _imageRoad.enabled = true;
            _imageRoad.sprite = sprite;
        }
        else
        {
            _imageGimmick.enabled = true;
            _imageGimmick.sprite = sprite;
        }

        ChangedGimmickTileRotate(_tile.RotateGimmickValue);
        ChangedRoadTileRotate(_tile.RotateRoadValue);
    }

    public void ChangedGimmickTileRotate(int rotateDir)
    {
        _tile.RotateGimmickValue = rotateDir;
        float rotationAngle = (rotateDir) * -90f;

        _imageGimmickRectTransform.rotation = Quaternion.Euler(0, 0, rotationAngle);
    }

    public void ChangedRoadTileRotate(int rotateValue)
    {
        _tile.RotateRoadValue = rotateValue;
        float rotationAngle = (rotateValue) * -90f;

        _imageRoadRectTransform.rotation = Quaternion.Euler(0, 0, rotationAngle);
    }

    public void DisEnableOutLine()
    {
        _backgroundOutline.enabled = false;
    }

    public void DeleteTileTypes(DeleteTileAttributeList deleteType)
    {
        switch (deleteType)
        {
            case DeleteTileAttributeList.Gimmick:
                _tile.GimmickType = GimmickType.None;
                _tile.GimmickTileShape = -1;
                _imageGimmick.sprite = null;
                _imageGimmick.enabled = false;
                break;
            case DeleteTileAttributeList.Road:
                _tile.Type = TileType.None;
                _tile.RoadTileShape = -1;
                _imageRoad.sprite = null;
                _imageRoad.enabled = false;
                break;
            case DeleteTileAttributeList.All:
                DeleteTileTypes(DeleteTileAttributeList.Gimmick);
                DeleteTileTypes(DeleteTileAttributeList.Road);
                break;
            default:
                DebugLogger.Log("DeleteTileTypes Default");
                break;
        }

        if(!_imageRoad.enabled && !_imageGimmick.enabled) 
            _background.enabled = false;
    }

    public void LoadTileInfo(Tile tileInfo, Sprite Roadsprite, Sprite GimmickSprite)
    {
        _isLoad = true;

        _tile = tileInfo;

        if (Roadsprite != null || GimmickSprite != null)
        {
            _background.enabled = true;

            if(Roadsprite != null)
            {
                _imageRoad.enabled = true;
                _imageRoad.sprite = Roadsprite;
                ChangedRoadTileRotate(tileInfo.RotateRoadValue);
            }
            else _imageRoad.enabled = false;

            if (GimmickSprite != null)
            {
                _imageGimmick.enabled = true;
                _imageGimmick.sprite = GimmickSprite;
                ChangedGimmickTileRotate(tileInfo.RotateGimmickValue);
            }else _imageGimmick.enabled = false;
            
        }      
    }
}
