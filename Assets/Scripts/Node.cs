using UnityEngine;
using System.Collections.Generic;

public class Node : MonoBehaviour
{
    public Node parentNode = null;

    public int Distance { get; private set; }
    public int X { get; private set; }
    public int Y { get; private set; }

    public void SetCoordinates(int x, int y)
    {
        X = x;
        Y = y;
    }

    private List<Node> connectedNeighbors = new List<Node>();

    /// <summary>
    /// Добавляет связь между комнатами
    /// </summary>
    /// <param name="startRoom"></param>
    public void AddConnection(Node startRoom)
    {
        if (!connectedNeighbors.Contains(startRoom))
        {
            connectedNeighbors.Add(startRoom);
            this.parentNode = startRoom;
            Distance = startRoom.Distance+1;
        }
    }


    public IEnumerable<Node> GetConnectedRoom => connectedNeighbors;
    
}
