using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class Node
{
    public float heuristica { get; set; }
    public int[] posicio { get; set; }

    public Node(int[] posicio, int[] objectiveNode = null) 
    {
        this.posicio = posicio;
        if(objectiveNode != null )
        {
            heuristica = Vector3.Distance(new Vector3(posicio[0], posicio[1], 0), new Vector3(objectiveNode[0], objectiveNode[1], 0));
        }
        else heuristica = 0;
        //this.node = node;
    }
}
