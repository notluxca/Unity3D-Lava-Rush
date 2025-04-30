using System;
using UnityEditor;
using UnityEngine;

public class CharacterChanger : MonoBehaviour
{
    private Character[] characters; // lista deve ser alimentada pelo inventory 
    private int currentIndex = 0;
    public GameObject modelFather;
    public Vector3 originalScale;
    public Vector3 originalposition;
    [SerializeField] Animator an;

    private InventoryManager inventoryManager;

    private void Start()
    {
        inventoryManager = FindAnyObjectByType<InventoryManager>();
        characters = inventoryManager.ownedCharacters.ToArray();
        Initialize();
        // SetCharacter(1); // force character to be Lava Croft at first
        // if (characters.Length > 0)
        // {
        //     SetCharacter(0);
        // }

        // Destroy(modelFather.transform.GetChild(0).gameObject);

    }

    public void Initialize()
    {
        originalScale = modelFather.transform.GetChild(0).transform.localScale;
        originalposition = modelFather.transform.GetChild(0).transform.localPosition;

        // Debug.Log($"{originalScale} {originalposition}");        
    }

    private void OnEnable()
    {
        PlayerEvents.CharacterLoaded();
    }

    [ContextMenu("Change Character")]
    public void ChangeCharacter()
    {
        if (characters.Length == 0) return;

        // Remover o modelo atual se existir
        if (modelFather.transform.GetChild(0) != null)
        {
            Destroy(modelFather.transform.GetChild(0).gameObject);
        }

        // Avançar no índice e voltar ao início se necessário
        currentIndex = (currentIndex + 1) % characters.Length;

        // Instanciar o novo modelo como filho do GameObject principal
        SetCharacter(currentIndex);
    }

    public void SetCharacter(int index)
    {
        Debug.Log("Setando e instanciando new character de index: " + index);
        if (characters.Length == 0 || characters[index].model == null) return;
        // Remover o modelo atual se existir
        if (modelFather.transform.GetChild(0) != null)
        {
            Destroy(modelFather.transform.GetChild(0).gameObject);
        }
        // Criar novo modelo como filho deste GameObject
        GameObject newCharacter = Instantiate(characters[index].model, modelFather.transform);
        newCharacter.transform.localScale = Vector3.one;
        newCharacter.transform.localPosition = Vector3.zero;
        newCharacter.gameObject.name = "Rig";
        PlayerEvents.CharacterModelChanged();
        an.Rebind();
        an.Update(0f);
        // newCharacter.transform = baseTransform;

    }
}


