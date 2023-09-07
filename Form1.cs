using System;
using System.CodeDom.Compiler;
using System.Drawing;
using System.Windows.Forms;

namespace Maze
{
    public partial class Form1 : Form
    {
        // размеры лабиринта, количество ячеек 16х16 пикселей
        int columns = 50;
        int rows = 30;

        // ширина и высота одной ячейки или картинки, в пикселях
        int pictureSize = 16;

        Labirint l; // ссылка на логику всего происходящего в лабиринте

        int medalsCollected = 0; // сколько собрал медалей, выводится
        int enemiesKilled = 0; // сколько убил врагов, выводится
        int healthNow = 100; // здоровье, выводится
        

        public Form1()
        {
            InitializeComponent();
            Options();
            StartGame(); 
        }

        public void Options()
        {
            Text = "Maze";

            BackColor = Color.FromArgb(255, 92, 118, 137);

            //Width = columns * 16 + 16;
            //Height = rows * 16 + 40;

            // размеры клиентской области формы
            // (того участка где размещаются элементы управления)
            ClientSize = new Size(
                columns * pictureSize,
                rows * pictureSize);
            
            StartPosition = FormStartPosition.CenterScreen;            
        }

        public void StartGame() {
            l = new Labirint(this, columns, rows);            
        }

