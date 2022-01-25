using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class Status : MonoBehaviour
{
    private string StatusS = "Status: ";

    [Header("Text")]
    public Text UiText;

    void FixedUpdate()
    {
        UiText.text = StatusS + PhotonNetwork.NetworkClientState;
    }
}
