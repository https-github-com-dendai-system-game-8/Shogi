using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UIElements;

public class PiecesMoveNet : MonoBehaviourPunCallbacks
{
    [SerializeField] GameObject gridCollider;//�}�X�̓����蔻��
    [SerializeField] private Text playerLog;//�v���C���[���Ƃ̃��O
    public bool isMoveStage = false;//���I�����Ă��邩�ǂ���
    public bool isPawnPlay = false;//�������I�����Ă��邩�ǂ���
    public bool isCanTouch = false;//�G��邩�ǂ���
    public int turn = -1;//�ǂ���̃^�[����
    public static float gridSize = 1f;//�}�X�̃T�C�Y
    private int[] pawnQuentity = { 0, 0 };//�������̐�
    public GameObject[] pieces;//��
    public PieceStatusNet[] pieceStatus;//��̃X�e�[�^�X
    private PieceStatusNet tmpPieceStatus;
    private Collider[] piecesCollider;//��̔���
    private GameObject[] grid;//�}�X
    public PlayerManager[] playerManager;//�����Ƒ���̃f�[�^
    [HideInInspector] public GridStatusNet[] gridStatus;//�}�X�̃X�e�[�^�X
    private List<int> pawn = new List<int>();//�������̔ԍ�
    private int tmp = 0;//�I��������̔ԍ�
    [SerializeField] private Select sel;//�I�������o��

