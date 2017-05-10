using System;
using System.Collections;
using System.Linq;
using TeamYaffa.CRaPI;
using TeamYaffa.CRaPI.Commands;
using TeamYaffa.CRaPI.Utility;
using TeamYaffa.CRaPI.World;
using TeamYaffa.CRaPI.World.GameObjects;

namespace RoboGang.RoboGang.BasicComponents.Personalities
{
    internal class Defensive : Personality
	{
		private bool _hasStartPosition;

		// The player dribbles the ball 
		public void DribbleWithBall()
		{

			//Calculate point to dribble to, based on distance to opponents
            var p = PlayerHandler.Context.Player;
			var points = DribblePoints();
			var longest = double.MinValue;
			var pointFound = false;
			var kickPoint = new Point2D();

			foreach (var point in points) {
				//check if the dribbling point is in the filed
				if (!MathUtility.WithinPlayFieldBoundary(p.World, point))
					continue;

				var currentDistance = p.World.Opponents.Cast<FieldPlayer>()
                                                .Sum(opponent => opponent.Position - point);

				//check distance to all opponents

			    if(!(currentDistance > longest)) continue;
			    pointFound = true;
			    longest = currentDistance;
			    kickPoint = point;
			}

			// if theres no dribbling point take the opponent goal as the point to shoot to
			if (!pointFound) {
				var staticObj = WorldModel.initStaticObjects(p.TeamSide);
				var goalMiddlePoint = p.TeamSide == Side.Left ? (Point2D)staticObj ["g r"] : (Point2D)staticObj ["g l"];
				goalMiddlePoint.X = goalMiddlePoint.X - 5;
				kickPoint = goalMiddlePoint;
			}

			p.CommandQueue.Enqueue(BasicActions.KickToPoint(p, kickPoint, 30));
			p.Send();

		}

		// The player runs to his home position
		public void RunHome() {
            var p = PlayerHandler.Context.Player;
			var wm = p.World;

			var angleToBall = MathUtility.NormalizeAngle(wm.Self.Position.AngleTo(wm.TheBall.Position) - wm.Self.AbsoluteBodyDirection);

			if((angleToBall < p.ServerParam.MinNeckAngle ||
				angleToBall > p.ServerParam.MaxNeckAngle) &&
				(p.SenseBody.ViewQuality != ViewQuality.high ||
					p.SenseBody.ViewWidth != ViewWidth.wide))
			{
				p.CommandQueue.Enqueue(new ChangeView(ViewWidth.wide, ViewQuality.high));
			}
			else if(angleToBall > p.ServerParam.MinNeckAngle 
                && angleToBall < p.ServerParam.MaxNeckAngle 
                && (p.SenseBody.ViewQuality != ViewQuality.high ||  p.SenseBody.ViewWidth != ViewWidth.normal))
			{
				p.CommandQueue.Enqueue(new ChangeView(ViewWidth.normal, ViewQuality.high));
			}
	   
            p.CommandQueue.Enqueue(BasicActions.DashToPoint(p, p.StartPoint, (int)(p.ServerParam.MaxPower*0.5)));
            p.Send();
		}


      
		// Pass the ball to a passable mate
		public void DoPass(ArrayList passableMates) {

            var p = PlayerHandler.Context.Player;
			var longest = double.MinValue;
			var bestTaker = (FieldPlayer)passableMates[0];

			foreach(FieldPlayer mate in passableMates)
			{
                //check distance to all opponents
				var currentDistance = p.World.Opponents.Cast<FieldPlayer>().Sum(opponent => opponent.Position - mate.Position);

			    if (!(currentDistance > longest) || mate.Goalie) continue;
			    longest = currentDistance;
			    bestTaker = mate;
			}

			p.CommandQueue.Enqueue(BasicActions.KickToPoint(p, bestTaker.Position, CalculateKickPower(8,bestTaker)));
			p.Send();
		}



