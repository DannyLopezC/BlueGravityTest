using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine.SceneManagement;

public class OldUIManager : MonoBehaviour {
    #region variables

    [BoxGroup("HUD")] public Button inventoryButton;
    [BoxGroup("HUD")] public Image lifeBar;
    [BoxGroup("HUD")] public TMP_Text hudMoney;

    [BoxGroup("Menu")] public GameObject menuGo;
    [BoxGroup("Menu")] public bool menuOpened = false;
    [BoxGroup("Menu")] public bool menuChanging = false;
    [BoxGroup("Menu")] public Animator menuAnimator;
    [BoxGroup("Menu")] public Image weaponSprite;
    [BoxGroup("Menu")] public Image skinSprite;
    [BoxGroup("Menu")] public TMP_Text moneyAmount;
    [BoxGroup("Menu")] public TMP_Text skinsAmount;
    [BoxGroup("Menu")] public TMP_Text weaponsAmount;
    [BoxGroup("Menu")] public Image menuLifeBar;
    [BoxGroup("Menu")] public TMP_Text damageAmount;
    [BoxGroup("Menu")] public TMP_Text forceAmount;

    [BoxGroup("Shop")] public GameObject shopWeaponPrefab;
    [BoxGroup("Shop")] public GameObject shopClothesPrefab;
    [BoxGroup("Shop")] public ShopItem currentShopItem;
    [BoxGroup("Shop")] public TMP_Text moneyShopAmount;
    [BoxGroup("Shop")] public Animator shopAnimator;
    [BoxGroup("Shop")] public bool shopOpened = false;
    [BoxGroup("Shop")] public bool shopChanging = false;
    [BoxGroup("Shop")] public GameObject shopGo;

    [InlineEditor, BoxGroup("Shop/AvailableItems")]
    public List<Weapon> availableWeapons;

    [InlineEditor, BoxGroup("Shop/AvailableItems")]
    public List<Clothes> availableClothes;

    [InlineEditor, BoxGroup("Shop/Selling")]
    public List<ShopItem> sellClothes;

    [InlineEditor, BoxGroup("Shop/Selling")]
    public List<ShopItem> sellWeapons;

    [BoxGroup("Shop/Selling")] public Transform sellContent;
    [BoxGroup("Shop/Selling")] public GameObject sellPanel;

    [InlineEditor, BoxGroup("Shop/Buying")]
    public List<ShopItem> buyClothes;

    [InlineEditor, BoxGroup("Shop/Buying")]
    public List<ShopItem> buyWeapons;

    [BoxGroup("Shop/Buying")] public Transform buyContent;
    [BoxGroup("Shop/Buying")] public GameObject buyPanel;

    public Scrollbar scrollBar;

    private AttackView _playerAttackViewComponent;
    private GameManager gm;

    #endregion

    private void Start() {
        gm = GameManager.instance;
        _playerAttackViewComponent = gm.player.GetComponentInChildren<AttackView>();
        weaponSprite.sprite = _playerAttackViewComponent.weapons[_playerAttackViewComponent.Equipped].sprite;
        UpdateUIValues();

        SetSellItems();
        SetBuyItems();
    }

    private void Update() {
        if (!shopOpened && !gm.inDialogue && !gm.inShop)
            if (Input.GetKeyDown(KeyCode.Escape))
                OnInventory(!menuOpened);
        if (shopOpened && gm.inShop)
            if (Input.GetKeyDown(KeyCode.Escape))
                OnShop(false);

        UpdateUIValues();
    }

    #region buttons

    public void OnNextWeapon() {
        Weapon nextWeapon = _playerAttackViewComponent.weapons[_playerAttackViewComponent.Equipped];
        int nextId = 0;

        for (int i = 0; i < _playerAttackViewComponent.weapons.Count; i++) {
            if (_playerAttackViewComponent.currentWeapon.id == _playerAttackViewComponent.weapons[i].id)
                nextId = i + 1;
        }

        //going back to first if is the last
        if (nextId > _playerAttackViewComponent.weapons.Count - 1) nextId = 0;

        nextWeapon = _playerAttackViewComponent.weapons[nextId];

        //setting sprites
        weaponSprite.sprite = nextWeapon.sprite;
        _playerAttackViewComponent.ChangeWeaponV2(nextId);
    }

