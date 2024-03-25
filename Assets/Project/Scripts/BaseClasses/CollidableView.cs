using UnityEngine;

public interface ICollidableView {
}

public class CollidableView : MonoBehaviour, ICollidableView {
    private ICollidableController _collidableController;

    public ICollidableController CollidableController {
        get { return _collidableController ??= new CollidableController(this); }
    }

    private void OnCollisionEnter2D(Collision2D c) => CollidableController.OnCollide(c.collider);
    private void OnTriggerEnter2D(Collider2D c) => CollidableController.OnCollide(c);

    private void OnCollisionExit2D(Collision2D c) => CollidableController.OnExitCollide(c.collider);
    private void OnTriggerExit2D(Collider2D c) => CollidableController.OnExitCollide(c);
}