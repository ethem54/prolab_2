using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace prolab_2
{
    public class SorcererTower : Tower
    {
        private Image towerImage;

        private string idleImage = "pack://application:,,,/prolab_2;component/Images/ortab.png";
        private string lookLeftImage = "pack://application:,,,/prolab_2;component/Images/solb.png";
        private string lookRightImage = "pack://application:,,,/prolab_2;component/Images/sagb.png";
        public SorcererTower(string id, double x, double y)
            // Maliyet: 90, Menzil: 200, Hasar: 40, Hız: 1.0s
            // Not: Atış hızı yavaştır ama zırhı yok sayar.
            : base(id, "Sorcerer Tower", x, y, 90, 200, 20, 1.0)
        {
            // Görünüm: Mor renkli, mistik bir daire
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
        // Saldırı Mantığı (Override)
        public override void Attack(Enemy target, List<Enemy> allEnemies)
        {
            if (CurrentCooldown <= 0 && target != null)
            {
                // Zırh hesaplaması yapan TakeDamage metodunu kullanmıyoruz.
                // Direkt can değerinden düşüyoruz.

                target.CurrentHealth -= Damage;

                CurrentCooldown = FireRate;
            }
        }
    }
}