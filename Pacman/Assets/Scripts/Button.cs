using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Button : MonoBehaviour {

    public Transform canvas;

	public void PauseGame(){
        if (canvas.gameObject.activeInHierarchy == false)
        {
            canvas.gameObject.SetActive(true);
            Time.timeScale = 0;
            Pacman.PausePacman = true;
            Ghost.PauseGhost = true;
        }
        else
        {
            canvas.gameObject.SetActive(false);
            Time.timeScale = 1;
            Pacman.PausePacman = false;
            Ghost.PauseGhost = false;
        }
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("Game Scene");
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
