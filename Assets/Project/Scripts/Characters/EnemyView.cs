﻿using System;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public interface IEnemyView : IFighterView, ICollidableView {
    float GetTriggerLength();
    float GetChaseLength();
    int GetDamage();
    float GetForce();
    float GetRotationThreshHold();
    float GetMovementGap();
}

public class EnemyView : FighterView, IEnemyView {
    [SerializeField] private float movementGap;
    [SerializeField] private Transform homeTransform;

    [SerializeField] private float triggerLength = 1;
    [SerializeField] private float chaseLength = 5;
    [SerializeField] private int damage;
    [SerializeField] private float force;

    [SerializeField] private float rotationThreshHold;

    public Image enemyLifeBarSlider;
    public GameObject enemyLifeBar;
    public Vector3 enemyLifeBarOffset;

    private IEnemyController _enemyController;

    public IEnemyController EnemyController {
        get { return _enemyController ??= new EnemyController(this, maxLife, homeTransform.position); }
    }

    private void Update() {
        enemyLifeBarSlider.fillAmount = EnemyController.GetCurrentLife() / maxLife;
        if (Camera.main != null)
            enemyLifeBar.transform.position = Camera.main.WorldToScreenPoint(transform.position + enemyLifeBarOffset);
    }

    private void FixedUpdate() {
        EnemyController.GlobalMovement();
    }

    public float GetTriggerLength() {
        return triggerLength;
    }

    public float GetChaseLength() {
        return chaseLength;
    }

    public int GetDamage() {
        return damage;
    }

    public float GetForce() {
        return force;
    }

    public float GetRotationThreshHold() {
        return rotationThreshHold;
    }

    public float GetMovementGap() {
        return movementGap;
    }

    public void OnCollisionEnter2D(Collision2D c) {
        EnemyController.OnCollide(c.collider);
    }

    public void OnTriggerEnter2D(Collider2D c) {
        EnemyController.OnCollide(c);
    }

    public void OnCollisionExit2D(Collision2D c) {
        EnemyController.OnExitCollide(c.collider);
    }

    public void OnTriggerExit2D(Collider2D c) {
        EnemyController.OnExitCollide(c);
    }

    protected override void ReceiveDamage(Damage dmg) {
        EnemyController.ReceiveDamage(dmg);

        GameManager.Instance.ShowText($"{dmg.damageAmount} damage", 35, Color.white, transform.position,
            Vector3.up * Random.Range(30, 50), 2f);

        if (EnemyController.Death()) gameObject.SetActive(false);
    }
}