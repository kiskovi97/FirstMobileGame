using UnityEngine;

public class Quicker : MonoBehaviour {

    public float scale = 1.5f;
    public float duration = 2f;

    private void OnTriggerEnter(Collider other)
    {
        Player player = other.gameObject.GetComponent<Player>();
        if (player != null)
        {
            player.TimeScale(scale, duration);
            gameObject.SetActive(false);
        }
    }

}