        /*
        * Function: BlockOpponent
        * If stamina value > 1000 calculate intersection point 
        * with choosen opponent player and block them
        */
        public void BlockOpponent()
        {
            var p = PlayerHandler.Player;

            //stamina < 1000
            if (p.SenseBody.Stamina > 1000)
            {

                Point2D intersectPoint;

                //get all blockable opponents in area
                var blockableOpps = GetBlockableOpps();

                //get closest opp in area
                var chosenOpponent = GetClosestOpponent(blockableOpps);

                //get intersection point
                MathUtility.IntersectionPoint(p, chosenOpponent, out intersectPoint);


                p.CommandQueue.Enqueue(BasicActions.DashToPoint(p, intersectPoint, (int)(p.ServerParam.MaxPower * 0.9)));
            }

            else
            {
                p.CommandQueue.Enqueue(BasicActions.TurnToPoint(p, p.World.TheBall.Position));
            }

            p.Send();
        }


		/*
        * Actions for each situation and return command to execute in the following code
        */


		//Before a kickoff
		public override Command DoBeforeKickOff()
		{
            var p = PlayerHandler.Context.Player;

			if (!p.IsGoalie)
			{
			    if (_hasStartPosition) return null;
			    _hasStartPosition = true;
			    //return BasicCommands.Move(p.World.TheBall.Position);
			    return BasicCommands.Move(p.StartPoint);
			}

		    return p.IsGoalie ? BasicCommands.Move(-50, 0) : null;
		}



		//While a kickoff when own team has to kick off.
		public override Command DoWhileKickOffOwn()
		{
            var p = PlayerHandler.Context.Player;

			if (p.BallIsKickable)
				//Random value for testing.
				return BasicCommands.Kick(40, 4);
		    if (p.UniformNumber == 2 || p.UniformNumber == 3)
		        return BasicCommands.Move(p.World.TheBall.Position);
		    return BasicCommands.Move(p.World.TheBall.Position);
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
			if (p.BallIsCatchable) {
				return BasicActions.CatchBall (p);
			}
			//what to do if the ball is kickable
			if (p.BallIsKickable) {
				var staticObj = WorldModel.initStaticObjects(p.TeamSide);
				var goalMiddlePoint = p.TeamSide == Side.Left ? (Point2D)staticObj["g r"] : (Point2D)staticObj["g l"];
				if (GetPlayerstoPass().Count > 0 && OppisNearPlayer()) {
					DoPass(GetPlayerstoPass ());
					return null;
				}
				//if the goal is near kick towards it
				if (p.World.Self.Position - goalMiddlePoint < 23){ 
					return BasicActions.KickToPoint(p, goalMiddlePoint, p.ServerParam.MaxPower);
				} 
				//nothing else to do then dribble
				DribbleWithBall ();
				return null;
			}
			if (ClosestOfMatesToPosition(p.World.TheBall.Position) && p.World.TheBall.SeenThisCycle && p.World.TheBall.SpeedAmount>1) {
				Point2D inersPoint;
				MathUtility.IntersectionPoint(p, p.World.TheBall, out inersPoint);
				var dist = inersPoint - p.World.MyPosition;
				var power = 0;
				if (!ShouldStopDash())
					power = 100;
				if(dist < p.ServerParam.KickableMargin && p.World.TheBall.SpeedAmount < 0.2 ||
					PassesNearPlayer(p.World.TheBall, p.ServerParam.KickableMargin))
				{
					p.CommandQueue.Enqueue(BasicActions.TurnToPoint(p, p.World.TheBall.Position));
					p.Send();
					return null;
				}
				if(PassesNearPlayer(p.World.TheBall, 2))
				{
					inersPoint = new Point2D(p.World.TheBall.Position, p.World.TheBall.SpeedDirection, p.World.TheBall.Distance);
				}

				p.CommandQueue.Enqueue(BasicActions.DashToPoint(p, inersPoint, power));
				p.Send();
				return null;
			}
			//if the player ist nearest to ball try to fight for it
			if (ClosestOfMatesToPosition(p.World.TheBall.Position) && p.World.TheBall.SeenThisCycle) {
				p.CommandQueue.Enqueue(BasicActions.DashToPoint(p, p.World.TheBall.Position, (int)(p.ServerParam.MaxPower * 0.9)));
				p.Send();
				return null;
			}
			//if a mate has got the ball try to make yourself passable 
			if (FindPlayerwithBall() != null && FindPlayerwithBall().UniformNumber != p.UniformNumber && p.World.TheBall.Distance < 20) {
				GetPassable(FindPlayerwithBall());
				return null;
			}
			//if the ball is too far away and our are not the closest go back to your base position
			if (p.World.TheBall.Distance > 20 && !(ClosestOfMatesToPosition(p.World.TheBall.Position))) {
				RunHome();
				return null;
			}
			//if the ball is not seen, search for it
		    if (p.World.TheBall.SeenThisCycle || ClosestOfMatesToPosition(p.World.TheBall.Position))
		        return p.World.TheBall.SeenThisCycle ? BasicActions.TurnToObject(p, p.World.TheBall) : BasicCommands.Turn(45);
		    return null;
		}



