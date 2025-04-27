using UnityEngine;

class Rifle : Weapon
{
    public Rifle() : base("Rifle", 30) 
    {
    }

    public override void Shoot()
    {
        if (IsAmmoEmpty())
        {
            Debug.Log($"{Name} is empty! Reload needed.");
        }
        else
        {
            CurrentAmmo--;
            Debug.Log($"{Name} fired! Bullets left: {CurrentAmmo}");
        }
    }

    public override void Reload()
    {
        Debug.Log($"{Name} reloading...");
        CurrentAmmo = MagazineSize;
    }
}

// ideas e.g.

// maybe : héritage imbriquer ? (abstract class Rifle : Weapon) 
    // constructeur e.g. : public Rifle(string name, int magazineSize) : base(name, magazineSize) { }
// or (intermédiaire) : abstract class auto/semiAuto : Weapon ?

// +++ 

// public class AK74 : Rifle
// {
//     public AK74() : base("AK-74", 30) { }
// }
