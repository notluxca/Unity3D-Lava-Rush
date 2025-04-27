using UnityEngine;
using TMPro;

public class TouchIndicatorController : MonoBehaviour
{

    private TextMeshProUGUI text;
    private Animator animator;
    public LayerMask plataformLayer;


    void Start()
    {
        text = GetComponentInChildren<TextMeshProUGUI>();
        animator = GetComponentInChildren<Animator>();
        CheckPlataformPosition();
    }



    private void CheckPlataformPosition()
    {
        Vector3 position = Vector3.zero; // player always start on ( x: 0 | y: Player height | z: 0 )
        position.z += GameInfo.verticalGridSize; // move up on grid position
        Vector3 firstRayPos = new Vector3(-GameInfo.verticalGridSize, 5, position.z);

        for (int i = 0; i <= 2; i++)
        {
            Debug.DrawRay(firstRayPos, Vector3.down * 10, Color.blue, 50f);
            if (Physics.Raycast(firstRayPos, Vector3.down, out RaycastHit hit, 50f, plataformLayer))
            {
                if (hit.collider.CompareTag("SafePlataform"))
                {
                    // Debug.Log("Achou plataforma");
                    hit.collider.gameObject.GetComponent<Platform>().SetFallTime(10);
                    Debug.Log(hit.point.x);
                    Debug.Log(hit.point.x == -1.5f);
                    if (hit.point.x == -1.5f) //! 1.5f retorna plataforma do meio, devia ser 0
                    {
                        animator.SetTrigger("Touch");
                        text.SetText("TOQUE PARA INICIAR");
                    }
                    else
                    {
                        animator.SetTrigger("Slide");
                        text.SetText("DESLIZE PARA INICIAR");
                    }

                    return;
                }
            }
            firstRayPos.x += GameInfo.horizontalGridSize;
        }
    }


    private void SetTouchIndicator()
    {

    }
}
