using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
// using GlobalGameJam2023.Globals;

public class SettingsMenu : MonoBehaviour
{
    public GameObject optionA;
    public GameObject overlayA;
    public GameObject optionB;
    public GameObject overlayB;
    public GameObject optionC;
    public GameObject overlayC;
    public GameObject optionD;
    public GameObject overlayD;

    private List<GameObject> options = new List<GameObject>();
    private List<GameObject> overlays = new List<GameObject>();

    void Start()
    {
        options.Add(optionA);
        options.Add(optionB);
        options.Add(optionC);
        options.Add(optionD);
        overlays.Add(overlayA);
        overlays.Add(overlayB);
        overlays.Add(overlayC);
        overlays.Add(overlayD);
        
        for (var i = 0; i < options.Count; i++)
        {
            object i2 = i;
            var button = options[i].GetComponent<Button>();
            button.onClick.AddListener(() => SelectMap((int) i2));

            if (i != 0)
            {
                overlays[i].SetActive(false);
            }
        }

        overlays[0].SetActive(true);
        PlayerPrefs.SetInt("mapSelection", 0);
    }

    private void SelectMap(int x)
    {
        for (var i = 0; i < overlays.Count; i++)
        {
            if (i != x)
            {
                overlays[i].SetActive(false);
            }
        }
        overlays[x].SetActive(true);
        PlayerPrefs.SetInt("mapSelection", x);
    }
}
