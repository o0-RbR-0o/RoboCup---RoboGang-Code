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
using System.Collections;

namespace TeamYaffa.CRaPI.Info
{
	/// <summary>
	/// Contains the information sent from the server in the <c>server_param</c> message.
	/// </summary>
	public class ServerParam : InfoBase
	{
		#region Members, constructor and Create

		/// <summary>
		/// Private constructor
		/// </summary>
		private ServerParam()
		{
			mValues = new Hashtable();
		}

		/// <summary>Parses the <c>server_param</c> message from the server</summary>
		/// <remarks><see cref="ServerParam"/> is able to parse <c>server_param</c> messages from robocup server 
		/// 9.0.4. Changes to the protocol could require changes to this class.</remarks>
		/// <param name="pServerData">The message from the server. Must begin with <c>(server_param</c>.</param>
		/// <returns>A ServerParam object if it was a valid <c>server_param</c> message. Null otherwise.</returns>
		public static ServerParam Create(string pServerData)
		{
			if(!pServerData.StartsWith("(server_param"))
				return null;

			ServerParam result = new ServerParam();
			int startIndex = pServerData.IndexOf('(', 14) + 1;
			while(startIndex != 0)
			{
				int endIndex = pServerData.IndexOf(')', startIndex);
				int splitIndex = pServerData.IndexOf(' ', startIndex);
				result.mValues[pServerData.Substring(startIndex, splitIndex-startIndex)] = pServerData.Substring(++splitIndex, endIndex-splitIndex);
				startIndex = pServerData.IndexOf('(', endIndex+1) + 1;
			}

			return result;
		}
		#endregion

		#region Frequently used parameters, as properties
		/// <summary>Gets the max power of i.e. kick and dash, according to the
		/// server_param message <c>maxpower</c>.</summary>
		public int MaxPower
		{
			get { return base.getInt("maxpower"); }
		}

		/// <summary>Gets the min power of i.e. kick and dash, according to the
		/// server_param message <c>minpower</c>.</summary>
		public int MinPower
		{
			get { return base.getInt("minpower"); }
		}

		/// <summary>Gets the step of the simulation in milliseconds, according
		/// to the server_param message <c>simulator_step</c>.</summary>
		public int SimulatorStep
		{
			get {  return getInt("simulator_step"); }
		}

		/// <summary>Gets the minimum value the neck-angle can be, according
		/// to the server_param message <c>minneckang</c>.</summary>
		public int MinNeckAngle
		{
			get { return base.getInt("minneckang"); }
		}

		/// <summary>Gets the maximum value the neck-angle can be, according
		/// to the server_param message <c>maxneckang</c>.</summary>
		public int MaxNeckAngle
		{
			get { return base.getInt("maxneckang"); }
		}

		/// <summary>Gets how much the neck can be turned to the left at the most
		///  in one turn, according to the server_param message <c>minneckmoment</c>.</summary>
		public int MinNeckMoment
		{
			get { return base.getInt("minneckmoment"); }
		}

		/// <summary>Gets how much the neck can be turned to the right at the most
		///  in one turn, according to the server_param message <c>maxneckmoment</c>.</summary>
		public int MaxNeckMoment
		{
			get { return base.getInt("maxneckmoment"); }
		}

		/// <summary>Gets how much the body can be turned to the left at the most
		///  in one turn, according to the server_param message <c>minmoment</c>.</summary>
		public int MinMoment
		{
			get { return base.getInt("minmoment"); }
		}

		/// <summary>Gets how much the body can be turned to the right at the most
		///  in one turn, according to the server_param message <c>maxmoment</c>.</summary>
		public int MaxMoment
		{
			get { return base.getInt("maxmoment"); }
		}

		/// <summary>Gets the length of the catchable area where the ball must reside if
		/// the goalie should be able to catch the ball, according to the server_param message
		/// <c>catchable_area_l</c>.</summary>
		public double CatchableAreaLength
		{
			get { return base.getDouble("catchable_area_l");}
		}

		/// <summary>Gets the width of the catchable area where the ball must reside if
		/// the goalie should be able to catch the ball, according to the server_param message
		/// <c>catchable_area_w</c>.</summary>
		public double CatchableAreaWidth
		{
			get { return base.getDouble("catchable_area_w"); }
		}

		/// <summary>Gets power rate of a kick, according to the server_param message
		/// <c>kick_power_rate</c>.</summary>
		public double KickPowerRate
		{
			get{ return base.getDouble("kick_power_rate"); }
		}

		/// <summary>Gets kickable margin, according to the server_param message
		/// <c>kickable_margin</c>.</summary>
		public double KickableMargin
		{
			get{ return base.getDouble("kickable_margin"); }
		}

		/// <summary>Gets ball size, according to the server_param message
		/// <c>ball_size</c>.</summary>
		public double BallSize
		{
			get{ return base.getDouble("ball_size"); }
		}

		/// <summary>Gets loss of speed of the ball, according to the server_param message
		/// <c>ball_decay</c>.</summary>
		public double BallDecay
		{
			get{ return base.getDouble("ball_decay"); }
		}

		/// <summary>Gets the noice of the ball, according to the server_param message
		/// <c>ball_rand</c>.</summary>
		public double BallRand
		{
			get{ return base.getDouble("ball_rand"); }
		}

		/// <summary>Gets the maximum speed of the ball, according to the server_param message
		/// <c>ball_speed_max</c>.</summary>
		public double BallSpeedMax
		{
			get{ return base.getDouble("ball_speed_max"); }
		}

		/// <summary>Gets maximum acceleration of the ball, according to the server_param message
		/// <c>ball_accel_mas</c>.</summary>
		public double BallAccelMax
		{
			get{ return base.getDouble("ball_accel_max"); }
		}

		/// <summary>Gets force of the wind, according to the server_param message <c>wind_force</c>.</summary>
		public double WindForce
		{
			get{ return base.getDouble("wind_force"); }
		}

		/// <summary>Gets direction of the wind, according to the server_param message <c>wind_dir</c>.</summary>
		public double WindDir
		{
			get{ return base.getDouble("wind_dir"); }
		}

		/// <summary>Gets noice of the wind, according to the server_param message <c>wind_rand</c>.</summary>
		public double WindRand
		{
			get{ return base.getDouble("wind_rand"); }
		}

		/// <summary>Gets the maximum stamina for a player, according to the server_param message <c>stamina_max</c>.</summary>
		public int MaxStamina
		{
			get{ return base.getInt("stamina_max"); }
		}

		/// <summary>Gets the maximum speed for a player, according to the server_param message <c>player_speed_max</c>.</summary>
		public double PlayerSpeedMax
		{
			get{ return base.getDouble("player_speed_max"); }
		}

		/// <summary>Gets the size of the player, according to the server_param message <c>player_size</c>.</summary>
		public double PlayerSize
		{
			get{ return base.getDouble("player_size"); }
		}

		/// <summary>Gets the decay of the player, according to the server_param message <c>player_decay</c>.</summary>
		public double PlayerDecay
		{
			get{ return base.getDouble("player_decay"); }
		}

		/// <summary>Gets the inertia moment, according to the server_param message <c>inertia_moment</c>.</summary>
		public double InertiaMoment
		{
			get{ return base.getDouble("inertia_moment");}
		}

		/// <summary>Gets the dash power rate, according to the server_param message <c>dash_power_rate</c>.</summary>
		public double DashPowerRate
		{
			get{ return base.getDouble("dash_power_rate");}
		}
		#endregion
	}
}
