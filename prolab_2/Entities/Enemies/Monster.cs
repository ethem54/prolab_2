using System.Collections.Generic;
using TowerDefense;
using System;
using System.Linq;
using prolab_2;
using System.Windows.Media;
using System.Windows.Shapes;

namespace TowerDefense
{

    //Uçabilen düşman türümüz
    public class FlyingMonster : Enemy
    {
       //CONSTRUCTOR
        public FlyingMonster(string id, int x, int y) : base (id, "Uçan Yaratık", x, y, 50, 0, 15, 15, true, 5)
        {
            Appearance = new Rectangle
            {
                Width = 25,
                Height = 25,
                Fill = Brushes.LawnGreen,
                Stroke = Brushes.Black
            };
            UpdateVisual();
        }
    }
}
