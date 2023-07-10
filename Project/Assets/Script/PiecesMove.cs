using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;
using Photon.Pun;

public class PiecesMove: MonoBehaviourPunCallbacks//駒の動きを制御するスクリプト
{
    [SerializeField] private Text[] playerLog = new Text[2];//プレイヤーごとのログ
    public Text masterLog;//全体のログ
    public bool isMoveStage = false;//駒を選択しているかどうか
    public bool isPawnPlay = false;//持ち駒を選択しているかどうか
    public bool isCanTouch = true;//触れるかどうか
    public int turn = -1;//どちらのターンか
    public static float gridSize = 1f;//マスのサイズ
    public int[] pawnQuentity = {0,0};//取った駒の数
    public GameObject[] pieces;//駒
    public PieceStatus[] pieceStatus;//駒のステータス
    public Collider[] piecesCollider;//駒の判定
    public GameObject[] grid;//マス
    [HideInInspector]public GridStatus[] gridStatus;//マスのステータス
    public List<int> pawn = new List<int>();//取った駒の番号
    public int tmp = 0;//選択した駒の番号
    public Select sel;
    public Vector3[] beforePosition;//移動前の場所
    public bool isCanMove = false;//動かせるかどうか
    public bool isPromotion = false;//成っている途中かどうか

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

