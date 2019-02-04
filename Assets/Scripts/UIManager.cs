using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {
    public GameObject mainMenu;
    public GameObject currentLevel;
    public GameObject gameOverPopup;
    public GameObject gameOverCurrentLevel;
    public GameObject victoryPopup;
    public GameObject victoryCurrentLevel;
    public GameObject cake;
    public GameObject currentCakes;

    public void Awake() {
        ToggleGameOver(false);
        ToggleVictory(false);
        ToggleMainMenu(false);
        ToggleCakes(false);
    }

    public void ToggleGameOver(bool show) {
        if (show) {
            gameOverCurrentLevel.GetComponent<Text>().text = "" + GameManager.instance.level;
        }
        gameOverPopup.SetActive(show);
    }

    public void ToggleVictory(bool show) {
        if (show) {
            victoryCurrentLevel.GetComponent<Text>().text = "" + GameManager.instance.level;
        }
        victoryPopup.SetActive(show);
    }

    public void ToggleMainMenu(bool show) {
        if (show) {
            currentLevel.GetComponent<Text>().text = "" + GameManager.instance.level;
        }
        mainMenu.SetActive(show);
    }

    public void ToggleCakes(bool show) {
        if (show) {
            SetCurrentCakesText();
        }
        cake.SetActive(show);
        currentCakes.SetActive(show);
    }

    public void SetCurrentCakesText() {
        currentCakes.GetComponent<Text>().text = "x " + GameManager.instance.lives;
    }


    // click handlers
    public void StartMission() {
        ToggleMainMenu(false);
        ToggleGameOver(false);
        ToggleVictory(false);
        GameManager.instance.InitGame();
    }

    public void ToMainMenu() {
        ToggleMainMenu(true);
        ToggleGameOver(false);
        ToggleVictory(false);
    }

    public void ResetGame() {
        GameManager.instance.level = 1;
        SaveSystem.SetInt("level", 1);
        currentLevel.GetComponent<Text>().text = "" + GameManager.instance.level;
    }

    public void Cheat() {
        GameManager.instance.level++;
        SaveSystem.SetInt("level", GameManager.instance.level);
        currentLevel.GetComponent<Text>().text = "" + GameManager.instance.level;
    }
}
