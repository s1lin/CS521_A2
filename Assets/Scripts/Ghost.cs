using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour {

    public float restitution = 0.5f;

    private GameObject ghostInstance;
    private Transform[] ghostVertices;
    private LineRenderer[] ghostLines;
    private Transform ghostBody;

    private Vector3[] curPosition;
    private Vector3[] prePosition;

    private float[] lineConstraint;
    private float lineTolerance = 0.1f;
    private float[] angleConstraint;
    private float angleTolerance = 0.1f;


    private Vector2 initV;
    private float[] velX;
    private float[] velY;

    private bool collision = false;
    private int rootIndex;

    public Transform[] GhostVertices() {
        return ghostVertices;
    }

    void Update() {

        //1.Move to the right without going into the stonehenges
        BasicMovement();

        //2.Destroy out of Scene and Respawn
        if (IsOutOfScence())
            GameObject.FindGameObjectWithTag("game").GetComponent<GameManager>().RespawnGhost(rootIndex);

    }

    private bool IsOutOfScence() {
        for (int i = 0; i < 15; i++) {
            if (ghostVertices[i].position.x > 35f || ghostVertices[i].position.y > 23f)
                return true;
        }
        return false;
    }
    private void BasicMovement() {

        if (!collision) {
            for (int i = 0; i < 15; i++) {

                velX[i] = initV.x;
                velY[i] = initV.y;

                curPosition[i].x = ghostVertices[i].position.x + velX[i] * Time.deltaTime;
                curPosition[i].y = ghostVertices[i].position.y + velY[i] * Time.deltaTime;

                prePosition[i].x = ghostVertices[i].position.x;
                prePosition[i].y = ghostVertices[i].position.y;

                ghostVertices[i].position = new Vector3(curPosition[i].x, curPosition[i].y, 0);
            }
            LineRedraw();
        } else {
            for (int i = 0; i < 15; i++) {
                curPosition[i].x = ghostVertices[i].position.x + velX[i] * Time.deltaTime;
                curPosition[i].y = ghostVertices[i].position.y + velY[i] * Time.deltaTime;

                prePosition[i].x = ghostVertices[i].position.x;
                prePosition[i].y = ghostVertices[i].position.y;
                //while (Vector3.Distance(ghostVertices[i].position, curPosition[i]) > 0.001f) {
                ghostVertices[i].position = new Vector3(curPosition[i].x, curPosition[i].y, 0);
                LineRedraw();
                //    ghostVertices[i].position = Vector3.MoveTowards(curPosition[i], ghostVertices[i].position, 0.1f);
                //}
            }       
        }

    }

    /*
     Upon contact with a moving cannonball the velocity of 
     an incoming ghost is added to whichever vertex or vertices intersect the cannonball
    */
    public void BallCollision(int vertexIndex) {
        
        velX[vertexIndex] = -velX[vertexIndex] * restitution;
        velY[vertexIndex] = -velY[vertexIndex] * restitution;

        curPosition[vertexIndex].x = ghostVertices[vertexIndex].position.x + velX[vertexIndex] * Time.deltaTime;
        curPosition[vertexIndex].y = ghostVertices[vertexIndex].position.y + velY[vertexIndex] * Time.deltaTime;

        ghostVertices[vertexIndex].position = new Vector3(curPosition[vertexIndex].x, curPosition[vertexIndex].y, 0);

        ApplyConstraint();
        LineRedraw();

        collision = true;
        //while (Vector3.Distance(ghostVertices[i].position, curPosition) > 0.001f) {
        //    LineRedraw();
        //    ghostVertices[i].position = Vector3.MoveTowards(curPosition, ghostVertices[i].position, 1f);
        //    //
        //}

        
    }

    private void ApplyConstraint() {

        int counter = 0;
        for (int i = 0; i < 15; i++) {

            float maxDistance = lineConstraint[i] * (1 + lineTolerance);
            float minDistance = lineConstraint[i] * (1 - lineTolerance);

            float maxAngle = 0;
            float minAngle = 0;
            if (angleConstraint[i] != 0) {
                maxAngle = angleConstraint[i] * (1 + angleTolerance);
                minAngle = angleConstraint[i] * (1 - angleTolerance);
            }

            velX[i] = -velX[i] * restitution;
            velY[i] = -velY[i] * restitution;
 
            curPosition[i].x = ghostVertices[i].position.x + velX[i] * Time.deltaTime * Time.deltaTime;
            curPosition[i].y = ghostVertices[i].position.y + velY[i] * Time.deltaTime * Time.deltaTime;


            ghostVertices[i].position = new Vector3(curPosition[i].x, curPosition[i].y, 0);



        }

    }
    public void LineRedraw() {

        //12 vertices
        for (int i = 0; i < 13; i++) {
            if (i > 0) {
                ghostLines[i - 1].startColor = Color.black;
                ghostLines[i - 1].startWidth = 0.05f;
                ghostLines[i - 1].endColor = Color.black;
                ghostLines[i - 1].endWidth = 0.05f;
                ghostLines[i - 1].positionCount = 2;
                ghostLines[i - 1].SetPosition(0, ghostVertices[i - 1].position);
                ghostLines[i - 1].SetPosition(1, ghostVertices[i].position);
                ghostLines[i - 1].material.color = Color.black;
                ghostLines[i - 1].numCapVertices = 1;
            }
        }

        ghostLines[12].startColor = Color.black;
        ghostLines[12].startWidth = 0.05f;
        ghostLines[12].positionCount = 2;
        ghostLines[12].SetPosition(0, ghostVertices[12].position);
        ghostLines[12].SetPosition(1, ghostVertices[0].position);
        ghostLines[12].material.color = Color.black;
        ghostLines[12].numCapVertices = 1;

        for (int i = 13; i < 15; i++) {
            ghostLines[i].startColor = Color.black;
            ghostLines[i].startWidth = 0.05f;
            ghostLines[i].endColor = Color.black;
            ghostLines[i].endWidth = 0.05f;
            ghostLines[i].positionCount = 2;
            ghostLines[i].SetPosition(0, ghostVertices[i].position);
            ghostLines[i].SetPosition(1, ghostVertices[i].position);
            ghostLines[i].material.color = Color.black;
            ghostLines[i].numCapVertices = 1;
        }
    }

    //connecting all the vertices
    public void GhostSpawn(GameObject ghost, int root) {

        ghostInstance = ghost;
        rootIndex = root;

        curPosition = new Vector3[20];
        prePosition = new Vector3[20];
        ghostVertices = new Transform[15];
        ghostLines = new LineRenderer[20];

        velX = new float[20];
        velY = new float[20];
        initV = new Vector2(Mathf.Cos(Mathf.Deg2Rad) / Random.Range(1f, 2f),
            Mathf.Sin(Mathf.Deg2Rad) * Random.Range(20f, 30f));

        ghostBody = ghostInstance.transform.Find("Body");

        //12 vertices
        for (int i = 0; i < 13; i++) {
            ghostVertices[i] = ghostBody.Find(i.ToString());
            curPosition[i] = ghostVertices[i].position;
            prePosition[i] = ghostVertices[i].position;

            if (i > 0) {
                ghostLines[i - 1] = ghostVertices[i - 1].gameObject.AddComponent<LineRenderer>();
                ghostLines[i - 1].startColor = Color.black;
                ghostLines[i - 1].startWidth = 0.05f;
                ghostLines[i - 1].endColor = Color.black;
                ghostLines[i - 1].endWidth = 0.05f;
                ghostLines[i - 1].positionCount = 2;
                ghostLines[i - 1].SetPosition(0, ghostVertices[i - 1].position);
                ghostLines[i - 1].SetPosition(1, ghostVertices[i].position);
                ghostLines[i - 1].material.color = Color.black;
                ghostLines[i - 1].numCapVertices = 1;
            }
        }

        //connect the 12th with the first one
        ghostLines[12] = ghostVertices[12].gameObject.AddComponent<LineRenderer>();
        ghostLines[12].startColor = Color.black;
        ghostLines[12].startWidth = 0.05f;
        ghostLines[12].positionCount = 2;
        ghostLines[12].SetPosition(0, ghostVertices[12].position);
        ghostLines[12].SetPosition(1, ghostVertices[0].position);
        ghostLines[12].material.color = Color.black;
        ghostLines[12].numCapVertices = 1;

        ghostVertices[13] = ghostBody.Find("left_eye");
        ghostVertices[14] = ghostBody.Find("right_eye");

        for (int i = 13; i < 15; i++) {
            curPosition[i] = ghostVertices[i].position;
            prePosition[i] = ghostVertices[i].position;
            ghostLines[i] = ghostVertices[i].gameObject.AddComponent<LineRenderer>();
            ghostLines[i].startColor = Color.black;
            ghostLines[i].startWidth = 0.05f;
            ghostLines[i].endColor = Color.black;
            ghostLines[i].endWidth = 0.05f;
            ghostLines[i].positionCount = 2;
            ghostLines[i].SetPosition(0, ghostVertices[i].position);
            ghostLines[i].SetPosition(1, ghostVertices[i].position);
            ghostLines[i].material.color = Color.black;
            ghostLines[i].numCapVertices = 1;
        }
    }

    //point 0,1,2,3,4,10,11,12,0 must remain shape, 
    //whereas 0 connect to the left eye, and 1 connect to the right eye
    public void ConstraintBuilder() {

        lineConstraint = new float[15];
        angleConstraint = new float[15];

        int counter;
        for (int i = 0; i < 15; i++) {
            if (i == 0)
                counter = 12;
            else
                counter = i - 1;

            if (i == 13) { //Left eye
                float distance = (ghostVertices[0].GetComponent<LineRenderer>().GetPosition(0) - ghostVertices[13].GetComponent<LineRenderer>().GetPosition(0)).magnitude;
                float angle = Vector3.Angle(
                    ghostVertices[i].GetComponent<LineRenderer>().GetPosition(0) - ghostVertices[i].GetComponent<LineRenderer>().GetPosition(1),
                    ghostVertices[0].GetComponent<LineRenderer>().GetPosition(1) - ghostVertices[0].GetComponent<LineRenderer>().GetPosition(0));

                lineConstraint[i] = distance;
                angleConstraint[i] = angle;

            } else if (i == 14) {//right eye
                float distance = (ghostVertices[1].GetComponent<LineRenderer>().GetPosition(0) - ghostVertices[14].GetComponent<LineRenderer>().GetPosition(0)).magnitude;
                float angle = Vector3.Angle(
                    ghostVertices[i].GetComponent<LineRenderer>().GetPosition(0) - ghostVertices[i].GetComponent<LineRenderer>().GetPosition(1),
                    ghostVertices[1].GetComponent<LineRenderer>().GetPosition(1) - ghostVertices[1].GetComponent<LineRenderer>().GetPosition(0));

                lineConstraint[i] = distance;
                angleConstraint[i] = angle;

            } else if (i < 5 && i > 9) {
                float distance = (ghostVertices[i].GetComponent<LineRenderer>().GetPosition(1) - ghostVertices[i].GetComponent<LineRenderer>().GetPosition(0)).magnitude;
                float angle = Vector3.Angle(
                    ghostVertices[counter].GetComponent<LineRenderer>().GetPosition(0) - ghostVertices[counter].GetComponent<LineRenderer>().GetPosition(1),
                    ghostVertices[i].GetComponent<LineRenderer>().GetPosition(1) - ghostVertices[i].GetComponent<LineRenderer>().GetPosition(0));

                lineConstraint[i] = distance;
                angleConstraint[i] = angle;
            } else {
                float distance = (ghostVertices[i].GetComponent<LineRenderer>().GetPosition(1) - ghostVertices[i].GetComponent<LineRenderer>().GetPosition(0)).magnitude;
                lineConstraint[i] = distance;
                angleConstraint[i] = 0;
            }
        }
    }

}
