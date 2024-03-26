using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System;
using UnityEngine.Serialization;

public interface IAttackView : ICollidableView {
    Transform GetTransform();
    void Swing();
    List<Weapon> GetWeapons();
    float GetCooldown();
}

public class AttackView : CollidableView, IAttackView {
    private IAttackController _attackController;

    public IAttackController AttackController {
        get { return _attackController ??= new AttackController(this); }
    }

    public SpriteRenderer spriteRenderer;

    [SerializeField] private List<Weapon> weapons;

    [SerializeField] private float cooldown = 0.5f;

    [SerializeField] private Animator _animator;
    private static readonly int Swing1 = Animator.StringToHash("Swing");

    private void Awake() {
        AttackController.SetEquipped(0);
        ChangeWeapon(0);
        UIManager.Instance.weaponsAmount = weapons.Count;
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            AttackController.Attack();
        }

        UIManager.Instance.weaponsAmount = weapons.Count;
    }

    public void Swing() {
        _animator.SetTrigger(Swing1);
    }

    public List<Weapon> GetWeapons() {
        return weapons;
    }

    public float GetCooldown() {
        return cooldown;
    }

    public void ChangeWeapon(int equip) {
        AttackController.ChangeWeapon(equip);

        spriteRenderer.sprite = AttackController.GetCurrenWeapon().sprite;
    }

    public void ChangeWeaponV2(int equip) {
        AttackController.ChangeWeapon(equip);

        spriteRenderer.sprite = AttackController.GetCurrenWeapon().sprite;
    }

    public override void OnCollisionEnter2D(Collision2D c) {
        AttackController.OnCollide(c.collider);
    }

    public override void OnTriggerEnter2D(Collider2D c) {
        AttackController.OnCollide(c);
    }

    public override void OnCollisionExit2D(Collision2D c) {
        AttackController.OnExitCollide(c.collider);
    }

    public override void OnTriggerExit2D(Collider2D c) {
        AttackController.OnExitCollide(c);
    }

    public Transform GetTransform() {
        return transform;
    }
}