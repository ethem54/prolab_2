using System.Collections.Generic;
using System.Linq; // Where için
using System.Windows.Media;
using System.Windows.Shapes;

namespace prolab_2
{
    public class CannonTower : Tower
    {
        // Alan Etkisi Yarıçapı (Piksel cinsinden. Ref: 3 birim -> WPF: ~100px diyelim)
        private const double ExplosionRadius = 100;

        public CannonTower(string id, double x, double y)
            // Ref: Maliyet=250, Hasar=50, Hız=3.0s, Menzil=12 birim (~300px)
            : base(id, "Cannon Tower", x, y, 75, 300, 20, 3.0)
        {
            // Görünüm: Siyah/Gri büyük bir daire
            Appearance = new Ellipse
            {
                Width = 60,
                Height = 60,
                Fill = Brushes.Black,
                Stroke = Brushes.DarkGray,
                StrokeThickness = 3
            };
            UpdateVisual();
        }

        public override void Attack(Enemy target, List<Enemy> allEnemies)
        {
            if (CurrentCooldown <= 0 && target != null)
            {
                // Patlama merkezi hedefin olduğu yer
                var explosionCenter = target.Location;

                // Tüm düşmanları kontrol et: Patlama alanında kimler var?
                // Not: Canlı olanlara vuruyoruz.
                foreach (var enemy in allEnemies.Where(e => e.CurrentHealth > 0))
                {
                    double dist = CalculateDistance(enemy.Location, explosionCenter);

                    if (dist <= ExplosionRadius)
                    {
                        // Alan içindeki düşmana hasar ver
                        enemy.TakeDamage(Damage);
                    }
                }

                CurrentCooldown = FireRate;
            }
        }
    }
}   