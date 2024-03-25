using System;
using UnityEngine;
using UnityEngine.UI;

public interface IEnemyView : IFighterView, ICollidableView {
    float GetTriggerLength();
    float GetChaseLength();
    int GetDamage();
    float GetForce();
    float GetRotationThreshHold();
}

public class EnemyView : FighterView, IEnemyView {
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

    public void OnCollisionEnter2D(Collision2D c) {
        EnemyController.OnCollide(c.collider);
    }

    public void OnTriggerEnter2D(Collider2D c) {
    }

    public void OnCollisionExit2D(Collision2D c) {
        EnemyController.OnExitCollide(c.collider);
    }

    public void OnTriggerExit2D(Collider2D c) {
    }
}