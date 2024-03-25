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

    
    [BoxGroup("HUD")] public TMP_Text hudMoney;

    [BoxGroup("Menu")] public GameObject menuGo;
    [BoxGroup("Menu")] public bool menuOpened = false;
    [BoxGroup("Menu")] public bool menuChanging = false;
    [BoxGroup("Menu")] public Animator menuAnimator;
    [BoxGroup("Menu")] public Image weaponSprite;
    [BoxGroup("Menu")] public Image skinSprite;
    [BoxGroup("Menu")] public TMP_Text skinsAmount;
    [BoxGroup("Menu")] public TMP_Text weaponsAmount;
    [BoxGroup("Menu")] public Image menuLifeBar;
    [BoxGroup("Menu")] public TMP_Text damageAmount;
    [BoxGroup("Menu")] public TMP_Text forceAmount;

    [BoxGroup("Shop")] 
    [BoxGroup("Shop")] 
    [BoxGroup("Shop")] 
    [BoxGroup("Shop")] public GameObject shopGo;

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