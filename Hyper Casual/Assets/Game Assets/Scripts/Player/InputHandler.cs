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


    // public void HandleInput()
    // {
    //     if (handleKeyboardInput) HandleKeyboardInput();
    // }



    // public void HandleUpInput(){}
    
    // public void HandleTapInput(){}

    // public void HandleKeyboardInput(){
    //     if (Input.GetKeyDown(KeyCode.UpArrow))
    //         playerMovement.MoveFront();
    //     else if (Input.GetKeyDown(KeyCode.RightArrow))
    //         playerMovement.MoveDiagonalWithSwipe(Vector2.one);
    //     else if (Input.GetKeyDown(KeyCode.LeftArrow))
    //         playerMovement.MoveDiagonalWithSwipe(new Vector2(-1, 1));
    // }


    // public void HandleTouchInput(){
    //     // Managing externally using LeanTouch
    // }
}
