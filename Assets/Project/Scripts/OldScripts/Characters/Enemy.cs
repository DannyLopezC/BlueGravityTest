using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : Fighter
{
    public float triggerLength = 1;
    public float chaseLength = 5;

    public int damage;
    public float force;

    private bool _chasing;
    private bool _goingHome;
    private bool _inHome;
    private Transform _player;

    public Transform homeTransform;
    private Vector3 _startingPos;

    private Vector3 _moveDelta;

    public float rotationThreshHold;

    public Image enemyLifeBarSlider;
    public GameObject enemyLifeBar;
    public Vector3 enemyLifeBarOffset;

    protected override void Start()
    {
        base.Start();

        _player = GameManager.instance.player.transform;
        _startingPos = homeTransform.position;
    }

    private void Update()
    {
        enemyLifeBarSlider.fillAmount = life / maxLife;
        enemyLifeBar.transform.position = Camera.main.WorldToScreenPoint(transform.position + enemyLifeBarOffset);
    }

    private void FixedUpdate()
    {
        if (_chasing) Movement(_player.position);
        else if (_goingHome) Movement(_startingPos);

        var position = _player.position;
        _chasing = Vector3.Distance(position, _startingPos) < chaseLength &&
                   Vector3.Distance(position, _startingPos) < triggerLength &&
                   GameManager.instance.player.isActiveAndEnabled;

        _goingHome = !_chasing && !_inHome;
    }

    private void OnTriggerEnter2D(Collider2D c)
    {
        DoDamage(c);

        if (_goingHome) HomeReached(c);
    }

    private void OnCollisionEnter2D(Collision2D c)
    {
        DoDamage(c.collider);

        if (_goingHome) HomeReached(c.collider);
    }

    private void OnTriggerExit2D(Collider2D c)
    {
        ExitHome(c);
    }

    private void OnCollisionExit2D(Collision2D c)
    {
        ExitHome(c.collider);
    }

    private void HomeReached(Collider2D c)
    {
        if (c.CompareTag("Home")) _inHome = true;
    }

    private void ExitHome(Collider2D c)
    {
        if (c.CompareTag("Home")) _inHome = false;
    }

    private void DoDamage(Collider2D c)
    {
        if (!c.CompareTag("Player")) return;
        Damage dmg = new Damage(transform.position, damage, force);

        c.SendMessage("ReceiveDamage", dmg);
    }

    private void Movement(Vector3 objective)
    {
        var position = transform.position;
        float x = objective.x > position.x ? 1 : -1;
        float y = objective.y > position.y ? 1 : -1;

        if (x != _moveDelta.x)
        {
            if (x <= -rotationThreshHold) transform.localScale = Vector3.one;
            else if (x > rotationThreshHold) transform.localScale = new Vector3(-1, 1, 1);
        }

        _moveDelta = new Vector3(x, y);

        //push
        _moveDelta += pushDirection;
        pushDirection = Vector3.Lerp(pushDirection, Vector3.zero, pushRecoverySpeed);

        transform.Translate((_moveDelta * Time.deltaTime) / 2);
    }
}