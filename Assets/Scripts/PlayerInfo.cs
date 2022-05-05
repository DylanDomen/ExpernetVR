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
        Debug.Log(this.GetComponent<PhotonView>().ViewID);
        PhotonNetwork.NickName = "Player " + this.GetComponent<PhotonView>().ViewID;
        Debug.Log(PhotonNetwork.NickName);
        //this.GetComponentInChildren<Canvas>().GetComponentInChildren<TextMesh>().text = PhotonNetwork.NickName;
        //this.transform.Find("Canvas").transform.Find("TextUsername").GetComponent<TextMesh>().text = PhotonNetwork.NickName;
        //this.GetComponentInChildren<TextMesh>().text = PhotonNetwork.NickName;
        //this.GetComponentInChildren<this>.GetComponentInChildren<TextMeshPro>().text = PhotonNetwork.NickName;
        //this.transform.GetChild(0).Find("TextUsername").gameObject.GetComponent<TextMeshPro>().text = PhotonNetwork.NickName;
        textUsername.text = PhotonNetwork.NickName;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
