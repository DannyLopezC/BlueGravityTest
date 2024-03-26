using UnityEngine;

public interface IFighterController {
    void ReceiveDamage(Damage dmg);
    bool Death();
}

public class FighterController : IFighterController {
    private readonly IFighterView _view;

    //Public fields
    private float _life;

    public float Life {
        get => _life;
        set => _life = Mathf.Clamp(value, value, _maxLife);
    }

    private readonly float _maxLife;

    //Inmunity

    protected float ImmuneTime = 1f;
    protected float LastImmune;

    //push
    protected Vector3 PushDirection;

    public FighterController(IFighterView view, float maxLife) {
        _view = view;
        _maxLife = maxLife;
        Life = _maxLife;
    }

    public virtual void ReceiveDamage(Damage dmg) {
        if (!(Time.time - LastImmune > ImmuneTime)) return;
        LastImmune = Time.time;

        Life -= dmg.damageAmount;

        PushDirection = (_view.GetTransform().position - dmg.origin).normalized * dmg.pushForce;

        if (!(Life <= 0)) return;
        Life = 0;
    }

    public virtual bool Death() {
        if (Life <= 0) {
            GameManager.Instance.ShowText("Defeated", 100, Color.black, _view.GetTransform().position, Vector3.zero,
                3f);
            return true;
        }

        return false;
    }
}