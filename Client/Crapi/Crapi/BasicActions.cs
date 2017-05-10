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
using TeamYaffa.CRaPI.World;
using TeamYaffa.CRaPI.Utility;
using TeamYaffa.CRaPI.Info;
using TeamYaffa.CRaPI.Commands;
using TeamYaffa.CRaPI.World.GameObjects;

namespace TeamYaffa.CRaPI
{
	/// <summary>
	/// This class contains static methods for the basic actions available
	/// <seealso cref="TeamYaffa.CRaPI.BasicCommands"/>
	/// </summary>
	/// <remarks>The methods in this class simplify some of the basic actions a player can do.
	/// <para>The actions available here, are the ones described in the Robocup soccer server manual.
	/// You are encouraged to read the manual for more details of the actions.</para></remarks>
	public class BasicActions
	{
		#region Look
		/// <overloads>
		/// Generates a command to turn the neck to look at an object
		/// </overloads>
		/// <remarks>It is not possible to turn the neck, if the angle to the point is more than
		/// <see cref="TeamYaffa.CRaPI.Info.ServerParam.MaxNeckAngle">MaxNeckAngle</see> or less than
		/// <see cref="TeamYaffa.CRaPI.Info.ServerParam.MinNeckAngle">MinNeckAngle</see> relative the players
		/// <see cref="TeamYaffa.CRaPI.World.GameObjects.FieldPlayer.AbsoluteBodyDirection">body direction</see>
		/// <para>If it is not possible to turn the neck sufficiently (looking directly at the object),
		/// the turn_neck will be as good as possible, i.e. the <see cref="TeamYaffa.CRaPI.Info.SenseBody.HeadAngle"/>
		/// will be either <see cref="TeamYaffa.CRaPI.Info.ServerParam.MaxNeckAngle">MaxNeckAngle</see> or
		/// <see cref="TeamYaffa.CRaPI.Info.ServerParam.MinNeckAngle">MinNeckAngle</see> after the command.</para>
		/// <para>This method does not send any command to the server, it is therefore up to the agent
		/// to execute the command on the server.</para></remarks>
		/// <param name="pPlayer">The player.</param>
		/// <param name="pObject">The object to look at.</param>
		/// <param name="action">The action command for the turn-neck.</param>
		/// <seealso cref="LookAtPoint"/>
		public static void LookAtObject(Player pPlayer, FieldObject pObject, out Command action)
		{
			action = LookAtPoint(pPlayer, pObject.Position);
		}

		/// <remarks>It is not possible to turn the neck, if the angle to the point is more than
		/// <see cref="TeamYaffa.CRaPI.Info.ServerParam.MaxNeckAngle">MaxNeckAngle</see> or less than
		/// <see cref="TeamYaffa.CRaPI.Info.ServerParam.MinNeckAngle">MinNeckAngle</see> relative the players
		/// <see cref="TeamYaffa.CRaPI.World.GameObjects.FieldPlayer.AbsoluteBodyDirection">body direction</see>
		/// <para>If it is not possible to turn the neck sufficiently (looking directly at the object),
		/// the turn_neck will be as good as possible, i.e. the <see cref="TeamYaffa.CRaPI.Info.SenseBody.HeadAngle"/>
		/// will be either <see cref="TeamYaffa.CRaPI.Info.ServerParam.MaxNeckAngle">MaxNeckAngle</see> or
		/// <see cref="TeamYaffa.CRaPI.Info.ServerParam.MinNeckAngle">MinNeckAngle</see> after the command.</para>
		/// <para>This method does not send any command to the server, it is therefore up to the agent
		/// to execute the command on the server.</para>
		/// </remarks>
		/// <param name="pPlayer">The player.</param>
		/// <param name="pObject">The object to look at.</param>
		/// <returns>The action command for the turn-neck.</returns>
		/// <seealso cref="LookAtPoint"/>
		public static Command LookAtObject(Player pPlayer, FieldObject pObject)
		{
			return LookAtPoint(pPlayer, pObject.Position);
		}

