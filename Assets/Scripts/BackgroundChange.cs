using UnityEngine;

public class BackgroundChange : MonoBehaviour {

    public GameObject lamp;
    public Material background;

    Vector3 around = new Vector3(0, 0, 1);

    // Use this for initialization
    void Start () {
		
	}
    float time = 0f;
    float changeColor = 0.0003f;
	// Update is called once per frame
	void Update () {
        time += changeColor;
        if (time > 1f) changeColor *= -1;
        if (time < 0f) changeColor *= -1;
        lamp.transform.Rotate(around, 0.1f, Space.World);
        background.color = Color.Lerp(Color.black, Color.white, time);

    }
}
