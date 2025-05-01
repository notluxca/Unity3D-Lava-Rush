using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField] private UIRegistry uiRegistry;
    [SerializeField] private GameObject popUpsBackground;
    public TransitionScreen transitionScreen;
    private GameManager gameManager;

    private GameUIs currentUI;
    private Dictionary<GameUIs, GameObject> uiScreens;
    private Dictionary<UIPopUps, GameObject> popUps;

    private void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;

        gameManager = FindAnyObjectByType<GameManager>();

        uiScreens = new Dictionary<GameUIs, GameObject>();
        popUps = new Dictionary<UIPopUps, GameObject>();

        foreach (var ui in uiRegistry.uiScreens)
            uiScreens.Add(ui.type, ui.gameObject);

        foreach (var popup in uiRegistry.popUps)
            popUps.Add(popup.type, popup.gameObject);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        gameManager = FindAnyObjectByType<GameManager>();
    }

    public void RevivePlayerButton()
    {
        if (CoinManager.TrySpendGems(100))
        {
            gameManager.RevivePlayer();
            gameManager.hasRevived = true;
        }
    }

    public void OpenUI(GameUIs uiType)
    {
        popUpsBackground.SetActive(false);
        // Desativar UI atual
        if (uiScreens.ContainsKey(currentUI))
            uiScreens[currentUI].SetActive(false);

        // Desativar todos os popups
        foreach (var popup in popUps.Values)
            popup.SetActive(false);

        // Ativar nova UI
        if (uiScreens.ContainsKey(uiType))
        {
            uiScreens[uiType].SetActive(true);
            currentUI = uiType;
        }
    }

    public void OpenPopUp(UIPopUps popUpType)
    {
        popUpsBackground.SetActive(true);
        // Desativar todos os popups
        foreach (var popup in popUps.Values)
            popup.SetActive(false);

        if (popUps.ContainsKey(popUpType))
            popUps[popUpType].SetActive(true);
    }

    public void ClosePopUp(UIPopUps popUpType)
    {
        popUpsBackground.SetActive(false);
        if (popUps.ContainsKey(popUpType))
            popUps[popUpType].SetActive(false);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void RestartGameButton()
    {
        Debug.Log("blablala");
        transitionScreen.FadeIn();
        Invoke(nameof(RestartGame), 1.5f);
        StartCoroutine(WaitToFadeOut());
    }

    private IEnumerator WaitToFadeOut()
    {
        yield return new WaitForSeconds(1.5f);
        transitionScreen.FadeOut();
    }
}
