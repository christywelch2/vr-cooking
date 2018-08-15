using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class AuthorityControl : NetworkBehaviour {

    NetworkIdentity playerID;
    GameObject player;
    UnityEvent myEvent = new UnityEvent();
    public delegate void GrabIng();
    public static event GrabIng Grab;

	void Start () {
        Grab += OnGrab;
	}

    //has event click on it
    public void GonnaGrab(){
        if (Grab != null)
        {
            Grab();
        }
    }

    public void OnGrab()//object sender) //InteractableObjectEventArgs e)
    {
        foreach (var go in GameObject.FindGameObjectsWithTag("Player"))
        {
            playerID = go.GetComponent<NetworkIdentity>();
            Debug.Log("before");
            go.GetComponent<testcontroller>().CmdSetAuth(netId, playerID, this.gameObject);
            go.GetComponent<testcontroller>().RollIng(this.gameObject);
            Debug.Log("after");

        }
    }
}
