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
    public GUIText gameOverScores;
    public GameObject resetScores;
    public bool limitLevels;
    public int level;
    public int maxLevel;
    public int score;
    public int highscore;
    public Animation bronzeAchievement;
    public bool hasBronze;
    public Animation silverAchievement;
    public bool hasSilver;
    public Animation goldAchievement;
    public bool hasGold;
    public GameObject bronze;
    public GameObject silver;
    public GameObject gold;

    void Start()
    {
        StartCoroutine(NextLevel());
        StartCoroutine(GenerateEnemies());

        highscore = PlayerPrefs.GetInt("highscore", 0);
        hasBronze = bool.Parse(PlayerPrefs.GetString("hasBronze", "false"));
        hasSilver = bool.Parse(PlayerPrefs.GetString("hasSilver", "false"));
        hasGold = bool.Parse(PlayerPrefs.GetString("hasGold", "false"));
    }

    void Update()
    {
        mousePosition = Input.mousePosition;
        mousePosition = mainCamera.ScreenToWorldPoint(mousePosition);

        Rect screen = new Rect(0, 0, Screen.width, Screen.height);

        if (cameraScript.active & screen.Contains(Input.mousePosition))
            transform.position = mousePosition;

        levelDisplay.text = "Level: " + level;
        scoreDisplay.text = "Score: " + score;

        bronze.active = hasBronze;
        silver.active = hasSilver;
        gold.active = hasGold;

        if (cameraScript.active)
        {
            Cursor.visible = false;
            gameOver.active = false;
            gameOverButton.active = false;
            gameOverScores.gameObject.active = false;
            resetScores.active = false;

            if (score >= 100 & !hasBronze)
            {
                hasBronze = true;
                bronzeAchievement.Play();
                PlayerPrefs.SetString("hasBronze", hasBronze.ToString());
                PlayerPrefs.Save();
            }

            if (level >= 10 & !hasSilver)
            {
                hasSilver = true;
                silverAchievement.Play();
                PlayerPrefs.SetString("hasSilver", hasSilver.ToString());
                PlayerPrefs.Save();
            }

            if (level >= 100 & !hasGold)
            {
                hasGold = true;
                goldAchievement.Play();
                PlayerPrefs.SetString("hasGold", hasGold.ToString());
                PlayerPrefs.Save();
            }
        }
        else
        {
            Cursor.visible = true;
            gameOver.active = true;
            gameOverButton.active = true;
            gameOverScores.gameObject.active = true;
            gameOverScores.text = "Highscore - " + highscore + " | Score - " + score;
            resetScores.active = true;

            if (Input.GetKeyDown(KeyCode.R))
            {
                highscore = 0;
                hasBronze = false;
                hasSilver = false;
                hasGold = false;
                PlayerPrefs.SetInt("highscore", highscore);
                PlayerPrefs.SetString("hasBronze", hasBronze.ToString());
                PlayerPrefs.SetString("hasSilver", hasSilver.ToString());
                PlayerPrefs.SetString("hasGold", hasGold.ToString());
                PlayerPrefs.Save();
            }
        }
    }

    public void EndGame()
    {
        if (score > highscore)
        {
            highscore = score;
            PlayerPrefs.SetInt("highscore", highscore);
            PlayerPrefs.Save();
        }
    }

    public void StartCoroutines()
    {
        StartCoroutine(NextLevel());
        StartCoroutine(GenerateEnemies());
    }

    IEnumerator GenerateEnemies()
    {
        while (cameraScript.active)
        {
            score += 10;

            int numberOfEnemies = Random.Range(0, level * 2);

            for (int i = 1; i <= numberOfEnemies; i++)
            {
                int y = Random.Range(0, 100);

                Vector2 screenLocation = new Vector2(Random.Range(0, Screen.width), -y);
                Vector2 worldLocation = mainCamera.ScreenToWorldPoint(screenLocation);
                Enemy enemy = enemyPrefab.GetComponent(typeof(Enemy)) as Enemy;
                enemy.enemyBehaviour = EnemyBehaviour.None;
                Instantiate(enemyPrefab, worldLocation, Quaternion.identity);

                float random = Random.Range(0f, 1f);

                if (level >= 10 & random < (float)((float) level / 100f))
                {
                    Enemy enemy1 = enemyPrefab.GetComponent(typeof(Enemy)) as Enemy;
                    enemy1.enemyBehaviour = EnemyBehaviour.Up;
                    Instantiate(enemyPrefab, worldLocation, Quaternion.identity);

                    Enemy enemy3 = enemyPrefab.GetComponent(typeof(Enemy)) as Enemy;
                    enemy3.enemyBehaviour = EnemyBehaviour.Left;
                    Instantiate(enemyPrefab, worldLocation, Quaternion.identity);

                    Enemy enemy4 = enemyPrefab.GetComponent(typeof(Enemy)) as Enemy;
                    enemy4.enemyBehaviour = EnemyBehaviour.Right;
                    Instantiate(enemyPrefab, worldLocation, Quaternion.identity);
                }
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