using UnityEngine;

public enum Direction
{
    East = 0, North = 1, West = 2, South = 3
}

public static class EnumExtension
{
    public static Direction ToEnum(this Vector2Int v)
    {
        if (v == Vector2Int.right) return Direction.East;
        else if (v == Vector2Int.up) return Direction.North;
        else if (v == Vector2Int.left) return Direction.West;
        else return Direction.South;
    }
}