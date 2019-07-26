using UnityEngine;
using System.Collections;

public class StartScreen : MonoBehaviour
{
    void OnGUI()
    {
        if (Event.current.type == EventType.KeyDown || Event.current.type == EventType.MouseDown)
        {
            Application.LoadLevel("Game");
        }
    }
}