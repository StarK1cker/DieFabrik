using UnityEngine;

public class WorldGeneration : MonoBehaviour
{
    public GameObject orePrefab;
    public GameObject backgroundPrefab;


    public int numberOfOres;
    public int worldSize;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for(int i = -worldSize - 2; i < worldSize + 2; i++)
        {
            for(int j = -worldSize * 16/9 - 2; j < worldSize * 16/9 + 2; j++)
            {
                Instantiate(backgroundPrefab, new Vector3(j, i, -1), Quaternion.identity);
            }
        }

        for(int i=0; i < numberOfOres; i++)
        {
            Vector3 randomPosition = new Vector3(Random.Range(-worldSize*16/9 + 2, worldSize*16/9 - 1), Random.Range(-worldSize  + 2, worldSize - 1), 0);
            Instantiate(orePrefab, randomPosition, Quaternion.identity);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
