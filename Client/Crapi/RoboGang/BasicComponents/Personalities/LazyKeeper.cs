using System;
using TeamYaffa.CRaPI;
using TeamYaffa.CRaPI.Commands;
using TeamYaffa.CRaPI.Utility;

namespace RoboGang.RoboGang.BasicComponents.Personalities
{
    internal class LazyKeeper : Personality
    {
      


       /*
       * Fill with actions for each situation and return command to execute in the following code
       */

        //Before a kickoff
        public override Command DoBeforeKickOff()
        {
           //Move the goalie into the goal
            return BasicCommands.Move(PlayerHandler.Context.Player.StartPoint);
        }

        //While a kickoff when own team has to kick off.
        public override Command DoWhileKickOffOwn()
        {
            var p = PlayerHandler.Context.Player;
			return p.BallIsKickable ? BasicCommands.Kick(100, 30) : null;

            //Do the same as before kickoff to finally move him into the goal;
            //return do_before_kickoff();
        }

        //While a kickoff when opponent team has to kick off.
        public override Command DoWhileKickOffOpponent()
        {
            return null;
        }

        //While play on
        public override Command DoWhilePlayOn()
        {
            //The player executing all this
            var p = PlayerHandler.Context.Player;

            //If the ball is kickable, then kick it, to prevent it from entering the goal
            if (p.BallIsKickable)
            {
            	return BasicActions.KickToPoint(p, new Point2D(-15, 0), p.ServerParam.MaxPower);
            }
            //If the ball is still catchable, then catch it
            if (p.BallIsCatchable)
                return BasicActions.CatchBall(p);


            //If the ball hasn't been seen this cycle or the ball is behind one of the goals, then focus the middle point, or, if the goalie is not at his start position, return
			if (!p.World.TheBall.SeenThisCycle || !(p.World.TheBall.Position.X > -52.5) || !(p.World.TheBall.Position.X < 52.5)) {
				if(IsNearStartPoint()){
					return BasicActions.TurnToPoint(p,new Point2D(0,0) );
				}else{
					return BasicActions.DashToPoint (p, p.StartPoint, 40);
				}
			}	
			//If the ball is near or inside the penalty area, either move to ball position while remaining the own X position (ball outside penalty area) or move towards the ball (ball inside penalty area)							
			if (!(Math.Abs (p.World.TheBall.Position.Y) < 16) || !(p.World.TheBall.Position.X < -35)) {
				if ((p.World.TheBall.Position.X < -30 && p.World.TheBall.Position.X > -35) && Math.Abs(p.World.TheBall.Position.Y) < 20)
					return BasicActions.DashToPoint (p, new Point2D (p.World.MyPosition.X, p.World.TheBall.Position.Y), 50);
                return BasicActions.TurnToPoint (p, p.World.TheBall.Position);
			}
            //If the ball is too near the goal, predict the ball position 5 cycles ahead and move there with maximum power
            if (p.World.TheBall.ForecastPosition(5, p.World.Positioner).X < -35 && p.World.TheBall.ForecastPosition(2, p.World.Positioner).X > -48) {

                return BasicActions.DashToPoint(p, new Point2D(p.World.TheBall.ForecastPosition(2, p.World.Positioner)), p.ServerParam.MaxPower);
            }
            return BasicActions.DashToPoint(p, new Point2D(-49, p.World.TheBall.ForecastPosition(8, p.World.Positioner).Y), p.ServerParam.MaxPower);
        }

        //Do nothing on corner own. We are a goalie
        public override Command DoWhileCornerOwn()
        {
            return null;
        }

        //Do nothing. We are goalie...
        public override Command DoWhileCornerOpponent()
        {
            return null;
        }

        //If the ball is kickable while freekick, kick it to the middle point with maximum power. Else dash to it (Everything only when distance < 20)
        public override Command DoWhileFreekickOwn()
        {
            var p = PlayerHandler.Context.Player;
			if (p.World.TheBall.Distance < 20)
			{
			    return p.BallIsKickable ? BasicActions.KickToPoint(p, new Point2D (0,0), p.ServerParam.MaxPower) 
                                        : BasicActions.DashToPoint (p, p.World.TheBall.Position, p.ServerParam.MaxPower);
			}
            return null;
        }

        //Do nothing, we are a goalie...
        public override Command DoWhileFreekickOpponent()
        {
			return null;
        }

        //Try to find the ball, kick it when kickable, dash to it if not.
        public override Command DoWhileGoalkickOwn()
        {
            var p = PlayerHandler.Context.Player;

            if (!p.World.TheBall.SeenThisCycle) return BasicCommands.Turn(45);

            return p.BallIsKickable ? BasicActions.KickToPoint(p, new Point2D (0,0), p.ServerParam.MaxPower) 
                                    : BasicActions.DashToPoint(p, p.World.TheBall.Position, p.ServerParam.MaxPower);
        }

        public override Command DoWhileGoalkickOpponent()
        {
			return null;
        }

        public override Command DoAfterGoalOwn()
        {
            return null;
        }

        public override Command DoAfterGoalOpponent()
        {
            PlayerHandler.Context.Player.CommandQueue.Clear();
            return null;
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
