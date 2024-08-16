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
    private Image _imageRoad;
    private Image _imageGimmick;
    private RectTransform _rectTransform;

    private void Awake()
    {
        _imageRoad = transform.GetChild(0).GetComponent<Image>();
        _imageGimmick = transform.GetChild(1).GetComponent<Image>();
        _rectTransform = GetComponent<RectTransform>();

        instanID = GetInstanceID();
    }

    private void Start()
    {
        RectTransform imageRoadRectTransform = _imageRoad.GetComponent<RectTransform>();
        imageRoadRectTransform.sizeDelta = _rectTransform.sizeDelta - new Vector2(20, 20);

        RectTransform imageGimmickRectTransform = _imageGimmick.GetComponent<RectTransform>();
        imageGimmickRectTransform.sizeDelta = _rectTransform.sizeDelta;
    }

    public void OnClickThisNode()
    {
        EventManager<TileEvent>.TriggerEvent(TileEvent.SelectTileNode, this);
    }
}
