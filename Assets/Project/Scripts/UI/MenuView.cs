using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public interface IMenuView {
    void SettingClothesSprites(int nextId, Clothes replaceClothes);
    void SettingWeaponSprites(int nextId, Weapon replaceWeapon);
}

public class MenuView : MonoBehaviour, IMenuView {
    [SerializeField] private Image skinSprite;
    [SerializeField] private Image weaponSprite;

    [SerializeField] private Image menuLifeBar;
    [SerializeField] private TMP_Text damageAmount;
    [SerializeField] private TMP_Text skinsAmount;
    [SerializeField] private TMP_Text weaponsAmount;
    [SerializeField] private TMP_Text moneyAmount;
    [SerializeField] private TMP_Text forceAmount;

    private IMenuController _menuController;

    public IMenuController MenuController {
        get { return _menuController ??= new MenuController(this); }
    }

    private void Start() {
        weaponSprite.sprite = GameManager.Instance.GetPlayerWeapons()[GameManager.Instance.GetEquippedWeapon()].sprite;
    }

    public void SettingClothesSprites(int nextId, Clothes replaceClothes) {
        skinSprite.sprite = replaceClothes.sprite;
        GameManager.Instance.ChangeClothingByIndex(nextId);
    }

    public void SettingWeaponSprites(int nextId, Weapon replaceWeapon) {
        weaponSprite.sprite = replaceWeapon.sprite;
        GameManager.Instance.ChangeWeaponByIndex(nextId);
    }

    public void OnNextWeapon() => MenuController.OnNextWeapon();
    public void OnBackWeapon() => MenuController.OnBackWeapon();
    public void OnNextSkin() => MenuController.OnNextSkin();
    public void OnBackSkin() => MenuController.OnBackSkin();

    public void UpdateUIValues() {
        skinsAmount.text = GameManager.Instance.GetPlayerClothes().Count.ToString();
        weaponsAmount.text = GameManager.Instance.GetPlayerWeapons().Count.ToString();
        damageAmount.text = UIManager.Instance.currentWeapon.damage.ToString();
        forceAmount.text = UIManager.Instance.currentWeapon.force.ToString();
        moneyAmount.text = UIManager.Instance.money.ToString();
        menuLifeBar.fillAmount = UIManager.Instance.playerLife / UIManager.Instance.playerMaxLife;
    }
}