                    if (gridStatus[i].isSelect && MoveLimit(i))//移動先の選択が完了していたら移動させる
                    {
                        pieces[tmp].transform.localPosition = grid[i].transform.localPosition;//移動させる
                        pieceStatus[tmp].isSelect = false;//駒の選択を解除
                        gridStatus[i].isSelect = false;//マスの選択を解除
                        isMoveStage = false;//他の駒を選べない状態を解除
                        SpriteRenderer pieceSprite = pieces[tmp].GetComponent<SpriteRenderer>();
                        pieceSprite.color = Color.white;//駒の色を戻す
                        masterLog.text = "移動終了";
                        isPromotion = false;
                        foreach (var col in piecesCollider)//駒の判定を戻す
                        {
                            col.enabled = true;
                        }
                        pieceStatus[tmp].piecePosition = new Vector2(Mathf.Round(pieces[tmp].transform.localPosition.x), Mathf.Round(pieces[tmp].transform.localPosition.y)) / gridSize + new Vector2(4, 4);
                        Debug.Log(pieceStatus[tmp].piecePosition);

                        break;
                    }
                    else if (gridStatus[i].isSelect && !MoveLimit(i))
                    {
                        pieceStatus[tmp].isSelect = false;//駒の選択を解除
                        gridStatus[i].isSelect = false;//マスの選択を解除
                        isMoveStage = false;//他の駒を選択できない状態を解除
                        SpriteRenderer pieceSprite = pieces[tmp].GetComponent<SpriteRenderer>();
                        pieceSprite.color = Color.white;//駒の色を戻す
                        masterLog.text = "そこは移動できません";
                        foreach (var col in piecesCollider)//駒の判定を戻す
                        {
                            col.enabled = true;
                        }
                        break;
                    }
                }
            }
            else if (isMoveStage && isPawnPlay)//持ち駒を置く
            {
                for(int i = 0;i < gridStatus.Length; i++)
                {
                    if (gridStatus[i].isSelect)
                    {
                        PawnPlay(i);
                        foreach (var col in piecesCollider)//駒の判定を戻す
                        {
                            col.enabled = true;
                        }
                        SpriteRenderer pieceSprite = pieces[tmp].GetComponent<SpriteRenderer>();
                        pieceSprite.color = Color.white;//駒の色を戻す
                        isMoveStage = false;//移動状態を解除
                        isPawnPlay = false;//持ち駒をだす状態を解除
                        pieceStatus[tmp].isSelect = false;//駒の選択を解除
                        gridStatus[i].isSelect = false;//マスの選択を解除
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
                        isMoveStage = true;//他の駒を選択できなくする
                        tmp = i;//選択された駒の番号を保存
                        SpriteRenderer pieceSprite = pieces[tmp].GetComponent<SpriteRenderer>();//駒の色を変えられる状態にする
                        pieceSprite.color = Color.gray;//駒の色を変える
                        foreach (var col in piecesCollider)//駒の判定を消す
                        {
                            col.enabled = false;
                        }
                        Debug.Log("移動モードに移行");
                        break;
                    }
                    else if (pieceStatus[i].CheckSelected())
                    {
                        IndicatePoint();
                        tmp = i;
                        isMoveStage = true;
                        isPawnPlay = true;
                        SpriteRenderer pieceSprite = pieces[tmp].GetComponent<SpriteRenderer>();//駒の色を変えられる状態にする
                        pieceSprite.color = Color.gray;//駒の色を変える
                        foreach (var col in piecesCollider)//駒の判定を消す
                        {
                            col.enabled = false;
                        }
                        Debug.Log("移動モードに移行");
                        break;
                    }
                }
            }
        }
        bool al = true;
        for (int i = 0; i < pieceStatus.Length; i++)//駒が動いているならターンを変える
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
        for (int i = 0; i < pieceStatus.Length; i++)//どちらかが王将を取っているならゲーム終了
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
        if (Input.GetKeyDown(KeyCode.Escape))//エスケープが押されたらゲームを閉じる
        {
            Application.Quit();
        }
    }

    public bool MoveLimit(int gridNumber)//移動可能先かどうかを確かめる
    {
        isCanMove = false;//動くかどうか
        StartCoroutine(TestCoroutine(gridNumber));
        
        return isCanMove;//移動可能かどうかを返す
    }
    public IEnumerator TestCoroutine(int gridNumber)
    {
        int getPawn = -1;//駒を取る場合の取る駒の番号
        Vector3 deltaPosition;//ココと駒の位置が合えば移動可能
        PieceStatus selectedPiece = pieceStatus[tmp];//今選ばれている駒
        GridStatus distinationgrid = gridStatus[gridNumber];//行き先のマス
        List<Vector3> limit = selectedPiece.distination;//移動可能な移動先
        for (int i = 0; i < pieceStatus.Length; i++)//ココからマスに駒が乗っているかどうかを確認
        {
            if (distinationgrid.myPosition == pieceStatus[i].piecePosition && pieceStatus[i].player == pieceStatus[tmp].player)
                //乗っているなら移動不可にする
            {
                isCanMove = false;
                yield break;
            }
            else if (distinationgrid.myPosition == pieceStatus[i].piecePosition && pieceStatus[i].player != pieceStatus[tmp].player)
                //敵の駒なら勝負
            {
                if (pieceStatus[i].piecePoint > pieceStatus[tmp].piecePoint)
                    getPawn = tmp;
                else
                    getPawn = i;
            }
            if (selectedPiece.type == 11 || selectedPiece.type == 12 || selectedPiece.type == 20 || selectedPiece.type == 23)//香車と飛車の処理
            {
                if (distinationgrid.myPosition.x - selectedPiece.piecePosition.x != 0 && distinationgrid.myPosition.y - selectedPiece.piecePosition.y == 0)
                {
                    Debug.Log("x方向の移動を確認");
                    for (float j = 0; Mathf.Abs(j) < Mathf.Abs(distinationgrid.myPosition.x - selectedPiece.piecePosition.x) - 1; j += (distinationgrid.myPosition.x - selectedPiece.piecePosition.x) / Mathf.Abs(distinationgrid.myPosition.x - selectedPiece.piecePosition.x))//ココから間に駒があるかどうか確認
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
                    Debug.Log("y方向の移動を確認");
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
            else if ((selectedPiece.type == 10 || selectedPiece.type == 22) && distinationgrid.myPosition.x - selectedPiece.piecePosition.x != 0 && distinationgrid.myPosition.y - selectedPiece.piecePosition.y != 0)//角行と竜馬の処理
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
        for (int i = 0; i < limit.Count; i++)//選択したマスが移動可能かここで確認
        {
            deltaPosition = new Vector2(distinationgrid.myPosition.x - limit[i].x, distinationgrid.myPosition.y - limit[i].y);//移動可能かどうか計算
            if (deltaPosition == selectedPiece.piecePosition)//移動可能ならそれを伝える
            {
                isCanMove = true;
                break;
            }
        }
        if (((distinationgrid.myPosition.y >= 6 && selectedPiece.player == -1) || (distinationgrid.myPosition.y <= 2 && selectedPiece.player == 1))
                            && selectedPiece.canPromotion && isCanMove && !isPromotion)//ここで成るかどうかを判定
        {
            sel.OnClick();
            isCanTouch = false;
            isCanMove = false;
            isPromotion = true;
            distinationgrid.isSelect = false;
            Debug.Log("待機開始");
            yield return new WaitUntil(() => isCanTouch);
            distinationgrid.isSelect = true;
            isCanMove = true;
        }
        if (getPawn != -1 && isCanMove)//ココから駒を取る処理
        {
            pawn.Add(getPawn);
            int num = 1;
            pieceStatus[getPawn].player *= -1;

            if (pieceStatus[getPawn].player == -1)
                num = 0;
            pieceStatus[getPawn].transform.localPosition = new Vector2((pawnQuentity[num] % 9 - 4) * pieceStatus[getPawn].player, (pawnQuentity[num] / 9 + 5) * pieceStatus[getPawn].player);
            if (pieceStatus[getPawn].type >= 21)//なっている駒を取った場合戻す
            {
                SpriteRenderer pieceSprite = pieces[getPawn].GetComponent<SpriteRenderer>();
                (pieceStatus[getPawn].type, pieceStatus[getPawn].promotionType, pieceSprite.sprite, pieceStatus[getPawn].promotionSprite)
                    = (pieceStatus[getPawn].promotionType, pieceStatus[getPawn].type, pieceStatus[getPawn].promotionSprite, pieceSprite.sprite);

            }
            pieceStatus[getPawn].PieceInitialize();
            pawnQuentity[num]++;
            if (pieceStatus[getPawn].type == 16)//取ったのが王将なら
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

    public void CheckPromotion(bool a)//成るときの処理、a==trueなら成る
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

    public void PawnPlay(int number) //持ち駒を出す処理
    {

        for (int i = 0; i < pieceStatus.Length; i++)//置けるかどうか
        {
            if (gridStatus[number].myPosition == pieceStatus[i].piecePosition)
            {
                masterLog.text = "そこには置けません";
                return;
            }
            else if (gridStatus[number].myPosition.x == pieceStatus[i].piecePosition.x
                && pieceStatus[i].piecePosition.y <= 8 && pieceStatus[i].piecePosition.y >= 0
                && pieceStatus[i].type >= 1 && pieceStatus[i].type <= 9
                && pieceStatus[i].player == pieceStatus[tmp].player
                && pieceStatus[tmp].type >= 1 && pieceStatus[tmp].type <= 9)

            {
                masterLog.text = "二歩です";
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

    private void GameEndEffect(int winner)//勝利時のエフェクト
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
        for(int i = 0; i < pieces.Length;i++)//駒の配置を元に戻す
        {
            if (pieceStatus[i].type > 20)//なっている駒を戻す
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