    public void OnBackWeapon() {
        Weapon backWeapon = _playerAttackViewComponent.weapons[_playerAttackViewComponent.Equipped];
        int nextId = 0;

        for (int i = 0; i < _playerAttackViewComponent.weapons.Count; i++) {
            if (_playerAttackViewComponent.currentWeapon.id == _playerAttackViewComponent.weapons[i].id)
                nextId = i - 1;
        }

        //going to last if is the first
        if (nextId < 0) nextId = _playerAttackViewComponent.weapons.Count - 1;

        backWeapon = _playerAttackViewComponent.weapons[nextId];

        //setting sprites
        weaponSprite.sprite = backWeapon.sprite;
        _playerAttackViewComponent.ChangeWeaponV2(nextId);
    }

    public void OnNextSkin() {
        Clothes nextClothes = gm.player.clothes[gm.player.ClothesId];
        int nextId = 0;

        for (int i = 0; i < gm.player.clothes.Count; i++) {
            if (gm.player.currentClothes.id == gm.player.clothes[i].id)
                nextId = i + 1;
        }

        //going back to first if is the last
        if (nextId > gm.player.clothes.Count - 1) nextId = 0;

        nextClothes = gm.player.clothes[nextId];

        //setting sprites
        skinSprite.sprite = nextClothes.sprite;
        gm.player.ChangeClothingV2(nextId);
    }

    public void OnBackSkin() {
        Clothes backClothes = gm.player.clothes[gm.player.ClothesId];
        int nextId = 0;

        for (int i = 0; i < gm.player.clothes.Count; i++) {
            if (gm.player.currentClothes.id == gm.player.clothes[i].id)
                nextId = i - 1;
        }

        //going to last if is the first
        if (nextId < 0) nextId = gm.player.clothes.Count - 1;

        backClothes = gm.player.clothes[nextId];

        //setting sprites
        skinSprite.sprite = backClothes.sprite;
        gm.player.ChangeClothingV2(nextId);
    }

    #endregion

    public void UpdateUIValues() {
        moneyAmount.text = gm.player.money.ToString();
        moneyShopAmount.text = gm.player.money.ToString();
        hudMoney.text = gm.player.money.ToString();

        skinsAmount.text = gm.player.clothes.Count.ToString();
        weaponsAmount.text = _playerAttackViewComponent.weapons.Count.ToString();

        damageAmount.text = _playerAttackViewComponent.currentWeapon.damage.ToString();
        forceAmount.text = _playerAttackViewComponent.currentWeapon.force.ToString();

        menuLifeBar.fillAmount = gm.player.life / gm.player.maxLife;
        lifeBar.fillAmount = gm.player.life / gm.player.maxLife;
    }

    public void OnReloadScene() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    public void OnExit() => Application.Quit();

    #region shop

    public void SetSellItems() {
        sellClothes.Clear();
        sellWeapons.Clear();

        for (int i = 0; i < sellContent.childCount; i++) {
            Destroy(sellContent.GetChild(i).gameObject);
        }

        //set skins to sell
        for (int i = 0; i < gm.player.clothes.Count; i++) {
            ShopItem sp = Instantiate(shopClothesPrefab, sellContent).GetComponent<ShopItem>();
            sp.price.text = gm.player.clothes[i].price.ToString() + "$";
            sp.priceNum = gm.player.clothes[i].price;
            sp.itemId = gm.player.clothes[i].id;
            sp.isWeapon = false;

            sp.weaponImage.sprite = gm.player.clothes[i].sprite;

            sellClothes.Add(sp);
        }

        //set weapons to sell
        for (int i = 0; i < _playerAttackViewComponent.weapons.Count; i++) {
            ShopItem sp = Instantiate(shopWeaponPrefab, sellContent).GetComponent<ShopItem>();
            sp.damage.text = _playerAttackViewComponent.weapons[i].damage.ToString();
            sp.force.text = _playerAttackViewComponent.weapons[i].force.ToString();
            sp.price.text = _playerAttackViewComponent.weapons[i].price.ToString() + "$";
            sp.priceNum = _playerAttackViewComponent.weapons[i].price;
            sp.itemId = _playerAttackViewComponent.weapons[i].id;
            sp.isWeapon = true;

            sp.weaponImage.sprite = _playerAttackViewComponent.weapons[i].sprite;

            sellWeapons.Add(sp);
        }

        for (int i = 0; i < sellClothes.Count; i++) {
            sellClothes[i].id = i;
        }

        for (int i = 0; i < sellWeapons.Count; i++) {
            sellWeapons[i].id = sellClothes.Count + i;
        }

        scrollBar.value = 100;
    }

