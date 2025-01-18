using UnityEngine;

public class BackgroundRepeater : MonoBehaviour
{
    public Transform player;
    public float threshold = 10f;

    private Transform firstChild;
    private Transform secondChild;
    private Transform temp;

    private void Start()
    {
        firstChild = transform.GetChild(0);
        secondChild = transform.GetChild(1);
    }

    private void Update()
    {
        if (player.position.z + threshold > firstChild.position.z)
        {
            TeleportFirstChild();
        }
    }

    private void TeleportFirstChild()
    {
        var firstChildSize = firstChild.localScale.z;
        var secondChildSize = secondChild.localScale.z;
        var pos = firstChild.position;
        pos.z = secondChild.position.z + (secondChildSize + firstChildSize);
        firstChild.position = pos;
    }
}
