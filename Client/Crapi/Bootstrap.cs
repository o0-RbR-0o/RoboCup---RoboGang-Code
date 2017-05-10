using System;
using RoboGang.Properties;
using RoboGang.RoboGang.BasicComponents;
using RoboGang.RoboGang.Team;

namespace RoboGang
{
    internal class Bootstrap
    {
        private static void Main()
        {
            StartGame();

            for (; ; ) {

                var restart = false;
                var exit = false;

                Console.WriteLine(Resources.Program_Main_Press_R_to_restart__press_Q_to_quit);

                switch (Console.ReadKey().KeyChar) {
                    case 'r': restart = true; break;
                    case 'q': exit = true; break;
                }
                if (exit)
                    return;
                if (restart)
                {
                    StartGame();
                }
            }

        }

        public static void StartGame()
        {
            Team testTeam = null;
            Team gegnerTeam = null;
            var tp = new TeamProperties();

            try
            {
                tp.LoadFromBinaryFile(Constants.TeamConfigFilePath);

                testTeam = new Team
                {
                    TeamProperties = tp.Properties,
                    TeamName = "Testteam",
					TeamSide = TeamYaffa.CRaPI.Side.Left
                }.CreateTeam();

               
                gegnerTeam = new Team
                {
					TeamProperties = tp.Properties,
					TeamName = "Gegnerteam",
					TeamSide = TeamYaffa.CRaPI.Side.Right
                }.CreateTeam();
                
            }

            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            if (testTeam != null) testTeam.ConnectAll();
            gegnerTeam.ConnectAll();

            Console.WriteLine(Resources.Program_StartGame_Maybe_connected_);
        }

    }
}
