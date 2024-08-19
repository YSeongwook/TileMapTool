using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public GameObject puzzleMapGrid;
    [SerializeField] private GameObject puzzlePrefab;

    private const int Puzzle3X3 = 9;
    private const int Puzzle4X4 = 16;
    private const int Puzzle5X5 = 25;
    private const int Puzzle6X6 = 36;
    private const int Puzzle7X7 = 49;

    private RectTransform _rectTransform;

    private void Awake()
    {
        _rectTransform = puzzleMapGrid.GetComponent<RectTransform>();
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
        int childCount = puzzleMapGrid.transform.childCount;

        if (childCount > 0)
        {
            for (int i = childCount - 1; i >= 0; i--)
            {
                Transform child = puzzleMapGrid.transform.GetChild(i);
                Destroy(child.gameObject);
            }
        }
    }

    private void CreatePuzzle(int puzzleSize)
    {
        LayoutRectTransformChanged(puzzleSize);

        for (int i = 0; i < puzzleSize; i++)
        {
            GameObject puzzlePiece = Instantiate(puzzlePrefab, puzzleMapGrid.transform.position, puzzleMapGrid.transform.rotation);
            puzzlePiece.transform.SetParent(puzzleMapGrid.transform);
        }
        
        // Todo: 퍼즐 
    }

    private void LayoutRectTransformChanged(int puzzleSize)
    {
        Vector2 ScreenPos = Vector2.zero;
        switch (puzzleSize)
        {
            case 9:
                ScreenPos = new Vector2(340, -760);
                break;
            case 16:
                ScreenPos = new Vector2(280, -820);
                break;
            case 25:
                ScreenPos = new Vector2(220, -880);
                break;
            case 36:
                ScreenPos = new Vector2(160, -940);
                break;
            case 49:
                ScreenPos = new Vector2(100, -1000);
                break;
        }

        _rectTransform.anchoredPosition = ScreenPos;
    }
}
