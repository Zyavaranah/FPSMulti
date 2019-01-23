using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class WeaponManager : NetworkBehaviour
{

    [SerializeField]
    public GameObject weaponHolder;

    [SerializeField]
    PlayerWeapon primaryWeapon;

    public PlayerWeapon currentWeapon;
    
    void Start()
    {
        Equip(primaryWeapon);
    }

    public void Equip(PlayerWeapon playerWeapon)
    {
        if (currentWeapon)
        {
            Destroy(currentWeapon.graphics);
        }
        currentWeapon = playerWeapon;
        GameObject weaponModel = Instantiate(playerWeapon.graphics, weaponHolder.transform);
        if (isLocalPlayer){
            SetLayer(weaponModel, LayerMask.NameToLayer("DontShowForLocal"));
        }
    }

    void SetLayer(GameObject model, LayerMask layer)
    {
        model.layer = layer;
        foreach (Transform child in model.GetComponentInChildren<Transform>())
        {
            SetLayer(child.gameObject, layer);
        }
    }
}
