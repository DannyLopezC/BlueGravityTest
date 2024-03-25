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
    private bool _menuOpened = false;
    private bool _menuChanging = false;
    public Animator menuAnimator;

    public bool inUI;

    //public TMP_Text moneyAmount;
    public TMP_Text hudMoney;
    public Image lifeBar;

    public Weapon currentWeapon;
    public float playerMaxLife;
    public float playerLife;
    public int money;
    public int skinsAmount;
    public int weaponsAmount;

    public ShopView ShopView;
    private bool _shopChanging = false;
    public Animator shopAnimator;
    private bool _inShop;
    private bool _shopOpened = false;


    private void Start() {
        UpdateUIValues();

        ShopView.ShopController.SetSellItems();
        ShopView.ShopController.SetBuyItems();
    }

    public void OnReloadScene() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    public void OnExit() => Application.Quit();

    private void Update() {
        if (!_shopOpened && GameManager.Instance.GetInDialogue() && !_inShop)
            if (Input.GetKeyDown(KeyCode.Escape))
                OnInventory(!_menuOpened);
        if (_shopOpened && _inShop)
            if (Input.GetKeyDown(KeyCode.Escape))
                OnShop(false);

        UpdateUIValues();
    }

    public void OnShop(bool open) {
        if (_shopChanging) return;

        Image buttonImage = inventoryButton.gameObject.GetComponent<Image>();

        if (open) {
            _shopChanging = true;
            buttonImage.DOFade(0, 0.2f)
                .OnComplete(() => {
                    inventoryButton.gameObject.SetActive(!open);
                    _shopChanging = false;
                });

            shopAnimator.SetTrigger("show");
            _inShop = true;
            _shopOpened = true;
        }
        else {
            _shopChanging = true;
            buttonImage.DOFade(100, 0.2f)
                .OnComplete(() => {
                    inventoryButton.gameObject.SetActive(!open);
                    _shopChanging = false;
                });

            shopAnimator.SetTrigger("hide");
            _inShop = false;
            _shopOpened = false;
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
        if (_menuChanging) return;

        Image buttonImage = inventoryButton.gameObject.GetComponent<Image>();

        if (open) {
            _menuChanging = true;
            buttonImage.DOFade(0, 0.2f)
                .OnComplete(() => {
                    inventoryButton.gameObject.SetActive(!open);
                    _menuChanging = false;
                });

            menuAnimator.SetTrigger("show");
            inUI = true;
            _menuOpened = true;
        }
        else {
            _menuChanging = true;
            buttonImage.DOFade(100, 0.2f)
                .OnComplete(() => {
                    inventoryButton.gameObject.SetActive(!open);
                    _menuChanging = false;
                });

            menuAnimator.SetTrigger("hide");
            inUI = false;
            _menuOpened = false;
        }
    }
}