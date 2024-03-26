using UnityEngine;
using UnityEngine.Serialization;

public interface IFighterView {
    Transform GetTransform();
    float GetPushRecovery();
}

public class FighterView : MonoBehaviour, IFighterView {
    [SerializeField] protected float _pushRecoverySpeed = 0.2f;
    [SerializeField] protected float maxLife;

    private IFighterController _fighterController;

    public IFighterController FighterController {
        get { return _fighterController ??= new FighterController(this, maxLife); }
    }

    protected virtual void ReceiveDamage(Damage dmg) {
        FighterController.ReceiveDamage(dmg);

        GameManager.Instance.ShowText($"{dmg.damageAmount} damage", 35, Color.white, transform.position,
            Vector3.up * Random.Range(30, 50), 2f);

        if (FighterController.Death()) gameObject.SetActive(false);
    }

    public Transform GetTransform() {
        return transform;
    }

    public float GetPushRecovery() {
        return _pushRecoverySpeed;
    }
}