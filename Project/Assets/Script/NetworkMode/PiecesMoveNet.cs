using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UIElements;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;

public class PiecesMoveNet : PiecesMove
{
    [SerializeField] private Text playerLogNet;//�v���C���[���Ƃ̃��O
    public PlayerManager[] playerManager;//�����Ƒ���̃f�[�^

    // Start is called before the first frame update
    void Start()
    {
        Initialize();
        PhotonNetwork.ConnectUsingSettings();
    }

    // Update is called once per frame
    void Update()
    {
        if (isCanTouch)
        {
            if (isMoveStage && !isPawnPlay)
            {
                for (int i = 0; i < gridStatus.Length; i++)
                {

                    if (gridStatus[i].isSelect && MoveLimit(i))//�ړ���̑I�����������Ă�����ړ�������
                    {
                        pieces[tmp].transform.localPosition = grid[i].transform.localPosition;//�ړ�������
                        pieceStatus[tmp].isSelect = false;//��̑I��������
                        gridStatus[i].isSelect = false;//�}�X�̑I��������
                        isMoveStage = false;//���̋��I�ׂȂ���Ԃ�����
                        SpriteRenderer pieceSprite = pieces[tmp].GetComponent<SpriteRenderer>();
                        pieceSprite.color = Color.white;//��̐F��߂�
                        Debug.Log("�ړ��I�������");
                        isPromotion = false;
                        foreach (var col in piecesCollider)//��̔����߂�
                        {
                            col.enabled = true;
                        }
                        pieceStatus[tmp].piecePosition = new Vector2(Mathf.Round(pieces[tmp].transform.localPosition.x), Mathf.Round(pieces[tmp].transform.localPosition.y)) / gridSize + new Vector2(4, 4);
                        Debug.Log(pieceStatus[tmp].piecePosition);
                        if (playerManager == null)//�v���C���[�f�[�^���Ȃ��ꍇ��ɓ����
                        {
                            PlayerManager[] tmpPm;
                            tmpPm = FindObjectsOfType<PlayerManager>();
                            playerManager = tmpPm;
                            break;
                                
                            
                        }
                        break;
                    }
                    else if (gridStatus[i].isSelect && !MoveLimit(i))
                    {
                        pieceStatus[tmp].isSelect = false;//��̑I��������
                        gridStatus[i].isSelect = false;//�}�X�̑I��������
                        isMoveStage = false;//���̋��I���ł��Ȃ���Ԃ�����
                        SpriteRenderer pieceSprite = pieces[tmp].GetComponent<SpriteRenderer>();
                        pieceSprite.color = Color.white;//��̐F��߂�
                        Debug.Log("�����͈ړ��ł��܂���\n�ړ����[�h����");
                        foreach (var col in piecesCollider)//��̔����߂�
                        {
                            col.enabled = true;
                        }
                        break;
                    }
                }
            }
            else if (isMoveStage && isPawnPlay)//�������u��
            {
                for (int i = 0; i < gridStatus.Length; i++)
                {
                    if (gridStatus[i].isSelect)
                    {
                        PawnPlay(i);
                        foreach (var col in piecesCollider)//��̔����߂�
                        {
                            col.enabled = true;
                        }
                        SpriteRenderer pieceSprite = pieces[tmp].GetComponent<SpriteRenderer>();
                        pieceSprite.color = Color.white;//��̐F��߂�
                        isMoveStage = false;//�ړ���Ԃ�����
                        isPawnPlay = false;//�������������Ԃ�����
                        pieceStatus[tmp].isSelect = false;//��̑I��������
                        gridStatus[i].isSelect = false;//�}�X�̑I��������
                        break;
                    }
                }
            }
            else//�������Ă��Ȃ����
            {
                for (int i = 0; i < pieceStatus.Length; i++)
                {
                    if (pieceStatus[i].CheckSelected() && pieceStatus[i].piecePosition.y >= 0 && pieceStatus[i].piecePosition.y <= 8)
                    //��̋��I�񂾏ꍇ
                    {
                        isMoveStage = true;//���̋��I���ł��Ȃ�����
                        tmp = i;//�I�����ꂽ��̔ԍ���ۑ�
                        SpriteRenderer pieceSprite = pieces[tmp].GetComponent<SpriteRenderer>();//��̐F��ς������Ԃɂ���
                        pieceSprite.color = Color.gray;//��̐F��ς���
                        foreach (var col in piecesCollider)//��̔��������
                        {
                            col.enabled = false;
                        }
                        Debug.Log("�ړ����[�h�Ɉڍs");
                        break;
                    }
                    else if (pieceStatus[i].CheckSelected())//�������I�񂾏ꍇ
                    {
                        tmp = i;
                        isMoveStage = true;
                        isPawnPlay = true;
                        SpriteRenderer pieceSprite = pieces[tmp].GetComponent<SpriteRenderer>();//��̐F��ς������Ԃɂ���
                        pieceSprite.color = Color.gray;//��̐F��ς���
                        foreach (var col in piecesCollider)//��̔��������
                        {
                            col.enabled = false;
                        }
                        Debug.Log("�ړ����[�h�Ɉڍs");
                        break;
                    }
                }
            }
        }
        bool al = true;
        for (int i = 0; i < pieceStatus.Length; i++)
        {
            pieceStatus[i].piecePosition = new Vector2(Mathf.Round(pieces[i].transform.localPosition.x), Mathf.Round(pieces[i].transform.localPosition.y)) / gridSize + new Vector2(4, 4);
            if (beforePosition[i] != pieceStatus[i].piecePosition && al)//�^�[����؂�ւ���
            {
                Debug.Log("change");
                turn *= -1;
                al = false;
            }
            beforePosition[i] = pieceStatus[i].piecePosition;
            PieceSafe(i);
        }
        for (int i = 0; i < gridStatus.Length; i++)
        {
            GridSafe(i);
        }
        if (Input.GetKeyDown(KeyCode.Escape))//�G�X�P�[�v�������ꂽ��Q�[�������
        {
            Application.Quit();
        }
    }

