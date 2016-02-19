using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Script of menu
/// </summary>
public class MenuScript : MonoBehaviour
{
    void OnGUI()
    {
        const int buttonWidth = 120;
        const int buttonHeight = 80;

        if (
          GUI.Button(
            new Rect(
              Screen.width / 2 - (buttonWidth / 2),
              (3 * Screen.height / 4) - (buttonHeight / 2),
              buttonWidth,
              buttonHeight
            ),
            "Demarrer !"
          )
        )
        {
            SceneManager.LoadScene("GameInitScene");
        }
    }
}