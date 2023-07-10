using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;
using Photon.Pun;

public class PiecesMove: MonoBehaviourPunCallbacks//��̓����𐧌䂷��X�N���v�g
{
    [SerializeField] private Text[] playerLog = new Text[2];//�v���C���[���Ƃ̃��O
    public Text masterLog;//�S�̂̃��O
    public bool isMoveStage = false;//���I�����Ă��邩�ǂ���
    public bool isPawnPlay = false;//�������I�����Ă��邩�ǂ���
    public bool isCanTouch = true;//�G��邩�ǂ���
    public int turn = -1;//�ǂ���̃^�[����
    public static float gridSize = 1f;//�}�X�̃T�C�Y
    public int[] pawnQuentity = {0,0};//�������̐�
    public GameObject[] pieces;//��
    public PieceStatus[] pieceStatus;//��̃X�e�[�^�X
    public Collider[] piecesCollider;//��̔���
    public GameObject[] grid;//�}�X
    [HideInInspector]public GridStatus[] gridStatus;//�}�X�̃X�e�[�^�X
    public List<int> pawn = new List<int>();//�������̔ԍ�
    public int tmp = 0;//�I��������̔ԍ�
    public Select sel;
    public Vector3[] beforePosition;//�ړ��O�̏ꏊ
    public bool isCanMove = false;//�������邩�ǂ���
    public bool isPromotion = false;//�����Ă���r�����ǂ���

