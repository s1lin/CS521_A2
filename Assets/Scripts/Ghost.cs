using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour {

    private bool collision = false;
    private int numOfVertices = 15;
    private int iterations = 12;

    private Vector3 initV;

    private Transform ghostTransform;
    private Transform ghostBody;
    private Transform ghostHead;
    private List<GameObject> verticesGO;
    private List<GhostNode> vertices;
    private List<LineRenderer> ghostLines;

    public List<GameObject> VerticesGO { get; }
    public int RootIndex { get; set; }

    void Start() {
        initV = new Vector3(Mathf.Cos(Mathf.Deg2Rad) / Random.Range(1f, 2f),
            Mathf.Sin(Mathf.Deg2Rad) * Random.Range(20f, 30f), 0);

        ghostLines = new List<LineRenderer>();
        verticesGO = new List<GameObject>();
        vertices = new List<GhostNode>();

        ghostTransform = transform;
        ghostBody = ghostTransform.Find("Body");
        ghostHead = ghostTransform.Find("Head");

        BuildGhost();
        LineDraw(true);
    }

    void Update() {

        float dt = Mathf.Pow(Time.deltaTime, 4);

        //var mouse = Input.mousePosition;
        //var cam = Camera.main;
        //var world = cam.ScreenToWorldPoint(new Vector3(mouse.x, mouse.y, cam.nearClipPlane + 30f));
        //vertices[8].position = ghostTransform.position = world;

        //1.Move to the right without going into the stonehenges
        BasicMovement(dt);

        //2.Destroy out of Scene and Respawn
        if (IsOutOfScence())
            GameObject.FindGameObjectWithTag("game").GetComponent<GameManager>().RespawnGhost(RootIndex);

        ApplyConstraints();
        for (int i = 0; i < numOfVertices; i++) {
            verticesGO[i].transform.position = vertices[i].position;
        }
        LineDraw(false);

    }
    private bool IsOutOfScence() {
        for (int i = 0; i < 15; i++) {
            if (vertices[i].position.x > 35f || vertices[i].position.y > 23f)
                return true;
        }
        return false;
    }

    private void BasicMovement(float dt) {
        if (!collision) {
            vertices.ForEach(p => {
                p.position += dt * initV;
            });
        } else {
            ApplyConstraints();
        }
    }


    public void BallCollision(int vertexIndex) {
        float dt = Time.deltaTime;
        vertices[vertexIndex].position -= dt * initV;
        collision = true;
    }

    void BuildGhost() {
        Transform t = ghostHead.Find("0");
        for (int i = 0; i < numOfVertices; i++) {

            if (i != 5 && i != 7 && i != 9)
                t = ghostHead.Find(i.ToString());
            else if (i < 13)
                t = ghostBody.Find(i.ToString());

            if (i == 13)
                t = ghostHead.Find("left_eye");
            if (i == 14)
                t = ghostHead.Find("right_eye");

            vertices.Add(new GhostNode(t.position));
            verticesGO.Add(t.gameObject);
        };

        for (int i = 0; i < numOfVertices; i++) {
            var a = vertices[i];
            if (i < 12) {
                var b = vertices[i + 1];
                var e = new GhostEdge(a, b, false);
                a.Connect(e);
                b.Connect(e);
            }
            if (i == 10) {
                var b = vertices[8];
                var e = new GhostEdge(a, b, true);
                a.Connect(e);
                b.Connect(e);

                b = vertices[1];
                e = new GhostEdge(a, b, true);
                a.Connect(e);
                b.Connect(e);
            }
            if (i == 5 || i == 7 || i == 9) {
                var b = vertices[1];
                var e = new GhostEdge(a, b, true);
                a.Connect(e);
                b.Connect(e);
            }
            if (i == 4) {
                var b = vertices[0];
                var e = new GhostEdge(a, b, true);
                a.Connect(e);
                b.Connect(e);
            }
            if (i == 6) {
                var b = vertices[4];
                var e = new GhostEdge(a, b, true);
                a.Connect(e);
                b.Connect(e);

                b = vertices[8];
                e = new GhostEdge(a, b, true);
                a.Connect(e);
                b.Connect(e);
            }

            if (i == 12) {
                var b = vertices[0];
                var e = new GhostEdge(a, b, false);
                a.Connect(e);
                b.Connect(e);

                b = vertices[2];
                e = new GhostEdge(a, b, true);
                a.Connect(e);
                b.Connect(e);
            }
            if (i == 13) {
                var b = vertices[0];
                var e = new GhostEdge(a, b, true);
                a.Connect(e);
                b.Connect(e);

                b = vertices[12];
                e = new GhostEdge(a, b, true);
                a.Connect(e);
                b.Connect(e);

                b = vertices[14];
                e = new GhostEdge(a, b, true);
                a.Connect(e);
                b.Connect(e);
            }
            if (i == 14) {
                var b = vertices[1];
                var e = new GhostEdge(a, b, true);
                a.Connect(e);
                b.Connect(e);

                b = vertices[2];
                e = new GhostEdge(a, b, true);
                a.Connect(e);
                b.Connect(e);
            }
        }

    }

    private void LineDraw(bool init) {

        for (int i = 0; i < 13; i++) {
            if (i > 0) {
                if (init)
                    ghostLines.Add(verticesGO[i - 1].AddComponent<LineRenderer>());
                ghostLines[i - 1].startColor = Color.black;
                ghostLines[i - 1].startWidth = 0.05f;
                ghostLines[i - 1].endColor = Color.black;
                ghostLines[i - 1].endWidth = 0.05f;
                ghostLines[i - 1].positionCount = 2;
                ghostLines[i - 1].SetPosition(0, vertices[i - 1].position);
                ghostLines[i - 1].SetPosition(1, vertices[i].position);
                ghostLines[i - 1].material.color = Color.black;
                ghostLines[i - 1].numCapVertices = 1;
            }
        }

        //connect the 12th with the first one
        if (init)
            ghostLines.Add(verticesGO[12].gameObject.AddComponent<LineRenderer>());
        ghostLines[12].startColor = Color.black;
        ghostLines[12].startWidth = 0.05f;
        ghostLines[12].positionCount = 2;
        ghostLines[12].SetPosition(0, vertices[12].position);
        ghostLines[12].SetPosition(1, vertices[0].position);
        ghostLines[12].material.color = Color.black;
        ghostLines[12].numCapVertices = 1;


        for (int i = 13; i < 15; i++) {
            if (init)
                ghostLines.Add(verticesGO[i].gameObject.AddComponent<LineRenderer>());
            ghostLines[i].startColor = Color.black;
            ghostLines[i].startWidth = 0f;
            ghostLines[i].endColor = Color.black;
            ghostLines[i].endWidth = 0f;
            ghostLines[i].positionCount = 2;
            ghostLines[i].SetPosition(0, vertices[i].position);
            if (i == 13)
                ghostLines[i].SetPosition(1, vertices[0].position);
            else
                ghostLines[i].SetPosition(1, vertices[1].position);
            ghostLines[i].material.color = Color.black;
            ghostLines[i].numCapVertices = 1;
        }
    }

    private void ApplyConstraints() {

        for (int i = 0; i < vertices.Count; i++) {
            vertices[i].NextPosition();
        }

        for (int iter = 0; iter < iterations; iter++) {
            for (int i = 0; i < vertices.Count; i++) {
                GhostNode a = vertices[i];
                for (int j = 0; j < a.Connection.Count; j++) {
                    GhostEdge e = a.Connection[j];
                    GhostNode b = e.ConnectedTo(a);

                    Vector3 delta = a.position - b.position;

                    float distance = delta.magnitude;
                    float f = (distance - e.Length) / distance;

                    a.position -= f * 0.5f * delta;
                    b.position += f * 0.5f * delta;
                }
            }
        }
    }
}
