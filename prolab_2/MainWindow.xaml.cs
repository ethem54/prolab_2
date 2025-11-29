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

        public double Speed { get; set; } = 1.5;
        public bool IsFlying { get; set; }

        public Shape rectangle { get; set; }
        public List<Point> Path { get; set; }
        public int CurrentWaypoint { get; set; } = 0;

        protected Enemy(string type, double id, double health)
        {
            Type = type;
            ID = id;
            Health = health;
        }

        public void TakeDamage(double damage)
        {
            double realDamage = Math.Max(0, damage - Armor);
            Health -= realDamage;
        }
    }
    public class ArmoredEnemy : Enemy
    {
        public ArmoredEnemy(double id) : base ("armored", id, 100)
        {
            Armor = 10;
            IsFlying = false;

            rectangle = new Rectangle
            {
                Height = 40,
                Width = 40,
                Fill = Brushes.Red
            };
        }
    }
    public abstract class Tower
    {
        public string Name { get; set; }
        public double Power { get; set; }
        public double Attackspeed { get; set; }
        public double Range { get; set; }
        public bool IsSlow { get; set; }
        public int cooldown { get; set; } = 0;
        public Shape ellipse { get; set; }

        protected Tower(string name)
        {
            Name = name;
        }

        private void Attack (Enemy enemy)
        {
            enemy.TakeDamage(Power);
        }
    }
    public class StandardTower : Tower
    {
        public StandardTower(string name) : base(name)
        {
            Power = 20;
            Attackspeed = 1;
            Range = 150;

            ellipse = new Ellipse
            {
                Height = 50,
                Width = 50,
                Fill = Brushes.Blue
            };
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
        DispatcherTimer spawnTimer;

        List<Point> PathFromRight = new List<Point>();
        List<Point> PathFromTop = new List<Point>();
        List<Enemy> enemies = new List<Enemy>();

        bool secondSpawnUnlocked = false;
        bool firstTick = false;
        bool waveCheck = false;

        int currentWave = 0;      
        int rightSpawned = 0;
        int topSpawned = 0;
        int wave1Count = 5;       
        int wave2EachCount = 5;

        
        GameManager GM = new GameManager();

        public MainWindow()
        {
            InitializeComponent();

            PlayerHealthBar.Value = GM.PlayerHealth;
            HealthText.Text = "Health: " + GM.PlayerHealth;
            gameLoop = new DispatcherTimer();
            gameLoop.Interval = TimeSpan.FromMilliseconds(16);
            gameLoop.Tick += GameLoop_Tick;

            buttonTimer = new DispatcherTimer();
            buttonTimer.Interval = TimeSpan.FromSeconds(5);
            buttonTimer.Tick += ButtonTimer_Tick;

            spawnTimer = new DispatcherTimer();
            spawnTimer.Interval = TimeSpan.FromSeconds(2);
            spawnTimer.Tick += SpawnTimer_Tick;

            PathFromRight.Add(new Point(950, 200));
            PathFromRight.Add(new Point(700, 250));
            PathFromRight.Add(new Point(400, 300));
            PathFromRight.Add(new Point(150, 300));
            PathFromRight.Add(new Point(150, 400));
            
            PathFromTop.Add(new Point(500, 10));
            PathFromTop.Add(new Point(450, 200));
            PathFromTop.Add(new Point(400, 300));
            PathFromTop.Add(new Point(150, 300));
            PathFromTop.Add(new Point(150, 400));

        }

        void SpawnEnemy(List<Point> path)
        {
            Enemy enemy = new ArmoredEnemy(enemies.Count + 1);

            enemy.Path = path;
            enemy.CurrentWaypoint = 0;

            Canvas.SetLeft(enemy.rectangle, path[0].X);
            Canvas.SetTop(enemy.rectangle, path[0].Y);

            enemies.Add(enemy);
            GameCanvas.Children.Add(enemy.rectangle);
        }
        private void GameLoop_Tick(object sender, EventArgs e)
        {
            HandleTowers();
            for (int i = enemies.Count - 1; i >= 0; i--)
            {
                Enemy enemy = enemies[i];
                Point target = enemy.Path[enemy.CurrentWaypoint];

                double x = Canvas.GetLeft(enemy.rectangle);
                double y = Canvas.GetTop(enemy.rectangle);

                double centerX = x + enemy.rectangle.Width / 2;
                double centerY = y + enemy.rectangle.Height / 2;

                double dx = target.X - centerX;
                double dy = target.Y - centerY;

                double distance = Math.Sqrt(dx * dx + dy * dy);

                if (distance < 2)
                {
                    enemy.CurrentWaypoint++;

                    if (enemy.CurrentWaypoint >= enemy.Path.Count)
                    {
                        double damageToPlayer = enemy.Health / 10;
                        GM.PlayerHealth -= damageToPlayer;

                        if (GM.PlayerHealth < 0)
                        {
                            GM.PlayerHealth = 0;
                        }

                        PlayerHealthBar.Value = GM.PlayerHealth;
                        HealthText.Text = "Health: " + Math.Round(GM.PlayerHealth);
                        GameCanvas.Children.Remove(enemy.rectangle);
                        enemies.RemoveAt(i);
                        continue;
                    }
                }
                else
                {
                    double nx = dx / distance;
                    double ny = dy / distance;

                    Canvas.SetLeft(enemy.rectangle, x + nx * enemy.Speed);
                    Canvas.SetTop(enemy.rectangle, y + ny * enemy.Speed);
                }
            }
        }
        private void HandleTowers()
        {
            foreach (var t in GM.Towers)
            {
                foreach (var e in enemies)
                {
                    double towerX = Canvas.GetLeft(t.ellipse) + t.ellipse.Width / 2;
                    double towerY = Canvas.GetTop(t.ellipse) + t.ellipse.Height / 2;

                    double enemyX = Canvas.GetLeft(e.rectangle) + e.rectangle.Width / 2;
                    double enemyY = Canvas.GetTop(e.rectangle) + e.rectangle.Height / 2;

                    double distance = Math.Sqrt((towerX - enemyX) * (towerX - enemyX) + (towerY - enemyY) * (towerY - enemyY));

                    if (distance <= t.Range)
                    {
                        e.TakeDamage(t.Power);
                    }
                }
            }

        // Ölü düşmanları temizle
            for (int i = enemies.Count - 1; i >= 0; i--)
            {
                if (enemies[i].Health <= 0)
                {
                    GameCanvas.Children.Remove(enemies[i].rectangle);
                    enemies.RemoveAt(i);
                }
            }
        }
        private void btnEnemy_Click(object sender, RoutedEventArgs e)
        {

            btnEnemy.Visibility = Visibility.Hidden;
            btnEnemy2.Visibility = Visibility.Hidden;
          
            if (currentWave == 0)
            {
                if (!waveCheck)
                {
                    currentWave = 1;
                }
                else
                {
                    currentWave = 2;
                }
                rightSpawned = 0;
                topSpawned = 0;
                spawnTimer.Start();
            }

            if (!secondSpawnUnlocked)
            {
                secondSpawnUnlocked = true;
            }

            if (!firstTick)
            {
                buttonTimer.Start();
            }

            if (!gameLoop.IsEnabled)
            {
                gameLoop.Start();
            }
        }
        private void btnEnemy2_Click(object sender, RoutedEventArgs e)
        {
            btnEnemy.Visibility = Visibility.Hidden;
            btnEnemy2.Visibility = Visibility.Hidden;

            if (currentWave == 0)
            {
                currentWave = 2;
                rightSpawned = 0;
                topSpawned = 0;
                spawnTimer.Start();
            }

            if (!gameLoop.IsEnabled)
            {
                gameLoop.Start();
            }
        }
        private void ButtonTimer_Tick(object sender, EventArgs e)
        {
            firstTick = true;
            buttonTimer.Stop();
            btnEnemy.Visibility = Visibility.Visible;
            btnEnemy2.Visibility = Visibility.Visible;
        }
        private void SpawnTimer_Tick(Object sender, EventArgs e)
        {
            if (currentWave == 1)
            {
                if (rightSpawned < wave1Count)
                {
                    SpawnEnemy(PathFromRight);
                    rightSpawned++;
                }
                else
                {
                    spawnTimer.Stop();
                    currentWave = 0;
                    waveCheck = true;
                }
            }
            else if (currentWave == 2)
            {
                bool spawnedSomething = false;

                if (rightSpawned < wave2EachCount)
                {
                    SpawnEnemy(PathFromRight);
                    rightSpawned++;
                    spawnedSomething = true;
                }

                if (topSpawned < wave2EachCount)
                {
                    SpawnEnemy(PathFromTop);
                    topSpawned++;
                    spawnedSomething = true;
                }

                if (!spawnedSomething || (rightSpawned >= wave2EachCount && topSpawned >= wave2EachCount))
                {
                    spawnTimer.Stop();
                    currentWave = 0;
                }
            }
        }
        private void tower_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (tower.SelectedItem == null) return;

            string selectedTower = ((ComboBoxItem)tower.SelectedItem).Content.ToString();

            Tower newTower = null;

            if (selectedTower == "StandardTower")
                newTower = new StandardTower("StandardTower");

            if (newTower != null)
            {
                // ComboBox'un konumunu al
                double x = Canvas.GetLeft(tower) + tower.Width / 2 - newTower.ellipse.Width / 2;
                double y = Canvas.GetTop(tower) + tower.Height / 2 - newTower.ellipse.Height / 2;

                Canvas.SetLeft(newTower.ellipse, x);
                Canvas.SetTop(newTower.ellipse, y);

                GM.Towers.Add(newTower);
                GameCanvas.Children.Add(newTower.ellipse);
                tower.Visibility = Visibility.Hidden;
            }
        }

    }
}
    