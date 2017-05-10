#region License
// Copyright (C) 2002 Team Yaffa
// 
// This library is free software; you can redistribute it and/or
// modify it under the terms of the GNU Lesser General Public
// License as published by the Free Software Foundation; either
// version 2.1 of the License, or (at your option) any later version.
// 
// This library is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
// Lesser General Public License for more details.
// 
// You should have received a copy of the GNU Lesser General Public
// License along with this library; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA
//
// Contact Team Yaffa: pt99jst@student.bth.se, pt99mbe@student.bth.se or pt99tol@student.bth.se
#endregion
using System;
using System.Threading;
using System.Collections;
using System.Globalization;
using TeamYaffa.CRaPI.World.Positioning;
using TeamYaffa.CRaPI.Utility;
using TeamYaffa.CRaPI.Info;
using TeamYaffa.CRaPI.Commands;
using TeamYaffa.CRaPI.World.GameObjects;

namespace TeamYaffa.CRaPI.World
{
	///	<summary>
	///	This class represents the WorldModel of a player. The world model is the players (agents)
	///	perception of the state of the play field. E.g. the players position, the position of
	///	the team mates, opponents and the ball etc.
	///	</summary>
	public class WorldModel
	{
		#region Members
		/// <summary>The noise factor</summary>
		public const double NOISE = 0.10;
        /// <summary>The ???resolution??? test value</summary>
        public const double RESOLUTION = 0.10;
		/// <summary>The number of flags used for positioning</summary>
		public const int MAXFLAGS = 5;
		/// <summary>Flag names and positions</summary>
		private Hashtable mStaticObjects;
		/// <summary>This player</summary>
		private FieldPlayer mSelf;
		/// <summary>A repository of seen players</summary>
		private PlayerRepository mPlayerReposit;
		/// <summary>This Player</summary>
		private Player mPlayer;
		/// <summary>The ball</summary>
		private Ball mBall;
		/// <summary>The frame (cycle) when this WorldModel was updated</summary>
		private int mUpdatedFrame;
		/// <summary>A number formatter</summary>
		private NumberFormatInfo mNumberFormatter;
		/// <summary>The positioner</summary>
        private Positioning.Positioner mPositioner;

        public Positioning.Positioner Positioner
        {
            get { return mPositioner; }
            set { mPositioner = value; }
        }
		/// <summary>The reference to the vague ball when only the direction to the ball is known.</summary>
		private VagueObject mVagueBall;
		#endregion

		#region Constructor
		/// <summary>Constructs a new worldmodel</summary>
		/// <param name="pPlayer">The player that is assoicated with this world model</param>
		public WorldModel(Player pPlayer)
		{
			mSelf =	new	FieldPlayer(pPlayer.UniformNumber, pPlayer.IsGoalie);
			mPlayer	= pPlayer;
			mPlayerReposit = new PlayerRepository(mPlayer.TeamName);
			mBall =	new	Ball();
			mStaticObjects = initStaticObjects(mPlayer.TeamSide);
			mUpdatedFrame = -1;

			mNumberFormatter = new NumberFormatInfo();
			mNumberFormatter.NumberDecimalSeparator = ".";

			mPositioner = new Positioning.Positioner(mPlayer.ServerParam.SimulatorStep, this);
		}
		#endregion

		#region Update methods
		/// <summary>
		/// Called on <c>see</c> packets. Updates the worldmodel of the player.
		/// </summary>
		/// <param name="pSee">The data string with the seen objects</param>
		internal void Update(string pSee)
		{
			int end = pSee.IndexOf(' ', 5);
			
			if(end == -1)
				end = pSee.IndexOf(')');

			mUpdatedFrame = Convert.ToInt32(pSee.Substring(5, end - 5));
			updateSelf(pSee);
			updateBall(pSee);
			OnBallPositionCalculated(new PositionEventArgs(mBall.Position, mUpdatedFrame));
			updateOtherPlayers(pSee);
			OnTeamMatesCalculated(new FieldPlayerEventArgs(mPlayerReposit.TeamMates, mUpdatedFrame));
			OnOpponentsCalculated(new FieldPlayerEventArgs(mPlayerReposit.Opponents, mUpdatedFrame));
		}

		/// <summary>
		/// Dead reckogning update
		/// </summary>
		/// <remarks>The dead reckoning is used when a new cycle begins, but there are no new see-data
		/// available for that cycle.</remarks>
		internal void Update()
		{
			updateSelf();
			updateOtherPlayers();
			updateBall();
		}

