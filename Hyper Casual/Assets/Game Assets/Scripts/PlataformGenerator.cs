using UnityEngine;

public class PlatformGenerator : MonoBehaviour
{
    [SerializeField] private GameObject platform;
    [SerializeField] private GameObject fakePlatform;
    [SerializeField] private GameObject coin;
    [SerializeField] private Transform player;
    [SerializeField] private float generationThreshold = 20f;
    [SerializeField] private float cleanupDistance = 30f;
    [SerializeField] private float coinSpawnChance = 0.1f;
    [SerializeField] private float fakePlatformSpawnChance = 0.3f;

    private float lastSpawnZ;
    private float lastSpawnX;
    private Transform platformsParent;
    private float horizontalGridSize;
    private float verticalGridSize;

    void Start()
    {
        lastSpawnX = transform.position.x;
        lastSpawnZ = transform.position.z;

        horizontalGridSize = GameInfo.Instance.horizontalGridSize;
        verticalGridSize = GameInfo.Instance.verticalGridSize;

        platformsParent = new GameObject("PlatformsParent").transform;

        GeneratePlatforms();
    }

    void Update()
    {
        if (player.position.z + generationThreshold > lastSpawnZ)
        {
            GeneratePlatforms();
        }

        CleanupPlatforms();
    }

    public void GeneratePlatforms()
    {
        for (int i = 0; i < verticalGridSize; i++)
        {
            lastSpawnZ += verticalGridSize;

            int randomIndex;
            float xLane = lastSpawnX;

            if (lastSpawnX == 0)
            {
                randomIndex = Random.Range(0, 3);
                xLane = randomIndex switch
                {
                    0 => 0,
                    1 => -horizontalGridSize,
                    2 => horizontalGridSize,
                    _ => 0
                };
            }
            else if (lastSpawnX == -horizontalGridSize)
            {
                randomIndex = Random.Range(0, 2);
                xLane = randomIndex switch
                {
                    0 => -horizontalGridSize,
                    1 => 0,
                    _ => -horizontalGridSize
                };
            }
            else if (lastSpawnX == horizontalGridSize)
            {
                randomIndex = Random.Range(0, 2);
                xLane = randomIndex switch
                {
                    0 => horizontalGridSize,
                    1 => 0,
                    _ => horizontalGridSize
                };
            }

            GameObject platformToSpawn = platform;

            if (Random.value < fakePlatformSpawnChance)
            {
                Instantiate(fakePlatform, new Vector3(xLane + (Random.value < 0.5f ? -horizontalGridSize : horizontalGridSize), transform.position.y, lastSpawnZ), Quaternion.Euler(0, Random.Range(0f, 360f), 0), platformsParent);
            }

            GameObject instantiatedPlatform = Instantiate(platformToSpawn, new Vector3(xLane, transform.position.y, lastSpawnZ), Quaternion.Euler(0, Random.Range(0f, 360f), 0), platformsParent);

            lastSpawnX = xLane;

            if (Random.value < coinSpawnChance)
            {
                Instantiate(coin, new Vector3(xLane, transform.position.y + 8, lastSpawnZ), Quaternion.identity, platformsParent);
            }
        }
    }

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

