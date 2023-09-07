using System;
using System.Windows.Forms;
using System.Drawing;

namespace Maze
{
    class Labirint
    {
        // позиция главного персонажа       
        public int characterPositionX { get; set; }
        public int characterPositionY { get; set; }

        public int height; // высота лабиринта (количество строк) 
        public int width; // ширина лабиринта (количество столбцов в каждой строке)

        public MazeObject[,] objects;
        public PictureBox[,] images;

        public static Random r = new Random();
        public Form parent;

        public int totalMedalsGenerated;
        public int totalEnemiesGenerated;
        public int totalHealthGenerated;

        public Labirint(Form parent, int width, int height)
        {
            this.width = width;
            this.height = height;
            this.parent = parent;

            objects = new MazeObject[height, width];
            images = new PictureBox[height, width];

            characterPositionX = 0;
            characterPositionY = 2;

            totalMedalsGenerated = 0;
            totalEnemiesGenerated = 0;
            totalHealthGenerated = 0;

            Generate();
        }

        public void Generate()
        {
            
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    MazeObject.MazeObjectType current = MazeObject.MazeObjectType.HALL;

                    // в 1 случае из 5 - ставим стену
                    if (r.Next(5) == 0)
                    {
                        current = MazeObject.MazeObjectType.WALL;
                    }

                    // в 1 случае из 250 - кладём денежку
                    if (r.Next(250) == 0)
                    {
                        //totalMedalsGenerated++;
                        current = MazeObject.MazeObjectType.MEDAL;
                    }

                    // в 1 случае из 250 - размещаем врага
                    if (r.Next(250) == 0)
                    {
                        //totalEnemiesGenerated++;
                        current = MazeObject.MazeObjectType.ENEMY;
                    }

                    // в 1 случае из 250 - размещаем аптечки
                    if (r.Next(250) == 0)
                    {
                        //totalHealthGenerated++;
                        current = MazeObject.MazeObjectType.HEALTH;
                    }

                    // стены по периметру обязательны
                    if (y == 0 || x == 0 || y == height - 1 | x == width - 1)
                    {
                        current = MazeObject.MazeObjectType.WALL;
                    }

                    // наш персонажик
                    if (x == characterPositionX && y == characterPositionY)
                    {
                        current = MazeObject.MazeObjectType.CHAR;
                    }

                    // есть выход, и соседняя ячейка справа всегда свободна
                    if (x == characterPositionX + 1 && y == characterPositionY || x == width - 1 && y == height - 2)
                    {
                        current = MazeObject.MazeObjectType.HALL;
                    }

                    objects[y, x] = new MazeObject(current);
                    images[y, x] = new PictureBox();
                    images[y, x].Location = new Point(x * objects[y, x].width, y * objects[y, x].height);
                    images[y, x].Parent = parent;
                    images[y, x].Width = objects[y, x].width;
                    images[y, x].Height = objects[y, x].height;
                    images[y, x].BackgroundImage = objects[y, x].texture;
                }
            }

            // метод подсчёта вынесен за рабки цикла выше,
            // т.к. накладываются элементы, и иногда получается не верный подсчёт 
            foreach (var cell in objects)
            {
                if (cell.type == MazeObject.MazeObjectType.MEDAL)
                {
                    totalMedalsGenerated++;
                }
                else if (cell.type == MazeObject.MazeObjectType.ENEMY)
                {
                    totalEnemiesGenerated++;
                }
                else if (cell.type == MazeObject.MazeObjectType.HEALTH)
                {
                    totalHealthGenerated++;
                }
            }
        }
    }
}
