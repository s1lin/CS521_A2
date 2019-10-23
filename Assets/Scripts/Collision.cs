using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collision : MonoBehaviour {

    private float ghostRadius = 0.35f;

    private float stoneRadius = 1f;

    private CannonBall connonBall;
    private Vector3[] stoneHengesVertices;

    public GameObject outter;
    
    void Start() {
        connonBall = GetComponent<CannonBall>();
        stoneHengesVertices = GameObject.FindGameObjectWithTag("stone").GetComponent<StoneHengeVertices>().AllVerrtices();
        outter = GameObject.FindGameObjectWithTag("outter");
    }

    // Update is called once per frame
    void Update() {
        BallWithStones();
        BallWithGhosts();
    }

    private void BallWithStones() {

        Mesh outterMesh = outter.GetComponent<MeshFilter>().mesh;
        Vector3[] outterVertices = outterMesh.vertices; 

        float distance = Vector3.Distance(transform.position, stoneHengesVertices[0]);
        float min = Vector3.Distance(transform.position, stoneHengesVertices[0]);
        int minIndex = 0;
        if (transform.position.x <= 19f && transform.position.x >= 11f) {
            for (int i = 0; i < outterVertices.Length; i++) {
                distance = Vector3.Distance(connonBall.transform.position, transform.TransformPoint(outterVertices[i]));
                print(distance);
                if (distance < 1f) {                    
                    connonBall.Bounce(stoneHengesVertices[i], i);
                    return;
                }
            }
            //if (min < stoneRadius) {
            //    print(min);
            //    connonBall.Bounce(stoneHengesVertices[minIndex], minIndex);
            //}
        }

    }

    private void BallWithGhosts() {

        GameObject[] ghosts = GameObject.FindGameObjectsWithTag("ghost");
        for (int index = 0; index < ghosts.Length; index++) {

            Ghost gCls = ghosts[index].GetComponent<Ghost>();
            List<GhostNode> vertices = gCls.vertices;
            if (vertices != null) {
                for (int i = 0; i < vertices.Count; i++) {
                    float distance = Vector3.Distance(transform.position, vertices[i].position);
                    if (distance <= Mathf.Abs(ghostRadius)) {
                        Vector3 vBall = new Vector3(connonBall.vX, -connonBall.vY, 0);
                        gCls.BallCollision(i, vBall);
                        Destroy(gameObject);
                        return;
                    }
                }
            }
        }
    }
}
