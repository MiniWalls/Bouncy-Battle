/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using Photon.Realtime;

[System.Serializable]
public class ProfileData //Used for storing Username and total kills.
{
    public string username;
    public int kills;

    public ProfileData()
    {
        this.username = "DEFAULT";
        this.kills = 0;
    }

    public ProfileData(string u, int k)
    {
        this.username = u;
        this.kills = k;
    }
}

public class Launcher : MonoBehaviourPunCallbacks
{
    public InputField usernameField;
    public InputField roomnameField;
    public Text killsText;
    public static ProfileData profile = new ProfileData();

    public GameObject _Main;
    public GameObject _Rooms;
    public GameObject _Create;

    public GameObject RoomButtonPrefab;

    private List<RoomInfo> roomList;

    public void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true; //On opening game sync scene and connect to server

        profile = Data.LoadProfile();
        usernameField.text = profile.username;
        killsText.text = "Total Kills: " + profile.kills;

        Connect();
    }

    public override void OnConnectedToMaster() //After establishing connection 
    {
        Debug.Log("Conencted");

        PhotonNetwork.JoinLobby();
        base.OnConnectedToMaster();
    }

    public override void OnJoinedRoom() //When joined a room start game
    {
        StartGame();

        base.OnJoinedRoom();
    }

    public override void OnJoinRandomFailed(short returnCode, string message) //Incase no rooms, create room.
    {
        Create();

        base.OnJoinRandomFailed(returnCode, message);
    }

    public void Connect() //Connects with game version and settings
    {
        Debug.Log("trying connect");
        PhotonNetwork.GameVersion = "0.1";
        PhotonNetwork.ConnectUsingSettings();
    }

    public void Join() //Joins random room
    {
        PhotonNetwork.JoinRandomRoom();
    }

    public void Create() //Creates room
    {
        RoomOptions options = new RoomOptions();
        options.MaxPlayers = 6; //Max 6 players in a room

        ExitGames.Client.Photon.Hashtable properties = new ExitGames.Client.Photon.Hashtable(); //Creates properties variable for room
        properties.Add("map", 0);
        options.CustomRoomProperties = properties;

        PhotonNetwork.CreateRoom(roomnameField.text, options);
    }

    public void ChangeMap()
    {
        Debug.Log("Map Changed");
    }

    public void StartGame()
    {
        if (string.IsNullOrEmpty(usernameField.text))
        {
            profile.username = "PLAYER_" + Random.Range(100, 10000);
        }
        else
        {
            profile.username = usernameField.text; //Set profile username to input field.
        }

        if (PhotonNetwork.CurrentRoom.PlayerCount == 1) //Loads scene/level if they are first player to join.
        {
            Data.SaveProfile(profile);
            PhotonNetwork.LoadLevel(1);
        }
    }

    private void ClearRoomList()
    {
        Transform content = _Rooms.transform.Find("Scroll View/Viewport/Content");
        foreach (Transform t in content) Destroy(t.gameObject);
    }

    public override void OnRoomListUpdate(List<RoomInfo> p_list) //Photon calls this function when room list gets updated
    {
        roomList = p_list;
        ClearRoomList();

        Transform content = _Rooms.transform.Find("Scroll View/Viewport/Content"); //Gets all content (buttons) from scroll view 
        foreach (RoomInfo i in roomList)
        {
            GameObject newRoomButton = Instantiate(RoomButtonPrefab, content) as GameObject;

            newRoomButton.transform.Find("Name").GetComponent<Text>().text = i.Name;
            newRoomButton.transform.Find("Players").GetComponent<Text>().text = i.PlayerCount + " / " + i.MaxPlayers;

            //newRoomButton.GetComponent<Button>().onClick.AddListener(delegate { Debug.Log("test"); });
            //newRoomButton.GetComponent<Button>().onClick.AddListener(delegate { JoinRoom(newRoomButton.transform); }); //Adds on click function to button
        }
        base.OnRoomListUpdate(roomList);
    }


    public void JoinRoom(Transform _button)
    {
        string _roomName = _button.transform.Find("Name").GetComponent<Text>().text; //Gets text field named "Name" and gets text from the text field
        PhotonNetwork.JoinRoom(_roomName); //Joins room with the name gotten from button
    }

    public void CloseAll()
    {
        _Main.SetActive(false);
        _Rooms.SetActive(false);
        _Create.SetActive(false);
    }

    public void OpenMain()
    {
        CloseAll();
        _Main.SetActive(true);
    }

    public void OpenRooms()
    {
        CloseAll();
        _Rooms.SetActive(true);
    }

    public void OpenCreate()
    {
        CloseAll();
        _Create.SetActive(true);
    }

}
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using Photon.Realtime;

[System.Serializable]
public class ProfileData //Used for storing Username and total kills.
{
    public string username;
    public int kills;
    public int wins;

    public ProfileData()
    {
        this.username = "DEFAULT";
        this.kills = 0;
        this.wins = 0;
    }

    public ProfileData(string u, int k, int w)
    {
        this.username = u;
        this.kills = k;
        this.wins = w;
    }
}

[System.Serializable]
public class MapData
{
    public string name;
    public int scene;
}

public class Launcher : MonoBehaviourPunCallbacks
{
    public InputField usernameField;
    public InputField roomnameField;
    public Text killsText;
    public Text mapText;
    public static ProfileData profile = new ProfileData();

    public GameObject _Main;
    public GameObject _Rooms;
    public GameObject _Create;