        private void Form1_Load(object sender, System.EventArgs e)
        {
            // текст, который выводится до начала движения героя
            Text = $"Maze  Coordinates: x = {l.characterPositionX}, y = {l.characterPositionY}, Health: {healthNow}, {medalsCollected} of {l.totalMedalsGenerated} medals collected, {enemiesKilled} of {l.totalEnemiesGenerated} enemies killed";
            
            
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (l.totalEnemiesGenerated == 0 || l.totalHealthGenerated == 0 || l.totalMedalsGenerated == 0)
            {
                Text = $"Maze  Coordinates: x = {l.characterPositionX}, y = {l.characterPositionY}, Health: {healthNow}, {medalsCollected} of {l.totalMedalsGenerated} medals collected, {enemiesKilled} of {l.totalEnemiesGenerated} enemies killed";
                MessageBox.Show("NOT THE BEST GENERATION OF MAZE", "Attention", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            else if (e.KeyCode == Keys.Right)
            {
                // проверка на то, свободна ли ячейка
                if (l.objects[l.characterPositionY, l.characterPositionX + 1].type == MazeObject.MazeObjectType.HALL)
                {
                    l.objects[l.characterPositionY, l.characterPositionX].texture = MazeObject.images[0]; // поменяли текстуру, где был герой на корридор
                    l.images[l.characterPositionY, l.characterPositionX].BackgroundImage = l.objects[l.characterPositionY, l.characterPositionX].texture;
                                        
                    l.characterPositionX++; // шаг на ячейку вправо

                    l.objects[l.characterPositionY, l.characterPositionX].texture = MazeObject.images[4]; // поменяли текстуру
                    l.images[l.characterPositionY, l.characterPositionX].BackgroundImage = l.objects[l.characterPositionY, l.characterPositionX].texture;

                    Text = $"Maze  Coordinates: x = {l.characterPositionX}, y = {l.characterPositionY}, Health: {healthNow}, {medalsCollected} of {l.totalMedalsGenerated} medals collected, {enemiesKilled} of {l.totalEnemiesGenerated} enemies killed";

                    // условия выхода из лабиринта, победы
                    if (l.characterPositionX == columns - 1 && l.characterPositionY == rows - 2)
                    {
                        Console.Beep();
                        Text = $"Maze  Coordinates: x = {l.characterPositionX}, y = {l.characterPositionY}, Health: {healthNow}, {medalsCollected} of {l.totalMedalsGenerated} medals collected, {enemiesKilled} of {l.totalEnemiesGenerated} enemies killed !!! YOU WIN !!!";

                        DialogResult dialogResultFinish = MessageBox.Show("YOU FOUND EXIT", "Game over", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        
                        if (dialogResultFinish == DialogResult.Yes)
                        {
                            MessageBox.Show("Thank you for playing");
                        }
                        else if (dialogResultFinish == DialogResult.No)
                        {
                            MessageBox.Show("Sorry, anyhow finish");
                        }
                    }
                }

                // проверка на то, если следующая ячейка приз (медаль в данном контексте)
                else if (l.objects[l.characterPositionY, l.characterPositionX + 1].type == MazeObject.MazeObjectType.MEDAL)
                {
                    l.objects[l.characterPositionY, l.characterPositionX].texture = MazeObject.images[0]; // поменяли текстуру, где был герой на корридор
                    l.images[l.characterPositionY, l.characterPositionX].BackgroundImage = l.objects[l.characterPositionY, l.characterPositionX].texture;

                    l.characterPositionX++; // шаг на ячейку вправо

                    l.objects[l.characterPositionY, l.characterPositionX].texture = MazeObject.images[4]; // поменяли текстуру
                    l.images[l.characterPositionY, l.characterPositionX].BackgroundImage = l.objects[l.characterPositionY, l.characterPositionX].texture;

                    medalsCollected++; // увеличиваем счётчик собранных медалей
                    Console.Beep();

                    // заканчиваем игру сообщением о победе, если собрали все медали
                    if (l.totalMedalsGenerated == medalsCollected)
                    {
                        Console.Beep();
                        MessageBox.Show("YOU WIN, ALL MEDALS COLLECTED", "Congrats", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    }

                    Text = $"Maze  Coordinates: x = {l.characterPositionX}, y = {l.characterPositionY}, Health: {healthNow}, {medalsCollected} of {l.totalMedalsGenerated} medals collected, {enemiesKilled} of {l.totalEnemiesGenerated} enemies killed";

                    // ячека перестаёт быть медалью не только внешне, но и по типу
                    l.objects[l.characterPositionY, l.characterPositionX].type = MazeObject.MazeObjectType.HALL;
                }

                // проверка на то, если следующая ячейка враг
                else if (l.objects[l.characterPositionY, l.characterPositionX + 1].type == MazeObject.MazeObjectType.ENEMY)
                {

                    l.objects[l.characterPositionY, l.characterPositionX].texture = MazeObject.images[0]; // поменяли текстуру, где был герой на корридор
                    l.images[l.characterPositionY, l.characterPositionX].BackgroundImage = l.objects[l.characterPositionY, l.characterPositionX].texture;

                    l.characterPositionX++; // шаг на ячейку вправо

                    l.objects[l.characterPositionY, l.characterPositionX].texture = MazeObject.images[4]; // поменяли текстуру
                    l.images[l.characterPositionY, l.characterPositionX].BackgroundImage = l.objects[l.characterPositionY, l.characterPositionX].texture;

                    healthNow -= 20; // уменьшаем здоровье, если занимаем ячейку с врагом
                    enemiesKilled++; // увеличиваем счётчик убитых врагов, если занимаем ячейку с врагом
                    Console.Beep();

                    // если здоровье достигает нуля - выводим сообщение о проигрыше
                    if (healthNow <= 0)
                    {
                        Console.Beep();
                        MessageBox.Show("YOU DIED, YOUR HEALTH FINISHED", "Game over", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    }

                    Text = $"Maze  Coordinates: x = {l.characterPositionX}, y = {l.characterPositionY}, Health: {healthNow}, {medalsCollected} of {l.totalMedalsGenerated} medals collected, {enemiesKilled} of {l.totalEnemiesGenerated} enemies killed";

                    // ячека перестаёт быть врагом не только внешне, но и по типу
                    l.objects[l.characterPositionY, l.characterPositionX].type = MazeObject.MazeObjectType.HALL;
                }

                // проверка на то, если следующая ячейка аптечка
                else if (l.objects[l.characterPositionY, l.characterPositionX + 1].type == MazeObject.MazeObjectType.HEALTH)
                {
                    // если здоровье героя 100 и выше, значит не можем взять аптечку, с точки зрения логики, она рассматривается как стена
                    // если здоровье меньше 100, значит можно взять аптечку, и увеличить здоровье
                    if (healthNow < 100)
                    {
                        l.objects[l.characterPositionY, l.characterPositionX].texture = MazeObject.images[0]; // поменяли текстуру, где был герой на корридор
                        l.images[l.characterPositionY, l.characterPositionX].BackgroundImage = l.objects[l.characterPositionY, l.characterPositionX].texture;

                        l.characterPositionX++; // шаг на ячейку вправо

                        l.objects[l.characterPositionY, l.characterPositionX].texture = MazeObject.images[4]; // поменяли текстуру
                        l.images[l.characterPositionY, l.characterPositionX].BackgroundImage = l.objects[l.characterPositionY, l.characterPositionX].texture;
                                                
                        healthNow += 20; // увеличиваем здоровье на 20 после занятия ячейки с аптечкой                     
                        Console.Beep();                        

                        Text = $"Maze  Coordinates: x = {l.characterPositionX}, y = {l.characterPositionY}, Health: {healthNow}, {medalsCollected} of {l.totalMedalsGenerated} medals collected, {enemiesKilled} of {l.totalEnemiesGenerated} enemies killed";

                        // ячека перестаёт быть врагом не только внешне, но и по типу
                        l.objects[l.characterPositionY, l.characterPositionX].type = MazeObject.MazeObjectType.HALL;
                    }
                }
            }

            // комментарии ниже аналогичны, и частично не дублируются

            else if (e.KeyCode == Keys.Left)
            {
                // проверка на то, свободна ли ячейка
                if (l.objects[l.characterPositionY, l.characterPositionX - 1].type == MazeObject.MazeObjectType.HALL)
                {
                    l.objects[l.characterPositionY, l.characterPositionX].texture = MazeObject.images[0]; // поменяли текстуру, где был герой на корридор
                    l.images[l.characterPositionY, l.characterPositionX].BackgroundImage = l.objects[l.characterPositionY, l.characterPositionX].texture;

                    l.characterPositionX--;

                    l.objects[l.characterPositionY, l.characterPositionX].texture = MazeObject.images[4]; // поменяли текстуру
                    l.images[l.characterPositionY, l.characterPositionX].BackgroundImage = l.objects[l.characterPositionY, l.characterPositionX].texture;
                    Text = $"Maze  Coordinates: x = {l.characterPositionX}, y = {l.characterPositionY}, Health: {healthNow}, {medalsCollected} of {l.totalMedalsGenerated} medals collected, {enemiesKilled} of {l.totalEnemiesGenerated} enemies killed";
                }

                // проверка на то, если следующая ячейка приз (медаль в данном контексте)
                else if (l.objects[l.characterPositionY, l.characterPositionX - 1].type == MazeObject.MazeObjectType.MEDAL)
                {
                    l.objects[l.characterPositionY, l.characterPositionX].texture = MazeObject.images[0]; // поменяли текстуру, где был герой на корридор
                    l.images[l.characterPositionY, l.characterPositionX].BackgroundImage = l.objects[l.characterPositionY, l.characterPositionX].texture;

                    l.characterPositionX--;

                    l.objects[l.characterPositionY, l.characterPositionX].texture = MazeObject.images[4]; // поменяли текстуру
                    l.images[l.characterPositionY, l.characterPositionX].BackgroundImage = l.objects[l.characterPositionY, l.characterPositionX].texture;

                    medalsCollected++;
                    Console.Beep();

                    if (l.totalMedalsGenerated == medalsCollected)
                    {
                        Console.Beep();
                        MessageBox.Show("YOU WIN, ALL MEDALS COLLECTED", "Congrats", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    }

                    Text = $"Maze  Coordinates: x = {l.characterPositionX}, y = {l.characterPositionY}, Health: {healthNow}, {medalsCollected} of {l.totalMedalsGenerated} medals collected, {enemiesKilled} of {l.totalEnemiesGenerated} enemies killed";
                    
                    // ячека перестаёт быть медалью не только внешне, но и по типу
                    l.objects[l.characterPositionY, l.characterPositionX].type = MazeObject.MazeObjectType.HALL;
                }

                // проверка на то, если следующая ячейка враг
                else if (l.objects[l.characterPositionY, l.characterPositionX - 1].type == MazeObject.MazeObjectType.ENEMY)
                {
                    l.objects[l.characterPositionY, l.characterPositionX].texture = MazeObject.images[0]; // поменяли текстуру, где был герой на корридор
                    l.images[l.characterPositionY, l.characterPositionX].BackgroundImage = l.objects[l.characterPositionY, l.characterPositionX].texture;

                    l.characterPositionX--;

                    l.objects[l.characterPositionY, l.characterPositionX].texture = MazeObject.images[4]; // поменяли текстуру
                    l.images[l.characterPositionY, l.characterPositionX].BackgroundImage = l.objects[l.characterPositionY, l.characterPositionX].texture;

                    healthNow -= 20;
                    enemiesKilled++;
                    Console.Beep();

                    if (healthNow <= 0)
                    {
                        Console.Beep();
                        MessageBox.Show("YOU DIED, YOUR HEALTH FINISHED", "Game over", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    }

                    Text = $"Maze  Coordinates: x = {l.characterPositionX}, y = {l.characterPositionY}, Health: {healthNow}, {medalsCollected} of {l.totalMedalsGenerated} medals collected, {enemiesKilled} of {l.totalEnemiesGenerated} enemies killed";

                    // ячека перестаёт быть врагом не только внешне, но и по типу
                    l.objects[l.characterPositionY, l.characterPositionX].type = MazeObject.MazeObjectType.HALL;
                }

                // проверка на то, если следующая ячейка аптечка                
                else if (l.objects[l.characterPositionY, l.characterPositionX - 1].type == MazeObject.MazeObjectType.HEALTH)
                {
                    if (healthNow < 100)
                    {
                        l.objects[l.characterPositionY, l.characterPositionX].texture = MazeObject.images[0]; // поменяли текстуру, где был герой на корридор
                        l.images[l.characterPositionY, l.characterPositionX].BackgroundImage = l.objects[l.characterPositionY, l.characterPositionX].texture;

                        l.characterPositionX--;

                        l.objects[l.characterPositionY, l.characterPositionX].texture = MazeObject.images[4]; // поменяли текстуру
                        l.images[l.characterPositionY, l.characterPositionX].BackgroundImage = l.objects[l.characterPositionY, l.characterPositionX].texture;

                        healthNow += 20;
                        Console.Beep();

                        Text = $"Maze  Coordinates: x = {l.characterPositionX}, y = {l.characterPositionY}, Health: {healthNow}, {medalsCollected} of {l.totalMedalsGenerated} medals collected, {enemiesKilled} of {l.totalEnemiesGenerated} enemies killed";

                        // ячейка перестаёт быть аптечкой не только внешне, но и по типу
                        l.objects[l.characterPositionY, l.characterPositionX].type = MazeObject.MazeObjectType.HALL;
                    }
                }
            }
            else if (e.KeyCode == Keys.Down)
            {
                // проверка на то, свободна ли ячейка
                if (l.objects[l.characterPositionY + 1, l.characterPositionX].type == MazeObject.MazeObjectType.HALL)
                {
                    l.objects[l.characterPositionY, l.characterPositionX].texture = MazeObject.images[0]; // поменяли текстуру, где был герой на корридор
                    l.images[l.characterPositionY, l.characterPositionX].BackgroundImage = l.objects[l.characterPositionY, l.characterPositionX].texture;

                    l.characterPositionY++;

                    l.objects[l.characterPositionY, l.characterPositionX].texture = MazeObject.images[4]; // поменяли текстуру
                    l.images[l.characterPositionY, l.characterPositionX].BackgroundImage = l.objects[l.characterPositionY, l.characterPositionX].texture;

                    Text = $"Maze  Coordinates: x = {l.characterPositionX}, y = {l.characterPositionY}, Health: {healthNow}, {medalsCollected} of {l.totalMedalsGenerated} medals collected, {enemiesKilled} of {l.totalEnemiesGenerated} enemies killed";
                }

                // проверка на то, если следующая ячейка приз (медаль в данном контексте)
                else if (l.objects[l.characterPositionY + 1, l.characterPositionX].type == MazeObject.MazeObjectType.MEDAL)
                {
                    l.objects[l.characterPositionY, l.characterPositionX].texture = MazeObject.images[0]; // поменяли текстуру, где был герой на корридор
                    l.images[l.characterPositionY, l.characterPositionX].BackgroundImage = l.objects[l.characterPositionY, l.characterPositionX].texture;

                    l.characterPositionY++;

                    l.objects[l.characterPositionY, l.characterPositionX].texture = MazeObject.images[4]; // поменяли текстуру
                    l.images[l.characterPositionY, l.characterPositionX].BackgroundImage = l.objects[l.characterPositionY, l.characterPositionX].texture;

                    medalsCollected++;
                    Console.Beep();

                    if (l.totalMedalsGenerated == medalsCollected)
                    {
                        Console.Beep();
                        MessageBox.Show("YOU WIN, ALL MEDALS COLLECTED", "Congrats", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    }

                    Text = $"Maze  Coordinates: x = {l.characterPositionX}, y = {l.characterPositionY}, Health: {healthNow}, {medalsCollected} of {l.totalMedalsGenerated} medals collected, {enemiesKilled} of {l.totalEnemiesGenerated} enemies killed";

                    // ячека перестаёт быть медалью не только внешне, но и по типу
                    l.objects[l.characterPositionY, l.characterPositionX].type = MazeObject.MazeObjectType.HALL;
                }

                // проверка на то, если следующая ячейка враг
                else if (l.objects[l.characterPositionY + 1, l.characterPositionX].type == MazeObject.MazeObjectType.ENEMY)
                {
                    l.objects[l.characterPositionY, l.characterPositionX].texture = MazeObject.images[0]; // поменяли текстуру, где был герой на корридор
                    l.images[l.characterPositionY, l.characterPositionX].BackgroundImage = l.objects[l.characterPositionY, l.characterPositionX].texture;

                    l.characterPositionY++;

                    l.objects[l.characterPositionY, l.characterPositionX].texture = MazeObject.images[4]; // поменяли текстуру
                    l.images[l.characterPositionY, l.characterPositionX].BackgroundImage = l.objects[l.characterPositionY, l.characterPositionX].texture;

                    healthNow -= 20;
                    enemiesKilled++;
                    Console.Beep();

                    if (healthNow <= 0)
                    {
                        Console.Beep();
                        MessageBox.Show("YOU DIED, YOUR HEALTH FINISHED", "Game over", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    }

                    Text = $"Maze  Coordinates: x = {l.characterPositionX}, y = {l.characterPositionY}, Health: {healthNow}, {medalsCollected} of {l.totalMedalsGenerated} medals collected, {enemiesKilled} of {l.totalEnemiesGenerated} enemies killed";

                    // ячека перестаёт быть врагом не только внешне, но и по типу
                    l.objects[l.characterPositionY, l.characterPositionX].type = MazeObject.MazeObjectType.HALL;
                }

                // проверка на то, если следующая ячейка аптечка                
                else if (l.objects[l.characterPositionY + 1, l.characterPositionX].type == MazeObject.MazeObjectType.HEALTH)
                {
                    if (healthNow < 100)
                    {
                        l.objects[l.characterPositionY, l.characterPositionX].texture = MazeObject.images[0]; // поменяли текстуру, где был герой на корридор
                        l.images[l.characterPositionY, l.characterPositionX].BackgroundImage = l.objects[l.characterPositionY, l.characterPositionX].texture;

                        l.characterPositionY++;

                        l.objects[l.characterPositionY, l.characterPositionX].texture = MazeObject.images[4]; // поменяли текстуру
                        l.images[l.characterPositionY, l.characterPositionX].BackgroundImage = l.objects[l.characterPositionY, l.characterPositionX].texture;

                        healthNow += 20;
                        Console.Beep();                        

                        Text = $"Maze  Coordinates: x = {l.characterPositionX}, y = {l.characterPositionY}, Health: {healthNow}, {medalsCollected} of {l.totalMedalsGenerated} medals collected, {enemiesKilled} of {l.totalEnemiesGenerated} enemies killed";

                        // ячейка перестаёт быть аптечкой не только внешне, но и по типу
                        l.objects[l.characterPositionY, l.characterPositionX].type = MazeObject.MazeObjectType.HALL;
                    }
                }
            }

            else if (e.KeyCode == Keys.Up)
            {
                // проверка на то, свободна ли ячейка
                if (l.objects[l.characterPositionY - 1, l.characterPositionX].type == MazeObject.MazeObjectType.HALL)
                {
                    l.objects[l.characterPositionY, l.characterPositionX].texture = MazeObject.images[0]; // поменяли текстуру, где был герой на корридор
                    l.images[l.characterPositionY, l.characterPositionX].BackgroundImage = l.objects[l.characterPositionY, l.characterPositionX].texture;

                    l.characterPositionY--;

                    l.objects[l.characterPositionY, l.characterPositionX].texture = MazeObject.images[4]; // поменяли текстуру
                    l.images[l.characterPositionY, l.characterPositionX].BackgroundImage = l.objects[l.characterPositionY, l.characterPositionX].texture;
                    Text = $"Maze  Coordinates: x = {l.characterPositionX}, y = {l.characterPositionY}, Health: {healthNow}, {medalsCollected} of {l.totalMedalsGenerated} medals collected, {enemiesKilled} of {l.totalEnemiesGenerated} enemies killed";
                }

                // проверка на то, если следующая ячейка приз (медаль в данном контексте)
                if (l.objects[l.characterPositionY - 1, l.characterPositionX].type == MazeObject.MazeObjectType.MEDAL)
                {
                    l.objects[l.characterPositionY, l.characterPositionX].texture = MazeObject.images[0]; // поменяли текстуру, где был герой на корридор
                    l.images[l.characterPositionY, l.characterPositionX].BackgroundImage = l.objects[l.characterPositionY, l.characterPositionX].texture;

                    l.characterPositionY--;

                    l.objects[l.characterPositionY, l.characterPositionX].texture = MazeObject.images[4]; // поменяли текстуру
                    l.images[l.characterPositionY, l.characterPositionX].BackgroundImage = l.objects[l.characterPositionY, l.characterPositionX].texture;

                    medalsCollected++;
                    Console.Beep();

                    if (l.totalMedalsGenerated == medalsCollected)
                    {
                        Console.Beep();
                        MessageBox.Show("YOU WIN, ALL MEDALS COLLECTED","Congrats",MessageBoxButtons.OK,MessageBoxIcon.Stop);
                    }

                    Text = $"Maze  Coordinates: x = {l.characterPositionX}, y = {l.characterPositionY}, Health: {healthNow}, {medalsCollected} of {l.totalMedalsGenerated} medals collected, {enemiesKilled} of {l.totalEnemiesGenerated} enemies killed";

                    // ячека перестаёт быть медалью не только внешне, но и по типу
                    l.objects[l.characterPositionY, l.characterPositionX].type = MazeObject.MazeObjectType.HALL;
                }

                // проверка на то, если следующая ячейка враг
                if (l.objects[l.characterPositionY - 1, l.characterPositionX].type == MazeObject.MazeObjectType.ENEMY)
                {
                    l.objects[l.characterPositionY, l.characterPositionX].texture = MazeObject.images[0]; // поменяли текстуру, где был герой на корридор
                    l.images[l.characterPositionY, l.characterPositionX].BackgroundImage = l.objects[l.characterPositionY, l.characterPositionX].texture;

                    l.characterPositionY--;

                    l.objects[l.characterPositionY, l.characterPositionX].texture = MazeObject.images[4]; // поменяли текстуру
                    l.images[l.characterPositionY, l.characterPositionX].BackgroundImage = l.objects[l.characterPositionY, l.characterPositionX].texture;

                    healthNow -= 20;
                    enemiesKilled++;
                    Console.Beep();

                    if (healthNow <= 0)
                    {
                        Console.Beep();
                        MessageBox.Show("YOU DIED, YOUR HEALTH FINISHED", "Game over", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    }

                    Text = $"Maze  Coordinates: x = {l.characterPositionX}, y = {l.characterPositionY}, Health: {healthNow}, {medalsCollected} of {l.totalMedalsGenerated} medals collected, {enemiesKilled} of {l.totalEnemiesGenerated} enemies killed";

                    // ячека перестаёт быть врагом не только внешне, но и по типу
                    l.objects[l.characterPositionY, l.characterPositionX].type = MazeObject.MazeObjectType.HALL;
                }

                // проверка на то, если следующая ячейка аптечка
                if (l.objects[l.characterPositionY - 1, l.characterPositionX].type == MazeObject.MazeObjectType.HEALTH)
                {
                    if (healthNow < 100)
                    {
                        l.objects[l.characterPositionY, l.characterPositionX].texture = MazeObject.images[0]; // поменяли текстуру, где был герой на корридор
                        l.images[l.characterPositionY, l.characterPositionX].BackgroundImage = l.objects[l.characterPositionY, l.characterPositionX].texture;

                        l.characterPositionY--;

                        l.objects[l.characterPositionY, l.characterPositionX].texture = MazeObject.images[4]; // поменяли текстуру
                        l.images[l.characterPositionY, l.characterPositionX].BackgroundImage = l.objects[l.characterPositionY, l.characterPositionX].texture;

                        healthNow += 20;                        
                        Console.Beep();                        

                        Text = $"Maze  Coordinates: x = {l.characterPositionX}, y = {l.characterPositionY}, Health: {healthNow}, {medalsCollected} of {l.totalMedalsGenerated} medals collected, {enemiesKilled} of {l.totalEnemiesGenerated} enemies killed";

                        // ячека перестаёт быть врагом не только внешне, но и по типу
                        l.objects[l.characterPositionY, l.characterPositionX].type = MazeObject.MazeObjectType.HALL;
                    }
                }
            }
        }
    }
}
