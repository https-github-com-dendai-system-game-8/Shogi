using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

public class CanvasActive : MonoBehaviour//メニューを開いたり閉じたりする
{

    private AudioSource se;//SE管理用のAudioSource

    public bool isOpen = false;//メニューを開いているかどうか
    public GameObject[] secondStage, firstStage;//左:最初に消すやつ,右:入れ替わりで消すやつ
    public AudioClip[] seClip;//メニュー閉じる、開く音
    // Start is called before the first frame update
    void Start()
    {
        foreach (var obj in secondStage)
        {
            obj.SetActive(false);
        }
        foreach (var obj in firstStage)
        {
            obj.SetActive(true);
        }
        se = GameObject.FindGameObjectWithTag("SE").GetComponent<AudioSource>();


    }

    // Update is called once per frame

    public void ReturnStage(bool set)
    {
        Debug.Log(Convert.ToInt32(set));
        if(se != null)
        {
            se.clip = seClip[Convert.ToInt32(set)];
            se.Play();
        }
        foreach (var obj in secondStage)
        {
            obj.SetActive(set);
        }
        foreach (var obj in firstStage)
        {
            obj.SetActive(!set);
        }
        isOpen = set;
        
    }

   
}
