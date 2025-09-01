using System.Collections.Generic;
using UnityEngine;

public class BaseLoader : MonoBehaviour
{
    public bool go = true;
    public GameObject[] baseObjects;
    public GameObject[] specialObjects;
    private Vector3 upLoad = new Vector3(0, 12, 0);
    private Vector3 downLoad = new Vector3(0, 1, 0);
    private Vector3 upMiddleLoad = new Vector3(0, 8, 0);
    private Vector3 downMiddleLoad = new Vector3(0, 4, 0);
    public List<GameObject> activeObjects = new List<GameObject>();
    private Vector3 direction = new Vector3(-1, 0, 0);
    private readonly float speed = 7f;
    private readonly float maxWidth = 20f;

    private float _speed = 0f;
    // Use this for initialization
    void Start()
    {
        Time.timeScale = 1f;
        InvokeRepeating("LoadBase", 0.1f, 1f);
        InvokeRepeating("LoadSpecialElements", 20f, 15f);
        upLoad.x = maxWidth;
        downLoad.x = maxWidth;
        downMiddleLoad.x = maxWidth;
        upMiddleLoad.x = maxWidth;
        _speed = speed;
    }

    private void Update()
    {
        if (go && !GameState.Pause)
        {
            //Time.timeScale += 0.0003f;
            _speed += Time.deltaTime * 0.1f;

            List<GameObject> deleteables = new List<GameObject>();
            foreach (GameObject obj in activeObjects)
            {
                obj.transform.position += direction * _speed * Time.deltaTime;
                if (obj.transform.position.x < -maxWidth) deleteables.Add(obj);
            }
            foreach (GameObject obj in deleteables)
            {
                activeObjects.Remove(obj);
                Destroy(obj, 0.1f);
            }
        }
    }

    void LoadSpecialElements()
    {
        GameObject newObj;
        if (Random.value > 0.3f)
            newObj = GetSpecialObject((upLoad + upMiddleLoad) / 2);
        else
            if (Random.value > 0.3f)
            newObj = GetSpecialObject((downMiddleLoad + upMiddleLoad) / 2);
        else
            newObj = GetSpecialObject((downLoad + downMiddleLoad) / 2);
        activeObjects.Add(newObj);
    }

    void LoadBase()
    {
        GameObject newObj;
        if (Random.value > 0.5f)
        {
            if (Random.value > 0.2f)
                newObj = GetBaseObject(upLoad);
            else
                newObj = GetBaseObject(upMiddleLoad);
        }
        else
        {
            if (Random.value > 0.2f)
                newObj = GetBaseObject(downLoad);
            else
                newObj = GetBaseObject(downMiddleLoad);
        }
        activeObjects.Add(newObj);
        
    }

    GameObject GetBaseObject(Vector3 pos)
    {
        int i = (int)(Random.value * baseObjects.Length);
        if (i >= baseObjects.Length) i = baseObjects.Length - 1;
        GameObject obj = Instantiate(baseObjects[i]);
        obj.transform.position = pos;
        return obj;
    }

    GameObject GetSpecialObject(Vector3 pos)
    {
        int i = (int)(Random.value * specialObjects.Length);
        if (i >= specialObjects.Length) i = specialObjects.Length - 1;
        GameObject obj = Instantiate(specialObjects[i]);
        obj.transform.position = pos;
        return obj;
    }
    
    float prevTime = 1f;
    public void Pause()
    {
        Debug.Log("Pause");
        if (!GameState.Pause)
        {
            prevTime = Time.timeScale;
            Time.timeScale = 0f;
            GameState.Pause = true;
        }
        else
        {
            Time.timeScale = prevTime;
            GameState.Pause = false;
        }
    }
}
