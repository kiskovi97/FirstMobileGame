using UnityEngine;
using System.Collections;

public class DoublePoints : MonoBehaviour
{
    public float duration = 2f;

    private void OnTriggerEnter(Collider other)
    {
        Player player = other.gameObject.GetComponent<Player>();
        if (player != null)
        {
            player.Double(duration);
            gameObject.SetActive(false);
        }
    }
}
