using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;

public class DeckNumber : MonoBehaviour
{
    public static int[] selectedNumber = { 0, 1 };//選ばれているデッキ
    public static int index;//どちらのデッキを変更しようとしているか
    public static int n;//プレイヤーの総数
    [SerializeField]private Text[] text = new Text[2];//デッキ名を表示するテキスト
    [SerializeField] private int num;//デッキの番号
    private Text deckName;//デッキ名
    
    // Start is called before the first frame update
   

    private void OnEnable()
    {
        deckName = transform.Find("Text").gameObject.GetComponent<Text>();
        deckName.text = File.ReadAllLines(Application.dataPath + "/Resources/pieceStatus" + num + ".txt")[^1];
        for(int i = 0;i < selectedNumber.Length;i++)
        {
            if(selectedNumber[i] != 0)
                selectedNumber[i] = 0;
            if (text[i].text != "未選択")
                text[i].text = "未選択";
        }
        if(index != 0)
            index = 0;
    }

    public void OnClick()
    {
        Debug.Log(n);
        if (index >= n)
            index = 0;
        text[index].text = deckName.text;
        selectedNumber[index++] = num;
        
    }
}
