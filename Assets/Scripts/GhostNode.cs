using System.Collections.Generic;
using UnityEngine;

public class GhostNode {
    public List<GhostEdge> Connection { get; }

    public Vector3 position;
    private Vector3 prev;

    public GhostNode(Vector3 p) {
        position = prev = p;
        Connection = new List<GhostEdge>();
    }

    public void NextPosition() {
        var v = position - prev;
        var next = position + v;
        prev = position;
        position = next;
    }

    public void Connect(GhostEdge e) {
        Connection.Add(e);
    }

}