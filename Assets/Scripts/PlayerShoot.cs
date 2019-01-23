using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerShoot : NetworkBehaviour
{
    public WeaponManager wm;
    private Camera cam;
    private float lastShot;
    private AudioSource snd;

    [SerializeField]
    private LayerMask mask;

    public WeaponEffect cameraFX;


    void Start()
    {
        snd = GetComponent<AudioSource>();
        cam = GetComponentInChildren<Camera>();
        if (!cam)
        {
            Debug.Log("No camera attached");
            this.enabled = false;
        }
    }

    private void Update()
    {
        if (wm.currentWeapon)
        {
            if (wm.currentWeapon.isAutomatic)
            {
                if (Input.GetButton("Fire1"))
                {
                    Shoot();
                }
            }
            else
            {
                if (Input.GetButtonDown("Fire1"))
                {
                    Shoot();
                }
            }
        }

    }

    [Client]
    void Shoot()
    {
        if (Time.time >= lastShot + wm.currentWeapon.cooldownBetweenShots)
        {
            snd.Play();
            if (isLocalPlayer)
            {
                CmdShowShoot();
            }



            RaycastHit hit;
            if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, wm.currentWeapon.range, mask))
            {
                if (hit.collider.tag == "Player")
                {
                    CmdPlayerShot(hit.collider.name, wm.currentWeapon.damage);
                }
                if (isLocalPlayer)
                {
                    CmdShowOnHit(hit.point, hit.normal);
                }
            }
            lastShot = Time.time;
        }


    }

    [Command]
    void CmdShowOnHit(Vector3 pos, Vector3 normal)
    {
        RpcShowOnHit(pos, normal);
    }
    [ClientRpc]
    void RpcShowOnHit(Vector3 pos, Vector3 normal)
    {
        GameObject hitfx = wm.weaponHolder.GetComponentInChildren<WeaponEffect>().hitPrefab;
        GameObject parSys = Instantiate(hitfx, pos + Vector3.up * 0.1f, Quaternion.LookRotation(normal));
        Destroy(parSys, 2f);
    }

    [Command]
    void CmdShowShoot()
    {
        RpcShowFX();
    }

    [ClientRpc]
    void RpcShowFX()
    {
        if (!cameraFX.muzzle.isPlaying) { cameraFX.muzzle.Play(true); }
        else { cameraFX.muzzle.Stop(true); cameraFX.muzzle.Play(true); }
        ParticleSystem ps = wm.weaponHolder.GetComponentInChildren<WeaponEffect>().muzzle;
        if (ps.isPlaying) { ps.Stop(true); ps.Play(true); }
        else { ps.Play(true); }
    }

    [Command]
    void CmdPlayerShot(string id, int dmg)
    {
        Player player = GameManager.GetPlayer(id);
        player.RpcTakeDamage(dmg,"Player "+GetComponent<Player>().netId.ToString());
        
    }
}
