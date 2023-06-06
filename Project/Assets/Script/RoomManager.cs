using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomManager : MonoBehaviour
{
    public static string roomName = "Room";//•”‰®‚Ì–¼‘O
    private InputField inputName;//“ü—Í‚³‚ê‚½–¼‘O
    private void Start()
    {
        inputName = GetComponent<InputField>();
    }

    public void ChangeRoomName()
    {
        roomName = inputName.text;
    }
}
