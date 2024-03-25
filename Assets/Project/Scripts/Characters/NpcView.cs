using UnityEngine;

public interface INpcView : ICollidableView {
}

public class NpcView : CollidableView, INpcView {
    private INpcController _npcController;

    public INpcController NpcController {
        get { return _npcController ??= new NpcController(this); }
    }
}