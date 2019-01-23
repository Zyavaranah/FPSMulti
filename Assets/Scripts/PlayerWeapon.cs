using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new Weapon", menuName = "Weapons/New Weapon")]
public class PlayerWeapon : ScriptableObject
{
    public string weaponName = "Gun";
    public int damage = 10;
    public float cooldownBetweenShots = 0.5f;
    public float range = 100;
    public GameObject graphics;
    public bool isAutomatic;
}
