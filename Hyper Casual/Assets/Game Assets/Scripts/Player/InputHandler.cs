using UnityEngine;


public class InputHandler : MonoBehaviour
{
    private MovementHandler playerMovement; // fortemente acomplado com o MovementHandler

    //  [SerializeField] private bool handleKeyboardInput = true;

    private void Awake() {
        playerMovement = GetComponent<MovementHandler>();
    }

    public void HandleTapInput(){
        PlayerEvents.PlayerTap();
    }
    
    public void HandleSwipeInput(Vector2 swipeDirection){
        PlayerEvents.PlayerSwipe(swipeDirection);
    }

}
