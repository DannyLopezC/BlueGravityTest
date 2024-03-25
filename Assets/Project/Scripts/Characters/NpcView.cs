using UnityEngine;

public interface INpcView : ICollidableView {
}

public class NpcView : CollidableView, INpcView {
    private INpcController _npcController;

    public INpcController NpcController {
        get { return _npcController ??= new NpcController(this); }
    }

    public override void OnCollisionEnter2D(Collision2D c) {
        NpcController.OnCollide(c.collider);
    }

    public override void OnTriggerEnter2D(Collider2D c) {
    }

    public override void OnCollisionExit2D(Collision2D c) {
        NpcController.OnExitCollide(c.collider);
    }

    public override void OnTriggerExit2D(Collider2D c) {
    }
}