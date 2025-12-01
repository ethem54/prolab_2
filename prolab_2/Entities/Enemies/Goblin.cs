using System.Windows.Media;
using System.Windows.Shapes;

namespace prolab_2
{
    public class Goblin : Enemy
    {
        public Goblin(string id, double x, double y)
            // Can: 50, Zırh: 0, Hız: 8, Para: 10, Uçuyor mu: Hayır
            : base(id, "Goblin Raider", x, y, 50, 0, 10, 10, false, 5)
        {
            Appearance = new Rectangle
            {
                Width = 25,
                Height = 25,
                Fill = Brushes.Orange,
                Stroke = Brushes.Black
            };
            UpdateVisual();
        }
    }
}