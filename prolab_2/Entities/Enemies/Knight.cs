using System.Security.Cryptography;
using System.Windows.Media;
using System.Windows.Shapes;

namespace prolab_2
{
    public class Knight : Enemy
    {
        public Knight(string id, double x, double y)
            // Can: 120, Zırh: 15, Hız: 4, Para: 25, Uçuyor mu: Hayır
            : base(id, "Armored Knight", x, y, 75, 50, 4, 20, false, 10)
        {
            SetupVisual();
        }
        public override bool IsChampion => true;

        public override string ImageName => "knightt";
    }
}