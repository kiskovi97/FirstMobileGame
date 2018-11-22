using UnityEngine;
using UnityEngine.SceneManagement;

public class Hell : MonoBehaviour {
    private Scene scene;

    private void Start()
    {
        scene = SceneManager.GetActiveScene();
    }

    private void OnTriggerEnter(Collider other)
    {
        Player player = other.gameObject.GetComponent<Player>();
        if (player != null)
        {
            GameState.LastScore = player.GetScore();
            SceneManager.LoadScene(scene.buildIndex + 1);
        }
       
    }
}
