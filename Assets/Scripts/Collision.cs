using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collision : MonoBehaviour {

    private float ghostRadius = 0.3f;
    private float stoneRadius = 0.3f;

    private CannonBall connonBall;
    private StoneHengeVertices stoneHengesVertices; 

    void Start() {
        connonBall = GetComponent<CannonBall>();
        stoneHengesVertices = GameObject.FindGameObjectWithTag("stone").GetComponent<StoneHengeVertices>();
        stoneHengesVertices.AllVerrtices(); 
    }

    // Update is called once per frame
    void Update() {
        BallWithStones();
        BallWithGhosts();
    }

    private void BallWithStones() {

        float dTop = transform.position.y - stoneHengesVertices.topMost.y; 
        float dRight = transform.position.x - stoneHengesVertices.rightMost.x;

        if (transform.position.x > 18.8f && dRight < stoneRadius && transform.position.y < 8f) {
            connonBall.Bounce(false, false, true);
        }

        if (transform.position.x <= 18.8f && dTop < stoneRadius && transform.position.x >= 11f) {
            connonBall.Bounce(true, false, false);
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
