using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls; // StackPanel ve ProgressBar için
using System.Windows.Media;
using System.Windows.Media.Imaging; 
using System.Windows.Shapes;
using WpfAnimatedGif;


namespace prolab_2
{
    public abstract class Enemy : GameObject
    {
        // --- ÖZELLİKLER ---
        public double MaxHealth { get; protected set; }
        public double CurrentHealth { get; set; }
        public int Armor { get; protected set; }
        public double Speed { get; protected set; }
        public int Bounty { get; protected set; }
        public double DamageToPlayer { get; protected set; }
        public bool IsFlying { get; protected set; }
        public virtual bool IsChampion => false;
        public List<Point> AssignedPath { get; set; }

        protected int WayPointIndex { get; set; } = 0;
        protected double CurrentSpeed;
        protected double SlowDuration;

        // --- GÖRSEL BİLEŞENLER ---
        protected Image EnemySprite;
        protected Rectangle IceOverlay;
        protected ProgressBar HealthBar;
        protected ProgressBar MaxHealthBar;
        protected StackPanel Container;
        protected Enemy(string id, string name, double x, double y, double maxHealth, int armor, double speed, int bounty, bool isFlying, double damageToPlayer) 
            : base(id, name, x, y)
        {
            MaxHealth = maxHealth;
            CurrentHealth = maxHealth;
            Armor = armor;
            Speed = speed;
            Bounty = bounty;
            DamageToPlayer = damageToPlayer;
            IsFlying = isFlying;
            CurrentSpeed = speed;
        }

        // --- GÖRSEL KURULUMU ---
        protected void SetupVisual()
        {
            double spriteSize = 40;
            Grid healthLayer = new Grid();
            healthLayer.Width = spriteSize;
            healthLayer.Height = IsChampion ? 10 : 6;

            // MAKSİMUM CAN BARI
            MaxHealthBar = new ProgressBar();
            MaxHealthBar.Minimum = 0;
            MaxHealthBar.Maximum = MaxHealth;
            MaxHealthBar.Value = MaxHealth;
            MaxHealthBar.Height = IsChampion ? 10 : 6;
            MaxHealthBar.Width = spriteSize;
            MaxHealthBar.Foreground = Brushes.DarkSlateGray;
            MaxHealthBar.Background = Brushes.Black;
            MaxHealthBar.BorderBrush = Brushes.Black;
            MaxHealthBar.BorderThickness = new Thickness(1);

            Container = new StackPanel();
            Container.Width = spriteSize;

            // CAN BARI
            HealthBar = new ProgressBar();
            HealthBar.Minimum = 0;
            HealthBar.Maximum = MaxHealth;
            HealthBar.Value = CurrentHealth;
            HealthBar.Height = IsChampion ? 10 : 6;
            HealthBar.Width = spriteSize;
            HealthBar.Foreground = Brushes.Red;
            HealthBar.Background = Brushes.Transparent;

            healthLayer.Children.Add(MaxHealthBar); // ALTTA
            healthLayer.Children.Add(HealthBar);    // ÜSTTE

            HealthBar.BorderThickness = new Thickness(1);
            HealthBar.Margin = new Thickness(0, 0, 0, 2);


            Grid imageLayer = new Grid();
            imageLayer.Width = spriteSize;
            imageLayer.Height = spriteSize;

            EnemySprite = new Image();
            EnemySprite.Width = spriteSize;
            EnemySprite.Height = spriteSize;

            RenderOptions.SetBitmapScalingMode(EnemySprite, BitmapScalingMode.NearestNeighbor);

            string imagePath = $"pack://application:,,,/prolab_2;component/Images/{ImageName}.gif";
            var imageUri = new Uri(imagePath);
            var bitmap = new BitmapImage(imageUri);

            ImageBehavior.SetAnimatedSource(EnemySprite, bitmap);

            IceOverlay = new Rectangle();
            IceOverlay.Width = spriteSize;
            IceOverlay.Height = spriteSize;
            IceOverlay.Fill = new SolidColorBrush(Color.FromArgb(140, 0, 160, 255));
            IceOverlay.Visibility = Visibility.Hidden;

            imageLayer.Children.Add(EnemySprite);  // ALTTA
            imageLayer.Children.Add(IceOverlay);   // ÜSTTE

            Container.Children.Add(healthLayer);
            Container.Children.Add(imageLayer);

            this.Appearance = Container;

            UpdateEnemyVisual();
        }

        public void TakeDamage(double rawDamage)
        {
            double damageReduction = 1.0 - (Armor / (Armor + 100.0));
            double netDamage = rawDamage * damageReduction;

            CurrentHealth -= netDamage;
            if (CurrentHealth < 0) CurrentHealth = 0;

            UpdateHealthBar();
        }

        protected void UpdateHealthBar()
        {
            if (HealthBar != null)
                HealthBar.Value = CurrentHealth;
        }

        public void ApplySlow(double duration, double percentage)
        {
            CurrentSpeed = Speed * (1.0 - percentage);
            SlowDuration = duration;
            SetFrozenVisual(true);
        }

        public bool Move(List<Point> path, double deltaTime)
        {
            if (WayPointIndex >= path.Count) return true;

            Point target = path[WayPointIndex];
            double step = CurrentSpeed * deltaTime * 10;

            double dx = target.X - Location.X;
            double dy = target.Y - Location.Y;
            double distance = Math.Sqrt(dx * dx + dy * dy);

            if (distance <= step)
            {
                Location = target;
                WayPointIndex++;
                return WayPointIndex >= path.Count;
            }
            else
            {
                double nx = dx / distance;
                double ny = dy / distance;
                Location = new Point(Location.X + nx * step, Location.Y + ny * step);
                return false;
            }
        }

        public override void Update(double deltaTime)
        {
            if (SlowDuration > 0)
            {
                SlowDuration -= deltaTime;
                if (SlowDuration <= 0) {  
                CurrentSpeed = Speed;
                SetFrozenVisual(false);
                }
            }

            UpdateEnemyVisual();
        }

        // --- GÖRSEL GÜNCELLEME ---
        protected void UpdateEnemyVisual()
        {
            if (Container == null) return;

            // 1. Konumu Ayarla
            Canvas.SetLeft(Container, Location.X - Container.ActualWidth / 2);
            Canvas.SetTop(Container, Location.Y - Container.ActualHeight / 2);

            // 2. Derinlik
            Canvas.SetZIndex(Container, 999);
        }
        public virtual string ImageName
        {
            get { return "default_enemy"; } // Eğer özel resim yoksa bunu kullanır
        }
        protected void SetFrozenVisual(bool isFrozen)
        {
            if (IceOverlay == null) return;

            if (isFrozen)
                IceOverlay.Visibility = Visibility.Visible;
            else
                IceOverlay.Visibility = Visibility.Hidden;
        }
    }
}