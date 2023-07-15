using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class GridStatus : MonoBehaviour,IPointerClickHandler
{
    public Vector3 myPosition;//���̃}�X�̈ʒu
    public bool isSelect = false;//���̃}�X���I�΂ꂽ���ǂ���
    public bool pieceIsOn = false;//���̃}�X�ɋ����Ă邩�ǂ���
    public bool safe;
    private SpriteRenderer gridRenderer;
    public Color myColor;
    void Start()
    {
        gridRenderer = transform.Find("�X�e�[�W�I���ʔw�i").GetComponent<SpriteRenderer>();
        myColor = gridRenderer.color;
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (safe)
        {
            isSelect = true;
            Debug.Log("�ړ���ɑI�΂ꂽ");
        }
    }

    public void IsSafe(bool ism,bool isc)//�G����Ԃ��ǂ���������
    {
        safe = ism && isc;
    }
}
