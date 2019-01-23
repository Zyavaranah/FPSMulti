using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(Player))]
public class PlayerSetup : NetworkBehaviour
{
    [SerializeField]
    private Behaviour[] componentsToDisable;

    public GameObject cameraWeapon;

    [SerializeField]
    private string remoteLayerName = "RemotePlayer";

    [SerializeField]
    private string hideLayerName = "DontShowForLocal";

    [SerializeField]
    private GameObject graphics;

    public GameObject playerUI;

    Camera sceneCam;

    private void Start()
    {

        if (!isLocalPlayer)
        {
            DisableComponents();
            AssignRemoteLayer();
            playerUI.SetActive(false);
        }
        else
        {
            sceneCam = Camera.main;
            if (sceneCam)
            {
                sceneCam.gameObject.SetActive(false);
            }
            SetLayer(graphics,LayerMask.NameToLayer(hideLayerName));
            playerUI.SetActive(true);
        }
        GetComponent<Player>().Setup();
    }

    void SetLayer(GameObject model,LayerMask layer)
    {
        model.layer = layer;
        foreach(Transform child in model.GetComponentInChildren<Transform>())
        {
            SetLayer(child.gameObject, layer);
        }
    }

    public override void OnStartClient()
    {
        base.OnStartClient();

        string id = GetComponent<NetworkIdentity>().netId.ToString();
        Player player = GetComponent<Player>();

        GameManager.RegisterPlayer(id, player);
    }

    void AssignRemoteLayer()
    {
        gameObject.layer = LayerMask.NameToLayer(remoteLayerName);
    }

    private void DisableComponents()
    {
        for (int i = 0; i < componentsToDisable.Length; i++)
        {
            componentsToDisable[i].enabled = false;
        }
        if (cameraWeapon)
        {
            cameraWeapon.SetActive(false);
        }
    }

    private void OnDisable()
    {
        if (sceneCam)
        {
            sceneCam.gameObject.SetActive(true);
        }
        GameManager.UnregisterPlayer(transform.name);
    }
}
