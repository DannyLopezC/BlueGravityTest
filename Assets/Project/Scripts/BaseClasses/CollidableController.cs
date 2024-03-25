using UnityEngine;

public interface ICollidableController {
    public void OnCollide(Collider2D c);
    public void OnExitCollide(Collider2D c);
}

public abstract class CollidableController : ICollidableController {
    private readonly ICollidableView _view;

    public CollidableController(ICollidableView view) {
        _view = view;
    }

    public abstract void OnCollide(Collider2D c);

    public abstract void OnExitCollide(Collider2D c);
}