using System.Collections.Generic;
using UnityEngine;

public class BackgroundRepeater : MonoBehaviour
{
    public float threshold = 10f; // Pode ser ajustado, mas parece que não está sendo usado
    public Transform player;     // Transform do jogador
    public List<GameObject> children = new List<GameObject>(); // Lista das tiles
    private GameObject middleTile; // Referência para a tile do meio
    private int currentIndex = 0; // Index da tile que será teleportada
    private int currentTilePosition = 3;
    private float tileWidth;      // Largura de uma tile no eixo X

    private void Start()
    {
        // player = GameObject.Find("Player").transform;

        // Adiciona todas as tiles (childs do parent)
        foreach (Transform child in transform)
        {
            children.Add(child.gameObject);
        }

        // Calcula a largura da tile com base no tamanho do collider ou na escala
        if (children.Count > 0)
        {
            tileWidth = children[0].GetComponent<Renderer>().bounds.size.x;
        }

        // Determina a tile do meio
        middleTile = GetMiddleTilePosition(children[0], children[1], children[2]);
    }

    private void Update()
    {
        DetectPlayerPosition();
    }

    private void DetectPlayerPosition()
    {
        // Checa se o jogador passou do centro da tile do meio no eixo Z
        if (player.position.z > middleTile.transform.position.z)
        {
            TeleportChild();
        }
    }

    private void TeleportChild()
    {
        children[currentIndex].transform.localPosition = new Vector3(currentTilePosition, 0, 0);
            currentIndex++;
            currentTilePosition++;
            if(currentIndex > 2){
                currentIndex = 0;
            }
            middleTile = GetMiddleTilePosition(children[0], children[1], children[2]);
    }

    public GameObject GetMiddleTilePosition(GameObject first, GameObject second, GameObject third)
    {
        var list = new List<GameObject> { first, second, third };
        list.Sort((a, b) => a.transform.position.x.CompareTo(b.transform.position.x));
        return list[1];
    }
}