		/// <overloads>
		/// Generates a command to turn the neck to look at a point
		/// </overloads>
		/// <remarks>It is not possible to turn the neck, if the angle to the point is more than
		/// <see cref="TeamYaffa.CRaPI.Info.ServerParam.MaxNeckAngle">MaxNeckAngle</see> or less than
		/// <see cref="TeamYaffa.CRaPI.Info.ServerParam.MinNeckAngle">MinNeckAngle</see> relative the players
		/// <see cref="TeamYaffa.CRaPI.World.GameObjects.FieldPlayer.AbsoluteBodyDirection">body direction</see>
		/// <para>If it is not possible to turn the neck sufficiently (looking directly at the point),
		/// the turn_neck will be as good as possible, i.e. the <see cref="TeamYaffa.CRaPI.Info.SenseBody.HeadAngle"/>
		/// will be either <see cref="TeamYaffa.CRaPI.Info.ServerParam.MaxNeckAngle">MaxNeckAngle</see> or
		/// <see cref="TeamYaffa.CRaPI.Info.ServerParam.MinNeckAngle">MinNeckAngle</see> after the command.</para>
		/// <para>This method does not send any command to the server, it is therefore up to the agent
		/// to execute the command on the server.</para>
		/// </remarks>
		/// <param name="pPlayer">The player.</param>
		/// <param name="pPoint">The point to look at.</param>
		/// <param name="action">The action command for the turn-neck.</param>
		/// <seealso cref="LookAtObject"/>
		public static void LookAtPoint(Player pPlayer, Point2D pPoint, out Command action)
		{
			action = LookAtPoint(pPlayer, pPoint);
		}

		/// <remarks>It is not possible to turn the neck, if the angle to the point is more than
		/// <see cref="TeamYaffa.CRaPI.Info.ServerParam.MaxNeckAngle">MaxNeckAngle</see> or less than
		/// <see cref="TeamYaffa.CRaPI.Info.ServerParam.MinNeckAngle">MinNeckAngle</see> relative the players
		/// <see cref="TeamYaffa.CRaPI.World.GameObjects.FieldPlayer.AbsoluteBodyDirection">body direction</see>
		/// <para>If it is not possible to turn the neck sufficiently (looking directly at the point),
		/// the turn_neck will be as good as possible, i.e. the <see cref="TeamYaffa.CRaPI.Info.SenseBody.HeadAngle"/>
		/// will be either <see cref="TeamYaffa.CRaPI.Info.ServerParam.MaxNeckAngle">MaxNeckAngle</see> or
		/// <see cref="TeamYaffa.CRaPI.Info.ServerParam.MinNeckAngle">MinNeckAngle</see> after the command.</para>
		/// <para>This method does not send any command to the server, it is therefore up to the agent
		/// to execute the command on the server.</para>
		/// </remarks>
		/// <param name="pPlayer">The player.</param>
		/// <param name="pPoint">The point to look at.</param>
		/// <returns>The action command for the turn-neck.</returns>
		/// <seealso cref="LookAtObject"/>
		public static Command LookAtPoint(Player pPlayer, Point2D pPoint)
		{
			//Calulate angle
			FieldPlayer self = pPlayer.World.Self;
			double angle = self.Position.AngleTo(pPoint) - self.AbsoluteFaceDirection;

			//Check if angle is possible
			int minneckangle = pPlayer.ServerParam.MinNeckAngle;
			int maxneckangle = pPlayer.ServerParam.MaxNeckAngle;

			int angleAfterTurn = (int)(self.HeadAngle + angle);

			if(angleAfterTurn > maxneckangle || angleAfterTurn < minneckangle)
			{
				return BasicCommands.TurnNeck((int)(maxneckangle - self.HeadAngle));
			}
			else
			{
				return BasicCommands.TurnNeck((int)angle);
			}
		}
		#endregion

