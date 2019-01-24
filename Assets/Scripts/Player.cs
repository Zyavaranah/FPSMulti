using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Player : NetworkBehaviour
{
    [SyncVar]
    private bool _isDead;
    public bool isDead
    {
        get { return _isDead; }
        protected set { _isDead = value; }
    }

    [SerializeField]
    private Behaviour[] disableOnDeath;
    private bool[] wasEnabled;

    [SerializeField]
    private int maxHealth = 100;

    [SyncVar]
    public int currentHealth;

    [SyncVar]
    public int killAmount=0;
    [SyncVar]
    private int deathAmount=0;

    [SerializeField]
    private Text hp;
    [SerializeField]
    public Text kills;
    [SerializeField]
    private Text deaths;
    [SerializeField]
    public Text matchResult;

    public void Setup()
    {
        killAmount = 0;
        deathAmount = 0;
        wasEnabled = new bool[disableOnDeath.Length];
        for (int i = 0; i < wasEnabled.Length; i++)
        {
            wasEnabled[i] = disableOnDeath[i].enabled;
        }
        SetDefaults();
    }


    [ClientRpc]
    public void RpcTakeDamage(int dmg,string from)
    {
        if (isDead) return;

        currentHealth -= dmg;
        hp.text = currentHealth.ToString();
        Debug.Log(transform.name + " Health: " + currentHealth + "/" + maxHealth);
        if (currentHealth <= 0)
        {
            hp.text = "0";
            GameManager.GetPlayer(from).killAmount++;
            GameManager.GetPlayer(from).kills.text = GameManager.GetPlayer(from).killAmount.ToString();
            GameManager.GameOver();
            Die();
        }
    }

    [ClientRpc]
    public void RpcHeal(int amount)
    {
        if (isDead) return;

        if (currentHealth + amount < maxHealth)
        {
            currentHealth += amount;
        }
        else{
            currentHealth = maxHealth;
        }
        hp.text = currentHealth.ToString();
        Debug.Log(transform.name + " Health: " + currentHealth + "/" + maxHealth);

    }

    IEnumerator Respawn()
    {
        yield return new WaitForSeconds(GameManager.instance.matchSettings.respawnTime);
        SetDefaults();
        Transform respawnPos = NetworkManager.singleton.GetStartPosition();
        transform.position = respawnPos.position;
    }

    private void Die()
    {
        isDead = true;
        deathAmount++;
        deaths.text = deathAmount.ToString();
        for (int i = 0; i < disableOnDeath.Length; i++)
        {
            disableOnDeath[i].enabled = false;
        }
        Collider col = GetComponent<Collider>();
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        if (col)
        {
            col.enabled = false;
        }
        Renderer[] renderers = GetComponentsInChildren<Renderer>();
        for (int i = 0; i < renderers.Length; i++)
        {
            renderers[i].enabled = false;
        }
        StartCoroutine(Respawn());
    }

    public void SetDefaults()
    {
        matchResult.text = "";
        isDead = false;
        currentHealth = maxHealth;
        hp.text = currentHealth.ToString();
        deaths.text = deathAmount.ToString();
        kills.text = killAmount.ToString();
        for (int i = 0; i < disableOnDeath.Length; i++)
        {
            disableOnDeath[i].enabled = wasEnabled[i];
        }
        Collider col = GetComponent<Collider>();
        if (col)
        {
            col.enabled = true;
        }
        Renderer[] renderers = GetComponentsInChildren<Renderer>();
        for (int i = 0; i < renderers.Length; i++)
        {
            renderers[i].enabled = true;
        }
    }
}
