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

namespace TeamYaffa.CRaPI.Commands
{
	/// <summary>
	/// A wrapper for a kick command.
	/// </summary>
	public class Kick : Command
	{
		#region Members and constructor

		/// <summary>Power of the kick.</summary>
		private int mPower;
		/// <summary>The angle to kick, relative the players own body-direction.</summary>
		private int mDirection;

		/// <summary>
		/// Constructs a Kick command object
		/// </summary>
		/// <remarks>The command will of the form:
		/// <code>(kick pPower pDirection)</code></remarks>
		/// <param name="pPower">Power of kick</param>
		/// <param name="pDirection">Angle where to kick, relative the players own body-direction.</param>
		public Kick(int pPower, int pDirection) : base(CommandType.Kick)
		{
			mPower= pPower;
			mDirection = pDirection;
			mCommand = mCommand.Substring(0, mCommand.Length - 1); // remove ')' from end of command string
			mCommand += " " + mPower + " " + mDirection + ")";
		}

		#endregion

		#region Properties

		/// <summary>
		/// Power of the kick
		/// </summary>
		public int Power
		{
			get{ return mPower; }
			set{ mPower = value; }
		}

		/// <summary>
		/// The angle to kick, relative the players own body-direction.
		/// </summary>
		public int Direction
		{
			get{ return mDirection; }
			set{ mDirection = value; }
		}
		#endregion
	}
}