		#region Turn
		/// <overloads>
		/// Generates a command to turn the body towards an object.
		/// </overloads>
		/// <remarks>The player is turned so the <see cref="TeamYaffa.CRaPI.World.GameObjects.FieldPlayer.AbsoluteBodyDirection"/>
		/// equals the angle between the players position and the object. This means that even if the object is
		/// visible before the turn, the object could go out of view after the turn. This method just makes sure the
		/// body has the right angle towards the object.
		/// <para>This method does not send any command to the server, it is therefore up to the agent
		/// to execute the command on the server.</para>
		/// </remarks>
		/// <param name="pPlayer">The player</param>
		/// <param name="pObject">The object to look at</param>
		/// <param name="action">The action command for the turn</param>
		/// <seealso cref="TurnToPoint"/>
		public static void TurnToObject(Player pPlayer, FieldObject pObject, out Command action)
		{
			action = TurnToPoint(pPlayer, pObject.Position);
		}

		/// <remarks>The player is turned so the <see cref="TeamYaffa.CRaPI.World.GameObjects.FieldPlayer.AbsoluteBodyDirection"/>
		/// equals the angle between the players position and the object. This means that even if the object is
		/// visible before the turn, the object could go out of view after the turn. This method just makes sure the
		/// body has the right angle towards the object.
		/// <para>This method does not send any command to the server, it is therefore up to the agent
		/// to execute the command on the server.</para>
		/// </remarks>
		/// <param name="pPlayer">The player</param>
		/// <param name="pObject">The object to look at</param>
		/// <returns>The action command for the turn</returns>
		/// <seealso cref="TurnToPoint"/>
		public static Command TurnToObject(Player pPlayer, FieldObject pObject)
		{
			return TurnToPoint(pPlayer, pObject.Position);
		}

		/// <overloads>
		/// Generates a command to turn the body towards a point.
		/// </overloads>
		/// <remarks>The player is turned so the <see cref="TeamYaffa.CRaPI.World.GameObjects.FieldPlayer.AbsoluteBodyDirection"/>
		/// equals the angle between the players position and the point. This means that even if the point is
		/// visible before the turn, the point could go out of view after the turn. This method just makes sure the
		/// body has the right angle towards the point.
		/// <para>This method does not send any command to the server, it is therefore up to the agent
		/// to execute the command on the server.</para>
		/// </remarks>
		/// <param name="pPlayer">The player</param>
		/// <param name="pPoint">The point to look at</param>
		/// <param name="action">The action command for the turn</param>
		/// <seealso cref="TurnToObject"/>
		public static void TurnToPoint(Player pPlayer, Point2D pPoint, out Command action)
		{
			action = TurnToPoint(pPlayer, pPoint);
		}

		/// <remarks>The player is turned so the <see cref="TeamYaffa.CRaPI.World.GameObjects.FieldPlayer.AbsoluteBodyDirection"/>
		/// equals the angle between the players position and the point. This means that even if the point is
		/// visible before the turn, the point could go out of view after the turn. This method just makes sure the
		/// body has the right angle towards the point.
		/// <para>This method does not send any command to the server, it is therefore up to the agent
		/// to execute the command on the server.</para>
		/// </remarks>
		/// <param name="pPlayer">The player</param>
		/// <param name="pPoint">The point to look at</param>
		/// <returns>The action command for the turn</returns>
		/// <seealso cref="TurnToObject"/>
		public static Command TurnToPoint(Player pPlayer, Point2D pPoint)
		{
			double diffAngle = MathUtility.NormalizeAngle(pPlayer.World.MyPosition.AngleTo(pPoint) - pPlayer.World.Self.AbsoluteBodyDirection);

			if(diffAngle > pPlayer.ServerParam.MaxMoment)
				diffAngle = pPlayer.ServerParam.MaxMoment;
			else if(diffAngle < pPlayer.ServerParam.MinMoment)
				diffAngle = pPlayer.ServerParam.MinMoment;
			
			return BasicCommands.Turn((int)diffAngle);
		}

		/// <summary>
		/// Generates a command to turn the body towards a point, and sets the
		/// <see cref="TeamYaffa.CRaPI.World.GameObjects.FieldPlayer.HeadAngle">HeadAngle</see>
		/// to zero, i.e. the player is looking in the direction of the body.
		/// </summary>
		/// <remarks>This method does not send any command to the server, it is therefore up to the agent
		/// to execute the command on the server.</remarks>
		/// <param name="pPlayer">The player</param>
		/// <param name="pPoint">The point to turn to</param>
		/// <returns>The action commands for the turn and turn-neck</returns>
		/// <seealso cref="TurnToPoint"/>
		/// <seealso cref="LookAtPoint"/>
		public static Command[] TurnAndLookAtPoint(Player pPlayer, Point2D pPoint)
		{
			Command[] commands = new Command[2];
			commands[0] = TurnToPoint(pPlayer, pPoint);
			commands[1] = new TurnNeck(-(int)pPlayer.World.Self.HeadAngle);
			return commands;
		}
		#endregion

