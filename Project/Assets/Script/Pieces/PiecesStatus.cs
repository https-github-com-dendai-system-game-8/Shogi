using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PiecesStatus : MonoBehaviour,IPointerClickHandler
{

    public Vector2 startPosition;//��̏����ʒu
    public Vector2 distination;//��̈ړ��\��
    public int type;//��̎��

    public bool isSelect = false;//���̋�I�΂�Ă��邩�ǂ���
    private Vector2 myPosition;
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

    public void CheckMove()
    {
        switch (type)
        {
            case 1:
                startPosition = new Vector2(); break;
        }
    }
}
