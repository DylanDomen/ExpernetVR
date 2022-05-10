using System.Collections;
using UnityEngine;
using Photon.Realtime;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using UnityEngine.Networking;
using ExpernetVR;
using Newtonsoft.Json.Linq;

public class LauncherScript : MonoBehaviourPunCallbacks
{
    public Button buttonConnection;
    public TMP_Text feedbackText;
    public bool isConnecting;
    public byte maxPlayerPerRoom = 10;
    public Canvas connectionCanvas;
    public Canvas roomsListCanvas;
    public string getRoomURL = "http://91.121.171.150:8080/rooms";
    public string authenticateURL = "http://91.121.171.150:8080/authenticate";

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void Awake()
    {
        //si il ya plusieurs personne dans la room ?a va synchro la sc?ne du premier joueur avec les autres
        PhotonNetwork.AutomaticallySyncScene = true;

        //StartCoroutine(getRooms());
    }

    public void Connect()
    {
        StartCoroutine(sendConnectionRequest());
    }

    public IEnumerator sendConnectionRequest()
    {
        string json = "{\"username\":\"seb\",\"password\": \"seb\"}";

        UnityWebRequest request = new UnityWebRequest(authenticateURL, "POST");
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if(request.result == UnityWebRequest.Result.ProtocolError || request.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.LogError("Connection error : " + request.error);
        }
        else
        {
            Debug.Log("Successfully connected");

            var res = request.downloadHandler.text;

            ExpernetVR.AuthenticateResponse response = Newtonsoft.Json.JsonConvert.DeserializeObject<ExpernetVR.AuthenticateResponse>(json);

            App.jwt = response.token;

            connectionCanvas.enabled = false;
            roomsListCanvas.enabled = true;

            StartCoroutine(getRooms(App.jwt));
        }

    }

    public void connectToRoom()
    {
        // we want to make sure the log is clear everytime we connect, we might have several failed attempted if connection failed.
        feedbackText.text = "";

        // keep track of the will to join a room, because when we come back from the game we will get a callback that we are connected, so we need to know what to do then
        isConnecting = true;

        buttonConnection.interactable = false;

        // we check if we are connected or not, we join if we are , else we initiate the connection to the server.
        if (PhotonNetwork.IsConnected)
        {
            LogFeedback("Joining Room...");
            // #Critical we need at this point to attempt joining a Random Room. If it fails, we'll get notified in OnJoinRandomFailed() and we'll create one.
            PhotonNetwork.JoinRandomRoom();
        }
        else
        {
            LogFeedback("Connecting...");

            // #Critical, we must first and foremost connect to Photon Online Server.
            PhotonNetwork.ConnectUsingSettings();
        }

    }

    void LogFeedback(string message)
    {
        // we do not assume there is a feedbackText defined.
        if (feedbackText == null)
        {
            return;
        }

        // add new messages as a new line and at the bottom of the log.
        feedbackText.text += System.Environment.NewLine + message;
    }

    //callback====================================================================================================


    //se d?clenche lorque la conenction au serveur ? ?t? effectu?
    public override void OnConnectedToMaster()
    {
        if (isConnecting)
        {
            LogFeedback("OnConnectedToMaster: Next -> try to Join Random Room");
            Debug.Log("PUN Basics Tutorial/Launcher: OnConnectedToMaster() was called by PUN. Now this client is connected and could join a room.\n Calling: PhotonNetwork.JoinRandomRoom(); Operation will fail if no room found");

            // #Critical: The first we try to do is to join a potential existing room. If there is, good, else, we'll be called back with OnJoinRandomFailed()
            PhotonNetwork.JoinRandomRoom();
        }
    }

    //si on arrive pas a join une random room
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        LogFeedback("<Color=Red>OnJoinRandomFailed</Color>: Next -> Create a new Room");
        Debug.Log("PUN Basics Tutorial/Launcher:OnJoinRandomFailed() was called by PUN. No random room available, so we create one.\nCalling: PhotonNetwork.CreateRoom");

        // #Critical: we failed to join a random room, maybe none exists or they are all full. No worries, we create a new room.
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = this.maxPlayerPerRoom });
    }

    
    // permet de savoir si un joueur est d?co ou ? ?t? d?co
    
    public override void OnDisconnected(DisconnectCause cause)
    {
        LogFeedback("<Color=Red>OnDisconnected</Color> " + cause);
        Debug.LogError("PUN Basics Tutorial/Launcher:Disconnected");
        isConnecting = false;
        buttonConnection.interactable = true;
    }

    //connection ? la room c'est bien pass? et que le jouer c'est connect? au jeu
    public override void OnJoinedRoom()
    {
        LogFeedback("<Color=Green>OnJoinedRoom</Color> with " + PhotonNetwork.CurrentRoom.PlayerCount + " Player(s)");
        Debug.Log("PUN Basics Tutorial/Launcher: OnJoinedRoom() called by PUN. Now this client is in a room.\nFrom here on, your game would be running.");

        // #Critical: We only load if we are the first player, else we rely on  PhotonNetwork.AutomaticallySyncScene to sync our instance scene.
        if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
        {
            Debug.Log("We load the 'Room for 1' ");

            // #Critical
            // Load the Room Level. 
            PhotonNetwork.LoadLevel("MyRoom");
        }
    }

    private IEnumerator getRooms(string jwt)
    {
        Debug.Log("Get rooms");
        Debug.Log(jwt);
        using (UnityWebRequest request = UnityWebRequest.Get(getRoomURL))
        {
            yield return request.SendWebRequest();

            if(request.result == UnityWebRequest.Result.ProtocolError || request.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.LogError("Get rooms error : " + request.error);
            }
            else
            {
                Debug.Log("Rooms succesfully retrieved");

                var json = request.downloadHandler.text;

                ExpernetVR.Room[] rooms = Newtonsoft.Json.JsonConvert.DeserializeObject<ExpernetVR.Room[]>(json);

                foreach (ExpernetVR.Room room in rooms)
                {
                    Debug.Log(room.name);
                }
            }
        }

    }

    //callback====================================================================================================
}
