using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System;

public class Attack : Collidable
{
    private int _equipped;

    public int Equipped
    {
        get => _equipped;
        set
        {
            _equipped = Mathf.Clamp(value, 0, weapons.Count - 1);
            ChangeWeapon(_equipped);
        }
    }

    public Weapon currentWeapon;
    public SpriteRenderer spriteRenderer;

    [InlineEditor] public List<Weapon> weapons;

    private float _cooldown = 0.5f;
    private float _lastSwing;

    private Animator _animator;
    private static readonly int Swing1 = Animator.StringToHash("Swing");

    private void Awake()
    {
        Equipped = 0;
    }

    protected override void Start()
    {
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!(Time.time - _lastSwing > _cooldown)) return;
            _lastSwing = Time.deltaTime;
            Swing();
        }
    }

    protected override void OnCollide(Collider2D c)
    {
        if (!c.CompareTag("Enemy")) return;
        Damage dmg = new Damage(transform.position, currentWeapon.damage, currentWeapon.force);

        c.SendMessage("ReceiveDamage", dmg);
    }

    private void Swing()
    {
        _animator.SetTrigger(Swing1);
    }

    private void ChangeWeapon(int equip)
    {
        currentWeapon = weapons.Find(w => w.id == equip);

        if (currentWeapon == null) currentWeapon = weapons[0];

        spriteRenderer.sprite = currentWeapon.sprite;
    }

    public void ChangeWeaponV2(int id)
    {
        currentWeapon = weapons[id];
        spriteRenderer.sprite = currentWeapon.sprite;
    }
}