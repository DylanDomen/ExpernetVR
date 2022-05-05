using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;
using TMPro;

public class PlayerInfo : MonoBehaviourPunCallbacks
{
    public TMP_Text textUsername;
    // Start is called before the first frame update
    void Start()
    {
        showPlayerName();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void showPlayerName()
    {
        PhotonNetwork.NickName = "Player " + this.GetComponent<PhotonView>().ViewID;
        textUsername.text = PhotonNetwork.NickName;
    }
}
