using System.Runtime.CompilerServices;
using UnityEngine;

public class GemCollectable : CollectableItem
{

    [SerializeField] private GameObject billboardPrefab; //! Provisório
    public override void OnCollect()
    {
        base.OnCollect();
        GameEvents.GemCollected(1);
        Instantiate(billboardPrefab, transform.position, Quaternion.identity);
    }

    private void Update()
    {

    }
}

