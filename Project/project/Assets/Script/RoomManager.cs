using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomManager : MonoBehaviour
{
    public static string roomName = "Room";//�����̖��O
    private InputField inputName;//���͂��ꂽ���O
    private void Start()
    {
        inputName = GetComponent<InputField>();
    }

    public void ChangeRoomName()
    {
        roomName = inputName.text;
    }
}
