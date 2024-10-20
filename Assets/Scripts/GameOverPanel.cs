using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class GameOverPanel : MonoBehaviour
{
    [SerializeField] Image[] duckFootPrintImages;
    [SerializeField] Image duckImage;
    [SerializeField] Button continueButton;
    [Space]
    [SerializeField] Color alphaZero;
    [SerializeField] Color alphaOne;

    private void Awake()
    {
        continueButton.onClick.AddListener(() =>
        {
            SceneManagement.Instance.LoadScene(0);
        });
        continueButton.interactable = false;
        continueButton.gameObject.SetActive(false);

        foreach (Image footPrint in duckFootPrintImages)
        {
            footPrint.color = alphaZero;
        }
        duckImage.color = alphaZero;
    }

    private void OnEnable()
    {
        StartCoroutine(IE_ShowFootprints());
    }

    IEnumerator IE_ShowFootprints()
    {
        foreach (Image footPrint in duckFootPrintImages)
        {
            yield return new WaitForSeconds(0.5f);
            footPrint.color = alphaOne;
            footPrint.DOFade(0, 3);
        }
        //yield return null;
        yield return new WaitForSeconds(0.2f);

        duckImage.DOFade(1, 1).OnComplete(() =>
        {
            //duckImage.DOFade(0, 3).OnComplete(() =>
            //{
            continueButton.interactable = true;
            continueButton.gameObject.SetActive(true);
            //});
        });
    }
}
