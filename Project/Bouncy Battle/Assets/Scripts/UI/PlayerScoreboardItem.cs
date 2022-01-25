using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScoreboardItem : MonoBehaviour
{
    [SerializeField]
    Text usernameText;
    [SerializeField]
    Text killsText;
    [SerializeField]
    Text scoreText;

    public void Setup(string username, int kills, int score)
    {
        usernameText.text = username;
        killsText.text = "Kills: " + kills;
        scoreText.text = "Score: " + score;
    }

}
