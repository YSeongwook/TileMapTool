using EnumTypes;
using EventLibrary;
using UnityEngine;
using UnityEngine.UI;

public class ChangedGimmickTile : MonoBehaviour
{
    [SerializeField] private Sprite tileSprite;
    [SerializeField] private GimmickShape gimmickShape;   

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

        // 현재 선택된 타일의 정보를 가져옴
        Tile currentTileInfo = selectedTileNode.GetTileInfo;

        // 길 타일이 배치되지 않은 경우, 기믹 타일을 배치할 수 없도록 함
        if (currentTileInfo.RoadShape == RoadShape.None)
        {
            DebugLogger.LogError("길 타일이 배치되지 않았습니다. 기믹 타일은 길 타일이 있어야 배치할 수 있습니다.");
            return;
        }

        // 길 타일이 있는 경우에만 기믹 타일을 배치
        Tile newTile = currentTileInfo; // 기존 타일 정보 복사
        newTile.Type = TileType.Gimmick; // 기믹 타일 설정
        newTile.GimmickShape = gimmickShape; // 기믹의 모양 설정

        // 기믹 스프라이트를 세 번째 자식 오브젝트에 적용
        EventManager<TileEvent>.TriggerEvent<Tile, Sprite, Sprite>(
            TileEvent.ChangedSelectTileInfo, newTile, selectedTileNode.GetRoadSprite(), tileSprite);
    }
}