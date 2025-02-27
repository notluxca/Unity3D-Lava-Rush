using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [SerializeField] private UIRegistry uiRegistry;
    // [SerializeField] private GameUIs interactableMode = GameUIs.InitialUI;
    

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

        // Ativar nova UI
        if (uiScreens.ContainsKey(uiType))
        {
            uiScreens[uiType].SetActive(true);
            currentUI = uiType;
        }
    }

    public void OpenPopUp(UIPopUps popUpType)
    {
        if (popUps.ContainsKey(popUpType))
            popUps[popUpType].SetActive(true);
    }

    public void ClosePopUp(UIPopUps popUpType)
    {
        if (popUps.ContainsKey(popUpType))
            popUps[popUpType].SetActive(false);
    }
}
