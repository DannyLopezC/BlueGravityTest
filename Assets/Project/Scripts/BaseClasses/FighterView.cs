using UnityEngine;

public interface IFighterView {
    Transform GetTransform();
}

public class FighterView : MonoBehaviour, IFighterView {
    [SerializeField] protected float maxLife;

    private IFighterController _fighterController;

    public IFighterController FighterController {
        get { return _fighterController ??= new FighterController(this, maxLife); }
    }

    protected virtual void ReceiveDamage(Damage dmg) {
        FighterController.ReceiveDamage(dmg);

        GameManager.Instance.ShowText($"{dmg.damageAmount} damage", 35, Color.white, transform.position,
            Vector3.up * Random.Range(30, 50), 2f);

        Death();
    }

    public Transform GetTransform() {
        return transform;
    }

    protected virtual void Death() {
        GameManager.Instance.ShowText("Defeated", 100, Color.black, transform.position, Vector3.zero, 3f);
        gameObject.SetActive(false);
    }
}