		#region Update self
		/// <summary>
		/// Updates the players own position and direction
		/// </summary>
		/// <remarks>If no flags are seen, dead reckogning is used</remarks>
		/// <param name="pSee">The data string with the seen objects</param>
		private void updateSelf(string pSee)
		{
			Flag[] flags = getFlags(pSee);
			Array.Sort(flags);
			OnFlagsParsed(new FlagsEventArgs(flags, mUpdatedFrame));
            Intersection[] intersections = updateSelf(flags);
			OnIntersectionsCalculated(new IntersectionsEventArgs(intersections, mUpdatedFrame));
			Point2D DRpos;
			Point2D prevPos = mSelf.Position;
			
			//Before kick off, dead reckogning should not be used since we
			//dont have any previous data to calculate on, instead simulate this
			//by using the start position, which we should be near to
			if(mPlayer.PlayMode == PlayMode.before_kick_off)
			{
				DRpos = mPlayer.StartPoint;
			}
			else
			{
				DRpos = mPositioner.CalculateDeadReckoningPosition(mSelf.Position, mSelf.AbsoluteBodyDirection, mPlayer.SenseBody.SpeedAmount, 1);
			}

			if(intersections.Length == 0) //no intersections, use Dead reckogning
			{
				mSelf.Position = DRpos;
				OnPositionUnknown();
			}
			else
			{
				mSelf.Position = mPositioner.CalculatePosition(intersections, DRpos);
			}
			OnPositionCalculated(new PositionEventArgs(mSelf.Position, mUpdatedFrame));
			ArrayList temp = new ArrayList();
			temp.Add(mSelf);
			OnPlayerCalculated(new FieldPlayerEventArgs(temp, mUpdatedFrame));
			updateOwnAngle(flags);
		}

		/// <summary>
		/// Calculates and sets the angle of the player, with the help of 0 to 3 flags
		/// </summary>
		/// <param name="pFlags">The seen flags</param>
		private void updateOwnAngle(Flag[] pFlags)
		{
			//calculate direction as the average of the direction from 3 flags
			int number = pFlags.Length >= 3 ? 3 : pFlags.Length;

			//no flags seen, use previous direction
			if(number == 0)
				return;

			ArrayList angles = new ArrayList();
			for(int i = 0; i < number; i++)
			{
				Flag curFlag = pFlags[i];
				double Xdist = mSelf.Position.SignedXDistanceTo(curFlag.Position);
				double Ydist = mSelf.Position.SignedYDistanceTo(curFlag.Position);
				double angle1 = Math.Atan(Ydist/Xdist);

				if(Xdist < 0)
				{
					if(Ydist < 0)
					{
						angle1 -= MathUtility.ToRadians(180);
					}
					else
					{
						angle1 += MathUtility.ToRadians(180);
					}
				}
			
				double angle = angle1 - MathUtility.ToRadians(curFlag.Direction);
				angles.Add(angle);
			}

			//take the median of the angles
			angles.Sort();
			double medAngle = (double)angles[angles.Count/2];

			mSelf.AbsoluteFaceDirection = MathUtility.NormalizeAngle(MathUtility.ToDegrees(medAngle));
			mSelf.AbsoluteBodyDirection = MathUtility.NormalizeAngle(mSelf.AbsoluteFaceDirection - mPlayer.SenseBody.HeadAngle);
			mSelf.HeadAngle = mPlayer.SenseBody.HeadAngle;
			
		}

		/// <summary>
		/// Calculates all <c>IntersectionPoints</c> that could exists with the seen flags.
		/// </summary>
		/// <remarks>The intersectionpoints that are outside the playfield (outside the physical boundary)
		/// is thrown away. The physical boundary is validated against the flags:
		/// <c>(f l 0)</c>, <c>(f t 0)</c>, <c>(f r 0)</c> and <c>(f b 0)</c></remarks>
		/// <param name="pFlags">An array with the flags that should be used to calculate the
		/// players new position. Only the first <see cref="MAXFLAGS"/> flags will be used, therefore the list should
		/// sorted so the flags closest is used. The closer the flag is, the more accurate
		/// is the perception of the flag.</param>
		/// <returns>An array of intersections of the distances to the flags</returns>
		private Intersection[] updateSelf(Flag[] pFlags)
		{
			double upperBoundary = mPlayer.TeamSide == Side.Left ? ((Point2D)mStaticObjects["f r 0"]).X : ((Point2D)mStaticObjects["f l 0"]).X;
			double lowerBoundary = mPlayer.TeamSide == Side.Left ? ((Point2D)mStaticObjects["f l 0"]).X : ((Point2D)mStaticObjects["f r 0"]).X;
			double leftBoundary = mPlayer.TeamSide == Side.Left ? ((Point2D)mStaticObjects["f t 0"]).Y : ((Point2D)mStaticObjects["f b 0"]).Y;
			double rightBoundary = mPlayer.TeamSide == Side.Left ? ((Point2D)mStaticObjects["f b 0"]).Y : ((Point2D)mStaticObjects["f t 0"]).Y;
			ArrayList intersections = new ArrayList();

			for(int i = 0; i < pFlags.Length - 1 && i < MAXFLAGS - 1; i++)
			{
				for(int j = i + 1; j < pFlags.Length && j < MAXFLAGS; j++)
				{
					Flag f0 = pFlags[i];
					Flag f1 = pFlags[j];
			
					// The distance between the two flags
					double dist_0_1 = f0.Position - f1.Position;

					if(dist_0_1 > f0.Distance + f1.Distance)
					{
						continue; // there exists no intersectionpoint
					}
					if((f0.Distance < f1.Distance) && (f0.Distance + dist_0_1 < f1.Distance))
					{
						continue; // there exists no intersectionpoint
					}
					if((f1.Distance < f0.Distance) && (f1.Distance + dist_0_1 < f0.Distance))
					{
						continue; // there exists no intersectionpoint
					}	

					// The distance between flag f0 and point P3
					double dist_0_3 = (Math.Pow(dist_0_1, 2) - Math.Pow(f1.Distance, 2) + Math.Pow(f0.Distance, 2)) / (2 * dist_0_1);

					double xDist = f1.Position.X - f0.Position.X;
					double yDist = f1.Position.Y - f0.Position.Y;

					double sub = Math.Sqrt(Math.Pow(f0.Distance, 2) - Math.Pow(dist_0_3, 2));
					
					double x1 = f0.Position.X + ((xDist * dist_0_3) / dist_0_1) + (yDist / dist_0_1) * sub;
					double y1 = f0.Position.Y + ((yDist * dist_0_3) / dist_0_1) - (xDist / dist_0_1) * sub;
				
					double x2 = f0.Position.X + ((xDist * dist_0_3) / dist_0_1) - (yDist / dist_0_1) * sub;
					double y2 = f0.Position.Y + ((yDist * dist_0_3) / dist_0_1) + (xDist / dist_0_1) * sub;

					Point2D ip1 = new Point2D(x1, y1);
					Point2D ip2 = new Point2D(x2, y2);

					if(ip1.Y > leftBoundary && ip1.Y < rightBoundary && ip1.X < upperBoundary && ip1.X > lowerBoundary)
					{
						intersections.Add(new Intersection(ip1, f0, f1));
					}

					if(ip2.Y > leftBoundary && ip2.Y < rightBoundary && ip2.X < upperBoundary && ip2.X > lowerBoundary)
					{
						intersections.Add(new Intersection(ip2, f0, f1));
					}
				}
			}
			return (Intersection[])intersections.ToArray(typeof(Intersection));
		}

