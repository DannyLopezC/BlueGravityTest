using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviourSingleton<GameManager> {
    [SerializeField] private PlayerView playerView;

    public void ShowText(string s, int i, Color white, object position, Vector3 range, float f) {
    }

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
}