		#region Dash
		#region DashToObject
		/// <overloads>
		/// Generates a command to dash to an object
		/// </overloads>
		/// <remarks>Dashes to the object if the angle to the object is +/- 10 degress. If the angle
		/// exceeds the 10 degree margin, a turn command is created so the angle to the
		/// object will be 0 degrees in the next cycle, provided that that neither the player nor the
		/// object has changed position.
		/// <para>This method does not send any command to the server, it is therefore up to the agent
		/// to execute the command on the server.</para>
		/// </remarks>
		/// <param name="pPlayer">The player</param>
		/// <param name="pObject">The object to dash to</param>
		/// <param name="pPower">The power to dash with. Should not exceed
		/// <see cref="TeamYaffa.CRaPI.Info.ServerParam.MaxPower"/> and
		/// <see cref="TeamYaffa.CRaPI.Info.ServerParam.MinPower"/>.</param>
		/// <param name="pAction">The action command for the dash</param>
		/// <seealso cref="DashToPoint"/>
		public static void DashToObject(Player pPlayer, FieldObject pObject, int pPower, out Command pAction)
		{
			pAction = DashToPoint(pPlayer, pObject.Position, pPower, 10);
		}

		/// <remarks>Dashes to the object if the angle to the object is +/- 10 degress. If the angle
		/// exceeds the 10 degree margin, a turn command is created so the angle to the
		/// object will be 0 degrees in the next cycle.
		/// <para>This method does not send any command to the server, it is therefore up to the agent
		/// to execute the command on the server.</para>
		/// </remarks>
		/// <param name="pPlayer">The player</param>
		/// <param name="pObject">The object to dash to</param>
		/// <param name="pPower">The power to dash with. Should not exceed
		/// <see cref="TeamYaffa.CRaPI.Info.ServerParam.MaxPower"/> and
		/// <see cref="TeamYaffa.CRaPI.Info.ServerParam.MinPower"/>.</param>
		/// <returns>The action command for the dash</returns>
		/// <seealso cref="DashToPoint"/>
		public static Command DashToObject(Player pPlayer, FieldObject pObject, int pPower)
		{
			return DashToPoint(pPlayer, pObject.Position, pPower, 10);
		}

		/// <remarks>Dashes to the object if the angle to the object is +/- <paramref name="pAngleMargin"/>
		/// degress. If the angle exceeds the <paramref name="pAngleMargin"/>, a turn command is created
		/// so the angle to the object will be 0 degrees in the next cycle.
		/// <para>This method does not send any command to the server, it is therefore up to the agent
		/// to execute the command on the server.</para>
		/// </remarks>
		/// <param name="pPlayer">The player</param>
		/// <param name="pObject">The object to dash to</param>
		/// <param name="pPower">The power to dash with. Should not exceed
		/// <see cref="TeamYaffa.CRaPI.Info.ServerParam.MaxPower"/> and
		/// <see cref="TeamYaffa.CRaPI.Info.ServerParam.MinPower"/>.</param>
		/// <param name="pAngleMargin">The angle margin</param>
		/// <param name="pAction">The action command for the dash</param>
		/// <seealso cref="DashToPoint"/>
		public static void DashToObject(Player pPlayer, FieldObject pObject, int pPower, uint pAngleMargin, out Command pAction)
		{
			pAction = DashToPoint(pPlayer, pObject.Position, pPower, pAngleMargin);
		}

