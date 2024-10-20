using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PuzzleGameManager : MonoBehaviour
{
    [SerializeField]
    List<PuzzleDataContainer> PuzzleSourceData;
    [SerializeField]
    int currentPuzzleCounter = 0;
    [SerializeField]
    TextMeshProUGUI riddle;
    [SerializeField]
    Image image;
    [SerializeField]
    Image fullScreenImage;
    [SerializeField]
    GameObject fullScreenPanel;
    [SerializeField]
    GameObject successPanel;
    [SerializeField]
    GameObject failurePanel;
    [SerializeField]
    GameObject narrativePanel;
    [SerializeField]
    Button narrativeButton;

    [SerializeField]
    List<Texture2D> narrativeImages;
    [SerializeField]
    Image narrativeImageHolder;
    int narrativeCounter = 0;
    [SerializeField]
    float fadeDuration = 0.5f;

    [SerializeField] GameOverPanel gameOverPanel;
    private void Start()
    {
        LoadData();
    }

    public void LoadData()
    {
        if (currentPuzzleCounter < PuzzleSourceData.Count)
        {
            image.sprite = PuzzleSourceData[currentPuzzleCounter].Sketch;
            fullScreenImage.sprite = PuzzleSourceData[currentPuzzleCounter].Sketch;
            riddle.SetText(PuzzleSourceData[currentPuzzleCounter].riddle);
            narrativeImages = PuzzleSourceData[currentPuzzleCounter].NarrativeImages;
        }
        else
        {
            gameOverPanel.gameObject.SetActive(true);
        }
    }

    public void CheckValidity()
    {

        Debug.Log(GPSLocationChecker.instance.IsPlayerWithinRadius(PuzzleSourceData[currentPuzzleCounter].location.x, PuzzleSourceData[currentPuzzleCounter].location.y, PuzzleSourceData[currentPuzzleCounter].radius));
        if (0 == 0 || DeviceCameraControl.Instance.CompareCapturedImage(PuzzleSourceData[currentPuzzleCounter].ReferenceImages) || GPSLocationChecker.instance.IsPlayerWithinRadius(PuzzleSourceData[currentPuzzleCounter].location.x, PuzzleSourceData[currentPuzzleCounter].location.y, PuzzleSourceData[currentPuzzleCounter].radius))
        {

            //macthedvpopup
            //panel off

            successPanel.SetActive(true);
            SoundManager.Instance.PlaySuccessSound();

        }
        else
        {
            // didnt macth popup
            //panel off
            failurePanel.SetActive(true);
            SoundManager.Instance.PlayFailureSound();
        }





    }

    public void successButton()
    {
        successPanel.SetActive(false);
        narrativePanel.SetActive(true);
        DeviceCameraControl.Instance.submitButton.SetActive(false);

        NarrativeFunction();

    }


    public void failureButton()
    {

        failurePanel.SetActive(false);
        DeviceCameraControl.Instance.PuzzleNextLevel();
    }

    public void LoadNextPuzzle()
    {
        narrativePanel.SetActive(false);
        currentPuzzleCounter++;
        LoadData();
        DeviceCameraControl.Instance.PuzzleNextLevel();
    }


    public void TurnOnFullScreen()
    {
        fullScreenPanel.SetActive(true);
    }

    public void TurnOffFullScreen()
    {
        fullScreenPanel.SetActive(false);
    }


    void NarrativeFunction()
    {
        NarrativeSlide();

        if (narrativeCounter < narrativeImages.Count)
        {

            narrativeButton.onClick.RemoveAllListeners();
            narrativeButton.onClick.AddListener(NarrativeSlide);
            Debug.Log("Hereee");
        }



    }

    void NarrativeSlide()
    {

        Sprite newSprite = TextureToSpriteConverter(narrativeImages[narrativeCounter]);
        // Assign the sprite to the SpriteRenderer component


        StartCoroutine(FadeOutAndIn(newSprite));
        narrativeCounter++;

        if (narrativeCounter >= narrativeImages.Count)
        {
            narrativeCounter = 0;
            narrativeButton.onClick.RemoveAllListeners();
            narrativeButton.onClick.AddListener(LoadNextPuzzle);
        }


    }



    Sprite TextureToSpriteConverter(Texture2D texture)
    {
        // Define the rect of the sprite (starting from top-left corner to the full texture size)
        Rect rect = new Rect(0, 0, texture.width, texture.height);
        // Create a new sprite from the texture
        Sprite sprite = Sprite.Create(texture, rect, new Vector2(0.5f, 0.5f)); // Pivot in the center
        return sprite;
    }


    // Coroutine to handle fade-out, change sprite, and fade-in
    private IEnumerator FadeOutAndIn(Sprite newSprite)
    {
        // Fade out the current sprite
        if (narrativeCounter != 0)
        {
            yield return StartCoroutine(FadeOut());
        }


        // Change to the new sprite
        narrativeImageHolder.sprite = newSprite;

        // Fade in the new sprite
        yield return StartCoroutine(FadeIn());
    }

    // Coroutine to fade out the sprite (alpha to 0)
    private IEnumerator FadeOut()
    {
        float elapsedTime = 0f;
        Color color = narrativeImageHolder.color;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            color.a = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
            narrativeImageHolder.color = color;
            yield return null;
        }

        // Ensure it's fully transparent
        color.a = 0f;
        narrativeImageHolder.color = color;
    }

    // Coroutine to fade in the sprite (alpha to 1)
    private IEnumerator FadeIn()
    {
        float elapsedTime = 0f;
        Color color = narrativeImageHolder.color;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            color.a = Mathf.Lerp(0f, 1f, elapsedTime / fadeDuration);
            narrativeImageHolder.color = color;
            yield return null;
        }

        // Ensure it's fully opaque
        color.a = 1f;
        narrativeImageHolder.color = color;
    }


}
