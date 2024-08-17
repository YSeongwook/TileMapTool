using EnumTypes;
using EventLibrary;
using System.Collections;
using System.Collections.Generic;
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
        Tile newTile = new Tile();
        newTile.Sprite = tileSprite;
        newTile.Type = TileType.Gimmick;
        newTile.GimmickType = gimmickType;
        newTile.TileShape = tileShape;

        EventManager<TileEvent>.TriggerEvent(TileEvent.ChangedSelectTileInfo, newTile);
    }
}
