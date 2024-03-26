using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IChestView : ICollectableView {
    Transform GetTransform();
    void ChangeSprite(Sprite s);
    Sprite GetOpenedChest();
    Sprite GetEmptyChest();
    int GetMoney();
}

public class ChestView : CollectableView, IChestView {
    [SerializeField] private int money;

    public Sprite openedChest;
    public Sprite emptyChest;

    private IChestController _chestController;

    public IChestController ChestController {
        get { return _chestController ??= new ChestController(this); }
    }

    public void ChangeSprite(Sprite s) => GetComponent<SpriteRenderer>().sprite = s;

    public Sprite GetOpenedChest() {
        return openedChest;
    }

    public Sprite GetEmptyChest() {
        return emptyChest;
    }

    public int GetMoney() {
        return money;
    }

    public override void OnCollisionEnter2D(Collision2D c) {
        ChestController.OnCollide(c.collider);
    }

    public override void OnTriggerEnter2D(Collider2D c) {
    }

    public override void OnCollisionExit2D(Collision2D c) {
    }

    public override void OnTriggerExit2D(Collider2D c) {
    }

    public Transform GetTransform() {
        return transform;
    }
}