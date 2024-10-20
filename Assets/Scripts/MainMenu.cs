using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] Button playButton;

    private void Awake()
    {
        playButton.onClick.AddListener(() =>
        {
            SoundManager.Instance.PlayUIButtonSound();
            SceneManagement.Instance.LoadScene(1);
        });
    }
}
