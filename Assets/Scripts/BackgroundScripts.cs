using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// all this does is run on all scenes to allow players to return to the main
// menu
public class BackgroundScripts : MonoBehaviour
{
    // don't want users to spam esc. and fuck up loading by loading too many
    // scenes simultaneously
    private bool loading = false;

    void OnGUI()
    {
        if (Event.current.type != EventType.KeyDown) return;

        switch (Event.current.keyCode)
        {
        case KeyCode.Escape:
            if (SceneManager.GetActiveScene().name == "UI")
            {
                var canvas = GameObject.Find("Canvas");
                var script = canvas.GetComponent<Menu>();
                script.ShowMainMenu();
            }
            else if (!loading)
            {
                loading = true;
                SceneManager.LoadScene("UI");
            }
            break;
        default:
            break;
        }
    }
}
