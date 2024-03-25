using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviourSingleton<UIManager> {
    // [BoxGroup("Menu")] public TMP_Text moneyAmount;
    public Image lifeBar;

    public Weapon currentWeapon;
    public float playerMaxLife;
    public float playerLife;
    public int money;
    public int skinsAmount;
    public int weaponsAmount;

    public ShopView ShopView;
    
    

    private void Start() {
        // weaponSprite.sprite = _playerAttackViewComponent.weapons[_playerAttackViewComponent.Equipped].sprite;
        UpdateUIValues();

        ShopView.ShopController.SetSellItems();
        SetBuyItems();
    }

    public void OnShop(bool open) {
    }

    public void DeselectAllSellItems(ShopItem origin) {
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