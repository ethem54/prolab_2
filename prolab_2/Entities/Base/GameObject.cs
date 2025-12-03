using System.Windows;
using System.Windows.Controls;

namespace prolab_2
{
    public abstract class GameObject
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public Point Location { get; set; }

        //Width/Height özelliği için FrameworkElement
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

                double w = Appearance.Width;
                double h = Appearance.Height;
                if (double.IsNaN(w)) w = Appearance.ActualWidth;
                if (double.IsNaN(h)) h = Appearance.ActualHeight;
                // Genişliğin yarısını çıkararak MERKEZİ konuma oturtuyoruz
                Canvas.SetLeft(Appearance, Location.X - w / 4);
                Canvas.SetTop(Appearance, Location.Y - h);

                Canvas.SetZIndex(Appearance, 99);
            }
        }

        public abstract void Update(double deltaTime);
    }
}