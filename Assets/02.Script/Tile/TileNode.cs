using System;
using EnumTypes;
using EventLibrary;
using UnityEngine;
using UnityEngine.UI;

// 빈 타일, 길 타일, 기믹 타일(길을 항상 포함)
public enum TileType
{
    None,
    Road,
    Gimmick
}

// 타일 모양(빈 타일의 경우 None, 순서대로 선, L자, T자, 십자, 출발점, 종료점)
public enum RoadShape
{
    None,
    Straight,
    L,
    T,
    Cross,
    Start,
    End
}

// 기믹 모양(기믹 없는 타일의 경우 None
public enum GimmickShape
{
    None,
    Gimmick1,
    Gimmick2
}

public struct Tile
{
    public TileType Type; // 빈 타일, 길 타일, 기믹 타일
    public RoadShape RoadShape;
    public GimmickShape GimmickShape;
    public int RotateValue;
}

public class TileNode : MonoBehaviour
{
    private Tile _tile;
    public Tile GetTileInfo { get { return _tile; } }

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
    }

    // 길 타일의 스프라이트를 반환하는 메서드
    public Sprite GetRoadSprite()
    {
        return _imageRoad.sprite;
    }

    public Sprite GetGimmickSprite()
    {
        return _imageGimmick.sprite;
    }

    public void OnClickThisNode()
    {
        EventManager<TileEvent>.TriggerEvent(TileEvent.SelectTileNode, this);
        EventManager<TileEvent>.TriggerEvent(TileEvent.GetTileRotateValue, _tile.RotateValue);

        _backgroundOutline.enabled = true;
    }

    public void ChangedTileInfo(Tile tileInfo, Sprite roadSprite, Sprite gimmickSprite)
    {
        _tile = tileInfo;

        Debug.Log($"타일 정보 - 타입: {_tile.Type}, 모양: {_tile.RoadShape}, 기믹: {_tile.GimmickShape}, 회전: {_tile.RotateValue}");

        _background.enabled = true;

        // 길 이미지는 항상 로드 또는 기믹 타일에서 표시됨
        if (tileInfo.Type == TileType.Road || tileInfo.Type == TileType.Gimmick)
        {
            DebugLogger.Log("길 타일 배치");
            _imageRoad.enabled = true;
            _imageRoad.sprite = roadSprite;
            ChangedRoadTileRotate(_tile.RotateValue);
        }

        // 기믹 타일인 경우 기믹 이미지를 추가로 표시
        if (tileInfo.Type == TileType.Gimmick)
        {
            _imageGimmick.enabled = true;
            _imageGimmick.sprite = gimmickSprite;
        }
    }

    public void ChangedRoadTileRotate(int rotateValue)
    {
        _tile.RotateValue = rotateValue;
        float rotationAngle = rotateValue * -90f;

        // 길과 기믹 모두 동일한 회전 각도를 적용
        _imageRoadRectTransform.rotation = Quaternion.Euler(0, 0, rotationAngle);
        _imageGimmickRectTransform.rotation = Quaternion.Euler(0, 0, rotationAngle);
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
                _tile.Type = TileType.Road;
                _tile.GimmickShape = GimmickShape.None;
                _imageGimmick.sprite = null;
                _imageGimmick.enabled = false;
                break;
            case DeleteTileAttributeList.Road:
                _tile.Type = TileType.None;
                _tile.RoadShape = RoadShape.None;
                _imageRoad.sprite = null;
                _imageRoad.enabled = false;
                _imageGimmick.sprite = null;
                _imageGimmick.enabled = false;
                break;
            case DeleteTileAttributeList.All:
                DeleteTileTypes(DeleteTileAttributeList.Gimmick);
                DeleteTileTypes(DeleteTileAttributeList.Road);
                break;
            default:
                DebugLogger.Log("DeleteTileTypes Default");
                break;
        }

        if (!_imageRoad.enabled && !_imageGimmick.enabled)
            _background.enabled = false;
    }

    public void LoadTileInfo(Tile tileInfo, Sprite roadSprite, Sprite gimmickSprite)
    {
        _isLoad = true;
        _tile = tileInfo;

        if (roadSprite != null || gimmickSprite != null)
        {
            _background.enabled = true;

            // 길 이미지는 항상 로드 또는 기믹 타일에서 표시됨
            if (roadSprite != null && (tileInfo.Type == TileType.Road || tileInfo.Type == TileType.Gimmick))
            {
                _imageRoad.enabled = true;
                _imageRoad.sprite = roadSprite;
                ChangedRoadTileRotate(tileInfo.RotateValue);
            }
            else
            {
                _imageRoad.enabled = false;
            }

            // 기믹 타일인 경우 기믹 이미지를 추가로 표시
            if (gimmickSprite != null && tileInfo.Type == TileType.Gimmick)
            {
                _imageGimmick.enabled = true;
                _imageGimmick.sprite = gimmickSprite;
            }
            else
            {
                _imageGimmick.enabled = false;
            }
        }
    }
}
