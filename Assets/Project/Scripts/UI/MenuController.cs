public interface IMenuController {
    void OnBackSkin();
    void OnNextSkin();
    void OnBackWeapon();
    void OnNextWeapon();
}

public class MenuController : IMenuController {
    private readonly IMenuView _view;

    public MenuController(IMenuView view) {
        _view = view;
    }

    public void OnBackSkin() {
        int nextId = 0;

        for (int i = 0; i < GameManager.Instance.GetPlayerClothes().Count; i++) {
            if (GameManager.Instance.GetClothesId() == GameManager.Instance.GetPlayerClothes()[i].id)
                nextId = i - 1;
        }

        //going to last if is the first
        if (nextId < 0) nextId = GameManager.Instance.GetPlayerClothes().Count - 1;

        Clothes backClothes = GameManager.Instance.GetPlayerClothes()[nextId];

        //setting sprites
        _view.SettingClothesSprites(nextId, backClothes);
    }

    public void OnNextSkin() {
        int nextId = 0;

        for (int i = 0; i < GameManager.Instance.GetPlayerClothes().Count; i++) {
            if (GameManager.Instance.GetClothesId() == GameManager.Instance.GetPlayerClothes()[i].id)
                nextId = i + 1;
        }

        //going back to first if is the last
        if (nextId > GameManager.Instance.GetPlayerClothes().Count - 1) nextId = 0;

        Clothes nextClothes = GameManager.Instance.GetPlayerClothes()[nextId];

        //setting sprites
        _view.SettingClothesSprites(nextId, nextClothes);
    }

    public void OnBackWeapon() {
        int nextId = 0;

        for (int i = 0; i < GameManager.Instance.GetPlayerWeapons().Count; i++) {
            if (GameManager.Instance.GetWeaponId() == GameManager.Instance.GetPlayerWeapons()[i].id)
                nextId = i - 1;
        }

        //going to last if is the first
        if (nextId < 0) nextId = GameManager.Instance.GetPlayerWeapons().Count - 1;

        Weapon backWeapon = GameManager.Instance.GetPlayerWeapons().Find(w => w.id == nextId);

        //setting sprites
        _view.SettingWeaponSprites(nextId, backWeapon);
    }

    public void OnNextWeapon() {
        int nextId = 0;

        for (int i = 0; i < GameManager.Instance.GetPlayerWeapons().Count; i++) {
            if (GameManager.Instance.GetWeaponId() == GameManager.Instance.GetPlayerWeapons()[i].id)
                nextId = i + 1;
        }

        //going back to first if is the last
        if (nextId >= GameManager.Instance.GetPlayerWeapons().Count - 1) nextId = 0;

        Weapon nextWeapon = GameManager.Instance.GetPlayerWeapons().Find(w => w.id == nextId);

        //setting sprites
        _view.SettingWeaponSprites(nextId, nextWeapon);
    }
}