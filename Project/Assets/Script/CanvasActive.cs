using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CanvasActive : MonoBehaviour//メニューを開いたり閉じたりする
{

    //private AudioSource se;//SE管理用のAudioSource

    public bool isOpen = false;//メニューを開いているかどうか
    public GameObject[] secondStage, firstStage;//左:最初に消すやつ,右:後から消すやつ
    //public AudioClip[] seClip;//メニュー閉じる、開く音
    // Start is called before the first frame update
    void Start()
    {
        foreach (var obj in secondStage)
        {
            obj.SetActive(false);
        }
        foreach (var obj in firstStage)
        {
            obj.SetActive(true);
        }
        //se = GameObject.FindGameObjectWithTag("SE").GetComponent<AudioSource>();


    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ReturnStage(false);
        }
        if (Input.GetKeyDown(KeyCode.Tab) && FindObjectOfType<Selectable>() != null)
        {
            FindObjectOfType<Selectable>().Select();
        }

        if (Input.GetKeyDown("q") && Input.GetKey(KeyCode.LeftControl))
        {
            SceneManager.LoadScene("TitleScene");
        }
    }
    public void ReturnStage(bool set)
    {
        //se.clip = seClip[0];
        //se.Play();
        foreach (var obj in secondStage)
        {
            obj.SetActive(set);
        }
        foreach (var obj in firstStage)
        {
            obj.SetActive(!set);
        }
        if (FindObjectOfType<Selectable>() != null)
        {
            FindObjectOfType<Selectable>().Select();
        }

        Time.timeScale = 1f;
        isOpen = set;
        
    }

    public void ChangeStage()
    {
        foreach (var obj in secondStage)
        {
            obj.SetActive(true);
        }
        foreach (var obj in firstStage)
        {
            obj.SetActive(false);
        }
        Time.timeScale = 0f;
        if (FindObjectOfType<Selectable>() != null)
        {
            FindObjectOfType<Selectable>().Select();
        }
        isOpen = true;
    }
    
}
