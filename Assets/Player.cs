using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
    private Vector2 mousePosition;
    public Camera mainCamera;
    public GUIText levelDisplay;
    public GUIText scoreDisplay;
    public GameObject enemyPrefab;
    public CameraScript cameraScript;
    public GameObject gameOver;
    public GameObject gameOverButton;
    public bool limitLevels;
    public int level;
    public int maxLevel;
    public int score;

    void Start()
    {
        StartCoroutine(NextLevel());
        StartCoroutine(GenerateEnemies());
    }

    void Update()
    {
        mousePosition = Input.mousePosition;
        mousePosition = mainCamera.ScreenToWorldPoint(mousePosition);

        if (cameraScript.active)
        transform.position = mousePosition;

        levelDisplay.text = "Level: " + level;
        scoreDisplay.text = "Score: " + score;

        if (cameraScript.active)
        {
            gameOver.active = false;
            gameOverButton.active = false;
        }
        else
        {
            gameOver.active = true;
            gameOverButton.active = true;
        }
    }

    IEnumerator GenerateEnemies()
    {
        while (cameraScript.active)
        {
            score += 10;

            int numberOfEnemies = Random.Range(0, level * 2);

            for (int i = 1; i <= numberOfEnemies; i++)
            {
                Vector2 screenLocation = new Vector2(Random.Range(0, Screen.width), -100);
                Vector2 worldLocation = mainCamera.ScreenToWorldPoint(screenLocation);
                Instantiate(enemyPrefab, worldLocation, Quaternion.identity);
            }

            yield return new WaitForSeconds(1);
        }
    }

    IEnumerator NextLevel()
    {
        while (cameraScript.active)
        {
            if (limitLevels)
            {
                if (level < maxLevel)
                    level++;
            }
            else
                level++;

            yield return new WaitForSeconds(10);
        }
    }
}