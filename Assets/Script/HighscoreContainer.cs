using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighscoreContainer : MonoBehaviour
{
    private Transform entryTemplate;
    private List<Transform> highscoreTransformList; 

    void Awake()
    {
        entryTemplate = transform.Find("Highscore Entry Template");

        entryTemplate.gameObject.SetActive(false);

        CreateHighscoreContainer();
        LoadHighscoreToContainer();
    }

    void CreateHighscoreContainer()
    {
        highscoreTransformList = new List<Transform>();
        float templateHeight = 50;
        ScoreManager.Init();
        for (int i = 0; i < 5; i++)
        {
            Transform entry = Instantiate(entryTemplate, transform);

            RectTransform entryRect = entry.GetComponent<RectTransform>();
            RectTransform entryTemplateRect = entryTemplate.GetComponent<RectTransform>();
            entryRect.anchoredPosition = new Vector2(0, entryTemplateRect.anchoredPosition.y - templateHeight * i);

            if(i%2 != 0)
            {
                entry.Find("background").gameObject.SetActive(false);
            }

            highscoreTransformList.Add(entry);
        }
    }

    void LoadHighscoreToContainer()
    {
        SortHighscoreList();
        for (int i = 0; i < 5; i++)
        {
            int rank = i + 1;
            string strRank = rank.ToString();
            switch (rank)
            {
                case 1: strRank = "1ST"; break;
                case 2: strRank = "2ND"; break;
                case 3: strRank = "3RD"; break;
                default: strRank = rank.ToString() + "TH"; break;
            }

            highscoreTransformList[i].Find("txtPos").GetComponent<Text>().text = strRank;
            highscoreTransformList[i].Find("txtScore").GetComponent<Text>().text = ScoreManager.highscoreList[i].score.ToString();
            highscoreTransformList[i].Find("txtName").GetComponent<Text>().text = ScoreManager.highscoreList[i].name.ToString();
            highscoreTransformList[i].gameObject.SetActive(true);
        }
    }

    public void Refresh()
    {
        LoadHighscoreToContainer();
    }

    private void SortHighscoreList()
    {
        for(int i=0; i<ScoreManager.highscoreList.Count; i++)
            for(int j=i; j<ScoreManager.highscoreList.Count; j++)
                if(ScoreManager.highscoreList[i].score < ScoreManager.highscoreList[j].score)
                {
                    HighscoreEntry temp = ScoreManager.highscoreList[i];
                    ScoreManager.highscoreList[i] = ScoreManager.highscoreList[j];
                    ScoreManager.highscoreList[j] = temp;
                }
    }

    public HighscoreEntry AddNewScore(int score, string name)
    {
        HighscoreEntry newEntry = new HighscoreEntry(score, name);

        ScoreManager.highscoreList.Add(newEntry);
        SortHighscoreList();
        LoadHighscoreToContainer();


        Debug.Log("Haha");
        return newEntry;
    }
}

[System.Serializable]
public class HighscoreEntry
{
    public int score;
    public string name;

    public HighscoreEntry(int score_in, string name_in)
    {
        score = score_in;
        name = name_in;
    }
}