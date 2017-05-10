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
	/// This class acts as a wrapper of initial player characteristics
	/// </summary>
	/// <remarks>
	/// This class is used for the player characteristics sent by the server when the player connects.
	/// </remarks>
	public class PlayerParam : InfoBase
	{
		#region Members, constructor and Create

		/// <summary>
		/// Private constructor
		/// </summary>
		private PlayerParam()
		{
			mValues = new Hashtable();
		}

		/// <summary>Parses the <c>player_param</c> message from the server</summary>
		/// <remarks><see cref="PlayerParam"/> is able to parse <c>player_param</c> messages from robocup server 
		/// 9.0.4. Changes to the protocol could require changes to this class.</remarks>
		/// <param name="pServerData">The message from the server. Must begin with <c>(player_param</c>.</param>
		/// <returns>A ServerParam object if it was a valid <c>player_param</c> message. Null otherwise.</returns>
		public static PlayerParam Create(string pServerData)
		{
			if(!pServerData.StartsWith("(player_param"))
				return null;

			PlayerParam result = new PlayerParam();
			int startIndex = pServerData.IndexOf('(', 13) + 1;
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
	}
}
