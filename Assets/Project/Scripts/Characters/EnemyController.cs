using UnityEngine;

public interface IEnemyController : IFighterController, ICollidableController {
    float GetCurrentLife();
    void GlobalMovement();
}

public class EnemyController : FighterController, IEnemyController {
    private readonly IEnemyView _view;

    private bool _chasing;
    private bool _goingHome;
    private bool _inHome;

    private Vector3 _startingPos;
    private Vector3 _moveDelta;

    public EnemyController(IEnemyView view, float maxLife, Vector3 startingPos) : base(view, maxLife) {
        _view = view;
        _startingPos = startingPos;
    }

    public void OnCollide(Collider2D c) {
        if (c.CompareTag("Player")) {
            Damage dmg = new(_view.GetTransform().position, _view.GetDamage(), _view.GetForce());

            c.SendMessage("ReceiveDamage", dmg);
        }

        if (_goingHome) {
            if (c.CompareTag("Home")) _inHome = true;
        }
    }

    public void OnExitCollide(Collider2D c) {
        if (c.CompareTag("Home")) _inHome = false;
    }

    public float GetCurrentLife() {
        return Life;
    }

    public void GlobalMovement() {
        if (_chasing) Movement(GameManager.Instance.GetPlayerTransform().position);
        else if (_goingHome) Movement(_startingPos);

        var position = GameManager.Instance.GetPlayerTransform().position;
        _chasing = Vector3.Distance(position, _startingPos) < _view.GetChaseLength() &&
                   Vector3.Distance(position, _startingPos) < _view.GetTriggerLength() &&
                   GameManager.Instance.GetPlayerTransform().gameObject.activeInHierarchy;

        _goingHome = !_chasing && !_inHome;
    }

    private void Movement(Vector3 objective) {
        if (!_chasing && _inHome) return;

        var position = _view.GetTransform().position;
        float x = 0;
        float y = 0;

        if (objective.x > position.x) {
            x = objective.x - _view.GetMovementGap() <= position.x ? 0 : 1;
        }
        else if (objective.x < position.x) {
            x = objective.x + _view.GetMovementGap() >= position.x ? 0 : -1;
        }

        if (objective.y > position.y) {
            y = objective.y - _view.GetMovementGap() <= position.y ? 0 : 1;
        }
        else if (objective.y < position.y) {
            y = objective.y + _view.GetMovementGap() >= position.y ? 0 : -1;
        }

        if (x == -1) _view.GetTransform().localScale = Vector3.one;
        else if (x == 1) _view.GetTransform().localScale = new Vector3(-1, 1, 1);

        _moveDelta = new Vector3(x, y);

        //push
        _moveDelta += PushDirection;
        PushDirection = Vector3.Lerp(PushDirection, Vector3.zero, _view.GetPushRecovery());

        _view.GetTransform().Translate((_moveDelta * Time.deltaTime) / 2);
    }
}