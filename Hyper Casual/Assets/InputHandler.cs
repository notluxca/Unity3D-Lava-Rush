using UnityEngine;


public class InputHandler : MonoBehaviour
{
    private MovementHandler playerMovement;

    private bool handleKeyboardInput = false;

    private void Awake() {
        playerMovement = GetComponent<MovementHandler>();
    }

    private void Update() {
        HandleInput();
    }

    public void HandleInput()
    {
        if (handleKeyboardInput) HandleKeyboardInput();
    }

    public void HandleKeyboardInput(){
        if (Input.GetKeyDown(KeyCode.UpArrow))
            playerMovement.MoveFront();
        else if (Input.GetKeyDown(KeyCode.RightArrow))
            playerMovement.MoveDiagonal(Vector2.one);
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
            playerMovement.MoveDiagonal(new Vector2(-1, 1));
    }


    public void HandleTouchInput(){
        // Managing externally using LeanTouch
    }
}
