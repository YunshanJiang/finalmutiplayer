using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
public class item2 : NetworkBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            
                other.GetComponent<player>().item = 2;
                other.GetComponent<player>().rotationspeed = 180.0f;
                other.GetComponent<player>().walkspeed = 5.0f;
                other.GetComponent<Renderer>().material.color = this.GetComponent<Renderer>().material.color;
            if (isServer)
                Rpcchangematerial(other.gameObject);
                 //   Cmdchangematerial(other.gameObject);
                Destroy(gameObject);
          

        }
    }

    [ClientRpc]
    void Rpcchangematerial(GameObject player)
    {
        player.GetComponent<Renderer>().material.color = this.GetComponent<Renderer>().material.color;
      

    }
}
