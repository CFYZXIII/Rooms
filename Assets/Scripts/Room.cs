using UnityEngine;
using System.Collections.Generic;
public class Room : Node
{
    [field: SerializeField] public Transform RightDoor { get; private set; }
    [field: SerializeField] public Transform TopDoor { get; private set; }
    [field: SerializeField] public SpriteRenderer Base { get; private set; }

   
}
