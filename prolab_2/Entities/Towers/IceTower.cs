using System.Collections.Generic;
using System.Windows.Media;
using System.Windows.Shapes;

namespace prolab_2
{
    public class IceTower : Tower
    {
        public IceTower(string id, double x, double y)
            // Ref: Maliyet=150, Hasar=0, Hız=1.5s (WPF Menzil: 120px)
            : base(id, "Ice Tower", x, y, 150, 70, 15, 2)
        {
            Appearance = new Ellipse
            {
                Width = 50,
                Height = 50,
                Fill = Brushes.Cyan,
                Opacity = 0.8,
                Stroke = Brushes.Blue
            };
            UpdateVisual();
        }

        // Metot imzası değiştiği için burayı güncelledik
        public override void Attack(Enemy target, List<Enemy> allEnemies)
        {
            if (CurrentCooldown <= 0 && target != null)
            {
                // Buz kulesi diğer düşmanlarla ilgilenmez, sadece hedefini yavaşlatır
                target.ApplySlow(3.0, 0.50); // 3 saniye %50 yavaşlat
                CurrentCooldown = FireRate;
            }
        }
    }
}