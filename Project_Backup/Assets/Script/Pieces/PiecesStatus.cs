using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.EventSystems;

public class PiecesStatus : MonoBehaviour,IPointerClickHandler
{

    public Vector2 startPosition;//駒の初期位置
    public List<Vector2> distination;//駒の移動可能先
    public int type;//駒の種類

    public bool isSelect = false;//この駒が選ばれているかどうか
    private Vector2 piecePosition;
    // Start is called before the first frame update

    private void Start()
    {
        CheckMove();
        piecePosition = startPosition;
        transform.position = (piecePosition - new Vector2(4, 4)) * PiecesMove.gridSize;
    }

    void Update()
    {
        piecePosition = transform.position;
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (!PiecesMove.isMoveStage)
        {
            isSelect = true;
            Debug.Log("選ばれた");
        }
        
        
        
    }

    public bool CheckSelected()
    {
        return isSelect;
    }

    private void CheckMove()
    {
        switch (type)
        {
            case 1:
                startPosition = new Vector2(8, 3);
                distination.Add(new Vector2(0, 1)); 
                break;
            case 2:
                startPosition = new Vector2(7, 3);
                distination.Add(new Vector2(0, 1));
                break;
            case 3:
                startPosition = new Vector2(6, 3);
                distination.Add(new Vector2(0, 1));
                break;
            case 4:
                startPosition = new Vector2(5, 3);
                distination.Add(new Vector2(0, 1));
                break;
            case 5:
                startPosition = new Vector2(4, 3);
                distination.Add(new Vector2(0, 1));
                break;
            case 6:
                startPosition = new Vector2(3, 3);
                distination.Add(new Vector2(0, 1));
                break;
            case 7:
                startPosition = new Vector2(2, 3);
                distination.Add(new Vector2(0, 1));
                break;
            case 8:
                startPosition = new Vector2(1, 3);
                distination.Add(new Vector2(0, 1));
                break;
            case 9:
                startPosition = new Vector2(3, 0);
                distination.Add(new Vector2(0, 1));
                break;
            case 10:
                startPosition = new Vector2(6, 2);
                for(int n = -8;n < 9; n++)
                {
                    distination.Add(new Vector2(n, n));
                    distination.Add(new Vector2(n, -n));
                }
                
                break;
            case 11:
                startPosition = new Vector2(2, 2);
                for(int n = -8;n < 8; n++)
                {
                    distination.Add(new Vector2(n, 0));
                    distination.Add(new Vector2(0, n));
                }
                
                break;
            case 12:
                startPosition = new Vector2(8, 1);
                for (int n = -8; n < 8; n++) 
                    distination.Add(new Vector2(0, Mathf.Abs(n)));
                break;
            case 13:
                startPosition = new Vector2(7, 1);
                distination.Add(new Vector2(1, 2));
                distination.Add(new Vector2(-1, 2));
                break;
            case 14:
                startPosition = new Vector2(6, 1);
                distination.Add(new Vector2(-1, 1));
                distination.Add(new Vector2(0, 1));
                distination.Add(new Vector2(1, 1));
                distination.Add(new Vector2(-1, -1));
                distination.Add(new Vector2(1, -1));
                break;
            case 15:
                startPosition = new Vector2(5, 1);
                distination.Add(new Vector2(-1, 1));
                distination.Add(new Vector2(0, 1));
                distination.Add(new Vector2(-1, 0));
                distination.Add(new Vector2(1, 1));
                distination.Add(new Vector2(-0, -1));
                distination.Add(new Vector2(1, 0));
                break;
            case 16:
                startPosition = new Vector2(4, 1);
                distination.Add(new Vector2(-1, 1));
                distination.Add(new Vector2(0, 1));
                distination.Add(new Vector2(-1, 0));
                distination.Add(new Vector2(1, 1));
                distination.Add(new Vector2(-0, -1));
                distination.Add(new Vector2(1, 0));
                distination.Add(new Vector2(-1, -1));
                distination.Add(new Vector2(1, -1));
                break;
            case 17:
                startPosition = new Vector2(3, 1);
                distination.Add(new Vector2(-1, 1));
                distination.Add(new Vector2(0, 1));
                distination.Add(new Vector2(-1, 0));
                distination.Add(new Vector2(1, 1));
                distination.Add(new Vector2(-0, -1));
                distination.Add(new Vector2(1, 0));
                break;
            case 18:
                startPosition = new Vector2(2, 1);
                distination.Add(new Vector2(-1, 1));
                distination.Add(new Vector2(0, 1));
                distination.Add(new Vector2(1, 1));
                distination.Add(new Vector2(-1, -1));
                distination.Add(new Vector2(1, -1));
                break;
            case 19:
                startPosition = new Vector2(1, 1);
                distination.Add(new Vector2(1, 2));
                distination.Add(new Vector2(-1, 2));
                break;
            case 20:
                startPosition = new Vector2(0, 1);
                for(int n = -8;n < 9; n++)
                {
                    distination.Add(new Vector2(0, Mathf.Abs(n)));
                }
                break;
        }
    }
}
