namespace prolab_2
{
    public class ArcherTower : Tower
    {
        public ArcherTower(string id, double x, double y)
            // Maliyet: 50, Menzil: 150, Hasar: 10, Hız: 1.0s
            : base(id, "OkcuKulesi", x, y, 50, 150, 10, 1.0)
        {
            // ... (Görünüm kodları aynı kalabilir) ...
            Appearance = new System.Windows.Shapes.Ellipse
            {
                Width = 50,
                Height = 50,
                Fill = System.Windows.Media.Brushes.Blue,
                Stroke = System.Windows.Media.Brushes.White,
                StrokeThickness = 2
            };
            UpdateVisual();
        }

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
    }
}