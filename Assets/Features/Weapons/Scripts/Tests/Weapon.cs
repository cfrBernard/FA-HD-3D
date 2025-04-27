using System;

abstract class Weapon
{
    protected string Name;
    protected int MagazineSize;
    protected int CurrentAmmo;

    public Weapon(string name, int magazineSize)
    {
        Name = name;
        MagazineSize = magazineSize;
        CurrentAmmo = magazineSize;
    }

    public abstract void Shoot();
    public abstract void Reload();

    public bool IsAmmoEmpty()
    {
        return CurrentAmmo == 0;
    }

    public string GetName()
    {
        return Name;
    }

    public int GetCurrentAmmo()
    {
        return CurrentAmmo;
    }

    public int GetMagazineSize()
    {
        return MagazineSize;
    }
}
