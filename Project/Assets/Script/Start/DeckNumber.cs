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
    }

    public void OnClick()
    {
        if (index >= n)
            index = 0;
        text[index].text = Convert.ToString(num);
        selectedNumber[index++] = num;
        
    }
}
