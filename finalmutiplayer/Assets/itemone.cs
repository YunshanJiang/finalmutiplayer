using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
public class itemone : MonoBehaviour
{
    private GameObject thegamemanager;

    private void Start()
    {
        thegamemanager = GameObject.FindGameObjectWithTag("gamemanager");
    }
   
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
           // if (other.GetComponent<player>().item == 0)
          //  {
                //zother.GetComponent<player>().m_itemstate = player.itemstate.immue;


                // Rpcchangematerial(other.gameObject);


                //  other.GetComponent<Renderer>().material.color = Color.black;

                // Cmdchangematerial(other.gameObject);
                other.GetComponent<player>().Cmdimmuestatetrigger();
                //Cmdchangematerial();
                Destroy(this.gameObject);
               
           // }
           
        }
    }


  //  [ClientRpc]
    void Rpcchangematerial()
    {
        thegamemanager.GetComponent<gamemanagerment>().respawnitem = gamemanagerment.hasitem.donthave;
    }

   // [Command]
    void Cmdchangematerial()
    {
        thegamemanager.GetComponent<gamemanagerment>().respawnitem = gamemanagerment.hasitem.donthave;
        Rpcchangematerial();
    }
}
