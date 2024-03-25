﻿using UnityEngine;

public interface IEnemyController : IFighterController, ICollidableController {
    float GetCurrentLife();
    void GlobalMovement();
}

public class EnemyController : FighterController, IEnemyController {
    private readonly IEnemyView _view;

    private bool _chasing;
    private bool _goingHome;
    private bool _inHome;

    private Vector3 _startingPos;
    private Vector3 _moveDelta;

    public EnemyController(IEnemyView view, float maxLife, Vector3 startingPos) : base(view, maxLife) {
        _view = view;
        _startingPos = startingPos;
    }

    public void OnCollide(Collider2D c) {
        if (c.CompareTag("Player")) {
            Damage dmg = new(_view.GetTransform().position, _view.GetDamage(), _view.GetForce());

            c.SendMessage("ReceiveDamage", dmg);
        }

        if (_goingHome) {
            if (c.CompareTag("Home")) _inHome = true;
        }
    }

    public void OnExitCollide(Collider2D c) {
        if (c.CompareTag("Home")) _inHome = false;
    }

    public float GetCurrentLife() {
        return Life;
    }

    public void GlobalMovement() {
        if (_chasing) Movement(GameManager.Instance.GetPlayerTransform().position);
        else if (_goingHome) Movement(_startingPos);

        var position = GameManager.Instance.GetPlayerTransform().position;
        _chasing = Vector3.Distance(position, _startingPos) < _view.GetChaseLength() &&
                   Vector3.Distance(position, _startingPos) < _view.GetTriggerLength() &&
                   GameManager.Instance.GetPlayerTransform().gameObject.activeInHierarchy;

        _goingHome = !_chasing && !_inHome;
    }

    private void Movement(Vector3 objective) {
        var position = _view.GetTransform().position;
        float x = objective.x > position.x ? 1 : -1;
        float y = objective.y > position.y ? 1 : -1;

        if (x != _moveDelta.x) {
            if (x <= -_view.GetRotationThreshHold()) _view.GetTransform().localScale = Vector3.one;
            else if (x > _view.GetRotationThreshHold()) _view.GetTransform().localScale = new Vector3(-1, 1, 1);
        }

        _moveDelta = new Vector3(x, y);

        //push
        _moveDelta += PushDirection;
        PushDirection = Vector3.Lerp(PushDirection, Vector3.zero, PushRecoverySpeed);

        _view.GetTransform().Translate((_moveDelta * Time.deltaTime) / 2);
    }
}