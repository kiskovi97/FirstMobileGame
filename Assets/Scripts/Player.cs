using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public AudioSource source;
    public AudioSource reverse;
    public AudioSource block;
    public AudioSource special;
    public float speed = 3f;
    public Text pointText;
    public Slider specialSlider;
    public Image specialColor;


    private Rigidbody _rigidbody;
    private Vector3 _force;
    private Material _material;
    private float _point = 0;
    private float _specialTime = 0;
    private float _pointScale = 1f;
    
    private void OnCollisionEnter(Collision collision)
    {
        block.volume = collision.relativeVelocity.magnitude / 10f;
        block.Play();
    }

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _material = GetComponent<MeshRenderer>().materials[0];

    }

    // Use this for initialization
    void Start()
    {
        GameState.LoadData();
        Physics.gravity = new Vector3(0, 0, 0);
        _force = new Vector3(0, -speed, 0);
        _point = 0;
        source = GetComponent<AudioSource>();
        if (source != null)
        source.pitch = 1.0f;
    }

    private void FixedUpdate()
    {
        _rigidbody.linearVelocity = _force;
    }

    public int GetScore()
    {
        return (int)_point;
    }

    void Update()
    {
        if (GameState.Pause) return;
        _point += Time.deltaTime * _pointScale;
        _force += (_force.y > 0 ? Vector3.up : Vector3.down) * Time.deltaTime * 0.1f;
        pointText.text = GetScore() + "";

        if ((int)(_specialTime * 10) == 0)
        {
            specialSlider.enabled = false;
            specialSlider.gameObject.SetActive(false);
        }
        else
        {
            specialSlider.enabled = true;
            specialSlider.gameObject.SetActive(true);
            specialSlider.value = _specialTime / 10f;
        }
    }

    public void GravityChange()
    {
        _force *= -1;
        _rigidbody.linearVelocity = Vector3.zero;
        reverse.Play();
    }
    public void ColorChange()
    {
        Color color = Color.white;
        _material.color = color;
        specialColor.color = color;
    }

    public void ColorChange(Color color)
    {
        special.Play();
        _material.color = color;
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
            _specialTime = i;
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
        _pointScale = 2f;
        ColorChange(Color.yellow);
        StartCoroutine(Simple(dur));
    }

    IEnumerator Simple(float delayTime)
    {
        yield return SpecialDuration(delayTime);
        _pointScale = 1f;
        ColorChange();
    }

    public void Center(float dur)
    {
        var prevForce = _force;
        _force = new Vector3(0, 0, 0);
        transform.position = new Vector3(0, 6, 0);
        _rigidbody.linearVelocity = new Vector3(0, 0, 0);
        ColorChange(Color.cyan);
        StartCoroutine(Fall(dur, prevForce));
    }

    IEnumerator Fall(float delayTime, Vector3 prevForce)
    {
        yield return SpecialDuration(delayTime);
        _force = prevForce;
        RaycastHit hit;
        if (!Physics.Raycast(new Ray(transform.position, _force), out hit, 10f))
        {
            _force *= -1;
        }
        ColorChange();
    }
}
