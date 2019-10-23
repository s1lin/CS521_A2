using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneHengeVertices : MonoBehaviour {

    private Vector3[] rightSH;
    private Vector3[] leftSH;
    private Vector3[] topSH;

    public Vector3 rightMost;
    public Vector3 topMost;
    public Vector3 leftMost;
    
    public void AllVerrtices() {

        GameObject top = GameObject.FindGameObjectWithTag("top_bound");
        GameObject left = GameObject.FindGameObjectWithTag("left_bound");
        GameObject right = GameObject.FindGameObjectWithTag("right_bound");

        topSH = top.GetComponent<MeshFilter>().mesh.vertices;
        leftSH = left.GetComponent<MeshFilter>().mesh.vertices;
        rightSH = right.GetComponent<MeshFilter>().mesh.vertices;

        int rightCount = rightSH.Length;
        int leftCount = leftSH.Length;
        int topCount = topSH.Length;
        int index = 0;

        for (int i = 0; i < rightCount; i++) {
            Vector3 v = right.transform.TransformPoint(rightSH[i]);
            if(v.x > rightMost.x) {
                rightMost = v;
            }
        }

        leftMost = rightMost;
        for (int i = 0; i < leftCount; i++) {
            Vector3 v = left.transform.TransformPoint(leftSH[i]);
            if (v.x < leftMost.x) {
                leftMost = v;
            }
        }

        for (int i = 0; i < topCount; i++) {
            Vector3 v = top.transform.TransformPoint(topSH[i]);
            if (v.y > topMost.x) {
                topMost = v;
            }
            //print(allVertices[i]);
            index++;
        }
    }
}
