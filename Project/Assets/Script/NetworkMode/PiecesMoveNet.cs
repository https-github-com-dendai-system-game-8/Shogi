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
    [SerializeField] GameObject gridCollider;//マスの当たり判定
    [SerializeField] private Text playerLog;//プレイヤーごとのログ
    public bool isMoveStage = false;//駒を選択しているかどうか
    public bool isPawnPlay = false;//持ち駒を選択しているかどうか
    public bool isCanTouch = false;//触れるかどうか
    public int turn = -1;//どちらのターンか
    public static float gridSize = 1f;//マスのサイズ
    private int[] pawnQuentity = { 0, 0 };//取った駒の数
    public GameObject[] pieces;//駒
    public PieceStatusNet[] pieceStatus;//駒のステータス
    private PieceStatusNet tmpPieceStatus;
    private Collider[] piecesCollider;//駒の判定
    private GameObject[] grid;//マス
    public PlayerManager[] playerManager;//自分と相手のデータ
    [HideInInspector] public GridStatusNet[] gridStatus;//マスのステータス
    private List<int> pawn = new List<int>();//取った駒の番号
    private int tmp = 0;//選択した駒の番号
    [SerializeField] private Select sel;//選択肢を出す

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

                    if (gridStatus[i].isSelect && MoveLimit(i))//移動先の選択が完了していたら移動させる
                    {
                        pieces[tmp].transform.localPosition = grid[i].transform.localPosition;//移動させる
                        pieceStatus[tmp].isSelect = false;//駒の選択を解除
                        gridStatus[i].isSelect = false;//マスの選択を解除
                        isMoveStage = false;//他の駒を選べない状態を解除
                        SpriteRenderer pieceSprite = pieces[tmp].GetComponent<SpriteRenderer>();
                        pieceSprite.color = Color.white;//駒の色を戻す
                        Debug.Log("移動終わったよ");
                        foreach (var col in piecesCollider)//駒の判定を戻す
                        {
                            col.enabled = true;
                        }
                        pieceStatus[tmp].piecePosition = new Vector2(Mathf.Round(pieces[tmp].transform.localPosition.x), Mathf.Round(pieces[tmp].transform.localPosition.y)) / gridSize + new Vector2(4, 4);
                        Debug.Log(pieceStatus[tmp].piecePosition);
                        if (((pieceStatus[tmp].piecePosition.y >= 6 && pieceStatus[tmp].player == -1) || (pieceStatus[tmp].piecePosition.y <= 2 && pieceStatus[tmp].player == 1))
                            && pieceStatus[tmp].canPromotion)//成れるかどうか
                        {
                            sel.OnClick();
                            isCanTouch = false;
                        }
                        if (playerManager == null)//プレイヤーデータがない場合手に入れる
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
                        pieceStatus[tmp].isSelect = false;//駒の選択を解除
                        gridStatus[i].isSelect = false;//マスの選択を解除
                        isMoveStage = false;//他の駒を選択できない状態を解除
                        SpriteRenderer pieceSprite = pieces[tmp].GetComponent<SpriteRenderer>();
                        pieceSprite.color = Color.white;//駒の色を戻す
                        Debug.Log("そこは移動できません\n移動モード解除");
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
                for (int i = 0; i < gridStatus.Length; i++)
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
            else//何もしていない状態
            {
                for (int i = 0; i < pieceStatus.Length; i++)
                {
                    if (pieceStatus[i].CheckSelected() && pieceStatus[i].piecePosition.y >= 0 && pieceStatus[i].piecePosition.y <= 8)
                    //場の駒を選んだ場合
                    {

                        isMoveStage = true;//他の駒を選択できなくする
                        tmp = i;//選択された駒の番号を保存
                        SpriteRenderer pieceSprite = pieces[tmp].GetComponent<SpriteRenderer>();//駒の色を変えられる状態にする
                        pieceSprite.color = Color.gray;//駒の色を変える
                        foreach (var col in piecesCollider)//駒の判定を消す
                        {
                            col.enabled = false;
                        }
                        Debug.Log("移動モードに移行");
                        tmpPieceStatus = pieceStatus[tmp];
                        break;
                    }
                    else if (pieceStatus[i].CheckSelected())//持ち駒を選んだ場合
                    {
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
                        tmpPieceStatus = pieceStatus[tmp];
                        break;
                    }
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
        int getPawn = -1;
        Vector3 deltaPosition;//ココと駒の位置が合えば移動可能
        PieceStatusNet selectedPiece = pieceStatus[tmp];//今選ばれている駒
        GridStatusNet tmpgrid = gridStatus[gridNumber];//行き先のマス
        List<Vector3> limit = selectedPiece.distination;//移動可能な移動先
        for (int i = 0; i < pieceStatus.Length; i++)//ココからマスに駒が乗っているかどうかを確認
        {
            if (tmpgrid.myPosition == pieceStatus[i].piecePosition && pieceStatus[i].player == pieceStatus[tmp].player)//乗っているなら移動不可にする
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
                    Debug.Log("x方向の移動を確認");
                    for (float j = 0; Mathf.Abs(j) < Mathf.Abs(tmpgrid.myPosition.x - selectedPiece.piecePosition.x) - 1; j += (tmpgrid.myPosition.x - selectedPiece.piecePosition.x) / Mathf.Abs(tmpgrid.myPosition.x - selectedPiece.piecePosition.x))//ココから間に駒があるかどうか確認
                    {
                        if (pieceStatus[i].piecePosition.x == j + selectedPiece.piecePosition.x + (tmpgrid.myPosition.x - selectedPiece.piecePosition.x) / Mathf.Abs(tmpgrid.myPosition.x - selectedPiece.piecePosition.x) && tmpgrid.myPosition.y - pieceStatus[i].piecePosition.y == 0)
                        {
                            return false;
                        }
                    }
                }
                if (tmpgrid.myPosition.y - selectedPiece.piecePosition.y != 0 && tmpgrid.myPosition.x - selectedPiece.piecePosition.x == 0)
                {
                    Debug.Log("y方向の移動を確認");
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
        for (int i = 0; i < limit.Count; i++)//選択したマスが移動可能かここで確認
        {
            deltaPosition = new Vector2(tmpgrid.myPosition.x - limit[i].x, tmpgrid.myPosition.y - limit[i].y);//移動可能かどうか計算
            if (deltaPosition == selectedPiece.piecePosition)//移動可能ならそれを伝える
            {
                isCanMove = true;
                break;
            }
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
            pieceStatus[getPawn].CheckMove();
            pawnQuentity[num]++;
            if (pieceStatus[getPawn].type == 16)
            {
                GameEndEffect();
                return false;
            }

        }
        return isCanMove;//移動可能かどうかを返す
    }

    public void CheckPromotion(bool a)//成るときの処理、a==trueなら成る
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

    private void PawnPlay(int number) //持ち駒を出す処理
    {

        for (int i = 0; i < pieceStatus.Length; i++)//置けるかどうか
        {
            if (gridStatus[number].myPosition == pieceStatus[i].piecePosition)
            {
                Debug.Log("そこには置けません");
                return;
            }
            else if (gridStatus[number].myPosition.x == pieceStatus[i].piecePosition.x
                && pieceStatus[i].piecePosition.y <= 8 && pieceStatus[i].piecePosition.y >= 0
                && pieceStatus[i].type >= 1 && pieceStatus[i].type <= 9
                && pieceStatus[i].player == pieceStatus[tmp].player
                && pieceStatus[tmp].type >= 1 && pieceStatus[tmp].type <= 9)

            {
                Debug.Log("二歩です");
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
        playerLog.text = "YouWin!";//勝利ログ
        for (int i = 0; i < pieces.Length; i++)//ここで駒を元に戻す
        {
            if (pieceStatus[i].type > 20)//成っている駒を戻す
            {
                SpriteRenderer sp = pieces[i].GetComponent<SpriteRenderer>();
                (pieceStatus[i].promotionSprite, sp.sprite) = (sp.sprite, pieceStatus[i].promotionSprite);
                (pieceStatus[i].type, pieceStatus[i].promotionType) = (pieceStatus[i].promotionType, pieceStatus[i].type);
            }
            pieceStatus[i].player = pieceStatus[i].holder;//駒を持ち主に戻す
            pieceStatus[i].CheckMove();//駒の設定を戻す
            pieces[i].transform.localPosition = (pieceStatus[i].startPosition - new Vector3(4, 4)) * gridSize;//駒の位置を戻す
            pieceStatus[i].piecePosition = (pieces[i].transform.localPosition) / gridSize + new Vector3(4, 4);//駒の位置情報を戻す
        }
        isCanTouch = true;
    }
    public override void OnConnectedToMaster()
    {
        var roomOption = new RoomOptions();
        roomOption.MaxPlayers = 2;
        // "Room"という名前のルームに参加する（ルームが存在しなければ作成して参加する）
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
