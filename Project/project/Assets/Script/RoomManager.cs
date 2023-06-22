using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomManager : MonoBehaviour
{
    public static string roomName = "Room";//部屋の名前
    private InputField inputName;//入力された名前
    private void Start()
    {
        inputName = GetComponent<InputField>();
    }

    public void ChangeRoomName()
    {
        roomName = inputName.text;
    }
}
