using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {
    public GameObject splashScreen;
    public GameObject loadingText;

    public GameObject mainMenu;
    public GameObject currentMonth;
    public GameObject currentYear;
    public GameObject startgameButton;

    public GameObject gameOverPopup;
    public GameObject gameOverText;
    public GameObject gameOverButton1;
    public GameObject gameOverButton2;


    public GameObject victoryPopup;
    public GameObject victoryText;
    public GameObject victoryButton;


    public GameObject cake;
    public GameObject currentCakes;

    public void Awake() {
        ToggleGameOver(false);
        ToggleVictory(false);
        mainMenu.SetActive(false);
        ToggleCakes(false);
    }

    public IEnumerator ToggleSplashScreen(bool show) {
        StartCoroutine(FadeImage(show, splashScreen));
        StartCoroutine(FadeText(show, loadingText));
        mainMenu.SetActive(true);
        currentMonth.SetActive(false);
        currentYear.SetActive(false);
        startgameButton.SetActive(false);
        yield return new WaitForSeconds(1);
        ToggleMainMenu(true);
    }

    public void ToggleGameOver(bool show) {
        gameOverPopup.SetActive(show);
        StartCoroutine(ScaleObject(show, gameOverButton1));
        StartCoroutine(ScaleObject(show, gameOverButton2));
        StartCoroutine(ScaleObject(show, gameOverText));
    }

    public void ToggleVictory(bool show) {
        victoryPopup.SetActive(show);
        StartCoroutine(ScaleObject(show, victoryText));
        StartCoroutine(ScaleObject(show, victoryButton));
    }

    private void SetCurrentProgress() {
        currentYear.GetComponent<Text>().text = "Year " + (Mathf.Floor((GameManager.instance.level + 5) / 12) + 1);
        switch ((GameManager.instance.level + 5) % 12) {
            case 0:
                SetCurrentMonth("January", new Color32(0, 135, 255, 255));
                break;
            case 1:
                SetCurrentMonth("February", new Color32(0, 135, 255, 255));
                break;
            case 2:
                SetCurrentMonth("March", new Color32(255, 75, 235, 255));
                break;
            case 3:
                SetCurrentMonth("April", new Color32(255, 75, 235, 255));
                break;
            case 4:
                SetCurrentMonth("May", new Color32(255, 75, 235, 255));
                break;
            case 5:
                SetCurrentMonth("June", new Color32(0, 255, 40, 255));
                break;
            case 6:
                SetCurrentMonth("July", new Color32(0, 255, 40, 255));
                break;
            case 7:
                SetCurrentMonth("August", new Color32(0, 255, 40, 255));
                break;
            case 8:
                SetCurrentMonth("September", new Color32(255, 65, 30, 255));
                break;
            case 9:
                SetCurrentMonth("October", new Color32(255, 65, 30, 255));
                break;
            case 10:
                SetCurrentMonth("November", new Color32(255, 65, 30, 255));
                break;
            case 11:
                SetCurrentMonth("December", new Color32(0, 135, 255, 255));
                break;
        }
    }

    private void SetCurrentMonth(string month, Color color) {
        Text text = currentMonth.GetComponent<Text>();
        text.color = color;
        text.text = month;
    }

    public void ToggleMainMenu(bool show) {
        if (show) {
            SetCurrentProgress();
        }
        StartCoroutine(ScaleObject(show, currentYear));
        StartCoroutine(ScaleObject(show, currentMonth));
        StartCoroutine(ScaleObject(show, startgameButton));
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
        SetCurrentProgress();
    }

    public void Cheat() {
        GameManager.instance.level++;
        SaveSystem.SetInt("level", GameManager.instance.level);
        SetCurrentProgress();
    }


    // Util
    IEnumerator FadeImage(bool hide, GameObject uiObject) {
        Image img = uiObject.GetComponent<Image>();
        float r = img.color.r;
        float g = img.color.g;
        float b = img.color.b;
        if (!hide) {
            for (float i = 1; i >= 0; i -= Time.deltaTime) {
                img.color = new Color(r, g, b, i);
                yield return null;
            }
            uiObject.SetActive(false);
        } else {
            uiObject.SetActive(true);
            for (float i = 0; i <= 1; i += Time.deltaTime) {
                img.color = new Color(r, g, b, i);
                yield return null;
            }
        }
    }
    IEnumerator FadeText(bool hide, GameObject uiObject) {
        Text txt = uiObject.GetComponent<Text>();
        float r = txt.color.r;
        float g = txt.color.g;
        float b = txt.color.b;
        if (!hide) {
            for (float i = 1; i >= 0; i -= Time.deltaTime) {
                txt.color = new Color(r, g, b, i);
                yield return null;
            }
            uiObject.SetActive(false);
        } else {
            uiObject.SetActive(true);
            for (float i = 0; i <= 1; i += Time.deltaTime) {
                txt.color = new Color(r, g, b, i);
                yield return null;
            }
        }
    }

    IEnumerator ScaleObject(bool hide, GameObject uiObject) {
        Transform rect = uiObject.transform;
        if (!hide) {
            for (float i = 1f; i >= 0; i -= (Time.deltaTime * 2)) {
                rect.localScale = new Vector3(i, i, i);
                yield return null;
            }
            uiObject.SetActive(false);
        } else {
            uiObject.SetActive(true);
            for (float i = 0; i <= 1.3; i += (Time.deltaTime * 4)) {
                rect.localScale = new Vector3(i, i, i);
                yield return null;
            }
            for (float i = 1.3f; i >= 1; i -= (Time.deltaTime * 4)) {
                rect.localScale = new Vector3(i, i, i);
                yield return null;
            }
            rect.localScale = new Vector3(1, 1, 1);
        }
    }
}
