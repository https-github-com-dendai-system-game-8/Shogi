using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChoiseStage : MonoBehaviour
{

    public string sceneName;//ˆÚ“®‚µ‚½‚¢ƒV[ƒ“‚Ì–¼‘O


    public void OnClick()
    {
        if (sceneName == "End")
        {
            Application.Quit();
        }
        else
        {
            SceneManager.LoadScene(sceneName);
        }
        
        
    }

   
    
    
    
}