		public override Command DoWhileCornerOwn()
		{
            var p = PlayerHandler.Context.Player;
			var upperCorner = false;
			var cornerPosition = new Point2D(50, 30);
			var rnd = new Random();

			if (p.World.TheBall.Position.Y > 0)
			{ //falls buggy, dann weil position hier fuer player nicht sichtbar. Pruefen
				upperCorner = true;
				cornerPosition.SetLocation(50, -30);
			}

			if (p.World.TheBall.SeenThisCycle && !p.IsGoalie)
			{
			    if (p.BallIsKickable)
				{
					var nearestTeammate = FindNearestTeammate();

					return nearestTeammate != null ? BasicActions.KickToPoint(p, nearestTeammate.Position, 40) : BasicActions.KickToPoint(p, upperCorner ? new Point2D(40 - p.UniformNumber, -20) : new Point2D(40 - p.UniformNumber, 20), 50);
				}

			    if (p.World.TheBall.Distance < 18)
			        return FindNearestTeammate() != null ? BasicActions.DashToPoint(p, p.World.TheBall.Position, 20) : BasicActions.DashToPoint(p, p.World.TheBall.Position, 30*p.UniformNumber*5);

			    return BasicActions.DashToPoint(p, upperCorner ? new Point2D(3 * p.UniformNumber, cornerPosition.Y + (10 + p.UniformNumber)) : new Point2D(3 * p.UniformNumber, cornerPosition.Y - (10 - p.UniformNumber)), 38 - p.UniformNumber);
			}

		    if (!p.IsGoalie)
		    {
		        if (p.UniformNumber > 8)
		        {
		            return BasicActions.DashToPoint(p, upperCorner ? new Point2D(3 * p.UniformNumber, cornerPosition.Y + (10 + p.UniformNumber)) : new Point2D(3 * p.UniformNumber, cornerPosition.Y - (10 - p.UniformNumber)), 38 - p.UniformNumber);
		        }
		        if (p.World.MyPosition.XDistanceTo(cornerPosition) < 30)
		        {
		            return BasicActions.DashToPoint(p, cornerPosition, 50);
		        }
		    }
		    else
		    {
		        return BasicActions.DashToPoint(p, new Point2D(-45, 0), 20);
		    }
		    if (p.UniformNumber == 7)
			{
				return BasicActions.DashToPoint(p, new Point2D(50, 30), 40);
			}
			if (p.UniformNumber == 8)
			{
				return BasicActions.DashToPoint(p, new Point2D(50, -30), 40);
			}

			return BasicActions.DashToPoint(p, new Point2D(rnd.Next(-20, 30), rnd.Next(-20, 20)), 40);
		}



