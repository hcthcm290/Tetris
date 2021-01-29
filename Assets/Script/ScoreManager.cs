using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    static public int score;
    static public int highScore;
    static public int level;
    static public int linesToNxtLvl;
    static public List<HighscoreEntry> highscoreList;
    static private bool Initalized = false;

    static int[] pointList = { 40, 70, 100, 200 };

    private void Awake()
    {
        string json = PlayerPrefs.GetString("highscore");
        Highscore hs = JsonUtility.FromJson<Highscore>(json);
        highscoreList = hs.list;
        if(highscoreList == null)
        {
            highscoreList = new List<HighscoreEntry>();
        }

        int nMissing = 5 - highscoreList.Count;
        for(; nMissing>0; nMissing--)
        {
            highscoreList.Add(new HighscoreEntry(0, "AAA"));
        }

        GameObject highScore = GameObject.Find("HighScore");
        if (highScore != null)
        {
            GameObject.Find("HighScore").GetComponent<Text>().text = highscoreList[0].score.ToString();
        }

        Initalized = true;
    }

    static public void Init()
    {
        if(!Initalized)
        {
            string json = PlayerPrefs.GetString("highscore");
            Highscore hs = JsonUtility.FromJson<Highscore>(json);
            highscoreList = hs.list;
            if (highscoreList == null)
            {
                highscoreList = new List<HighscoreEntry>();
            }

            int nMissing = 5 - highscoreList.Count;
            for (; nMissing > 0; nMissing--)
            {
                highscoreList.Add(new HighscoreEntry(0, "AAA"));
            }

            GameObject highScore = GameObject.Find("HighScore");
            if (highScore != null)
            {
                GameObject.Find("HighScore").GetComponent<Text>().text = highscoreList[0].score.ToString();
            }

            Initalized = true;
        }
    }

    private void Start()
    {
        score = 0;
        level = 1;
        linesToNxtLvl = 10;
        GameObject.Find("Level").GetComponent<UnityEngine.UI.Text>().text = level.ToString();
    }

    public static void AddScore(int nLine)
    {
        score += pointList[nLine - 1] * level;
        GameObject.Find("Score").GetComponent<UnityEngine.UI.Text>().text = score.ToString();
        linesToNxtLvl -= nLine;

        if(linesToNxtLvl <= 0)
        {
            level += 1;
            linesToNxtLvl += 10;

            GameObject.Find("Level").GetComponent<UnityEngine.UI.Text>().text = level.ToString(); // show new level in GUI
        }
    }

    public static void SaveHighscore()
    {
        Highscore hs = new Highscore(highscoreList);
        string json = JsonUtility.ToJson(hs);
        PlayerPrefs.SetString("highscore", json);
        PlayerPrefs.Save();
        Debug.Log(PlayerPrefs.GetString("highscore"));
    }
}

public class Highscore
{
    public List<HighscoreEntry> list;

    public Highscore(List<HighscoreEntry> list_in)
    {
        list = list_in;
    }
}
