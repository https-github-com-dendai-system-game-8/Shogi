using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class PiecesMove: MonoBehaviour//��̓����𐧌䂷��X�N���v�g
{
    [SerializeField] GameObject gridCollider;//�}�X�̓����蔻��
    public bool isMoveStage = false;//���I�����Ă��邩�ǂ���
    public bool isPawnPlay = false;
    public bool isCanTouch = true;//�G��邩�ǂ���
    public static float gridSize = 1f;//�}�X�̃T�C�Y
    private int[] pawnQuentity = {0,0};
    private GameObject[] pieces;//��
    private PieceStatus[] pieceStatus;//��̃X�e�[�^�X
    private Collider[] piecesCollider;//��̔���
    private GameObject[] grid;//�}�X
    private GridStatus[] gridStatus;//�}�X�̃X�e�[�^�X
    private Transform[] gridTransform; //�}�X�̈ʒu
    private List<int> pawn = new List<int>();
    private int tmp = 0;//�I��������̔ԍ�
    [SerializeField]private Select sel;

    // Start is called before the first frame update
    void Start()
    {
        grid = GameObject.FindGameObjectsWithTag("Grid");
        int x = 0, y = 0;
        for(int k = 0;k < grid.Length; k++)
        {

            grid[k].transform.localPosition = new Vector3(x * gridSize - 4, y * gridSize - 4, 0);
            
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
        for (int i = 0; i < pieces.Length; i++)
        {
            pieceStatus[i] = pieces[i].GetComponent<PieceStatus>();
            piecesCollider[i] = pieces[i].GetComponent<Collider>();
        }
        gridStatus = new GridStatus[grid.Length];
        gridTransform = new Transform[grid.Length];
        for (int i = 0; i < grid.Length; i++)
        {
            gridTransform[i] = grid[i].GetComponent<Transform>();
            gridStatus[i] = grid[i].GetComponent<GridStatus>();
            gridStatus[i].myPosition = new Vector2(gridTransform[i].localPosition.x/ gridSize + 4, gridTransform[i].localPosition.y / gridSize + 4);
        }
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
                        pieces[tmp].transform.position = grid[i].transform.position;//�ړ�������
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
            else if (isMoveStage && isPawnPlay)
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
            else
            {
                for (int i = 0; i < pieceStatus.Length; i++)
                {
                    if (pieceStatus[i].CheckSelected() && pieceStatus[i].piecePosition.y >= 0 && pieceStatus[i].piecePosition.y <= 8)
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
                    else if (pieceStatus[i].CheckSelected())
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
        if (Input.GetKeyDown(KeyCode.Escape))//�G�X�P�[�v�������ꂽ��Q�[�������
        {
            Application.Quit();
        }
    }

    private bool MoveLimit(int gridNumber)//�ړ��\�悩�ǂ������m���߂�
    {
        bool isCanMove = false;
        int getPawn = -1;
        Vector2 deltaPosition;//�R�R�Ƌ�̈ʒu�������Έړ��\
        PieceStatus selectedPiece = pieceStatus[tmp];//���I�΂�Ă����
        GridStatus tmpgrid = gridStatus[gridNumber];//�s����̂܂�
        List<Vector2> limit = selectedPiece.distination;//�ړ��\�Ȉړ���
        //Debug.Log((tmpgrid.myPosition.x - selectedPiece.piecePosition.x) / Mathf.Abs(tmpgrid.myPosition.x - selectedPiece.piecePosition.x));
        for (int i = 0; i < pieceStatus.Length; i++)//�R�R����}�X�ɋ����Ă��邩�ǂ������m�F
        {
            if (tmpgrid.myPosition == pieceStatus[i].piecePosition && pieceStatus[i].player == pieceStatus[tmp].player)//����Ă���Ȃ�ړ��s�ɂ���
            {
                return false;
            }
            else if(tmpgrid.myPosition == pieceStatus[i].piecePosition && pieceStatus[i].player != pieceStatus[tmp].player)
            {
                getPawn = i;
            }
            if(selectedPiece.type == 11 || selectedPiece.type == 12 || selectedPiece.type == 20 || selectedPiece.type == 23)
            {
                //bool bitweenX = false, bitweenY = false;
                if(tmpgrid.myPosition.x - selectedPiece.piecePosition.x != 0 && tmpgrid.myPosition.y - selectedPiece.piecePosition.y == 0)
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
                if(tmpgrid.myPosition.y - selectedPiece.piecePosition.y != 0 && tmpgrid.myPosition.x - selectedPiece.piecePosition.x == 0)
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
            else if((selectedPiece.type == 10 || selectedPiece.type == 22 ) && tmpgrid.myPosition.x - selectedPiece.piecePosition.x != 0 &&tmpgrid.myPosition.y - selectedPiece.piecePosition.y != 0)
            {
                for(float j = 0; Mathf.Abs(j) < Mathf.Abs(tmpgrid.myPosition.x - selectedPiece.piecePosition.x) - 1; j += (tmpgrid.myPosition.x - selectedPiece.piecePosition.x) / Mathf.Abs(tmpgrid.myPosition.x - selectedPiece.piecePosition.x))
                    {
                    float k = Mathf.Abs(j) * ((tmpgrid.myPosition.y - selectedPiece.piecePosition.y) / Mathf.Abs(tmpgrid.myPosition.y - selectedPiece.piecePosition.y));
                    if (pieceStatus[i].piecePosition == new Vector2(j + selectedPiece.piecePosition.x + (tmpgrid.myPosition.x - selectedPiece.piecePosition.x) / Mathf.Abs(tmpgrid.myPosition.x - selectedPiece.piecePosition.x), k + selectedPiece.piecePosition.y + (tmpgrid.myPosition.y - selectedPiece.piecePosition.y) / Mathf.Abs(tmpgrid.myPosition.y - selectedPiece.piecePosition.y)))
                        {
                            return false;
                        }
                    }
            }
           
        }
        for (int i = 0;i < limit.Count; i++)//�I�������}�X���ړ��\�������Ŋm�F
        {
            deltaPosition = new Vector2(tmpgrid.myPosition.x  - limit[i].x, tmpgrid.myPosition.y - limit[i].y);//�ړ��\���ǂ����v�Z
            if(deltaPosition == selectedPiece.piecePosition)//�ړ��\�Ȃ炻���`����
            {
                isCanMove = true;
                break;
            }
        }
        if(getPawn != -1 && isCanMove)//�R�R��������鏈��
        {
            pawn.Add(getPawn);
            int num = 1;
            if (pieceStatus[getPawn].player == 1)
                pieceStatus[getPawn].player = -1;
            else if (pieceStatus[getPawn].player == -1)
                pieceStatus[getPawn].player = 1;

            if (pieceStatus[getPawn].player == -1)
                num = 0;
            pieceStatus[getPawn].transform.localPosition = new Vector2((pawnQuentity[num] % 9 - 4) * pieceStatus[getPawn].player, (pawnQuentity[num] / 9 + 5) * pieceStatus[getPawn].player);
            if(pieceStatus[getPawn].type >= 21)//�Ȃ��Ă�����������ꍇ�߂�
            {
                (pieceStatus[getPawn].type, pieceStatus[getPawn].promotionType) 
                    = (pieceStatus[getPawn].promotionType, pieceStatus[getPawn].type);
            }
            pieceStatus[getPawn].CheckMove();
            pawnQuentity[num]++;
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

        for(int i = 0;i < pieceStatus.Length; i++)//�u���邩�ǂ���
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
        pieces[tmp].transform.localPosition = gridStatus[number].myPosition / gridSize - new Vector2(4,4);
        pieceStatus[tmp].CheckMove();
    }
}