    private new bool MoveLimit(int gridNumber)//�ړ��\�悩�ǂ������m���߂�
    {
        isCanMove = false;
        StartCoroutine(TestCoroutine(gridNumber));
        for (int i = 0; i < pieceStatus.Length; i++)
        {
            if (pieceStatus[i].type == 30)
            {
                GameEndEffect();
                break;
            }


        }
        return isCanMove;
    }

   

    public new void CheckPromotion(bool a)//����Ƃ��̏����Aa==true�Ȃ琬��
    {
        if (a)
        {
            (pieceStatus[tmp].type, pieceStatus[tmp].promotionType) = (pieceStatus[tmp].promotionType, pieceStatus[tmp].type);
            pieceStatus[tmp].PieceInitialize();
            SpriteRenderer pieceSprite = pieces[tmp].GetComponent<SpriteRenderer>();
            (pieceSprite.sprite, pieceStatus[tmp].promotionSprite) = (pieceStatus[tmp].promotionSprite, pieceSprite.sprite);
        }
        isCanTouch = true;
    }

    private void GameEndEffect()
    {
        isCanTouch = false;
        playerLogNet.text = "YouWin!";//�������O
        for (int i = 0; i < pieces.Length; i++)//�����ŋ�����ɖ߂�
        {
            if (pieceStatus[i].type > 20)//�����Ă�����߂�
            {
                SpriteRenderer sp = pieces[i].GetComponent<SpriteRenderer>();
                (pieceStatus[i].promotionSprite, sp.sprite) = (sp.sprite, pieceStatus[i].promotionSprite);
                (pieceStatus[i].type, pieceStatus[i].promotionType) = (pieceStatus[i].promotionType, pieceStatus[i].type);
            }
            pieceStatus[i].player = pieceStatus[i].holder;//���������ɖ߂�
            pieceStatus[i].PieceInitialize();//��̐ݒ��߂�
            pieces[i].transform.localPosition = (pieceStatus[i].startPosition - new Vector3(4, 4)) * gridSize;//��̈ʒu��߂�
            pieceStatus[i].piecePosition = (pieces[i].transform.localPosition) / gridSize + new Vector3(4, 4);//��̈ʒu����߂�
        }
        isCanTouch = true;
    }
    public override void OnConnectedToMaster()
    {
        var roomOption = new RoomOptions();
        roomOption.MaxPlayers = 2;
        // "Room"�Ƃ������O�̃��[���ɎQ������i���[�������݂��Ȃ���΍쐬���ĎQ������j
        PhotonNetwork.JoinOrCreateRoom(RoomManager.roomName, roomOption, TypedLobby.Default);
    }
    public override void OnJoinedRoom()
    {
        Vector3 position = Vector3.zero;
        PhotonNetwork.Instantiate("Avatar", position, Quaternion.identity);
    }
}