		/// <summary>
		/// Parses the seen flags.
		/// </summary>
		/// <param name="pSee">The data string with the seen objects</param>
		/// <returns>An unsorted array of all flags seen.</returns>
		private Flag[] getFlags(string pSee)
		{
			ArrayList resultAL = new ArrayList();
			int start, end;
			string name, values;
			string [] splitvalues;
			double distance, direction;

			//((F are ignored as of now
			start = pSee.IndexOf("((f");

			while(start != -1) // for each flag
			{
				start += 2;
				end = pSee.IndexOf(')', start); // end of name
				name = pSee.Substring(start, end - start);
				start = end + 2;
				end = pSee.IndexOf(')', start + 1);
				values = pSee.Substring(start, end-start);
				splitvalues = values.Split(' ');
				if(splitvalues.Length < 2)
				{
					start = pSee.IndexOf("((f", end);
					continue;
				}

				distance = double.Parse(splitvalues[0], mNumberFormatter);
				direction = double.Parse(splitvalues[1], mNumberFormatter);
				resultAL.Add(new Flag(name, (Point2D)mStaticObjects[name], distance, direction));
				start = pSee.IndexOf("((f", end);
			}
			return (Flag[])resultAL.ToArray(typeof(Flag));
		}

		/// <summary>
		/// Updates the Players own position and direction, based on dead-reckogning
		/// </summary>
		private void updateSelf()
		{
			SenseBody curSense = mPlayer.SenseBody;
			SenseBody prevSense = mPlayer.PreviousSenseBody;

			mSelf.SpeedAmount = mPlayer.SenseBody.SpeedAmount;
			mSelf.SpeedDirection = mPlayer.SenseBody.SpeedDirection;
			
			if(curSense == null || prevSense == null)
			{
				return;
			}

			int neckTurns = curSense.TurnNeckCount - prevSense.TurnNeckCount;
			int bodyTurns = curSense.TurnCount - prevSense.TurnCount;
			int dashes = curSense.DashCount - prevSense.DashCount;

			//have turned the neck
			if(neckTurns > 0)
			{
				TurnNeck command = (TurnNeck)mPlayer.PreviousCommandQueue.GetCommand(CommandType.Turn_Neck);

				if(command != null)
				{
					//Calculate possible turn
					double moment = command.Moment;
					
					if(moment + mSelf.HeadAngle > mPlayer.ServerParam.MaxNeckAngle)
						moment = mPlayer.ServerParam.MaxNeckAngle - mSelf.HeadAngle;
					else if(moment + mSelf.HeadAngle < mPlayer.ServerParam.MinNeckAngle)
						moment = mPlayer.ServerParam.MinNeckAngle - mSelf.HeadAngle;
					
					if(moment > mPlayer.ServerParam.MaxNeckMoment)
						moment = mPlayer.ServerParam.MaxNeckMoment;
					else if(moment < mPlayer.ServerParam.MinNeckMoment)
						moment = mPlayer.ServerParam.MinNeckMoment;

					mSelf.HeadAngle += moment;
					mSelf.AbsoluteFaceDirection += moment;
					mSelf.AbsoluteFaceDirection = MathUtility.NormalizeAngle(mSelf.AbsoluteFaceDirection);
					mSelf.ClearForecasts();
				}
			}

			//have turned the body
			if(bodyTurns > 0)
			{
				Turn command = (Turn)mPlayer.PreviousCommandQueue.GetCommand(CommandType.Turn);
				
				if(command != null)
				{
					//Calculate possible turn
					double moment = command.Moment;

					if(moment > mPlayer.ServerParam.MaxMoment)
						moment = mPlayer.ServerParam.MaxMoment;
					else if(moment < mPlayer.ServerParam.MinMoment)
						moment = mPlayer.ServerParam.MinMoment;

					//inertia
					moment = moment / (1 + mPlayer.ServerParam.InertiaMoment * Self.SpeedAmount);

					mSelf.AbsoluteFaceDirection += command.Moment;
					mSelf.AbsoluteFaceDirection = MathUtility.NormalizeAngle(mSelf.AbsoluteFaceDirection);
					mSelf.AbsoluteBodyDirection = mSelf.AbsoluteFaceDirection-mPlayer.SenseBody.HeadAngle;
					mSelf.AbsoluteBodyDirection = MathUtility.NormalizeAngle(mSelf.AbsoluteBodyDirection);
				}
			}
			mSelf.Position = mPositioner.CalculateDeadReckoningPosition(mSelf.Position, mSelf.SpeedDirection, mSelf.SpeedAmount, 1);
		}
		#endregion

