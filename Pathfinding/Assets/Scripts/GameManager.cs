using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEditor.Experimental.GraphView;
using UnityEditor.Tilemaps;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject token1, token2, token3, token4, token5;
    private int[,] GameMatrix; //0 not chosen, 1 player, 2 enemy
    private int[] startPos = new int[2];
    private int[] objectivePos = new int[2];

    List<Node> llistaOberta = new List<Node>();
    List<Node> llistaTancada = new List<Node>();

    Node actualNode;
    Node objectiveNode;

    bool win = false;

    int posicioLlista = 0;
    bool alreadyDone = false;
    private void Awake()
    {
        GameMatrix = new int[Calculator.length, Calculator.length];

        for (int i = 0; i < Calculator.length; i++) //fila
            for (int j = 0; j < Calculator.length; j++) //columna
                GameMatrix[i, j] = 0;

        //randomitzar pos final i inicial;
        var rand1 = Random.Range(0, Calculator.length);
        var rand2 = Random.Range(0, Calculator.length);
        startPos[0] = rand1;
        startPos[1] = rand2;
        SetObjectivePoint(startPos);
        actualNode = new Node(startPos, objectiveNode.posicio );
        llistaOberta.Add(actualNode);
        llistaTancada.Add(actualNode);

        GameMatrix[startPos[0], startPos[1]] = 1;
        GameMatrix[objectivePos[0], objectivePos[1]] = 2;

        InstantiateToken(token1, startPos);
        InstantiateToken(token2, objectivePos);
        ShowMatrix();
    }
    private void InstantiateToken(GameObject token, int[] position)
    {
        Instantiate(token, Calculator.GetPositionFromMatrix(position),
            Quaternion.identity);
    }
    private void SetObjectivePoint(int[] startPos) 
    {
        var rand1 = Random.Range(0, Calculator.length);
        var rand2 = Random.Range(0, Calculator.length);
        if (rand1 != startPos[0] || rand2 != startPos[1])
        {
            objectivePos[0] = rand1;
            objectivePos[1] = rand2;
            objectiveNode = new Node(objectivePos);
        }
    }
    private void ShowMatrix() //fa un debug log de la matriu
    {
        string matrix = "";
        for (int i = 0; i < Calculator.length; i++)
        {
            for (int j = 0; j < Calculator.length; j++)
            {
                matrix += GameMatrix[i, j] + " ";
            }
            matrix += "\n";
        }
        Debug.Log(matrix);
    }
    //EL VOSTRE EXERCICI COMENÇA AQUI
    private void Update()
    {
        if(!win)
        {
            //Left
            if (actualNode.posicio[1] - 1 >= 0)
            {
                llistaOberta.Add(new Node(getArray(actualNode.posicio[0], actualNode.posicio[1] - 1), objectiveNode.posicio));
                InstantiateToken(token3, getArray(actualNode.posicio[0], actualNode.posicio[1] - 1));
            }
            //Right
            if (actualNode.posicio[1] + 1 < Calculator.length)
            {
                llistaOberta.Add(new Node(getArray(actualNode.posicio[0], actualNode.posicio[1] + 1), objectiveNode.posicio));
                InstantiateToken(token3, getArray(actualNode.posicio[0], actualNode.posicio[1] + 1));
            }
            //Top
            if (actualNode.posicio[0] - 1 >= 0)
            {
                llistaOberta.Add(new Node(getArray(actualNode.posicio[0] - 1, actualNode.posicio[1]), objectiveNode.posicio));
                InstantiateToken(token3, getArray(actualNode.posicio[0] - 1, actualNode.posicio[1]));
            }
            //Bottom
            if (actualNode.posicio[0] + 1 < Calculator.length)
            {
                llistaOberta.Add(new Node(getArray(actualNode.posicio[0] + 1, actualNode.posicio[1]), objectiveNode.posicio));
                InstantiateToken(token3, getArray(actualNode.posicio[0] + 1, actualNode.posicio[1]));
            }
            actualNode = getBestNode(llistaOberta[posicioLlista], llistaOberta[posicioLlista+1], llistaOberta[posicioLlista+2], llistaOberta[posicioLlista+3]);
            llistaTancada.Add(actualNode);
            posicioLlista = posicioLlista + 4;
        }
        if (actualNode.posicio[0] == objectiveNode.posicio[0] && actualNode.posicio[1] == objectiveNode.posicio[1])
        {
            win = true;
            if (!alreadyDone)
            {
                List<Node> camiEscollit = new List<Node>();
                Node currentNode = llistaTancada[0];
                camiEscollit.Add(actualNode);

                StartCoroutine(print(llistaTancada, 1, token4)); 
                
                for(int i = 1; i < llistaTancada.Count; i++)
                {                    
                    if (currentNode.heuristica > llistaTancada[i].heuristica)
                    {
                        camiEscollit.Add(llistaTancada[i]);
                        actualNode = llistaTancada[i];
                    }
                }

                StartCoroutine(print(camiEscollit, 2, token5));
                alreadyDone = true;
            }          
        }       
    }
    IEnumerator print(List<Node> llista, int seconds, GameObject token)
    {
        yield return new WaitForSeconds(seconds);
        foreach (Node node in llista)
        {
            InstantiateToken(token, node.posicio);
        }
    }
    Node getBestNode(Node topNode, Node bottomNode,  Node leftNode, Node rightNode) 
    {
        Node bestNode = topNode;
        if(bottomNode.heuristica < bestNode.heuristica) bestNode = bottomNode;    
        if(leftNode.heuristica < bestNode.heuristica) bestNode = leftNode;
        if (rightNode.heuristica < bestNode.heuristica) bestNode = rightNode;
        return bestNode;
    }
    int[] getArray(int a, int b)
    {
        int[] array = {a, b };
        return array;
    }
}
