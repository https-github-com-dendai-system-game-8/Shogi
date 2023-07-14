using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;

public class Indicate : MonoBehaviour
{
    [SerializeField] private char num; 
    // Start is called before the first frame update

    

    private void OnEnable()
    {
       
        ChangeDeckIndicate();
        
    }

    public void ChangeDeckIndicate()
    {
         Text text = GetComponent<Text>();
        try
        {
            EditManager edi = GameObject.FindGameObjectWithTag("DeckDataManager").GetComponent<EditManager>();
            text.text = edi.deckData[num].deckName;
        }
        catch
        {
            string[] str = new string[22];
            if (File.Exists(Application.dataPath + "/Resources/pieceStatus" + num + ".txt"))
                str = File.ReadAllLines(Application.dataPath + "/Resources/pieceStatus" + num + ".txt");
            
            text.text = str[^1];
        }
    }
    // Update is called once per frame

}
