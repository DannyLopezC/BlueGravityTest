using UnityEngine;

public interface ICollidableView {
    public void OnCollisionEnter2D(Collision2D c);
    public void OnTriggerEnter2D(Collider2D c);

    public void OnCollisionExit2D(Collision2D c);
    public void OnTriggerExit2D(Collider2D c);
}

public abstract class CollidableView : MonoBehaviour, ICollidableView {
    public abstract void OnCollisionEnter2D(Collision2D c);
    public abstract void OnTriggerEnter2D(Collider2D c);

    public abstract void OnCollisionExit2D(Collision2D c);
    public abstract void OnTriggerExit2D(Collider2D c);
}