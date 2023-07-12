using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecideDestination : MonoBehaviour
{
    [SerializeField] private string destination;
    [SerializeField]private ChoiseStage cs;
    // Start is called before the first frame update
    
    public void OnClick()
    {
        cs.sceneName = destination;
    }
}
