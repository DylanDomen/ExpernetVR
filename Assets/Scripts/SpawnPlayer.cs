using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class SpawnPlayer : MonoBehaviourPunCallbacks
{
    public GameObject player;
    // Start is called before the first frame update
    void Start()
    {

        // � faire : v�rifier que le joueur est connect� avant d'instancier
        // � faire v�rifier que le player est pas null
        Vector3 randomSpawnPosition = new Vector3(Random.Range(-3, 3), 0, Random.Range(-10, -5));
        PhotonNetwork.Instantiate(player.name, randomSpawnPosition, Quaternion.identity);
        PhotonNetwork.NickName = "Player " + player.GetComponent<PhotonView>().ViewID;
        player.GetComponentInChildren<Canvas>().GetComponentInChildren<TextMesh>().text = PhotonNetwork.NickName;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            QuitApplication();
        }
    }

    //lorsque un joeueur se connecte

    public void OnPlayerEnterRoom(Player other)
    {
        Debug.Log(other.NickName + " s'est connect� ! ");
    }

    //lorsque un joueur se d�connecte
    public override void OnPlayerLeftRoom(Player other)
    {
        Debug.Log(other.NickName + " s'est deconnect� ! ");
    }

    //lorsque un joueur quitte la room

    public override void OnLeftRoom()
    {
        SceneManager.LoadScene("Launcher");
    }

    public void QuitApplication()
    {

    }
}
