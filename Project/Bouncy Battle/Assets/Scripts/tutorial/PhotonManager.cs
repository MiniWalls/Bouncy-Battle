using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.Linq;
using Photon.Realtime;

public class PhotonManager : MonoBehaviourPunCallbacks
{
    public Vector3 DogStartPos;
    public Vector3 AlienStartPos;

    void Start()
    {
        ChooseTeam();
    }

    public void ChooseTeam()
    {
        int amount = PhotonNetwork.PlayerList.Length;
        print(amount);
        string rtn = "Dog";

        if (amount % 2 == 0)
        {
            rtn = "Dog";
        }
        else
        {
            rtn = "Alien";
        }
        
        Vector3 StartPos;
        if(rtn == "Dog")
        {
            StartPos = DogStartPos;
        }
        else
        {
            StartPos = AlienStartPos;
        }
        GameObject a = PhotonNetwork.Instantiate("Player " + rtn, StartPos, Quaternion.identity);
        a.GetComponent<placeWall>().Team = rtn;
    }
}
