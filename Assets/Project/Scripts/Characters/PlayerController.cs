using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public interface IPlayerController : IFighterController {
    void ChangeClothingById(int id);
    void ChangeClothingByIndex(int id);
    Clothes GetCurrentClothes();
    void Movement();
    void SetInDialogue(bool isStarting);
    void Heal(float amount);
    void AddMoney(int money);
    void UpdateUIValues();
    bool GetInDialogue();
}

public class PlayerController : FighterController, IPlayerController {
    private readonly IPlayerView _view;
    private Vector3 _moveDelta;

    private int _money;

    private Clothes _currentClothes;

    private bool _inUI;
    private bool _inDialogue;
    private bool _inShop;

    public PlayerController(IPlayerView view, float maxLife) : base(view, maxLife) {
        _view = view;
        ClothesId = 0;
        UIManager.Instance.playerMaxLife = maxLife;
        UpdateUIValues();
    }

    private int _clothesId;

    public int ClothesId {
        get => _clothesId;
        set {
            _clothesId = Mathf.Clamp(value, 0, _view.GetClothesList().Count - 1);
            ChangeClothingById(_clothesId);
        }
    }

    public void ChangeClothingById(int id) {
        _currentClothes = _view.GetClothesList().Find(w => w.id == id);

        if (_currentClothes == null) _currentClothes = _view.GetClothesList()[0];
    }

    public Clothes GetCurrentClothes() {
        return _currentClothes;
    }

    public void ChangeClothingByIndex(int id) {
        _currentClothes = _view.GetClothesList()[id];
    }

    public void Movement() {
        if (UIManager.Instance._inShop || _inDialogue || UIManager.Instance.inUI) return;

        _moveDelta = Vector3.zero;

        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        //set delta
        _moveDelta = new Vector3(x, y);

        //change facing direction
        if (_moveDelta.x > 0) _view.GetTransform().localScale = Vector3.one;
        else if (_moveDelta.x < 0) _view.GetTransform().localScale = new Vector3(-1, 1, 1);

        //push
        _moveDelta += PushDirection;
        PushDirection = Vector3.Lerp(PushDirection, Vector3.zero, _view.GetPushRecovery());

        //movement
        _view.GetTransform().Translate(_moveDelta * Time.deltaTime);
    }

    public void SetInDialogue(bool isStarting) {
        _inDialogue = isStarting;
    }

    public void Heal(float amount) {
        Life += amount;
    }

    public void AddMoney(int money) {
        _money += money;
    }

    public void UpdateUIValues() {
        UIManager.Instance.money = _money;
        UIManager.Instance.playerLife = Life;
        UIManager.Instance.skinsAmount = _view.GetClothesList().Count;
    }

    public bool GetInDialogue() {
        return _inDialogue;
    }
}