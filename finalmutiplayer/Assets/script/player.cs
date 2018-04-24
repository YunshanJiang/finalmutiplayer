using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
public class player : NetworkBehaviour
{

    public GameObject bulletPrefab;
    public Transform bulletSpawn;
    // [SyncVar]
    public int playercolor;

    [SyncVar]
    public float walkspeed;
    [SyncVar]
    public float rotationspeed;

    public int color;
    [SyncVar]
    public int item = 0;

    public GameObject characterUI;

    private GameObject thegamemanager;

    [SyncVar]
    public int score;
    private float floatscore;
    private float itemtime = 3;
    private bool colroset = false;
    private void Start()
    {
        characterUI = GameObject.FindGameObjectWithTag("characterui");
        walkspeed = 3;
        rotationspeed = 150.0f;
        thegamemanager = GameObject.FindGameObjectWithTag("gamemanager");


    }
    // Update is called once per frame
    void Update()
    {

        if (thegamemanager.GetComponent<gamemanagerment>().m_gamestate != gamemanagerment.CTF_GameState.Ingame)
        {
            return;
        }
        if (isLocalPlayer == false)
        {
            return;
        }

        if (colroset == false)
        {
            resettheplayercolor();
            colroset = true;
        }


        if (GetComponent<Health>().flag != null && GetComponent<Health>().flag.transform.parent != null)
        {
            Cmdchangescore();
        }

        if (item != 0)
        {

            itemtime -= Time.deltaTime;
            if (itemtime <= 0)
            {
                item = 0;
                itemtime = 7;
                Cmdchangematerial();

            }
            if (item == 1)
            {
                this.GetComponent<Renderer>().material.color = Color.black;
                Cmdchangeimmuematerial();
            }
        }

        var x = Input.GetAxis("Horizontal") * Time.deltaTime * rotationspeed;
        var z = Input.GetAxis("Vertical") * Time.deltaTime * walkspeed;

        transform.Rotate(0, x, 0);
        transform.Translate(0, 0, z);


        if (Input.GetKeyDown(KeyCode.Space))
        {

            CmdFire();
        }
    }

    void resettheplayercolor()
    {

        Cmdchangecolor();
    }

    [Command]
    void Cmdchangecolor()
    {

        Rpcchangecolor();
    }

    [ClientRpc]
    void Rpcchangecolor()
    {
        switch (playercolor)
        {
            case 1:
                this.GetComponent<MeshRenderer>().material.color = Color.red;
                break;
            case 2:
                this.GetComponent<MeshRenderer>().material.color = Color.green;
                break;
            case 3:
                this.GetComponent<MeshRenderer>().material.color = Color.blue;
                break;
            case 4:
                this.GetComponent<MeshRenderer>().material.color = Color.yellow;
                break;
            default:
                break;

        }

    }
    [Command]
    void Cmdchangeimmuematerial()
    {

        Rpcchangeimmuematerial();

    }
    [ClientRpc]
    void Rpcchangeimmuematerial()
    {
        this.GetComponent<Renderer>().material.color = Color.black;
    }

    [Command]
    void Cmdchangematerial()
    {

        Rpcchangematerial();
    }

    [ClientRpc]
    void Rpcchangematerial()
    {

        GetComponent<player>().rotationspeed = 150.0f;
        GetComponent<player>().walkspeed = 3.0f;
        resettheplayercolor();
        item = 0;
    }

    [Command]
    void Cmdchangescore()
    {
        floatscore += Time.deltaTime;
        score = (int)Mathf.Round(floatscore);
    }

    [Command]
    void CmdFire()
    {
        // Create the Bullet from the Bullet Prefab
        var bullet = (GameObject)Instantiate(
            bulletPrefab,
            bulletSpawn.position,
            bulletSpawn.rotation);

        // Add velocity to the bullet
        bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * 7;

        NetworkServer.Spawn(bullet);
        // Destroy the bullet after 2 seconds
        Destroy(bullet, 2.0f);
    }

    public override void OnStartAuthority()
    {
        base.OnStartAuthority();


    }

    public override void OnStartLocalPlayer()
    {

        this.GetComponent<MeshRenderer>().material.color = Color.yellow;



    }



    //[Command]
    public void Cmdimmuestatetrigger()
    {


        item = 1;
    }


    public void redc()
    {
        if (isServer)
        {



            Rpcdisablethecharacter(0);
        }
        else
        {
            characterUI = GameObject.FindGameObjectWithTag("characterui");
            characterUI.transform.GetChild(0).gameObject.SetActive(false);


            Cmddisablethecharacter(0);
        }

    }
    public void greenc()
    {

    }
    public void bluec()
    {

    }
    public void yellowc()
    {

    }


    [ClientRpc]
    void Rpcdisablethecharacter(int index)
    {
        characterUI.transform.GetChild(index).gameObject.SetActive(false);

    }


    [Command]
    void Cmddisablethecharacter(int index)
    {
        characterUI = GameObject.FindGameObjectWithTag("characterui");
        Debug.Log(true);
        characterUI.transform.GetChild(index).gameObject.SetActive(false);

    }

}
