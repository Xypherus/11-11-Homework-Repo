using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TestCode : MonoBehaviour 
{
    private Transform startPos, endPos;
    public Node startNode { get; set; }
    public Node goalNode { get; set; }

    public List<Node> pathArray;

    GameObject objStartCube, objEndCube;

    private float elapsedTime = 0.0f;
    public float intervalTime = 1.0f; //Interval time between path finding

    public Node[] path;

    // Use this for initialization
    void Start()
    {

        objStartCube = GameObject.FindGameObjectWithTag("Start");
        objEndCube = GameObject.FindGameObjectWithTag("End");

        //AStar Calculated Path
        pathArray = new List<Node>();
        FindPath();
    }

    // Update is called once per frame
    void Update()
    {
        elapsedTime += Time.deltaTime;

        if (elapsedTime >= intervalTime)
        {
            elapsedTime = 0.0f;
            FindPath();
        }
    }

    void FindPath(/*startTrans, endTrans*/)
    {
        startPos = objStartCube.transform; // start trans
        endPos = objEndCube.transform; // end trans

        //Assign StartNode and Goal Node
        startNode = new Node(GridManager.instance.GetGridCellCenter(GridManager.instance.GetGridIndex(startPos.position)));
        goalNode = new Node(GridManager.instance.GetGridCellCenter(GridManager.instance.GetGridIndex(endPos.position)));

        pathArray = AStar.FindPath(startNode, goalNode);
        Debug.Log("hello");

    }

    void OnDrawGizmos()
    {
        if (pathArray == null)
            return;

        if (pathArray.Count > 0)
        {
            int index = 1;
            foreach (Node node in pathArray)
            {
                if (index < pathArray.Count)
                {
                    Node nextNode = (Node)pathArray[index];
                    Debug.DrawLine(node.position, nextNode.position, Color.green);
                    index++;
                }
            };
        }
    }
}