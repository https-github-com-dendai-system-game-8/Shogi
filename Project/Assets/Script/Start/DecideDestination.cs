using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DecideDestination : MonoBehaviour//どのステージを使用するかを決めるボタン
{
    [SerializeField] private string destination;//使うステージの名前
    [SerializeField] private ChoiseStage choiceStage;//シーン移動のためのクラス
    [SerializeField] private int number;//ステージの番号
    [SerializeField] private SelectStage selectStage;//ステージを選ぶクラス
    // Start is called before the first frame update


    public void OnClick()//ゲームちゅうボタンがクリックされたら
    {
        choiceStage.sceneName = "Stage"+ selectStage.stageNum + destination;
        DeckNumber.n = number;
    }
}
