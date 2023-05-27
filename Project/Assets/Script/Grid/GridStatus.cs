using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class GridStatus : MonoBehaviour,IPointerClickHandler
{
    public Vector2 myPosition;//このマスの位置
    public bool isSelect = false;//このマスが選ばれたかどうか
    public bool pieceIsOn = false;//このマスに駒が乗ってるかどうか
    private PiecesMove pm;
    void Start()
    {
        pm = GameObject.FindGameObjectWithTag("ShogiStage").GetComponent<PiecesMove>();
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (pm.isMoveStage && pm.isCanTouch)
        {
            isSelect = true;
            Debug.Log("移動先に選ばれた");
        }
    }
}
