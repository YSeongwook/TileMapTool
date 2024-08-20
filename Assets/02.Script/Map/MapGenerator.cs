using EnumTypes;
using EventLibrary;
using System;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : Singleton<MapGenerator>
{
    public GameObject puzzleMapGrid;
    [SerializeField] private GameObject puzzlePrefab;

    [SerializeField] private List<Sprite> Road;
    [SerializeField] private List<Sprite> Gimmick;

    private const int Puzzle3X3 = 9;
    private const int Puzzle4X4 = 16;
    private const int Puzzle5X5 = 25;
    private const int Puzzle6X6 = 36;
    private const int Puzzle7X7 = 49;

    private RectTransform _rectTransform;
    private bool _isMapCreated; // 맵 생성 여부를 추적하는 플래그 변수

    public int PreviousSize { get; private set; } = 0; // 초기값을 0으로 설정하여 맵이 없는 상태를 표시

    protected override void Awake()
    {
        base.Awake();
        
        _rectTransform = puzzleMapGrid.GetComponent<RectTransform>();
        EventManager<TileEvent>.StartListening<List<Tile>>(TileEvent.JsonLoadData, SetTileList);
        EventManager<DeleteTileAttributeList>.StartListening<int>(DeleteTileAttributeList.All, OnDeleteAllTiles);
    }

    private void OnDestroy()
    {
        EventManager<TileEvent>.StopListening<List<Tile>>(TileEvent.JsonLoadData, SetTileList);
        EventManager<DeleteTileAttributeList>.StopListening<int>(DeleteTileAttributeList.All, OnDeleteAllTiles);
    }

    public void OnCreate3X3Puzzle()
    {
        DestroyAllChildren();
        CreatePuzzle(Puzzle3X3);
    }

    public void OnCreate4X4Puzzle()
    {
        DestroyAllChildren();
        CreatePuzzle(Puzzle4X4);
    }

    public void OnCreate5X5Puzzle()
    {
        DestroyAllChildren();
        CreatePuzzle(Puzzle5X5);
    }

    public void OnCreate6X6Puzzle()
    {
        DestroyAllChildren();
        CreatePuzzle(Puzzle6X6);
    }

    public void OnCreate7X7Puzzle()
    {
        DestroyAllChildren();
        CreatePuzzle(Puzzle7X7);
    }

    private void DestroyAllChildren()
    {
        if (!_isMapCreated)
        {
            // 최초 맵 생성 전에 로그를 출력하지 않음
            return;
        }

        int childCount = puzzleMapGrid.transform.childCount;

        if (childCount == 0)
        {
            DebugLogger.Log("삭제할 맵이 없습니다.");
            return; // 삭제할 것이 없으면 메서드 종료
        }

        for (int i = childCount - 1; i >= 0; i--)
        {
            Transform child = puzzleMapGrid.transform.GetChild(i);
            Destroy(child.gameObject);
        }

        PreviousSize = 0; // 맵이 삭제되었으므로 PreviousSize를 0으로 초기화
        _isMapCreated = false; // 맵이 삭제되었으므로 플래그를 false로 설정
    }

    private void CreatePuzzle(int puzzleSize)
    {
        LayoutRectTransformChanged(puzzleSize);

        for (int i = 0; i < puzzleSize; i++)
        {
            GameObject puzzlePiece = Instantiate(puzzlePrefab, puzzleMapGrid.transform.position, puzzleMapGrid.transform.rotation);
            puzzlePiece.transform.SetParent(puzzleMapGrid.transform);
        }

        PreviousSize = puzzleSize; // 새로운 맵이 생성되었으므로 PreviousSize를 업데이트
        _isMapCreated = true; // 맵이 생성되었으므로 플래그를 true로 설정
    }

    private void OnDeleteAllTiles(int puzzleSize)
    {
        DestroyAllChildren(); // 모든 타일을 삭제하고 로그를 남김
        CreatePuzzle(puzzleSize);
        DebugLogger.Log("모든 타일 삭제");
    }

    private void LayoutRectTransformChanged(int puzzleSize)
    {
        Vector2 screenPos = Vector2.zero;
        switch (puzzleSize)
        {
            case 9:
                screenPos = new Vector2(340, -760);
                break;
            case 16:
                screenPos = new Vector2(280, -820);
                break;
            case 25:
                screenPos = new Vector2(220, -880);
                break;
            case 36:
                screenPos = new Vector2(160, -940);
                break;
            case 49:
                screenPos = new Vector2(100, -1000);
                break;
        }

        _rectTransform.anchoredPosition = screenPos;
    }

    public List<Tile> GetTileList()
    {
        List<Tile> tileList = new List<Tile>();

        foreach (Transform t in puzzleMapGrid.transform)
        {
            var newTileInfo = t.GetComponent<TileNode>();
            if (newTileInfo == null) continue;

            tileList.Add(newTileInfo.GetTileInfo);
        }

        return tileList;
    }

    public void SetTileList(List<Tile> tileList)
    {
        int tileCount = tileList.Count;

        DestroyAllChildren();

        LayoutRectTransformChanged(tileCount);

        try
        {
            for (int i = 0; i < tileCount; i++)
            {
                GameObject puzzlePiece = Instantiate(puzzlePrefab, puzzleMapGrid.transform.position, puzzleMapGrid.transform.rotation);
                puzzlePiece.transform.SetParent(puzzleMapGrid.transform);
                var tile = puzzlePiece.GetComponent<TileNode>();
                Sprite roadSprite = tileList[i].RoadTileShape != -1 ? Road[tileList[i].RoadTileShape] : null;
                Sprite gimmickSprite = tileList[i].GimmickTileShape != -1 ? Gimmick[tileList[i].GimmickTileShape] : null;
                tile.LoadTileInfo(tileList[i], roadSprite, gimmickSprite);
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("에러가 발생했습니다." + ex.Message);
        }
    }
}
