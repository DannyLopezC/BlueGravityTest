using UnityEngine;

public interface INpcController : ICollidableController {
}

public class NpcController : CollidableController, INpcController {
    private readonly INpcView _view;

    public NpcController(INpcView view) : base(view) {
        _view = view;
    }

    public override void OnCollide(Collider2D c) {
        if (c.CompareTag("Player")) {
            TriggerDialogue();
        }
    }

    public override void OnExitCollide(Collider2D c) {
    }

    private void TriggerDialogue() {
        DialogueManager.Instance.StartDialogue(_view.GetFirstDialogue());
    }
}