using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PiecesStatus : MonoBehaviour,IPointerClickHandler
{

    public Vector2 startPosition;//駒の初期位置
    public Vector2 distination;//駒の移動可能先
    public int type;//駒の種類

    public bool isSelect = false;//この駒が選ばれているかどうか
    private Vector2 myPosition;
    // Start is called before the first frame update


    public void OnPointerClick(PointerEventData eventData)
    {
        isSelect = true;
        Debug.Log("選ばれた");
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
                startPosition = new Vector2(); break;
        }
    }
}
