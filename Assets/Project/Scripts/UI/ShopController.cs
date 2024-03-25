using System.Collections.Generic;
using UnityEngine;

public interface IShopController {
    ref List<ShopItem> GetSellClothes();
    ref List<ShopItem> GetSellWeapons();
    ref List<ShopItem> GetBuyClothes();
    ref List<ShopItem> GetBuyWeapons();
    void SetSellItems();
}

public class ShopController : IShopController {
    private readonly IShopView _view;

    private List<ShopItem> sellClothes;
    private List<ShopItem> sellWeapons;

    public List<ShopItem> buyClothes;
    public List<ShopItem> buyWeapons;


    public ShopController(IShopView view) {
        _view = view;
    }

    public void SetSellItems() {
        sellClothes.Clear();
        sellWeapons.Clear();

        // clear gameobject children
        for (int i = 0; i < _view.GetSellContent().childCount; i++) {
            _view.DestroyGO((_view.GetSellContent().GetChild(i).gameObject));
        }

        _view.SetSkinsToSell();
        _view.SetWeaponsToSell();

        for (int i = 0; i < sellClothes.Count; i++) {
            sellClothes[i].id = i;
        }

        for (int i = 0; i < sellWeapons.Count; i++) {
            sellWeapons[i].id = sellClothes.Count + i;
        }

        _view.SetScrollBarValue(100);
    }

    public void SetBuyItems() {
        buyClothes.Clear();
        buyWeapons.Clear();

        for (int i = 0; i < _view.GetBuyContent().childCount; i++) {
            _view.DestroyGO(_view.GetBuyContent().GetChild(i).gameObject);
        }

        List<Clothes> clothesToAdd = GetClothesToAdd();

        _view.SetClothesToBuy(clothesToAdd);

        List<Weapon> weaponsToAdd = GetWeaponsToAdd();

        _view.SetWeaponsToBuy(weaponsToAdd);

        for (int i = 0; i < buyClothes.Count; i++) {
            buyClothes[i].id = i;
        }

        for (int i = 0; i < buyWeapons.Count; i++) {
            buyWeapons[i].id = buyClothes.Count + i;
        }

        _view.SetScrollBarValue(100);
    }

    public ref List<ShopItem> GetSellClothes() {
        return ref sellClothes;
    }

    public ref List<ShopItem> GetSellWeapons() {
        return ref sellWeapons;
    }

    public ref List<ShopItem> GetBuyClothes() {
        return ref buyClothes;
    }

    public ref List<ShopItem> GetBuyWeapons() {
        return ref buyWeapons;
    }

    private List<Clothes> GetClothesToAdd() {
        List<Clothes> clothesToAdd = new List<Clothes>();

        for (int i = 0; i < _view.GetAvailableClothes().Count; i++) {
            clothesToAdd.Add(_view.GetAvailableClothes()[i]);
        }

        //add items to the sell list
        for (int i = 0; i < GameManager.Instance.GetPlayerClothes().Count; i++) {
            for (int j = 0; j < clothesToAdd.Count; j++) {
                if (GameManager.Instance.GetPlayerClothes()[i] == clothesToAdd[j]) {
                    clothesToAdd.Remove(clothesToAdd[j]);
                }
            }
        }

        return clothesToAdd;
    }

    private List<Weapon> GetWeaponsToAdd() {
        List<Weapon> weaponsToAdd = new List<Weapon>();

        for (int i = 0; i < _view.GetAvailableWeapons().Count; i++) {
            weaponsToAdd.Add(_view.GetAvailableWeapons()[i]);
        }

        //add items to the sell list
        for (int i = 0; i < GameManager.Instance.GetPlayerWeapons().Count; i++) {
            for (int j = 0; j < weaponsToAdd.Count; j++) {
                if (GameManager.Instance.GetPlayerWeapons()[i] == weaponsToAdd[j]) {
                    weaponsToAdd.Remove(weaponsToAdd[j]);
                }
            }
        }

        return weaponsToAdd;
    }
}