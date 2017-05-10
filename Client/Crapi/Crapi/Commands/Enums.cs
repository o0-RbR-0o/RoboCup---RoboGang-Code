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
namespace TeamYaffa.CRaPI.Commands
{
	#region Commands
	/// <summary>
	/// Describes the possible commands to the server
	/// <seealso cref="TeamYaffa.CRaPI.Commands"/>
	/// </summary>
	/// <remarks>This is used when creating <c>Commands</c>.</remarks>
	public enum CommandType
	{
		/// <summary>A <c>bye</c> command. Used when disconnecting a player.</summary>
		Bye,
		/// <summary>A <c>catch</c> command. Used when a goalie tries to catch the ball.</summary>
		Catch,
		/// <summary>A <c>change_view</c> command. Used with <see cref="ViewWidth"/> and <see cref="ViewQuality"/>
		/// when the player changes the view.</summary>
		Change_View,
		/// <summary>A <c>dash</c> command. Used to dash.</summary>
		Dash,
		/// <summary>A <c>kick</c> command. Used when to kick the ball.</summary>
		Kick,
		/// <summary>A <c>move</c> command. Used when to move the player when the <see cref="PlayMode"/> is
		/// <see cref="PlayMode.before_kick_off"/>.</summary>
		Move,
		/// <summary>A <c>say</c> command.</summary>
		Say,
		/// <summary>A <c>sense_body</c> command.</summary>
		Sense_Body,
		/// <summary>A <c>score</c> command. Used to retrieve the current score of the game.</summary>
		Score,
		/// <summary>A <c>turn</c> command. Used to turn.</summary>
		Turn,
		/// <summary>A <c>turn_neck</c> command. Used to turn the neck.</summary>
		Turn_Neck
	}
	#endregion
}