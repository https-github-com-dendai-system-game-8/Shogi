using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DecideDestination : MonoBehaviour
{
    [SerializeField] private string destination;
    [SerializeField] private ChoiseStage cs;
    [SerializeField] private int n;
    // Start is called before the first frame update
    
    public void OnClick()
    {
        cs.sceneName = destination;
        DeckNumber.n = n;
    }
}
