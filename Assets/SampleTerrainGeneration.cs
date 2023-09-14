using UnityEngine;

public class SampleTerrainGeneration : MonoBehaviour
{
    public int width = 10000; //x-axis of the terrain
    public int height = 10000; //z-axis

    public int depth = 874; //y-axis

    public float scale = 20f;

    public float offsetX = 100f;
    public float offsetY = 100f;

    private void Start()
    {
        offsetX = Random.Range(0f, 9999f);
        offsetY = Random.Range(0f, 9999f);
    }

    private void Update()
    {
        Terrain terrain = GetComponent<Terrain>();
//        terrain.terrainData = GenerateTerrain(terrain.terrainData);
    }

    TerrainData GenerateTerrain (TerrainData terrainData)
    {
        terrainData.heightmapResolution = 1025;
        terrainData.size = new Vector3(width, depth, height);

        CSVHandler csvHandler = new CSVHandler("everestElevations", 257);
        ElevationResult[,] elevations = csvHandler.ReadCSV();

        
        terrainData.SetHeights(0, 0, GenerateHeights());
        return terrainData;
    }

    float[,] GenerateHeights()
    {
        float[,] heights = new float[1025, 1025];
        for(int x = 0; x < 1025; x++)
        {
            for (int y = 0; y < 1025; y++)
            {
                heights[x, y] = CalculateHeight(x, y);
            }
        }

        return heights;
    }

    private float[,] calculateHeigths(ElevationResult[,] elevations) {
        float[,] newElevations = new float[257, 257];
        for (int i = 0; i < 257; i++) {
            for (int j = 0; j < 257; j++) {
                if (elevations[i,j] != null) {
                    newElevations[i, j] = getScaleHeigth(elevations[i,j]);
                }
            }
        }
        return newElevations;
    }

    private float getScaleHeigth(ElevationResult elevationResult) {
        float result = (float) (elevationResult.elevation /depth);
        Debug.Log(result);
        return result;
    }

    float CalculateHeight (int x, int y)
    {
        float xCoord = (float)x / width * scale + offsetX;
        float yCoord = (float)y / height * scale + offsetY;

        return Mathf.PerlinNoise(xCoord, yCoord);
    }
}