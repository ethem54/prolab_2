using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace prolab_2
{
    

    public class IceTower : Tower
    {
        private Image towerImage;

        // Durum resimleri
        private string idleImage = "pack://application:,,,/prolab_2;component/Images/ortai.png";
        private string lookLeftImage = "pack://application:,,,/prolab_2;component/Images/soli.png";
        private string lookRightImage = "pack://application:,,,/prolab_2;component/Images/sagi.png";
        public IceTower(string id, double x, double y)
            // Maliyet: 70, Menzil: 150, Hasar: 15, Hız: 2.0s
            : base(id, "BuzKulesi", x, y, 70, 150, 15, 2.0)
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

        public override void Attack(Enemy target, System.Collections.Generic.List<Enemy> allEnemies)
        {
            if (CurrentCooldown <= 0 && target != null)
            {
                target.TakeDamage(Damage);
                target.ApplySlow(3.0, 0.50); // 3 saniye %50 yavaşlatma

                Logger.Log($"Kule '{this.ID}', '{target.ID}' 'ye slow attı.");
                CurrentCooldown = FireRate;
            }
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