        public override Command DoWhileCornerOpponent()
        {
            var p = PlayerHandler.Context.Player;
            if (GetBlockableOpps().Count > 0 && GetClosestOpponent(GetBlockableOpps()) != null)
            {
                BlockOpponent();
            }
            return p.World.TheBall.SeenThisCycle ? BasicActions.TurnToObject(p, p.World.TheBall) : BasicCommands.Turn(45);
        }



		public override Command DoWhileFreekickOwn()
		{
            var p = PlayerHandler.Context.Player;
			if (p.BallIsKickable) {
				var staticObj = WorldModel.initStaticObjects(p.TeamSide);
				var goalMiddlePoint = p.TeamSide == Side.Left ? (Point2D)staticObj["g r"] : (Point2D)staticObj["g l"];
				if(p.World.Self.Position - goalMiddlePoint < 25)
                { 
					return BasicActions.KickToPoint (p, goalMiddlePoint, p.ServerParam.MaxPower);
				}
			    if(GetPlayerstoPass().Count <= 0) return BasicActions.KickToPoint(p, goalMiddlePoint, p.ServerParam.MaxPower);
			    DoPass(GetPlayerstoPass());
			    return null;
			}
			if(ClosestOfMatesToPosition (p.World.TheBall.Position) && p.World.TheBall.SeenThisCycle) {
				p.CommandQueue.Enqueue(BasicActions.DashToPoint(p, p.World.TheBall.Position, (int)(p.ServerParam.MaxPower * 0.9)));
				p.Send ();
				return null;
			}
			if(FindPlayerwithBall() != null && FindPlayerwithBall().UniformNumber != p.UniformNumber && (p.World.TheBall.Distance <= 20)) {
				GetPassable(FindPlayerwithBall());
				return null;
			}
            if(p.World.TheBall.SeenThisCycle)
            {
                return BasicActions.TurnToObject(p, p.World.TheBall);
            }
		    return BasicCommands.Turn(45);
		}



		public override Command DoWhileFreekickOpponent()
		{
            var p = PlayerHandler.Context.Player;
            if (GetBlockableOpps().Count > 0 && GetClosestOpponent(GetBlockableOpps()) != null)
            {
                BlockOpponent();
            }
            return p.World.TheBall.SeenThisCycle ? BasicActions.TurnToObject(p, p.World.TheBall) : BasicCommands.Turn(45);
		}




		public override Command DoWhileGoalkickOwn()  
		{
            var p = PlayerHandler.Context.Player;
			if (p.BallIsKickable) 
            {
				var staticObj = WorldModel.initStaticObjects(p.TeamSide);
				var goalMiddlePoint = p.TeamSide == Side.Left ? (Point2D)staticObj["g r"] : (Point2D)staticObj["g l"];
				if(p.World.Self.Position - goalMiddlePoint < 25)
                {
					return BasicActions.KickToPoint(p, goalMiddlePoint, p.ServerParam.MaxPower);
				}
                if (GetPlayerstoPass().Count <= 0)
                    return BasicActions.KickToPoint(p, goalMiddlePoint, p.ServerParam.MaxPower);

                DoPass (GetPlayerstoPass());
                return null;
            }
			if (ClosestOfMatesToPosition(p.World.TheBall.Position) && p.World.TheBall.SeenThisCycle) 
            {
				p.CommandQueue.Enqueue(BasicActions.DashToPoint(p, p.World.TheBall.Position, (int)(p.ServerParam.MaxPower * 0.9)));
				p.Send();
				return null;
			}
		    if (FindPlayerwithBall() == null || FindPlayerwithBall().UniformNumber == p.UniformNumber)
		        return p.World.TheBall.SeenThisCycle ? BasicActions.TurnToObject(p, p.World.TheBall) : BasicCommands.Turn(45);

		    GetPassable(FindPlayerwithBall());
		    return null;
		}



