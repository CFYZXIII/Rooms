using UnityEngine;
using System.Collections.Generic;

public class MazeGenerator : MonoBehaviour
{
    [field: SerializeField] public Room RoomPrefab { get; private set; }
    [field: SerializeField] public int Height { get; private set; }
    [field: SerializeField] public int Width { get; private set; }
    [field: SerializeField] public float Delta { get; private set; }


    public void Start()
    {
        GenerateMaze();
    }

    /// <summary>
    /// Массив комнат
    /// </summary>
    private Room[,] roomGrid;

    /// <summary>
    /// Список посещенных комнат
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
        //Добавляем в стэк первую комнату

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

                //Смотрим с какой строны стоит комната по отношению к предыдущей

                //Справа
                if (curentRoom.X - startRoom.X > 0)
                {
                    curentRoom.LeftDoor.gameObject.SetActive(true);
                    startRoom.RightDoor.gameObject.SetActive(true);
                }

                //Слева
                if (curentRoom.X - startRoom.X < 0)
                {
                    startRoom.LeftDoor.gameObject.SetActive(true);
                    curentRoom.RightDoor.gameObject.SetActive(true);
                }

                //Сверху
                if (curentRoom.Y - startRoom.Y > 0)
                {
                    startRoom.TopDoor.gameObject.SetActive(true);
                    curentRoom.BottomDoor.gameObject.SetActive(true);
                }

                //Снизу
                if (curentRoom.Y - startRoom.Y < 0)
                {
                    curentRoom.TopDoor.gameObject.SetActive(true);
                    startRoom.BottomDoor.gameObject.SetActive(true);
                }

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
