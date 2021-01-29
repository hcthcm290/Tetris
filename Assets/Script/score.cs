using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class score : MonoBehaviour
{
    [SerializeField] HighscoreContainer highscoreContainer;
    int lastPlayScore;
    [SerializeField] Transform inputField;
    [SerializeField] Transform playerNameField;
    [SerializeField] Transform gameoverTitle;
    [SerializeField] Transform highscoreTitle;
    [SerializeField] Transform playerScore;
    HighscoreEntry highscoreEntry;


    // Start is called before the first frame update
    void Start()
    {
        lastPlayScore = ScoreManager.score;
        inputField.gameObject.SetActive(false);
        playerScore.GetComponent<Text>().text = lastPlayScore.ToString();
        if(lastPlayScore > ScoreManager.highscoreList[4].score)
        {
            // High score effect in here
            gameoverTitle.gameObject.SetActive(false);
            highscoreTitle.gameObject.SetActive(true);

            // Popup field letting player enter his name
            StartCoroutine(PopupInputField(3));

            // Add Highscore to Highscore table
            highscoreEntry = highscoreContainer.AddNewScore(lastPlayScore, "");
            Debug.Log("Save");
        }
        else
        {
            //show the gameover title and their score
            gameoverTitle.gameObject.SetActive(true);
            highscoreTitle.gameObject.SetActive(false);


        }
    }

    private IEnumerator PopupInputField(float delay)
    {
        yield return new WaitForSeconds(delay);

        inputField.GetComponent<EasyTween>().OpenCloseObjectAnimation();
    }

    public void CommitHighscorePlayerName()
    {
        string playerName = playerNameField.GetComponent<Text>().text;
        highscoreEntry.name = playerName;
        highscoreContainer.Refresh();
    }

    public void SaveHighscore()
    {
        ScoreManager.SaveHighscore();
    }
}
