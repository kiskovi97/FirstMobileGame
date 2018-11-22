using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    new Rigidbody rigidbody;
    public AudioSource source;
    public AudioSource reverse;
    public AudioSource block;
    public AudioSource special;
    private Vector3 force;
    public float speed = 3f;
    private Material material;
    public Text pointText;
    public Slider specialSlider;
    public Image specialColor;
    private float point = 0;
    private float specialTime = 0;
    private float pointScale = 1f;
    
    private void OnCollisionEnter(Collision collision)
    {
        block.volume = collision.relativeVelocity.magnitude / 10f;
        block.Play();
    }

    // Use this for initialization
    void Start()
    {
        GameState.LoadData();
        Physics.gravity = new Vector3(0, 0, 0);
        force = new Vector3(0, -speed, 0);
        if (rigidbody == null)
            rigidbody = GetComponent<Rigidbody>();
        material = GetComponent<MeshRenderer>().materials[0];
        point = 0;
        source = GetComponent<AudioSource>();
        if (source != null)
        source.pitch = 1.0f;
    }

    public void GoForce()
    {
        rigidbody.AddForce(force/10, ForceMode.Impulse);
    }

    public int GetScore()
    {
        return (int)point;
    }

    void Update()
    {
        if (GameState.Pause) return;
        point += Time.deltaTime * pointScale;
        pointText.text = GetScore() + "";

        if ((int)(specialTime * 10) == 0)
        {
            specialSlider.enabled = false;
            specialSlider.gameObject.SetActive(false);
        }
        else
        {
            specialSlider.enabled = true;
            specialSlider.gameObject.SetActive(true);
            specialSlider.value = specialTime / 10f;
        }

        GoForce();
        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();

    }

    public void GravityChange()
    {
        Physics.gravity *= -1;
        force *= -1;
        rigidbody.velocity /= 3f;
        reverse.Play();
        GoForce();
    }
    public void ColorChange()
    {
        Color color = Color.white;
        material.color = color;
        specialColor.color = color;
    }

    public void ColorChange(Color color)
    {
        special.Play();
        material.color = color;
        specialColor.color = color;
    }

    public void TimeScale(float scale, float dur)
    {
        if (Time.timeScale + scale < 0.8f)
        {
            scale = 0.8f - Time.timeScale;
        }
        if (source != null)
            source.pitch += scale / 5f;
        Time.timeScale += scale;
        if (scale > 0)
            ColorChange(Color.green);
        else
            ColorChange(Color.magenta);
        StartCoroutine(Back(scale, dur));
    }

    IEnumerator SpecialDuration(float dur)
    {
        for (float i = dur; i >= 0; i -= 0.1f)
        {
            specialTime = i;
            yield return new WaitForSeconds(0.1f);
        }
    }

    IEnumerator Back(float scale, float delayTime)
    {
        yield return SpecialDuration(delayTime);
        Time.timeScale -= scale;
        if (source != null)
            source.pitch -= scale / 5f;
        ColorChange();
    }

    public void Double(float dur)
    {
        pointScale = 2f;
        ColorChange(Color.yellow);
        StartCoroutine(Simple(dur));
    }

    IEnumerator Simple(float delayTime)
    {
        yield return SpecialDuration(delayTime);
        pointScale = 1f;
        ColorChange();
    }

    public void Center(float dur)
    {
        force = new Vector3(0, 0, 0);
        transform.position = new Vector3(0, 6, 0);
        rigidbody.velocity = new Vector3(0, 0, 0);
        ColorChange(Color.cyan);
        StartCoroutine(Fall(dur));
    }

    IEnumerator Fall(float delayTime)
    {
        yield return SpecialDuration(delayTime);
        force = new Vector3(0, -speed, 0);
        RaycastHit hit;
        if (Physics.Raycast(new Ray(transform.position, force), out hit, 10f))
        {
            if (hit.transform.gameObject.tag == "Finish")
            {
                force = new Vector3(0, 1, 0);
            }
        }
        else force = new Vector3(0, speed, 0);
        ColorChange();
    }
}
