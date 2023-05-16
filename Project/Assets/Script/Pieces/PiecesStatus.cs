using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PiecesStatus : MonoBehaviour,IPointerClickHandler
{

    public Vector2 startPosition;
    public int[] distination;//駒の移動可能先
    public string type;//駒の種類

    public bool isSelect = false;//この駒が選ばれているかどうか
    
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
}
