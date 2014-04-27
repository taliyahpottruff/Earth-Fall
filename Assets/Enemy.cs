using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    public Camera mainCamera;

    void Start()
    {
        mainCamera = GameObject.Find("Main Camera").GetComponent(typeof(Camera)) as Camera;
    }

	void Update()
    {
        Vector2 screenPoint = mainCamera.WorldToScreenPoint(transform.position);

        if (screenPoint.y > Screen.height)
            Destroy(gameObject);
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        CameraScript mainCamera = other.transform.parent.GetComponent(typeof(CameraScript)) as CameraScript;
        mainCamera.active = false;
        Player player = other.GetComponentInChildren(typeof(Player)) as Player;
        player.StopAllCoroutines();
    }
}