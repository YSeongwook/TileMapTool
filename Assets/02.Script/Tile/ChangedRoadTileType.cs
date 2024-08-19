using EnumTypes;
using EventLibrary;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangedRoadTileType : MonoBehaviour
{
    [SerializeField] private Sprite tileSprite;
    [SerializeField] private TileType tileType;
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
        newTile.Type = tileType;
        newTile.GimmickType = GimmickType.None;
        newTile.RotateValue = 0;
        newTile.RoadTileShape = tileShape;

        EventManager<TileEvent>.TriggerEvent(TileEvent.ChangedSelectTileInfo, newTile, tileSprite);
    }
}
