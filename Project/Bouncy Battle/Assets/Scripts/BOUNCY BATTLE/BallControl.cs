using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class BallControl : MonoBehaviourPunCallbacks
{
    public static Material[] material;
    private int ballBounces = 10;
    public int currentBallBounces = 0;
    public string ballOwner; //The player that shot the ball
    public string teamTag;

    [Header("Collisions")]
    private static string wallBounceTag = "BallCollision";
    private static string teamOneBallTag = "GreenBall";
    private static string teamTwoBallTag = "BlueBall";

    public void DelayedStart()
    {
        StartCoroutine(wait());
        teamTag = transform.tag.ToString();
        photonView.RPC("ChangeBallColour", RpcTarget.All, teamTag);
        
    }

    private IEnumerator wait()
    {
        yield return new WaitForSeconds(15);
        photonView.RPC("Deestroy", GetComponent<PhotonView>().Owner, this.gameObject);
    }

    private void Update()
    {
        if (currentBallBounces >= ballBounces)
        {
            photonView.RPC("Deestroy", GetComponent<PhotonView>().Owner, this.gameObject);
        }
    }
    private void OnCollisionEnter(Collision collision) //Add bounce on collision for point counting
    {
        if (collision.transform.tag == wallBounceTag)
        {
            currentBallBounces += 1;
        }

        if(collision.transform.tag == teamOneBallTag || collision.transform.tag == teamTwoBallTag) //Collision with other balls
        {
            photonView.RPC("Deestroy", GetComponent<PhotonView>().Owner, this.gameObject);
        }
        
        /*if(collision.transform.tag == "Player")
        {
            photonView.RPC("Deestroy", GetComponent<PhotonView>().Owner, gameObject);
        }*/
    }

    [PunRPC]
    public void Deestroy(GameObject obj)
    {
        PhotonNetwork.Destroy(obj);
    }

    [PunRPC]
    public void ChangeBallColour(string tag)
    {
        if (!photonView.IsMine) return;
        Renderer rend = GetComponent<Renderer>();
        rend.enabled = true;
        if (transform.tag == teamOneBallTag) //Material changing broke, using .color instead for now
        {
            rend.material.color = Color.green;
        }
        else if (transform.tag == teamTwoBallTag)
        {
            rend.material.color = Color.blue;
        }
    }

    /*public static void ChangeBallColour(GameObject ball, string teamTag) //Change ball colour to teams colour
    {
        Renderer rend = ball.GetComponent<Renderer>();
        rend.enabled = true;
        if (ball.transform.tag == teamOneBallTag) //Material changing broke, using .color instead for now
        {
            rend.material.color = Color.green;
        }
        else if(ball.transform.tag == teamTwoBallTag)
        {
            rend.material.color = Color.blue;
        }
    }*/
}
