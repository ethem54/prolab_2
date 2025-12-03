using System.Collections.Generic;
using System.Windows.Media;
using System.Windows.Shapes;

namespace prolab_2
{
    public class IceTower : Tower
    {
        public IceTower(string id, double x, double y)
            // Maliyet: 70, Menzil: 150, Hasar: 15, Hız: 2.0s
            : base(id, "BuzKulesi", x, y, 70, 150, 15, 2.0)
        {
            Appearance = new System.Windows.Shapes.Ellipse
            {
                Width = 40,
                Height = 40,
                Fill = System.Windows.Media.Brushes.Cyan,
                Opacity = 0.4,
                Stroke = System.Windows.Media.Brushes.Blue
            };
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
    }
}