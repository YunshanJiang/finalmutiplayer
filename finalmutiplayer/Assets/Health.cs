using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;

public class Health : NetworkBehaviour
{

    public const int maxHealth = 100;

    [SyncVar(hook = "OnChangeHealth")]
    public int currentHealth = maxHealth;

    public RectTransform healthBar;

    public Text scoretext;

    private NetworkStartPosition[] spawnPoints;

    private GameObject Levelflag;
    [SyncVar]
    public bool hashag;

    [SyncVar]
    public GameObject flag;



    private int score;
    private void Start()
    {
        if(isLocalPlayer)
        {
            spawnPoints = FindObjectsOfType<NetworkStartPosition>();
            
        }

        
    }

    private void Update()
    {
        if (flag != null && flag.transform.parent != null)
        {
            score = this.GetComponent<player>().score;
            scoretext.text = score.ToString();
            if (isServer)
                RpcOnscorechange(score);
        }
    }


    [ClientRpc]
    void RpcOnscorechange(int score)
    {
        scoretext.text = score.ToString();
    }



    public void TakeDamage(int amount)
    {
        if (!isServer)
        {
            return;
        }

        if(this.GetComponent<player>().item != 1)
        {
            currentHealth -= amount;
        }
        Debug.Log(this.GetComponent<player>().item);
        if (currentHealth <= 0)
        {
            if(flag != null)
           //if(hashag)
            {
              
                Rpcchangeflag();
            }
            currentHealth = maxHealth;

            // called on the Server, but invoked on the Clients
            RpcRespawn();
        }
    }
    [ClientRpc]
    void Rpcchangeflag()
    {

      

        
        Vector3 tempplayerposition = transform.position;
        flag.transform.position = transform.position;

            flag.transform.position = tempplayerposition;
            Invoke("Rpcflagavabile", 2);
           
            flag.transform.parent = null;
       


    }

    [ClientRpc]
    void Rpcflagavabile()
    {
        flag.GetComponent<flag>().m_state = global::flag.State.Available;
        flag = null;
    }
    void OnChangeHealth(int currentHealth)
    {
        healthBar.sizeDelta = new Vector2(currentHealth, healthBar.sizeDelta.y);
    }

    [ClientRpc]
    void RpcRespawn()
    {
      
        if (isLocalPlayer)
        {
            
            // move back to zero location
            Vector3 spawnPoint = Vector3.zero;

            if(spawnPoints !=  null && spawnPoints.Length > 0)
            {
                spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)].transform.position;
            }

            transform.position = spawnPoint;

           
        }
      
    }
    
}