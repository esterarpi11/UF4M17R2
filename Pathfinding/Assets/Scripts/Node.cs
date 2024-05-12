using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class Node
{
    public float heuristica { get; set; }
    public int[] posicio { get; set; }
    public float cost = Calculator.distance;
    public Node parent { get; set; }
    public float total { get; set; }

    public Node(int[] posicio, int[] objectiveNode = null, Node parent = null)
    {
        this.posicio = posicio;
        if (objectiveNode != null)
        {
            heuristica = Vector3.Distance(new Vector3(posicio[0], posicio[1], 0), new Vector3(objectiveNode[0], objectiveNode[1], 0));
        }
        else heuristica = 0;
        if (parent != null)
        {
            this.parent = parent;
            total = heuristica + cost + (parent.total - parent.heuristica);
        }
        else
        {
            this.parent = null;
            total = heuristica + cost * 2;
        }
    }
}