		public override Command DoWhileGoalkickOpponent()
		{
            var p = PlayerHandler.Context.Player;
            if (GetBlockableOpps().Count > 0 && GetClosestOpponent(GetBlockableOpps()) != null)
            {
                BlockOpponent();
            }
            return p.World.TheBall.SeenThisCycle ? BasicActions.TurnToObject(p, p.World.TheBall) : BasicCommands.Turn(45);
		}



		public override Command DoAfterGoalOwn()
		{
            var p = PlayerHandler.Context.Player;
            return BasicCommands.Move(p.StartPoint);
		}



		public override Command DoAfterGoalOpponent()
		{
            var p = PlayerHandler.Context.Player;
            return BasicCommands.Move(p.StartPoint);
            
		}



		public override Command DoWhileKickInOwn()
		{
            var p = PlayerHandler.Context.Player;
			if (p.BallIsKickable) 
            {
				var staticObj = WorldModel.initStaticObjects(p.TeamSide);
				var goalMiddlePoint = (p.TeamSide == Side.Left) ? (Point2D)staticObj["g r"] : (Point2D)staticObj["g l"];
				if (p.World.Self.Position - goalMiddlePoint < 25)
                { 
					return BasicActions.KickToPoint(p, goalMiddlePoint, p.ServerParam.MaxPower);
				}
                if(GetPlayerstoPass().Count <= 0)
                    return BasicActions.KickToPoint(p, goalMiddlePoint, p.ServerParam.MaxPower);
                DoPass(GetPlayerstoPass());
                return null;
            }
			if (ClosestOfMatesToPosition(p.World.TheBall.Position) && p.World.TheBall.SeenThisCycle) 
            {
				p.CommandQueue.Enqueue(BasicActions.DashToPoint(p, p.World.TheBall.Position, (int)(p.ServerParam.MaxPower * 0.9)));
				p.Send();
				return null;
			}
		    if (FindPlayerwithBall() == null || FindPlayerwithBall().UniformNumber == p.UniformNumber)
		        return p.World.TheBall.SeenThisCycle ? BasicActions.TurnToObject(p, p.World.TheBall) : BasicCommands.Turn(45);

		    GetPassable(FindPlayerwithBall());
		    return null;
		}




		public override Command DoWhileKickInOpponent()
		{
            var p = PlayerHandler.Context.Player;
            if (GetBlockableOpps().Count > 0 && GetClosestOpponent(GetBlockableOpps()) != null)
            {
                BlockOpponent();
            }
            return p.World.TheBall.SeenThisCycle ? BasicActions.TurnToObject(p, p.World.TheBall) : BasicCommands.Turn(45);
		}



		public override Command DoWhileUnknownPlaymode()
		{
			throw new NotImplementedException();
		}



		public override Command DoIfOffsideOwn()
		{
            var p = PlayerHandler.Context.Player;
			if (p.BallIsKickable) { 
				if (GetPlayerstoPass().Count > 0) {
					DoPass(GetPlayerstoPass());
					return null;
				}
				if (FindNearestTeammate() != null) {
					return BasicActions.KickToPoint(p, FindNearestTeammate().Position, CalculateKickPower(10, FindNearestTeammate()));
				}
				if (FindTeammatefarbehind() != null) {
					return BasicActions.KickToPoint(p, FindTeammatefarbehind().Position, CalculateKickPower(10, FindTeammatefarbehind()));
				}
			}
			if (ClosestOfMatesToPosition (p.World.TheBall.Position) && p.World.TheBall.SeenThisCycle) {
				p.CommandQueue.Enqueue(BasicActions .DashToPoint(p, p.World.TheBall.Position, (int)(p.ServerParam.MaxPower * 0.9)));
				p.Send();
				return null;
			}
		    if (FindPlayerwithBall() == null || FindPlayerwithBall().UniformNumber == p.UniformNumber)
		        return p.World.TheBall.SeenThisCycle ? BasicActions.TurnToObject(p, p.World.TheBall) : BasicCommands.Turn(45);

		    GetPassable (FindPlayerwithBall());
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
