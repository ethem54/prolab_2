using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace prolab_2
{
    public abstract class Tower : GameObject
    {
        public int Cost { get; protected set; }
        public double Range { get; protected set; }
        public double Damage { get; protected set; }
        public double FireRate { get; protected set; }
        public double CurrentCooldown { get; set; } = 0;
        public double X => Location.X;
        public double Y => Location.Y;

        // Tower görüntü güncelleme için virtual method
        public virtual void UpdateTowerImage(bool enemyLeft, bool enemyRight)
        {
            //subclass override edecek
        }

        protected Tower(string id, string name, double x, double y, int cost, double range, double damage, double fireRate)
            : base(id, name, x, y)
        {
            Cost = cost;
            Range = range;
            Damage = damage;
            FireRate = fireRate;
        }

        public override void Update(double deltaTime)
        {
            if (CurrentCooldown > 0) CurrentCooldown -= deltaTime;
            UpdateVisual();
        }

        public virtual Enemy GetTarget(List<Enemy> enemies)
        {
            return enemies
                .Where(e => CalculateDistance(e) <= Range && e.CurrentHealth > 0)
                .OrderBy(e => CalculateDistance(e))
                .FirstOrDefault();
        }

        public virtual void Attack(Enemy target, List<Enemy> allEnemies)
        {
            if (CurrentCooldown <= 0 && target != null)
            {
                // Varsayılan davranış: Sadece hedefe vur
                target.TakeDamage(Damage);
                CurrentCooldown = FireRate;
            }
        }

        protected double CalculateDistance(GameObject other)
        {
            return Math.Sqrt(Math.Pow(other.Location.X - Location.X, 2) + Math.Pow(other.Location.Y - Location.Y, 2));
        }

        // İki nokta arası mesafe (Topçu kulesi için lazım olacak)
        protected double CalculateDistance(Point p1, Point p2)
        {
            return Math.Sqrt(Math.Pow(p1.X - p2.X, 2) + Math.Pow(p1.Y - p2.Y, 2));
        }
    }
}