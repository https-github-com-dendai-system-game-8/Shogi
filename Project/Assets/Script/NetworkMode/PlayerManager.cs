using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

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
    private int beforePlayerNum = 0;
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
        if (myNumber == 1)
        {
            for (int i = 0; i < pieceStatus.Length; i++)
            {
                for(int j = 0;j < pieceStatus.Length; j++)
                {
                    if (j == i)
                        continue;
                    if (pieceStatus[i].pieceID == -pieceStatus[j].pieceID)
                    {
                        (pieceStatus[i].piecePoint, pieceStatus[j].piecePoint) = (pieceStatus[j].piecePoint, pieceStatus[i].piecePoint);
                    }
                }
            }
        }

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
            pubLog.text = "�ΐ�J�n";
        }
        else if(players.Length != 2)
        {
            moveNet.isCanTouch = false;
            playing = false;
            pubLog.text = "�����҂��Ă��܂�";
        }
        if (players[0] == player)
            myNumber = -1;
        else
            myNumber = 1;
        if (beforePlayerNum > players.Length)
            SceneManager.LoadScene("1StartScene");
        beforePlayerNum = players.Length;
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
                        if (pieceStatus[j].holder != myNumber)
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

    public void GameEndLog(int ouPlayer)
    {
        if (photonView.IsMine)
        {
            if (ouPlayer == myNumber)
            {
                log.text = "YouWin!";
            }
            else
            {
                log.text = "YouLose";
            }
        }
        
    }

}
