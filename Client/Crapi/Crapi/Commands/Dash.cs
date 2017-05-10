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
	/// A wrapper for a dash command.
	/// </summary>
	public class Dash : Command
	{
		#region Members and constructor

		/// <summary>The power to dash with</summary>
		private int mPower;

		/// <summary>
		/// Constructs a Dash command object
		/// </summary>
		/// <remarks>The command will of the form:
		/// <code>(dash pPower)</code></remarks>
		/// <param name="pPower">The power to dash with</param>
		public Dash(int pPower) : base(CommandType.Dash)
		{
			mPower= pPower;
			mCommand = mCommand.Substring(0, mCommand.Length - 1); // remove ')' from end of command string
			mCommand += " " + mPower + ")";
		}

		#endregion

		#region Properties

		/// <summary>
		/// The power to dash with
		/// </summary>
		public int Power
		{
			get{ return mPower; }
			set{ mPower = value; }
		}

		#endregion
	}
}
