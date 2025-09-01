using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ScoreScene : MonoBehaviour
{

    public Text score;
    public Text maxScore;

    void Update()
    {
        score.text = "" + GameState.LastScore;
        maxScore.text = "" + GameState.MaxScore;
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }
}
