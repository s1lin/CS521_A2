﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneHengeVertices : MonoBehaviour {

    private Vector3[] allVertices;

    private Vector3[] rightSH;
    private Vector3[] leftSH;
    private Vector3[] topSH;

    public Vector3[] AllVerrtices() {

        rightSH = GameObject.FindGameObjectWithTag("right").GetComponent<StoneHenge>().wolrdVertices;
        leftSH = GameObject.FindGameObjectWithTag("left").GetComponent<StoneHenge>().wolrdVertices;
        topSH = GameObject.FindGameObjectWithTag("top").GetComponent<StoneHenge>().wolrdVertices;

        int rightCount = rightSH.Length;
        int leftCount = leftSH.Length;
        int topCount = topSH.Length;
        int index = 0;

        allVertices = new Vector3[rightCount + leftCount + topCount];
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

        //for (int i = 0; i < allVertices.Length; i++) {
        //    print(allVertices[i].x + "     " + allVertices[i].y + "     " + allVertices[i].z);
        //}

        return allVertices;
    }
}