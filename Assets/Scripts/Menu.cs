using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour {

    public Scene level1;

    public void LoadLevel1()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
