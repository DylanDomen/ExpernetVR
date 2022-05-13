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
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void OnPlayerEnteredRoom(Player other)
    {
        Debug.Log(other.NickName + " s'est connect? ! ");
        if(other == PhotonNetwork.LocalPlayer)
        {
            textUsername.text = PhotonNetwork.NickName;
        } else
        {
            textUsername.text = other.NickName;
        }
        
    }
}
