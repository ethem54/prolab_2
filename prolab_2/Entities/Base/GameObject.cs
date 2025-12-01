using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;          // Point için
using System.Windows.Shapes;   // Shape (Rectangle, Ellipse) için
using System.Windows.Controls; // Canvas işlemleri için

namespace prolab_2
{
    // Eski "Object" sınıfının yeni adı: GameObject
    public abstract class GameObject
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public Point Location { get; set; }     // Konum (X, Y)
        public Shape Appearance { get; set; }   // Görünüm (Ekrandaki Şekil)

        // Constructor (Kurucu)
        protected GameObject(string id, string name, double x, double y)
        {
            ID = id;
            Name = name;
            Location = new Point(x, y);
        }
        public void UpdateVisual()
        {
            if (Appearance != null)
            {
                // Şekli tam merkezinden konumlandırır
                Canvas.SetLeft(Appearance, Location.X - Appearance.Width / 2);
                Canvas.SetTop(Appearance, Location.Y - Appearance.Height / 2);
            }
        }
        public abstract void Update(double deltaTime);
    }
}