		/// <remarks>Dashes to the object if the angle to the object is +/- <paramref name="pAngleMargin"/>
		/// degress. If the angle exceeds the <paramref name="pAngleMargin"/>, a turn command is created
		/// so the angle to the object will be 0 degrees in the next cycle.
		/// <para>This method does not send any command to the server, it is therefore up to the agent
		/// to execute the command on the server.</para>
		/// </remarks>
		/// <param name="pPlayer">The player</param>
		/// <param name="pObject">The object to dash to</param>
		/// <param name="pPower">The power to dash with. Should not exceed
		/// <see cref="TeamYaffa.CRaPI.Info.ServerParam.MaxPower"/> and
		/// <see cref="TeamYaffa.CRaPI.Info.ServerParam.MinPower"/>.</param>
		/// <param name="pAngleMargin">The angle margin</param>
		/// <returns>The action command for the dash</returns>
		/// <seealso cref="DashToPoint"/>
		public static Command DashToObject(Player pPlayer, FieldObject pObject, int pPower, uint pAngleMargin)
		{
			return DashToPoint(pPlayer, pObject.Position, pPower, pAngleMargin);
		}
		#endregion

		#region DashToPoint
		/// <overloads>
		/// Generates a command to dash to a point
		/// </overloads>
		/// <remarks>Dashes to the point if the angle to the point is +/- 10 degress. If the angle
		/// exceeds the 10 degree margin, a turn command is created so the angle to the
		/// point will be 0 degrees in the next cycle.
		/// <para>This method does not send any command to the server, it is therefore up to the agent
		/// to execute the command on the server.</para>
		/// </remarks>
		/// <param name="pPlayer">The player</param>
		/// <param name="pPoint">The point to dash to</param>
		/// <param name="pPower">The power to dash with. Should not exceed
		/// <see cref="TeamYaffa.CRaPI.Info.ServerParam.MaxPower"/> and
		/// <see cref="TeamYaffa.CRaPI.Info.ServerParam.MinPower"/>.</param>
		/// <param name="pAction">The action command for the dash</param>
		/// <seealso cref="DashToObject"/>
		public static void DashToPoint(Player pPlayer, Point2D pPoint, int pPower, out Command pAction)
		{
			pAction = DashToPoint(pPlayer, pPoint, pPower, 10);
		}

		/// <remarks>Dashes to the point if the angle to the point is +/- 10 degress. If the angle
		/// exceeds the 10 degree margin, a turn command is created so the angle to the
		/// point will be 0 degrees in the next cycle.
		/// <para>This method does not send any command to the server, it is therefore up to the agent
		/// to execute the command on the server.</para>
		/// </remarks>
		/// <param name="pPlayer">The player</param>
		/// <param name="pPoint">The point to dash to</param>
		/// <param name="pPower">The power to dash with. Should not exceed
		/// <see cref="TeamYaffa.CRaPI.Info.ServerParam.MaxPower"/> and
		/// <see cref="TeamYaffa.CRaPI.Info.ServerParam.MinPower"/>.</param>
		/// <returns>The action command for the dash</returns>
		/// <seealso cref="DashToObject"/>
		public static Command DashToPoint(Player pPlayer, Point2D pPoint, int pPower)
		{
			return DashToPoint(pPlayer, pPoint, pPower, 10);
		}

		/// <remarks>Dashes to the point if the angle to the point is +/- <paramref name="pAngleMargin"/>
		/// degress. If the angle exceeds the <paramref name="pAngleMargin"/>, a turn command is created
		/// so the angle to the point will be 0 degrees in the next cycle.
		/// <para>This method does not send any command to the server, it is therefore up to the agent
		/// to execute the command on the server.</para>
		/// </remarks>
		/// <param name="pPlayer">The player</param>
		/// <param name="pPoint">The point to dash to</param>
		/// <param name="pPower">The power to dash with. Should not exceed
		/// <see cref="TeamYaffa.CRaPI.Info.ServerParam.MaxPower"/> and
		/// <see cref="TeamYaffa.CRaPI.Info.ServerParam.MinPower"/>.</param>
		/// <param name="pAngleMargin">The angle margin</param>
		/// <param name="pAction">The action command for the dash</param>
		/// <seealso cref="DashToObject"/>
		public static void DashToPoint(Player pPlayer, Point2D pPoint, int pPower, uint pAngleMargin, out Command pAction)
		{
			pAction = DashToPoint(pPlayer, pPoint, pPower, pAngleMargin);
		}

