using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class GridStatus : MonoBehaviour,IPointerClickHandler
{
    public Vector2 myPosition;//���̃}�X�̈ʒu
    public bool isSelect = false;
    // Start is called before the first frame update

    public void OnPointerClick(PointerEventData eventData)
    {
        if (PiecesMove.isMoveStage)
        {
            isSelect = true;
            Debug.Log("�ړ���ɑI�΂ꂽ");
        }
        
    }
}
