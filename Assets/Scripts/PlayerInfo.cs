using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;
using TMPro;
using ExpernetVR;

public class PlayerInfo : MonoBehaviourPunCallbacks
{
    public TMP_Text textUsername;
    private App gameManager;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").gameObject.GetComponent<App>();
        showPlayerName(gameManager.username);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void showPlayerName(string username)
    {
        PhotonNetwork.NickName = username;
        textUsername.text = PhotonNetwork.NickName;
    }
}
