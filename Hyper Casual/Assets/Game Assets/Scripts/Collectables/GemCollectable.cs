using UnityEngine;

public class GemCollectable : CollectableItem
{
    public static event System.Action gemCollected;

    public override void OnCollect()
    {
        base.OnCollect();
        gemCollected?.Invoke();
    }

    private void Update() {
        
    }
}

