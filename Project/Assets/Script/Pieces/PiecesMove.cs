using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class PiecesMove: MonoBehaviour//��̓����𐧌䂷��X�N���v�g
{
    [SerializeField] GameObject gridCollider;//�}�X�̓����蔻��
    public static bool isMoveStage = false;//���I�����Ă��邩�ǂ���
    public static float gridSize = 1f;//�}�X�̃T�C�Y
    private GameObject[] pieces;//��
    private PieceStatus[] pieceStatus;//��̃X�e�[�^�X
    private Collider[] piecesCollider;//��̔���
    private GameObject[] grid;//�}�X
    private GridStatus[] gridStatus;//�}�X�̃X�e�[�^�X
    private Transform[] gridTransform; //�}�X�̈ʒu
    private int tmp = 0;//�I��������̔ԍ�
    private SpriteRenderer pieceSprite;//��̌����ڂȂ�

    // Start is called before the first frame update
    void Start()
    {
        GameObject go;
        for (int j = 4; j > -5; j--)//�}�X�ɔ����z�u����
        {
            for (int i = -4; i < 5; i++)
            {
                go = Instantiate(gridCollider);
                go.transform.position = new Vector3(i * gridSize, j * gridSize,0 );
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
        grid = GameObject.FindGameObjectsWithTag("Grid");
        gridStatus = new GridStatus[grid.Length];
        gridTransform = new Transform[grid.Length];
        for (int i = 0; i < grid.Length; i++)
        {
            gridTransform[i] = grid[i].GetComponent<Transform>();
            gridStatus[i] = grid[i].GetComponent<GridStatus>();
            gridStatus[i].myPosition = new Vector2(gridTransform[i].position.x/ gridSize + 4, gridTransform[i].position.y / gridSize + 4);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isMoveStage)
        {
            for (int i = 0; i < gridStatus.Length; i++)
            {
                
                if (gridStatus[i].isSelect && MoveLimit(i))//�ړ���̑I�����������Ă�����ړ�������
                {
                    pieces[tmp].transform.position = grid[i].transform.position;//�ړ�������
                    pieceStatus[tmp].isSelect = false;//��̑I��������
                    gridStatus[i].isSelect = false;//�}�X�̑I��������
                    isMoveStage = false;//���̋��I�ׂȂ���Ԃ�����
                    pieceSprite.color = Color.white;//��̐F��߂�
                    pieceSprite = null;//��̐F��߂�
                    Debug.Log("�ړ��I�������");
                    foreach (var col in piecesCollider)//��̔����߂�
                    {
                        col.enabled = true;
                    }
                    break;
                }
                else if (gridStatus[i].isSelect && !MoveLimit(i))
                {
                    pieceStatus[tmp].isSelect = false;//��̑I��������
                    gridStatus[i].isSelect = false;//�}�X�̑I��������
                    isMoveStage = false;//���̋��I���ł��Ȃ���Ԃ�����
                    pieceSprite.color = Color.white;//��̐F��߂�
                    pieceSprite = null;//��̐F��߂�
                    Debug.Log("�����͈ړ��ł��܂���\n�ړ����[�h����");
                    foreach (var col in piecesCollider)//��̔����߂�
                    {
                        col.enabled = true;
                    }
                    break;
                }
            }
        }
        else
        {
            for (int i = 0; i < pieceStatus.Length; i++)
            {
                if (pieceStatus[i].CheckSelected())
                {

                    isMoveStage = true;//���̋��I���ł��Ȃ�����
                    tmp = i;//�I�����ꂽ��̔ԍ���ۑ�
                    pieceSprite = pieces[i].GetComponent<SpriteRenderer>();//��̐F��ς������Ԃɂ���
                    pieceSprite.color = Color.gray;//��̐F��ς���
                    foreach(var col in piecesCollider)//��̔��������
                    {
                        col.enabled = false;
                    }
                    Debug.Log("�ړ����[�h�Ɉڍs");
                    break;
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
        Vector2 deltaPosition;//�R�R�Ƌ�̈ʒu�������Έړ��\
        PieceStatus selectedPiece = pieceStatus[tmp];//���I�΂�Ă����
        GridStatus tmpgrid = gridStatus[gridNumber];//�s����̂܂�
        List<Vector2> limit = selectedPiece.distination;//�ړ��\�Ȉړ���
        //Debug.Log((tmpgrid.myPosition.x - selectedPiece.piecePosition.x) / Mathf.Abs(tmpgrid.myPosition.x - selectedPiece.piecePosition.x));
        for (int i = 0; i < pieceStatus.Length; i++)//�R�R����}�X�ɋ����Ă��邩�ǂ������m�F
        {
            if (tmpgrid.myPosition == pieceStatus[i].piecePosition)//����Ă���Ȃ�ړ��s�ɂ���
            {
                return false;
            }
            if(selectedPiece.type == 11 || selectedPiece.type == 12 || selectedPiece.type == 20)
            {
                //bool bitweenX = false, bitweenY = false;
                if(tmpgrid.myPosition.x - selectedPiece.piecePosition.x != 0 && tmpgrid.myPosition.y - selectedPiece.piecePosition.y == 0)
                {
                    Debug.Log("x�����̈ړ����m�F");
                    for (float j = 0; Mathf.Abs(j) < Mathf.Abs(tmpgrid.myPosition.x - selectedPiece.piecePosition.x); j += (tmpgrid.myPosition.x - selectedPiece.piecePosition.x) / Mathf.Abs(tmpgrid.myPosition.x - selectedPiece.piecePosition.x))//�R�R����Ԃɋ���邩�ǂ����m�F
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
                    for (float j = 0; Mathf.Abs(j) < Mathf.Abs(tmpgrid.myPosition.y - selectedPiece.piecePosition.y); j += (tmpgrid.myPosition.y - selectedPiece.piecePosition.y) / Mathf.Abs(tmpgrid.myPosition.y - selectedPiece.piecePosition.y))
                    {
                        if (pieceStatus[i].piecePosition.y == j + selectedPiece.piecePosition.y + (tmpgrid.myPosition.y - selectedPiece.piecePosition.y) / Mathf.Abs(tmpgrid.myPosition.y - selectedPiece.piecePosition.y) && tmpgrid.myPosition.x - pieceStatus[i].piecePosition.x == 0)
                        {
                            return false;
                        }
                    }
                }
               
            }
            else if(selectedPiece.type == 10 && tmpgrid.myPosition.x - selectedPiece.piecePosition.x != 0 &&tmpgrid.myPosition.y - selectedPiece.piecePosition.y != 0)
            {
                for(float j = 0; Mathf.Abs(j) < Mathf.Abs(tmpgrid.myPosition.x - selectedPiece.piecePosition.x); j += (tmpgrid.myPosition.x - selectedPiece.piecePosition.x) / Mathf.Abs(tmpgrid.myPosition.x - selectedPiece.piecePosition.x))
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
        return isCanMove;//�ړ��\���ǂ�����Ԃ�
    }

}
