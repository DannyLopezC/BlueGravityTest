using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public interface IShopController {
    ref List<ShopItem> GetSellClothes();
    ref List<ShopItem> GetSellWeapons();
    ref List<ShopItem> GetBuyClothes();
    ref List<ShopItem> GetBuyWeapons();
    void SetSellItems();
    void SetBuyItems();
    void DeselectAllSellItems(ShopItem origin);
    void OnSell();
    void OnBuy();
}

public class ShopController : IShopController {
    private readonly IShopView _view;

    private List<ShopItem> _sellClothes;
    private List<ShopItem> _sellWeapons;

    private List<ShopItem> _buyClothes;
    private List<ShopItem> _buyWeapons;

    private ShopItem _currentShopItem;


    public ShopController(IShopView view) {
        _view = view;
    }

    public void SetSellItems() {
        _sellClothes.Clear();
        _sellWeapons.Clear();

        // clear gameobject children
        for (int i = 0; i < _view.GetSellContent().childCount; i++) {
            _view.DestroyGO((_view.GetSellContent().GetChild(i).gameObject));
        }

        _view.SetSkinsToSell();
        _view.SetWeaponsToSell();

        for (int i = 0; i < _sellClothes.Count; i++) {
            _sellClothes[i].id = i;
        }

        for (int i = 0; i < _sellWeapons.Count; i++) {
            _sellWeapons[i].id = _sellClothes.Count + i;
        }

        _view.SetScrollBarValue(100);
    }

    public void SetBuyItems() {
        _buyClothes.Clear();
        _buyWeapons.Clear();

        for (int i = 0; i < _view.GetBuyContent().childCount; i++) {
            _view.DestroyGO(_view.GetBuyContent().GetChild(i).gameObject);
        }

        List<Clothes> clothesToAdd = GetClothesToAdd();

        _view.SetClothesToBuy(clothesToAdd);

        List<Weapon> weaponsToAdd = GetWeaponsToAdd();

        _view.SetWeaponsToBuy(weaponsToAdd);

        for (int i = 0; i < _buyClothes.Count; i++) {
            _buyClothes[i].id = i;
        }

        for (int i = 0; i < _buyWeapons.Count; i++) {
            _buyWeapons[i].id = _buyClothes.Count + i;
        }

        _view.SetScrollBarValue(100);
    }

    public ref List<ShopItem> GetSellClothes() {
        return ref _sellClothes;
    }

    public ref List<ShopItem> GetSellWeapons() {
        return ref _sellWeapons;
    }

    public ref List<ShopItem> GetBuyClothes() {
        return ref _buyClothes;
    }

    public ref List<ShopItem> GetBuyWeapons() {
        return ref _buyWeapons;
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

    public void DeselectAllSellItems(ShopItem origin) {
        _currentShopItem = origin;

        //de activate sell
        foreach (ShopItem sp in _sellClothes.Where(sp => sp.id != origin.id)) {
            sp.Selected = false;
        }

        foreach (ShopItem sp in _sellWeapons.Where(sp => sp.id != origin.id)) {
            sp.Selected = false;
        }

        //de activate buy
        foreach (ShopItem t in _buyClothes.Where(t => t.id != origin.id)) {
            t.Selected = false;
        }

        foreach (ShopItem t in _buyWeapons.Where(t => t.id != origin.id)) {
            t.Selected = false;
        }
    }

    public void OnSell() {
        if (_currentShopItem == null) return;

        if (_currentShopItem.isWeapon) {
            if (GameManager.Instance.GetPlayerWeapons().Count <= 1) {
                GameManager.Instance.ShowText($"You have only one item of this type", 50, Color.yellow,
                    GameManager.Instance.GetPlayerTransform().position,
                    Vector3.up * Random.Range(30, 50), 2f);
                return;
            }

            Weapon w = GameManager.Instance.GetPlayerWeapons().Find(w => w.id == _currentShopItem.itemId);
            if (w != null) GameManager.Instance.GetPlayerWeapons().Remove(w);

            if (GameManager.Instance.GetPlayerWeapons().Count == 1) GameManager.Instance.ChangeWeaponV2(0);
        }
        else {
            if (GameManager.Instance.GetPlayerClothes().Count <= 1) {
                GameManager.Instance.ShowText($"You have only one item of this type", 50, Color.yellow,
                    GameManager.Instance.GetPlayerTransform().position,
                    Vector3.up * Random.Range(30, 50), 2f);
                return;
            }

            Clothes c = GameManager.Instance.GetPlayerClothes().Find(c => c.id == _currentShopItem.itemId);
            if (c != null) GameManager.Instance.GetPlayerClothes().Remove(c);

            if (GameManager.Instance.GetPlayerClothes().Count == 1) GameManager.Instance.ChangeClothing(0);
        }

        SetSellItems();
        SetBuyItems();

        GameManager.Instance.AddMoney(_currentShopItem.priceNum);
        GameManager.Instance.ShowText($"+{_currentShopItem.priceNum} gold", 50, Color.yellow,
            GameManager.Instance.GetPlayerTransform().position,
            Vector3.up * Random.Range(30, 50), 2f);
    }

    public void OnBuy() {
        if (_currentShopItem == null) return;
        if (UIManager.Instance.money < _currentShopItem.priceNum) {
            GameManager.Instance.ShowText("You have no gold", 50, Color.yellow,
                GameManager.Instance.GetPlayerTransform().position,
                Vector3.up * Random.Range(30, 50), 2f);
            return;
        }

        if (_currentShopItem.isWeapon) {
            Weapon weapon = _view.GetAvailableWeapons().Find(w => w.id == _currentShopItem.itemId);
            if (weapon != null) GameManager.Instance.GetPlayerWeapons().Add(weapon);
        }
        else {
            Clothes c = _view.GetAvailableClothes().Find(c => c.id == _currentShopItem.itemId);
            if (c != null) GameManager.Instance.GetPlayerClothes().Add(c);
        }

        SetSellItems();
        SetBuyItems();

        GameManager.Instance.AddMoney(-_currentShopItem.priceNum);
        GameManager.Instance.ShowText($"-{_currentShopItem.priceNum} gold", 50, Color.yellow,
            GameManager.Instance.GetPlayerTransform().position,
            Vector3.up * Random.Range(30, 50), 2f);
    }
}