    public void DeselectAllSellItems(ShopItem origin) {
        currentShopItem = origin;

        //de activate sell
        for (int i = 0; i < sellClothes.Count; i++) {
            if (sellClothes[i].id != origin.id) {
                sellClothes[i].selected = false;
            }
        }

        for (int i = 0; i < sellWeapons.Count; i++) {
            if (sellWeapons[i].id != origin.id) {
                sellWeapons[i].selected = false;
            }
        }

        //de activate buy
        for (int i = 0; i < buyClothes.Count; i++) {
            if (buyClothes[i].id != origin.id) {
                buyClothes[i].selected = false;
            }
        }

        for (int i = 0; i < buyWeapons.Count; i++) {
            if (buyWeapons[i].id != origin.id) {
                buyWeapons[i].selected = false;
            }
        }
    }

    public void SetBuyItems() {
        buyClothes.Clear();
        buyWeapons.Clear();

        for (int i = 0; i < buyContent.childCount; i++) {
            Destroy(buyContent.GetChild(i).gameObject);
        }

        List<Clothes> clothesToAdd = new List<Clothes>();

        for (int i = 0; i < availableClothes.Count; i++) {
            clothesToAdd.Add(availableClothes[i]);
        }

        //add items to the sell list
        for (int i = 0; i < gm.player.clothes.Count; i++) {
            for (int j = 0; j < clothesToAdd.Count; j++) {
                if (gm.player.clothes[i] == clothesToAdd[j]) {
                    clothesToAdd.Remove(clothesToAdd[j]);
                }
            }
        }

        for (int i = 0; i < clothesToAdd.Count; i++) {
            ShopItem sp = Instantiate(shopClothesPrefab, buyContent).GetComponent<ShopItem>();
            sp.price.text = clothesToAdd[i].price.ToString() + "$";
            sp.priceNum = clothesToAdd[i].price;
            sp.itemId = clothesToAdd[i].id;
            sp.isWeapon = false;

            sp.weaponImage.sprite = clothesToAdd[i].sprite;

            buyClothes.Add(sp);
        }

        List<Weapon> weaponsToAdd = new List<Weapon>();

        for (int i = 0; i < availableWeapons.Count; i++) {
            weaponsToAdd.Add(availableWeapons[i]);
        }

        for (int i = 0; i < _playerAttackViewComponent.weapons.Count; i++) {
            for (int j = 0; j < weaponsToAdd.Count; j++) {
                if (_playerAttackViewComponent.weapons[i] == weaponsToAdd[j]) {
                    weaponsToAdd.Remove(weaponsToAdd[j]);
                }
            }
        }

        for (int i = 0; i < weaponsToAdd.Count; i++) {
            ShopItem sp = Instantiate(shopWeaponPrefab, buyContent).GetComponent<ShopItem>();
            sp.damage.text = weaponsToAdd[i].damage.ToString();
            sp.force.text = weaponsToAdd[i].force.ToString();
            sp.price.text = weaponsToAdd[i].price.ToString() + "$";
            sp.priceNum = weaponsToAdd[i].price;
            sp.itemId = weaponsToAdd[i].id;
            sp.isWeapon = true;

            sp.weaponImage.sprite = weaponsToAdd[i].sprite;

            buyWeapons.Add(sp);
        }

        for (int i = 0; i < buyClothes.Count; i++) {
            buyClothes[i].id = i;
        }

        for (int i = 0; i < buyWeapons.Count; i++) {
            buyWeapons[i].id = buyClothes.Count + i;
        }

        scrollBar.value = 100;
    }

    public void OnSellPanel() {
        buyPanel.SetActive(false);
        sellPanel.SetActive(true);
    }

