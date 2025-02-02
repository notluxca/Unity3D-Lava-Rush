using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public List<UIGroup> uiGroups;

    private void Start() {
        uiGroups = new List<UIGroup>();
        FindAllUIGroups();
        // MovementHandler.OnPlayerFirstMove += StartGameplayUi;
        for(int i = 0; i < uiGroups.Count; i++){
            if(!uiGroups[i].IsOpen) uiGroups[i].Close();
        }
    }

    private void FindAllUIGroups() {
        foreach (UIGroup uiGroup in GetComponentsInChildren<UIGroup>())
        {
            uiGroups.Add(uiGroup);
        }
    }


    public void Switch_UI_group_On(int groupIndex){
        Debug.Log("Switch called");
        for(int i = 0; i < uiGroups.Count; i++){
            if(i == groupIndex){
                uiGroups[i].Open();
            }else{
                uiGroups[i].Close();
            }
        }
    }

    public void StartGameplayUi(Vector3 newPosition){
        Switch_UI_group_On(1);
        PlayerEvents.OnPlayerFirstMove -= StartGameplayUi;
    }

    public void OpenUrl(string url){
        Application.OpenURL(url);
    }
}

