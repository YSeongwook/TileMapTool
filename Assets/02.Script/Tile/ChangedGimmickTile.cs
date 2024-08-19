using EnumTypes;
using EventLibrary;
using UnityEngine;
using UnityEngine.UI;

public class ChangedGimmickTile : MonoBehaviour
{
    [SerializeField] private Sprite tileSprite;
    [SerializeField] private GimmickType gimmickType;   
    [SerializeField] private int tileShape;

    private Image _tileImage;

    private void Awake()
    {
        _tileImage = transform.GetChild(0).GetChild(0).GetComponent<Image>();
        _tileImage.sprite = tileSprite;
    }

    public void OnClickChangedTileInfo()
    {
        Tile newTile = PuzzleMapData.Instance._selectTile.GetTileInfo;
        newTile.Type = TileType.Gimmick;
        newTile.GimmickType = gimmickType;
        newTile.GimmickTileShape = tileShape;

        EventManager<TileEvent>.TriggerEvent(TileEvent.ChangedSelectTileInfo, newTile, tileSprite);
    }


}
