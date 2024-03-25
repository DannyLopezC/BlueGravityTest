using UnityEngine;

public interface INpcController : ICollidableController {
}

public class NpcController : CollidableController, INpcController {
    private readonly INpcView _view;

    private static Dialogue _firstDialogue;
    private static Dialogue _goodbyeDialogue;

    public NpcController(INpcView view) : base(view) {
        _view = view;
    }

    public override void OnCollide(Collider2D c) {
        if (c.CompareTag("Player")) {
            TriggerDialogue();
        }
    }

    public override void OnExitCollide(Collider2D c) {
        throw new System.NotImplementedException();
    }

    private static void TriggerDialogue() {
        DialogueManager.Instance.StartDialogue(_firstDialogue);
    }
}