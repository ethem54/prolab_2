using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows; // Point için gerekli

namespace prolab_2
{
    public abstract class Enemy : GameObject
    {
        // Özellikler
        public double MaxHealth { get; protected set; }
        public double CurrentHealth { get; set; }
        public int Armor { get; protected set; }
        public double Speed { get; protected set; }
        public int Bounty { get; protected set; }        // Ödül Parası
        public double DamageToPlayer { get; protected set; } // Üs hasarı
        public bool IsFlying { get; protected set; }     // Uçuyor mu?

        // İç Hesaplama Değişkenleri
        protected int WayPointIndex { get; set; } = 0;   // Rota sırası
        protected double CurrentSpeed;                   // Anlık Hız (Yavaşlayınca değişir)
        protected double SlowDuration;                   // Yavaşlama süresi sayacı

        public List<Point> AssignedPath { get; set; }
        // Constructor
        protected Enemy(string id, string name, double x, double y, double maxHealth, int armor, double speed, int bounty, bool isFlying, double damageToPlayer)
            : base(id, name, x, y)
        {
            MaxHealth = maxHealth;
            CurrentHealth = maxHealth;
            Armor = armor;
            Speed = speed;
            Bounty = bounty;
            DamageToPlayer = damageToPlayer;
            IsFlying = isFlying;
            CurrentSpeed = speed;
        }

        // Hasar Alma
        public void TakeDamage(double rawDamage)
        {
            double damageReduction = 1.0 - (Armor / (Armor + 100.0));
            double netDamage = rawDamage * damageReduction;

            CurrentHealth -= netDamage;
        }

        // Yavaşlatma Uygulama (ApplySlow)
        public void ApplySlow(double duration, double percentage)
        {
            CurrentSpeed = Speed * (1.0 - percentage);
            SlowDuration = duration;
        }

        // Hareket Mantığı (Move)
        public bool Move(List<Point> path, double deltaTime)
        {
            // Rota bittiyse true döndür
            if (WayPointIndex >= path.Count) return true;

            Point target = path[WayPointIndex];

            // Adım Hesabı: Hız * Zaman * 10 (Piksel çarpanı)
            double step = CurrentSpeed * deltaTime * 10;

            double dx = target.X - Location.X;
            double dy = target.Y - Location.Y;
            double distance = Math.Sqrt(dx * dx + dy * dy);

            // Hedefe vardı mı?
            if (distance <= step)
            {
                Location = target; // Tam üstüne oturt
                WayPointIndex++;   // Sıradaki hedefe geç
                return WayPointIndex >= path.Count; // Yol bitti mi?
            }
            else
            {
                // Hedefe doğru adım at
                double nx = dx / distance;
                double ny = dy / distance;

                Location = new Point(Location.X + nx * step, Location.Y + ny * step);
                return false; // Yol devam ediyor
            }
        }

        // Güncelleme (Update)
        public override void Update(double deltaTime)
        {
            // Yavaşlatma süresini kontrol et
            if (SlowDuration > 0)
            {
                SlowDuration -= deltaTime;
                if (SlowDuration <= 0) CurrentSpeed = Speed; // Süre bitince hızı düzelt
            }

            // Görseli yeni konuma taşı (GameObject'ten gelen metot)
            UpdateVisual();
        }
    }
}