		#region Update ball and other players
		
		/// <summary>
		/// Updates the position of the ball
		/// </summary>
		/// <remarks>When only the direction to the ball is seen, the information of the ball
		/// is saved in <see cref="VagueBall"/>. This is just a temporary solution, and the
		/// worldmodel will be refactored for the next big release.</remarks>
		/// <param name="pSee">The data string with the seen objects</param>
		private void updateBall(string pSee)
		{
			//Parse ball data
			int start, end;
			string [] splitvalues;
			string values;
			double distance, direction;
			start = indexOf(pSee, "((b", "((B");

			//ball is not seen, maybe send event
			if(start == -1)
			{
				mBall.SeenThisCycle = false;
				mVagueBall = null;
				return;
			}

			start += 5; //start of values
			end = pSee.IndexOf(')', start);
			values = pSee.Substring(start, end-start);
			splitvalues = values.Split(' ');

			// to less information about the position. Consider for the vague worldmodel 
			// see Feature Request# 712708 at project page
			if(splitvalues.Length < 2)
			{
				mBall.SeenThisCycle = false;
				try
				{
					mVagueBall = new VagueObject(VagueType.ball, double.Parse(splitvalues[0], mNumberFormatter), mUpdatedFrame);
				}
				catch(IndexOutOfRangeException)
				{
					mVagueBall = null;
				}
				return;
			}

			mVagueBall = null;
			distance = double.Parse(splitvalues[0], mNumberFormatter);
			direction = double.Parse(splitvalues[1], mNumberFormatter);

			Point2D prevPos = mBall.Position;
			mBall.Position = new Point2D(mSelf.Position, direction + mSelf.AbsoluteFaceDirection, distance);
			bool calcSpeed = false; 
			if(splitvalues.Length == 4)
			{
				mBall.DistanceChange = double.Parse(splitvalues[2], mNumberFormatter);
				mBall.DirectionChange = double.Parse(splitvalues[3], mNumberFormatter);
				calcSpeed = true;
			}

			if(!MathUtility.WithinPhysicalBoundary(this, mBall.Position))
				mBall.Position = mPositioner.CalculateClosestLegalPosition(mBall.Position);

			mBall.Direction = direction;
			mBall.Distance = distance;
			mBall.LastSeen = mUpdatedFrame;
			mBall.SeenThisCycle = true;
			
			if(calcSpeed)
				mBall.CalculateSpeed(Self);

		}

		/// <summary>
		/// Updates the position of the ball, with dead-reckogning
		/// </summary>
		private void updateBall()
		{
			SenseBody curSense = mPlayer.SenseBody;
			SenseBody prevSense = mPlayer.PreviousSenseBody;
			
			if(curSense == null || prevSense == null)
			{
				return;
			}

			int neckTurns = curSense.TurnNeckCount - prevSense.TurnNeckCount;
			int bodyTurns = curSense.TurnCount - prevSense.TurnCount;
			int kicks = curSense.KickCount - prevSense.KickCount;
			
			double directionChange = 0;
		
			if(neckTurns > 0)
			{
				TurnNeck command = (TurnNeck)mPlayer.PreviousCommandQueue.GetCommand(CommandType.Turn_Neck);
				if(command != null)
				{
					directionChange += command.Moment;
				}
			}
			if(bodyTurns > 0)
			{
				Turn command = (Turn)mPlayer.PreviousCommandQueue.GetCommand(CommandType.Turn);
				if(command != null)
				{
					directionChange += command.Moment;
				}
			}
			if(kicks > 0)
			{
				Kick command = (Kick)mPlayer.PreviousCommandQueue.GetCommand(CommandType.Kick);
				if(command != null)
				{
					//SpeedDirection
					directionChange += command.Direction;
					mBall.SpeedDirection = Self.AbsoluteBodyDirection + directionChange;

					//SpeedAmount
					double dir_diff = Math.Abs(mBall.Direction + Self.HeadAngle);
					double kickFactor = (1 - (0.25*(dir_diff/180)) - (0.25*(mBall.Distance/mPlayer.ServerParam.KickableMargin)));
					mBall.SpeedAmount = command.Power * mPlayer.ServerParam.KickPowerRate * kickFactor;
					
					if(mBall.DistanceChange < 0)
					{
						mBall.SpeedAmount += ((Self.SpeedAmount + mBall.SpeedAmount) * 0.1);
					}
				}
			}

			mBall.Position = mPositioner.CalculateDeadReckoningPosition(mBall.Position, mBall.SpeedDirection, mBall.SpeedAmount, 1);
			mBall.Direction -= directionChange;
			mBall.Direction = MathUtility.NormalizeAngle(mBall.Direction);
			mBall.Distance = mSelf.Position - mBall.Position;
			mBall.ClearForecasts();
		}

