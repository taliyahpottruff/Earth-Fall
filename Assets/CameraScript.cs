using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour
{
    public bool active = true;
    public int speed;

    private void Update()
    {
        if (active)
            transform.position += Vector3.down * speed * Time.deltaTime;
    }
}