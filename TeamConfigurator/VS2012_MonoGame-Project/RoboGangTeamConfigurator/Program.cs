using System;

/* RoboGang Team Configurator - A small editor for visual RoboCup2D startup configuration - Made for use with the RoboGang project which is based on Crapi*/

namespace RoboGangTeamConfigurator
{
#if WINDOWS || LINUX
    public static class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            using (var game = new Main())
                if (args.Length > 0)
                    try
                    {
                        //Scale is passed by command line argument 0
                        game.Scale = Convert.ToInt32(args[0]);
                        game.Run();
                    }
                    catch
                    {
                        game.Scale = 1;
                        game.Run();
                    }
                else
                {
                    game.Scale = 1;
                    game.Run();
                }
        }
    }
#endif
}
