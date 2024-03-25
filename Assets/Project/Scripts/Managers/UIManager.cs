using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviourSingleton<UIManager> {
    public Button inventoryButton;

    // [BoxGroup("Menu")] public TMP_Text moneyAmount;
    public Image lifeBar;

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
        // moneyAmount.text = gm.player.money.ToString();
        ShopView.UpdateUIValues();
        // hudMoney.text = gm.player.money.ToString();
        //
        // skinsAmount.text = gm.player.clothes.Count.ToString();
        // weaponsAmount.text = _playerAttackViewComponent.weapons.Count.ToString();
        //
        // damageAmount.text = _playerAttackViewComponent.currentWeapon.damage.ToString();
        // forceAmount.text = _playerAttackViewComponent.currentWeapon.force.ToString();
        //
        // menuLifeBar.fillAmount = gm.player.life / gm.player.maxLife;}

        lifeBar.fillAmount = playerLife / playerMaxLife;
    }
}