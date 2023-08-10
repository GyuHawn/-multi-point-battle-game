using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MainMenu : MonoBehaviour
{
    public Launcher launcher;
    public TMP_InputField createInput;
    public void JoinMatch()
    {
        launcher.JoinRandom();
    }

    public void CreateMatch()
    {
        string roomName = createInput.text;
        launcher.CreateRoom(roomName);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
