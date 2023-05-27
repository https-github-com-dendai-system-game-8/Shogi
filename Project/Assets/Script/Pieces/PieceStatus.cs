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

public class PieceStatus : MonoBehaviour,IPointerClickHandler
{

    public Vector2 startPosition;//��̏����ʒu
    public List<Vector2> distination;//��̈ړ��\��
    public int type;//��̎��
    public int player;//�ǂ��炪��������
    public int holder;//���X�̎�����
    public bool canPromotion = true;//����邩�ǂ���
    public int promotionType;//��������̃^�C�v
    public Sprite promotionSprite;//��������̌�����

    public bool isSelect = false;//���̋�I�΂�Ă��邩�ǂ���
    public Vector2 piecePosition;//���݂̋�̈ʒu

    private PiecesMove pm;

    // Start is called before the first frame update

    private void Start()
    {
        CheckMove();
        piecePosition = startPosition;
        transform.localPosition = (piecePosition - new Vector2(4, 4)) * PiecesMove.gridSize;
        pm = GameObject.FindGameObjectWithTag("ShogiStage").GetComponent<PiecesMove>();
    }

    void Update()
    {
        piecePosition =new Vector2(Mathf.Round(transform.localPosition.x),Mathf.Round(transform.localPosition.y)) / PiecesMove.gridSize + new Vector2(4, 4);
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (!pm.isMoveStage && pm.isCanTouch)
        {
            isSelect = true;
            Debug.Log("�I�΂ꂽ");
        }
    }

    public bool CheckSelected()
    {
        return isSelect;
    }