		/// <summary>
		/// Updates position and direction of the other players
		/// </summary>
		/// <param name="pSee">The data string with the seen objects</param>
		private void updateOtherPlayers(string pSee)
		{
			mPlayerReposit.ClearUnknownPlayers();
			
			int start, end, lastSeen;
			lastSeen = mUpdatedFrame;
			ArrayList TeamMates = new ArrayList();
			ArrayList Opponents = new ArrayList();
			
			start = indexOf(pSee, "((p", "((P");

			//Parse the whole see string
			while(start != -1)
			{
				int number = 0;
				bool isGoalie = false;
				string TeamName, values, name;
				string[] splitvalues, splitname;
				double distance = 0;
				double direction = 0;
				double directionChange = 0;
				double distanceChange = 0;
				double bodyFacingDir = 0;
				double headFacingDir = 0;

				start += 2;
				
				//Name
				end = pSee.IndexOf(')', start); // end of name
				name = pSee.Substring(start, end - start);
				splitname = name.Split(' ');

				//unknown player
				if(splitname.Length == 1)
				{
					TeamName = "";
					number = 0;
				}
				else
				{
					TeamName = splitname[1];
					TeamName = TeamName.Substring(1, TeamName.Length-2);
					if(splitname.Length == 4)
					{
						isGoalie = splitname[3].Equals("goalie") ? true : false;
					}
					if(splitname.Length >= 3)
					{
						number = int.Parse(splitname[2], mNumberFormatter);
					}
				}

				//Values: Distance Direction DistChange DirChange BodyFacingDir HeadFacingDir
				start = end + 2;
				end = pSee.IndexOf(')', start + 1);
				values = pSee.Substring(start, end-start);
				splitvalues = values.Split(' ');
				int noOfValues = splitvalues.Length;
				
				bool calcSpeed = false;

				if(noOfValues == 1)
				{
					direction = double.Parse(splitvalues[0], mNumberFormatter);
				}
				if(noOfValues >= 2)
				{
					distance = double.Parse(splitvalues[0], mNumberFormatter);
					direction = double.Parse(splitvalues[1], mNumberFormatter);
				}
				if(noOfValues >= 4)
				{
					distanceChange = double.Parse(splitvalues[2], mNumberFormatter);
					directionChange = double.Parse(splitvalues[3], mNumberFormatter);
					calcSpeed = true;
				}
				if(noOfValues == 6)
				{
					bodyFacingDir = double.Parse(splitvalues[4], mNumberFormatter);
					headFacingDir = double.Parse(splitvalues[5], mNumberFormatter);
				}

				Point2D position = new Point2D(mSelf.Position, direction + mSelf.AbsoluteFaceDirection, distance);
				
				if(!MathUtility.WithinPhysicalBoundary(this, position))
					position = mPositioner.CalculateClosestLegalPosition(position);

				FieldPlayer fp = new FieldPlayer(TeamName, number, isGoalie, directionChange, distanceChange, lastSeen, position);
				fp.Distance = distance;
				fp.Direction = direction;
				fp.SeenThisCycle = true;
				if(calcSpeed)
					fp.CalculateSpeed(Self);

				if(TeamName.Equals(mPlayer.TeamName)) //my team
				{
					TeamMates.Add(fp);
				}
				else if(TeamName.Equals("")) //unknown team
				{
					mPlayerReposit.AddUnknownPlayer(fp);
				}
				else //other team
				{
					Opponents.Add(fp);
				}

				start = indexOf(pSee, "((p", "((P", end);
			}

			//Angles
			foreach(FieldPlayer fp in TeamMates)
			{
				fp.AbsoluteBodyDirection = fp.BodyDirection + mSelf.AbsoluteBodyDirection;
				fp.AbsoluteFaceDirection = fp.FaceDirection + mSelf.AbsoluteFaceDirection;
			}
			foreach(FieldPlayer fp in Opponents)
			{
				fp.AbsoluteBodyDirection = fp.BodyDirection + mSelf.AbsoluteBodyDirection;
				fp.AbsoluteFaceDirection = fp.FaceDirection + mSelf.AbsoluteFaceDirection;
			}

			mPlayerReposit.UpdateMates(TeamMates);
			mPlayerReposit.UpdateOpponents(Opponents);
		}

