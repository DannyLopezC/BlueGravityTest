using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILifeFountainView : ICollidableView {
}

public class LifeFountainView : CollidableView, ILifeFountainView {
    private ILifeFountainController _lifeFountainController;

    public ILifeFountainController LifeFountainController {
        get { return _lifeFountainController ??= new LifeFountainController(this); }
    }

    [SerializeField] private float healAmount;

    private void Update() {
        if (LifeFountainController.GetIsHealing()) GameManager.Instance.HealPlayer(healAmount);
    }

    public override void OnCollisionEnter2D(Collision2D c) {
        LifeFountainController.OnCollide(c.collider);
    }

    public override void OnTriggerEnter2D(Collider2D c) {
    }

    public override void OnCollisionExit2D(Collision2D c) {
        LifeFountainController.OnExitCollide(c.collider);
    }

    public override void OnTriggerExit2D(Collider2D c) {
    }
}