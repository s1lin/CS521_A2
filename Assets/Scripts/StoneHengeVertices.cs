using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneHengeVertices : MonoBehaviour {

    private Vector3[] allVertices;

    private Vector3[] rightSH;
    private Vector3[] right2SH;
    private Vector3[] leftSH;
    private Vector3[] topSH;
    private Vector3[] top2SH;

    public Vector3[] AllVerrtices() {

        rightSH = GameObject.FindGameObjectWithTag("right").GetComponent<StoneHenge>().wolrdVertices;
        right2SH = GameObject.FindGameObjectWithTag("right2").GetComponent<StoneHenge>().wolrdVertices;
        leftSH = GameObject.FindGameObjectWithTag("left").GetComponent<StoneHenge>().wolrdVertices;
        topSH = GameObject.FindGameObjectWithTag("top").GetComponent<StoneHenge>().wolrdVertices;
        top2SH = GameObject.FindGameObjectWithTag("top2").GetComponent<StoneHenge>().wolrdVertices;

        int rightCount = rightSH.Length;
        int right2Count = right2SH.Length;
        int leftCount = leftSH.Length;
        int topCount = topSH.Length;
        int top2Count = top2SH.Length;
        int index = 0;

        allVertices = new Vector3[rightCount + leftCount + topCount + top2Count + right2Count];
        for (int i = 0; i < rightCount; i++) {
            allVertices[index] = rightSH[i];
            index++;
        }

        for (int i = 0; i < leftCount; i++) {
            allVertices[index] = leftSH[i];
            index++;
        }

        for (int i = 0; i < topCount; i++) {
            allVertices[index] = topSH[i];
            index++;
        }

        for (int i = 0; i < top2Count; i++) {
            allVertices[index] = topSH[i];
            index++;
        } 

        for (int i = 0; i < right2Count; i++) {
            allVertices[index] = rightSH[i];
            index++;
        }

        //for (int i = 0; i < allVertices.Length; i++) {
        //    print(allVertices[i].x + "     " + allVertices[i].y + "     " + allVertices[i].z);
        //}

        return allVertices;
    }
}
