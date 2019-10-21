using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public Wind wind;
    public StoneHengeVertices hengeVertices;

    //Ghost    
    public GameObject ghostPrefab;
    public List<GameObject> ghostInstances;
    private float posX = -2f;
    private float posY = 3f;
    private float offset = 1f;

    void Start() {
        InitGhosts();
    }

    private void InitGhosts() {
        ghostInstances = new List<GameObject>();

        for (int i = 0; i < 5; i++) {
            ghostInstances.Add(Instantiate(ghostPrefab) as GameObject);
            Ghost ghost = ghostInstances[i].GetComponent<Ghost>();

            float initX = Random.Range(posX, posX + i * offset);
            float initY = Random.Range(posY, posY + i * offset);

            ghostInstances[i].transform.position = new Vector3(initX, initY, 0);
            ghost.GhostSpawn(ghostInstances[i], i);
            ghost.ConstraintBuilder();
        }
        //BUG FIX:
        Destroy(ghostInstances[4]);
        ghostInstances.RemoveAt(4);
    }

    public void RespawnGhost(int index) {
        Destroy(ghostInstances[index]);
        ghostInstances[index] = Instantiate(ghostPrefab) as GameObject;
        Ghost ghost = ghostInstances[index].GetComponent<Ghost>();

        float initX = Random.Range(posX, posX + offset);
        float initY = Random.Range(posY, posY + offset);

        ghostInstances[index].transform.position = new Vector3(initX, initY, 0);
        ghost.GhostSpawn(ghostInstances[index], index);
        ghost.ConstraintBuilder();
    }
}
