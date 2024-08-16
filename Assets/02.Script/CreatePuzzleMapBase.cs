using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreatePuzzleMapBase : MonoBehaviour
{
    [SerializeField] private GameObject puzzlePrefab;

    private const int Puzzle3X3 = 9;
    private const int Puzzle4X4 = 16;
    private const int Puzzle5X5 = 25;
    private const int Puzzle6X6 = 36;
    private const int Puzzle7X7 = 49;

    private RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
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
        int childCount = transform.childCount;

        if (childCount > 0)
        {
            for (int i = childCount - 1; i >= 0; i--)
            {
                Transform child = transform.GetChild(i);
                Destroy(child.gameObject);
            }
        }
    }

    private void CreatePuzzle(int puzzleSize)
    {
        LayoutRectTransformChanged(puzzleSize);

        for (int i = 0; i < puzzleSize; i++)
        {
            GameObject puzzlePiece = Instantiate(puzzlePrefab, transform.position, transform.rotation);
            puzzlePiece.transform.SetParent(gameObject.transform);
        }
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

        rectTransform.anchoredPosition = ScreenPos;
    }
}
