using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagment: MonoBehaviour
{
    public void ToEndScene()
    {
        SceneManager.LoadScene("End", LoadSceneMode.Single);
    }

    public void ToBeginScene()
    {
        SceneManager.LoadScene("Start", LoadSceneMode.Single);
    }

    public void ToPlayScene()
    {
        SceneManager.LoadScene("Play", LoadSceneMode.Single);
    }
}