		/// <remarks>Dashes to the point if the angle to the point is +/- <paramref name="pAngleMargin"/>
		/// degress. If the angle exceeds the <paramref name="pAngleMargin"/>, a turn command is created
		/// so the angle to the point will be 0 degrees in the next cycle.
		/// <para>This method does not send any command to the server, it is therefore up to the agent
		/// to execute the command on the server.</para>
		/// </remarks>
		/// <param name="pPlayer">The player</param>
		/// <param name="pPoint">The point to dash to</param>
		/// <param name="pPower">The power to dash with. Should not exceed
		/// <see cref="TeamYaffa.CRaPI.Info.ServerParam.MaxPower"/> and
		/// <see cref="TeamYaffa.CRaPI.Info.ServerParam.MinPower"/>.</param>
		/// <param name="pAngleMargin">The angle margin</param>
		/// <returns>The action command for the dash</returns>
		/// <seealso cref="DashToObject"/>
		public static Command DashToPoint(Player pPlayer, Point2D pPoint, int pPower, uint pAngleMargin)
		{
			double diffAngle = MathUtility.NormalizeAngle(pPlayer.World.MyPosition.AngleTo(pPoint) - pPlayer.World.Self.AbsoluteBodyDirection);
			//Turn
			if(diffAngle > pAngleMargin || diffAngle < (pAngleMargin * -1))
			{
				return TurnToPoint(pPlayer, pPoint);
			}
			else
			{
				return BasicCommands.Dash(pPower);
			}
		}
		#endregion
		#endregion

		#region DashToPointLookingAtPoint
		/// <overloads>
		/// Generates a command to dash to one point while looking at another.
		/// </overloads>
		/// <remarks>
		/// If the angle between the look-point and the run-point is greater than
		/// <see cref="TeamYaffa.CRaPI.Info.ServerParam.MaxNeckAngle">MaxNeckAngle</see> or lesser than
		/// <see cref="TeamYaffa.CRaPI.Info.ServerParam.MinNeckAngle">MinNeckAngle</see>, then consider
		/// to change the view-width of the agent to make sure the agent can see the look-point.
		/// <para>The angle-margin for when to turn and when to dash is +/- 5 degrees.</para>
		/// <para>This method does not send any command to the server, it is therefore up to the agent
		/// to execute the command on the server.</para>
		/// </remarks>
		/// <param name="pPlayer">The agent</param>
		/// <param name="pDashPoint">The point to dash to</param>
		/// <param name="pPower">What power to use when dashing</param>
		/// <param name="pLookPoint">The point to look at</param>
		/// <returns>The action commands for the dash and the turn-neck</returns>
		/// <seealso cref="DashToPoint"/>
		/// <seealso cref="LookAtPoint"/>
		public static Command[] DashToPointLookingAtPoint(Player pPlayer, Point2D pDashPoint, int pPower, Point2D pLookPoint)
		{
			return DashToPointLookingAtPoint(pPlayer, pDashPoint, pPower, 5, pLookPoint);
		}
		
