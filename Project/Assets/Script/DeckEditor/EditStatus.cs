using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EditStatus : MonoBehaviour, IPointerClickHandler
{
    public int myID;//この駒の番号
    [SerializeField] private int type;//この駒の種類
    private int max;//この駒のポイントの最大値
    private EditManager manager;//マネージャー
    private Text text;
    private SpriteRenderer[] otherSr;
    public SpriteRenderer sr;

    // Start is called before the first frame update
    void Start()
    {
        manager = FindObjectOfType<EditManager>();
        Initialize();
        text = transform.Find("point").gameObject.GetComponent<Text>();
        
    }

    // Update is called once per frame
    void Update()
    {
        text.text = manager.status[myID] + "/" + max;
    }
    public void OnPointerClick (PointerEventData eventdata)
    {
        manager.GetPieceData(myID,max,type);
        foreach(var ob in otherSr)
        {
            ob.color = Color.white;
        }
        sr.color = Color.gray;
        Debug.Log("選ばれた");
    }

    private void Initialize()
    {
        switch (type)
        {
            case 0:
                max = 10;
                break;
            case 1:
                max = 10;
                break;
            case 2:
                max = 10;
                break;
            case 3:
                max = 10;
                break;
            case 4:
                max = 10;
                break;
            case 5:
                max = 10;
                break;
            case 6:
                max = 10;
                break;
            case 7:
                max = 0;
                break;
            default:
                max = 10;
                break;
        }
        GameObject[] go;
        sr = GetComponent<SpriteRenderer>();
        go = GameObject.FindGameObjectsWithTag("Pieces");
        otherSr = new SpriteRenderer[go.Length];
        for(int i = 0;i < go.Length; i++)
        {
            otherSr[i] = go[i].GetComponent<SpriteRenderer>();
        }
    }
}
