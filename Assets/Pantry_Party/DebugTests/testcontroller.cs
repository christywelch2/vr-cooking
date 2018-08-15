using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class testcontroller : NetworkBehaviour {

    public GameObject ingredient;
    GameObject firstspawn;
    public AuthorityControl authorityControl;
    UnityEvent myEvent = new UnityEvent();
    NetworkIdentity playerID;
    GameObject player;

    //public delegate void GrabIng();
    //public static event GrabIng Grab;

    public void Start()
    {
        //AuthorityControl.Grab += CmdSetAuth;
    }

    private void Update()
    {
        if(!isClient){
            Debug.Log("not local player");
            return;
        }
        if(Input.GetKey(KeyCode.Space)){
            SpawnIng();
            //RollIng();
        }
    }

    void SpawnIng(){
        CmdSpawnIng();
    }

    [Command]
    void CmdSpawnIng()
    {
        Debug.Log("in cmd");
        firstspawn = Instantiate(ingredient, ingredient.transform.position, ingredient.transform.rotation);
        GameObject owner = this.gameObject;
        //instance.GetComponent<Rigidbody>().velocity = instance.transform.forward * 10;
        NetworkServer.Spawn(firstspawn);
        myEvent.Invoke();

    }

    public void RollIng(GameObject a)
    {
        CmdRollIng(a);
    }

    [Command]
    void CmdRollIng(GameObject a)
    {
        GameObject instance = Instantiate(a, ingredient.transform.position, ingredient.transform.rotation);
        //a.SetActive(false);
        instance.GetComponent<Rigidbody>().velocity = instance.transform.forward * 10;
        NetworkServer.Spawn(instance);
        //Destroy(firstspawn);
    }


    [Command]
    public void CmdSetAuth(NetworkInstanceId objectId, NetworkIdentity player, GameObject ing)
    {
        var iObject = NetworkServer.FindLocalObject(objectId);
        var networkIdentity = iObject.GetComponent<NetworkIdentity>();
        var otherOwner = networkIdentity.clientAuthorityOwner;
        Debug.Log("client with authority: " + otherOwner);

        if (otherOwner == player.connectionToClient)
        {
            Debug.Log("inside here");
            return;
        }
        else
        {
            if (otherOwner != null)
            {
                networkIdentity.RemoveClientAuthority(otherOwner);
            }
            networkIdentity.AssignClientAuthority(player.connectionToClient);
            //ing.GetComponent<Rigidbody>().velocity = this.transform.forward * 10;
        }

    }
    //public void OnGrab(GameObject a)//object sender) //InteractableObjectEventArgs e)
    //{
        
    //    foreach (var go in GameObject.FindGameObjectsWithTag("Player"))
    //    {
    //        playerID = go.GetComponent<NetworkIdentity>();
    //        Debug.Log("before");
    //        go.GetComponent<testcontroller>().CmdSetAuth(netId, playerID, a);
    //        Debug.Log("after");

    //    }
    //    //Debug.Log("inside on grab");
    //    //player = GameObject.FindGameObjectWithTag("Player");

    //    //a.GetComponent<Rigidbody>().velocity = this.transform.forward * 10;
    //    //AuthorityControl.Grab -= CmdOnGrab;

    //    var newa = Instantiate(a);
    //    a.GetComponent<MeshRenderer>().material.color = Color.blue;
    //    a.GetComponent<Rigidbody>().velocity = this.transform.forward * 10;
    //    if (NetworkServer.active == true)
    //    {
    //        NetworkServer.Spawn(a);
    //    }
    //}
    //--------------------------------------------------------------------------------------------------------------

    public void clickToMove(){
        Debug.Log("in non command one");

        CmdClickToMove();
    }

    [Command]
    public void CmdClickToMove(){
        Debug.Log("click to move");
        GameObject instance = Instantiate(ingredient, ingredient.transform.position + new Vector3(-3,0,0), ingredient.transform.rotation);
        instance.GetComponent<Rigidbody>().velocity = instance.transform.forward * 10;

        instance.GetComponent<NetworkIdentity>().AssignClientAuthority(this.GetComponent<NetworkIdentity>().connectionToClient);
        NetworkServer.Spawn(instance);
    }
}
