using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeSE : MonoBehaviour
{
    private AudioSource sound;
    [SerializeField] private AudioClip clip;
    // Start is called before the first frame update
    void Start()
    {
        sound = GameObject.FindGameObjectWithTag("SE").GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClick()
    {
        sound.clip = clip;
        sound.Play();
    }
}
