using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    public GameObject optionA;
    public GameObject overlayA;
    public GameObject optionB;
    public GameObject overlayB;

    private List<GameObject> options = new List<GameObject>();
    private List<GameObject> overlays = new List<GameObject>();

    // workaround for C# weirdness related to () => stuff
    private class Integer
    {
        public Integer(int x) { this.x = x; }
        public int x;
    }

    void Start()
    {
        options.Add(optionA);
        options.Add(optionB);
        overlays.Add(overlayA);
        overlays.Add(overlayB);
        
        for (var i = 0; i < options.Count; i++)
        {
            Integer i2 = new Integer(i);
            var button = options[i].GetComponent<Button>();
            button.onClick.AddListener(() => SelectMap(i2.x));
        }
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
    }
}
