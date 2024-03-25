using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviourSingleton<GameManager> {
    [SerializeField] private PlayerView playerView;

    public void ShowText(string msg, int fontSize, Color color, Vector3 position, Vector3 motion, float duration) =>
        FloatingTextManager.Instance.Show(msg, fontSize, color, position, motion, duration);

    public event Action<bool> DialogueEvent;

    public void OnDialogue(bool isStarting) {
        DialogueEvent?.Invoke(isStarting);
    }

    public Transform GetPlayerTransform() {
        return playerView.transform;
    }

    public void HealPlayer(float amount) {
        playerView.PlayerController.Heal(amount);
    }

    public void AddMoney(int money) {
        playerView.PlayerController.AddMoney(money);
    }

    public void DeselectAllSellItems(ShopItem origin) => UIManager.Instance.DeselectAllSellItems(origin);

    [Button]
    public void ChangeWeapon(int id) => playerView.GetComponentInChildren<AttackView>().ChangeWeapon(id);

    [Button]
    public void ChangeClothing(int id) => playerView.ChangeClothing(id);
}