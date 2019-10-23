using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour {

    private bool collision = false;
    private int numOfVertices = 15;
    private int constraintIter = 12;
    private float dt = 1e-05f;

    private Vector3 initV;

    private Transform ghostTransform;
    private Transform ghostBody;
    private Transform ghostHead;

    public List<GhostNode> vertices;
    private List<GameObject> verticesGO;
    private List<LineRenderer> ghostLines;

    private Vector3[] stoneHengesVertices;

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

        //use for debug:
        //Vector3 mouse = Input.mousePosition;
        //Camera camera = Camera.main;
        //Vector3 world = camera.ScreenToWorldPoint(new Vector3(mouse.x, mouse.y, cam.nearClipPlane + 30f));
        //vertices[8].position = ghostTransform.position = world;

        //1.Move to the right without going into the stonehenges
        BasicMovement(dt);
        ApplyConstraints();
        UpdatePosition();

        LineDraw(false);

        //2.Destroy out of Scene and Respawn
        if (IsOutOfScence())
            GameObject.FindGameObjectWithTag("game").GetComponent<GameManager>().RespawnGhost(RootIndex);
      

    }

    private void UpdatePosition() {
        for (int i = 0; i < numOfVertices; i++) {
            verticesGO[i].transform.position = vertices[i].position;
        }
    }

    private bool IsOutOfScence() {
        for (int i = 0; i < 15; i++) {
            if (transform.position.x > 35f || transform.position.x < -3f ||
                    vertices[i].position.y > 24f || vertices[i].position.y < 1f) {
                Destroy(gameObject);
                return true;
            }
        }
        return false;
    }

    private void BasicMovement(float dt) {
        if (!collision) {
            for (int i = 0; i < vertices.Count; i++) {
                vertices[i].position += dt * initV;
            }
        } 
    }

    //Do after ball hit the Ghost
    public void BallCollision(int vertexIndex, Vector3 vBall) {
        vertices[vertexIndex].position -= vBall * Time.deltaTime * 0.5f;
        UpdatePosition();
        collision = true;
    }

    private void BuildGhost() {

        //Iterate through all the vertices and add them to the vertices array
        //vertice[] store GhostNode and the acutal GameObject is stored in verticesGO.
        //See my documentation for the position of each vertices.
        Transform t;
        for (int i = 0; i < numOfVertices; i++) {

            if (i == 5 || i == 7 || i == 9)
                t = ghostBody.Find(i.ToString());
            else if (i == 13)
                t = ghostHead.Find("left_eye");
            else if (i == 14)
                t = ghostHead.Find("right_eye");
            else
                t = ghostHead.Find(i.ToString());

            vertices.Add(new GhostNode(t.position));
            verticesGO.Add(t.gameObject);
        }

        for (int i = 0; i < numOfVertices; i++) {
            GhostNode a = vertices[i];

            //Basic Shape of Ghost:
            if (i < 12) {
                GhostNode b = vertices[i + 1];
                GhostEdge e = new GhostEdge(a, b, false);
                a.Connect(e);
                b.Connect(e);
            }

            //Additional Constraints:
            if (i == 4) {
                GhostNode b = vertices[0];
                GhostEdge e = new GhostEdge(a, b, true);
                a.Connect(e);
                b.Connect(e);
            }
            if (i == 6) {
                GhostNode b = vertices[4];
                GhostEdge e = new GhostEdge(a, b, true);
                a.Connect(e);
                b.Connect(e);

                b = vertices[8];
                e = new GhostEdge(a, b, true);
                a.Connect(e);
                b.Connect(e);
            }
            if (i == 10) {
                GhostNode b = vertices[8];
                GhostEdge e = new GhostEdge(a, b, true);
                a.Connect(e);
                b.Connect(e);

                b = vertices[1];
                e = new GhostEdge(a, b, true);
                a.Connect(e);
                b.Connect(e);
            }
            if (i == 11) {
                GhostNode b = vertices[3];
                GhostEdge e = new GhostEdge(a, b, false);
                a.Connect(e);
                b.Connect(e);
            }
            if (i == 12) {
                GhostNode b = vertices[0];
                GhostEdge e = new GhostEdge(a, b, false);
                a.Connect(e);
                b.Connect(e);
            }
            if (i == 13) {
                GhostNode b = vertices[0];
                GhostEdge e = new GhostEdge(a, b, true);
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
                GhostNode b = vertices[1];
                GhostEdge e = new GhostEdge(a, b, true);
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

        for (int iter = 0; iter < constraintIter; iter++) {
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
