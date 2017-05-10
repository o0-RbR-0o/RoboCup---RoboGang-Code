using System;
using TeamYaffa.CRaPI;
using TeamYaffa.CRaPI.Commands;
using TeamYaffa.CRaPI.Utility;

namespace RoboGang.RoboGang.BasicComponents.Personalities
{
    internal class ImprovedKeeper : Personality
    {

       /*
       * Actions for each situation and return command to execute in the following code
       * THIS PERSONALITY IS NOT USED, LEGACY AND UNFINISHED. WE JUST KEPT THE FILE, JUST IN CASE WE MIGHT USE IT. IT DOESN'T DO MUCH THOUGH.
       */

        //Before a kickoff
        public override Command DoBeforeKickOff()
        {
            //Move the goalie into the goal
            return BasicCommands.Move(-50, 0);
        }

        //While a kickoff when own team has to kick off.
        public override Command DoWhileKickOffOwn()
        {
            //Do the same as before kickoff to finally move him into the goal;
            return DoBeforeKickOff();
        }

        //While a kickoff when opponent team has to kick off.
        public override Command DoWhileKickOffOpponent()
        {
            return null;
        }

        //While play on
        public override Command DoWhilePlayOn()
        {
            var p = PlayerHandler.Context.Player;

            if (p.BallIsKickable)
            {
                var tm = FindNearestTeammate();

                return BasicActions.KickToPoint(p, tm != null ? 
                                                   tm.Position : 
                                new Point2D(0, 0), p.ServerParam.MaxPower);
            }

            if (p.BallIsCatchable)
                return BasicActions.CatchBall(p);


            if (p.World.TheBall.SeenThisCycle)
            {
                if (Math.Abs(p.World.TheBall.Position.Y) < 15 && p.World.TheBall.Position.X < -36)
                {
                    return Math.Abs(p.World.TheBall.Position.Y - p.World.MyPosition.Y) > 2 ? 
                            BasicActions.DashToPoint(p, new Point2D(-51, p.World.TheBall.Position.Y), p.ServerParam.MaxPower) : BasicActions.TurnToObject(p, p.World.TheBall);
                }
                return BasicActions.TurnToPoint(p, p.World.TheBall.Position);
            }

            p.CommandQueue.Enqueue(BasicActions.DashToPoint(p, new Point2D(-50, 0), 40));
            return BasicActions.TurnToPoint(p, new Point2D(50, 0));
        }

        public override Command DoWhileCornerOwn()
        {
			return null;
        }

        public override Command DoWhileCornerOpponent()
        {
			return null;
        }

        public override Command DoWhileFreekickOwn()
        {
			return null;
        }

        public override Command DoWhileFreekickOpponent()
        {
			return null;
        }

        public override Command DoWhileGoalkickOwn()
        {
			return null;
        }

        public override Command DoWhileGoalkickOpponent()
        {
			return null;
        }

        public override Command DoAfterGoalOwn()
        {
            throw new NotImplementedException();
        }

        public override Command DoAfterGoalOpponent()
        {
            throw new NotImplementedException();
        }

        public override Command DoWhileKickInOwn()
        {
			return null;
        }

        public override Command DoWhileKickInOpponent()
        {
			return null;
        }

        public override Command DoWhileUnknownPlaymode()
        {
            throw new NotImplementedException();
        }

        public override Command DoIfOffsideOwn()
        {
			return null;
        }

        public override Command DoIfOffsideOpponent()
        {
			return null;
        }

        public override Command DoAfterTimeOut()
        {
            throw new NotImplementedException();
        }
    }
}
