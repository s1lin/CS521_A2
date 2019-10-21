using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneHenge : MonoBehaviour {

    private Mesh stoneMeshes;
    private Vector3[] vertices;
    public Vector3[] wolrdVertices;

    void Start() {

        stoneMeshes = GetComponent<MeshFilter>().mesh;
        vertices = stoneMeshes.vertices;

        StoneHengeGenerator(vertices);

        stoneMeshes.vertices = vertices;
        stoneMeshes.RecalculateBounds();
        stoneMeshes.RecalculateNormals();

        SaveWorldLocation();
    }

    void SaveWorldLocation() {
        wolrdVertices = new Vector3[vertices.Length];
        for (int i = 0; i < vertices.Length; i++) {
            Vector3 worldPt = transform.TransformPoint(stoneMeshes.vertices[i]);
            wolrdVertices[i] = worldPt;
        }
    }

    private float Interp(float t, float a, float b) {
        return (3 * Mathf.Pow(t, 2) + 2 * Mathf.Pow(t, 3)) * (b - a) + a;
    }

    void StoneHengeGenerator(Vector3[] vertices) {
        float maxOffset = Mathf.Abs(vertices[30].z - Mathf.Abs(vertices[1].z));
        for (int i = 0; i < vertices.Length; i++) {
            float offset =  Random.Range(-maxOffset, maxOffset);
            float noise = PerlinNoise(vertices[i].z);
            vertices[i].z += offset * noise;
        }
    }


    float PerlinNoise(float x) {
        int chunkSize = 16;
        int range = 3;
        float noise = 0;
        
        while (chunkSize > 0) {
            int chunkIndex = (int) x / chunkSize;

            float prog = (x % chunkSize) / (chunkSize * 1f);

            float left_random = Random2(chunkIndex, range);
            float right_random = Random2(chunkIndex + 1, range);


            noise += (1 - prog) * left_random + prog * right_random;

            chunkSize /= 2;
            range /= 2;

            range = Mathf.Max(1, range);
        }

        return (int)Mathf.Round(noise);

    }

    private int Random2(long x, int range) {
        return (int)(((x + 1376312589L) ^ 5) % range);
    }

}
