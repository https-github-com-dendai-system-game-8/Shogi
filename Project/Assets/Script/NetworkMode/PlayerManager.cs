using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using UnityEngine.UI;
using System;

public class PlayerManager : MonoBehaviourPunCallbacks, IPunObservable
{
    private Text log;
    private Text pubLog;
    private Player player;
    public int myNumber;
    public PieceStatus[] pieceStatus;
    private PiecesMoveNet moveNet;
    private bool playing = false;
    private float tp;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(PhotonNetwork.PlayerList[0] == PhotonNetwork.LocalPlayer);
        log = GameObject.Find("P1Log").GetComponent<Text>();//�s���̃��O
        pubLog = GameObject.Find("Log").GetComponent<Text>();//�����̃f�[�^
        player = PhotonNetwork.LocalPlayer;//����
        moveNet = FindObjectOfType<PiecesMoveNet>();
        if (PhotonNetwork.PlayerList[0] == player)//�����̔ԍ������߂�
            myNumber = -1;
        else
            myNumber = 1;
        pieceStatus = moveNet.pieceStatus;//��̃f�[�^

        if (photonView.IsMine)
        {
            log.text = Convert.ToString(myNumber);//�����̔ԍ���\��
            pubLog.text = RoomManager.roomName;//�����̖��O���Ђ傤��


            GameObject camera, camera2;
            if (myNumber == 1)
            {
                camera = GameObject.FindGameObjectWithTag("MainCamera");
                camera2 = GameObject.FindGameObjectWithTag("SubCamera");
            }
            else
            {
                camera = GameObject.FindGameObjectWithTag("SubCamera");
                camera2 = GameObject.FindGameObjectWithTag("MainCamera");
            }
            if (camera.activeInHierarchy)
                camera.SetActive(false);
            if (!camera2.activeInHierarchy)
                camera2.SetActive(true);
        }
        
    }
    private void Update()
    {
        var players = PhotonNetwork.PlayerList;
        
        //�v���C���[����l���������J�n
        if(players.Length == 2 && !playing)
        {
            playing = true;
            moveNet.isCanTouch = true;
        }
        else if(players.Length != 2)
        {
            moveNet.isCanTouch = false;
            playing = false;
        }
        if (players[0] == player)
            myNumber = -1;
        else
            myNumber = 1;
        //log.text = Convert.ToString(myNumber);
    }

    // Update is called once per frame
    
    void IPunObservable.OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            Debug.Log("���M���܂���");
            // Transform�̒l���X�g���[���ɏ�������ő��M����
            for(int i = 0;i < pieceStatus.Length; i++)
            {
                stream.SendNext(pieceStatus[i].pieceID);
                stream.SendNext(pieceStatus[i].piecePoint);
                stream.SendNext(pieceStatus[i].type);
                stream.SendNext(pieceStatus[i].promotionType);
                stream.SendNext(pieceStatus[i].player);
                stream.SendNext(pieceStatus[i].transform.localPosition);
            }
        }
        else
        {
            // ��M�����X�g���[����ǂݍ����Transform�̒l���X�V����
            Debug.Log("��M���܂���");

            for(int i = 0; i < pieceStatus.Length; i++)
            {
                int tmpi = (int)stream.ReceiveNext();
                for(int j = 0; j < pieceStatus.Length; j++)
                {
                    if (pieceStatus[j].pieceID == tmpi)
                    {
                        int tmptype = pieceStatus[j].type;
                        if (pieceStatus[j].player != myNumber)
                            pieceStatus[j].piecePoint = (float)stream.ReceiveNext();
                        else
                            tp = (float)stream.ReceiveNext();
                        pieceStatus[j].type = (int)stream.ReceiveNext();
                        pieceStatus[j].promotionType = (int)stream.ReceiveNext();
                        pieceStatus[j].player = (int)stream.ReceiveNext();
                        pieceStatus[j].transform.localPosition = (Vector3)stream.ReceiveNext();
                        pieceStatus[j].PieceInitialize();
                        if (pieceStatus[j].type != tmptype)
                        {
                            SpriteRenderer pieceSprite = pieceStatus[j].gameObject.GetComponent<SpriteRenderer>();
                            (pieceStatus[j].promotionSprite, pieceSprite.sprite) = (pieceSprite.sprite, pieceStatus[j].promotionSprite);
                        }
                        pieceStatus[j].piecePosition = new Vector3(Mathf.Round(pieceStatus[j].transform.localPosition.x), Mathf.Round(pieceStatus[j].transform.localPosition.y)) / PiecesMove.gridSize + new Vector3(4, 4);
                    }
                }
            }
            
        }
    }

}
