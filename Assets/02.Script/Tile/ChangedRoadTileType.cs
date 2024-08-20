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
        Tile newTile = PuzzleMapData.Instance._selectTile.GetTileInfo;
        newTile.Type = TileType.Road; // 길 타일 설정
        newTile.RoadShape = roadShape;   // 길의 모양 설정
        newTile.GimmickShape = GimmickShape.None; // 기믹은 없음으로 설정
        
        EventManager<TileEvent>.TriggerEvent<Tile, Sprite, Sprite>(
            TileEvent.ChangedSelectTileInfo, newTile, tileSprite, null);
    }
}