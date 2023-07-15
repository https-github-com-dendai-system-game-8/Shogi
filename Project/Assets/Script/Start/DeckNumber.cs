using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;

public class DeckNumber : MonoBehaviour
{
    public static int[] selectedNumber = { 0, 1 };
    public static int index;
    public static int n;

    [SerializeField]private Text[] text = new Text[2];
    [SerializeField] private int num;
    private Text deckName;
    
    // Start is called before the first frame update
   

    private void OnEnable()
    {
        deckName = transform.Find("Text").gameObject.GetComponent<Text>();
        deckName.text = File.ReadAllLines(Application.dataPath + "/Resources/pieceStatus" + num + ".txt")[^1];
        for(int i = 0;i < selectedNumber.Length;i++)
        {
            if(selectedNumber[i] != 0)
                selectedNumber[i] = 0;
            if (text[i].text != "–¢‘I‘ð")
                text[i].text = "–¢‘I‘ð";
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
