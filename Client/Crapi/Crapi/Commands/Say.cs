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
	/// A wrapper for a say command.
	/// </summary>
	public class Say : Command
	{
		#region Members and constructor

		/// <summary>Message to say</summary>
		private string mMessage;

		/// <summary>
		/// Constructs a Say command object
		/// </summary>
		/// <remarks>The command will of the form:
		/// <code>(say pMessage)</code></remarks>
		/// <param name="pMessage">Message to say</param>
		public Say(string pMessage) : base(CommandType.Say)
		{
			mMessage = pMessage;
		
			mCommand = mCommand.Substring(0, mCommand.Length - 1); // remove ')' from end of command string
			mCommand += " " + mMessage + ")";
		}
		#endregion

		#region Properties

		/// <summary>
		/// Message to say
		/// </summary>
		public string Message
		{
			get{ return mMessage; }
			set{ mMessage = value; }
		}
		#endregion

	}
}
