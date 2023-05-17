using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PiecesMove: MonoBehaviour
{
    [SerializeField] GameObject gridCollider;//グリッドの当たり判定
    public static bool isMoveStage = false;
    public static float gridSize = 1.012f;
    private GameObject[] pieces;//駒
    private GameObject[] grid;//マス
    private GridStatus[] gridStatus;//マスのステータス
    private PiecesStatus[] piecesStatus;//駒のステータス
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
                
                if (gridStatus[i].isSelect)//移動先の選択が完了していたら移動させる
                {
                    pieces[tmp].transform.position = grid[i].transform.position;
                    piecesStatus[tmp].isSelect = false;
                    gridStatus[i].isSelect = false;
                    isMoveStage = false;
                    Debug.Log("移動終わったよ");
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
                    Debug.Log("移動モードに移行");
                    break;
                }
            }
        }
    }

    private void MoveLimit()
    {
        List<Vector2> canMove = piecesStatus[tmp].distination;
        
    }
}
