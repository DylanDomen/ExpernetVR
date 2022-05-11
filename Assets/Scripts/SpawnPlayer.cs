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
    // Start is called before the first frame update
    void Start()
    {

        // ? faire : v?rifier que le joueur est connect? avant d'instancier
        // ? faire v?rifier que le player est pas null
        Vector3 randomSpawnPosition = new Vector3(Random.Range(-3, 3), 0.5f, Random.Range(-10, -5));
        GameObject xrorigin = PhotonNetwork.Instantiate(player.name, randomSpawnPosition, Quaternion.identity);
        xrorigin.name = "XR Origin";
        GameObject.Find("Locomotion System").GetComponent<LocomotionSystem>().xrOrigin = xrorigin.GetComponent<XROrigin>();

        if (xrorigin.gameObject.GetPhotonView().IsMine == false)
        {
            xrorigin.gameObject.GetComponent<XROrigin>().enabled = false;
        }
    }

    /*private void Awake()
    {
        Vector3 randomSpawnPosition = new Vector3(Random.Range(-3, 3), 0.5f, Random.Range(-10, -5));
        PhotonNetwork.Instantiate(player.name, randomSpawnPosition, Quaternion.identity);
    }*/

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
