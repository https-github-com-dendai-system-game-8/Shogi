using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChoiseStage : MonoBehaviour
{

    public string sceneName;//à⁄ìÆÇµÇΩÇ¢ÉVÅ[ÉìÇÃñºëO
    public bool scale;


    public void OnClick()
    {
        
        if (!scale)
        {
            Time.timeScale = 1f;
        }
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
