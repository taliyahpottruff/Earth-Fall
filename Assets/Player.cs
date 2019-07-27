using TMPro;
using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
    private Vector2 mousePosition;
    public Camera mainCamera;
    public GUIText levelDisplay;
    public GUIText scoreDisplay;
	[SerializeField] private TextMeshProUGUI pausedText;
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
    private float startingY;
	[SerializeField] private int diffModifier = 10000;
	[SerializeField] private int[] medalGates = new int[3];
	[SerializeField] [Range(0, 10000)] private int[] difficultyGates;
	private bool paused;

    void Start() {
        startingY = Camera.main.transform.position.y;

        StartCoroutine(NextLevel());
        StartCoroutine(GenerateEnemies());

        highscore = PlayerPrefs.GetInt("highscore", 0);
        hasBronze = bool.Parse(PlayerPrefs.GetString("hasBronze", "false"));
        hasSilver = bool.Parse(PlayerPrefs.GetString("hasSilver", "false"));
        hasGold = bool.Parse(PlayerPrefs.GetString("hasGold", "false"));
    }

    void Update() {
        mousePosition = Input.mousePosition;
        mousePosition = mainCamera.ScreenToWorldPoint(mousePosition);

		Time.timeScale = (paused) ? 0f : 1f;
		pausedText.enabled = paused;

        Rect screen = new Rect(0, 0, Screen.width, Screen.height);

		if (cameraScript.active & screen.Contains(Input.mousePosition) && !paused) {
			transform.position = mousePosition;
		}

		if (score > highscore) {
			highscore = score;
		}

        levelDisplay.text = "Highscore: " + highscore;
        scoreDisplay.text = "Score: " + score;

        bronze.active = hasBronze;
        silver.active = hasSilver;
        gold.active = hasGold;

        if (cameraScript.active) { //Game logic goes here
            Cursor.visible = false;
            gameOver.active = false;
            gameOverButton.active = false;
            gameOverScores.gameObject.active = false;
            resetScores.active = false;

            score = (int)(startingY - Camera.main.transform.position.y) * 2;

            if (score >= medalGates[0] & !hasBronze) {
                hasBronze = true;
                bronzeAchievement.Play();
                PlayerPrefs.SetString("hasBronze", hasBronze.ToString());
                PlayerPrefs.Save();
            } else if (score >= medalGates[1] & !hasSilver) {
                hasSilver = true;
                silverAchievement.Play();
                PlayerPrefs.SetString("hasSilver", hasSilver.ToString());
                PlayerPrefs.Save();
            } else if (score >= medalGates[2] & !hasGold) {
                hasGold = true;
                goldAchievement.Play();
                PlayerPrefs.SetString("hasGold", hasGold.ToString());
                PlayerPrefs.Save();
            }
        } else {
            Cursor.visible = true;
            gameOver.active = true;
            gameOverButton.active = true;
            gameOverScores.gameObject.active = true;
            gameOverScores.text = "Highscore - " + highscore + " | Score - " + score;
            resetScores.active = true;

            if (Input.GetKeyDown(KeyCode.R)) {
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
		PlayerPrefs.SetInt("highscore", highscore);
        PlayerPrefs.Save();
    }

    public void StartCoroutines() {
        StartCoroutine(NextLevel());
        StartCoroutine(GenerateEnemies());
    }

    IEnumerator GenerateEnemies() {
        while (cameraScript.active) {
			float difficulty = (score * score) / diffModifier;
			int numberOfEnemies = Mathf.RoundToInt(Random.Range(difficulty/2, difficulty));

            for (int i = 1; i <= numberOfEnemies; i++) {
                Vector2 screenLocation = new Vector2(Random.Range(0, Screen.width), 0);
                Vector2 worldLocation = mainCamera.ScreenToWorldPoint(screenLocation);
				worldLocation.y += Random.Range(-5f, 0f);

                float random = Random.Range(0f, 1f);

                if (score >= difficultyGates[0] & random < (float)((float) level / 100f)) { //Moving Enemy
                    Enemy enemy1 = enemyPrefab.GetComponent(typeof(Enemy)) as Enemy;
                    enemy1.enemyBehaviour = EnemyBehaviour.Up;
                    Instantiate(enemyPrefab, worldLocation, Quaternion.identity);

                    Enemy enemy3 = enemyPrefab.GetComponent(typeof(Enemy)) as Enemy;
                    enemy3.enemyBehaviour = EnemyBehaviour.Left;
                    Instantiate(enemyPrefab, worldLocation, Quaternion.identity);

                    Enemy enemy4 = enemyPrefab.GetComponent(typeof(Enemy)) as Enemy;
                    enemy4.enemyBehaviour = EnemyBehaviour.Right;
                    Instantiate(enemyPrefab, worldLocation, Quaternion.identity);
                } else { //Regular Enemy
					Enemy enemy = enemyPrefab.GetComponent(typeof(Enemy)) as Enemy;
					enemy.enemyBehaviour = EnemyBehaviour.None;
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