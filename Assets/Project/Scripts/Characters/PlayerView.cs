using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public interface IPlayerView : IFighterView {
    void ChangeClothingById(int id);
    List<Clothes> GetClothesList();
}

public class PlayerView : FighterView, IPlayerView {
    [SerializeField] private List<Clothes> clothes;
    public SpriteRenderer spriteRenderer;

    private IPlayerController _playerController;

    public IPlayerController PlayerController {
        get { return _playerController ??= new PlayerController(this, maxLife); }
    }

    private void Awake() {
        GameManager.Instance.DialogueEvent += OnDialogue;
    }

    private void Update() {
        PlayerController.UpdateUIValues();
    }

    private void OnDialogue(bool isStarting) {
        PlayerController.SetInDialogue(isStarting);
    }

    public void ChangeClothingById(int id) {
        PlayerController.ChangeClothingById(id);

        spriteRenderer.sprite = PlayerController.GetCurrentClothes().sprite;
    }

    public void ChangeClothingByIndex(int id) {
        PlayerController.ChangeClothingByIndex(id);

        spriteRenderer.sprite = PlayerController.GetCurrentClothes().sprite;
    }

    public List<Clothes> GetClothesList() {
        return clothes;
    }

    private void FixedUpdate() {
        PlayerController.Movement();
    }

    private void OnDestroy() {
        if (GameManager.HasInstance()) {
            GameManager.Instance.DialogueEvent -= OnDialogue;
        }
    }

    protected override void ReceiveDamage(Damage dmg) {
        PlayerController.ReceiveDamage(dmg);

        GameManager.Instance.ShowText($"{dmg.damageAmount} damage", 35, Color.white, transform.position,
            Vector3.up * Random.Range(30, 50), 2f);

        if (PlayerController.Death()) gameObject.SetActive(false);
    }
}