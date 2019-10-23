using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collision : MonoBehaviour {

    private float ghostRadius = 0.35f;

    private float stoneRadius = 1f;

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
        GhostsWithStone();
    }

    private void BallWithStones() {

        float dTop = transform.position.y - stoneHengesVertices.topMost.y; 
        float dRight = transform.position.x - stoneHengesVertices.rightMost.x;

        if (transform.position.x > 18.8f && dRight < 0.3f && transform.position.y < 8f) {
            connonBall.Bounce(false, false, true);
        }

        if (transform.position.x <= 18.8f && dTop < 0.3f && transform.position.x >= 11f) {
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

    private void GhostsWithStone() {

        GameObject[] ghosts = GameObject.FindGameObjectsWithTag("ghost");
        for (int index = 0; index < ghosts.Length; index++) {

            Ghost gCls = ghosts[index].GetComponent<Ghost>();
            List<GhostNode> vertices = gCls.vertices;
            if (vertices != null) {
                for (int i = 0; i < vertices.Count; i++) {
                    Vector3 ghostP = vertices[i].position;

                    if (ghostP.x <= 19f && ghostP.x >= 11f && ghostP.y < 8f) {

                        float dTop = ghostP.y - stoneHengesVertices.topMost.y;
                        float dLeft = ghostP.x - stoneHengesVertices.leftMost.x;
                        float dRight = ghostP.x - stoneHengesVertices.rightMost.x;


                        //if (dLeft < 0.5f) {
                        //    gCls.StoneCollision(false, true, false);
                        //}
                        if (vertices[i].position.y > 8f && dTop < 0.5f) {
                            print("here");
                            gCls.StoneCollision(true, false, false);
                        }
                        //} else if(dRight < 1f) {
                        //     gCls.StoneCollision(false, false, true);
                        //}

                    }

                }
            }
        }
    }
}
