﻿using Diablerie.Engine.Utility;
using UnityEngine;

namespace Diablerie.Engine
{
    public class CollisionMap
    {
        private static CollisionMap instance;

        public struct Cell
        {
            public CollisionLayers blocked;
            public GameObject gameObject;
        }

        private int width;
        private int height;
        private int origin;
        private Cell[] map;

        public CollisionMap(int width, int height)
        {
            instance = this;
            this.width = width;
            this.height = height;
            map = new Cell[width * height];
            for (int i = 0; i < map.Length; ++i)
            {
                map[i].blocked = CollisionLayers.All;
            }
            origin = width * 5 + 5;
        }

        private int MapToIndex(Vector3 pos)
        {
            return MapToIndex(Iso.Snap(pos));
        }

        private int MapToIndex(Vector2i pos)
        {
            return origin + pos.x + pos.y * width;
        }

        private Vector3 MapToIso(int index)
        {
            index -= origin;
            return new Vector3(index % width, index / width);
        }

        public static Cell GetCell(Vector3 pos)
        {
            var tilePos = Iso.Snap(pos);
            int index = instance.MapToIndex(tilePos);
            return instance.map[index];
        }

        public static bool Passable(Vector3 pos, CollisionLayers mask, int size = 1, GameObject ignore = null)
        {
            return Passable(Iso.Snap(pos), mask, size, ignore);
        }

        public static bool Passable(Vector2i pos, CollisionLayers mask, int size = 1, GameObject ignore = null)
        {
            int index = instance.MapToIndex(pos);
            return Passable(index, mask, size, ignore);
        }

        public static bool Passable(int index, CollisionLayers mask, int size = 1, GameObject ignore = null)
        {
            if (index - size - size * instance.width < 0 || index + size + size * instance.width >= instance.map.Length)
                return false;
        
            index = index - size / 2 - size / 2 * instance.height;
            int step = instance.width - size;
            for (int y = 0; y < size; ++y)
            {
                int end = index + size;
                while (index < end)
                {
                    var cell = instance.map[index];
                    bool passable = (cell.blocked & mask) == 0;
                    if (!passable && (ignore == null || ignore != cell.gameObject))
                    {
                        return false;
                    }
                    ++index;
                }
                index += step;
            }
        
            return true;
        }

        public static void SetBlocked(Vector3 pos, CollisionLayers value)
        {
            SetBlocked(Iso.Snap(pos), value);
        }

        public static void SetBlocked(Vector2i pos, CollisionLayers value)
        {
            int index = instance.MapToIndex(pos);
            instance.map[index].blocked = value;
        }

        public static void SetPassable(Vector3 pos, int sizeX, int sizeY, bool passable, GameObject gameObject)
        {
            SetPassable(Iso.Snap(pos), sizeX, sizeY, passable, gameObject);
        }

        public static void SetPassable(Vector2i pos, int sizeX, int sizeY, bool passable, GameObject gameObject)
        {
            CollisionLayers layers = CollisionLayers.Walk;
            int index = instance.MapToIndex(pos) - sizeX / 2 - sizeY / 2 * instance.height;
            int step = instance.width - sizeX;
            for (int y = 0; y < sizeY; ++y)
            {
                int end = index + sizeX;
                while (index < end)
                {
                    Cell cell = instance.map[index];
                    if (passable && cell.gameObject == gameObject)
                    {
                        cell.blocked = CollisionLayers.None;
                        cell.gameObject = null;
                        instance.map[index] = cell;
                    }
                    else if (!passable && cell.blocked == CollisionLayers.None)
                    {
                        cell.blocked = layers;
                        cell.gameObject = gameObject;
                        instance.map[index] = cell;
                    }
                    ++index;
                }
                index += step;
            }
        }

        public struct RaycastHit
        {
            public bool hit;
            public GameObject gameObject;
            public Vector2 pos;

            public static implicit operator bool(RaycastHit value)
            {
                return value.hit;
            }
        }

        public static RaycastHit Raycast(Vector2 from, Vector2 to, float rayLength = Mathf.Infinity, float maxRayLength = Mathf.Infinity, int size = 1, GameObject ignore = null)
        {
            CollisionLayers mask = CollisionLayers.Walk;
            var hit = new RaycastHit();
            var diff = to - from;
            var stepLen = 0.2f;
            if (rayLength == Mathf.Infinity)
                rayLength = Mathf.Min(diff.magnitude, maxRayLength);
            int stepCount = Mathf.RoundToInt(rayLength / stepLen);
            var step = diff.normalized * stepLen;
            var pos = from;
            for (int i = 0; i < stepCount; ++i)
            {
                pos += step;
                Cell cell = GetCell(pos);
                bool passable = Passable(pos, mask, size, ignore);
                if (!passable)
                {
                    hit.hit = !passable;
                    hit.gameObject = cell.gameObject;
                    hit.pos = pos;
                    break;
                }
            }
            return hit;
        }

        public static int OverlapBox(Vector2 center, Vector2 size, GameObject[] result)
        {
            int count = 0;
            if (result.Length == 0)
                return 0;
            int rows = Mathf.RoundToInt(size.y);
            int columns = Mathf.RoundToInt(size.x);
            int index = instance.MapToIndex(Iso.Snap(center - size / 2));
            for(int row = 0; row < rows; ++row)
            {
                for(int column = 0; column < columns; ++column)
                {
                    var gameObject = instance.map[index + column].gameObject;
                    if (gameObject != null)
                    {
                        result[count] = gameObject;
                        count += 1;
                        if (count >= result.Length)
                            return count;
                    }
                }
                index += instance.width;
            }
            return count;
        }

        public static void Move(Vector2 from, Vector2 to, int size, GameObject gameObject)
        {
            SetPassable(from, size, size, true, gameObject);
            SetPassable(to, size, size, false, gameObject);
        }

        public static bool Fit(Vector3 pos, out Vector3 result, int size = 1, CollisionLayers mask = CollisionLayers.Walk)
        {
            int index = instance.MapToIndex(pos);

            int maxIterations = 100;
            int sign = 1;
            for(int i = 1; i < maxIterations; ++i, sign=-sign)
            {
                int end = index + sign * i;
                for (; index != end && index > size && index < instance.map.Length - size - 1; index += sign)
                {
                    if (Passable(index, mask, size))
                    {
                        result = instance.MapToIso(index);
                        return true;
                    }
                }

                end = index - sign * i * instance.width;
                int step = -sign * instance.width;
                for (; index != end && index > size && index < instance.map.Length - size - 1; index += step)
                {
                    if (Passable(index, mask, size))
                    {
                        result = instance.MapToIso(index);
                        return true;
                    }
                }
            }

            result = new Vector3();
            return false;
        }
    }
}
