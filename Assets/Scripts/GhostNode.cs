using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostNode {
    public List<GhostEdge> Connection { get { return connection; } }

    public Vector3 position;
    protected Vector3 prev;

    List<GhostEdge> connection;

    public GhostNode(Vector3 p) {
        position = prev = p;
        connection = new List<GhostEdge>();
    }

    public void Step() {
        var v = position - prev;
        var next = position + v;
        prev = position;
        position = next;
    }

    public void Connect(GhostEdge e) {
        connection.Add(e);
    }

}