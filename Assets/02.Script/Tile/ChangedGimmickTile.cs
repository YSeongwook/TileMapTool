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
        Tile newTile = PuzzleMapData.Instance._selectTile.GetTileInfo;
        newTile.Type = TileType.Gimmick; // 기믹 타일 설정
        newTile.GimmickShape = gimmickShape; // 기믹의 모양 설정

        EventManager<TileEvent>.TriggerEvent<Tile, Sprite, Sprite>(
            TileEvent.ChangedSelectTileInfo, newTile, tileSprite, null);
    }
}