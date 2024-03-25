using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviourSingleton<UIManager> {
    public MenuView MenuView;
    public Button inventoryButton;
    public bool menuOpened = false;
    public bool menuChanging = false;
    public Animator menuAnimator;

    public bool inUI;

    //public TMP_Text moneyAmount;
    public TMP_Text hudMoney;
    public Image lifeBar;
    public Image menuLifeBar;

    public Weapon currentWeapon;
    public float playerMaxLife;
    public float playerLife;
    public int money;
    public int skinsAmount;
    public int weaponsAmount;

    public ShopView ShopView;
    public bool shopChanging = false;
    public Animator shopAnimator;
    public bool inShop;
    public bool shopOpened = false;


    private void Start() {
        // weaponSprite.sprite = _playerAttackViewComponent.weapons[_playerAttackViewComponent.Equipped].sprite;
        UpdateUIValues();

        ShopView.ShopController.SetSellItems();
        ShopView.ShopController.SetBuyItems();
    }

    public void OnReloadScene() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    public void OnExit() => Application.Quit();

    private void Update() {
        if (!shopOpened && GameManager.Instance.GetInDialogue() && !inShop)
            if (Input.GetKeyDown(KeyCode.Escape))
                OnInventory(!menuOpened);
        if (shopOpened && inShop)
            if (Input.GetKeyDown(KeyCode.Escape))
                OnShop(false);

        UpdateUIValues();
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
            inShop = true;
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
            inShop = false;
            shopOpened = false;
            DialogueManager.Instance.EndDialogue(false, NpcController.GoodbyeDialogue);
        }
    }

    public void UpdateUIValues() {
        ShopView.UpdateUIValues();
        hudMoney.text = money.ToString();

        MenuView.UpdateUIValues();

        lifeBar.fillAmount = playerLife / playerMaxLife;
    }

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
            inUI = true;
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
            inUI = false;
            menuOpened = false;
        }
    }
}