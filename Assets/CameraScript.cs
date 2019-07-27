using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour
{
    public bool active = true;
    public int speed;
    private int originalSpeed;

    private void Start() {
        originalSpeed = speed;
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Space)) {
            speed = originalSpeed * 2;
        } else {
            speed = originalSpeed;
        }

        if (active)
            transform.position += Vector3.down * speed * Time.deltaTime;
    }
}