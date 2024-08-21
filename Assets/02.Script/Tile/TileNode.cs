using System.Collections;
using System.Collections.Generic;
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
    
    [SerializeField] private float rotationDuration = 1f; // 회전에 걸리는 시간
    private readonly Queue<int> _rotationQueue = new Queue<int>();
    private int _currentRotation;
    private bool _isRotating;

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

    public void EnableGimmickImage(bool enable)
    {
        _imageGimmick.enabled = enable;
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
        // 회전 값을 타일 구조체에 업데이트
        _tile.RotateValue = rotateValue;

        // 큐에 회전 명령 추가
        _rotationQueue.Enqueue(rotateValue);

        // 현재 회전 중이 아니면 큐의 첫 번째 명령을 실행
        if (!_isRotating)
        {
            StartCoroutine(ProcessRotationQueue());
        }
    }

    private IEnumerator ProcessRotationQueue()
    {
        while (_rotationQueue.Count > 0)
        {
            _isRotating = true;
            _currentRotation = _rotationQueue.Dequeue();
            float rotationAngle = _currentRotation * -90f;
            Quaternion targetRotation = Quaternion.Euler(0, 0, rotationAngle);

            yield return StartCoroutine(RotateOverTime(targetRotation, rotationDuration));
        }

        _isRotating = false;
    }

    private IEnumerator RotateOverTime(Quaternion endRotation, float duration)
    {
        Quaternion startRotation = _imageRoadRectTransform.rotation;
        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            _imageRoadRectTransform.rotation = Quaternion.Lerp(startRotation, endRotation, elapsed / duration);
            _imageGimmickRectTransform.rotation = Quaternion.Lerp(startRotation, endRotation, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        _imageRoadRectTransform.rotation = endRotation;
        _imageGimmickRectTransform.rotation = endRotation;
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

        // 로드 타일, 기믹 타일 전부 비활성화 시 배경 타일 이미지도 비활성화
        // if (!_imageRoad.enabled && !_imageGimmick.enabled) _background.enabled = false;
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
