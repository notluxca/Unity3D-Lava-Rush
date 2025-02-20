using UnityEngine;

public class CollectableItem : MonoBehaviour, ICollectable
{

    public Vector3 rotVector;
    
    public virtual void OnCollect()
    {
        // Comportamento padrão ao coletar
        Debug.Log($"{gameObject.name} coletado!");
        
        // Exemplo: Desativar o objeto após a coleta ou colocar ele de volta em uma pool
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            OnCollect();
        }
    }

    private void FixedUpdate() {
        transform.Rotate(rotVector * Time.deltaTime * 20f);
    }
}
