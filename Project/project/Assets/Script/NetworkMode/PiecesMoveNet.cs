using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UIElements;
using Unity.VisualScripting;

public class PiecesMoveNet : PiecesMove
{
    [SerializeField] private Text playerLogNet;//プレイヤーごとのログ

    //public PieceStatusNet[] pieceStatus;//駒のステータス

    public PlayerManager[] playerManager;//自分と相手のデータ
   //[HideInInspector] public GridStatusNet[] gridStatus;//マスのステータス

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
        pieceStatus = new PieceStatus[pieces.Length];
        piecesCollider = new Collider[pieces.Length];
        beforePosition = new Vector3[pieces.Length];
        for (int i = 0; i < pieces.Length; i++)
        {
            pieceStatus[i] = pieces[i].GetComponent<PieceStatus>();
            piecesCollider[i] = pieces[i].GetComponent<Collider>();
        }
        gridStatus = new GridStatus[grid.Length];
        for (int i = 0; i < grid.Length; i++)
        {
            gridStatus[i] = grid[i].GetComponent<GridStatus>();
            gridStatus[i].myPosition = new Vector3(grid[i].transform.localPosition.x / gridSize + 4, grid[i].transform.localPosition.y / gridSize + 4);
        }
        for (int i = 0; i < pieceStatus.Length; i++)
        {
            pieceStatus[i].CheckMove();
            beforePosition[i] = pieceStatus[i].startPosition;
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
                        isPromotion = false;
                        foreach (var col in piecesCollider)//駒の判定を戻す
                        {
                            col.enabled = true;
                        }
                        pieceStatus[tmp].piecePosition = new Vector2(Mathf.Round(pieces[tmp].transform.localPosition.x), Mathf.Round(pieces[tmp].transform.localPosition.y)) / gridSize + new Vector2(4, 4);
                        Debug.Log(pieceStatus[tmp].piecePosition);
                        if (playerManager == null)//プレイヤーデータがない場合手に入れる
                        {
                            PlayerManager[] tmpPm;
                            tmpPm = FindObjectsOfType<PlayerManager>();
                            playerManager = tmpPm;
                            break;
                                
                            
                        }
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
                        break;
                    }
                }
            }
        }
        bool al = true;
        for (int i = 0; i < pieceStatus.Length; i++)
        {
            pieceStatus[i].piecePosition = new Vector2(Mathf.Round(pieces[i].transform.localPosition.x), Mathf.Round(pieces[i].transform.localPosition.y)) / gridSize + new Vector2(4, 4);
            if (beforePosition[i] != pieceStatus[i].piecePosition && al && !(pieceStatus[i].piecePosition.y >= 9 || pieceStatus[i].piecePosition.y <= 0))//ターンを切り替える
            {
                Debug.Log("change");
                turn *= -1;
                al = false;
            }
            beforePosition[i] = pieceStatus[i].piecePosition;
            PieceSafe(i);
        }
        for (int i = 0; i < gridStatus.Length; i++)
        {
            GridSafe(i);
        }
        if (Input.GetKeyDown(KeyCode.Escape))//エスケープが押されたらゲームを閉じる
        {
            Application.Quit();
        }
    }

    private new bool MoveLimit(int gridNumber)//移動可能先かどうかを確かめる
    {
        isCanMove = false;
        StartCoroutine(TestCoroutine(gridNumber));
        for (int i = 0; i < pieceStatus.Length; i++)
        {
            if (pieceStatus[i].type == 30)
            {
                GameEndEffect();
                break;
            }


        }
        return isCanMove;
    }

   

    public new void CheckPromotion(bool a)//成るときの処理、a==trueなら成る
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

    private void GameEndEffect()
    {
        isCanTouch = false;
        playerLogNet.text = "YouWin!";//勝利ログ
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
        Vector3 position = Vector3.zero;
        PhotonNetwork.Instantiate("Avatar", position, Quaternion.identity);
    }
}
