using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class PiecesMove: MonoBehaviour//駒の動きを制御するスクリプト
{
    [SerializeField] GameObject gridCollider;//マスの当たり判定
    public static bool isMoveStage = false;//駒を選択しているかどうか
    public static float gridSize = 1f;//マスのサイズ
    private GameObject[] pieces;//駒
    private PieceStatus[] pieceStatus;//駒のステータス
    private Collider[] piecesCollider;//駒の判定
    private GameObject[] grid;//マス
    private GridStatus[] gridStatus;//マスのステータス
    private Transform[] gridTransform; //マスの位置
    private int tmp = 0;//選択した駒の番号
    private SpriteRenderer pieceSprite;//駒の見た目など

    // Start is called before the first frame update
    void Start()
    {
        GameObject go;
        for (int j = 4; j > -5; j--)//マスに判定を配置する
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
                
                if (gridStatus[i].isSelect && MoveLimit(i))//移動先の選択が完了していたら移動させる
                {
                    pieces[tmp].transform.position = grid[i].transform.position;//移動させる
                    pieceStatus[tmp].isSelect = false;//駒の選択を解除
                    gridStatus[i].isSelect = false;//マスの選択を解除
                    isMoveStage = false;//他の駒を選べない状態を解除
                    pieceSprite.color = Color.white;//駒の色を戻す
                    pieceSprite = null;//駒の色を戻す
                    Debug.Log("移動終わったよ");
                    foreach (var col in piecesCollider)//駒の判定を戻す
                    {
                        col.enabled = true;
                    }
                    break;
                }
                else if (gridStatus[i].isSelect && !MoveLimit(i))
                {
                    pieceStatus[tmp].isSelect = false;//駒の選択を解除
                    gridStatus[i].isSelect = false;//マスの選択を解除
                    isMoveStage = false;//他の駒を選択できない状態を解除
                    pieceSprite.color = Color.white;//駒の色を戻す
                    pieceSprite = null;//駒の色を戻す
                    Debug.Log("そこは移動できません\n移動モード解除");
                    foreach (var col in piecesCollider)//駒の判定を戻す
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

                    isMoveStage = true;//他の駒を選択できなくする
                    tmp = i;//選択された駒の番号を保存
                    pieceSprite = pieces[i].GetComponent<SpriteRenderer>();//駒の色を変えられる状態にする
                    pieceSprite.color = Color.gray;//駒の色を変える
                    foreach(var col in piecesCollider)//駒の判定を消す
                    {
                        col.enabled = false;
                    }
                    Debug.Log("移動モードに移行");
                    break;
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.Escape))//エスケープが押されたらゲームを閉じる
        {
            Application.Quit();
        }
    }

    private bool MoveLimit(int gridNumber)//移動可能先かどうかを確かめる
    {
        bool isCanMove = false;
        Vector2 deltaPosition;//ココと駒の位置が合えば移動可能
        PieceStatus selectedPiece = pieceStatus[tmp];//今選ばれている駒
        GridStatus tmpgrid = gridStatus[gridNumber];//行き先のます
        List<Vector2> limit = selectedPiece.distination;//移動可能な移動先
        //Debug.Log((tmpgrid.myPosition.x - selectedPiece.piecePosition.x) / Mathf.Abs(tmpgrid.myPosition.x - selectedPiece.piecePosition.x));
        for (int i = 0; i < pieceStatus.Length; i++)//ココからマスに駒が乗っているかどうかを確認
        {
            if (tmpgrid.myPosition == pieceStatus[i].piecePosition)//乗っているなら移動不可にする
            {
                return false;
            }
            if(selectedPiece.type == 11 || selectedPiece.type == 12 || selectedPiece.type == 20)
            {
                //bool bitweenX = false, bitweenY = false;
                if(tmpgrid.myPosition.x - selectedPiece.piecePosition.x != 0 && tmpgrid.myPosition.y - selectedPiece.piecePosition.y == 0)
                {
                    Debug.Log("x方向の移動を確認");
                    for (float j = 0; Mathf.Abs(j) < Mathf.Abs(tmpgrid.myPosition.x - selectedPiece.piecePosition.x); j += (tmpgrid.myPosition.x - selectedPiece.piecePosition.x) / Mathf.Abs(tmpgrid.myPosition.x - selectedPiece.piecePosition.x))//ココから間に駒があるかどうか確認
                    {
                        if (pieceStatus[i].piecePosition.x == j + selectedPiece.piecePosition.x + (tmpgrid.myPosition.x - selectedPiece.piecePosition.x) / Mathf.Abs(tmpgrid.myPosition.x - selectedPiece.piecePosition.x) && tmpgrid.myPosition.y - pieceStatus[i].piecePosition.y == 0)
                        {
                            return false;
                        }
                    }
                }
                if(tmpgrid.myPosition.y - selectedPiece.piecePosition.y != 0 && tmpgrid.myPosition.x - selectedPiece.piecePosition.x == 0)
                {
                    Debug.Log("y方向の移動を確認");
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
        for (int i = 0;i < limit.Count; i++)//選択したマスが移動可能かここで確認
        {
            deltaPosition = new Vector2(tmpgrid.myPosition.x  - limit[i].x, tmpgrid.myPosition.y - limit[i].y);//移動可能かどうか計算
            if(deltaPosition == selectedPiece.piecePosition)//移動可能ならそれを伝える
            {
                isCanMove = true;
                break;
            }
        }
        return isCanMove;//移動可能かどうかを返す
    }

}
