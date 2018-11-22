using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour {

    public Scene level1;

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();
    }

    public void LoadLevel1()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
