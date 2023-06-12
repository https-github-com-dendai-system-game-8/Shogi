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

public class PieceStatusNet : MonoBehaviour, IPointerClickHandler
{

    public Vector3 startPosition;//駒の初期位置
    public List<Vector3> distination;//駒の移動可能先
    public int type;//駒の種類
    public int player;//どちらが動かせる駒か
    public int holder;//元々の持ち主
    public bool canPromotion = true;//成れるかどうか
    public int promotionType;//成った後のタイプ
    public Sprite promotionSprite;//成った後の見た目
    public int pieceID;

    public bool isSelect = false;//この駒が選ばれているかどうか
    public Vector3 piecePosition;//現在の駒の位置

    private PiecesMoveNet pm;

    // Start is called before the first frame update

    private void Start()
    {
        CheckMove();
        piecePosition = startPosition;
        transform.localPosition = (piecePosition - new Vector3(4, 4)) * PiecesMoveNet.gridSize;
        pm = GameObject.FindGameObjectWithTag("ShogiStage").GetComponent<PiecesMoveNet>();
        pieceID = type * holder;
    }

    void Update()
    {
        piecePosition = new Vector3(Mathf.Round(transform.localPosition.x), Mathf.Round(transform.localPosition.y)) / PiecesMoveNet.gridSize + new Vector3(4, 4);
        for (int i = 0; i < pm.gridStatus.Length; i++)
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z);
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (!pm.isMoveStage && pm.isCanTouch && player == pm.turn)
        {
            isSelect = true;
            Debug.Log("選ばれた");
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
                startPosition = new Vector3(8, 2);
                distination.Clear();
                canPromotion = true;
                promotionType = 21;
                distination.Add(new Vector3(0, 1));
                break;
            case 2:
                startPosition = new Vector3(7, 2);
                distination.Clear();
                canPromotion = true;
                promotionType = 21;
                distination.Add(new Vector3(0, 1));
                break;
            case 3:
                startPosition = new Vector3(6, 2);
                distination.Clear();
                canPromotion = true;
                promotionType = 21;
                distination.Add(new Vector3(0, 1));

                break;
            case 4:
                startPosition = new Vector3(5, 2);
                distination.Clear();
                canPromotion = true;
                promotionType = 21;
                distination.Add(new Vector3(0, 1));
                break;
            case 5:
                startPosition = new Vector3(4, 2);
                distination.Clear();
                canPromotion = true;
                promotionType = 21;
                distination.Add(new Vector3(0, 1));
                break;
            case 6:
                startPosition = new Vector3(3, 2);
                distination.Clear();
                canPromotion = true;
                promotionType = 21;
                distination.Add(new Vector3(0, 1));
                break;
            case 7:
                startPosition = new Vector3(2, 2);
                distination.Clear();
                canPromotion = true;
                promotionType = 21;
                distination.Add(new Vector3(0, 1));
                break;
            case 8:
                startPosition = new Vector3(1, 2);
                distination.Clear();
                canPromotion = true;
                promotionType = 21;
                distination.Add(new Vector3(0, 1));
                break;
            case 9:
                startPosition = new Vector3(0, 2);
                distination.Clear();
                canPromotion = true;
                promotionType = 21;
                distination.Add(new Vector3(0, 1));
                break;
            case 10:
                startPosition = new Vector3(1, 1);
                distination.Clear();
                canPromotion = true;
                promotionType = 22;
                for (int n = -8; n < 9; n++)
                {
                    distination.Add(new Vector3(n, n));
                    distination.Add(new Vector3(n, -n));
                }
                break;
            case 11:
                startPosition = new Vector3(7, 1);
                distination.Clear();
                canPromotion = true;
                promotionType = 23;
                for (int n = -8; n < 9; n++)
                {
                    distination.Add(new Vector3(n, 0));
                    distination.Add(new Vector3(0, n));
                }
                break;
            case 12:
                startPosition = new Vector3(8, 0);
                distination.Clear();
                canPromotion = true;
                promotionType = 21;
                for (int n = 0; n < 9; n++)
                    distination.Add(new Vector3(0, n));
                break;
            case 13:
                startPosition = new Vector3(7, 0);
                distination.Clear();
                canPromotion = true;
                promotionType = 21;
                distination.Add(new Vector3(1, 2));
                distination.Add(new Vector3(-1, 2));
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
                break;
            case 16:
                startPosition = new Vector3(4, 0);
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
                break;
            case 19:
                startPosition = new Vector3(1, 0);
                distination.Clear();
                canPromotion = true;
                promotionType = 21;
                distination.Add(new Vector3(1, 2));
                distination.Add(new Vector3(-1, 2));
                break;
            case 20:
                startPosition = new Vector3(0, 0);
                distination.Clear();
                canPromotion = true;
                promotionType = 21;
                for (int n = -8; n < 9; n++)
                {
                    distination.Add(new Vector3(0, Mathf.Abs(n)));
                }
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
        }
        distination.Remove(new Vector3(0, 0));//
        SpriteRenderer psp = GetComponent<SpriteRenderer>();//駒の見た目向きを変える用
        if (player == 1)//プレイヤーの向きに駒の向きを合わせる
        {
            for (int i = 0; i < distination.Count; i++)
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
        else if (player == -1)
        {
            psp.flipX = false;
            psp.flipY = false;
            gameObject.layer = 0;
        }
    }


}
