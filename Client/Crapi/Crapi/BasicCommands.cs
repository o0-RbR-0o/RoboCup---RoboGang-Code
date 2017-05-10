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
using TeamYaffa.CRaPI.Commands;
using TeamYaffa.CRaPI.Utility;

namespace TeamYaffa.CRaPI
{
	/// <summary>
	/// This class provides the command strings for the basic commands available in the server.
	/// </summary>
	public class BasicCommands
	{
		#region Commands

		/// <summary>
		/// Builds the server command string for the catch command
		/// </summary>
		/// <param name="pDirection">The direction to catch in</param>
		/// <returns>A Command</returns>
		public static Command Catch(int pDirection)
		{
			return new Catch(pDirection);
		}	

		/// <summary>
		/// Builds the server command string for the change_view command
		/// </summary>
		/// <param name="pWidth">The view width</param>
		/// <param name="pQuality">The view quality</param>
		/// <returns>A Command</returns>
		public static Command ChangeView(ViewWidth pWidth, ViewQuality pQuality)
		{
			return new ChangeView(pWidth, pQuality);
		}
	
		/// <summary>
		/// Builds the server command string for the dash command
		/// </summary>
		/// <param name="pPower">The power to dash with</param>
		/// <returns>A Command</returns>
		public static Command Dash(int pPower)
		{
			return new Dash(pPower);
		}

		/// <summary>
		/// Builds the server command string for the kick command
		/// </summary>
		/// <param name="pPower">The power to kick with</param>
		/// <param name="pDirection">The direction to kick in</param>
		/// <returns>A Command</returns>
		public static Command Kick(int pPower, int pDirection)
		{
			return new Kick(pPower, pDirection);
		}

		/// <summary>
		/// Builds the server command string for the move command
		/// </summary>
		/// <param name="pX">The X-Coordinate to move to</param>
		/// <param name="pY">The Y-Coordinate to move to</param>
		/// <returns>A Command</returns>
		public static Command Move(int pX, int pY)
		{
			return new Move(pX, pY);
		}

		/// <summary>
		/// Builds the server command string for the move command
		/// </summary>
		/// <param name="pPoint">The point to move to</param>
		/// <returns>A Command</returns>
		public static Command Move(Point2D pPoint)
		{
			return new Move((int)pPoint.X, (int)pPoint.Y);
		}

		/// <summary>
		/// Builds the server command string for the say command
		/// </summary>
		/// <param name="pMessage">The message to say</param>
		/// <returns>A Command</returns>
		public static Command Say(string pMessage)
		{
			return new Say(pMessage);
		}

		/// <summary>
		/// Builds the server command string for the sense_body command
		/// </summary>
		/// <returns>A Command</returns>
		public static Command SenseBody()
		{
			return new Command(CommandType.Sense_Body);
		}

		/// <summary>
		/// Builds the server command string for the score command
		/// </summary>
		/// <returns>A Command</returns>
		public static Command Score()
		{
			return new Command(CommandType.Score);
		}

		/// <summary>
		/// Builds the server command string for the turn command
		/// </summary>
		/// <param name="pDegrees">The degrees to turn</param>
		/// <returns>A Command</returns>
		public static Command Turn(int pDegrees)
		{
			return new Turn(pDegrees);
		}

		/// <summary>
		/// Builds the server command string for the turn_neck command
		/// </summary>
		/// <param name="pDegrees">The degrees to turn the neck</param>
		/// <returns>A Command</returns>
		public static Command TurnNeck(int pDegrees)
		{
			return new TurnNeck(pDegrees);
		}
		
		/// <summary>
		/// Builds the server command string for the bye command
		/// </summary>
		/// <returns></returns>
		public static Command Bye()
		{
			return new Command(CommandType.Bye);
		}
		#endregion
	}
}
