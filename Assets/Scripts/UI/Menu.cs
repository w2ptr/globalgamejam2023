using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject settingsMenu;

    private string[] levels = {
        "Level_1_vegetalViolence",
        "Level_2_entwined",
        "Level_3_TwinValley",
        "Level_4_QuatroCarnivoria",
        "SpriteShapeTest",
    };

    void Start()
    {
        ShowMainMenu();
    }

    public void ShowMainMenu()
    {
        mainMenu.SetActive(true);
        settingsMenu.SetActive(false);
    }
    public void ShowSettings()
    {
        mainMenu.SetActive(false);
        settingsMenu.SetActive(true);
    }
    public void StartGame()
    {
        int level = PlayerPrefs.GetInt("mapSelection", 0);
        SceneManager.LoadScene(levels[level]);
    }
}
