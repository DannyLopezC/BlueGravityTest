public interface ICollectableView : ICollidableView {
}

public class CollectableView : CollidableView, ICollectableView {
    private ICollectableController _collectableController;

    public ICollectableController CollectableController {
        get { return _collectableController ??= new CollectableController(this); }
    }
}