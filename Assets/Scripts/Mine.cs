using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;
using Unity.XR.CoreUtils;
using UnityEngine.InputSystem.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class Mine : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.GetPhotonView().IsMine == false)
        {
            Debug.Log("test" + gameObject.GetComponent<PhotonView>().name);
            gameObject.GetComponent<XROrigin>().enabled = false;
            gameObject.transform.Find("Camera Offset").transform.Find("Main Camera").gameObject.SetActive(false);
            gameObject.transform.Find("Camera Offset").transform.Find("LeftHand Controller").gameObject.GetComponent<ActionBasedController>().enabled = false;
            gameObject.transform.Find("Camera Offset").transform.Find("RightHand Controller").gameObject.GetComponent<ActionBasedController>().enabled = false;
            gameObject.transform.Find("Camera Offset").transform.Find("LeftHand Controller").gameObject.GetComponent<XRRayInteractor>().enabled = false;
            gameObject.transform.Find("Camera Offset").transform.Find("RightHand Controller").gameObject.GetComponent<XRRayInteractor>().enabled = false;
            gameObject.transform.Find("Camera Offset").transform.Find("LeftHand Controller").gameObject.GetComponent<XRInteractorLineVisual>().enabled = false;
            gameObject.transform.Find("Camera Offset").transform.Find("RightHand Controller").gameObject.GetComponent<XRInteractorLineVisual>().enabled = false;
        }
    }
}
