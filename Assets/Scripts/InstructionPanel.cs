using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InstructionPanel : MonoBehaviour
{
    [SerializeField] RectTransform mainPanelRect;
    [SerializeField] Button okButton;

    TeleType teleType;

    private void Awake()
    {
        okButton.onClick.AddListener(() =>
        {
            SoundManager.Instance.PlayUIButtonSound();
            gameObject.SetActive(false);
        });
        okButton.interactable = false;
        mainPanelRect.gameObject.SetActive(true);
    }

    private void Start()
    {
        teleType = GetComponentInChildren<TeleType>();
        teleType.OnTeleTextComplete += OnTeleTextComplete;
    }

    private void OnTeleTextComplete()
    {
        okButton.interactable = true;
    }
}
