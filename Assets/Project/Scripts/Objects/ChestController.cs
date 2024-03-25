using UnityEngine;

public interface IChestController : ICollectableController {
}

public class ChestController : CollectableController, IChestController {
    private readonly IChestView _view;
    private bool _opened;

    public ChestController(IChestView view) : base(view) {
        _view = view;
    }

    public override void OnCollide(Collider2D c) {
        if (!c.CompareTag("Player")) return;

        if (!_opened) {
            _opened = true;
            _view.ChangeSprite(_view.GetOpenedChest());
        }
        else OnCollect(c);
    }

    protected override void OnCollect(Collider2D c) {
        if (!Collected) {
            GameManager.Instance.ShowText($"+{_view.GetMoney()} gold", 50, Color.yellow, _view.GetTransform().position,
                Vector3.up * Random.Range(30, 50), 2f);
            _view.ChangeSprite(_view.GetEmptyChest());
            GameManager.Instance.AddMoney(_view.GetMoney());
        }

        base.OnCollect(c);
    }
}