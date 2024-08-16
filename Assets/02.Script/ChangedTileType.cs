using EnumTypes;
using EventLibrary;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangedTileType : MonoBehaviour
{
    [SerializeField] private Sprite tileSprite;
    [SerializeField] private TileType tileType;
    [SerializeField] private int tileShape;

    private Image tileImage;

    private void Awake()
    {
        tileImage = transform.GetChild(0).GetChild(0).GetComponent<Image>();
        tileImage.sprite = tileSprite;
    }

    public void OnCickChangedTileInfo()
    {
        Tile newTile = new Tile();
        newTile.sprite = tileSprite;
        newTile.type = tileType;
        newTile.tileShape = tileShape;

        EventManager<TileEvent>.TriggerEvent(TileEvent.ChangedSelectTileNodeInfo, newTile);
    }
}
