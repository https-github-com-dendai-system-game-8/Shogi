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
    // Start is called before the first frame update
    
    void OnEnable()
    {
        myPosition = new Vector2(transform.position.x - 4 * PiecesMove.gridSize, transform.position.y - 4 * PiecesMove.gridSize);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (PiecesMove.isMoveStage)
        {
            isSelect = true;
            Debug.Log("移動先に選ばれた");
        }
    }
}
