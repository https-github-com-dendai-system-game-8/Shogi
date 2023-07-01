//using OpenCover.Framework.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;
using Unity.VisualScripting;

public class EditManager : MonoBehaviour
{
    private string[] statusStr;//�e�R�}�̃|�C���g(������)
    public int[] status = new int[20];//�e�R�}�̃|�C���g(����)
    private string path;//�f�b�L��ۑ�����t�@�C��
    public int deckID = 0;//�ҏW����f�b�L�̔ԍ�
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
        for (int i = 0; i < status.Length; i++)
            status[i] = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
        if (!canvasActive.isOpen)
        {
            if (isStart == true)
            {
                isStart = false;
                Save();
            }
            return;
        }
        else if (!isStart)
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
        for (int i = 0; i < status.Length - 1; i++)
            sum += status[i];
        if (status[pieceID] < 0)
            status[pieceID] = 0;
        else if (status[pieceID] > pieceMax || sum > deckMax)
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
        text.text = "\n�c��|�C���g:" + status[20];

        if (Input.GetKeyDown(KeyCode.M))
        {
            Save();
        }
        
    }

    public void GetPieceData(int id,int max,int type)
    {
        pieceID = id;
        pieceMax = max;
        pieceType = type;
    }

    public void GetDeckID(int id)
    {
        deckID = id;
    }

    private void Save()
    {
        for (int i = 0; i < statusStr.Length; i++)
            statusStr[i] = Convert.ToString(status[i]);
        File.WriteAllLines(path, statusStr);
    }

    private void ChangeValue()
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

    private void NewStart()
    {
        path = Application.dataPath + "/TextFile/pieceStatus" + deckID + ".txt";

        if (File.Exists(path))
            statusStr = File.ReadAllLines(path);
        else
        {
            new StreamWriter(path);
            statusStr = new string[21];
            for (int i = 0; i < statusStr.Length; i++)
            {
                statusStr[i] = "0";
            }
        }
        status = new int[statusStr.Length];
        for (int i = 0; i < status.Length; i++)
            status[i] = Convert.ToInt32(statusStr[i]);
        text = FindObjectOfType<Text>();
        Debug.Log(status[0]);
        isStart = true;
    }

}
