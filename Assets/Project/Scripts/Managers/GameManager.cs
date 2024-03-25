using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviourSingleton<GameManager> {
    [SerializeField] private PlayerView playerView;
    [SerializeField] private AttackView attackView;
    [SerializeField] private NpcView npcView;

    public void ShowText(string msg, int fontSize, Color color, Vector3 position, Vector3 motion, float duration) =>
        FloatingTextManager.Instance.Show(msg, fontSize, color, position, motion, duration);

    public event Action<bool> DialogueEvent;

    public void OnDialogue(bool isStarting) {
        DialogueEvent?.Invoke(isStarting);
    }

    public Dialogue GetFirstNpcDialogue() {
        return npcView.GetFirstDialogue();
    }

    public Dialogue GetGoodbyeNpcDialogue() {
        return npcView.GetGoodbyeDialogue();
    }

    public Transform GetPlayerTransform() {
        return playerView.transform;
    }

    public int GetEquippedWeapon() {
        return attackView.AttackController.GetCurrentEquipped();
    }

    public List<Clothes> GetPlayerClothes() {
        return playerView.GetClothesList();
    }

    public List<Weapon> GetPlayerWeapons() {
        return attackView.GetWeapons();
    }

    public int GetClothesId() {
        return playerView.PlayerController.GetCurrentClothes().id;
    }

    public int GetWeaponId() {
        return attackView.AttackController.GetCurrenWeapon().id;
    }

    public void HealPlayer(float amount) {
        playerView.PlayerController.Heal(amount);
    }

    public void AddMoney(int money) {
        playerView.PlayerController.AddMoney(money);
    }

    public bool GetInDialogue() {
        return playerView.PlayerController.GetInDialogue();
    }

    public void DeselectAllSellItems(ShopItem origin) =>
        UIManager.Instance.ShopView.ShopController.DeselectAllSellItems(origin);

    public void ChangeWeaponV2(int id) => playerView.GetComponentInChildren<AttackView>().ChangeWeaponV2(id);

    [Button]
    public void ChangeWeapon(int id) => playerView.GetComponentInChildren<AttackView>().ChangeWeapon(id);

    [Button]
    public void ChangeClothing(int id) => playerView.ChangeClothing(id);

    public void ChangeClothingV2(int nextId) => playerView.ChangeClothingV2(nextId);
}