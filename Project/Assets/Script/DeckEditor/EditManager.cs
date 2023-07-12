using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;

public class DeckData
{
    public string deckName;
    public int deckID;
    public DeckData(string n,int i)
    {
        deckName = n;
        deckID = i;
    }

    public DeckData()
    {
        deckID = 0;
        deckName = "";
    }
}
public class EditManager : MonoBehaviour
{

    private string[] statusStr;//各コマのポイント(文字列)とデッキ名
    public int[] status = {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0};//各コマのポイント(数字)
    private string path;//デッキを保存するファイル
    public DeckData[] deckData = new DeckData[10];//編集するデッキのデータ
    private int nowDeckID;
    private InputField deckNameField;//デッキ名を入力する場所
    public int deckMax = 200;
    public int pieceID = 0;//編集する駒の番号
    public int pieceMax = 0;//編集する駒の最大値
    public int pieceType = 0;//編集する駒の種類
    private int time = 0;//長押しした時間
    private Text text;//編集中のデータを表示するテキスト
    private CanvasActive canvasActive;//これを使って編集可能かどうかを判断
    private bool isStart = false;
    // Start is called before the first frame update
    void Start()
    {
        canvasActive = FindObjectOfType<CanvasActive>().GetComponent<CanvasActive>();
        for(int i = 0; i < 10; i++)
        {
            string[] str = File.ReadAllLines(Application.dataPath + "/Resources/pieceStatus" + i + ".txt");
            deckData[i] = new DeckData(str[^1],i);
        }
        deckNameField = FindObjectOfType<InputField>().GetComponent<InputField>();
        deckNameField.gameObject.SetActive(false);
        for (int i = 0; i < status.Length; i++)
            status[i] = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
        if (!canvasActive.isOpen)//デッキを編集する画面がとじていたら
        {
            if (isStart)
            {
                isStart = false;
                Save();
                deckNameField.gameObject.SetActive(false);
            }
            if (Input.GetKeyDown(KeyCode.Escape))
                BackTitle();
            return;
        }
        else if (!isStart)//開くときに初期化
        {
            NewStart();
        }
        if (Input.GetAxisRaw("Vertical3") != 0 || Input.GetAxisRaw("Horizontal3") != 0)
        {
            ChangeValue();
        }
        else
        {
            time = 0;
        }
        int sum = 0;
        for (int i = 0; i < status.Length - 1; i++)//合計ポイントを計算し残りから引く
            sum += status[i];
        if (status[pieceID] < 0)//0を下回らないようにする
            status[pieceID] = 0;
        else if (status[pieceID] > pieceMax || sum > deckMax)//合計値もしくは個々の限界を超えないようにする
        {
            while (status[pieceID] > pieceMax || sum > deckMax)
            {
                status[pieceID] -= 1;
                sum = 0;
                for (int i = 0; i < status.Length - 1; i++)
                    sum += status[i];
            }
        }
        sum = 0;
        for(int i = 0;i < status.Length - 1; i++)
            sum += status[i];
        status[20] = deckMax - sum;
        text.text = "残りポイント:" + status[20];
#if !UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.S) && (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)))
        {
            Save();
        }
#else
        if (Input.GetKeyDown(KeyCode.M))
        {
            Save();
        }
#endif

    }

    public void GetPieceData(int id,int max,int type)
    {
        pieceID = id;
        pieceMax = max;
        pieceType = type;
    }

    public void GetDeckID(int id)//
    {
        nowDeckID = id;
        deckData[nowDeckID].deckName = Application.dataPath + "/Resources/pieceStatus" + id + ".txt";
    }

    private void Save()//保存する処理
    {
        deckData[nowDeckID].deckName = deckNameField.text;
        for (int i = 0; i < status.Length; i++)
            statusStr[i] = Convert.ToString(status[i]);
        statusStr[^1] = deckData[nowDeckID].deckName;
        File.WriteAllLines(path, statusStr);
    }

    private void ChangeValue()//値を変化させる処理
    {
        Debug.Log("変化ちゅう");
        time++;
        if (time % 60 == 0)
        {
            status[pieceID] += (int)Input.GetAxisRaw("Vertical3");
            status[pieceID] += (int)Input.GetAxisRaw("Horizontal3") * 10;
        }
        else if (time == 1)
        {
            status[pieceID] += (int)Input.GetAxisRaw("Vertical3");
            status[pieceID] += (int)Input.GetAxisRaw("Horizontal3") * 10;
        }

    }

    private void NewStart()//デッキ編集画面の初期化
    {
        deckNameField.gameObject.SetActive(true);
        path = Application.dataPath + "/Resources/pieceStatus" + deckData[nowDeckID].deckID + ".txt";
        
        if (File.Exists(path))
        {
            statusStr = File.ReadAllLines(path);
            Debug.Log(statusStr[0]);
            deckData[nowDeckID].deckName = statusStr[^1];
        }
        else
        {
            new StreamWriter(path);
            statusStr = new string[22];
            int i;
            for (i = 0; i < statusStr.Length - 2; i++)
            {
                statusStr[i] = "0";
            }
            statusStr[i + 1] = Convert.ToString(deckMax);
            deckData[nowDeckID].deckName = "";
            statusStr[^1] = deckData[nowDeckID].deckName;
        }
        for (int i = 0; i < status.Length; i++)
            status[i] = Convert.ToInt32(statusStr[i]);
        text = GameObject.FindGameObjectWithTag("Player").GetComponent<Text>();
        deckData[nowDeckID].deckName = statusStr[^1];
        deckNameField.text = deckData[nowDeckID].deckName;
        isStart = true;
    }

    public void BackTitle()//タイトルに戻る
    {
        SceneManager.LoadScene("1StartScene");
    }

    public void ChangeDeckName(Text name)
    {
        deckData[nowDeckID].deckName = name.text;
        statusStr[^1] = name.text;
    }

    public void DeckPointReset()
    {
        for (int i = 0; i < status.Length; i++)
            status[i] = 0;
    }

    public void RestoreDeck()
    {
        for (int i = 0; i < status.Length; i++)
            status[i] = Convert.ToInt32(File.ReadAllLines(path)[i]);
    }
}
