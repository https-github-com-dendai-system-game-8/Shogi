using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class GridStatus : MonoBehaviour,IPointerClickHandler
{
    public Vector2 myPosition;//���̃}�X�̈ʒu
    public bool isSelect = false;//���̃}�X���I�΂ꂽ���ǂ���
    public bool pieceIsOn = false;//���̃}�X�ɋ����Ă邩�ǂ���
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
            Debug.Log("�ړ���ɑI�΂ꂽ");
        }
    }
}
