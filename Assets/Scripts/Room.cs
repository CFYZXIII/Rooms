using UnityEngine;

public class Room : MonoBehaviour
{
    [field: SerializeField] public Transform LeftDoor { get; private set; }
    [field: SerializeField] public Transform RightDoor { get; private set; }
    [field: SerializeField] public Transform TopDoor { get; private set; }
    [field: SerializeField] public Transform BottomDoor { get; private set; }
    [field: SerializeField] public SpriteRenderer Base { get; private set; }

    public int X { get; private set; }
    public int Y { get; private set; }

    public void SetCoordinates(int x, int y)
    {
        X = x;
        Y = y;
    }
}
