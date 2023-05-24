using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Select : MonoBehaviour
{
    private GameObject[] choice;//YES��NO�̃{�^�����擾
    private GameObject[] log;//�e�L�X�g�I�u�W�F�N�g���擾

    [SerializeField]private Text logText;//���O���o���e�L�X�g�I�u�W�F�N�g

    [SerializeField,Multiline] private string before;//�I��O�ɕ\������e�L�X�g�̓��e
    [SerializeField,Multiline] private string after; //�I����ɕ\������e�L�X�g�̓��e
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
