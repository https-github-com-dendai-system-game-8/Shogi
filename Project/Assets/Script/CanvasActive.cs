using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

public class CanvasActive : MonoBehaviour//���j���[���J����������肷��
{

    private AudioSource se;//SE�Ǘ��p��AudioSource

    public bool isOpen = false;//���j���[���J���Ă��邩�ǂ���
    public GameObject[] secondStage, firstStage;//��:�ŏ��ɏ������,�E:����ւ��ŏ������
    public AudioClip[] seClip;//���j���[����A�J����
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
