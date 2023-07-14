using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.EventSystems;
using System.IO;
using UnityEditor;
using Unity.VisualScripting;
using UnityEngine.UIElements;
using System;

public class PieceStatus : MonoBehaviour,IPointerClickHandler
{
    public Vector3 startPosition;//��̏����ʒu
    public List<Vector3> distination;//��̈ړ��\��
    public int type;//��̎��
    public int player;//�ǂ��炪��������
    public int holder;//���X�̎�����
    public bool canPromotion = true;//����邩�ǂ���
    public int promotionType;//��������̃^�C�v
    public Sprite promotionSprite;//��������̌�����
    public int pieceID;//������ʂ���ԍ�
    public float piecePoint = 0;//��Ɋ���U��ꂽ�|�C���g
    public int[] deckNum = {0,1};//�f�b�L�̔ԍ�
    public string role;//��̖��O

    public bool isSelect = false;//���̋�I�΂�Ă��邩�ǂ���
    public Vector3 piecePosition;//���݂̋�̈ʒu

    private bool safe = false;//�I���ł����Ԃ��ǂ���
    private int grLen;//�}�X�̐�

    // Start is called before the first frame update

    private void Start()
    {
        deckNum[0] = DeckNumber.selectedNumber[0];
        deckNum[1] = DeckNumber.selectedNumber[1];
        PieceInitialize();
        piecePosition = startPosition;
        transform.localPosition = (piecePosition - new Vector3(4, 4)) * PiecesMove.gridSize;
        pieceID = holder * type;
        grLen = GameObject.FindGameObjectsWithTag("Grid").Length;
        int p = 0;
        if (holder == 1)
            p = 1;
        piecePoint = Convert.ToSingle(File.ReadAllLines(Application.dataPath + "/Resources/pieceStatus" + deckNum[p] + ".txt")[type]);
    }

    void Update()
    {
        piecePosition =new Vector3(Mathf.Round(transform.localPosition.x),Mathf.Round(transform.localPosition.y)) / PiecesMove.gridSize + new Vector3(4, 4);
        for(int i = 0;i < grLen;i++)
            transform.localPosition = new Vector3(transform.localPosition.x,transform.localPosition.y,transform.localPosition.z);
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (safe)
        {
            isSelect = true;
            Debug.Log("�I�΂ꂽ");
        }
    }

    public bool CheckSelected()
    {
        return isSelect;
    }

    public void IsSafe(bool isMoveStage,bool isCanTouch, int turn)
    {
        safe = !isMoveStage && isCanTouch && turn == player;
    }

