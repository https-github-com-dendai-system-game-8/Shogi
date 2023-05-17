using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class GridStatus : MonoBehaviour,IPointerClickHandler
{
    public Vector2 myPosition;//このマスの位置
    public bool isSelect = false;
    // Start is called before the first frame update

    public void OnPointerClick(PointerEventData eventData)
    {
        if (PiecesMove.isMoveStage)
        {
            isSelect = true;
            Debug.Log("移動先に選ばれた");
        }
        
    }
}