    public void OnBuyPanel() {
        buyPanel.SetActive(true);
        sellPanel.SetActive(false);
    }

    public void OnSell() {
        if (currentShopItem == null) return;

        if (currentShopItem.isWeapon) {
            if (_playerAttackViewComponent.weapons.Count <= 1) {
                gm.ShowText($"You have only one item of this type", 50, Color.yellow, gm.player.transform.position,
                    Vector3.up * Random.Range(30, 50), 2f);
                return;
            }

            Weapon w = _playerAttackViewComponent.weapons.Find(_w => _w.id == currentShopItem.itemId);
            if (w != null) _playerAttackViewComponent.weapons.Remove(w);

            if (_playerAttackViewComponent.weapons.Count == 1) _playerAttackViewComponent.ChangeWeaponV2(0);
        }
        else {
            if (gm.player.clothes.Count <= 1) {
                gm.ShowText($"You have only one item of this type", 50, Color.yellow, gm.player.transform.position,
                    Vector3.up * Random.Range(30, 50), 2f);
                return;
            }

            Clothes c = gm.player.clothes.Find(_c => _c.id == currentShopItem.itemId);
            if (c != null) gm.player.clothes.Remove(c);

            if (gm.player.clothes.Count == 1) gm.player.ChangeClothing(0);
        }

        SetSellItems();
        SetBuyItems();

        gm.player.money += currentShopItem.priceNum;
        gm.ShowText($"+{currentShopItem.priceNum} gold", 50, Color.yellow, gm.player.transform.position,
            Vector3.up * Random.Range(30, 50), 2f);
    }

    public void OnBuy() {
        if (currentShopItem == null) return;
        if (gm.player.money < currentShopItem.priceNum) {
            gm.ShowText("You have no gold", 50, Color.yellow, gm.player.transform.position,
                Vector3.up * Random.Range(30, 50), 2f);
            return;
        }

        if (currentShopItem.isWeapon) {
            Weapon W = availableWeapons.Find(_w => _w.id == currentShopItem.itemId);
            if (W != null) _playerAttackViewComponent.weapons.Add(W);
        }
        else {
            Clothes c = availableClothes.Find(_c => _c.id == currentShopItem.itemId);
            if (c != null) gm.player.clothes.Add(c);
        }

        SetSellItems();
        SetBuyItems();

        gm.player.money -= currentShopItem.priceNum;
        gm.ShowText($"-{currentShopItem.priceNum} gold", 50, Color.yellow, gm.player.transform.position,
            Vector3.up * Random.Range(30, 50), 2f);
    }

    public void OnShop(bool open) {
        if (shopChanging) return;

        Image buttonImage = inventoryButton.gameObject.GetComponent<Image>();

        if (open) {
            shopChanging = true;
            buttonImage.DOFade(0, 0.2f)
                .OnComplete(() => {
                    inventoryButton.gameObject.SetActive(!open);
                    shopChanging = false;
                });

            shopAnimator.SetTrigger("show");
            gm.inShop = true;
            shopOpened = true;
        }
        else {
            shopChanging = true;
            buttonImage.DOFade(100, 0.2f)
                .OnComplete(() => {
                    inventoryButton.gameObject.SetActive(!open);
                    shopChanging = false;
                });

            shopAnimator.SetTrigger("hide");
            gm.inShop = false;
            shopOpened = false;
            gm.dialogueManager.EndDialogue(false);
        }
    }

    #endregion

    public void OnInventory(bool open) {
        if (menuChanging) return;

        Image buttonImage = inventoryButton.gameObject.GetComponent<Image>();

        if (open) {
            menuChanging = true;
            buttonImage.DOFade(0, 0.2f)
                .OnComplete(() => {
                    inventoryButton.gameObject.SetActive(!open);
                    menuChanging = false;
                });

            menuAnimator.SetTrigger("show");
            gm.inUI = true;
            menuOpened = true;
        }
        else {
            menuChanging = true;
            buttonImage.DOFade(100, 0.2f)
                .OnComplete(() => {
                    inventoryButton.gameObject.SetActive(!open);
                    menuChanging = false;
                });

            menuAnimator.SetTrigger("hide");
            gm.inUI = false;
            menuOpened = false;
        }
    }
}