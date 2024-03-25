using UnityEngine;

public interface ILifeFountainController : ICollidableController {
    bool GetIsHealing();
}

public class LifeFountainController : CollidableController, ILifeFountainController {
    private bool _isHealing;

    public LifeFountainController(ICollidableView view) : base(view) {
    }

    public override void OnCollide(Collider2D c) {
        if (c.CompareTag("Player")) _isHealing = true;
    }

    public override void OnExitCollide(Collider2D c) {
        if (c.CompareTag("Player")) _isHealing = false;
    }

    public bool GetIsHealing() {
        return _isHealing;
    }
}