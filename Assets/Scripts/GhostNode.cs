using System.Collections.Generic;
using UnityEngine;

public class GhostNode {
    public List<GhostEdge> Connection { get; }

    public Vector3 position;
    private Vector3 prePosition;

    public GhostNode(Vector3 p) {
        position = prePosition = p;
        Connection = new List<GhostEdge>();
    }

    public void NextPosition() {
        Vector3 diff = position - prePosition;
        Vector3 next = position + diff;
        prePosition = position;
        position = next;
    }

    public void Connect(GhostEdge e) {
        Connection.Add(e);
    }

}