		/// <summary>
		/// Updates position and direction of the other players, based on dead reckogning
		/// </summary>
		private void updateOtherPlayers()
		{
			ArrayList TeamMates = mPlayerReposit.TeamMates;
			ArrayList Opponents = mPlayerReposit.Opponents;
						
			foreach(FieldPlayer fp in TeamMates)
			{
				fp.BodyDirection = mSelf.AbsoluteBodyDirection - fp.AbsoluteBodyDirection;
				fp.FaceDirection = mSelf.AbsoluteFaceDirection - fp.AbsoluteFaceDirection;
				fp.SpeedDirection *= mPlayer.ServerParam.PlayerDecay;
				fp.Position = mPositioner.CalculateDeadReckoningPosition(fp.Position, fp.SpeedDirection, fp.SpeedAmount, 1);
				fp.Distance = Self.Position - fp.Position;
				fp.Direction = Self.Position.AngleTo(fp.Position);
				fp.ClearForecasts();
			}
			foreach(FieldPlayer fp in Opponents)
			{
				fp.BodyDirection = mSelf.AbsoluteBodyDirection - fp.AbsoluteBodyDirection;
				fp.FaceDirection = mSelf.AbsoluteFaceDirection - fp.AbsoluteFaceDirection;
				fp.SpeedDirection *= mPlayer.ServerParam.PlayerDecay;
				fp.Position = mPositioner.CalculateDeadReckoningPosition(fp.Position, fp.SpeedDirection, fp.SpeedAmount, 1);
				fp.Distance = Self.Position - fp.Position;
				fp.Direction = Self.Position.AngleTo(fp.Position);
				fp.ClearForecasts();
			}

			mPlayerReposit.UpdateMates(TeamMates);
			mPlayerReposit.UpdateOpponents(Opponents);
		}
		#endregion
		#endregion

		#region Help methods for string parsing
		/// <summary>
		/// Searches for the first appearance of any of two key strings in a search string
		/// </summary>
		/// <param name="pSearchString">The string to search in</param>
		/// <param name="pKeyString1">The first string to search for</param>
		/// <param name="pKeyString2">The second string to search for</param>
		/// <returns>The index of the first found string in the search string</returns>
		/// <remarks>Returns -1 if no of the two key strings where found in the search string</remarks>
		private int indexOf(string pSearchString, string pKeyString1, string pKeyString2)
		{
			int start = pSearchString.IndexOf(pKeyString1);
			start = start == -1 ? int.MaxValue : start;
			int alternativeStart = pSearchString.IndexOf(pKeyString2);
			alternativeStart = alternativeStart == -1 ? int.MaxValue : alternativeStart;
			start = Math.Min(start, alternativeStart);
			return start == int.MaxValue ? -1 : start;
		}

		/// <summary>
		/// Searches for the first appearance of any of two key strings in a search string
		/// </summary>
		/// <param name="pSearchString">The string to search in</param>
		/// <param name="pKeyString1">The first string to search for</param>
		/// <param name="pKeyString2">The second string to search for</param>
		/// <param name="pStartIndex">The index in the search string to start the search on</param>
		/// <returns>The index of the first found string in the search string</returns>
		/// <remarks>Returns -1 if no of the two key strings where found in the search string</remarks>
		private int indexOf(string pSearchString, string pKeyString1, string pKeyString2, int pStartIndex)
		{
			int start = pSearchString.IndexOf(pKeyString1, pStartIndex);
			start = start == -1 ? int.MaxValue : start;
			int alternativeStart = pSearchString.IndexOf(pKeyString2, pStartIndex);
			alternativeStart = alternativeStart == -1 ? int.MaxValue : alternativeStart;
			start = Math.Min(start, alternativeStart);
			return start == int.MaxValue ? -1 : start;
		}
		#endregion

		#region Properties

		/// <summary>
		/// The own position
		/// </summary>
		public Point2D MyPosition
		{
			get { return mSelf.Position; }
		}

		/// <summary>
		/// Returns the ball
		/// </summary>
		/// <value>The ball object.</value>
		public Ball TheBall
		{
			get { return mBall; }
		}

		/// <summary>
		/// Gets all teammates.
		/// </summary>
		/// <value>An ArrayList</value>
		public ArrayList TeamMates
		{
			get{return mPlayerReposit.TeamMates;}
		}

		/// <summary>
		/// Gets all opponents.
		/// </summary>
		/// <value>An ArrayList</value>
		public ArrayList Opponents
		{
			get{return mPlayerReposit.Opponents;}
		}

		/// <summary>
		/// Gets the representation of the player that is associated with this worldmodel.
		/// </summary>
		/// <value>A represention of ourself</value>
		public FieldPlayer Self
		{
			get { return mSelf; }
		}

		/// <summary>Gets all known team-mates</summary>
		public FieldPlayer[] AllTeamMates
		{
			get{ return mPlayerReposit.AllMates; }
		}

		/// <summary>Gets all known opponents</summary>
		public FieldPlayer[] AllOpponents
		{
			get{ return mPlayerReposit.AllOpponents; }
		}
		
		/// <summary>
		/// Returns the simulation frame when the worldmodel was updated.
		/// </summary>
		/// <value>Initially this will return <c>-1</c> until the worldmodel is updated.</value>
		public int UpdatedFrame
		{
			get { return mUpdatedFrame; }
		}

		/// <summary>
		/// Returns the static objects, e.g. flags and lines
		/// </summary>
		public Hashtable StaticObjects
		{
			get{ return mStaticObjects; }
		}

		/// <summary>
		/// Returns the vague information about the ball, if the last information about
		/// the ball from the server wasn't accurate enough to calculate the balls position.
		/// </summary>
		public VagueObject VagueBall
		{
			get{ return mVagueBall; }
		}

		internal Player Player
		{
			get{return mPlayer;}
		}
		#endregion

