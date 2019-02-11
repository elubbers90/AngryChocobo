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
    public GameObject startGameButton;
    public GameObject upgradeButton;

    public GameObject gameOverPopup;
    public GameObject gameOverText;
    public GameObject gameOverButton1;
    public GameObject gameOverButton2;

    public GameObject pausePopup;

    public GameObject victoryPopup;
    public GameObject victoryText;
    public GameObject victoryButton;
    
    public GameObject currentCakes;
    public GameObject gameOverlay;

    public GameObject upgradeScreen;

    public void Awake() {
        ToggleGameOver(false);
        ToggleVictory(false);
        TogglePause(false);
        mainMenu.SetActive(false);
        ToggleGameOverlay(false);
        upgradeScreen.SetActive(false);
    }

    public IEnumerator ToggleSplashScreen(bool show) {
        StartCoroutine(FadeImage(show, splashScreen));
        StartCoroutine(FadeText(show, loadingText));
        mainMenu.SetActive(true);
        currentMonth.SetActive(false);
        currentYear.SetActive(false);
        startGameButton.SetActive(false);
        upgradeButton.SetActive(false);
        yield return new WaitForSeconds(1);
        ToggleMainMenu(true, true);
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

    public void TogglePause(bool show) {
        if (!GameManager.instance.paused) {
            StartCoroutine(ScaleObject(show, pausePopup, 1.5f));
        }
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

    public void ToggleMainMenu(bool show, bool visibleBackground) {
        if (show) {
            SetCurrentProgress();
        }
        StartCoroutine(ScaleObject(show, currentYear));
        StartCoroutine(ScaleObject(show, currentMonth));
        StartCoroutine(ScaleObject(show, startGameButton));
        StartCoroutine(ScaleObject(show, upgradeButton));
        mainMenu.SetActive(show || visibleBackground);
    }

    private IEnumerator ShowUpgradeScreen() {
        ToggleMainMenu(false, true);
        yield return new WaitForSeconds(0.5f);
        upgradeScreen.SetActive(true);
        foreach (Transform child in upgradeScreen.transform) {
            StartCoroutine(ScaleObject(true, child.gameObject));
        }
    }

    private IEnumerator HideUpgradeScreen() {
        foreach (Transform child in upgradeScreen.transform) {
            StartCoroutine(ScaleObject(false, child.gameObject));
        }
        yield return new WaitForSeconds(0.5f);
        upgradeScreen.SetActive(false);
        ToggleMainMenu(true, true);
    }

    public void ToggleGameOverlay(bool show) {
        if (show) {
            SetCurrentCakesText();
        }
        gameOverlay.SetActive(show);
    }

    public void SetCurrentCakesText() {
        currentCakes.GetComponent<Text>().text = "x " + GameManager.instance.lives;
    }


    // click handlers
    public void StartMission() {
        ToggleMainMenu(false, false);
        ToggleGameOver(false);
        ToggleVictory(false);
        GameManager.instance.InitGame();
    }

    public void ToMainMenu() {
        ToggleMainMenu(true, true);
        ToggleGameOver(false);
        ToggleVictory(false);
    }

    public void ToUpgradeScreen() {
        StartCoroutine(ShowUpgradeScreen());
    }

    public void CloseUpgradeScreen() {
        StartCoroutine(HideUpgradeScreen());
    }

    public void ResetGame() {
        GameManager.instance.level = 1;
        SaveSystem.SetInt("level", 1);
        GameManager.instance.purchasedEggs = new List<int>();
        GameManager.instance.purchasedEggs.Add(0);
        SaveSystem.SetString("purchasedEggs", "0");
        SetCurrentProgress();
    }

    public void Cheat() {
        GameManager.instance.level++;
        SaveSystem.SetInt("level", GameManager.instance.level);
        SetCurrentProgress();
    }

    public void PauseGame() {
        Time.timeScale = 0;
        TogglePause(true);
        GameManager.instance.paused = true;
    }

    private void ContinueGame() {
        GameManager.instance.paused = false;
        TogglePause(false);
        Time.timeScale = 1;
    }

    public void ResumeGame() {
        ContinueGame();
    }

    public void ExitGame() {
        ContinueGame();
        GameManager.instance.ClearLevel();
        ToMainMenu();
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

    IEnumerator ScaleObject(bool hide, GameObject uiObject, float speedUp = 1f, float largerSize = 1.3f) {
        Transform rect = uiObject.transform;
        float lastTime = Time.realtimeSinceStartup;
        if (!hide) {
            for (float i = 1f; i >= 0; i -= ((Time.realtimeSinceStartup - lastTime)  * 2 * speedUp)) {
                lastTime = Time.realtimeSinceStartup;
                rect.localScale = new Vector3(i, i, i);
                yield return null;
            }
            uiObject.SetActive(false);
        } else {
            uiObject.SetActive(true);
            for (float i = 0; i <= largerSize; i += ((Time.realtimeSinceStartup - lastTime) * 4 * speedUp)) {
                lastTime = Time.realtimeSinceStartup;
                rect.localScale = new Vector3(i, i, i);
                yield return null;
            }
            if (largerSize != 1f) {
                for (float i = largerSize; i >= 1; i -= ((Time.realtimeSinceStartup - lastTime) * 4 * speedUp)) {
                    lastTime = Time.realtimeSinceStartup;
                    rect.localScale = new Vector3(i, i, i);
                    yield return null;
                }
            }
            rect.localScale = new Vector3(1, 1, 1);
        }
    }
}
