using UnityEngine;

public class PlatformGenerator : MonoBehaviour
{
    [SerializeField] private GameObject platform;
    [SerializeField] private GameObject fakePlatform;
    [SerializeField] private float verticalGrid;
    [SerializeField] private float horizontalGrid;
    [SerializeField] private Transform player;
    [SerializeField] private float generationThreshold = 20f;
    [SerializeField] private float cleanupDistance = 30f;

    private float lastSpawnZ;
    private float lastSpawnX;
    private Transform platformsParent;

    void Start()
    {
        lastSpawnX = transform.position.x; // Inicializa a última posição X
        lastSpawnZ = transform.position.z; // Inicializa a última posição Z

        horizontalGrid = GameInfo.Instance.horizontalGridSize;
        verticalGrid = GameInfo.Instance.verticalGridSize;

        platformsParent = new GameObject("PlatformsParent").transform; // Cria um container para as plataformas

        // Gera plataformas iniciais
        GeneratePlatforms();
    }

    void Update()
    {
        // Verifica se o jogador está próximo do limite de geração e cria novas plataformas
        if (player.position.z + generationThreshold > lastSpawnZ)
        {
            GeneratePlatforms();
        }

        // Remove plataformas antigas
        CleanupPlatforms();
    }

    /// <summary>
    /// Gera plataformas em um padrão dinâmico baseado na posição do último spawn.
    /// </summary>
    public void GeneratePlatforms()
    {
        for (int i = 0; i < verticalGrid; i++)
        {
            lastSpawnZ += horizontalGrid; // Incrementa a posição Z

            int randomIndex;
            float xLane = lastSpawnX; // Começa com a última posição X

            if (lastSpawnX == 0)
            {
                randomIndex = Random.Range(0, 3); // Aleatório entre 0, -10, e 10
                xLane = randomIndex switch
                {
                    0 => 0,
                    1 => -10,
                    2 => 10,
                    _ => 0
                };
            }
            else if (lastSpawnX == -10)
            {
                randomIndex = Random.Range(0, 2); // Aleatório entre -10 e 0
                xLane = randomIndex switch
                {
                    0 => -10,
                    1 => 0,
                    _ => -10
                };
            }
            else if (lastSpawnX == 10)
            {
                randomIndex = Random.Range(0, 2); // Aleatório entre 10 e 0
                xLane = randomIndex switch
                {
                    0 => 10,
                    1 => 0,
                    _ => 10
                };
            }

            // Escolhe o tipo de plataforma a ser instanciada
            int platformChosen = Random.Range(0, 2);
            GameObject platformToSpawn = platformChosen == 0 ? platform : fakePlatform;

            // Instancia a plataforma na posição calculada
            Instantiate(platformToSpawn, new Vector3(xLane, transform.position.y, lastSpawnZ), Quaternion.identity, platformsParent);

            // Atualiza a última posição X gerada
            lastSpawnX = xLane;
        }
    }

    /// <summary>
    /// Remove plataformas que estão muito distantes do jogador.
    /// </summary>
    private void CleanupPlatforms()
    {
        foreach (Transform child in platformsParent)
        {
            if (child.position.z < player.position.z - cleanupDistance)
            {
                Destroy(child.gameObject);
            }
        }
    }
}
