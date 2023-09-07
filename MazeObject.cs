﻿using System;
using System.Drawing;

namespace Maze
{
    class MazeObject
    {
        public enum MazeObjectType { HALL, WALL, MEDAL, ENEMY, CHAR, HEALTH };

        public static Bitmap[] images = {
            new Bitmap(Properties.Resources.hall),
            new Bitmap(Properties.Resources.wall),
            new Bitmap(Properties.Resources.medal),
            new Bitmap(Properties.Resources.enemy),
            new Bitmap(Properties.Resources.player),
            new Bitmap(Properties.Resources.health)
        };

        public MazeObjectType type;
        public int width; 
        public int height;
        public Image texture;

        public MazeObject(MazeObjectType type)
        {
            this.type = type;
            width = 16;
            height = 16;
            texture = images[(int)type];
        }

    }
}
