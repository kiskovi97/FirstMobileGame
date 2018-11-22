using UnityEngine;
using System.Collections;

public class SpecialObject : MonoBehaviour
{
    void Update()
    {
        transform.Rotate(new Vector3(0, 0, 1), 1f);
    }
}
