using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagement : MonoBehaviour
{
    public static SceneManagement Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void LoadScene(int buildIndex)
    {

        SceneManager.LoadScene(buildIndex);

    }

    public void RestartScene()
    {

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

    }
}
