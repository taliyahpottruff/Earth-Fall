using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    public EnemyBehaviour enemyBehaviour;
    public int speed = 5;
    public Camera mainCamera;

    void Start()
    {
        mainCamera = GameObject.Find("Main Camera").GetComponent(typeof(Camera)) as Camera;
    }

	void Update()
    {
        Vector2 screenPoint = mainCamera.WorldToScreenPoint(transform.position);

        if (screenPoint.y > Screen.height || screenPoint.x < 0 || screenPoint.x > Screen.width)
            Destroy(gameObject);

        if (enemyBehaviour == EnemyBehaviour.Up)
            transform.position += Vector3.up * speed * Time.deltaTime;
        else if (enemyBehaviour == EnemyBehaviour.Down)
            transform.position += Vector3.down * speed * Time.deltaTime;
        else if (enemyBehaviour == EnemyBehaviour.Left)
            transform.position += Vector3.left * speed * Time.deltaTime;
        else if (enemyBehaviour == EnemyBehaviour.Right)
            transform.position += Vector3.right * speed * Time.deltaTime;
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        CameraScript mainCamera = other.transform.parent.GetComponent(typeof(CameraScript)) as CameraScript;
        mainCamera.active = false;
        Player player = other.GetComponentInChildren(typeof(Player)) as Player;
        player.StopAllCoroutines();
        player.EndGame();
    }
}

public enum EnemyBehaviour
{
    None, Up, Down, Left, Right
}