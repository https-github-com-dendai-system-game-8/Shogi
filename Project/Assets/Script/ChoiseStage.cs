using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChoiseStage : MonoBehaviour
{

    public string sceneName;//�ړ��������V�[���̖��O


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
