using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class gamemanagerment : NetworkBehaviour
{
    public int m_mumPlayers = 4;

    public NetworkManager netmag;

    public GameObject flag;
    public GameObject item1;
    public GameObject item2;



    public GameObject flagposition;

    public Text timer;


    [SyncVar]
    public GameObject lobbywaitplayerUI;


    public Text numofplayerui;

    [SyncVar]
    public GameObject characterUI;


    [SyncVar]
    public GameObject endUi;


    public Text winnerUI;


    public enum hasitem
    {
        has,
        donthave,
    }
    [SyncVar]
    public hasitem respawnitem = hasitem.has;

    [SyncVar]
    private float gametime = 90;
    [SyncVar]
    private int numofplayeringame = 0;

    [SyncVar]
    private GameObject list1 = null;
    [SyncVar]
    private GameObject list2 = null;


    private bool spawning = false;
    public enum CTF_GameState
    {
        WaitingForPlayer,
        Ready,
        selectcharacter,
        Ingame,
        gameend,
    }

    bool IsNumPlayersReached()
    {

        return netmag.numPlayers == m_mumPlayers;
    }
    [SyncVar]
    public CTF_GameState m_gamestate = CTF_GameState.WaitingForPlayer;

    [SyncVar]
    public int colorindex = 0;
    // Use this for initialization
    void Start()
    {
        //lobbywaitplayerUI.SetActive(true);
        respawnitem = hasitem.has;


    }

    // Update is called once per frame
    void Update()
    {

        if (!isServer)
        {
            return;
        }
        if (m_gamestate == CTF_GameState.WaitingForPlayer)
        {
            // m_gamestate = CTF_GameState.Ready;
            //sapwnflag();
            if (netmag.numPlayers != 0)
            {
                numofplayeringame = netmag.numPlayers;
                Rpcchangenumofplayerui(numofplayeringame);
                RpclobbyUI();
            }


        }
        if (m_gamestate == CTF_GameState.Ready)
        {
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
            int cindex = 1;
            for (int a = 0; a < players.Length; a++)
            {
                Rpcupdatecolor(players[a], cindex);
                // a.GetComponent<player>().playercolor = cindex;
                cindex++;
            }

            m_gamestate = CTF_GameState.Ingame;
            //RpccharacterUI();
            RpclobbyUIDisable();
            // selectcharacter
        }
        if (m_gamestate == CTF_GameState.Ingame)
        {


            gametime -= Time.deltaTime;
            Rpcchangetime(gametime);
            if (GameObject.FindGameObjectWithTag("item1") == null && spawning == false)
            {
                spawning = true;
                Invoke("respawn", 7);



                // Rpcrandomgenerateitem(randomposition);
            }

            if (gametime <= 0)
            {
                GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
                int hightestscore = 0;
                int playernum = 0;
                string playercolor = "";
                foreach (GameObject a in players)
                {
                    if (a.GetComponent<player>().score > hightestscore)
                    {
                        hightestscore = a.GetComponent<player>().score;
                        playernum = a.GetComponent<player>().playercolor;
                        switch (playernum)
                        {
                            case 1:
                                playercolor = "red";
                                break;
                            case 2:
                                playercolor = "green";
                                break;
                            case 3:
                                playercolor = "blue";
                                break;
                            case 4:
                                playercolor = "yellow";
                                break;
                        }

                    }
                }

                m_gamestate = CTF_GameState.gameend;
                RpcshowendUI();
                RpcwinnerUI(hightestscore, playernum, playercolor);
            }
        }

    }



    void respawn()
    {
        Vector3 randomposition = new Vector3(Random.Range(-4, 6), 1, Random.Range(7, -10));
        int aorb = Random.Range(0, 2);
        if (aorb == 0)
        {
            GameObject theitem1 = Instantiate(item1, randomposition, Quaternion.identity);
            NetworkServer.Spawn(theitem1);
        }
        else
        {
            GameObject theitem1 = Instantiate(item2, randomposition, Quaternion.identity);
            NetworkServer.Spawn(theitem1);
        }
        spawning = false;
    }

    [ClientRpc]
    void Rpcupdatecolor(GameObject a, int cindex)
    {
        a.GetComponent<player>().playercolor = cindex;
    }

    [Command]
    void Cmdrandomgenerateitem(Vector3 theram)
    {
        GameObject theitem1 = Instantiate(item1, theram, Quaternion.identity);

        NetworkServer.Spawn(theitem1);
        respawnitem = hasitem.has;


    }

    [ClientRpc]
    void Rpcrandomgenerateitem(Vector3 theram)
    {

        GameObject theitem1 = Instantiate(item1, theram, Quaternion.identity);

        NetworkServer.Spawn(theitem1);
        respawnitem = hasitem.has;

    }


    [ClientRpc]
    void RpccharacterUI()
    {
        characterUI.SetActive(true);

    }

    [ClientRpc]
    void RpclobbyUI()
    {
        lobbywaitplayerUI.SetActive(true);
        if (!isServer)
            lobbywaitplayerUI.transform.Find("startbutton").gameObject.SetActive(false);
        else
            lobbywaitplayerUI.transform.Find("waitstart").gameObject.SetActive(false);
    }

    [ClientRpc]
    void RpclobbyUIDisable()
    {
        lobbywaitplayerUI.SetActive(false);
    }
    //winner ui
    [ClientRpc]
    void RpcwinnerUI(int score, int nummer, string color)
    {
        winnerUI.text = "Player" + nummer + " " + color + " win, it gets " + score.ToString() + " score";

    }

    //end ui
    [ClientRpc]
    void RpcshowendUI()
    {
        endUi.SetActive(true);
    }
    //the player number ui
    [ClientRpc]
    void Rpcchangenumofplayerui(int numop)
    {
        numofplayerui.text = "number of player: " + numop.ToString();
    }
    //the timer ui
    [ClientRpc]
    void Rpcchangetime(float thetime)
    {
        timer.text = Mathf.Round(thetime).ToString();
    }
    //spawn the flag
    void sapwnflag()
    {
        //  if(m_gamestate == CTF_GameState.Ready)
        // {
        // m_gamestate = CTF_GameState.Ingame;
        GameObject theflag = Instantiate(flag, flagposition.transform.position, Quaternion.identity);
        Vector3 randomposition = new Vector3(Random.Range(-4, 6), 0.5f, Random.Range(7, -10));
        int aorb = Random.Range(0, 2);


        if (aorb == 0)
        {
            GameObject theitem1 = Instantiate(item1, randomposition, Quaternion.identity);
            NetworkServer.Spawn(theitem1);
        }
        else
        {
            GameObject theitem1 = Instantiate(item2, randomposition, Quaternion.identity);
            NetworkServer.Spawn(theitem1);
        }

        NetworkServer.Spawn(theflag);
        // }
    }


    //switch to select character screen
    public void gotochoosescreen()
    {
        if (!isServer)
        {
            return;
        }
        if (netmag.numPlayers >= 2)
        {
            m_gamestate = CTF_GameState.Ready;
            sapwnflag();
        }
    }



    public void redc()
    {
        if (isServer)
        {



            Rpcdisablethecharacter(0);
        }
        else
        {

            characterUI.transform.GetChild(0).gameObject.SetActive(false);



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
    [ClientRpc]
    public void Rpcrestart()
    {
        SceneManager.LoadScene(0);
    }

}
