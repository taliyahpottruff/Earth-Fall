using UnityEngine;
using System.Collections;

public class StartScreen : MonoBehaviour
{
    void OnGUI()
    {
        if (Event.current.type == EventType.keyDown || Event.current.type == EventType.mouseDown)
        {
            Application.LoadLevel("Game");
        }
    }
}