using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit;
using Unity.XR.CoreUtils;

public class SpawnPlayer : MonoBehaviourPunCallbacks
{
    public GameObject player;
    private GameObject xrorigin;
    public GameObject sphere;
    public GameObject spawnPoint;
    // Start is called before the first frame update
    void Start()
    {

        // ? faire : v?rifier que le joueur est connect? avant d'instancier
        // ? faire v?rifier que le player est pas null
        Vector3 spawnPosition = spawnPoint.transform.position;
        xrorigin = PhotonNetwork.Instantiate(player.name, spawnPosition, Quaternion.identity);
        xrorigin.name = "XR Origin";
        GameObject.Find("Locomotion System").GetComponent<LocomotionSystem>().xrOrigin = xrorigin.GetComponent<XROrigin>();

        Debug.Log("avant" + xrorigin.gameObject.GetPhotonView().IsMine);

        if(PhotonNetwork.IsMasterClient)
        {
            Vector3 randomSpawnPositionSphere1 = new Vector3(Random.Range(-3, 3), 0.5f, Random.Range(-10, -5));
            Vector3 randomSpawnPositionSphere2 = new Vector3(Random.Range(-3, 3), 0.5f, Random.Range(-10, -5));

            PhotonNetwork.InstantiateRoomObject(sphere.name, randomSpawnPositionSphere1, Quaternion.identity);
            PhotonNetwork.InstantiateRoomObject(sphere.name, randomSpawnPositionSphere2, Quaternion.identity);
        }
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

    public override void OnPlayerEnteredRoom(Player other)
    {
        Debug.Log(other.NickName + " s'est connect? ! ");
    }

    //lorsque un joueur se d?connecte
    public override void OnPlayerLeftRoom(Player other)
    {
        Debug.Log(other.NickName + " s'est deconnect? ! ");
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
