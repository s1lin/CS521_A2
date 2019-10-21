using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collision : MonoBehaviour {

    private float ghostRadius = 0.35f;
    private List<GameObject> ghosts;

    private float stoneRadius = 0.2f;
    private StoneHengeVertices stoneHengesVertices;

    private CannonBall connonBall;

    void Start() {
        connonBall = GetComponent<CannonBall>();
        ghosts = GameObject.FindGameObjectWithTag("game").GetComponent<GameManager>().ghostInstances;
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
                print(minIndex + " " + min);
                //Debug.Break();
            }
        }

    }

    private void BallWithGhosts() {

        for (int index = 0; index < ghosts.Count; index++) {

            Ghost gCls = ghosts[index].GetComponent<Ghost>();
            Transform[] points = gCls.GhostVertices();
            for (int i = 0; i < points.Length; i++) {
                if (Vector3.Distance(transform.position, points[i].position) <= Mathf.Abs(ghostRadius)) {
                    gCls.BallCollision(i);
                    Destroy(gameObject);
                    return;
                }
            }
        }
    }
}
