using UnityEngine;

public class StoneHenge : MonoBehaviour {

    private Mesh stoneMeshes;
    private Vector3[] vertices;
    public Vector3[] wolrdVertices;

    void Start() {

        stoneMeshes = GetComponent<MeshFilter>().mesh;
        vertices = stoneMeshes.vertices;

        StoneHengeGenerator(vertices, 0, 15);

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
    
    void StoneHengeGenerator(Vector3[] vertices, int start, int end) {
        
        int mid = (end + start) / 2;

        if (start != mid && end != mid) {

            float maxOffset = Mathf.Abs(vertices[end].z - vertices[start].z);
            float offset = (float)Random.Range(-maxOffset, maxOffset);
            float noise = Mathf.PerlinNoise(vertices[end].z, 0.0f);
            //float noise = Noise(vertices[start].z);
            //print(noise);

            vertices[mid].z += noise;

            if (offset < 0) {
                for (int i = mid + 11; i < this.vertices.Length; i++) {
                    vertices[i].z += offset * noise;
                }
            }

            if (start < mid) {
                for (int i = mid - 1; i >= start; i--) {
                    vertices[i].z += noise;
                    if (offset < 0) {
                        for (int j = i; j < this.vertices.Length; j += 11) {
                            vertices[j].z += noise;
                        }
                    }
                }
                StoneHengeGenerator(vertices, start, mid);
            }
            if (mid < end) {
                for (int i = mid + 1; i <= end; i++) {
                    vertices[i].z += noise;
                    if (offset < 0) {
                        for (int j = i; j < this.vertices.Length; j += 11) {
                            vertices[j].z += noise;
                        }
                    }
                }
                StoneHengeGenerator(vertices, mid, end);
            }
        }
    }

}
