using UnityEngine;

class Pistol : Weapon 
{
    public Pistol() : base("Pistol", 12) 
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

// maybe : héritage imbriquer ? (abstract class Pistol : Weapon)
    // constructeur e.g. : public Pistol(string name, int magazineSize) : base(name, magazineSize) { }
// or (intermédiaire) : abstract class auto/semiAuto : Weapon ?

// +++ 

// public class Glock : Pistol
// {
//     public Glock() : base("Glock 17", 17) { }
// }