using System.Collections.Generic;
using System.Windows.Media;
using System.Windows.Shapes;

namespace prolab_2
{
    public class SorcererTower : Tower
    {
        public SorcererTower(string id, double x, double y)
            // Maliyet: 200, Menzil: 200, Hasar: 40, Hız: 1.0s
            // Not: Atış hızı yavaştır ama zırhı yok sayar.
            : base(id, "Sorcerer Tower", x, y, 90, 200, 30, 1.0)
        {
            // Görünüm: Mor renkli, mistik bir daire
            Appearance = new Ellipse
            {
                Width = 50,
                Height = 50,
                Fill = Brushes.Purple,
                Stroke = Brushes.Thistle,
                StrokeThickness = 2
            };
            UpdateVisual();
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