using UnityEngine;
using System.Collections.Generic;

class Program
{
    static void Main(string[] args)
    {
        List<Weapon> inventory = new List<Weapon>
        {
            new Pistol(),
            new Rifle()
        };

        foreach (var weapon in inventory)
        {
            Debug.Log($"Equipped: {weapon.GetName()}");

            weapon.Shoot();
            weapon.Shoot();
            weapon.Reload();
            weapon.Shoot();
        }

        Debug.Log("All tests done!");
    }
}
