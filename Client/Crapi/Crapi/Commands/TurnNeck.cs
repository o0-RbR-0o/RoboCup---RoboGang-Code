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
	/// A wrapper for a turn_neck command.
	/// </summary>
	public class TurnNeck : Command
	{
		#region Members and constructor

		/// <summary>The moment of the turn, i.e. how many degrees to turn.</summary>
		private int mMoment;

		/// <summary>
		/// Constructs a TurnNeck command object
		/// </summary>
		/// <remarks>The command will of the form:
		/// <code>(turn_neck pMoment)</code></remarks>
		/// <param name="pMoment">The moment of the turn, i.e. how many degrees to turn.</param>
		public TurnNeck(int pMoment) : base(CommandType.Turn_Neck)
		{
			mMoment = pMoment;

			mCommand = mCommand.Substring(0, mCommand.Length - 1); // remove ')' from end of command string
			mCommand += " " + mMoment + ")";
		}
		#endregion

		#region Properties

		/// <summary>
		/// The moment of the turn, i.e. how many degrees to turn.
		/// </summary>
		public int Moment
		{
			get{ return mMoment; }
			set{ mMoment = value; }
		}
		#endregion

	}
}
