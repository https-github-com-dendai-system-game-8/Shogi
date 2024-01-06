using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;

public class DeckNumber : MonoBehaviour//�I�΂�Ă���f�b�L
{
    public static int[] selectedNumber = { 0, 1 };//�I�΂�Ă���f�b�L
    public static int index;//�ǂ���̃f�b�L��ύX���悤�Ƃ��Ă��邩
    public static int n;//�v���C���[�̑���
    [SerializeField]private Text[] text = new Text[2];//�f�b�L����\������e�L�X�g
    [SerializeField] private int num;//�f�b�L�̔ԍ�
    private Text deckName;//�f�b�L��
    
    // Start is called before the first frame update
   

    private void OnEnable()
    {
        deckName = transform.Find("Text").gameObject.GetComponent<Text>();//
        deckName.text = File.ReadAllLines(Application.dataPath + "/Resources/pieceStatus" + num + ".txt")[^1];//�e�L�X�g�t�@�C������f�b�L����ǂݎ��
        for(int i = 0;i < selectedNumber.Length;i++)//�I�΂�Ă���f�b�L��\��
        {
            if(selectedNumber[i] != 0)
                selectedNumber[i] = 0;
            if (text[i].text != "���I��")
                text[i].text = "���I��";
        }
        if(index != 0)
            index = 0;
    }

    public void OnClick()
    {
        if (index >= n)
            index = 0;
        text[index].text = deckName.text;
        selectedNumber[index++] = num;
        
    }
}
