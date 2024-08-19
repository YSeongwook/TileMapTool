using UnityEngine;

namespace EnumTypes
{
    public enum Layers
    {
        Default,
        TransparentFX,
        IgnoreRaycast,
        Reserved1,
        Water,
        UI,
        Reserved2,
        Reserved3,
        Player,
        Enemy,
    }

    public enum UIEvents
    {
        OnClickSignInGoogle,
        OnClickStart,
        OnClickItemBuyButton,
        OnClickGoldBuyButton,
        OnClickChangeBuyItemCount,
        OnClickEnableItemBuyPopup,
        OnClickEnableGoldBuyPopup,
        OnClickGoldBuyExit,
        OnCreateItemSlot,
        OnCreateGoldPackageSlot,
        GetPlayerInventoryResources,
        GoldStorePopup,
        GoldStoreExit
    }

    public enum DataEvents
    {
        OnUserInventoryLoad,
        OnUserInventorySave,
        OnItemDataLoad,
        OnPaymentSuccessful,
        MVVMChangedGold,
        MVVMChangedERC,
        MVVMChangedInventoryItemDictionary,
        PlayerGoldChanged,
        PlayerERCChanged,
        PlayerItemListChanged
    }

    public enum GoldEvent
    {
        OnGetGold,
        OnUseERC,
        OnGetERC
    }

    public enum TileEvent
    {
        SelectTileNode,
        ChangedSelectTileInfo,
        RotationSelectTileNodeInfo,
        GetTileRotateValue,
        DeleteTIleAttribute
    }
    
    public enum DeleteTileAttributeList 
    {
        Gimmick,
        Road,
        All
    }
    
    public class EnumTypes : MonoBehaviour
    {
    }
}