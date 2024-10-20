using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PuzzleGameManager : MonoBehaviour
{
    [SerializeField]
    List<PuzzleDataContainer> PuzzleSourceData;
    [SerializeField]
    int currentPuzzleCounter =0;
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
   
    private void Start()
    {
        LoadData();
    }

    public void LoadData()
    {
        if (currentPuzzleCounter<PuzzleSourceData.Count)
        {
            image.sprite = PuzzleSourceData[currentPuzzleCounter].Sketch;
            fullScreenImage.sprite = PuzzleSourceData[currentPuzzleCounter].Sketch;
            riddle.SetText(PuzzleSourceData[currentPuzzleCounter].riddle);
        }
    } 

    public void CheckValidity()
    {
        
        Debug.Log(GPSLocationChecker.instance.IsPlayerWithinRadius(PuzzleSourceData[currentPuzzleCounter].location.x, PuzzleSourceData[currentPuzzleCounter].location.y, PuzzleSourceData[currentPuzzleCounter].radius));
       if(DeviceCameraControl.Instance.CompareCapturedImage(PuzzleSourceData[currentPuzzleCounter].ReferenceImages)|| GPSLocationChecker.instance.IsPlayerWithinRadius(PuzzleSourceData[currentPuzzleCounter].location.x, PuzzleSourceData[currentPuzzleCounter].location.y, PuzzleSourceData[currentPuzzleCounter].radius))
        {

            //macthedvpopup
            //panel off

            successPanel.SetActive(true);
            

        }
        else
        {
            // didnt macth popup
            //panel off
            failurePanel.SetActive(true);

        }




        
    }

    public void successButton()
    {
        successPanel.SetActive(false);
        narrativePanel.SetActive(true);
        DeviceCameraControl.Instance.submitButton.SetActive(false);

    }


    public void failureButton() {

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
}
