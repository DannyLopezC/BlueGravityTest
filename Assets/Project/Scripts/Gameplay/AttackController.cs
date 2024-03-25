﻿using UnityEngine;

public interface IAttackController : ICollidableController {
    void SetEquipped(int weaponIndex);
    void ChangeWeapon(int equip);
    void ChangeWeaponV2(int equip);
    Weapon GetCurrenWeapon();
    void Attack();
    void UpdateUIValues();
}

public class AttackController : CollidableController, IAttackController {
    private readonly IAttackView _view;

    private Weapon _currentWeapon;

    private float _lastSwing;

    private int _equipped;

    public int Equipped {
        get => _equipped;
        set {
            _equipped = Mathf.Clamp(value, 0, _view.GetWeapons().Count - 1);
            ChangeWeapon(_equipped);
        }
    }

    public AttackController(IAttackView view) : base(view) {
        _view = view;
    }

    public override void OnCollide(Collider2D c) {
        if (!c.CompareTag("Enemy")) return;
        Damage dmg = new Damage(_view.GetTransform().position, _currentWeapon.damage, _currentWeapon.force);

        c.SendMessage("ReceiveDamage", dmg);
    }

    public override void OnExitCollide(Collider2D c) {
        throw new System.NotImplementedException();
    }

    public void ChangeWeapon(int equip) {
        _currentWeapon = _view.GetWeapons().Find(w => w.id == equip);

        if (_currentWeapon == null) _currentWeapon = _view.GetWeapons()[0];
        UpdateUIValues();
    }

    public void ChangeWeaponV2(int id) {
        _currentWeapon = _view.GetWeapons()[id];
        UpdateUIValues();
    }

    public Weapon GetCurrenWeapon() {
        return _currentWeapon;
    }

    public void SetEquipped(int weaponIndex) {
        Equipped = 0;
        UpdateUIValues();
    }

    public void Attack() {
        if (!(Time.time - _lastSwing > _view.GetCooldown())) return;
        _lastSwing = Time.deltaTime;
        _view.Swing();
    }

    public void UpdateUIValues() {
        UIManager.Instance.currentWeapon = _currentWeapon;
        UIManager.Instance.skinsAmount = _view.GetWeapons().Count;
    }
}