using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EditStatus : MonoBehaviour, IPointerClickHandler
{
    public int myID;//���̋�̔ԍ�
    [SerializeField] private int type;//���̋�̎��
    private int max;//���̋�̃|�C���g�̍ő�l
    private EditManager manager;//�}�l�[�W���[
    private Text text;
    // Start is called before the first frame update
    void Start()
    {
        manager = FindObjectOfType<EditManager>();
        Initialize();
        text = transform.Find("point").gameObject.GetComponent<Text>();
        max = 0;
    }

    // Update is called once per frame
    void Update()
    {
        text.text = manager.status[myID] + "/" + max;
    }
    public void OnPointerClick (PointerEventData eventdata)
    {
        manager.GetPieceData(myID,max,type);
        Debug.Log("�I�΂ꂽ");
    }

    private void Initialize()
    {
        switch (type)
        {
            case 0:
                max = 98;
                break;
            case 1:
                max = 96;
                break;
            case 2:
                max = 92;
                break;
            case 3:
                max = 90;
                break;
            case 4:
                max = 88;
                break;
            case 5:
                max = 68;
                break;
            case 6:
                max = 68;
                break;
            case 7:
                max = 0;
                break;
            default:
                max = 0;
                break;
        }
    }
}
