using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PiecesStatus : MonoBehaviour,IPointerClickHandler
{

    public Vector2 startPosition;
    public int[] distination;//��̈ړ��\��
    public string type;//��̎��

    public bool isSelect = false;//���̋�I�΂�Ă��邩�ǂ���
    
    // Start is called before the first frame update


    public void OnPointerClick(PointerEventData eventData)
    {
        isSelect = true;
        Debug.Log("�I�΂ꂽ");
    }

    public bool CheckSelected()
    {
        return isSelect;
    }
}
