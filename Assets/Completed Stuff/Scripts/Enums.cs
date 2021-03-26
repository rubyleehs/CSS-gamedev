using UnityEngine;

public enum Direction
{
    East = 0, North = 1, West = 2, South = 3, None = 4
}

public static class EnumExtension
{
    public static Direction ToEnum(this Vector2Int v)
    {
        if (v == Vector2Int.right) return Direction.East;
        else if (v == Vector2Int.up) return Direction.North;
        else if (v == Vector2Int.left) return Direction.West;
        else if (v == Vector2Int.down) return Direction.South;
        else return Direction.None;
    }

    public static Vector3Int ToVector3Int(this Direction dir)
    {
        switch (dir)
        {
            case Direction.East:
                return Vector3Int.right;
            case Direction.North:
                return Vector3Int.up;
            case Direction.West:
                return Vector3Int.left;
            case Direction.South:
                return Vector3Int.down;
            default:
                return Vector3Int.zero;
        }
    }
}