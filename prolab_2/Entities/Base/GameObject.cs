using System.Windows;
using System.Windows.Controls;

namespace prolab_2
{
    public abstract class GameObject
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public Point Location { get; set; }

        // DÜZELTME 1: UIElement yerine FrameworkElement (Width/Height özelliği için)
        public FrameworkElement Appearance { get; set; }

        protected GameObject(string id, string name, double x, double y)
        {
            ID = id;
            Name = name;
            Location = new Point(x, y);
        }

        // Bu metot hem Düşman hem Kule için çalışır, şekli tam ortaya koyar.
        public void UpdateVisual()
        {
            if (Appearance != null)
            {
                // Genişliğin yarısını çıkararak MERKEZİ konuma oturtuyoruz
                Canvas.SetLeft(Appearance, Location.X - Appearance.ActualWidth / 2); // ActualWidth daha güvenlidir
                Canvas.SetTop(Appearance, Location.Y - Appearance.ActualHeight / 2);

                Canvas.SetZIndex(Appearance, 99);
            }
        }

        public abstract void Update(double deltaTime);
    }
}