using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class GridStatusNet : MonoBehaviour,IPointerClickHandler
{
    public Vector3 myPosition;//このマスの位置
    public bool isSelect = false;//このマスが選ばれたかどうか
    public bool pieceIsOn = false;//このマスに駒が乗ってるかどうか
    private PiecesMoveNet pm;
    void Start()
    {
        pm = GameObject.FindGameObjectWithTag("ShogiStage").GetComponent<PiecesMoveNet>();
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
