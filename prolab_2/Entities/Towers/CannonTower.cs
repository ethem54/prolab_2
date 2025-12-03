using System;
using System.Collections.Generic;
using System.Linq; // Where için
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace prolab_2
{
    public class CannonTower : Tower
    {
        private Image towerImage;

        // Durum resimleri
        private string lookLeftImage = "pack://application:,,,/prolab_2;component/Images/solc.png";
        private string lookRightImage = "pack://application:,,,/prolab_2;component/Images/sagc.png";
        public CannonTower(string id, double x, double y)
            // Maliyet: 75, Menzil: 200, Hasar: 20, Hız: 3.0s
            : base(id, "TopcuKulesi", x, y, 75, 200, 20, 3.0)
        {
            Grid container = new Grid
            {
                Width = 120,
                Height = 120
            };
            towerImage = new Image
            {
                Width = 120,
                Height = 120,
                Source = new BitmapImage(new Uri(lookRightImage)),
                Stretch = Stretch.Uniform
            };
            container.Children.Add(towerImage);
            this.Appearance = container;

            UpdateVisual();
        }
        public override Enemy GetTarget(System.Collections.Generic.List<Enemy> enemies)
        {
            return enemies
                .Where(e => !(e is FlyingMonster) && CalculateDistance(e) <= Range && e.CurrentHealth > 0)
                .OrderBy(e => CalculateDistance(e)) // en yakın düşmana hedef alıyor
                .FirstOrDefault();
        }

        public override void Attack(Enemy target, System.Collections.Generic.List<Enemy> allEnemies)
        {
            if (CurrentCooldown <= 0 && target != null)
            {
                var center = target.Location;
                double radius = 50;

                foreach (var enemy in allEnemies.ToList()) // ToList() hata almamak için
                {
                    if (enemy is FlyingMonster) continue; // Uçanlara alan hasarı da vurmaz

                    double dist = System.Math.Sqrt(System.Math.Pow(enemy.Location.X - center.X, 2) + System.Math.Pow(enemy.Location.Y - center.Y, 2));
                    if (dist <= radius)
                    {
                        enemy.TakeDamage(Damage);
                    }
                }

                Logger.Log($"Kule '{this.ID}' odaklı alan hasarı vurdu. Merkez: '{target.ID}'");
                CurrentCooldown = FireRate;
            }
        }
        public override void UpdateTowerImage(bool enemyLeft, bool enemyRight)
        {
            if (enemyLeft)
                towerImage.Source = new BitmapImage(new Uri(lookLeftImage));
            else
                towerImage.Source = new BitmapImage(new Uri(lookRightImage));
        }
    }
}