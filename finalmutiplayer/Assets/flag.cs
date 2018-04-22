using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class flag : NetworkBehaviour
{
    public enum State
    {
        Available,
        Possessed
    };

    //[SyncVar]
    public State m_state = State.Available;
    public GameObject isavailableparticle;

    [SyncVar]
    public GameObject player;

    void Start()
    {
        
        m_state = State.Available;

    }

    private void Update()
    {
        if(m_state == State.Available)
        {
            isavailableparticle.SetActive(true);
        }
        else
        {
            isavailableparticle.SetActive(false);

        }

      
    }
   



        private void OnTriggerStay(Collider other)
    {
       

        if (other.gameObject.tag == "Player" && m_state == State.Available)
        {

            transform.parent = other.transform;
            transform.position = other.transform.position;
            transform.position += new Vector3(0, 3, 0);
            m_state = State.Possessed;
            other.GetComponent<Health>().flag = this.gameObject;
           // player = other.gameObject;
        }
    }
  
  
}
