public class GhostEdge {
    public float Length { get; }
    public bool IsConstraint { get; } = false;

    private GhostNode a;
    private GhostNode b;

    public GhostEdge(GhostNode a, GhostNode b, bool isConstraint) {
        this.a = a;
        this.b = b;
        Length = (a.position - b.position).magnitude;
        IsConstraint = isConstraint;
    }

    //Find which Node does p connected to
    public GhostNode ConnectedTo(GhostNode p) {
        return a == p ? b : a;
    }

}