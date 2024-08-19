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
    public int RotateValue;
    public int RoadTileShape;
    public int GimmickTileShape;
}

public class TileNode : MonoBehaviour
{
    public int instanID;
    
     private Tile _tile;
    public Tile GetTileInfo {  get { return _tile; } }

    private Image _background;
    private Image _imageRoad;
    private Image _imageGimmick;
    private RectTransform _rectTransform;

    private RectTransform _imageRoadRectTransform;
    private RectTransform _imageGimmickRectTransform;

    private Outline _backgroundOutline;

    private bool isLoad;

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

        _imageRoadRectTransform = _imageRoad.GetComponent<RectTransform>();
        _imageRoadRectTransform.sizeDelta = new Vector2(120, 120);

        _imageGimmickRectTransform = _imageGimmick.GetComponent<RectTransform>();
        _imageGimmickRectTransform.sizeDelta = _rectTransform.sizeDelta - new Vector2(10, 10);

        _backgroundOutline.enabled = false;

        if (isLoad)
        {
            isLoad = false;
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
        EventManager<TileEvent>.TriggerEvent(TileEvent.GetTileRotateValue, _tile.RotateValue);

        _backgroundOutline.enabled = true;
    }

    public void ChangedTileInfo(Tile tileInfo, Sprite sprite)
    {
        _tile.Type = tileInfo.Type;
        _tile.GimmickType = tileInfo.GimmickType;
        _tile.RotateValue = tileInfo.RotateValue;
        _tile.RoadTileShape = tileInfo.RoadTileShape;

        Debug.Log(_tile.RotateValue);

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
    }

    public void ChangedGimmickTileRotate(int rotateDir)
    {
        float rotationAngle = (rotateDir) * -90f;

        _imageGimmickRectTransform.rotation = Quaternion.Euler(0, 0, rotationAngle);
    }

    public void ChangedTileRotate(int rotateValue)
    {
        _tile.RotateValue = rotateValue;

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
        isLoad = true;

        _tile = tileInfo;

        if (Roadsprite != null || GimmickSprite != null)
        {
            _background.enabled = true;

            if(Roadsprite != null)
            {
                _imageRoad.enabled = true;
                _imageRoad.sprite = Roadsprite;
            }else _imageRoad.enabled = false;

            if (GimmickSprite != null)
            {
                _imageGimmick.enabled = true;
                _imageGimmick.sprite = GimmickSprite;
            }else _imageGimmick.enabled = false;
            
        }      
    }
}
