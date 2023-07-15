using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectStage : MonoBehaviour
{
    public int stageNum;
    private Image stageImage;
    [SerializeField]private Sprite[] stageSprite;
    private RectTransform re;
    // Start is called before the first frame update
    void Start()
    {
        stageImage = GetComponent<Image>();
        re = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        stageImage.sprite = stageSprite[stageNum - 1];
        re.sizeDelta = stageImage.sprite.textureRect.size;
    }

    public void OnClick(int num)
    {
        stageNum = num;
    }
}
