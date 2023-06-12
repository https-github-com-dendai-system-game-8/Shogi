using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using UnityEngine.UI;
using System;
using System.Runtime.CompilerServices;
using TMPro;

public class DataSend : MonoBehaviourPunCallbacks, IPunObservable
{
    private PiecesMoveNet moveNet;
    private PieceStatusNet[] pieceStatus;
    // Start is called before the first frame update
    void Start()
    {
        moveNet = FindObjectOfType<PiecesMoveNet>();
        pieceStatus = moveNet.pieceStatus;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void IPunObservable.OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        
        if (stream.IsWriting)
        {
            // �X�g���[���ɏ�������ő��M����
            for (int i = 0; i < pieceStatus.Length; i++)
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
            // ��M�����X�g���[����ǂݍ���ōX�V����
            Debug.Log("��M���܂���");

            for (int i = 0; i < pieceStatus.Length; i++)
            {
                int tmpi = (int)stream.ReceiveNext();
                for (int j = 0; j < pieceStatus.Length; j++)
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
