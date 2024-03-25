using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public interface IShopView {
    // List<ShopItem> GetSellClothes();
    // List<ShopItem> GetSellWeapons();
    Transform GetSellContent();
    void DestroyGO(GameObject GO);
    void InstantiateGO(GameObject GO, Transform parent);
    void SetSkinsToSell();
    void SetWeaponsToSell();
    void SetScrollBarValue(int value);
    Transform GetBuyContent();
    List<Clothes> GetAvailableClothes();
    List<Weapon> GetAvailableWeapons();
    void SetClothesToBuy(List<Clothes> clothesToAdd);
    void SetWeaponsToBuy(List<Weapon> weaponsToAdd);
}

public class ShopView : MonoBehaviour, IShopView {
    [SerializeField] private Transform buyContent;
    [SerializeField] private Transform sellContent;
    [SerializeField] private GameObject shopClothesPrefab;
    [SerializeField] private GameObject shopWeaponPrefab;

    [SerializeField] private TMP_Text moneyShopAmount;

    [SerializeField] private Scrollbar scrollBar;

    [SerializeField] private List<Weapon> availableWeapons;
    [SerializeField] private List<Clothes> availableClothes;

    [SerializeField] private GameObject sellPanel;
    [SerializeField] private GameObject buyPanel;


    private IShopController _shopController;

    public IShopController ShopController {
        get { return _shopController ??= new ShopController(this); }
    }

    // public List<ShopItem> GetSellClothes() {
    //     return sellClothes;
    // }
    //
    // public List<ShopItem> GetSellWeapons() {
    //     return sellWeapons;
    // }

    public Transform GetSellContent() {
        return sellContent;
    }

    public Transform GetBuyContent() {
        return buyContent;
    }

    public List<Weapon> GetAvailableWeapons() {
        return availableWeapons;
    }

    public List<Clothes> GetAvailableClothes() {
        return availableClothes;
    }

    public void DestroyGO(GameObject GO) {
        Destroy(GO);
    }

    public void InstantiateGO(GameObject GO, Transform parent) {
        Instantiate(GO, parent);
    }

    public void SetSkinsToSell() {
        for (int i = 0; i < UIManager.Instance.skinsAmount; i++) {
            ShopItem sp = Instantiate(shopClothesPrefab, sellContent).GetComponent<ShopItem>();
            Clothes clothes = GameManager.Instance.GetPlayerClothes()[i];
            sp.price.text = clothes.price.ToString() + "$";
            sp.priceNum = clothes.price;
            sp.itemId = clothes.id;
            sp.isWeapon = false;

            sp.weaponImage.sprite = clothes.sprite;

            ShopController.GetSellClothes().Add(sp);
        }
    }

    public void SetWeaponsToSell() {
        for (int i = 0; i < UIManager.Instance.weaponsAmount; i++) {
            ShopItem sp = Instantiate(shopWeaponPrefab, sellContent).GetComponent<ShopItem>();
            Weapon weapon = GameManager.Instance.GetPlayerWeapons()[i];
            sp.damage.text = weapon.damage.ToString();
            sp.force.text = weapon.force.ToString();
            sp.price.text = weapon.price.ToString() + "$";
            sp.priceNum = weapon.price;
            sp.itemId = weapon.id;
            sp.isWeapon = true;

            sp.weaponImage.sprite = weapon.sprite;

            ShopController.GetSellWeapons().Add(sp);
        }
    }

    public void SetClothesToBuy(List<Clothes> clothesToAdd) {
        foreach (Clothes c in clothesToAdd) {
            ShopItem sp = Instantiate(shopClothesPrefab, buyContent).GetComponent<ShopItem>();
            sp.price.text = c.price.ToString() + "$";
            sp.priceNum = c.price;
            sp.itemId = c.id;
            sp.isWeapon = false;

            sp.weaponImage.sprite = c.sprite;
            ShopController.GetBuyClothes().Add(sp);
        }
    }

    public void SetWeaponsToBuy(List<Weapon> weaponsToAdd) {
        foreach (Weapon w in weaponsToAdd) {
            ShopItem sp = Instantiate(shopWeaponPrefab, buyContent).GetComponent<ShopItem>();
            sp.damage.text = w.damage.ToString();
            sp.force.text = w.force.ToString();
            sp.price.text = w.price.ToString() + "$";
            sp.priceNum = w.price;
            sp.itemId = w.id;
            sp.isWeapon = true;

            sp.weaponImage.sprite = w.sprite;

            ShopController.GetBuyWeapons().Add(sp);
        }
    }

    public void SetScrollBarValue(int value) {
        scrollBar.value = value;
    }

    public void UpdateUIValues() {
        moneyShopAmount.text = UIManager.Instance.money.ToString();
    }

    public void OnSellPanel() {
        buyPanel.SetActive(false);
        sellPanel.SetActive(true);
    }

    public void OnBuyPanel() {
        buyPanel.SetActive(true);
        sellPanel.SetActive(false);
    }

    public void OnBuy() {
        ShopController.OnBuy();
    }

    public void OnSell() {
        ShopController.OnSell();
    }
}