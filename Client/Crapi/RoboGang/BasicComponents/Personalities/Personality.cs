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
    /*Abstract class defining the interface to implement in all personalities, implementing helper methods which are usefull for all personalities*/
	public abstract class Personality{

		public PlayerHandler PlayerHandler { get; set; }

		//Searching for dribbling points
		protected Point2D[] DribblePoints()
		{
            Point2D[] points;
            var p = PlayerHandler.Context.Player;
			var myPos = p.World.MyPosition;
			var bodyDir = p.World.Self.AbsoluteFaceDirection;
            
			if(bodyDir < 90 - 45 && bodyDir > 45 - 90)
			{
				//If we're facing the opponent's goal and the kick will be forward or to the side. 
				//We will not dribble home.
				points = new[]{ 
					new Point2D(myPos, bodyDir, 2),
					new Point2D(myPos, bodyDir - 45, 2),
					new Point2D(myPos, bodyDir + 45, 2) 
				};
			}
			else
			{
				var temp = WorldModel.initStaticObjects(p.TeamSide);
				var goalCenter = p.TeamSide == Side.Left ? (Point2D)temp["g r"] : (Point2D)temp["g l"];

				//If we're facing our own goal, create a point behind you and turn and make a new evaluation.
				points = new[]{ goalCenter };
			}
			return points;
		}

		//Set two pass positions
		protected void PassPositions(FieldPlayer playerwithball, Point2D pMatePosition, out Point2D pFirstPoint, out Point2D pSecondPoint)
		{
			var distance = playerwithball.Position - pMatePosition;

			pFirstPoint = 
                new Point2D(playerwithball.Position, playerwithball.Position.AngleTo(pMatePosition) - 45/2, distance);
			pSecondPoint = 
                new Point2D(playerwithball.Position, playerwithball.Position.AngleTo(pMatePosition) + 45/2, distance);
		}

		//Check if any opponent is between the player with the ball and the given Point
		protected bool FreePassCone(FieldPlayer playerwithball, Point2D pPassPoint)
		{
			var distance = playerwithball.Position - pPassPoint;
			var passPointAngle = playerwithball.Position.AngleTo(pPassPoint);


			var leftAngle = MathUtility.NormalizeAngle(passPointAngle - 15 / 2);
			var rightAngle = MathUtility.NormalizeAngle(passPointAngle + 15 / 2);

            var opponents = PlayerHandler.Context.Player.World.Opponents;
			foreach(FieldPlayer opponent in opponents)
			{
			    if (!(distance >= playerwithball.Position - opponent.Position)) continue;

			    var oppAngle = playerwithball.Position.AngleTo(opponent.Position);

			    if(rightAngle < leftAngle)
			    {
			        if((oppAngle < rightAngle && oppAngle > -180) ||
			           (oppAngle > leftAngle && oppAngle < 180))
			            return false;
			    }
			    else
			    {
			        if(oppAngle > leftAngle && oppAngle < rightAngle)
			            return false;
			    }
			}
			return true;
		}

		// Returns true if any opponent is near to the current player
		protected bool OppisNearPlayer() {

            var p = PlayerHandler.Context.Player;

		    return p.World.AllOpponents.Any(opp => p.World.Self.Position - opp.Position < 5);
		}

		// Add any team mate who is passable to passableMates and returns that list
		protected ArrayList GetPlayerstoPass() {
            var p = PlayerHandler.Context.Player;
			var passableMates = new ArrayList ();
			foreach(FieldPlayer fPlayer in p.World.TeamMates)
			{
				if(fPlayer.Position - p.World.Self.Position > 5 &&
					FreePassCone(p.World.Self, fPlayer.Position) && !fPlayer.Goalie)
					passableMates.Add(fPlayer);
			}
			return passableMates;
		}

		// Searches for the team mate which distance is the most to the current player
		protected FieldPlayer FindTeammatefarbehind() {

            var p = PlayerHandler.Context.Player;
			double distance = -1;
			FieldPlayer tempPlayer = null;

			foreach (FieldPlayer temp in p.World.TeamMates) {

			 if (!(distance < temp.Distance)) continue;

			    distance = temp.Distance;
			    tempPlayer = temp;
			}
			return tempPlayer;
		}



		// Searches all team mates for the player who is near to the ball
		protected FieldPlayer FindPlayerwithBall() {

            var p = PlayerHandler.Context.Player;

		    return p.World.TeamMates.Cast<FieldPlayer>()
                    .FirstOrDefault(mate => mate.Position - p.World.TheBall.Position <= 3);
		}


		// Find nearest player
		protected FieldPlayer FindNearestTeammate()
		{
			double distance = -1;
			FieldPlayer tempPlayer = null;
            var p = PlayerHandler.Context.Player;

			foreach (FieldPlayer t in p.World.TeamMates)
			{
			    if (distance != -1 && !(t.Distance < distance)) continue;

			    tempPlayer = t;
			    distance = t.Distance;
			}

			return tempPlayer;
		}

		// Checks if oneself is closest to a position, compared
		// to the team mates.
		protected bool ClosestOfMatesToPosition(Point2D ballPos) {

            var p = PlayerHandler.Context.Player;
			var myDist = p.World.MyPosition - ballPos;

		    return p.World.TeamMates.Cast<FieldPlayer>()
                    .All(mate => !(mate.Position - ballPos < myDist));
		}

		// calculates the power needed to accelerate the ball
		// to a certain speed
		// The calculation based on a mobile object
		// Problem: the current calculation is not finished and probably not in use
		// here are just some ideas to handle the power for a kick to a team mate
		protected int CalculateKickPower(int pWantedEndSpeed, MobileObject mobile)
		{
			var distance = mobile.Distance;
            var p = PlayerHandler.Context.Player;
			MobileObject ball = p.World.TheBall;
			var decay = p.ServerParam.BallDecay;
			var kickAbleMargin = p.ServerParam.KickableMargin;
			var kickPowerRate = p.ServerParam.KickPowerRate;
			var dirDiff = Math.Abs(ball.Direction + p.World.Self.HeadAngle);

			var power = 100;
			var kickFactor = 1 - 0.25*(dirDiff/180) - 0.25*(ball.Distance/kickAbleMargin);
			var ep = power * kickPowerRate * kickFactor;
			var wantedEp = pWantedEndSpeed * kickPowerRate;
			double tmpDistance = 0;
			double cycleDistance;
			var step = 0;
			const int decrease = 5;

			if(ball.DistanceChange < 0)
			{
				ep += (p.World.Self.SpeedAmount + ball.SpeedAmount) * 0.1;
			}

			if(ep > p.ServerParam.BallSpeedMax)
				ep = p.ServerParam.BallSpeedMax;

			do
			{
				step++;
				cycleDistance = ep * Math.Pow(decay, step);
				tmpDistance += cycleDistance;

			    if(!(tmpDistance > distance)) continue;

			    power -= decrease;
			    ep = power * kickPowerRate * kickFactor;
			    tmpDistance = 0;
			    step = 0;
			}

			while(cycleDistance > wantedEp);
			return power;

		}

        // Check if the player is near home position
        protected bool IsNearStartPoint()
        {
            var p = PlayerHandler.Context.Player;
            if (p.World.Self.Position.X == p.StartPoint.X && p.World.Self.Position.Y == p.StartPoint.Y)
                return true;
			return Math.Abs(p.World.Self.Position - p.StartPoint) < 2;
        }

		// Check if the ball is near to the player
		protected bool PassesNearPlayer(MobileObject ball, double distance)
		{
			var p = PlayerHandler.Player;

			var passPointAngle = ball.Position.AngleTo(p.World.MyPosition);

			var leftAngle = MathUtility.NormalizeAngle(passPointAngle - distance);
			var rightAngle = MathUtility.NormalizeAngle(passPointAngle + distance);

			var passerAngle = p.World.TheBall.SpeedDirection;

			if(rightAngle < leftAngle)
			{
				if((passerAngle < rightAngle && passerAngle > -180) ||
					(passerAngle > leftAngle && passerAngle < 180))
					return true;
			}
			else
			{
				if(passerAngle > leftAngle && passerAngle < rightAngle)
					return true;
			}

			return false;
		}

		// Check if the player should stop the dash
		protected bool ShouldStopDash()
		{
            var crapiPlayer = PlayerHandler.Context.Player;
			return crapiPlayer.SenseBody.SpeedAmount > crapiPlayer.World.TheBall.Distance;
		}


        //Get all blockable Opponents near player
        protected ArrayList GetBlockableOpps()
        {
            var p = PlayerHandler.Player;
            var blockableopps = new ArrayList();
            foreach (FieldPlayer opp in p.World.Opponents)
            {
                if (ClosestOfMatesToPosition(opp.Position))
                {
                    blockableopps.Add(opp);
                }
            }
            return blockableopps;
        }

        //Get closest Opponent from given List
        protected FieldPlayer GetClosestOpponent(ArrayList blockableOpps)
        {
            var p = PlayerHandler.Player;
            var closest = double.MaxValue;
            FieldPlayer chosenOpponent = null;
            foreach (FieldPlayer opp in blockableOpps)
            {
                var tmp = opp.Position - p.World.MyPosition;
                if (!(tmp < closest)) continue;
                closest = tmp;
                chosenOpponent = opp;
            }
            return chosenOpponent;
        }

		// Make the current player passable to the player who got the ball
		public void GetPassable(FieldPlayer playerwithball)
		{
			var p = PlayerHandler.Context.Player;

			var chosenPos = new Point2D();
			var possiblePositions = new ArrayList();
			var passPoints = new Point2D[3];
			passPoints[0] = p.World.Self.Position;
			PassPositions(playerwithball, passPoints[0], out passPoints[1], out passPoints[2]);

			foreach (var point in passPoints)
			{
				if (FreePassCone(playerwithball, point))
					possiblePositions.Add(point);
			}

			var longest = double.MinValue;
			foreach (Point2D position in possiblePositions)
			{
				var currentDistance = p.World.Opponents.Cast<FieldPlayer>()
					.Sum(opponent => opponent.Position - position);

				//check distance to all opponents

				if (!(currentDistance > longest)) continue;
				longest = currentDistance;
				chosenPos = position;
			}

			var dist = p.World.MyPosition - chosenPos;
			//which power is needed to shoot the ball to the position
			if (!(dist > 1)) return;
			var power = dist > 2 ? p.ServerParam.MaxPower : (int) (p.ServerParam.MaxPower*0.5);
			p.CommandQueue.Enqueue(BasicActions.DashToPointLookingAtPoint(p, chosenPos, power, p.World.TheBall.Position));
			p.Send();
		}

         /*
         * Actions for each situation and return command to execute in the following code
         */

		//Before a kickoff
		public abstract Command DoBeforeKickOff();

		//While a kickoff when own team has to kick off.
		public abstract Command DoWhileKickOffOwn();

		//While a kickoff when opponent team has to kick off.
		public abstract Command DoWhileKickOffOpponent();

		//While play on
		public abstract Command DoWhilePlayOn();

		//While corner for own team
		public abstract Command DoWhileCornerOwn();

		//While corner for opponent team
		public abstract Command DoWhileCornerOpponent();

		//While freekick for own team
		public abstract Command DoWhileFreekickOwn();

		//While freekick for opponent team
		public abstract Command DoWhileFreekickOpponent();

		//While goalkick for own team
		public abstract Command DoWhileGoalkickOwn();

		//While goalkick for opponent team
		public abstract Command DoWhileGoalkickOpponent();

		//After goal for own team
		public abstract Command DoAfterGoalOwn();

		//After goal for opponent team
		public abstract Command DoAfterGoalOpponent();

		//While kick in for own team
		public abstract Command DoWhileKickInOwn();

		//While kick in for opponent team
		public abstract Command DoWhileKickInOpponent();

		//While unknown playmode
		public abstract Command DoWhileUnknownPlaymode();

		//If own team is offside
		public abstract Command DoIfOffsideOwn();

		//If opponent team is offside
		public abstract Command DoIfOffsideOpponent();

		//If the game is finished
		public abstract Command DoAfterTimeOut();


	}

}

