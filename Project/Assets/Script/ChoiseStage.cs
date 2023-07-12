using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChoiseStage : MonoBehaviour
{
    public string sceneName;//移動したいシーンの名前

    public void OnClick()
    {

        if (sceneName == "End")
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
        else
        {
            SceneManager.LoadScene(sceneName);
        }

        
    }

   
    
    
    
}
