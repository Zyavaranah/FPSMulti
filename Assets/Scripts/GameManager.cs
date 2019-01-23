using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public MatchSettings matchSettings;

    private void Awake()
    {
        if (!instance) instance = this;
    }

    private static Dictionary<string, Player> players = new Dictionary<string, Player>();

    public static void RegisterPlayer(string netID, Player player)
    {
        string playerID = "Player " + netID;
        players.Add(playerID, player);
        player.transform.name = playerID;
        Cursor.lockState=CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public static void UnregisterPlayer(string playerID)
    {
        players.Remove(playerID);
    }

    public static Player GetPlayer(string playerID)
    {
        return players[playerID];
    }

    public static int GetID(Player p)
    {
        for (int i = 0; i < players.Count; i++)
        {
            if(players["Player " + i] == p)
            {
                return i;
            }
        }
        return -1;
    }

    public static void GameOver()
    {
        for (int i = 1; i <= players.Count; i++)
        {
            if(players["Player " + i].killAmount >= 3){
                players["Player " + i].matchResult.text = "You have won the match!";
                for (int j = 1; j <= players.Count; j++)
                {
                    if(players["Player " + i]!= players["Player " + j])
                    {
                        players["Player " + j].matchResult.text = "You have lost the match!";
                    }
                    Cursor.lockState = CursorLockMode.Confined;
                    Cursor.visible = true;
                    GameManager.instance.Invoke("ReloadScene", 2f);
                }
            }
        }
    }

    void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
