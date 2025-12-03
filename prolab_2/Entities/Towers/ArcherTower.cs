using System;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace prolab_2
{
    public class ArcherTower : Tower
    {
        public override void Attack(Enemy target, System.Collections.Generic.List<Enemy> allEnemies)
        {
            if (CurrentCooldown <= 0 && target != null)
            {
                double finalDamage = Damage;

                if (target is Knight)
                {
                    finalDamage = finalDamage * 0.5;
                }

                target.TakeDamage(finalDamage);
                CurrentCooldown = FireRate;

                // Loglama
                Logger.Log($"Kule '{this.ID}', '{target.ID}' hedefine atış yaptı. Hasar: {finalDamage}");
            }
        }
        private Image towerImage;

        // Durum resimleri
        private string idleImage = "pack://application:,,,/prolab_2;component/Images/orta.png";
        private string lookLeftImage = "pack://application:,,,/prolab_2;component/Images/sol.png";
        private string lookRightImage = "pack://application:,,,/prolab_2;component/Images/sag.png";

        public ArcherTower(string id, double x, double y)
            : base(id, "OkcuKulesi", x, y, 50, 150, 10, 0.2)
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
                Source = new BitmapImage(new Uri(idleImage)),
                Stretch = Stretch.Uniform
            };

            container.Children.Add(towerImage);
            this.Appearance = container;

            UpdateVisual();
        }
        public override void UpdateTowerImage(bool enemyLeft, bool enemyRight)
        {
            if (enemyLeft)
                towerImage.Source = new BitmapImage(new Uri(lookLeftImage));
            else if (enemyRight)
                towerImage.Source = new BitmapImage(new Uri(lookRightImage));
            else
                towerImage.Source = new BitmapImage(new Uri(idleImage));
        }
    }
}