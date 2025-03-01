using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [SerializeField] private UIRegistry uiRegistry;
    // [SerializeField] private GameUIs interactableMode = GameUIs.InitialUI;
    public TransitionScreen transitionScreen;
    

    private GameUIs currentUI;
    private Dictionary<GameUIs, GameObject> uiScreens;
    private Dictionary<UIPopUps, GameObject> popUps;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
            
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        uiScreens = new Dictionary<GameUIs, GameObject>();
        popUps = new Dictionary<UIPopUps, GameObject>();

        foreach (var ui in uiRegistry.uiScreens)
            uiScreens.Add(ui.type, ui.gameObject);

        foreach (var popup in uiRegistry.popUps)
            popUps.Add(popup.type, popup.gameObject);
    }


    public void OpenUI(GameUIs uiType)
    {
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
        // Desativar todos os popups
        foreach (var popup in popUps.Values)
            popup.SetActive(false);

        if (popUps.ContainsKey(popUpType))
            popUps[popUpType].SetActive(true);
    }

    public void ClosePopUp(UIPopUps popUpType)
    {
        if (popUps.ContainsKey(popUpType))
            popUps[popUpType].SetActive(false);
    }

    public void RestartGame(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); }

        public void RestartGameButton(){
        // call gamemanager
        transitionScreen.FadeIn();
        Invoke("RestartGame", 1.5f);
        StartCoroutine(WaitToFadeOut());
    }

    public IEnumerator WaitToFadeOut(){
        yield return new WaitForSeconds(1.5f);
        transitionScreen.FadeOut();
    }


}

