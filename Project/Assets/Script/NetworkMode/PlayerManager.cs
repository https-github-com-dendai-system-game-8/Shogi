using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using UnityEngine.UI;
using System;
using System.Runtime.CompilerServices;
using TMPro;

public class PlayerManager : MonoBehaviourPunCallbacks, IPunObservable
{
    private Text log;
    private Text pubLog;
    private Player player;
    public int myNumber;
    public PieceStatusNet[] pieceStatus;
    private PiecesMoveNet moveNet;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(PhotonNetwork.PlayerList[0]);
        log = GameObject.Find("P1Log").GetComponent<Text>();
        pubLog = GameObject.Find("Log").GetComponent<Text>();
        player = PhotonNetwork.LocalPlayer;
        moveNet = FindObjectOfType<PiecesMoveNet>();
        if (photonView.IsMine)
            myNumber = -1;
        else
            myNumber = 1;
        log.text = Convert.ToString(myNumber);
        pubLog.text = RoomManager.roomName;
        pieceStatus = moveNet.pieceStatus;
        
        GameObject camera;
        if (myNumber == 1)
            camera = GameObject.FindGameObjectWithTag("MainCamera");
        else
            camera = GameObject.FindGameObjectWithTag("SubCamera");
        if(camera.activeInHierarchy)
            camera.SetActive(false);
    }
    private void Update()
    {
        var players = PhotonNetwork.PlayerList;
        if (players.Length != 2)
            moveNet.isCanTouch = false;
        else
            moveNet.isCanTouch = true;
    }

    // Update is called once per frame
    
    void IPunObservable.OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // Transformの値をストリームに書き込んで送信する
            for(int i = 0;i < pieceStatus.Length; i++)
            {
                stream.SendNext(pieceStatus[i].pieceID);
                stream.SendNext(pieceStatus[i].type);
                stream.SendNext(pieceStatus[i].player);
                stream.SendNext(pieceStatus[i].transform.localPosition);
            }
            stream.SendNext(moveNet.turn);
            
        }
        else
        {
            // 受信したストリームを読み込んでTransformの値を更新する
            Debug.Log("受信しました");

            for(int i = 0; i < pieceStatus.Length; i++)
            {
                int tmpi = (int)stream.ReceiveNext();
                for(int j = 0; j < pieceStatus.Length; j++)
                {
                    if (pieceStatus[j].pieceID == tmpi)
                    {
                        pieceStatus[j].type = (int)stream.ReceiveNext();
                        pieceStatus[j].player = (int)stream.ReceiveNext();
                        pieceStatus[j].transform.localPosition = (Vector3)stream.ReceiveNext();
                        pieceStatus[j].CheckMove();
                    }
                }
            }
            moveNet.turn = (int)stream.ReceiveNext();
            Debug.Log(moveNet.turn);
        }
    }



}
