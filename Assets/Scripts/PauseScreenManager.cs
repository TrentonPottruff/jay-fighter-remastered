using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.Networking.Match;

/*
 * AUTHOR: Trenton Pottruff
 * CONTRUBITOR: Garrett Nicholas
 */

public class PauseScreenManager : MonoBehaviour {
    private bool opened = false; //Is the pause screen open?

    private Animator anim;

    private void Start() {
        //Grab animator component
        anim = GetComponent<Animator>();
    }

    /// <summary>
    /// Opens the pause screen
    /// </summary>
    public void OpenScreen() {
        anim.Play("pauseScreen-Open");
        opened = true;
        Game.PAUSED = true;
    }

    /// <summary>
    /// Closes the pause screen
    /// </summary>
    public void CloseScreen() {
        anim.Play("pauseScreen-Close");
        opened = false;
        Game.PAUSED = false;
    }

    public void CloseScreenWithoutUnpausing() {
        //UNDONE This shouldn't be empty?
    }

    /// <summary>
    /// Toggles the pause screen
    /// </summary>
    public void ToggleScreen() {
        if (opened)
            CloseScreen();
        else
            OpenScreen();
    }


    /// <summary>
    /// Exits the game and returns the user to the main menu
    /// </summary>
    public void ExitGame() {
        SceneManager.LoadScene("MainMenu");
    }

    /// <summary>
    /// Gets wether the pause screen is open or not
    /// </summary>
    /// <returns>True if the pause screen is open, False if it's closed</returns>
    public bool getOpened() {
        return opened;
    }
}
