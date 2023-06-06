using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using UnityEngine.UI;
using System;

public class PlayerManager : MonoBehaviourPunCallbacks
{
    private Text log;
    private Text pubLog;
    private Player player;
    public int myNumber;
    public GameObject[] pieces;
    public PieceStatusNet[] pieceStatus;
    public PiecesMoveNet moveNet;
    // Start is called before the first frame update
    void Start()
    {
        log = GameObject.Find("P1Log").GetComponent<Text>();
        pubLog = GameObject.Find("Log").GetComponent<Text>();
        player = PhotonNetwork.LocalPlayer;
        if (player.ActorNumber == 1)
            myNumber = 1;
        else if (player.ActorNumber == 2)
            myNumber = -1;
        log.text = Convert.ToString(myNumber);
        pubLog.text = RoomManager.roomName;
        for(int i = 0;i < 20; i++)
        {
            
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
