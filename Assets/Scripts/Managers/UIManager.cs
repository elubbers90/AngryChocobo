using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {
    [HideInInspector]
    public UpgradeManager upgradeManager;

    public GameObject splashScreen;
    public GameObject loadingText;

    public GameObject mainMenu;
    public GameObject currentMonth;
    public GameObject currentYear;
    public GameObject startGameButton;
    public GameObject upgradeButton;
    public GameObject selectPlayerButton;
    public List<GameObject> selectPlayers;

    public GameObject cakes;
    public GameObject candy;
    public GameObject totalCakes;
    public GameObject totalCandy;

    public GameObject gameOverPopup;
    public GameObject gameOverText;
    public GameObject gameOverButton1;
    public GameObject gameOverButton2;
    public GameObject gameOverCandy;
    public GameObject gameOverCandyText;
    public GameObject gameOverTip;

    public GameObject pausePopup;

    public GameObject victoryPopup;
    public GameObject victoryText;
    public GameObject victoryButton;
    public GameObject victoryCakes;
    public GameObject victoryCandy;
    public GameObject victoryCakesText;
    public GameObject victoryCandyText;
    public GameObject victoryTip;

    public GameObject currentCakes;
    public GameObject gameOverlay;

    public GameObject upgradeScreen;

    public GameObject chickenScreen;

    public List<string> tips;

    public void Awake() {
        ToggleGameOver(false);
        ToggleVictory(false);
        TogglePause(false);
        mainMenu.SetActive(false);
        ToggleGameOverlay(false);
        upgradeScreen.SetActive(false);
        chickenScreen.SetActive(false);
        upgradeManager = GameObject.Find("UpgradeManager").GetComponent<UpgradeManager>();
    }

    public IEnumerator HideSplashScreen() {
        StartCoroutine(FadeImage(false, splashScreen));
        StartCoroutine(FadeText(false, loadingText));
        mainMenu.SetActive(true);
        currentMonth.SetActive(false);
        currentYear.SetActive(false);
        startGameButton.SetActive(false);
        upgradeButton.SetActive(false);
        selectPlayerButton.SetActive(false);
        yield return new WaitForSeconds(1);
        if (GameManager.instance.level == 0) {
            StartMission();
        } else {
            ToggleMainMenu(true, true, true);
        }
    }

    public void ToggleGameOver(bool show) {
        gameOverPopup.SetActive(show);
        if (show) {
            gameOverCandyText.GetComponent<Text>().text = "x " + GameManager.instance.collectedCandy;
            string tip = "";
            if (GameManager.instance.level == 0 || GameManager.instance.level == 1) {
                tip = tips[0];
            } else if (GameManager.instance.level == 2) {
                tip = tips[1];
            } else {
                tip = tips[Random.Range(0, tips.Count - 1)];
            }
            gameOverTip.GetComponent<Text>().text = tip;
        }
        StartCoroutine(ScaleObject(show, gameOverButton1));
        StartCoroutine(ScaleObject(show, gameOverButton2));
        StartCoroutine(ScaleObject(show, gameOverText));
        StartCoroutine(ScaleObject(show, gameOverCandy));
        StartCoroutine(ScaleObject(show, gameOverCandyText));
        StartCoroutine(FadeText(show, gameOverTip));
    }

    public void ToggleVictory(bool show) {
        victoryPopup.SetActive(show);
        if (show) {
            victoryCakesText.GetComponent<Text>().text = GameManager.instance.lives + " x";
            victoryCandyText.GetComponent<Text>().text = "x " + GameManager.instance.collectedCandy;
            string tip = "";
            if (GameManager.instance.level == 0 || GameManager.instance.level == 1) {
                tip = tips[0];
            } else if (GameManager.instance.level == 2) {
                tip = tips[1];
            } else {
                tip = tips[Random.Range(0, tips.Count - 1)];
            }
            victoryTip.GetComponent<Text>().text = tip;
        }
        StartCoroutine(ScaleObject(show, victoryText));
        StartCoroutine(ScaleObject(show, victoryButton));
        StartCoroutine(ScaleObject(show, victoryCandyText));
        StartCoroutine(ScaleObject(show, victoryCandy));
        StartCoroutine(ScaleObject(show, victoryCakes));
        StartCoroutine(ScaleObject(show, victoryCakesText));
        StartCoroutine(FadeText(show, victoryTip));
    }

    public void TogglePause(bool show) {
        if (!GameManager.instance.paused) {
            StartCoroutine(ScaleObject(show, pausePopup, 1.5f));
        }
    }

    public void SetCurrentCurrency() {
        totalCakes.GetComponent<Text>().text = "x " + GameManager.instance.totalCakes;
        totalCandy.GetComponent<Text>().text = "x " + GameManager.instance.totalCandy;
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

    public void ToggleMainMenu(bool show, bool visibleBackground, bool modifyCurrency) {
        if (show) {
            SetCurrentProgress();
            int i = 0;
            foreach (GameObject player in selectPlayers) {
                player.SetActive(i == GameManager.instance.currentSelectedPlayer);
                i++;
            }
        }
        if (modifyCurrency) {
            if (show) {
                SetCurrentCurrency();
            }
            StartCoroutine(ScaleObject(show, cakes));
            StartCoroutine(ScaleObject(show, candy));
            StartCoroutine(ScaleObject(show, totalCandy));
            StartCoroutine(ScaleObject(show, totalCakes));
        }
        StartCoroutine(ScaleObject(show, currentYear));
        StartCoroutine(ScaleObject(show, currentMonth));
        StartCoroutine(ScaleObject(show, startGameButton));
        StartCoroutine(ScaleObject(show, upgradeButton));

        StartCoroutine(ScaleObject(show, selectPlayerButton));
        
        mainMenu.SetActive(show || visibleBackground);
    }

    private IEnumerator ShowUpgradeScreen() {
        ToggleMainMenu(false, true, false);
        yield return new WaitForSeconds(0.5f);
        upgradeScreen.SetActive(true);
        foreach (Transform child in upgradeScreen.transform) {
            if (child.tag == "UpgradeTree") {
                child.gameObject.SetActive(false);
            } else {
                child.gameObject.SetActive(true);
                StartCoroutine(ScaleObject(true, child.gameObject));
            }
        }
        yield return new WaitForSeconds(0.5f);
        UpdateUpgradeScreenButtons();
        upgradeManager.OpenBasicEgg();
    }

    private IEnumerator HideUpgradeScreen() {
        upgradeManager.CloseCurrentEgg();
        foreach (Transform child in upgradeScreen.transform) {
            if (child.tag != "UpgradeTree") {
                StartCoroutine(ScaleObject(false, child.gameObject));
            }
        }
        yield return new WaitForSeconds(0.5f);
        upgradeScreen.SetActive(false);
        ToggleMainMenu(true, true, false);
        SaveSystem.SaveToDisk();
    }

    private IEnumerator ShowChickenScreen() {
        ToggleMainMenu(false, true, false);
        yield return new WaitForSeconds(0.5f);
        chickenScreen.SetActive(true);
        foreach (Transform child in chickenScreen.transform) {
            if (child.tag == "UpgradeTree") {
                child.gameObject.SetActive(false);
            } else {
                child.gameObject.SetActive(true);
                StartCoroutine(ScaleObject(true, child.gameObject));
            }
        }
        yield return new WaitForSeconds(0.5f);
        upgradeManager.OpenChickenScreen();
    }

    private IEnumerator HideChickenScreen() {
        upgradeManager.CloseChickenScreen();
        foreach (Transform child in upgradeScreen.transform) {
            if (child.tag != "UpgradeTree") {
                StartCoroutine(ScaleObject(false, child.gameObject));
            }
        }
        yield return new WaitForSeconds(0.5f);
        chickenScreen.SetActive(false);
        ToggleMainMenu(true, true, false);
        SaveSystem.SaveToDisk();
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

    public void UpdateUpgradeScreenButtons() {
        UpgradeButton[] buttons = upgradeScreen.GetComponentsInChildren<UpgradeButton>();
        foreach (UpgradeButton button in buttons) {
            button.SetState();
        }
    }

    public void UpdateChickenScreenButtons() {
        UpgradeButton[] buttons = chickenScreen.GetComponentsInChildren<UpgradeButton>();
        foreach (UpgradeButton button in buttons) {
            button.SetState();
        }
    }

    // click handlers
    public void StartMission() {
        ToggleMainMenu(false, false, true);
        ToggleGameOver(false);
        ToggleVictory(false);
        GameManager.instance.InitGame();
    }

    public void ToMainMenu() {
        ToggleMainMenu(true, true, true);
        ToggleGameOver(false);
        ToggleVictory(false);
    }

    public void ToUpgradeScreen() {
        StartCoroutine(ShowUpgradeScreen());
    }

    public void CloseUpgradeScreen() {
        StartCoroutine(HideUpgradeScreen());
    }

    public void ToChickenScreen() {
        StartCoroutine(ShowChickenScreen());
    }

    public void CloseChickenScreen() {
        StartCoroutine(HideChickenScreen());
    }

    public void ResetGame() {
        GameManager.instance.level = 0;
        GameManager.instance.totalCakes = 0;
        GameManager.instance.totalCandy = 0;
        GameManager.instance.purchasedEggs.Clear();
        GameManager.instance.purchasedEggs.Add(0);
        SaveSystem.ClearAll();
        SaveSystem.SaveToDisk();
        SetCurrentProgress();
        SetCurrentCurrency();
    }

    public void Cheat() {
        GameManager.instance.level++;
        SaveSystem.SetInt("level", GameManager.instance.level);
        GameManager.instance.totalCandy += 1000000;
        SaveSystem.SetInt("candy", GameManager.instance.totalCandy);
        GameManager.instance.totalCakes += 1000;
        SaveSystem.SetInt("cakes", GameManager.instance.totalCakes);
        SetCurrentProgress();
        SetCurrentCurrency();
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
