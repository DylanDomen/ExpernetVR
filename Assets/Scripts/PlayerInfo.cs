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
        if(this.gameObject.GetComponent<PhotonView>().Owner == PhotonNetwork.LocalPlayer)
        {
            textUsername.text = PhotonNetwork.NickName;
        } else
        {
            textUsername.text = GetComponent<PhotonView>().Owner.NickName;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
