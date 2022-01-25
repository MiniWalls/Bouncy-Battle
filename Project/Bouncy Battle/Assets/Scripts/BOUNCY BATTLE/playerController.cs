using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class playerController : MonoBehaviourPunCallbacks
{

    //For player movement
    private Rigidbody rb;
    private float moveSpeed = 7f;
    private Vector3 moveInput;
    private Vector3 moveVelocity;
    private CapsuleCollider playerCol;
    private float originalHeight;
    private float reducedHeight;

    //Shooting
    public float shootCooldown = 1f;

    [Header("Ball")]
    //public GameObject Ball;
    private Vector3 ballOffset;
    private float ballSpeed = 2500f;

    //For invidual scores
    public int score = 0;
    public int kills = 0;
    public int win = 0;

    private Camera playerCam; //Player specific camera
    public Vector3 cameraOffset;//Variable to store the offset distance between the player and camera
    public GameObject camParent;

    //Animation
    public Animator animator;
    public bool isRunning;


    //Tags
    private string teamOneTag = "GreenBall";
    private string teamTwoTag = "BlueBall";

    //Player ID
    private string insID;
    private string username;
    [HideInInspector]
    public ProfileData playerProfile;

    private PhotonManager pM;

    

    public enum TeamColour //Teams to choose
    {
        GreenBall,
        BlueBall
    }
    public TeamColour team; //Variable for team

    void Start()
    {
        if (!photonView.IsMine) return;  //Used as quick fix for now, calling playercam causes null object ref, fix later.
        playerCam = GetComponentInChildren<Camera>();
        animator = GetComponent<Animator>();
        foreach  (Camera camy in Camera.allCameras)
        {
            if(camy != playerCam)
            {
                camy.gameObject.SetActive(false);
            }
        }

        pM = GameObject.Find("Game Manager").GetComponent<PhotonManager>();

        username = Launcher.profile.username; //Set players username
        camParent.SetActive(photonView.IsMine); //Set camera active if its client's

        if (Camera.main) Camera.main.enabled = false;

        rb = GetComponent<Rigidbody>();
        playerCol = GetComponent<CapsuleCollider>();
        originalHeight = playerCol.height;
        reducedHeight = originalHeight / 2;
        

        insID = GetInstanceID().ToString();
        playerController _player = GetComponent<playerController>(); //playerController is main script for players, hence its used to confirm object is a player.
        
        //PlayerTeamColour(team.ToString());

        var daata = Data.LoadProfile();

        photonView.RPC("SyncProfile", RpcTarget.All, daata.username, daata.kills, daata.wins);
    }


    void Update()
    {
        if (!photonView.IsMine) return;

        shootCooldown -= Time.deltaTime;

        moveInput = new Vector3(Input.GetAxisRaw("Vertical"), 0, Input.GetAxisRaw("Horizontal"));
        moveVelocity = moveInput * moveSpeed; //Check for movement inputs

        Ray cameraRay = playerCam.ScreenPointToRay(Input.mousePosition); //Player rotation
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
        float rayLength;

        if (groundPlane.Raycast(cameraRay, out rayLength))
        {
            Vector3 playerLookAtPoint = cameraRay.GetPoint(rayLength) + new Vector3(-6, 0, 0);
            transform.LookAt(new Vector3(playerLookAtPoint.x, transform.position.y, playerLookAtPoint.z));
        }

        if (Input.GetButtonDown("Fire1") && shootCooldown <= 0f) //Player Shoot
        {
            photonView.RPC("Shoot", RpcTarget.All);
            animator.SetTrigger("Shoot");
        }

        if (Input.GetKeyDown(KeyCode.LeftControl)) //Crouch
        {
            Crouch();
        }
        else if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            UnCrouch();
        }
    }

    private void FixedUpdate()
    {
        if (!photonView.IsMine) return;
        rb.velocity = moveVelocity;
        if (rb.velocity.magnitude > 0)
        {
            isRunning = true;
            animator.SetBool("IsRunning", true);
        }
        else
        {
            isRunning = false;
            animator.SetBool("IsRunning", false);
        }
    }
    private void LateUpdate()
    {
        if (!photonView.IsMine) return;
        playerCam.transform.position = transform.position + cameraOffset; //Set camera transform to players + offset
        playerCam.transform.rotation = Quaternion.Euler(60, 90, 0); //Lock rotation
    }

    private void OnCollisionEnter(Collision collision) //Used for checking collisions with Balls
    {
        if (collision.transform.tag == teamOneTag && team != TeamColour.GreenBall || collision.transform.tag == teamTwoTag && team != TeamColour.BlueBall) 
        {
            var ballScript = collision.transform.GetComponent<BallControl>();
            int pointMultiplier = ballScript.currentBallBounces;
            string CollOwner = ballScript.ballOwner;
            if (pointMultiplier != 0) //If 0 bounces don't give any points and dont kill player
            {
                photonView.RPC("GiveKillCredit", collision.gameObject.GetComponent<PhotonView>().Owner, pointMultiplier);
                var respawn = GetComponent<PlayerRespawn>();
                respawn.spawnPlayer();
            }
            photonView.RPC("Deestroy", collision.gameObject.GetComponent<PhotonView>().Owner, collision.gameObject);
        }
        else if(collision.transform.tag == team.ToString())
        {
            photonView.RPC("Deestroy", collision.gameObject.GetComponent<PhotonView>().Owner, collision.gameObject);
        }
        
    }

    //private void PlayerTeamColour(string teamColour)
    //{
    //    Renderer rend = GetComponent<Renderer>();
    //    if (teamColour == teamOneTag)
    //    {
    //        rend.material.color = Color.green;
    //    }
    //    else if (teamColour == teamTwoTag)
    //    {
    //        rend.material.color = Color.blue;
    //    }
    //}

    [PunRPC]
    public void GiveKillCredit(int pointMulti)
    {
        score += 100 * pointMulti;
        kills += 1;
        SyncProfile(username, kills, win); //Syncs total kills to profile
    }

    [PunRPC]
    private void Shoot()
    {
        if (!photonView.IsMine) return; //Make sure it is our player
        shootCooldown = 1f;
        ballOffset = transform.forward * 2f;
        Quaternion ballRot = Quaternion.Euler(0, transform.rotation.y, 0);

        //New ball
        GameObject newBall = PhotonNetwork.Instantiate("Ball", (transform.position + ballOffset), ballRot);

        //Set ball tag
        string ballTag = team.ToString();
        newBall.transform.tag = ballTag;
         
        //Set ball colour
        //BallControl.ChangeBallColour(newBall, team.ToString());

        //Set the player who shot as owner of the ball
        var ballScript = newBall.GetComponent<BallControl>();
        ballScript.ballOwner = insID;

        //Set rotation
        newBall.transform.rotation = transform.rotation;

        //Give ball speed
        Rigidbody ballRB = newBall.GetComponent<Rigidbody>();
        ballRB.AddRelativeForce(transform.forward * ballSpeed);

        ballScript.DelayedStart();
    }


    void Crouch()
    {
        playerCol.height = reducedHeight;
    }
    void UnCrouch()
    {
        playerCol.height = originalHeight;
    }

    [PunRPC]
    public void SyncProfile(string _username, int _kills, int _wins) //Sync username with other connects
    {
        playerProfile = new ProfileData(_username, _kills, _wins);
        username = playerProfile.username; //Sets username to profile username

    }

    [PunRPC]
    public void Deestroy(GameObject obj)
    {
        PhotonNetwork.Destroy(obj);
    }
}

