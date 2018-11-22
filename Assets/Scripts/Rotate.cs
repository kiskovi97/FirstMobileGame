using UnityEngine;

public class Rotate : MonoBehaviour {

    public float speed = 0.3f;
    // Update is called once per frame
    float time = 0;
	void Update () {
        time += 0.01f * speed;
        transform.Rotate(Vector3.up, Mathf.Cos(time) * speed, Space.World);
	}
}