    // Start is called before the first frame update
    void Start()
    {
        grid = GameObject.FindGameObjectsWithTag("Grid");
        int x = 0, y = 0;
        for (int k = 0; k < grid.Length; k++)
        {

            grid[k].transform.localPosition = new Vector3(x * gridSize - 4, y * gridSize - 4, grid[k].transform.localPosition.z);

            if (x < 8)
                x++;
            else if (x == 8)
            {
                y++;
                x = 0;
            }

        }

        pieces = GameObject.FindGameObjectsWithTag("Pieces");
        pieceStatus = new PieceStatusNet[pieces.Length];
        piecesCollider = new Collider[pieces.Length];
        for (int i = 0; i < pieces.Length; i++)
        {
            pieceStatus[i] = pieces[i].GetComponent<PieceStatusNet>();
            piecesCollider[i] = pieces[i].GetComponent<Collider>();
        }
        gridStatus = new GridStatusNet[grid.Length];
        for (int i = 0; i < grid.Length; i++)
        {
            gridStatus[i] = grid[i].GetComponent<GridStatusNet>();
            gridStatus[i].myPosition = new Vector3(grid[i].transform.localPosition.x / gridSize + 4, grid[i].transform.localPosition.y / gridSize + 4);
        }
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
                        foreach (var col in piecesCollider)//��̔����߂�
                        {
                            col.enabled = true;
                        }
                        pieceStatus[tmp].piecePosition = new Vector2(Mathf.Round(pieces[tmp].transform.localPosition.x), Mathf.Round(pieces[tmp].transform.localPosition.y)) / gridSize + new Vector2(4, 4);
                        Debug.Log(pieceStatus[tmp].piecePosition);
                        if (((pieceStatus[tmp].piecePosition.y >= 6 && pieceStatus[tmp].player == -1) || (pieceStatus[tmp].piecePosition.y <= 2 && pieceStatus[tmp].player == 1))
                            && pieceStatus[tmp].canPromotion)//����邩�ǂ���
                        {
                            sel.OnClick();
                            isCanTouch = false;
                        }
                        if (playerManager == null)//�v���C���[�f�[�^���Ȃ��ꍇ��ɓ����
                        {
                            PlayerManager[] tmpPm;
                            tmpPm = FindObjectsOfType<PlayerManager>();
                            playerManager = tmpPm;
                            break;
                                
                            
                        }

                        turn *= -1;
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
                        tmpPieceStatus = pieceStatus[tmp];
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
                        tmpPieceStatus = pieceStatus[tmp];
                        break;
                    }
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.Escape))//�G�X�P�[�v�������ꂽ��Q�[�������
        {
            Application.Quit();
        }
    }

    private bool MoveLimit(int gridNumber)//�ړ��\�悩�ǂ������m���߂�
    {
        bool isCanMove = false;
        int getPawn = -1;
        Vector3 deltaPosition;//�R�R�Ƌ�̈ʒu�������Έړ��\
        PieceStatusNet selectedPiece = pieceStatus[tmp];//���I�΂�Ă����
        GridStatusNet tmpgrid = gridStatus[gridNumber];//�s����̃}�X
        List<Vector3> limit = selectedPiece.distination;//�ړ��\�Ȉړ���
        for (int i = 0; i < pieceStatus.Length; i++)//�R�R����}�X�ɋ����Ă��邩�ǂ������m�F
        {
            if (tmpgrid.myPosition == pieceStatus[i].piecePosition && pieceStatus[i].player == pieceStatus[tmp].player)//����Ă���Ȃ�ړ��s�ɂ���
            {
                return false;
            }
            else if (tmpgrid.myPosition == pieceStatus[i].piecePosition && pieceStatus[i].player != pieceStatus[tmp].player)
            {
                getPawn = i;
            }
            if (selectedPiece.type == 11 || selectedPiece.type == 12 || selectedPiece.type == 20 || selectedPiece.type == 23)
            {
                if (tmpgrid.myPosition.x - selectedPiece.piecePosition.x != 0 && tmpgrid.myPosition.y - selectedPiece.piecePosition.y == 0)
                {
                    Debug.Log("x�����̈ړ����m�F");
                    for (float j = 0; Mathf.Abs(j) < Mathf.Abs(tmpgrid.myPosition.x - selectedPiece.piecePosition.x) - 1; j += (tmpgrid.myPosition.x - selectedPiece.piecePosition.x) / Mathf.Abs(tmpgrid.myPosition.x - selectedPiece.piecePosition.x))//�R�R����Ԃɋ���邩�ǂ����m�F
                    {
                        if (pieceStatus[i].piecePosition.x == j + selectedPiece.piecePosition.x + (tmpgrid.myPosition.x - selectedPiece.piecePosition.x) / Mathf.Abs(tmpgrid.myPosition.x - selectedPiece.piecePosition.x) && tmpgrid.myPosition.y - pieceStatus[i].piecePosition.y == 0)
                        {
                            return false;
                        }
                    }
                }
                if (tmpgrid.myPosition.y - selectedPiece.piecePosition.y != 0 && tmpgrid.myPosition.x - selectedPiece.piecePosition.x == 0)
                {
                    Debug.Log("y�����̈ړ����m�F");
                    for (float j = 0; Mathf.Abs(j) < Mathf.Abs(tmpgrid.myPosition.y - selectedPiece.piecePosition.y) - 1; j += (tmpgrid.myPosition.y - selectedPiece.piecePosition.y) / Mathf.Abs(tmpgrid.myPosition.y - selectedPiece.piecePosition.y))
                    {
                        if (pieceStatus[i].piecePosition.y == j + selectedPiece.piecePosition.y + (tmpgrid.myPosition.y - selectedPiece.piecePosition.y) / Mathf.Abs(tmpgrid.myPosition.y - selectedPiece.piecePosition.y) && tmpgrid.myPosition.x - pieceStatus[i].piecePosition.x == 0)
                        {
                            return false;
                        }
                    }
                }

            }
            else if ((selectedPiece.type == 10 || selectedPiece.type == 22) && tmpgrid.myPosition.x - selectedPiece.piecePosition.x != 0 && tmpgrid.myPosition.y - selectedPiece.piecePosition.y != 0)
            {
                for (float j = 0; Mathf.Abs(j) < Mathf.Abs(tmpgrid.myPosition.x - selectedPiece.piecePosition.x) - 1; j += (tmpgrid.myPosition.x - selectedPiece.piecePosition.x) / Mathf.Abs(tmpgrid.myPosition.x - selectedPiece.piecePosition.x))
                {
                    float k = Mathf.Abs(j) * ((tmpgrid.myPosition.y - selectedPiece.piecePosition.y) / Mathf.Abs(tmpgrid.myPosition.y - selectedPiece.piecePosition.y));
                    if (pieceStatus[i].piecePosition == new Vector3(j + selectedPiece.piecePosition.x + (tmpgrid.myPosition.x - selectedPiece.piecePosition.x) / Mathf.Abs(tmpgrid.myPosition.x - selectedPiece.piecePosition.x), k + selectedPiece.piecePosition.y + (tmpgrid.myPosition.y - selectedPiece.piecePosition.y) / Mathf.Abs(tmpgrid.myPosition.y - selectedPiece.piecePosition.y)))
                    {
                        return false;
                    }
                }
            }

        }
        for (int i = 0; i < limit.Count; i++)//�I�������}�X���ړ��\�������Ŋm�F
        {
            deltaPosition = new Vector2(tmpgrid.myPosition.x - limit[i].x, tmpgrid.myPosition.y - limit[i].y);//�ړ��\���ǂ����v�Z
            if (deltaPosition == selectedPiece.piecePosition)//�ړ��\�Ȃ炻���`����
            {
                isCanMove = true;
                break;
            }
        }
        if (getPawn != -1 && isCanMove)//�R�R��������鏈��
        {
            pawn.Add(getPawn);
            int num = 1;
            pieceStatus[getPawn].player *= -1;

            if (pieceStatus[getPawn].player == -1)
                num = 0;
            pieceStatus[getPawn].transform.localPosition = new Vector2((pawnQuentity[num] % 9 - 4) * pieceStatus[getPawn].player, (pawnQuentity[num] / 9 + 5) * pieceStatus[getPawn].player);
            if (pieceStatus[getPawn].type >= 21)//�Ȃ��Ă�����������ꍇ�߂�
            {
                SpriteRenderer pieceSprite = pieces[getPawn].GetComponent<SpriteRenderer>();
                (pieceStatus[getPawn].type, pieceStatus[getPawn].promotionType, pieceSprite.sprite, pieceStatus[getPawn].promotionSprite)
                    = (pieceStatus[getPawn].promotionType, pieceStatus[getPawn].type, pieceStatus[getPawn].promotionSprite, pieceSprite.sprite);

            }
            pieceStatus[getPawn].CheckMove();
            pawnQuentity[num]++;
            if (pieceStatus[getPawn].type == 16)
            {
                GameEndEffect();
                return false;
            }

        }
        return isCanMove;//�ړ��\���ǂ�����Ԃ�
    }

    public void CheckPromotion(bool a)//����Ƃ��̏����Aa==true�Ȃ琬��
    {
        if (a)
        {
            (pieceStatus[tmp].type, pieceStatus[tmp].promotionType) = (pieceStatus[tmp].promotionType, pieceStatus[tmp].type);
            pieceStatus[tmp].CheckMove();
            SpriteRenderer pieceSprite = pieces[tmp].GetComponent<SpriteRenderer>();
            (pieceSprite.sprite, pieceStatus[tmp].promotionSprite) = (pieceStatus[tmp].promotionSprite, pieceSprite.sprite);
        }
        isCanTouch = true;
    }

    private void PawnPlay(int number) //��������o������
    {

        for (int i = 0; i < pieceStatus.Length; i++)//�u���邩�ǂ���
        {
            if (gridStatus[number].myPosition == pieceStatus[i].piecePosition)
            {
                Debug.Log("�����ɂ͒u���܂���");
                return;
            }
            else if (gridStatus[number].myPosition.x == pieceStatus[i].piecePosition.x
                && pieceStatus[i].piecePosition.y <= 8 && pieceStatus[i].piecePosition.y >= 0
                && pieceStatus[i].type >= 1 && pieceStatus[i].type <= 9
                && pieceStatus[i].player == pieceStatus[tmp].player
                && pieceStatus[tmp].type >= 1 && pieceStatus[tmp].type <= 9)

            {
                Debug.Log("����ł�");
                return;
            }
        }
        pieces[tmp].transform.localPosition = gridStatus[number].myPosition / gridSize - new Vector3(4, 4);
        pieceStatus[tmp].CheckMove();
        pawn.Remove(tmp);
        turn *= -1;
    }

    private void GameEndEffect()
    {
        isCanTouch = false;
        playerLog.text = "YouWin!";//�������O
        for (int i = 0; i < pieces.Length; i++)//�����ŋ�����ɖ߂�
        {
            if (pieceStatus[i].type > 20)//�����Ă�����߂�
            {
                SpriteRenderer sp = pieces[i].GetComponent<SpriteRenderer>();
                (pieceStatus[i].promotionSprite, sp.sprite) = (sp.sprite, pieceStatus[i].promotionSprite);
                (pieceStatus[i].type, pieceStatus[i].promotionType) = (pieceStatus[i].promotionType, pieceStatus[i].type);
            }
            pieceStatus[i].player = pieceStatus[i].holder;//���������ɖ߂�
            pieceStatus[i].CheckMove();//��̐ݒ��߂�
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
        Vector3 position = new Vector3(UnityEngine.Random.Range(-3f, 3f), UnityEngine.Random.Range(-3f, 3f));
        if (PhotonNetwork.LocalPlayer.ActorNumber == 1)
            PhotonNetwork.Instantiate("Avatar", position, Quaternion.identity);
        else
            PhotonNetwork.Instantiate("Avatar2", position, Quaternion.identity);
    }
}
