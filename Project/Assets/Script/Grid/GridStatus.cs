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
            Debug.Log("�ړ���ɑI�΂ꂽ");
        }
    }
}
