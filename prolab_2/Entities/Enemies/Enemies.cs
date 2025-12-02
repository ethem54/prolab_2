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

        public List<Point> AssignedPath { get; set; }

        protected int WayPointIndex { get; set; } = 0;
        protected double CurrentSpeed;
        protected double SlowDuration;

        // --- GÖRSEL BİLEŞENLER (Burada tanımlı olmaları ŞART) ---
        protected ProgressBar HealthBar;
        protected StackPanel Container; // <-- İşte hatayı çözen satır bu!

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
            double spriteSize = 40; // Boyutu buradan ayarlayabilirsin

            // 1. Kapsayıcı (Kutu)
            Container = new StackPanel();
            Container.Width = spriteSize;

            // 2. Hareketli Resim (GIF) Oluşturma
            Image enemySprite = new Image();
            enemySprite.Width = spriteSize;
            enemySprite.Height = spriteSize;

            // Piksel çizim olduğu için bulanıklaşmayı engelliyoruz
            RenderOptions.SetBitmapScalingMode(enemySprite, BitmapScalingMode.NearestNeighbor);

            // --- GIF OYNATMA KISMI ---
            // Dosya uzantısını .gif yaptık
            string imagePath = $"pack://application:,,,/prolab_2;component/Images/{ImageName}.gif";

            // Kütüphaneyi kullanarak resmi yüklüyoruz
            var imageUri = new Uri(imagePath);
            var bitmap = new BitmapImage(imageUri);

            // İşte burası GIF'in oynamasını sağlayan komut:
            ImageBehavior.SetAnimatedSource(enemySprite, bitmap);

            // 3. Can Barı (Aynı kalıyor)
            HealthBar = new ProgressBar();
            HealthBar.Minimum = 0;
            HealthBar.Maximum = MaxHealth;
            HealthBar.Value = CurrentHealth;
            HealthBar.Height = 6;
            HealthBar.Width = spriteSize;
            HealthBar.Foreground = Brushes.Red;
            HealthBar.Background = Brushes.White;
            HealthBar.BorderBrush = Brushes.Black;
            HealthBar.BorderThickness = new Thickness(1);
            HealthBar.Margin = new Thickness(0, 0, 0, 2);

            // 4. Ekrana Ekleme
            Container.Children.Add(HealthBar);   // Üste bar
            Container.Children.Add(enemySprite); // Alta hareketli GIF

            // GameObject'e bildiriyoruz
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
                if (SlowDuration <= 0) CurrentSpeed = Speed;
            }

            // Pozisyonu ve Z-Index'i güncelle
            UpdateEnemyVisual();
        }

        // --- GÖRSEL GÜNCELLEME (Z-INDEX DAHİL) ---
        protected void UpdateEnemyVisual()
        {
            // Container null ise işlem yapma (Hata koruması)
            if (Container == null) return;

            // 1. Konumu Ayarla
            Canvas.SetLeft(Container, Location.X - Container.ActualWidth / 2); // Merkeze al
            Canvas.SetTop(Container, Location.Y - Container.ActualHeight / 2);

            // 2. Derinlik (Z-Index) Ayarı - Öndekiler üstte görünsün
            Canvas.SetZIndex(Container, (int)Location.Y);
        }
        public virtual string ImageName
        {
            get { return "default_enemy"; } // Eğer özel resim yoksa bunu kullanır
        }
    }
}