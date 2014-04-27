using UnityEngine;
using System.Collections;

public class TryAgainButton : MonoBehaviour
{
    public CameraScript cameraScript;
    public Player player;
    public Animation instructions;

    void OnMouseDown()
    {
        cameraScript.gameObject.transform.position = new Vector3(0, 65, -10);
        player.score = 0;
        player.level = 0;

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        for (int i = 0; i < enemies.Length; i++)
        {
            Destroy(enemies[i]);
        }

        cameraScript.active = true;
        instructions.Play();
        player.StartCoroutines();
    }
}