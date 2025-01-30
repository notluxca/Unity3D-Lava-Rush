using UnityEngine;


public class InputHandler : MonoBehaviour
{
    private MovementHandler playerMovement;

    [SerializeField] private bool handleKeyboardInput = true;

    private void Awake() {
        playerMovement = GetComponent<MovementHandler>();
    }

    public void HandleInput()
    {
        if (handleKeyboardInput) HandleKeyboardInput();
    }

    public void HandleKeyboardInput(){
        if (Input.GetKeyDown(KeyCode.UpArrow))
            playerMovement.MoveFront();
        else if (Input.GetKeyDown(KeyCode.RightArrow))
            playerMovement.MoveDiagonalWithSwipe(Vector2.one);
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
            playerMovement.MoveDiagonalWithSwipe(new Vector2(-1, 1));
    }


    public void HandleTouchInput(){
        // Managing externally using LeanTouch
    }
}
