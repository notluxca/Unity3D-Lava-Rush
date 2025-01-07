using System.Collections.Generic;
using UnityEngine;

public class BackgroundRepeater : MonoBehaviour
{
    public Transform cameraTransform; // A câmera ou o player
    public List<GameObject> possibleChunks; // Lista de chunks possíveis para spawnar
    public int chunkPoolSize = 5; // Quantos chunks serão mantidos ativos
    public Vector3 spawnAxis = new Vector3(0, 0, 1); // Eixo de spawn (Z como exemplo)
    public float plataformHeight; // Altura fixa da plataforma

    private Queue<GameObject> chunkQueue = new Queue<GameObject>();
    private Vector3 nextSpawnPosition;
    [SerializeField] GameObject chunkPrefab;

    void Start()
    {
        // Inicializa a fila com os chunks
        for (int i = 0; i < chunkPoolSize; i++)
        {
            SpawnChunk();
        }
    }

    void Update()
    {
        // Checa se a câmera está próxima do último chunk
        if (cameraTransform.position.z + GetChunkSizeZ() > nextSpawnPosition.z - GetChunkSizeZ())
        {
            SpawnChunk();
        }

        // Remove chunks que estão muito atrás da câmera
        RemoveFarChunks();
    }

    void SpawnChunk()
    {
        // Escolhe um chunk aleatório da lista de possíveis chunks
        GameObject chosenChunk = possibleChunks[Random.Range(0, possibleChunks.Count)];

        // Ajusta a posição do próximo chunk
        Vector3 spawnPosition = new Vector3(nextSpawnPosition.x, plataformHeight, nextSpawnPosition.z);

        // Instancia o novo chunk
        GameObject newChunk = Instantiate(chosenChunk, spawnPosition, Quaternion.identity);
        chunkQueue.Enqueue(newChunk);

        // Atualiza a posição para o próximo chunk considerando o tamanho real do chunk no eixo Z
        nextSpawnPosition += spawnAxis * GetChunkSizeZ(newChunk);
    }

    void RemoveFarChunks()
    {
        // Verifica se há chunks para remover
        while (chunkQueue.Count > 0)
        {
            GameObject oldestChunk = chunkQueue.Peek(); // Pega o chunk mais antigo (primeiro da fila)
            
            // Checa se está muito atrás da câmera
            if (oldestChunk.transform.position.z + GetChunkSizeZ(oldestChunk) < cameraTransform.position.z - GetChunkSizeZ(oldestChunk))
            {
                // Remove o chunk
                Destroy(chunkQueue.Dequeue());
            }
            else
            {
                // Se o chunk mais antigo ainda está perto, não precisa remover mais
                break;
            }
        }
    }

    float GetChunkSizeZ(GameObject chunk)
    {
        // Obtém o tamanho do chunk no eixo Z
        if (chunk.TryGetComponent(out Renderer renderer))
        {
            return renderer.bounds.size.z;
        }
        else
        {
            return 10f; // Valor padrão caso não seja encontrado um Renderer
        }
    }

    float GetChunkSizeZ()
    {
        // Mantém compatibilidade com chamadas antigas, assume o chunkPrefab padrão
        return GetChunkSizeZ(chunkPrefab);
    }
}
