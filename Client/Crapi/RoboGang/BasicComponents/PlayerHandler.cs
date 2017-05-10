using System;
using System.Threading;
using TeamYaffa.CRaPI;
using TeamYaffa.CRaPI.Commands;
using TeamYaffa.CRaPI.Utility;

namespace RoboGang.RoboGang.BasicComponents
{
    public class PlayerHandler
    {
        public Point2D Randpos { get; set; }

        public int Runcycles { get; set; }

        public PlayerContext Context { get; set; }

        public Player Player { get; set; }


        public PlayerHandler(Player p, PlayerContext pc)
        {
            Context = pc;
            Context.Player = p;

            pc.Personality.PlayerHandler = this;
            Context.Personality = pc.Personality;
            Context.PlayerThread = new Thread(Context.Player.PlayGame);
            Player = Context.Player;
            p.AfterNewCycle += Tick;
        }

        public void StartThread()
        {
            Context.PlayerThread.Start();
        }


         /*
         * Method to call every cycle, containing the logic of the connected player
         */
        private void Tick(object sender, TimeEventArgs arg)
        {
            var rnd=new Random();
            Command commandToExecute = null;

            //Do stuff before kickoff
            switch (Context.Player.PlayMode)
            {
                case PlayMode.before_kick_off:
                    commandToExecute = Context.Personality.DoBeforeKickOff();
                    break;
                case PlayMode.kick_off_l:
                    //if kickoff is same side as team of player
                    commandToExecute = Context.Player.TeamSide == Side.Left ? Context.Personality.DoWhilePlayOn() : Context.Personality.DoWhileKickOffOpponent();
                    break;
                case PlayMode.kick_off_r:
                    //if kickoff is same side as team of player
                    commandToExecute = Context.Player.TeamSide == Side.Right ? Context.Personality.DoWhilePlayOn() : Context.Personality.DoWhileKickOffOpponent();
                    break;
                case PlayMode.kick_in_l:
                    commandToExecute = Context.Player.TeamSide == Side.Left ? Context.Personality.DoWhileKickInOwn() : Context.Personality.DoWhileKickInOpponent();
                    break;
                case PlayMode.kick_in_r:
                    commandToExecute = Context.Player.TeamSide == Side.Right ? Context.Personality.DoWhileKickInOwn() : Context.Personality.DoWhileKickInOpponent();
                    break;
                case PlayMode.offside_l:
                    commandToExecute = Context.Player.TeamSide == Side.Left ? Context.Personality.DoIfOffsideOwn() : Context.Personality.DoIfOffsideOpponent();
                    break;
                case PlayMode.offside_r:
                    commandToExecute = Context.Player.TeamSide == Side.Right ? Context.Personality.DoIfOffsideOwn() : Context.Personality.DoIfOffsideOpponent();
                    break;
                case PlayMode.free_kick_l:
                    commandToExecute = Context.Player.TeamSide == Side.Left ? Context.Personality.DoWhileFreekickOwn() : Context.Personality.DoWhileFreekickOpponent();
                    break;
                case PlayMode.free_kick_r:
                    commandToExecute = Context.Player.TeamSide == Side.Right ? Context.Personality.DoWhileFreekickOwn() : Context.Personality.DoWhileFreekickOpponent();
                    break;
                case PlayMode.goal_kick_l:
                    Context.Player.CommandQueue.Clear();
                    commandToExecute = Context.Player.TeamSide == Side.Left ? Context.Personality.DoWhileGoalkickOwn() : Context.Personality.DoWhileGoalkickOpponent();
                    break;
                case PlayMode.goal_kick_r:
                    Context.Player.CommandQueue.Clear();
                    commandToExecute = Context.Player.TeamSide == Side.Right ? Context.Personality.DoWhileGoalkickOwn() : Context.Personality.DoWhileGoalkickOpponent();
                    break;
                case PlayMode.corner_kick_l:
                    Context.Player.CommandQueue.Clear();
                    commandToExecute = Context.Player.TeamSide == Side.Right ? Context.Personality.DoWhileCornerOpponent() : this.Context.Personality.DoWhileCornerOwn();
                    break;
                case PlayMode.corner_kick_r:
                    Context.Player.CommandQueue.Clear();
                    commandToExecute = Context.Player.TeamSide == Side.Right ? Context.Personality.DoWhileCornerOwn() : Context.Personality.DoWhileCornerOpponent();
                    break;
                case PlayMode.goal_r:
                case PlayMode.goal_l:
                    Context.Player.CommandQueue.Clear();
                    commandToExecute = Context.Player.TeamSide == Side.Right ? Context.Personality.DoAfterGoalOwn() : Context.Personality.DoAfterGoalOpponent();
                    break;
                case PlayMode.play_on:
                    commandToExecute = Context.Personality.DoWhilePlayOn();
                    break;
                case PlayMode.drop_ball:
                    break;
                case PlayMode.none:
                    break;
                case PlayMode.time_over:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            //Execute the command
            if (commandToExecute != null)
            {
                Player.CommandQueue.Enqueue(commandToExecute);
                Player.Send();
            }

            Runcycles = (Runcycles + 1)%60;
            if (Runcycles == 0)
            {
                Randpos = new Point2D(rnd.Next(-50, 50), rnd.Next(-25, 25));
            }
        }
    }
}