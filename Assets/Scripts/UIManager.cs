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
    public GameObject eggSwitcher;

    public void Awake() {
        ToggleGameOver(false);
        ToggleVictory(false);
        ToggleEggSwitcher(false);
        ToggleMainMenu(false);
    }

    public void ShowEggType(int eggType) {
        switch (eggType) {
            case 0:
                eggSwitcher.GetComponent<Image>().color = new Color32(0, 255, 225, 255);
                break;
            case 1:
                eggSwitcher.GetComponent<Image>().color = new Color32(255, 154, 0, 255);
                break;
        }
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

    public void ToggleEggSwitcher(bool show) {
        eggSwitcher.SetActive(show);
    }



    // click handlers
    public void StartMission() {
        ToggleMainMenu(false);
        ToggleGameOver(false);
        ToggleVictory(false);
        GameManager.instance.InitGame();
    }

    public void SwitchEggType() {
        GameManager.instance.SwitchEggType();
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
}
