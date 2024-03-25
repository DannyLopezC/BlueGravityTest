using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Fighter
{
    private Vector3 _moveDelta;

    public int money;

    public List<Clothes> clothes;
    public Clothes currentClothes;
    public SpriteRenderer spriteRenderer;

    private int _clothesId;

    private void Awake()
    {
        ClothesId = 0;
    }

    public int ClothesId
    {
        get => _clothesId;
        set
        {
            _clothesId = Mathf.Clamp(value, 0, clothes.Count - 1);
            ChangeClothing(_clothesId);
        }
    }

    private void FixedUpdate()
    {
        if (!GameManager.instance.inUI && !GameManager.instance.inDialogue && !GameManager.instance.inShop) Movement();
    }

    private void Movement()
    {
        _moveDelta = Vector3.zero;

        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        //set delta
        _moveDelta = new Vector3(x, y);

        //change facing direction
        if (_moveDelta.x > 0) transform.localScale = Vector3.one;
        else if (_moveDelta.x < 0) transform.localScale = new Vector3(-1, 1, 1);

        //push
        _moveDelta += pushDirection;
        pushDirection = Vector3.Lerp(pushDirection, Vector3.zero, pushRecoverySpeed);

        //movement
        transform.Translate(_moveDelta * Time.deltaTime);
    }

    public void ChangeClothing(int id)
    {
        currentClothes = clothes.Find(w => w.id == id);

        if (currentClothes == null) currentClothes = clothes[0];

        spriteRenderer.sprite = currentClothes.sprite;
    }

    public void ChangeClothingV2(int id)
    {
        currentClothes = clothes[id];
        spriteRenderer.sprite = currentClothes.sprite;
    }
}