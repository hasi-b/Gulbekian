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
            currentPuzzleCounter++;
            LoadData();
            

        }
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