    public void CheckMove()
    {
        switch (type)
        {
            case 1:
                startPosition = new Vector2(8, 2);
                distination.Clear();
                canPromotion = true;
                promotionType = 21;
                distination.Add(new Vector2(0, 1));
                break;
            case 2:
                startPosition = new Vector2(7, 2);
                distination.Clear();
                canPromotion = true;
                promotionType = 21;
                distination.Add(new Vector2(0, 1));
                break;
            case 3:
                startPosition = new Vector2(6, 2);
                distination.Clear();
                canPromotion = true;
                promotionType = 21;
                distination.Add(new Vector2(0, 1));
                
                break;
            case 4:
                startPosition = new Vector2(5, 2);
                distination.Clear();
                canPromotion = true;
                promotionType = 21;
                distination.Add(new Vector2(0, 1));
                break;
            case 5:
                startPosition = new Vector2(4, 2);
                distination.Clear();
                canPromotion = true;
                promotionType = 21;
                distination.Add(new Vector2(0, 1));
                break;
            case 6:
                startPosition = new Vector2(3, 2);
                distination.Clear();
                canPromotion = true;
                promotionType = 21;
                distination.Add(new Vector2(0, 1));
                break;
            case 7:
                startPosition = new Vector2(2, 2);
                distination.Clear();
                canPromotion = true;
                promotionType = 21;
                distination.Add(new Vector2(0, 1));
                break;
            case 8:
                startPosition = new Vector2(1, 2);
                distination.Clear();
                canPromotion = true;
                promotionType = 21;
                distination.Add(new Vector2(0, 1));
                break;
            case 9:
                startPosition = new Vector2(0, 2);
                distination.Clear();
                canPromotion = true;
                promotionType = 21;
                distination.Add(new Vector2(0, 1));
                break;
            case 10:
                startPosition = new Vector2(1, 1);
                distination.Clear();
                canPromotion = true;
                promotionType = 22;
                for (int n = -8;n < 9; n++)
                {
                    distination.Add(new Vector2(n, n));
                    distination.Add(new Vector2(n, -n));
                }
                break;
            case 11:
                startPosition = new Vector2(7, 1);
                distination.Clear();
                canPromotion = true;
                promotionType = 23;
                for (int n = -8;n < 9; n++)
                {
                    distination.Add(new Vector2(n, 0));
                    distination.Add(new Vector2(0, n));
                }
                break;
            case 12:
                startPosition = new Vector2(8, 0);
                distination.Clear();
                canPromotion = true;
                promotionType = 21;
                for (int n = 0; n < 9; n++) 
                    distination.Add(new Vector2(0, n));
                break;
            case 13:
                startPosition = new Vector2(7, 0);
                distination.Clear();
                canPromotion = true;
                promotionType = 21;
                distination.Add(new Vector2(1, 2));
                distination.Add(new Vector2(-1, 2));
                break;
            case 14:
                startPosition = new Vector2(6, 0);
                distination.Clear();
                canPromotion = true;
                promotionType = 21;
                distination.Add(new Vector2(-1, 1));
                distination.Add(new Vector2(0, 1));
                distination.Add(new Vector2(1, 1));
                distination.Add(new Vector2(-1, -1));
                distination.Add(new Vector2(1, -1));
                break;
            case 15:
                startPosition = new Vector2(5, 0);
                distination.Clear();
                distination.Add(new Vector2(-1, 1));
                distination.Add(new Vector2(0, 1));
                distination.Add(new Vector2(-1, 0));
                distination.Add(new Vector2(1, 1));
                distination.Add(new Vector2(-0, -1));
                distination.Add(new Vector2(1, 0));
                canPromotion = false;
                break;
            case 16:
                startPosition = new Vector2(4, 0);
                distination.Clear();
                distination.Add(new Vector2(-1, 1));
                distination.Add(new Vector2(0, 1));
                distination.Add(new Vector2(-1, 0));
                distination.Add(new Vector2(1, 1));
                distination.Add(new Vector2(-0, -1));
                distination.Add(new Vector2(1, 0));
                distination.Add(new Vector2(-1, -1));
                distination.Add(new Vector2(1, -1));
                canPromotion = false;
                break;
            case 17:
                startPosition = new Vector2(3, 0);
                distination.Clear();
                distination.Add(new Vector2(-1, 1));
                distination.Add(new Vector2(0, 1));
                distination.Add(new Vector2(-1, 0));
                distination.Add(new Vector2(1, 1));
                distination.Add(new Vector2(-0, -1));
                distination.Add(new Vector2(1, 0));
                canPromotion = false;
                break;
            case 18:
                startPosition = new Vector2(2, 0);
                distination.Clear();
                canPromotion = true;
                promotionType = 21;
                distination.Add(new Vector2(-1, 1));
                distination.Add(new Vector2(0, 1));
                distination.Add(new Vector2(1, 1));
                distination.Add(new Vector2(-1, -1));
                distination.Add(new Vector2(1, -1));
                break;
            case 19:
                startPosition = new Vector2(1, 0);
                distination.Clear();
                canPromotion = true;
                promotionType = 21;
                distination.Add(new Vector2(1, 2));
                distination.Add(new Vector2(-1, 2));
                break;
            case 20:
                startPosition = new Vector2(0, 0);
                distination.Clear();
                canPromotion = true;
                promotionType = 21;
                for (int n = -8;n < 9; n++)
                {
                    distination.Add(new Vector2(0, Mathf.Abs(n)));
                }
                break;
            case 21:
                startPosition = new Vector2(0, 0);
                distination.Add(new Vector2(-1, 1));
                distination.Add(new Vector2(0, 1));
                distination.Add(new Vector2(-1, 0));
                distination.Add(new Vector2(1, 1));
                distination.Add(new Vector2(-0, -1));
                distination.Add(new Vector2(1, 0));
                canPromotion = false;
                break;
            case 22:
                startPosition = new Vector2(0, 0);
                for (int n = -8; n < 9; n++)
                {
                    distination.Add(new Vector2(n, n));
                    distination.Add(new Vector2(n, -n));
                }
                distination.Add(new Vector2(1, 0));
                distination.Add(new Vector2(0, -1));
                distination.Add(new Vector2(-1, 0));
                distination.Add(new Vector2(0, 1));
                canPromotion = false;
                break;
            case 23:
                startPosition = new Vector2(0, 0);
                for (int n = -8; n < 9; n++)
                {
                    distination.Add(new Vector2(n, 0));
                    distination.Add(new Vector2(0, n));
                }
                distination.Add(new Vector2(1, 1));
                distination.Add(new Vector2(1, -1));
                distination.Add(new Vector2(-1, -1));
                distination.Add(new Vector2(-1, 1));
                canPromotion = false;
                break;
        }
        distination.Remove(new Vector2(0, 0));
        SpriteRenderer psp = GetComponent<SpriteRenderer>();
        if (player == 1)
        {
            for(int i = 0;i < distination.Count; i++)
            {
                distination[i] *= -1;
            }
            psp.flipX = true;
            psp.flipY = true;
            startPosition -= new Vector2(4, 4);
            startPosition *= -1;
            startPosition += new Vector2(4, 4);
        }
        else if(player == -1)
        {
            psp.flipX = false;
            psp.flipY = false;
        }
    }


}
