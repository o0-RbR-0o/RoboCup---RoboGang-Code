using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RoboGang.RoboGang.BasicComponents;
using RoboGang.RoboGang.BasicComponents.Personalities;
using TeamYaffa.CRaPI;
using TeamYaffa.CRaPI.Utility;

namespace RoboGang.RoboGang.Team
{

    public class Team
    {

        /*
         * Team Properties
         */
        public int Players { get; set; }
        public string TeamName { get; set; }
        public Side TeamSide { get; set; }
        public TeamProperty[] TeamProperties { get; set; }

        private readonly List<PlayerHandler> _playerHandlerList = new List<PlayerHandler>();

         /*
         * Class Constuctor
         */
        public Team CreateTeam()
        {
            var tp = TeamProperties;

            foreach (var t in tp)
            {
                var p = new Player(TeamName, t.IsGoalie);

                p.TeamSide = TeamSide != 0 ? TeamSide : p.TeamSide;

                p.StartPoint = new Point2D(t.StartpointX, t.StartpointY);

                Personality pers;
                //We could also do this by using reflection - But we like safer code ;)
                switch (t.Personality)
                {
                    case "Renegade": pers = new Renegade(); break;
                    case "RenegadeOld": pers = new Offensive(); break;
                    case "LazyKeeper": pers = new LazyKeeper(); break;
                    case "RenegadeNew": pers = new Defensive(); break;
                    case "ImprovedKeeper": pers = new ImprovedKeeper(); break;

                    default: pers = new Renegade(); break;
                }

                var pc = new PlayerContext {Personality = pers};
                _playerHandlerList.Add(new PlayerHandler(p, pc));
            }

            return this;
        }


        /*
         * Connect to Server and start Thread
         */
        public void ConnectAll()
        {
            Parallel.ForEach(_playerHandlerList, p =>
            {
                p.Context.Player.StartUp(Constants.HostIp, Constants.Port, Constants.TimeOut);
                p.StartThread();
            });
        }

    }
}
