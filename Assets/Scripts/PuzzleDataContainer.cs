using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[CreateAssetMenu(fileName = "PuzzleData", menuName = "PuzzleDataContainer")]
public class PuzzleDataContainer : ScriptableObject
{
    public Sprite Sketch;
    public string riddle;
    public List<Texture2D> ReferenceImages;
    
    public int puzzleNumber;

    public Vector2 location;
    public float radius;
}
