using UnityEngine;
using System.Collections;

public static class GameState
{
    private static int lastScore = 0;

    private static int maxScore = 0;

    private static bool pause = false;

    public static void LoadData()
    {
        Pause = false;
        if (PlayerPrefs.HasKey("MaxScore"))
            maxScore = PlayerPrefs.GetInt("MaxScore");
        else
            maxScore = 0;
    }

    public static void SaveData()
    {
        PlayerPrefs.SetInt("MaxScore", maxScore);
    }

    public static int LastScore
    {
        get
        {
            return lastScore;
        }

        set
        {
            lastScore = value;
            if (lastScore > maxScore)
            {
                maxScore = lastScore;
                SaveData();
            }
        }
    }

    public static int MaxScore
    {
        get
        {
            return maxScore;
        }
    }

    public static bool Pause
    {
        get
        {
            return pause;
        }

        set
        {
            pause = value;
        }
    }

    public static void Reset()
    {
        lastScore = 0;
        maxScore = 0;
    }
}
