using UnityEngine;

public interface INpcController : ICollidableController {
}

public class NpcController : CollidableController, INpcController {
    private readonly INpcView _view;

    private static Dialogue FirstDialogue;
    private static Dialogue GoodbyeDialogue;

    public NpcController(INpcView view) : base(view) {
        _view = view;
    }

    public override void OnCollide(Collider2D c) {
        if (c.CompareTag("Player")) {
            TriggerDialogue();
        }
    }

    private static void TriggerDialogue() {
        DialogueManager.Instance.StartDialogue(FirstDialogue);
    }
}