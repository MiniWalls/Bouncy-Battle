using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class RoomButtonScript : MonoBehaviour
{    public void JoinRoom(Transform _button) //Joins room with specific name
    {
        string roomName = _button.transform.Find("Name").GetComponent<Text>().text; //Gets text field named "Name" and gets text from the text field
        PhotonNetwork.JoinRoom(roomName); //Joins room with the name gotten from button
    }
}
