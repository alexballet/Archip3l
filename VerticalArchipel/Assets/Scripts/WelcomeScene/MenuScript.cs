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
        const int buttonHeight = 50;

        if (
          GUI.Button(
            new Rect(
              Screen.width / 2 - (buttonWidth / 2),
              (5 * Screen.height / 10) - (buttonHeight / 2),
              buttonWidth,
              buttonHeight
            ),
            "Demarrer !"
          )
        )
        {
            SceneManager.LoadScene("GameInitScene");
        }
        if (
          GUI.Button(
            new Rect(
              Screen.width / 2 - (buttonWidth / 2),
              (7 * Screen.height / 10) - (buttonHeight / 2),
              buttonWidth,
              buttonHeight
            ),
            "Crédits"
          )
        )
        {
            SceneManager.LoadScene("GameInitScene");
        }
    }
}