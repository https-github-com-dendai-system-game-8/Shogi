using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectStage : MonoBehaviour
{
    public int stageNum;
    private Image stageImage;
    [SerializeField]private Sprite[] stageSprite;
    // Start is called before the first frame update
    void Start()
    {
        stageImage = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        stageImage.sprite = stageSprite[stageNum - 1];
    }

    public void OnClick(int num)
    {
        stageNum = num;
    }
}
