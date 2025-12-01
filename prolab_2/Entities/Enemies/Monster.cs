using System.Windows.Media;
using System.Windows.Shapes;

namespace prolab_2
{
    public class FlyingMonster : Enemy
    {
        public FlyingMonster(string id, double x, double y)
            // PDF İsterleri (Uçan Düşman):
            // Can: 50 (Standart ile aynı)
            // Hız: 12 (Standarttan %50 hızlı - Goblin 8 ise bu 12 olur)
            // Zırh: 0 (Zırhsız)
            // Para: 15 (Ödül)
            // Üs Hasarı: 5
            // Uçuyor mu: EVET (true)
            : base(id, "Flying Monster", x, y, 50, 0, 12, 15, true, 5)
        {
            // Görünüm: Mor renkli, küçük ve hızlı bir daire
            Appearance = new Ellipse
            {
                Width = 20,
                Height = 20,
                Fill = Brushes.Indigo, // Ayırt edici mor renk
                Stroke = Brushes.White,
                StrokeThickness = 1
            };
            UpdateVisual();
        }
    }
}