using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Select : MonoBehaviour
{
    private GameObject[] choice;//YESとNOのボタンを取得
    private GameObject[] log;//テキストオブジェクトを取得

    [SerializeField]private Text logText;//ログを出すテキストオブジェクト

    [SerializeField,Multiline] private string before;//選択前に表示するテキストの内容
    [SerializeField,Multiline] private string after; //選択後に表示するテキストの内容
    private void Awake()
    {
        choice = GameObject.FindGameObjectsWithTag("TwoChoice");
        log = GameObject.FindGameObjectsWithTag("AfterChoice");
        foreach(var obj in choice)
        {
            obj.SetActive(false);
        }
    }
    private void OnEnable()
    {
        foreach (var obj in log)
        {
            obj.SetActive(false);
        }
    }
    public void OnClick()
    {

        foreach(var obj in choice)
        {
            obj.SetActive(true);
        }
        foreach(var obj in log)
        {
            obj.SetActive(true);
        }
        logText.text = before;
    }

    public void OnClickNO()
    {
        foreach(var obj in choice)
        {
            obj.SetActive(false);
        }
        foreach(var obj in log)
        {
            obj.SetActive(false);
        }
        if(FindObjectOfType<Selectable>() != null) FindObjectOfType<Selectable>().Select();

    }

    public void OnClickYes()
    {

        logText.text = after;
        
        foreach (var obj in choice)
        {
            obj.SetActive(false);
        }
        if (FindObjectOfType<Selectable>() != null)
        {
            FindObjectOfType<Selectable>().Select();
        }
    }

}
