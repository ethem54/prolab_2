using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace prolab_2
{
    public List<Point> GamePath;
    public MainWindow()
    {
        InitializeComponent();
        //enemy'lerin yol haritası

    }
    public abstract class Enemy
    {
        public String Type { get; set; }
        public double Health { get; set; }
        public double Armor { get; set; }
        public double ID { get; set; }
        public bool IsFlying { get; set; }
        //...

        protected Enemy(enemyType type, double id, double health, double armor) // burayı da enemy tipine göre değiştirirsin
        {
            this.ID = id;
            this.Health = health;
            this.Armor = armor;
            //...
        }

        public void TakeDamage(double damage)
        {
            double gettingDamage = damage; //formül var burada
        }
    }

    public class StandardEnemy : Enemy
    {
        public StandardEnemy(string id) : base(id, ...) { }
    }
    //...

    public abstract class Tower
    {
        public String Type { get; set; }
        public double Power { get; set; }
        public double AttackSpeed { get; set; }
        public double Range { get; set; }
        public bool IsSlow { get; set; }
        // kule çeşitlerini dosyaya göre düzenlersin

        protected Tower(double power, double range) // kulelere koyduğun özellliklere göre düzenleyebilirsin
        {
            this.Power = power;
            this.Range = range;
            //...
        }

        // burada enemy'nin koordinatlarına göre range kontrolü yapıp vurup vurmayacağını belirleyebilirsin
    }

    public class AsdTower : Tower
    {
        public AsdTower(...) { }
    }
    //...
    public class GameManager
    {
        // değişkenlerin olduğu yer
        public double PlayerHealth { get; set; } = 100;
        public double Gold { get; set; } = 200;
        public double wave = 0;

        public List<Enemy> Enemies { get; private set; } = new List<Enemy>();
        public List<Tower> Towers { get; private set; } = new List<Tower>();
        public List<Point> PathWaypoints { get; private set; } = new List<Point>();

        public GameManager()
        {
            GameLogger.ClearLog(); //temizliyor
            GameLogger.Log("Game Started");
            // oyun kaydı
        }
    }
    
}