		#region Flags and other static objects
		/// <summary>Sets up the static objects, e.g. flags</summary>
		/// <returns>The static objects</returns>
		public static Hashtable initStaticObjects(Side pTeamSide)
		{
			int sideRegulator = pTeamSide == Side.Left ? 1 : -1;
			Hashtable staticObjects = new Hashtable();

			// perimeter flags clockwise from lower-left
			staticObjects.Add("f l b", new Point2D(sideRegulator * -52.5f, sideRegulator * 34));
			staticObjects.Add("f l b 30", new Point2D(sideRegulator * -57.5f, sideRegulator * 30));
			staticObjects.Add("f l b 20", new Point2D(sideRegulator * -57.5f, sideRegulator * 20));
			staticObjects.Add("f l b 10", new Point2D(sideRegulator * -57.5f, sideRegulator * 10));
			staticObjects.Add("f l 0", new Point2D(sideRegulator * -57.5f, sideRegulator * 0));
			staticObjects.Add("f l t 10", new Point2D(sideRegulator * -57.5f, sideRegulator * -10));
			staticObjects.Add("f l t 20", new Point2D(sideRegulator * -57.5f, sideRegulator * -20));
			staticObjects.Add("f l t 30", new Point2D(sideRegulator * -57.5f, sideRegulator * -30));
			staticObjects.Add("f l t", new Point2D(sideRegulator * -52.5f, sideRegulator * -34));
			staticObjects.Add("f t l 50", new Point2D(sideRegulator * -50, sideRegulator * -39));
			staticObjects.Add("f t l 40", new Point2D(sideRegulator * -40, sideRegulator * -39));
			staticObjects.Add("f t l 30", new Point2D(sideRegulator * -30, sideRegulator * -39));
			staticObjects.Add("f t l 20", new Point2D(sideRegulator * -20, sideRegulator * -39));
			staticObjects.Add("f t l 10", new Point2D(sideRegulator * -10, sideRegulator * -39));
			staticObjects.Add("f t 0", new Point2D(sideRegulator * 0, sideRegulator * -39));
			staticObjects.Add("f t r 10", new Point2D(sideRegulator * 10, sideRegulator * -39));
			staticObjects.Add("f t r 20", new Point2D(sideRegulator * 20, sideRegulator * -39));
			staticObjects.Add("f t r 30", new Point2D(sideRegulator * 30, sideRegulator * -39));
			staticObjects.Add("f t r 40", new Point2D(sideRegulator * 40, sideRegulator * -39));
			staticObjects.Add("f t r 50", new Point2D(sideRegulator * 50, sideRegulator * -39));
			staticObjects.Add("f r t", new Point2D(sideRegulator * 52.5f, sideRegulator * -34));
			staticObjects.Add("f r t 30", new Point2D(sideRegulator * 57.5f, sideRegulator * -30));
			staticObjects.Add("f r t 20", new Point2D(sideRegulator * 57.5f, sideRegulator * -20));
			staticObjects.Add("f r t 10", new Point2D(sideRegulator * 57.5f, sideRegulator * -10));
			staticObjects.Add("f r 0", new Point2D(sideRegulator * 57.5f, sideRegulator * 0));
			staticObjects.Add("f r b 10", new Point2D(sideRegulator * 57.5f, sideRegulator * 10));
			staticObjects.Add("f r b 20", new Point2D(sideRegulator * 57.5f, sideRegulator * 20));
			staticObjects.Add("f r b 30", new Point2D(sideRegulator * 57.5f, sideRegulator * 30));
			staticObjects.Add("f r b", new Point2D(sideRegulator * 52.5f, sideRegulator * 34));
			staticObjects.Add("f b r 50", new Point2D(sideRegulator * 50, sideRegulator * 39));
			staticObjects.Add("f b r 40", new Point2D(sideRegulator * 40, sideRegulator * 39));
			staticObjects.Add("f b r 30", new Point2D(sideRegulator * 30, sideRegulator * 39));
			staticObjects.Add("f b r 20", new Point2D(sideRegulator * 20, sideRegulator * 39));
			staticObjects.Add("f b r 10", new Point2D(sideRegulator * 10, sideRegulator * 39));
			staticObjects.Add("f b 0", new Point2D(sideRegulator * 0, sideRegulator * 39));
			staticObjects.Add("f b l 10", new Point2D(sideRegulator * -10, sideRegulator * 39));
			staticObjects.Add("f b l 20", new Point2D(sideRegulator * -20, sideRegulator * 39));
			staticObjects.Add("f b l 30", new Point2D(sideRegulator * -30, sideRegulator * 39));
			staticObjects.Add("f b l 40", new Point2D(sideRegulator * -40, sideRegulator * 39));
			staticObjects.Add("f b l 50", new Point2D(sideRegulator * -50, sideRegulator * 39));

			// goals and penalty area fs
			staticObjects.Add("g l", new Point2D(sideRegulator * -52.5f, sideRegulator * 0));
			staticObjects.Add("f g l b", new Point2D(sideRegulator * -52.5f, sideRegulator * 7.01f));
			staticObjects.Add("f g l t", new Point2D(sideRegulator * -52.5f, sideRegulator * -7.01f));
			staticObjects.Add("f p l b", new Point2D(sideRegulator * -36, sideRegulator * 20.16f));
			staticObjects.Add("f p l c", new Point2D(sideRegulator * -36, sideRegulator * 0));
			staticObjects.Add("f p l t", new Point2D(sideRegulator * -36, sideRegulator * -20.16f));
			staticObjects.Add("g r", new Point2D(sideRegulator * 52.5f, sideRegulator * 0));
			staticObjects.Add("f g r b", new Point2D(sideRegulator * 52.5f, sideRegulator * 7.01f));
			staticObjects.Add("f g r t", new Point2D(sideRegulator * 52.5f, sideRegulator * -7.01f));
			staticObjects.Add("f p r b", new Point2D(sideRegulator * 36, sideRegulator * 20.16f));
			staticObjects.Add("f p r c", new Point2D(sideRegulator * 36, sideRegulator * 0));
			staticObjects.Add("f p r t", new Point2D(sideRegulator * 36, sideRegulator * -20.16f));

			// center fs
			staticObjects.Add("f c", new Point2D(sideRegulator * 0, sideRegulator * 0));
			staticObjects.Add("f c b", new Point2D(sideRegulator * 0, sideRegulator * 34));
			staticObjects.Add("f c t", new Point2D(sideRegulator * 0, sideRegulator * -34));

			// lines
			// TODO: change lines to be real lines and not just a point
			staticObjects.Add("l t", new Point2D(sideRegulator * 0, sideRegulator * -34));
			staticObjects.Add("l b", new Point2D(sideRegulator * 0, sideRegulator * 34));
			staticObjects.Add("l l", new Point2D(sideRegulator * -52.5f, sideRegulator * 0));
			staticObjects.Add("l r", new Point2D(sideRegulator * 52.5f, sideRegulator * 0));

			return staticObjects;
		}
		#endregion

