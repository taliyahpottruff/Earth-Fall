using UnityEngine;
using System.Collections;

public class SplashScreen : MonoBehaviour
{
	void Start()
    {
        StartCoroutine(SwitchScenes());
	}

    IEnumerator SwitchScenes()
    {
        while (true)
        {
            yield return new WaitForSeconds(3);
            Application.LoadLevel("StartScreen");
        }
    }
}