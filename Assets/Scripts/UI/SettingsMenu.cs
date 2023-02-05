using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
// using GlobalGameJam2023.Globals;
using GlobalGameJam2023;

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
    public GameObject optionE;
    public GameObject overlayE;

    [SerializeField] private List<MapUIElement> _uiElements;

    private List<GameObject> options;
    private List<GameObject> overlays;

    void Start()
    {
        options = new List<GameObject>() {
            optionA, optionB, optionC, optionD, optionE,
        };
        overlays = new List<GameObject>() {
            overlayA, overlayB, overlayC, overlayD, overlayE,
        };
        
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

    public void Select(MapUIElement mapUIElement)
    {
        foreach (MapUIElement element in _uiElements)
        {
            if (mapUIElement != element)
            {
                element.IsSelected = false;
            }
        }
    }

    public void TryDeselect(MapUIElement mapUIElement)
    {
        mapUIElement.IsSelected = false;
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
