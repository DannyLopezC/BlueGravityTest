using UnityEngine;

public interface ICollectableController : ICollidableController {
}

public class CollectableController : CollidableController, ICollectableController {
    protected bool Collected;

    public CollectableController(ICollectableView view) : base(view) {
    }

    public override void OnCollide(Collider2D c) {
        if (c.CompareTag("Player")) OnCollect(c);
    }

    protected virtual void OnCollect(Collider2D c) => Collected = true;
}