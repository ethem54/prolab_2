using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace prolab_2
{
    public abstract class Enemy
    {
        public string Type { get; set; }
        public double Health { get; set; }
        public double Armor { get; set; }
        public double ID { get; set; }
        public bool IsFlying { get; set; }

        protected Enemy(string type, double id, double health, double armor)
        {
            this.Type = type;
            this.ID = id;
            this.Health = health;
            this.Armor = armor;
        }

        public void TakeDamage(double damage)
        {
            double gettingDamage = damage; // Formül sonra eklenecek
        }
    }
    public class SimpleEnemy
    {
        public Rectangle Shape;
        public int CurrentWaypoint = 0;
        public double Speed = 1.5;
        public List<Point> Path;
    }
    public abstract class Tower
    {
        public string Type { get; set; }
        public double Power { get; set; }
        public double AttackSpeed { get; set; }
        public double Range { get; set; }
        public bool IsSlow { get; set; }

        protected Tower(double power, double range)
        {
            this.Power = power;
            this.Range = range;
        }
    }

    public class GameManager
    {
        // değişkenlerin olduğu yer
        public double PlayerHealth { get; set; } = 100;
        public double Gold { get; set; } = 200;
        public double wave = 0;

        public List<Enemy> Enemies { get; private set; } = new List<Enemy>();
        public List<Tower> Towers { get; private set; } = new List<Tower>();
        public List<Point> PathWaypoints { get; private set; } = new List<Point>();
    }

    public partial class MainWindow : Window
    {
        DispatcherTimer gameLoop;
        DispatcherTimer buttonTimer;
        List<Point> PathFromRight = new List<Point>();
        List<Point> PathFromTop = new List<Point>();
        List<SimpleEnemy> enemies = new List<SimpleEnemy>();

        bool secondSpawnUnlocked = false;
        GameManager GM = new GameManager();
        public MainWindow()
        {
            InitializeComponent();

            gameLoop = new DispatcherTimer();
            gameLoop.Interval = TimeSpan.FromMilliseconds(9);
            gameLoop.Tick += GameLoop_Tick;

            buttonTimer = new DispatcherTimer();
            buttonTimer.Interval = TimeSpan.FromSeconds(5);
            buttonTimer.Tick += ButtonTimer_Tick;

            PathFromRight.Add(new Point(950, 200));
            PathFromRight.Add(new Point(700, 250));
            PathFromRight.Add(new Point(400, 300));
            PathFromRight.Add(new Point(150, 300));
            PathFromRight.Add(new Point(150, 400));

            PathFromTop.Add(new Point(500, 10));
            PathFromTop.Add(new Point(500, 200));
            PathFromTop.Add(new Point(400, 300));
            PathFromTop.Add(new Point(150, 300));
            PathFromTop.Add(new Point(150, 400));
        }
        void SpawnEnemy(List<Point> path)
        {
            SimpleEnemy enemy = new SimpleEnemy();

            enemy.Shape = new Rectangle
            {
                Width = 30,
                Height = 30,
                Fill = Brushes.Red
            };

            enemy.Path = path;
            enemy.CurrentWaypoint = 0;

            Canvas.SetLeft(enemy.Shape, path[0].X);
            Canvas.SetTop(enemy.Shape, path[0].Y);

            enemies.Add(enemy);
            GameCanvas.Children.Add(enemy.Shape);
        }
        private void GameLoop_Tick(object sender, EventArgs e)
        {
            for (int i = enemies.Count - 1; i >= 0; i--)
            {
                SimpleEnemy enemy = enemies[i];
                Point target = enemy.Path[enemy.CurrentWaypoint];

                double x = Canvas.GetLeft(enemy.Shape);
                double y = Canvas.GetTop(enemy.Shape);

                double dx = target.X - x;
                double dy = target.Y - y;

                double distance = Math.Sqrt(dx * dx + dy * dy);

                if (distance < 2)
                {
                    enemy.CurrentWaypoint++;

                    if (enemy.CurrentWaypoint >= enemy.Path.Count)
                    {
                        GM.PlayerHealth -= 10;
                        GameCanvas.Children.Remove(enemy.Shape);
                        enemies.RemoveAt(i);
                        continue;
                    }
                }
                else
                {
                    double nx = dx / distance;
                    double ny = dy / distance;

                    Canvas.SetLeft(enemy.Shape, x + nx * enemy.Speed);
                    Canvas.SetTop(enemy.Shape, y + ny * enemy.Speed);
                }
            }
        }
        private void btnEnemy_Click(object sender, RoutedEventArgs e)
        {
            SpawnEnemy(PathFromRight);

            btnEnemy.Visibility = Visibility.Hidden;
            btnEnemy2.Visibility = Visibility.Hidden;

            if (!secondSpawnUnlocked)
            {
                secondSpawnUnlocked = true;
            }

            buttonTimer.Start();
            if(!gameLoop.IsEnabled)
            {
                gameLoop.Start();
            }
        }
        private void btnEnemy2_Click(object sender, RoutedEventArgs e)
        {
            SpawnEnemy(PathFromTop);
            if (!gameLoop.IsEnabled)
            {
                gameLoop.Start();
            }
        }
        private void ButtonTimer_Tick(object sender, EventArgs e)
        {
            buttonTimer.Stop();
            btnEnemy.Visibility = Visibility.Visible;
            btnEnemy2.Visibility = Visibility.Visible;
        }

    }

}
    