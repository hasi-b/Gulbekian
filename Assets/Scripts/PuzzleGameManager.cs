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
    private void Start()
    {
        LoadData();
    }

    public void LoadData()
    {
        if (currentPuzzleCounter<PuzzleSourceData.Count)
        {
            image.sprite = PuzzleSourceData[currentPuzzleCounter].Sketch;
            riddle.SetText(PuzzleSourceData[currentPuzzleCounter].riddle);
        }
    } 

    public void CheckValidity()
    {
       if(DeviceCameraControl.Instance.CompareCapturedImage(PuzzleSourceData[currentPuzzleCounter].ReferenceImages))
        {
            currentPuzzleCounter++;
            LoadData();
            

        }
        DeviceCameraControl.Instance.PuzzleNextLevel();
    }

}
