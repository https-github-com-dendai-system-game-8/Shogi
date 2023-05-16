using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PiecesMove: MonoBehaviour
{
    [SerializeField] GameObject gridCollider;//�O���b�h�̓����蔻��
    public static bool isMoveStage = false;
    private GameObject[] pieces;//��
    private GameObject[] grid;//�}�X
    private GridStatus[] gridStatus;//��̃X�e�[�^�X
    private PiecesStatus[] piecesStatus;//��̃X�e�[�^�X
    private int tmp = 0;
    // Start is called before the first frame update
    void Start()
    {
        Transform go;
        for (int j = -4; j < 5; j++)
        {
            for (int i = -4; i < 5; i++)
            {
                go = Instantiate(gridCollider).transform;
                go.position = new Vector3(j * 1.012f, 0, i * 1.012f);
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
        for (int i = 0; i < grid.Length; i++)
        {
            gridStatus[i] = grid[i].GetComponent<GridStatus>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isMoveStage)
        {
            Debug.Log(gridStatus.Length);
            for (int i = 0; i < gridStatus.Length; i++)
            {
                
                if (gridStatus[i].isSelect)//�ړ���̑I�����������Ă�����ړ�������
                {
                    pieces[tmp].transform.position = grid[i].transform.position;
                    piecesStatus[tmp].isSelect = false;
                    gridStatus[i].isSelect = false;
                    isMoveStage = false;
                    Debug.Log("�ړ��I�������");
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
}
