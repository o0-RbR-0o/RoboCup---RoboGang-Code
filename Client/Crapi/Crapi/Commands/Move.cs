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
using TeamYaffa.CRaPI.Utility;

namespace TeamYaffa.CRaPI.Commands
{
	/// <summary>
	/// A wrapper for a move command.
	/// </summary>
	public class Move : Command
	{
		#region Members and constructor

		/// <summary>X-coordinate</summary>
		int mX;
		/// <summary>Y-coordinate</summary>
		int mY;

		/// <summary>
		/// Constructs a Move command object
		/// </summary>
		/// <remarks>The command will be of the form:
		/// <code>(move pX pY)</code></remarks>
		/// <param name="pX">X-coordinate</param>
		/// <param name="pY">Y-coordinate</param>
		public Move(int pX, int pY) : base(CommandType.Move)
		{
			mX = pX;
			mY = pY;

			mCommand = mCommand.Substring(0, mCommand.Length - 1); // remove ')' from end of command string
			mCommand += " " + mX + " " + mY + ")";
		}

		/// <summary>
		/// Constructs a Move command object
		/// </summary>
		/// <remarks>The command will be of the form:
		/// <code>(move pPosition.X pPosition.Y)</code></remarks>
		/// <param name="pPosition">The position to move to</param>
		public Move(Point2D pPosition) : base(CommandType.Move)
		{
			mX = (int)pPosition.X;
			mY = (int)pPosition.Y;

			mCommand = mCommand.Substring(0, mCommand.Length - 1); // remove ')' from end of command string
			mCommand += " " + mX + " " + mY + ")";
		}
		#endregion

		#region Properties

		/// <summary>
		/// X-coordinate
		/// </summary>
		public int X
		{
			get{ return mX; }
			set{ mX = value; }
		}

		/// <summary>
		/// Y-coordinate
		/// </summary>
		public int Y
		{
			get{ return mY; }
			set{ mY = value; }
		}
		#endregion
	}
}