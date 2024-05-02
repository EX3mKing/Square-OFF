using UnityEngine;

public class Room : MonoBehaviour
{
    public Type type;
    public Type up;
    public Type down;
    public Type right;
    public Type left;
    [Tooltip("X coordinate of the room in the maze grid.")]
    public int x;
    [Tooltip("Y coordinate of the room in the maze grid.")]
    public int y;
    
    public enum Type
    {
        Normal,
        Empty,
        Spawn,
        Rest,
        Shop,
        Treasure,
        Hall,
        Boss,
        BossHall,
        Null
    }
    
    public void AssignNeighbours(Type up, Type down, Type right, Type left)
    {
        if (up != Type.Null) this.up = up;
        if (down != Type.Null) this.down = down;
        if (right != Type.Null) this.right = right;
        if (left != Type.Null) this.left = left;
    }
    
    public int NeighbourTypeNumber (Type t)
    {
        int number = 0;
        if (up == t) number++;
        if (down == t) number++;
        if (right == t) number++;
        if (left == t) number++;
        return number;
    }
    
    public void Copy(Room room)
    {
        type = room.type;
        up = room.up;
        down = room.down;
        right = room.right;
        left = room.left;
        x = room.x;
        y = room.y;
    }
}
