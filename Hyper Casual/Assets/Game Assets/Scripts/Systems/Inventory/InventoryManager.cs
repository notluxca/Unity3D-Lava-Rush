using UnityEngine;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public List<Character> ownedCharacters; // lista não é alimentada 
    public TMP_Text itemHolderName;
    public TextMeshProUGUI buttonText;
    public Image image;
    public CharacterChanger characterChanger;

    private int currentEquipedCharIndex = 1;

    private int currentCharacterIndex = 0;

    // Referência ao GameObject armazenado (model ativo na UI)
    private GameObject currentModelInstance;

    private void OnEnable()
    {
        // UpdateCharacterUI();
    }

    public void Start()
    {
        PlayerEvents.OnCharacterLoaded += Initialize;
    }

    public void Initialize()
    {
        if (ownedCharacters.Count > 0)
        {
            UpdateCharacterUI();
        }
        characterChanger = FindFirstObjectByType<CharacterChanger>();
    }

    public void NextCharacterButton()
    {
        if (ownedCharacters.Count == 0) return;

        // Avança para o próximo personagem (cíclico)
        currentCharacterIndex = (currentCharacterIndex + 1) % ownedCharacters.Count;
        UpdateCharacterUI();
    }

    public void LastCharacterButton()
    {
        if (ownedCharacters.Count == 0) return;

        // Avança para o próximo personagem (cíclico)
        currentCharacterIndex = (currentCharacterIndex - 1) % ownedCharacters.Count;
        UpdateCharacterUI();
    }

    private void UpdateCharacterUI()
    {
        // Destroi o modelo anterior se existir
        if (currentModelInstance != null)
        {
            Destroy(currentModelInstance);
        }

        // Obtém o personagem atual
        Character currentCharacter = ownedCharacters[currentCharacterIndex];

        // Atualiza os elementos da UI
        itemHolderName.text = currentCharacter.name;
        image.sprite = currentCharacter.modelImage;

        if (currentCharacterIndex == currentEquipedCharIndex) buttonText.SetText("EQUIPADO");
        else buttonText.SetText("EQUIPAR");
        // Instancia o modelo associado (caso necessário, ajuste a posição e rotação)
        // currentModelInstance = Instantiate(currentCharacter.model);
        // currentModelInstance.transform.SetParent(image.transform, false);
    }

    public void EquipCharacter()
    {
        characterChanger.SetCharacter(currentCharacterIndex);
        currentEquipedCharIndex = currentCharacterIndex;

        buttonText.SetText("EQUIPADO");
    }
}