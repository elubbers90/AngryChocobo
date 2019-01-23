using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {
    public GameObject gameOverPopup;
    public GameObject gameOverCurrentLevel;
    public GameObject victoryPopup;
    public GameObject victoryCurrentLevel;

    public void Awake() {
        HideGameOver();
        HideVictory();
    }

    public void ShowGameOver() {
        gameOverCurrentLevel.GetComponent<Text>().text = "" + GameManager.instance.level;
        gameOverPopup.SetActive(true);
    }

    public void ShowVictory() {
        victoryCurrentLevel.GetComponent<Text>().text = "" + GameManager.instance.level;
        victoryPopup.SetActive(true);
    }

    private void HideGameOver() {
        gameOverPopup.SetActive(false);
    }

    private void HideVictory() {
        victoryPopup.SetActive(false);
    }

    public void StartMission() {
        HideGameOver();
        HideVictory();
        GameManager.instance.InitGame();
    }
}
