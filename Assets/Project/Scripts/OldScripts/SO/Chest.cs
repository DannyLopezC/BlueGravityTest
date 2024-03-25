using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : Collectable
{
    public Sprite openedChest;
    public Sprite emptyChest;
    public int money;
    private bool _opened;

    protected override void OnCollide(Collider2D c)
    {
        if (!c.CompareTag("Player")) return;

        if (!_opened)
        {
            _opened = true;
            ChangeSprite(openedChest);
        }
        else OnCollect(c);
    }

    protected override void OnCollect(Collider2D c)
    {
        if (!collected)
        {
            GameManager.instance.ShowText($"+{money} gold", 50, Color.yellow, transform.position,
                Vector3.up * Random.Range(30, 50), 2f);
            ChangeSprite(emptyChest);
            c.GetComponent<Player>().money += money;
        }

        base.OnCollect(c);
    }

    private void ChangeSprite(Sprite s) => GetComponent<SpriteRenderer>().sprite = s;
}