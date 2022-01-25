using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scoreboard : MonoBehaviour
{
    public GameObject[] players;

    [SerializeField]
    GameObject playerScoreboardItemPrefab;
    [SerializeField]
    Transform playerScoreboardList;

    private static int GreenScore = 0;
    private static int BlueScore = 0;


    private void OnEnable()
    {
        playerController[] players = gameManager.GetAllPlayers();

        foreach(playerController player in players)
        {
            GameObject itemGO =     Instantiate(playerScoreboardItemPrefab, playerScoreboardList);
            PlayerScoreboardItem item = itemGO.GetComponent<PlayerScoreboardItem>();
            if (item != null)
            {
                item.Setup(player.name, player.kills, player.score);
            }
        }
    }

    private void OnDisable()
    {
        foreach(Transform child in playerScoreboardList)
        {
            Destroy(child.gameObject);
        }
    }

    public static void giveScore(string TeamColour, int ScoreAmount)
    {
        if(TeamColour == "Green")
        {
            GreenScore += ScoreAmount;
        }
        if (TeamColour == "Blue")
        {
            BlueScore += ScoreAmount;
        }
    }
}
