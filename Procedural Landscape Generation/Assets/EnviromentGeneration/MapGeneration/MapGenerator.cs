using UnityEngine;
using System.Collections;

public class MapGenerator : MonoBehaviour {

    public float noiseScale;
    public int octaves;
    [Range(0, 1)]
    public float persistance;
    public float lacunarity;
    public int seed;
    public Vector2 offset;
    public bool autoUpdate;
    public TerrainType[] regions;
    [Range(0,6)]
    public int levelOfDetail; // Higher = more mesh simplification

    public enum DrawMode { NoiseMap, ColourMap, MeshMap };
    public DrawMode drawMode;

    public float meshHeightMultiplier;
    public AnimationCurve meshHeightCurve;

    // To do with number of vertices in mesh, 241 - 1 has nice even factors
    private const int mapChunkSize = 241; 

    public void GenerateMap()
    {
        //float[,] noiseMap = Noise.GenerateNoiseMap(mapWidth, mapHeight, noiseScale);
        float[,] noiseMap = Noise.GenerateNoiseMap(mapChunkSize, mapChunkSize, noiseScale, octaves, persistance, lacunarity);
        Color[] colourMap = new Color[mapChunkSize * mapChunkSize];
        
        // Perlin "hieght" value corrosponds to land type stuff
        for (int y = 0; y < mapChunkSize; y++)
        {
            for (int x = 0; x < mapChunkSize; x++)
            {
                //print(regions);
                float currentHeight = noiseMap[x, y];
                for (int i = 0; i < regions.Length; i++)
                {
                    if (currentHeight <= regions[i].height)
                    {
                        colourMap[y * mapChunkSize + x] = regions[i].colour;
                        break;
                    }
                }
            }
        }

        MapDisplay display = FindObjectOfType<MapDisplay>();
        if (drawMode == DrawMode.NoiseMap) 
        {
            display.DrawTexture(TextureGenerator.TextureFromHeightMap(noiseMap));
        }          
        else if (drawMode == DrawMode.ColourMap) 
        {
            display.DrawTexture(TextureGenerator.TextureFromColourMap(colourMap, mapChunkSize, mapChunkSize));
        }            
        else if (drawMode == DrawMode.MeshMap)
        {
            display.DrawMesh(MeshGenerator.GenerateTerrainMesh(noiseMap, meshHeightMultiplier, meshHeightCurve, levelOfDetail),
                TextureGenerator.TextureFromColourMap(colourMap, mapChunkSize, mapChunkSize));
        }
    }

    void OnValidate()
    {   // Stop map dimensions being -ve
        /*if (mapChunkSize < 1)
            mapChunkSize = 1;
        
        if (mapChunkSize < 1)       
            mapChunkSize = 1;*/
       
        if (lacunarity < 1)     
            lacunarity = 1;
        // Octaves can not go below zero
        if (octaves < 0)     
            octaves = 0;
    }

    [System.Serializable] // Make sure it shows up in the inspector
    public struct TerrainType
    {
        public string name; // Name of region
        public float height; // Segement of Perlin range belonging to region
        public Color colour; // Colour of region
    }


}