    public GameObject RoomButtonPrefab;

    public MapData[] maps;
    private int currentmap = 0;

    private List<RoomInfo> roomList;

    public void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true; //On opening game sync scene and connect to server

        profile = Data.LoadProfile();
        usernameField.text = profile.username;
        killsText.text = "Total Kills: " + profile.kills;

        Connect();
    }

    public override void OnConnectedToMaster() //After establishing connection 
    {
        Debug.Log("Connected");

        PhotonNetwork.JoinLobby();
        base.OnConnectedToMaster();
    }

    public override void OnJoinedRoom() //When joined a room start game
    {
        StartGame();

        base.OnJoinedRoom();
    }

    public override void OnJoinRandomFailed(short returnCode, string message) //Incase no rooms, create room.
    {
        Create();

        base.OnJoinRandomFailed(returnCode, message);
    }

    public void Connect() //Connects with game version and settings
    {
        Debug.Log("trying to connect");
        PhotonNetwork.GameVersion = "0.0";
        PhotonNetwork.ConnectUsingSettings();
    }

    public void Join() //Joins random room
    {
        PhotonNetwork.JoinRandomRoom();
    }

    public void Create() //Creates room
    {
        RoomOptions options = new RoomOptions();
        options.MaxPlayers = 6; //Max 6 players in a room
        options.CustomRoomPropertiesForLobby = new string[] { "map" };

        ExitGames.Client.Photon.Hashtable properties = new ExitGames.Client.Photon.Hashtable(); //Creates properties variable for room
        properties.Add("map", currentmap);
        options.CustomRoomProperties = properties;

        PhotonNetwork.CreateRoom(roomnameField.text, options);
    }

    public void CreateRandom() //Creates room with random map and name
    {
        RoomOptions options = new RoomOptions();
        options.MaxPlayers = 6; //Max 6 players in a room

        ExitGames.Client.Photon.Hashtable properties = new ExitGames.Client.Photon.Hashtable(); //Creates properties variable for room
        int randomMap = Random.Range(1, 1);
        properties.Add("map", randomMap); //Chooses random map
        options.CustomRoomProperties = properties;

        PhotonNetwork.CreateRoom("", options);
    }

    public void ChangeMap()
    {
        currentmap++;
        if (currentmap >= maps.Length)
        {
            currentmap = 0;
        }
        mapText.text = "MAP: " + maps[currentmap].name;
    }

    public void StartGame() //Starts game
    {
        if (string.IsNullOrEmpty(usernameField.text)) //If username field is empty give random name
        {
            profile.username = "PLAYER_" + Random.Range(100, 10000);
        }
        else
        {
            profile.username = usernameField.text; //Set profile username to input field.
        }

        if (PhotonNetwork.CurrentRoom.PlayerCount == 1) //Loads scene/level if they are first player to join.
        {
            Data.SaveProfile(profile); //Might be wrong place (place outside if)
            PhotonNetwork.LoadLevel(maps[currentmap].scene);
        }
    }

    private void ClearRoomList()
    {
        Transform content = _Rooms.transform.Find("Scroll View/Viewport/Content");
        foreach (Transform t in content) Destroy(t.gameObject);
    }

    public override void OnRoomListUpdate(List<RoomInfo> p_list) //Photon calls this function when room list gets updated, give list of room info as argument
    {
        roomList = p_list;
        ClearRoomList();
        Debug.Log("Room list emptied");

        Transform content = _Rooms.transform.Find("Scroll View/Viewport/Content"); //Gets transform of content, aka GO with vertical layout group

        foreach (RoomInfo i in roomList)
        {
            GameObject newRoomButton = Instantiate(RoomButtonPrefab, content);
            Debug.Log("new room button created");
            if (newRoomButton != null)
            { 
                newRoomButton.transform.Find("Name").GetComponent<Text>().text = i.Name;
                newRoomButton.transform.Find("Players").GetComponent<Text>().text = i.PlayerCount + " / " + i.MaxPlayers;

                if (i.CustomProperties.ContainsKey("map"))
                {
                    newRoomButton.transform.Find("Map/Name").GetComponent<Text>().text = maps[(int)i.CustomProperties["map"]].name;
                }
                else
                {
                    newRoomButton.transform.Find("Map/Name").GetComponent<Text>().text = "";
                }

                //newRoomButton.GetComponent<Button>().onClick.AddListener(delegate { JoinRoom(newRoomButton.transform); }); //Adds on click function to button
            }
        }
        base.OnRoomListUpdate(roomList);
    }

    


    public void JoinRoom(Transform _button) //Joins room with specific name
    {
        string roomName = _button.transform.Find("Name").GetComponent<Text>().text; //Gets text field named "Name" and gets text from the text field
        PhotonNetwork.JoinRoom(roomName); //Joins room with the name gotten from button
    }

    public void CloseAll()
    {
        _Main.SetActive(false);
        _Rooms.SetActive(false);
        _Create.SetActive(false);
    }

    public void OpenMain() //Closes all tabs and Opens Main Menu
    {
        CloseAll();
        _Main.SetActive(true);
    }

    public void OpenRooms() //Closes all tabs and Opens Rooms Menu
    {
        CloseAll();
        _Rooms.SetActive(true);
    }

    public void OpenCreate() //Closes all tabs and Opens Create Menu
    {
        CloseAll();
        _Create.SetActive(true);

        roomnameField.text = "";

        currentmap = 0;
        mapText.text = "MAP: " + maps[currentmap].name;
    }

}
