/*
Given rooms (A, B, C, D etc.) and ladders between them 
(A -> B, A -> C, C -> K etc). You are moving down from the top rooms 
Please identify all starting locations (the ones on the top with no ladders towards them)
and possible ending location for every starting location (the ones with no ladders from) 


  A      E    J     Key: [Origins]
 / \    / \    \          \
B   C  F   L    M         [Destinations]
 \ / \ /
  K   G
     / \
    H   I

paths = [
   ["B", "K"],
   ["C", "K"],
   ["E", "L"],
   ["F", "G"],
   ["J", "M"],
   ["E", "F"],
   ["G", "H"],
   ["G", "I"],
   ["C", "G"],
   ["A", "B"],
   ["A", "C"]
]

Expected output (unordered):
  [ "A": ["K", "H", "I"], 
    "E": ["H", "L", "I"], 
    "J": ["M"] 
  ]

N: Number of pairs in the input.
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using System.Security.Cryptography.X509Certificates;

class Node
{
    public string Value { get; set; }
    public Node Parent { get; set; }
    public List<Node> Nexts { get; set; }

    public Node(string value)
    {
        Value = value;
        Nexts = new List<Node>();
    }
}

class Solution
{
    static void Main(String[] args)
    {
        var paths = new string[][]
        {
            new []{"B", "K"},
            new []{"C", "K"},
            new []{"E", "L"},
            new []{"F", "G"},
            new []{"J", "M"},
            new []{"E", "F"},
            new []{"G", "H"},
            new []{"G", "I"},
            new []{"C", "G"},
            new []{"A", "B"},
            new []{"A", "C"},
        };

        Dictionary<string, Node> allNodes = new Dictionary<string, Node>();
        for (int i = 0; i < paths.Length; i++)
        {
            Node parent = null;
            if (allNodes.ContainsKey(paths[i][0]))
            {
                parent = allNodes[paths[i][0]];
            }

            Node child = null;
            if (allNodes.ContainsKey(paths[i][1]))
            {
                child = allNodes[paths[i][1]];
            }

            if (parent == null && child == null)
            {
                // need to create boths nodes and make references
                parent = new Node(paths[i][0]);
                child = new Node(paths[i][1]);
                child.Parent = parent;
                parent.Nexts.Add(child);

                allNodes.Add(paths[i][0], parent);
                allNodes.Add(paths[i][1], child);
            }
            else if (parent != null && child == null)
            {
                // need to create child and make refferences
                child = new Node(paths[i][1]);
                child.Parent = parent;
                parent.Nexts.Add(child);

                allNodes.Add(paths[i][1], child);
            }
            else if (parent == null && child != null)
            {
                // need to create parent and make refferences
                parent = new Node(paths[i][0]);
                child.Parent = parent;
                parent.Nexts.Add(child);

                allNodes.Add(paths[i][0], parent);
            }
            else
            {
                // both were created before
                child.Parent = parent;
                parent.Nexts.Add(child);
            }


        }

        List<Node> topRooms = new List<Node>();
        foreach (var room in allNodes)
        {
            if (room.Value.Parent == null)
            {
                topRooms.Add(room.Value);
            }
        }

        Dictionary<Node, List<Node>> output = new Dictionary<Node, List<Node>>();
        Stack<Node> stack = new Stack<Node>();
        foreach (var room in topRooms)
        {
            List<Node> endingLocations = new List<Node>();
            output.Add(room, endingLocations);
            stack.Push(room);

            while (stack.Count > 0)
            {
                Node currentNode = stack.Pop();
                if (currentNode.Nexts.Count > 0)
                {
                    foreach (var nextNode in currentNode.Nexts)
                    {
                        stack.Push(nextNode);
                    }
                }
                else
                {
                    bool alreadyInTheList = endingLocations.Any(x => x.Value == currentNode.Value);
                    if (!alreadyInTheList)
                    {
                        endingLocations.Add(currentNode);
                    }
                }
            }
        }

        // Result is an output dictionary
        // where Keys are starting location
        // Values are ending locations


    }
}
