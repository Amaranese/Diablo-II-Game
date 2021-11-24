﻿using System.Collections.Generic;
using System.IO;
using Diablerie.Engine.Utility;
using UnityEngine;

namespace Diablerie.Engine.IO.D2Formats
{
    public class DC6 : Spritesheet
    {
        public struct Frame
        {
            public int width;
            public int height;
            public int offsetX;
            public int offsetY;
            public Texture2D texture;
            public int textureX;
            public int textureY;
            public int dataOffset;
            public int dataSize;
        }

        public struct Direction
        {
            public Frame[] frames;
            public Sprite[] sprites;
        }

        new public int directionCount;
        public int framesPerDirection;
        public Direction[] directions;
        public List<Texture2D> textures = new List<Texture2D>();
        int[] offsets;
        byte[] bytes;
        int textureSize = -1;
        Color32[] palette;

        public static DC6 Load(string filename, Color32[] palette, bool mpq = true, int textureSize = -1, bool loadAllDirections = false)
        {
            UnityEngine.Profiling.Profiler.BeginSample("DC6.DecodeDirection");
            try
            {
                var bytes = mpq ? Mpq.ReadAllBytes(filename) : File.ReadAllBytes(filename);

                using (var stream = new MemoryStream(bytes))
                using (var reader = new BinaryReader(stream))
                {
                    int version1 = reader.ReadInt32();
                    var version2 = reader.ReadInt32();
                    var version3 = reader.ReadInt32();
                    if (version1 != 6 || version2 != 1 || version3 != 0)
                    {
                        Debug.LogWarning("Unknown dc6 version " + version1 + " " + version2 + " " + version3);
                        return null;
                    }

                    DC6 dc6 = Load(stream, reader, bytes, palette, textureSize, loadAllDirections);
                    return dc6;
                }
            }
            finally
            {
                UnityEngine.Profiling.Profiler.EndSample();
            }
        }

        static DC6 Load(Stream stream, BinaryReader reader, byte[] bytes, Color32[] palette, int textureSize = -1, bool loadAllDirections = false)
        {
            var dc6 = new DC6();
            reader.ReadInt32();
            dc6.directionCount = reader.ReadInt32();
            dc6.framesPerDirection = reader.ReadInt32();
            dc6.directions = new Direction[dc6.directionCount];
            dc6.offsets = new int[dc6.directionCount * dc6.framesPerDirection];
            dc6.bytes = bytes;
            dc6.textureSize = textureSize;
            dc6.palette = palette;
            for(int i = 0; i < dc6.offsets.Length; ++i)
            {
                dc6.offsets[i] = reader.ReadInt32();
            }

            if (loadAllDirections)
            {
                for (int i = 0; i < dc6.directionCount; ++i)
                {
                    dc6.LoadDirection(stream, reader, i);
                }
            }

            return dc6;
        }

        public override Sprite[] GetSprites(int d)
        {
            if (directions[d].sprites == null)
                LoadDirection(d);

            return directions[d].sprites;
        }

        void LoadDirection(int dirIndex)
        {
            using (var stream = new MemoryStream(bytes))
            using (var reader = new BinaryReader(stream))
            {
                LoadDirection(stream, reader, dirIndex);
            }
        }

        void LoadDirection(Stream stream, BinaryReader reader, int dirIndex)
        {
            Direction dir = ReadFrames(stream, reader, dirIndex);
            directions[dirIndex] = dir;
        }

        private Direction ReadFrames(Stream stream, BinaryReader reader, int dirIndex)
        {
            int internalIndex = DirectionMapping.MapToInternal(directionCount, dirIndex);
            var dir = new Direction();
            dir.frames = new Frame[framesPerDirection];
            dir.sprites = new Sprite[framesPerDirection];
            int maxWidth = 0;
            int maxHeight = 0;

            for (int frameIndex = 0; frameIndex < framesPerDirection; frameIndex++)
            {
                int offset = offsets[internalIndex * framesPerDirection + frameIndex];
                stream.Seek(offset, SeekOrigin.Begin);

                var frame = new Frame();
                reader.ReadInt32(); // skip
                frame.width = reader.ReadInt32();
                frame.height = reader.ReadInt32();
                frame.offsetX = reader.ReadInt32();
                frame.offsetY = reader.ReadInt32();
                reader.ReadInt32(); // skip
                reader.ReadInt32(); // skip
                frame.dataSize = reader.ReadInt32();
                frame.dataOffset = (int)stream.Position;
                dir.frames[frameIndex] = frame;

                maxWidth = Mathf.Max(maxWidth, frame.width);
                maxHeight = Mathf.Max(maxHeight, frame.height);
            }

            TexturePacker packer;
            int padding = framesPerDirection > 1 ? 2 : 0;

            if (textureSize == -1)
            {
                int textureWidth = Mathf.NextPowerOfTwo((maxWidth + padding) * framesPerDirection);
                int textureHeight = Mathf.NextPowerOfTwo(maxHeight + padding);
                textureWidth = Mathf.Min(1024, textureWidth);
                packer = new TexturePacker(textureWidth, textureHeight, padding);
            }
            else
            {
                packer = new TexturePacker(textureSize, textureSize, padding);
            }

            DrawFrames(dir, packer);

            return dir;
        }

        private void DrawFrames(Direction dir, TexturePacker packer)
        {
            int textureWidth = packer.maxWidth;
            int textureHeight = packer.maxHeight;
            Texture2D texture = null;
            Color32[] pixels = null;

            for (int frameIndex = 0; frameIndex < framesPerDirection; frameIndex++)
            {
                var frame = dir.frames[frameIndex];
                var pack = packer.put(frame.width, frame.height);
                if (pack.newTexture)
                {
                    if (texture != null)
                    {
                        texture.SetPixels32(pixels);
                        texture.Apply(false);
                    }
                    texture = new Texture2D(textureWidth, textureHeight, TextureFormat.ARGB32, false);
                    pixels = new Color32[textureWidth * textureHeight];
                    textures.Add(texture);
                }

                DrawFrame(bytes, frame.dataOffset, frame.dataSize, pixels, palette, textureWidth, textureHeight, pack.x, pack.y + frame.height);
                frame.texture = texture;
                frame.textureX = pack.x;
                frame.textureY = pack.y;
                dir.frames[frameIndex] = frame;

                var textureRect = new Rect(frame.textureX, textureHeight - frame.textureY - frame.height, frame.width, frame.height);
                var pivot = new Vector2(-frame.offsetX / (float)frame.width, frame.offsetY / (float)frame.height);
                dir.sprites[frameIndex] = Sprite.Create(texture, textureRect, pivot, Iso.pixelsPerUnit, extrude: 0, meshType: SpriteMeshType.FullRect);
            }

            if (texture != null)
            {
                texture.SetPixels32(pixels);
                texture.Apply(false);
            }
        }

        static void DrawFrame(byte[] data, int offset, int size, Color32[] pixels, Color32[] palette, int textureWidth, int textureHeight, int x0, int y0)
        {
            int dst = textureWidth * (textureHeight - 1) - y0 * textureWidth;
            int i2, x = x0, c, c2;

            for (int i = 0; i < size; i++)
            {
                c = data[offset];
                ++offset;

                if (c == 0x80)
                {
                    x = x0;
                    dst += textureWidth;
                }
                else if ((c & 0x80) != 0)
                    x += c & 0x7F;
                else
                {
                    for (i2 = 0; i2 < c; i2++)
                    {
                        c2 = data[offset];
                        ++offset;
                        i++;
                        pixels[dst + x] = palette[c2];
                        x++;
                    }
                }
            }
        }
    }
}
