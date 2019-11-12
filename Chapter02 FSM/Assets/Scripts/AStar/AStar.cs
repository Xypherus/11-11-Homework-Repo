using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AStar
{
    #region List fields

    public static PriorityQueue openList;
    public static List<Node> closedList;

    #endregion

    /// <summary>
    /// Calculate the final path in the path finding
    /// </summary>
    private static List<Node> CalculatePath(Node node) //11 (not in algorithm): find complete path
																						   //Gets the complete path from end to start, then reverses it
    {
        List<Node> list = new List<Node>();
        while (node != null)
        {
            Debug.Log("Inside While Loop");
            list.Add(node);
            node = node.parent;
        }
        list.Reverse();

        Debug.Log("List Count: " + list.Count);
        return list;
    }

    /// <summary>
    /// Calculate the estimated Heuristic cost to the goal
    /// </summary>
    private static float HeuristicEstimateCost(Node curNode, Node goalNode) //5: Determine the cost of nodes
																															   //Gets a rough heuristic cost from a node to the end position
    {
        Vector3 vecCost = curNode.position - goalNode.position;
        return vecCost.magnitude;
    }

    /// <summary>
    /// Find the path between start node and goal node using AStar Algorithm
    /// </summary>
    public static List<Node> FindPath(Node start, Node goal) //1-10: pretty much the entire algorithm
																									//This function is the backbone for the whole algorithm
    {
        //Start Finding the path
        openList = new PriorityQueue();
        openList.Push(start);
        start.nodeTotalCost = 0.0f;
        start.estimatedCost = HeuristicEstimateCost(start, goal);

        closedList = new List<Node>();
        Node node = null;

        while (openList.Length != 0)
        {
            Debug.Log("Find path While has run");
            node = openList.First();

            if (node.position == goal.position)
            {
                Debug.Log("Pos = Goal");
                return CalculatePath(node);
            }
			
            List<Node> neighbours = new List<Node>();
            GridManager.instance.GetNeighbours(node, neighbours);

            #region CheckNeighbours

            //Get the Neighbours
            for (int i = 0; i < neighbours.Count; i++)
            {
                //Cost between neighbour nodes
                Node neighbourNode = (Node)neighbours[i];

                if (!closedList.Contains(neighbourNode))
                {					
					//Cost from current node to this neighbour node
	                float cost = HeuristicEstimateCost(node, neighbourNode);	
	                
					//Total Cost So Far from start to this neighbour node
	                float totalCost = node.nodeTotalCost + cost;
					
					//Estimated cost for neighbour node to the goal
	                float neighbourNodeEstCost = HeuristicEstimateCost(neighbourNode, goal);					
					
					//Assign neighbour node properties
	                neighbourNode.nodeTotalCost = totalCost;
	                neighbourNode.parent = node;
	                neighbourNode.estimatedCost = totalCost + neighbourNodeEstCost;
	
	                //Add the neighbour node to the list if not already existed in the list
	                if (!openList.Contains(neighbourNode))
	                {
	                    openList.Push(neighbourNode);
	                }
                }
            }
			
            #endregion
            
            closedList.Add(node);
            openList.Remove(node);
        }

        //If finished looping and cannot find the goal then return null
        if (node.position != goal.position)
        {
            Debug.LogError("Goal Not Found");
            return null;
        }

        //Calculate the path based on the final node
        return CalculatePath(node);
    }
}
