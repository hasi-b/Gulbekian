using UnityEngine;
using System.Collections;
using System;
using TMPro;

public class TeleType : MonoBehaviour
{

    public Action OnTeleTextComplete = delegate { };
    [Range(0, 10)]
    public float RevealSpeed = 5.0f;

    private string label01 = "";
    private string label02 = "";


    private TMP_Text m_textMeshPro;


    void Awake()
    {
        // Get Reference to TextMeshPro Component
        m_textMeshPro = GetComponent<TMP_Text>();
        label01 = label02 = m_textMeshPro.text;
        //m_textMeshPro.text = label01;
        m_textMeshPro.enableWordWrapping = true;
        m_textMeshPro.alignment = TextAlignmentOptions.Top;

        //if (GetComponentInParent(typeof(Canvas)) as Canvas == null)
        //{
        //    GameObject canvas = new GameObject("Canvas", typeof(Canvas));
        //    gameObject.transform.SetParent(canvas.transform);
        //    canvas.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;

        //    // Set RectTransform Size
        //    gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(500, 300);
        //    m_textMeshPro.fontSize = 48;
        //}
    }


    IEnumerator Start()
    {

        // Force and update of the mesh to get valid information.
        m_textMeshPro.ForceMeshUpdate();


        int totalVisibleCharacters = m_textMeshPro.textInfo.characterCount; // Get # of Visible Character in text object
        int counter = 0;
        int visibleCount = 0;

        while (true)
        {
            visibleCount = counter % (totalVisibleCharacters + 1);

            m_textMeshPro.maxVisibleCharacters = visibleCount; // How many characters should TextMeshPro display?

            // Once the last character has been revealed, wait 1.0 second and start over.
            if (visibleCount >= totalVisibleCharacters)
            {
                yield return new WaitForSeconds(1.0f);
                m_textMeshPro.text = label02;
                yield return new WaitForSeconds(1.0f);
                m_textMeshPro.text = label01;
                yield return new WaitForSeconds(1.0f);
            }

            counter += 1;

            yield return new WaitForSeconds(RevealSpeed / 100);
            //print("counter" + counter);
            //print(totalVisibleCharacters);
            if (counter > totalVisibleCharacters)
            {
                //Debug.Log("Done revealing the text.");
                OnTeleTextComplete?.Invoke();
                break;
            }
        }

    }
}
