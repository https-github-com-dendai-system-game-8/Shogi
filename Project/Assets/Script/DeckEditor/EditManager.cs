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

    private string[] statusStr;//�e�R�}�̃|�C���g(������)�ƃf�b�L��
    public int[] status = {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0};//�e�R�}�̃|�C���g(����)
    private string path;//�f�b�L��ۑ�����t�@�C��
    public DeckData[] deckData = new DeckData[10];//�ҏW����f�b�L�̃f�[�^
    private int nowDeckID;
    private InputField deckNameField;//�f�b�L������͂���ꏊ
    public int deckMax = 200;
    public int pieceID = 0;//�ҏW�����̔ԍ�
    public int pieceMax = 0;//�ҏW�����̍ő�l
    public int pieceType = 0;//�ҏW�����̎��
    private int time = 0;//��������������
    private Text text;//�ҏW���̃f�[�^��\������e�L�X�g
    private CanvasActive canvasActive;//������g���ĕҏW�\���ǂ����𔻒f
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
        
        if (!canvasActive.isOpen)//�f�b�L��ҏW�����ʂ��Ƃ��Ă�����
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
        else if (!isStart)//�J���Ƃ��ɏ�����
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
        for (int i = 0; i < status.Length - 1; i++)//���v�|�C���g���v�Z���c�肩�����
            sum += status[i];
        if (status[pieceID] < 0)//0�������Ȃ��悤�ɂ���
            status[pieceID] = 0;
        else if (status[pieceID] > pieceMax || sum > deckMax)//���v�l�������͌X�̌��E�𒴂��Ȃ��悤�ɂ���
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
        text.text = "�c��|�C���g:" + status[20];
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

    private void Save()//�ۑ����鏈��
    {
        deckData[nowDeckID].deckName = deckNameField.text;
        for (int i = 0; i < status.Length; i++)
            statusStr[i] = Convert.ToString(status[i]);
        statusStr[^1] = deckData[nowDeckID].deckName;
        File.WriteAllLines(path, statusStr);
    }

    private void ChangeValue()//�l��ω������鏈��
    {
        Debug.Log("�ω����イ");
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

    private void NewStart()//�f�b�L�ҏW��ʂ̏�����
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

    public void BackTitle()//�^�C�g���ɖ߂�
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
