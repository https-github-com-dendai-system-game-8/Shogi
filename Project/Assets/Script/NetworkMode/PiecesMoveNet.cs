using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UIElements;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;

public class PiecesMoveNet : PiecesMove
{
    [SerializeField] private Text playerLogNet;//プレイヤーごとのログ
    public PlayerManager[] playerManager;//自分と相手のデータ

    // Start is called before the first frame update
    void Start()
    {
        Initialize();
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
                        masterLog.text = "移動終了";
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
                        SpriteRenderer[] gridSprite = new SpriteRenderer[grid.Length];//マスの色を戻せるようにする
                        for (int k = 0; k < grid.Length; k++)
                        {
                            gridSprite[k] = grid[k].transform.Find("ステージ選択個別背景").gameObject.GetComponent<SpriteRenderer>();
                            gridSprite[k].color = Color.white;//マスの色を戻す
                        }
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
                        masterLog.text = pieceStatus[tmp].role + "が選択されています";
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
                        SpriteRenderer[] gridSprite = new SpriteRenderer[pieceStatus[tmp].distination.Count];//マスの色を変えられる状態にする
                        int j = 0;
                        for (int k = 0; k < grid.Length; k++)
                        {
                            for (int l = 0; l < pieceStatus[tmp].distination.Count; l++)
                            {
                                if (gridStatus[k].myPosition - pieceStatus[tmp].piecePosition == pieceStatus[tmp].distination[l])
                                    gridSprite[j++] = grid[k].transform.Find("ステージ選択個別背景").GetComponent<SpriteRenderer>();
                            }

                        }
                        for (int k = 0; k < gridSprite.Length; k++)//色を変える
                        {
                            if (gridSprite[k] != null)
                            {
                                gridSprite[k].color = Color.gray;
                            }
                        }
                        masterLog.text = pieceStatus[tmp].role + "が選択されています";
                        break;
                    }
                }
            }
        }
        bool al = true;
        for (int i = 0; i < pieceStatus.Length; i++)
        {
            pieceStatus[i].piecePosition = new Vector2(Mathf.Round(pieces[i].transform.localPosition.x), Mathf.Round(pieces[i].transform.localPosition.y)) / gridSize + new Vector2(4, 4);
            if (beforePosition[i] != pieceStatus[i].piecePosition && al)//ターンを切り替える
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
            GameEndEffect();
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
            pieceStatus[tmp].PieceInitialize();
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
            pieceStatus[i].PieceInitialize();//駒の設定を戻す
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
