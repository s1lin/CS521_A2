using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collision : MonoBehaviour {

    private float ghostRadius = 0.35f;

    private float stoneRadius = 0.2f;
    private StoneHengeVertices stoneHengesVertices;

    private CannonBall connonBall;
    private GameManager game;

    void Start() {
        connonBall = GetComponent<CannonBall>();
        game = GameObject.FindGameObjectWithTag("game").GetComponent<GameManager>();
        stoneHengesVertices = GameObject.FindGameObjectWithTag("stone").GetComponent<StoneHengeVertices>();
    }

    // Update is called once per frame
    void Update() {
        BallWithStones();
        BallWithGhosts();
    }

    private void BallWithStones() {

        Vector3[] stoneHenges = stoneHengesVertices.AllVerrtices();
        float distance = Vector3.Distance(transform.position, stoneHenges[0]);
        float min = Vector3.Distance(transform.position, stoneHenges[0]);
        int minIndex = 0;
        if (transform.position.x <= 17f && transform.position.x >= 11f) {
            for (int i = 0; i < stoneHenges.Length; i++) {
                distance = Vector3.Distance(transform.position, stoneHenges[i]);
                if (distance < stoneRadius) {
                    if (min > distance) {
                        min = distance;
                        minIndex = i;
                    }

                }
            }
            if (min < stoneRadius) {
                connonBall.Bounce(stoneHenges[minIndex], minIndex);
            }
        }

    }

    private void BallWithGhosts() {

        GameObject[] ghosts = GameObject.FindGameObjectsWithTag("ghost");

        for (int index = 0; index < ghosts.Length; index++) {
            Ghost gCls = ghosts[index].GetComponent<Ghost>();
            List<GhostNode> vertices = gCls.Vertices;
            print(vertices[0].position);
            for (int i = 0; i < vertices.Count; i++) {
                print(vertices[i].position);
                if (Vector3.Distance(
                    transform.position,
                    transform.TransformPoint(vertices[i].position)) <= Mathf.Abs(ghostRadius)) {
                    gCls.BallCollision(i);
                    Destroy(gameObject);
                    return;
                }
            }
        }
    }
}
