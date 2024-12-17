using UnityEngine;

public class PlataformGenerator : MonoBehaviour
{
    [SerializeField] GameObject plataform;
    [SerializeField] GameObject FakePlataform;
    [SerializeField] private int laneSizeY;
    [SerializeField] private float gridSize;

    private float lastSpawnX;

    void Start()
    {
        lastSpawnX = transform.position.x; // Initialize last spawn position
        GeneratePlataforms();
    }

    public void GeneratePlataforms()
    {
        float zPosition = transform.position.z;

        for (int i = 0; i < laneSizeY; i++)
        {
            zPosition += gridSize;

            int randomIndex;
            float xLane = lastSpawnX; // Start with the last spawned X position

            if (lastSpawnX == 0)
            {
                randomIndex = Random.Range(0, 3); // Randomize between 0, -10, and 10
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
                randomIndex = Random.Range(0, 2); // Randomize between -10 and 0
                xLane = randomIndex switch
                {
                    0 => -10,
                    1 => 0,
                    _ => -10
                };
            }
            else if (lastSpawnX == 10)
            {
                randomIndex = Random.Range(0, 2); // Randomize between 10 and 0
                xLane = randomIndex switch
                {
                    0 => 10,
                    1 => 0,
                    _ => 10
                };
            }

            // Instantiate the platform at the calculated position
            int plataformChosen = Random.Range(0,2);
            if(plataformChosen == 0){
                Instantiate(plataform, new Vector3(xLane, transform.position.y, zPosition), Quaternion.identity);
            } else {
                Instantiate(FakePlataform, new Vector3(xLane, transform.position.y, zPosition), Quaternion.identity);
            }

            // Update the last spawned X position
            lastSpawnX = xLane;
        }
    }
}