		/// <remarks>
		/// If the angle between the look-point and the run-point is greater than
		/// <see cref="TeamYaffa.CRaPI.Info.ServerParam.MaxNeckAngle">MaxNeckAngle</see> or lesser than
		/// <see cref="TeamYaffa.CRaPI.Info.ServerParam.MinNeckAngle">MinNeckAngle</see>, then consider
		/// to change the view-width of the agent to make sure the agent can see the look-point.
		/// <para>This method does not send any command to the server, it is therefore up to the agent
		/// to execute the command on the server.</para>
		/// </remarks>
		/// <param name="pPlayer">The agent</param>
		/// <param name="pDashPoint">The point to dash to</param>
		/// <param name="pPower">What power to use when dashing</param>
		/// <param name="pAngleMargin">The angle-margin for when to turn, and when to dash.</param>
		/// <param name="pLookPoint">The point to look at</param>
		/// <returns>The action commands for the dash and the turn-neck</returns>
		/// <seealso cref="DashToPoint"/>
		/// <seealso cref="LookAtPoint"/>
		public static Command[] DashToPointLookingAtPoint(Player pPlayer, Point2D pDashPoint, int pPower, uint pAngleMargin, Point2D pLookPoint)
		{
			double turnAngle = MathUtility.NormalizeAngle(pPlayer.World.MyPosition.AngleTo(pDashPoint) - pPlayer.World.Self.AbsoluteBodyDirection);
			bool shouldTurn = turnAngle > pAngleMargin || turnAngle < (pAngleMargin * -1);
			Command dashCommand = (shouldTurn) ? TurnToPoint(pPlayer, pDashPoint) : BasicCommands.Dash(pPower);

			double wantedNeckAngle = MathUtility.NormalizeAngle(pPlayer.World.MyPosition.AngleTo(pLookPoint) - pPlayer.World.Self.AbsoluteBodyDirection);
			if(shouldTurn)
			{
				wantedNeckAngle -= ((Turn)dashCommand).Moment;
			}

			if(wantedNeckAngle > pPlayer.ServerParam.MaxNeckAngle)
				wantedNeckAngle = pPlayer.ServerParam.MaxNeckAngle;
			else if(wantedNeckAngle < pPlayer.ServerParam.MinNeckAngle)
				wantedNeckAngle = pPlayer.ServerParam.MinNeckAngle;

			wantedNeckAngle -= pPlayer.World.Self.HeadAngle;
			Command turnNeckCommand = new TurnNeck((int)(wantedNeckAngle));
			return new Command[]{dashCommand, turnNeckCommand};
		}
		#endregion

		#region Will be considered later
//		/// <summary>
//		/// Kicks the ball to a point (should stop at this point)
//		/// </summary>
//		/// <param name="pBall">The ball</param>
//		/// <param name="pPoint">The position to kick to</param>
//		/// <param name="action">The action string</param>
//		public static void KickToPoint(Player pPlayer, Ball pBall, Point2D pPoint, out Command action)
//		{
//			double distance = pPlayer.World.MyPosition - pPoint;
//			
//			//kolla först om det går att sparka så långt
//			double power;
//			double effectiveKickPower;
//
//			double kickDirection = Math.Abs(pBall.Direction - pPlayer.World.Self.NeckAngle);
//			double ballSpeedMax = pPlayer.ServerParam.BallSpeedMax; //max hatighet
//			double accelerateX= Power * pPlayer.ServerParam.KickPowerRate * Math.Cos(kickDirection);
//			double accelerateY= Power * pPlayer.ServerParam.KickPowerRate * Math.Sin(kickDirection);
//		
//			double decay = pPlayer.ServerParam.BallDecay;
//
//
//			Point2D = possition = pBall.Position;
//			do
//			{
//				
//
//			}while();
//			
//
//				double kickPower = kick * pPlayer.ServerParam.KickPowerRate;
//			effectiveKickPower = kickPower * (1 - 0.25*(kickDirection/180) - 0.25*(pBall.Distance/pPlayer.ServerParam.KickableMargin)));
//
//			action = BasicCommands.Kick(pPower, angle);
//		}
		#endregion

		#region Kick
		/// <overloads>
		/// Generates a command to kick the ball towards an object
		/// </overloads>
		/// <remarks>This method does not send any command to the server, it is therefore up to the agent
		/// to execute the command on the server.</remarks>
		/// <param name="pPlayer">The player</param>
		/// <param name="pObject">The object to kick to</param>
		/// <param name="pPower">The power to kick with. Should not exceed
		/// <see cref="TeamYaffa.CRaPI.Info.ServerParam.MaxPower"/> and
		/// <see cref="TeamYaffa.CRaPI.Info.ServerParam.MinPower"/>.</param>
		/// <param name="pAction">The action command for the kick</param>
		/// <seealso cref="KickToPoint"/>
		public static void KickToObject(Player pPlayer, FieldObject pObject, int pPower, out Command pAction)
		{
			pAction = KickToPoint(pPlayer, pObject.Position, pPower);
		}

