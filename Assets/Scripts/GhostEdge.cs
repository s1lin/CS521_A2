using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GhostEdge {
    public float Length { get { return length; } }

    GhostNode a, b;
    float length;

    public GhostEdge(GhostNode a, GhostNode b) {
        this.a = a;
        this.b = b;
        this.length = (a.position - b.position).magnitude;
    }

    public GhostEdge(GhostNode a, GhostNode b, float len) {
        this.a = a;
        this.b = b;
        this.length = len;
    }

    public GhostNode Other(GhostNode p) {
        if (a == p) {
            return b;
        } else {
            return a;
        }
    }
}

