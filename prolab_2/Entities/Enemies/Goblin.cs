using System.Windows.Media;
using System.Windows.Shapes;

namespace prolab_2
{
    public class Goblin : Enemy
    {
        public Goblin(string id, double x, double y)
            : base(id, "Goblin", x, y, 50, 0, 8, 10, false, 5)
        {
            SetupVisual();
        }
        public override bool IsChampion => true;

        public override string ImageName => "goblinn";
    }
}