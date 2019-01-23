using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class HealthPack : NetworkBehaviour
{
    public int healAmount;
    private void OnTriggerEnter(Collider other)
    {
        Player player = other.gameObject.GetComponent<Player>();
        if (player)
        {
            GetComponent<AudioSource>().Play();
            player.RpcHeal(healAmount);
            StartCoroutine(RpcCooldown());
            
        }
    }

    IEnumerator RpcCooldown()
    {
        GetComponent<Renderer>().enabled=false;
        GetComponent<Collider>().enabled = false;
        yield return new WaitForSeconds(10);
        GetComponent<Renderer>().enabled = true;
        GetComponent<Collider>().enabled = true;
    }
}