		#region Events
		/// <summary>Event for Flags parsed</summary>
		public event FlagsEventHandler FlagsParsed;
		/// <summary>Event for Intersections calculated</summary>
		public event IntersectionsEventHandler IntersectionsCalculated;
		/// <summary>Event for position of player calculated</summary>
		public event PositionEventHandler PositionCalculated;
		/// <summary>Event for ball position calculated</summary>
		public event PositionEventHandler BallPositionCalculated;
		/// <summary>Event for team mates calculated</summary>
		public event TeamMatesEventHandler TeamMatesCalculated;
		/// <summary>Event for opponents calculated</summary>
		public event OpponentsEventHandler OpponentsCalculated;
		/// <summary>Event for player calculated</summary>
		public event PlayerCalculatedEventHandler PlayerCalculated;
		/// <summary>Event indicating that the players own position is unknown</summary>
		public event PositionUnknownEventHandler PositionUnknown;

		/// <summary>
		/// Generates a FlagsParsed event when all flags have been parsed
		/// </summary>
		/// <param name="e">The event arguments</param>
		protected void OnFlagsParsed(FlagsEventArgs e)
		{
			if(FlagsParsed != null)
			{
				FlagsParsed(this, e);
			}
		}

		/// <summary>
		/// Generates a IntersectionsCalculated when all intersections have been calculated
		/// </summary>
		/// <param name="e">The event arguments</param>
		protected void OnIntersectionsCalculated(IntersectionsEventArgs e)
		{
			if(IntersectionsCalculated != null)
			{
				IntersectionsCalculated(this, e);
			}
		}

		/// <summary>
		/// Generates a PositionCalculated event when the players own position has been calculated
		/// </summary>
		/// <param name="e">The event arguments</param>
		protected void OnPositionCalculated(PositionEventArgs e)
		{
			if(PositionCalculated != null)
			{
				PositionCalculated(this, e);
			}
		}

		/// <summary>
		/// Generates a PlayerCalculated event when the player has been updated
		/// </summary>
		/// <param name="e">The event arguments</param>
		protected void OnPlayerCalculated(FieldPlayerEventArgs e)
		{
			if(PlayerCalculated != null)
			{
				PlayerCalculated(this, e);
			}
		}

		/// <summary>
		/// Generates a BallPositionCalculated event when the position of the ball has calculated
		/// </summary>
		/// <param name="e">The event arguments</param>
		protected void OnBallPositionCalculated(PositionEventArgs e)
		{
			if(BallPositionCalculated != null)
			{
				BallPositionCalculated(this, e);
			}
		}


		/// <summary>
		/// Generates a TeamMates calculated event when all team mates have been calulated
		/// </summary>
		/// <param name="e">The event arguments</param>
		protected void OnTeamMatesCalculated(FieldPlayerEventArgs e)
		{
			if(TeamMatesCalculated != null)
			{
				TeamMatesCalculated(this, e);
			}
		}

		/// <summary>
		/// Generates a OpponentsCalculated event when all have been calculated
		/// </summary>
		/// <param name="e">The event arguments</param>
		protected void OnOpponentsCalculated(FieldPlayerEventArgs e)
		{
			if(OpponentsCalculated != null)
			{
				OpponentsCalculated(this, e);
			}
		}

		/// <summary>
		/// Generates a PositionUnknown event when own position is unknown
		/// </summary>
		protected void OnPositionUnknown()
		{
			if(PositionUnknown != null)
			{
				PositionUnknown(this, EventArgs.Empty);
			}
		}
		#endregion
	}
}
