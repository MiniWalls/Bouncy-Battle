using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Photon.Pun;

public class gameManager : MonoBehaviour
{
    public string player_prefab;
    public Transform spawn_point;

    private static Dictionary<string, playerController> players = new Dictionary<string, playerController>();

    private void Start()
    {
        Spawn();
    }

    public void Spawn()
    {
        PhotonNetwork.Instantiate(player_prefab, spawn_point.position, spawn_point.rotation);

    }

    public static void RegisterPlayer(string username, string insID, playerController _player)
    {
        string playerID = username + insID;
        players.Add(playerID, _player);

        _player.transform.name = playerID;
    }

    public static void UnRegisterPlayer (string playerID)
    {
        players.Remove(playerID);
    }

    public static playerController GetPlayer(string playerID)
    {
        return players[playerID];
    }

    public static playerController[] GetAllPlayers()
    {
        return players.Values.ToArray();
    }
}
