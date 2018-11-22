using UnityEngine;

public class Center : MonoBehaviour
{
    public float duration = 2f;

    private void OnTriggerEnter(Collider other)
    {
        Player player = other.gameObject.GetComponent<Player>();
        if (player != null)
        {
            player.Center(duration);
            gameObject.SetActive(false);
        }
    }
}
