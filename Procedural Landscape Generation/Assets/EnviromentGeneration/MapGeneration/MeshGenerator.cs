using UnityEngine;
using System.Collections;

public static class MeshGenerator {

    // heightMap is the perlin noise map, heightMultiplier scales the height values, heightCurve is used to 
    // adjust regions' topography eg make water/desert regions smooth 
    public static MeshData GenerateTerrainMesh(float[,] heightMap, float heightMultiplier, AnimationCurve heightCurve, int levelOfDetail)
    {
        int width = heightMap.GetLength(0);
        int height = heightMap.GetLength(1);
        float topLeftX = (width - 1) / -2f; // Making the mesh centered
        float topLeftZ = (width - 1) / -2f;

        int meshSimplificationIncrement;
        if (levelOfDetail == 0)
            meshSimplificationIncrement = 1;
        else
            meshSimplificationIncrement = levelOfDetail * 2;

        int verticesPerLine = (width - 1) / meshSimplificationIncrement + 1; // Can think of as width

        MeshData meshData = new MeshData(verticesPerLine, verticesPerLine); // Calculate correct number of vertices for the array
        int vertexIndex = 0;

        for (int y = 0; y < height; y+= meshSimplificationIncrement)
        {
            for (int x = 0; x < width; x += meshSimplificationIncrement)
            {
                meshData.vertices[vertexIndex] = new Vector3(topLeftX + x, heightCurve.Evaluate(heightMap[x, y]) * heightMultiplier, topLeftZ - y);
                meshData.uvs[vertexIndex] = new Vector2(x / (float)width, y / (float)height);
                // Create triangles
                if (x < width - 1 && y < height - 1)
                {   // See notes
                    meshData.AddTriangle(vertexIndex, vertexIndex + verticesPerLine + 1, vertexIndex + verticesPerLine);
                    meshData.AddTriangle(vertexIndex + width + 1, vertexIndex, vertexIndex + 1);
                }
                vertexIndex++;
            }
        }
        
        return meshData; // Returning this and not the mesh to allow threading
    }
	
}

public class MeshData
{
    public Vector3[] vertices;
    public int[] triangles;
    public Vector2[] uvs;
    
    private int triangleIndex;

    public MeshData(int meshWidth, int meshHeight)
    {
        this.vertices = new Vector3[meshWidth * meshHeight];
        this.uvs = new Vector2[meshWidth * meshHeight];
        this.triangles = new int[(meshWidth - 1) * (meshHeight - 1) * 6];
    }

    public void AddTriangle(int a, int b, int c)
    {
        triangles[triangleIndex] = a;
        triangles[triangleIndex + 1] = b;
        triangles[triangleIndex + 2] = c;
        triangleIndex += 3;
    }

    public Mesh CreateMesh()
    {
        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uvs;
        mesh.RecalculateNormals();
        return mesh;
    }
}
