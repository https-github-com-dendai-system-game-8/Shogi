using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DecideDestination : MonoBehaviour//�ǂ̃X�e�[�W���g�p���邩�����߂�{�^��
{
    [SerializeField] private string destination;//�g���X�e�[�W�̖��O
    [SerializeField] private ChoiseStage choiceStage;//�V�[���ړ��̂��߂̃N���X
    [SerializeField] private int number;//�X�e�[�W�̔ԍ�
    [SerializeField] private SelectStage selectStage;//�X�e�[�W��I�ԃN���X
    // Start is called before the first frame update


    public void OnClick()//�Q�[�����イ�{�^�����N���b�N���ꂽ��
    {
        choiceStage.sceneName = "Stage"+ selectStage.stageNum + destination;
        DeckNumber.n = number;
    }
}