    public void PieceInitialize()
    {
        switch (type)//�^�C�v���Ƃɐݒ��ς���
        {
            case 1:
                startPosition = new Vector3(8, 2);
                distination.Clear();
                canPromotion = true;
                promotionType = 21;
                distination.Add(new Vector3(0, 1));
                role = "����";
                break;
            case 2:
                startPosition = new Vector3(7, 2);
                distination.Clear();
                canPromotion = true;
                promotionType = 21;
                distination.Add(new Vector3(0, 1));
                role = "����";
                break;
            case 3:
                startPosition = new Vector3(6, 2);
                distination.Clear();
                canPromotion = true;
                promotionType = 21;
                distination.Add(new Vector3(0, 1));
                role = "����";
                break;
            case 4:
                startPosition = new Vector3(5, 2);
                distination.Clear();
                canPromotion = true;
                promotionType = 21;
                distination.Add(new Vector3(0, 1));
                role = "����";
                break;
            case 5:
                startPosition = new Vector3(4, 2);
                distination.Clear();
                canPromotion = true;
                promotionType = 21;
                distination.Add(new Vector3(0, 1));
                role = "����";
                break;
            case 6:
                startPosition = new Vector3(3, 2);
                distination.Clear();
                canPromotion = true;
                promotionType = 21;
                distination.Add(new Vector3(0, 1));
                role = "����";
                break;
            case 7:
                startPosition = new Vector3(2, 2);
                distination.Clear();
                canPromotion = true;
                promotionType = 21;
                distination.Add(new Vector3(0, 1));
                role = "����";
                break;
            case 8:
                startPosition = new Vector3(1, 2);
                distination.Clear();
                canPromotion = true;
                promotionType = 21;
                distination.Add(new Vector3(0, 1));
                role = "����";
                break;
            case 9:
                startPosition = new Vector3(0, 2);
                distination.Clear();
                canPromotion = true;
                promotionType = 21;
                distination.Add(new Vector3(0, 1));
                role = "����";
                break;
            case 10:
                startPosition = new Vector3(1, 1);
                distination.Clear();
                canPromotion = true;
                promotionType = 22;
                for (int n = -8;n < 9; n++)
                {
                    distination.Add(new Vector3(n, n));
                    distination.Add(new Vector3(n, -n));
                }
                role = "�p�s";
                break;
            case 11:
                startPosition = new Vector3(7, 1);
                distination.Clear();
                canPromotion = true;
                promotionType = 23;
                for (int n = -8;n < 9; n++)
                {
                    distination.Add(new Vector3(n, 0));
                    distination.Add(new Vector3(0, n));
                }
                role = "���";
                break;
            case 12:
                startPosition = new Vector3(8, 0);
                distination.Clear();
                canPromotion = true;
                promotionType = 21;
                for (int n = 0; n < 9; n++) 
                    distination.Add(new Vector3(0, n));
                role = "����";
                break;
            case 13:
                startPosition = new Vector3(7, 0);
                distination.Clear();
                canPromotion = true;
                promotionType = 21;
                distination.Add(new Vector3(1, 2));
                distination.Add(new Vector3(-1, 2));
                role = "�j�n";
                break;
            case 14:
                startPosition = new Vector3(6, 0);
                distination.Clear();
                canPromotion = true;
                promotionType = 21;
                distination.Add(new Vector3(-1, 1));
                distination.Add(new Vector3(0, 1));
                distination.Add(new Vector3(1, 1));
                distination.Add(new Vector3(-1, -1));
                distination.Add(new Vector3(1, -1));
                role = "�⏫";
                break;
            case 15:
                startPosition = new Vector3(5, 0);
                distination.Clear();
                distination.Add(new Vector3(-1, 1));
                distination.Add(new Vector3(0, 1));
                distination.Add(new Vector3(-1, 0));
                distination.Add(new Vector3(1, 1));
                distination.Add(new Vector3(-0, -1));
                distination.Add(new Vector3(1, 0));
                canPromotion = false;
                role = "����";
                break;
            case 16:
                startPosition = new Vector3(4, 0);
                promotionType = 30;
                distination.Clear();
                distination.Add(new Vector3(-1, 1));
                distination.Add(new Vector3(0, 1));
                distination.Add(new Vector3(-1, 0));
                distination.Add(new Vector3(1, 1));
                distination.Add(new Vector3(-0, -1));
                distination.Add(new Vector3(1, 0));
                distination.Add(new Vector3(-1, -1));
                distination.Add(new Vector3(1, -1));
                canPromotion = false;
                role = "����";
                break;
            case 17:
                startPosition = new Vector3(3, 0);
                distination.Clear();
                distination.Add(new Vector3(-1, 1));
                distination.Add(new Vector3(0, 1));
                distination.Add(new Vector3(-1, 0));
                distination.Add(new Vector3(1, 1));
                distination.Add(new Vector3(-0, -1));
                distination.Add(new Vector3(1, 0));
                canPromotion = false;
                role = "����";
                break;
            case 18:
                startPosition = new Vector3(2, 0);
                distination.Clear();
                canPromotion = true;
                promotionType = 21;
                distination.Add(new Vector3(-1, 1));
                distination.Add(new Vector3(0, 1));
                distination.Add(new Vector3(1, 1));
                distination.Add(new Vector3(-1, -1));
                distination.Add(new Vector3(1, -1));
                role = "�⏫";
                break;
            case 19:
                startPosition = new Vector3(1, 0);
                distination.Clear();
                canPromotion = true;
                promotionType = 21;
                distination.Add(new Vector3(1, 2));
                distination.Add(new Vector3(-1, 2));
                role = "�j�n";
                break;
            case 20:
                startPosition = new Vector3(0, 0);
                distination.Clear();
                canPromotion = true;
                promotionType = 21;
                for (int n = -8;n < 9; n++)
                {
                    distination.Add(new Vector3(0, Mathf.Abs(n)));
                }
                role = "����";
                break;
            case 21:
                startPosition = new Vector3(0, 0);
                distination.Add(new Vector3(-1, 1));
                distination.Add(new Vector3(0, 1));
                distination.Add(new Vector3(-1, 0));
                distination.Add(new Vector3(1, 1));
                distination.Add(new Vector3(-0, -1));
                distination.Add(new Vector3(1, 0));
                canPromotion = false;
                break;
            case 22:
                startPosition = new Vector3(0, 0);
                for (int n = -8; n < 9; n++)
                {
                    distination.Add(new Vector3(n, n));
                    distination.Add(new Vector3(n, -n));
                }
                distination.Add(new Vector3(1, 0));
                distination.Add(new Vector3(0, -1));
                distination.Add(new Vector3(-1, 0));
                distination.Add(new Vector3(0, 1));
                canPromotion = false;
                break;
            case 23:
                startPosition = new Vector3(0, 0);
                for (int n = -8; n < 9; n++)
                {
                    distination.Add(new Vector3(n, 0));
                    distination.Add(new Vector3(0, n));
                }
                distination.Add(new Vector3(1, 1));
                distination.Add(new Vector3(1, -1));
                distination.Add(new Vector3(-1, -1));
                distination.Add(new Vector3(-1, 1));
                canPromotion = false;
                break;
            case 30:
                startPosition = new Vector3(4, 0);
                promotionType = 30;
                distination.Clear();
                distination.Add(new Vector3(-1, 1));
                distination.Add(new Vector3(0, 1));
                distination.Add(new Vector3(-1, 0));
                distination.Add(new Vector3(1, 1));
                distination.Add(new Vector3(-0, -1));
                distination.Add(new Vector3(1, 0));
                distination.Add(new Vector3(-1, -1));
                distination.Add(new Vector3(1, -1));
                canPromotion = false;
                break;
            default:
                break;

        }
        distination.Remove(new Vector3(0, 0));
        SpriteRenderer psp = GetComponent<SpriteRenderer>();
        if (player == 1)
        {
            for(int i = 0;i < distination.Count; i++)
            {
                distination[i] *= -1;
            }
            psp.flipX = true;
            psp.flipY = true;
            startPosition -= new Vector3(4, 4);
            startPosition *= -1;
            startPosition += new Vector3(4, 4);
            gameObject.layer = 3;
            
        }
        else if(player == -1)
        {
            psp.flipX = false;
            psp.flipY = false;
            gameObject.layer = 0;
                
        }
    }


}
