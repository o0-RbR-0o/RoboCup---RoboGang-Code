using System.Threading;
using RoboGang.RoboGang.BasicComponents.Personalities;
using TeamYaffa.CRaPI;
using TeamYaffa.CRaPI.Utility;

namespace RoboGang.RoboGang.BasicComponents
{
    public class PlayerContext
    {

        public Player Player { get; set; }

        public Thread PlayerThread { get; set; }

        public Point2D Randpos { get; set; }

        public int Runcycles { get; set; }

        public Personality Personality { get; set; }

        public PlayerContext()
        {
            Runcycles = 0;
        }
    }
}
