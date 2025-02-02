using UnityEngine;

public class UIGroup : MonoBehaviour
{
    public bool IsOpen = false;

    public void Open(){
        IsOpen = true;
        // play open animation
        gameObject.SetActive(true);
    }

    public void Close(){

        //if(!IsOpen) return;
        IsOpen = false;
        gameObject.SetActive(false);
    }
}
