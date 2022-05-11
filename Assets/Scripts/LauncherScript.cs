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
    public byte maxPlayerPerRoom = 20;
    public GameObject connectionCanvas;
    public GameObject roomsListCanvas;
    public GameObject roomsListPanel;
    public Button buttonPrefab;
    public string getRoomURL = "http://91.121.171.150:8080/rooms";
    public string authenticateURL = "http://91.121.171.150:8080/authenticate";
    private ExpernetVR.Room[] rooms;

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
        // #Critical, we must first and foremost connect to Photon Online Server.
        PhotonNetwork.ConnectUsingSettings();
    }

    public void Connect()
    {
        StartCoroutine(sendConnectionRequest());
        //connectToRoom();
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

            ExpernetVR.AuthenticateResponse response = Newtonsoft.Json.JsonConvert.DeserializeObject<ExpernetVR.AuthenticateResponse>(res);

            connectionCanvas.SetActive(false);
            roomsListCanvas.SetActive(true);

            StartCoroutine(getRooms(response.token));
        }

    }

    public void connectToRoom(string roomName)
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
            RoomOptions roomOptions = new RoomOptions();
            roomOptions.IsOpen = true;
            roomOptions.IsVisible = true;
            roomOptions.MaxPlayers = maxPlayerPerRoom;

            PhotonNetwork.JoinOrCreateRoom(roomName, roomOptions, null);

        } else
        {
            Debug.LogError("Error not connected to master");
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
            LogFeedback("OnConnectedToMaster");
            Debug.Log("PUN Basics Tutorial/Launcher: OnConnectedToMaster() was called by PUN. Now this client is connected and could join a room.");
        }
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
        Debug.Log("Room joined : " + PhotonNetwork.CurrentRoom.ToString());

        // #Critical: We only load if we are the first player, else we rely on  PhotonNetwork.AutomaticallySyncScene to sync our instance scene.
        if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
        {
            // #Critical
            // Load the Room Level. 
            PhotonNetwork.LoadLevel("MyRoom");
        }
    }

    public override void OnCreatedRoom()
    {
        Debug.Log("Room created: " + PhotonNetwork.CurrentRoom.Name);
    }

    private IEnumerator getRooms(string jwt)
    {
        Debug.Log("Get rooms");

        using (UnityWebRequest request = UnityWebRequest.Get(getRoomURL))
        {
            request.SetRequestHeader("Authorization", "Bearer " + jwt);
            yield return request.SendWebRequest();

            if(request.result == UnityWebRequest.Result.ProtocolError || request.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.LogError("Get rooms error : " + request.error);
            }
            else
            {
                Debug.Log("Rooms succesfully retrieved");

                var json = request.downloadHandler.text;

                rooms = Newtonsoft.Json.JsonConvert.DeserializeObject<ExpernetVR.Room[]>(json);

                foreach (ExpernetVR.Room room in rooms)
                {
                    var button = Instantiate(buttonPrefab, Vector3.zero, Quaternion.identity);
                    button.transform.SetParent(roomsListPanel.transform, false);
                    var text = button.GetComponentInChildren<TextMeshProUGUI>();
                    text.text = room.name;
                    button.GetComponent<Button>().onClick.AddListener(delegate { connectToRoom(room.name); });
                }
            }
        }

    }

    //callback====================================================================================================
}
