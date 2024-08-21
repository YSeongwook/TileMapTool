using EnumTypes;
using EventLibrary;
using UnityEngine;
using UnityEngine.UI;

public class ChangedRoadTileType : MonoBehaviour
{
    [SerializeField] private Sprite tileSprite;
    [SerializeField] private RoadShape roadShape; // 타일의 모양을 정의

    private Image _tileImage;

    private void Awake()
    {
        _tileImage = transform.GetChild(0).GetChild(0).GetComponent<Image>();
        _tileImage.sprite = tileSprite;
    }

    public void OnClickChangedTileInfo()
    {
        TileNode selectedTileNode = PuzzleMapData.Instance._selectTile;

        if (selectedTileNode == null) return;

        Tile currentTileInfo = selectedTileNode.GetTileInfo;
        Tile newTile = currentTileInfo; // 기존 타일 정보 복사

        newTile.Type = TileType.Road; // 길 타일 설정
        newTile.RoadShape = roadShape; // 길의 모양 설정

        // 기존에 기믹 타일이 설정되어 있는지 확인하여, 그 값을 유지하도록 함
        Sprite existingGimmickSprite = currentTileInfo.GimmickShape != GimmickShape.None ? selectedTileNode.GetGimmickSprite() : null;

        EventManager<TileEvent>.TriggerEvent<Tile, Sprite, Sprite>(
            TileEvent.ChangedSelectTileInfo, newTile, tileSprite, existingGimmickSprite);
    }
}