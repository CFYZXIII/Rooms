using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class MazeGenerator : MonoBehaviour
{
    [field: SerializeField] public Room RoomPrefab { get; private set; }

    [field: SerializeField] public GameObject Dot { get; private set; }
    [field: SerializeField] public int Height { get; private set; }
    [field: SerializeField] public int Width { get; private set; }
    [field: SerializeField] public float Delta { get; private set; }


    public void Start()
    {
        GenerateMaze();
        var lastNode = GetMaxDistanceNode();
        ((Room)lastNode).Base.color = Color.green;
        DrawPath(lastNode);
    }

    private void DrawPath(Node node)
    {
        var next = node;

        while (next != null)
        {
            var dot = Instantiate<GameObject>(Dot);
            dot.transform.position = next.transform.position;
            next = next.parentNode;
        }
    }

    private Node GetMaxDistanceNode()
    {
        Node node = null;
        for (int i = 0; i < roomGrid.GetLength(0); i++)
        {
            for (int j = 0; j < roomGrid.GetLength(0); j++)
            {
                if (node == null)
                    node = roomGrid[i, j];
                else
                    if (roomGrid[i, j].Distance > node.Distance)
                {
                    node = roomGrid[i, j];
                }
            }
        }
        return node;
    }

    /// <summary>
    /// ������ ������
    /// </summary>
    private Room[,] roomGrid;

    /// <summary>
    /// ������ ���������� ������
    /// </summary>
    private List<Room> visitedList = new List<Room>();

    private void GenerateMaze()
    {
        roomGrid = new Room[Width, Height];
        GameObject maze = new GameObject();
        maze.name = nameof(maze);
        maze.transform.position = Vector2.zero;

        for (int i = 0; i < Width; i++)
        {
            for (int j = 0; j < Height; j++)
            {
                Room room = Instantiate<Room>(RoomPrefab);
                room.transform.parent = maze.transform;
                room.transform.localPosition = new Vector3(((i-1)-Width/2)+i*Delta,((j-1)-Height/2)+j*Delta, maze.transform.position.z);
                room.name = i + " " + j;
                room.SetCoordinates(i, j);
                roomGrid[i, j] = room;
            }
        }

        Stack<Room> roomStack = new Stack<Room>();
        //��������� � ���� ������ �������

        var startRoom = roomGrid[Random.Range(0, roomGrid.GetLength(0)), Random.Range(0, roomGrid.GetLength(1))];
        startRoom.Base.color = Color.magenta;

        roomStack.Push(startRoom);
        visitedList.Add(startRoom);
        Room curentRoom = null;
        while (roomStack.Count > 0)
        {
            var neigbors = GetNeighbors(startRoom);
            if (neigbors.Count > 0)
            {
                curentRoom = neigbors[Random.Range(0, neigbors.Count)];

                //������� � ����� ������ ����� ������� �� ��������� � ����������

                //������
                if (curentRoom.X - startRoom.X > 0)
                {
                    startRoom.RightDoor.gameObject.SetActive(true);
                }

                //�����
                if (curentRoom.X - startRoom.X < 0)
                {
                    curentRoom.RightDoor.gameObject.SetActive(true);
                }

                //������
                if (curentRoom.Y - startRoom.Y > 0)
                {
                    startRoom.TopDoor.gameObject.SetActive(true);
                }

                //�����
                if (curentRoom.Y - startRoom.Y < 0)
                {
                    curentRoom.TopDoor.gameObject.SetActive(true);
                }

                curentRoom.AddConnection(startRoom);

                roomStack.Push(curentRoom);
                visitedList.Add(curentRoom);
                startRoom = curentRoom;


            }
            else
                startRoom = roomStack.Pop();

        }
    }

    private List<Room> GetNeighbors(Room room)
    {
        List<Room> rooms = new List<Room>();

        var w = roomGrid.GetLength(0);
        var h = roomGrid.GetLength(1);
        for (int i = -1; i <= 1; i += 2)
        {
            if (room.X + i >= 0 && room.X + i < w)
            {
                if (!visitedList.Contains(roomGrid[room.X + i, room.Y]))
                    rooms.Add(roomGrid[room.X + i, room.Y]);
            }
        }

        for (int i = -1; i <= 1; i += 2)
        {
            if (room.Y + i >= 0 && room.Y + i < h)
            {
                if (!visitedList.Contains(roomGrid[room.X, room.Y + i]))
                    rooms.Add(roomGrid[room.X, room.Y + i]);
            }
        }




        return rooms;
    }
}
