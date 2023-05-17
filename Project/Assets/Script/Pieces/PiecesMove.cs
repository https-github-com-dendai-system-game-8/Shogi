using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class PiecesMove: MonoBehaviour
{
    [SerializeField] GameObject gridCollider;//�O���b�h�̓����蔻��
    public static bool isMoveStage = false;
    public static float gridSize = 1.012f;
    private GameObject[] pieces;//��
    private GameObject[] grid;//�}�X
    private GridStatus[] gridStatus;//�}�X�̃X�e�[�^�X
    private PiecesStatus[] piecesStatus;//��̃X�e�[�^�X
    private Transform[] gridTransform; 
    private int tmp = 0;

    // Start is called before the first frame update
    void Start()
    {
        GameObject go;
        for (int j = 4; j > -5; j--)
        {
            for (int i = -4; i < 5; i++)
            {
                go = Instantiate(gridCollider);
                go.transform.position = new Vector3(i * gridSize, j * gridSize,0 );
            }
        }
        pieces = GameObject.FindGameObjectsWithTag("Pieces");
        piecesStatus = new PiecesStatus[pieces.Length];
        for (int i = 0; i < pieces.Length; i++)
        {
            piecesStatus[i] = pieces[i].GetComponent<PiecesStatus>();
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
                    pieces[tmp].transform.position = grid[i].transform.position;
                    piecesStatus[tmp].isSelect = false;
                    gridStatus[i].isSelect = false;
                    isMoveStage = false;
                    Debug.Log("�ړ��I�������");
                    break;
                }
                else if (gridStatus[i].isSelect && !MoveLimit(i))
                {
                    Debug.Log("�����͈ړ��ł��܂���\n�ړ����[�h����");
                    gridStatus[i].isSelect = false;
                    isMoveStage = false;
                    piecesStatus[tmp].isSelect = false;
                    break;
                }
            }
        }
        else
        {
            for (int i = 0; i < piecesStatus.Length; i++)
            {
                if (piecesStatus[i].CheckSelected())
                {
                    isMoveStage = true;
                    tmp = i;
                    Debug.Log("�ړ����[�h�Ɉڍs");
                    break;
                }
            }
        }
    }

    private bool MoveLimit(int gridNumber)//�ړ��\�悩�ǂ������m���߂�
    {
        bool isCanMove = false;
        Vector2 deltaPosition;//�R�R�Ƌ�̈ʒu�������Έړ��\
        List<Vector2> limit = piecesStatus[tmp].distination;
        for(int i = 0;i < limit.Count; i++)
        {
            deltaPosition = new Vector2(gridStatus[gridNumber].myPosition.x  - limit[i].x, gridStatus[gridNumber].myPosition.y - limit[i].y);
            Debug.Log(deltaPosition);
            Debug.Log(piecesStatus[tmp].piecePosition);
            if(deltaPosition == piecesStatus[tmp].piecePosition)
            {
                isCanMove = true;
                break;
            }
        }
        //Debug.Log(gridStatus[gridNumber].myPosition);
        return isCanMove;
    }
}