		/// <remarks>This method does not send any command to the server, it is therefore up to the agent
		/// to execute the command on the server.</remarks>
		/// <param name="pPlayer">The player</param>
		/// <param name="pObject">The object to kick to</param>
		/// <param name="pPower">The power to kick with. Should not exceed
		/// <see cref="TeamYaffa.CRaPI.Info.ServerParam.MaxPower"/> and
		/// <see cref="TeamYaffa.CRaPI.Info.ServerParam.MinPower"/>.</param>
		/// <returns>The action command for the kick</returns>
		/// <seealso cref="KickToPoint"/>
		public static Command KickToObject(Player pPlayer, FieldObject pObject, int pPower)
		{
			return KickToPoint(pPlayer, pObject.Position, pPower);
		}

		/// <overloads>
		/// Generates a command to kick the ball towards a point
		/// </overloads>
		/// <remarks>This method does not send any command to the server, it is therefore up to the agent
		/// to execute the command on the server.</remarks>
		/// <param name="pPlayer">The player</param>
		/// <param name="pPoint">The position to kick to</param>
		/// <param name="pPower">The power to kick with. Should not exceed
		/// <see cref="TeamYaffa.CRaPI.Info.ServerParam.MaxPower"/> and
		/// <see cref="TeamYaffa.CRaPI.Info.ServerParam.MinPower"/>.</param>
		/// <param name="pAction">The action command for the kick</param>
		/// <seealso cref="KickToObject"/>
		public static void KickToPoint(Player pPlayer, Point2D pPoint, int pPower, out Command pAction)
		{
			pAction = KickToPoint(pPlayer, pPoint, pPower);
		}

		/// <remarks>This method does not send any command to the server, it is therefore up to the agent
		/// to execute the command on the server.</remarks>
		/// <param name="pPlayer">The player</param>
		/// <param name="pPoint">The position to kick to</param>
		/// <param name="pPower">The power to kick with. Should not exceed
		/// <see cref="TeamYaffa.CRaPI.Info.ServerParam.MaxPower"/> and
		/// <see cref="TeamYaffa.CRaPI.Info.ServerParam.MinPower"/>.</param>
		/// <returns>The action command for the kick</returns>
		/// <seealso cref="KickToObject"/>
		public static Command KickToPoint(Player pPlayer, Point2D pPoint, int pPower)
		{
			int angle = (int)(MathUtility.NormalizeAngle(pPlayer.World.MyPosition.AngleTo(pPoint) - pPlayer.World.Self.AbsoluteBodyDirection));
			return BasicCommands.Kick(pPower, angle);
		}
		#endregion

		#region Catch ball
		/// <overloads>
		/// Generates a command to catch the ball
		/// </overloads>
		/// <remarks>Creates a catch command, with the direction to the ball. It is not considered if
		/// the player is a goalie, if the ball is catchable, or if the game is on. Use
		/// <see cref="TeamYaffa.CRaPI.Player.BallIsCatchable">Player.BallIsCatchable</see> to determine
		/// if the ball is catchable or not.
		/// <para>This method does not send any command to the server, it is therefore up to the agent
		/// to execute the command on the server.</para>
		/// </remarks>
		/// <param name="pPlayer">The player (goalie)</param>
		/// <param name="pAction">The action command for the catch.</param>
		public static void CatchBall(Player pPlayer, out Command pAction)
		{
			pAction = CatchBall(pPlayer);
		}

		/// <remarks>Creates a catch command, with the direction to the ball. It is not considered if
		/// the player is a goalie, if the ball is catchable, or if the game is on. Use
		/// <see cref="TeamYaffa.CRaPI.Player.BallIsCatchable">Player.BallIsCatchable</see> to determine
		/// if the ball is catchable or not.
		/// <para>This method does not send any command to the server, it is therefore up to the agent
		/// to execute the command on the server.</para>
		/// </remarks>
		/// <param name="pPlayer">The player (goalie)</param>
		/// <returns>The action command for the catch</returns>
		public static Command CatchBall(Player pPlayer)
		{
			Point2D intersectionPoint;
			CRaPI.Utility.MathUtility.IntersectionPoint(pPlayer, pPlayer.World.TheBall, out intersectionPoint);
			Double direction = pPlayer.World.Self.Position.AngleTo(pPlayer.World.TheBall.Position) - pPlayer.World.Self.AbsoluteBodyDirection;
			return BasicCommands.Catch((int)(direction));
		}
		#endregion
	}
}