    // Start is called before the first frame update
    void Start()
    {
        Initialize();
        playerLog[0].text = "P1";
        playerLog[1].text = "P2";
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
                        masterLog.text = "�ړ��I��";
                        isPromotion = false;
                        foreach (var col in piecesCollider)//��̔����߂�
                        {
                            col.enabled = true;
                        }
                        pieceStatus[tmp].piecePosition = new Vector2(Mathf.Round(pieces[tmp].transform.localPosition.x), Mathf.Round(pieces[tmp].transform.localPosition.y)) / gridSize + new Vector2(4, 4);
                        Debug.Log(pieceStatus[tmp].piecePosition);

                        break;
                    }
                    else if (gridStatus[i].isSelect && !MoveLimit(i))
                    {
                        pieceStatus[tmp].isSelect = false;//��̑I��������
                        gridStatus[i].isSelect = false;//�}�X�̑I��������
                        isMoveStage = false;//���̋��I���ł��Ȃ���Ԃ�����
                        SpriteRenderer pieceSprite = pieces[tmp].GetComponent<SpriteRenderer>();
                        pieceSprite.color = Color.white;//��̐F��߂�
                        masterLog.text = "�����͈ړ��ł��܂���";
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
                for(int i = 0;i < gridStatus.Length; i++)
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
            else if(!isMoveStage)
            {
                for (int i = 0; i < pieceStatus.Length; i++)
                {
                    if (pieceStatus[i].CheckSelected() && pieceStatus[i].piecePosition.y >= 0 && pieceStatus[i].piecePosition.y <= 8)
                    {
                        IndicatePoint();
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
                    else if (pieceStatus[i].CheckSelected())
                    {
                        IndicatePoint();
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
        for (int i = 0; i < pieceStatus.Length; i++)//������Ă���Ȃ�^�[����ς���
        {
            pieceStatus[i].piecePosition = new Vector2(Mathf.Round(pieces[i].transform.localPosition.x), Mathf.Round(pieces[i].transform.localPosition.y)) / gridSize + new Vector2(4, 4);
            if (beforePosition[i] != pieceStatus[i].piecePosition && al)
            {
                Debug.Log("change");
                turn *= -1;
                al = false;
            }
            beforePosition[i] = pieceStatus[i].piecePosition;

            PieceSafe(i);
        }
        int[] tmptype = { 0, 0 };
        int playertmp = 0;
        for (int i = 0; i < pieceStatus.Length; i++)//�ǂ��炩������������Ă���Ȃ�Q�[���I��
        {
            if (pieceStatus[i].type == 16)
            {
                tmptype[playertmp] = i;
                playertmp++;
            }
        }
        if (pieceStatus[tmptype[0]].player == pieceStatus[tmptype[1]].player)
            GameEndEffect(pieceStatus[tmptype[0]].player);
        for (int i = 0;i < gridStatus.Length; i++)
        {
            GridSafe(i);
        }
        if (Input.GetKeyDown(KeyCode.Escape))//�G�X�P�[�v�������ꂽ��Q�[�������
        {
            Application.Quit();
        }
    }

    public bool MoveLimit(int gridNumber)//�ړ��\�悩�ǂ������m���߂�
    {
        isCanMove = false;//�������ǂ���
        StartCoroutine(TestCoroutine(gridNumber));
        
        return isCanMove;//�ړ��\���ǂ�����Ԃ�
    }
    public IEnumerator TestCoroutine(int gridNumber)
    {
        int getPawn = -1;//������ꍇ�̎���̔ԍ�
        Vector3 deltaPosition;//�R�R�Ƌ�̈ʒu�������Έړ��\
        PieceStatus selectedPiece = pieceStatus[tmp];//���I�΂�Ă����
        GridStatus distinationgrid = gridStatus[gridNumber];//�s����̃}�X
        List<Vector3> limit = selectedPiece.distination;//�ړ��\�Ȉړ���
        for (int i = 0; i < pieceStatus.Length; i++)//�R�R����}�X�ɋ����Ă��邩�ǂ������m�F
        {
            if (distinationgrid.myPosition == pieceStatus[i].piecePosition && pieceStatus[i].player == pieceStatus[tmp].player)
                //����Ă���Ȃ�ړ��s�ɂ���
            {
                isCanMove = false;
                yield break;
            }
            else if (distinationgrid.myPosition == pieceStatus[i].piecePosition && pieceStatus[i].player != pieceStatus[tmp].player)
                //�G�̋�Ȃ珟��
            {
                if (pieceStatus[i].piecePoint > pieceStatus[tmp].piecePoint)
                    getPawn = tmp;
                else
                    getPawn = i;
            }
            if (selectedPiece.type == 11 || selectedPiece.type == 12 || selectedPiece.type == 20 || selectedPiece.type == 23)//���ԂƔ�Ԃ̏���
            {
                if (distinationgrid.myPosition.x - selectedPiece.piecePosition.x != 0 && distinationgrid.myPosition.y - selectedPiece.piecePosition.y == 0)
                {
                    Debug.Log("x�����̈ړ����m�F");
                    for (float j = 0; Mathf.Abs(j) < Mathf.Abs(distinationgrid.myPosition.x - selectedPiece.piecePosition.x) - 1; j += (distinationgrid.myPosition.x - selectedPiece.piecePosition.x) / Mathf.Abs(distinationgrid.myPosition.x - selectedPiece.piecePosition.x))//�R�R����Ԃɋ���邩�ǂ����m�F
                    {
                        if (pieceStatus[i].piecePosition.x == j + selectedPiece.piecePosition.x + (distinationgrid.myPosition.x - selectedPiece.piecePosition.x) / Mathf.Abs(distinationgrid.myPosition.x - selectedPiece.piecePosition.x) && distinationgrid.myPosition.y - pieceStatus[i].piecePosition.y == 0)
                        {
                            isCanMove =  false;
                            yield break;
                        }
                    }
                }
                if (distinationgrid.myPosition.y - selectedPiece.piecePosition.y != 0 && distinationgrid.myPosition.x - selectedPiece.piecePosition.x == 0)//
                {
                    Debug.Log("y�����̈ړ����m�F");
                    for (float j = 0; Mathf.Abs(j) < Mathf.Abs(distinationgrid.myPosition.y - selectedPiece.piecePosition.y) - 1; j += (distinationgrid.myPosition.y - selectedPiece.piecePosition.y) / Mathf.Abs(distinationgrid.myPosition.y - selectedPiece.piecePosition.y))
                    {
                        if (pieceStatus[i].piecePosition.y == j + selectedPiece.piecePosition.y + (distinationgrid.myPosition.y - selectedPiece.piecePosition.y) / Mathf.Abs(distinationgrid.myPosition.y - selectedPiece.piecePosition.y) && distinationgrid.myPosition.x - pieceStatus[i].piecePosition.x == 0)
                        {
                            isCanMove =  false;
                            yield break;
                        }
                    }
                }

            }
            else if ((selectedPiece.type == 10 || selectedPiece.type == 22) && distinationgrid.myPosition.x - selectedPiece.piecePosition.x != 0 && distinationgrid.myPosition.y - selectedPiece.piecePosition.y != 0)//�p�s�Ɨ��n�̏���
            {
                for (float j = 0; Mathf.Abs(j) < Mathf.Abs(distinationgrid.myPosition.x - selectedPiece.piecePosition.x) - 1; j += (distinationgrid.myPosition.x - selectedPiece.piecePosition.x) / Mathf.Abs(distinationgrid.myPosition.x - selectedPiece.piecePosition.x))
                {
                    float k = Mathf.Abs(j) * ((distinationgrid.myPosition.y - selectedPiece.piecePosition.y) / Mathf.Abs(distinationgrid.myPosition.y - selectedPiece.piecePosition.y));
                    if (pieceStatus[i].piecePosition == new Vector3(j + selectedPiece.piecePosition.x + (distinationgrid.myPosition.x - selectedPiece.piecePosition.x) / Mathf.Abs(distinationgrid.myPosition.x - selectedPiece.piecePosition.x), k + selectedPiece.piecePosition.y + (distinationgrid.myPosition.y - selectedPiece.piecePosition.y) / Mathf.Abs(distinationgrid.myPosition.y - selectedPiece.piecePosition.y)))
                    {
                        isCanMove = false;
                        yield break;
                    }
                }
            }

        }
        for (int i = 0; i < limit.Count; i++)//�I�������}�X���ړ��\�������Ŋm�F
        {
            deltaPosition = new Vector2(distinationgrid.myPosition.x - limit[i].x, distinationgrid.myPosition.y - limit[i].y);//�ړ��\���ǂ����v�Z
            if (deltaPosition == selectedPiece.piecePosition)//�ړ��\�Ȃ炻���`����
            {
                isCanMove = true;
                break;
            }
        }
        if (((distinationgrid.myPosition.y >= 6 && selectedPiece.player == -1) || (distinationgrid.myPosition.y <= 2 && selectedPiece.player == 1))
                            && selectedPiece.canPromotion && isCanMove && !isPromotion)//�����Ő��邩�ǂ����𔻒�
        {
            sel.OnClick();
            isCanTouch = false;
            isCanMove = false;
            isPromotion = true;
            distinationgrid.isSelect = false;
            Debug.Log("�ҋ@�J�n");
            yield return new WaitUntil(() => isCanTouch);
            distinationgrid.isSelect = true;
            isCanMove = true;
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
            pieceStatus[getPawn].PieceInitialize();
            pawnQuentity[num]++;
            if (pieceStatus[getPawn].type == 16)//������̂������Ȃ�
            {
                isCanMove = true;
            }
            else if (pieceStatus[getPawn].player != turn)
            {
                isCanMove = false;
            }
        }

    }
    
    public void PieceSafe(int i)
    {
        pieceStatus[i].IsSafe(isMoveStage, isCanTouch, turn);
    }
    public void GridSafe(int i)
    {
        gridStatus[i].IsSafe(isMoveStage, isCanTouch);
    }

    public void CheckPromotion(bool a)//����Ƃ��̏����Aa==true�Ȃ琬��
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

    public void PawnPlay(int number) //��������o������
    {

        for (int i = 0; i < pieceStatus.Length; i++)//�u���邩�ǂ���
        {
            if (gridStatus[number].myPosition == pieceStatus[i].piecePosition)
            {
                masterLog.text = "�����ɂ͒u���܂���";
                return;
            }
            else if (gridStatus[number].myPosition.x == pieceStatus[i].piecePosition.x
                && pieceStatus[i].piecePosition.y <= 8 && pieceStatus[i].piecePosition.y >= 0
                && pieceStatus[i].type >= 1 && pieceStatus[i].type <= 9
                && pieceStatus[i].player == pieceStatus[tmp].player
                && pieceStatus[tmp].type >= 1 && pieceStatus[tmp].type <= 9)

            {
                masterLog.text = "����ł�";
                return;
            }
        }
        pieces[tmp].transform.localPosition = gridStatus[number].myPosition / gridSize - new Vector3(4, 4);
        pieceStatus[tmp].PieceInitialize();
        pawn.Remove(tmp);
        pieceStatus[tmp].piecePosition = new Vector2(Mathf.Round(pieces[tmp].transform.localPosition.x), Mathf.Round(pieces[tmp].transform.localPosition.y)) / gridSize + new Vector2(4, 4);
    }

    public void Initialize()
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
        pieceStatus = new PieceStatus[pieces.Length];
        piecesCollider = new Collider[pieces.Length];
        beforePosition = new Vector3[pieces.Length];
        for (int i = 0; i < pieces.Length; i++)
        {
            pieceStatus[i] = pieces[i].GetComponent<PieceStatus>();
            piecesCollider[i] = pieces[i].GetComponent<Collider>();
            pieceStatus[i].PieceInitialize();
            beforePosition[i] = pieceStatus[i].startPosition;
        }
        gridStatus = new GridStatus[grid.Length];
        for (int i = 0; i < grid.Length; i++)
        {
            gridStatus[i] = grid[i].GetComponent<GridStatus>();
            gridStatus[i].myPosition = new Vector2(grid[i].transform.localPosition.x / gridSize + 4, grid[i].transform.localPosition.y / gridSize + 4);
        }
    }

    private void IndicatePoint()
    {
        if (pieceStatus[tmp].player == -1)
            playerLog[0].text = Convert.ToString(pieceStatus[tmp].piecePoint);
        else if (pieceStatus[tmp].player == 1)
            playerLog[1].text = Convert.ToString(pieceStatus[tmp].piecePoint);
    }

    private void GameEndEffect(int winner)//�������̃G�t�F�N�g
    {
        int loser;
        if (winner == -1)
        {
            loser = 1;
            winner = 0;
        }
        else if (winner == 1)
        {
            loser = 0;
        }
        else
            return;
        isCanTouch = false;
        playerLog[winner].text = "YouWin!";
        playerLog[loser].text = "YouLose...";
        for(int i = 0; i < pieces.Length;i++)//��̔z�u�����ɖ߂�
        {
            if (pieceStatus[i].type > 20)//�Ȃ��Ă�����߂�
            {
                SpriteRenderer sp = pieces[i].GetComponent<SpriteRenderer>();
                (pieceStatus[i].promotionSprite, sp.sprite) = (sp.sprite, pieceStatus[i].promotionSprite);
                (pieceStatus[i].type, pieceStatus[i].promotionType) = (pieceStatus[i].promotionType, pieceStatus[i].type);
            }
            pieceStatus[i].player = pieceStatus[i].holder;
            pieceStatus[i].PieceInitialize();
            pieces[i].transform.localPosition = (pieceStatus[i].startPosition - new Vector3(4, 4)) * gridSize;
            pieceStatus[i].piecePosition = (pieces[i].transform.localPosition) / gridSize + new Vector3(4, 4);
        }
        isCanTouch = true;
    }

}
