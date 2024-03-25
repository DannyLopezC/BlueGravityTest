using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System;

public class OldGameManager : MonoBehaviour {
    public static GameManager instance;
    public UIManager uIManager;

    public DialogueManager dialogueManager;
    public Dialogue firstDialogue;
    public Dialogue goodbyeDialogue;

    public bool inUI;
    public bool inDialogue;
    public bool inShop;

    public List<Sprite> playerSprites;
    public List<Sprite> weaponSprites;
    public List<int> weaponPrices;

    //referenced in inspector
    public Player player;

    public FloatingTextManager ftm;

    public int money;

    private void Awake() {
        if (GameManager.instance != null) {
            Destroy(gameObject);
            return;
        }

        instance = this;
        //DontDestroyOnLoad(gameObject);
    }

    public void DeselectAllSellItems(ShopItem origin) => uIManager.DeselectAllSellItems(origin);

    [Button]
    public void ChangeWeapon(int id) => player.GetComponentInChildren<AttackView>().Equipped = id;

    [Button]
    public void ChangeClothing(int id) => player.ClothesId = id;

    public void ShowText(string msg, int fontSize, Color color, Vector3 position, Vector3 motion, float duration) =>
        ftm.Show(msg, fontSize, color, position, motion, duration);
}