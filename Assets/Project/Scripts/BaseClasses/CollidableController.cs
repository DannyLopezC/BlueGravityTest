using UnityEngine;

public interface ICollidableController {
    void OnCollide(Collider2D c);
    void OnExitCollide(Collider2D c);
}

public class CollidableController : ICollidableController {
    private readonly ICollidableView _view;

    public CollidableController(ICollidableView view) {
        _view = view;
    }

    public virtual void OnCollide(Collider2D c) {
    }

    public virtual void OnExitCollide(Collider2D c) {
    }
}