using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class GridStatus : MonoBehaviour,IPointerClickHandler
{
    public Vector3 myPosition;//このマスの位置
    public bool isSelect = false;//このマスが選ばれたかどうか
    public bool pieceIsOn = false;//このマスに駒が乗ってるかどうか
    public bool safe;
    private SpriteRenderer gridRenderer;
    public Color myColor;
    void Start()
    {
        gridRenderer = transform.Find("ステージ選択個別背景").GetComponent<SpriteRenderer>();
        myColor = gridRenderer.color;
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (safe)
        {
            isSelect = true;
            Debug.Log("移動先に選ばれた");
        }
    }

    public void IsSafe(bool ism,bool isc)//触れる状態かどうかを見る
    {
        safe = ism && isc;
    }
}
