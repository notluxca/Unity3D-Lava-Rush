using UnityEngine;

public class GemCollectable : CollectableItem
{
    public override void OnCollect()
    {
        base.OnCollect();
        GameEvents.GemCollected(1);
    }

    private void Update() {
        
    }
}

