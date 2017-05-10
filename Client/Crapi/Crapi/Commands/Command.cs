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
	/// This class acts as a wrapper around a server command
	/// </summary>
	public class Command
	{
		#region Members and Constructor
		/// <summary>The command string</summary>
		protected string mCommand;
		/// <summary>The command type</summary>
		protected CommandType mCommandType;

		/// <summary>
		/// Constructs a Command object
		/// </summary>
		/// <remarks>The command will look like:
		/// <code>(pCommandType.ToString().ToLower())</code>
		/// </remarks>
		/// <param name="pCommandType">What type of command it will be.</param>
		public Command(CommandType pCommandType)
		{
			mCommandType = pCommandType;
			string type = mCommandType.ToString().ToLower();
			mCommand = "(" + type + ")";
		}

		/// <summary>
		/// Default constructor
		/// </summary>
		public Command()
		{
			mCommandType = CommandType.Turn;
			string type = mCommandType.ToString().ToLower();
			mCommand = "(" + type + ")";
		}
		#endregion

		#region Operators
		/// <summary>
		/// Compares two Command objects for equality
		/// </summary>
		/// <remarks>Two Command objects are equal if:
		/// <list type="bullet">
		/// <item><description>The references are equal</description></item>
		/// <item><description>or the commandstrings and commandtypes are equal</description></item>
		/// </list>
		/// </remarks>
		/// <param name="pCommand1">First Command object</param>
		/// <param name="pCommand2">Second Command object</param>
		/// <returns>true if equal, otherwise false</returns>
		public static bool operator==(Command pCommand1, Command pCommand2)
		{
			if((Object)pCommand1 == (Object)pCommand2)
			{
				return true;
			}
			else if((Object)pCommand1 == null || (Object)pCommand2 == null)
			{
				return false;
			}

			if(pCommand1.CommandString == pCommand2.CommandString)
			{
				return true;
			}
			return false;
		}

		/// <summary>
		/// Compares two Command objects for inequality <seealso cref="operator=="/>
		/// </summary>
		/// <param name="pCommand1">First Command object</param>
		/// <param name="pCommand2">Second Command object</param>
		/// <returns>true if equal, otherwise false</returns>
		public static bool operator!=(Command pCommand1, Command pCommand2)
		{
			return !(pCommand1 == pCommand2);
		}

		/// <summary>
		/// Compares if the Command-object are equal another object.
		/// </summary>
		/// <remarks>The <paramref name="o">object</paramref> must be of type Command and be equal
		/// according to <see cref="operator==">operator ==</see></remarks>
		/// <param name="o">The object to compare with.</param>
		/// <returns>true if equal, false otherwise.</returns>
		public override bool Equals(Object o)
		{
			if(o is Command)
			{
				return this == (Command)o;
			}
			return false;
		}

		/// <summary>
		/// Returns the hashcode of the Command object.
		/// </summary>
		/// <remarks>The hashcode of the Command object is the same as the hashcode of the <see cref="CommandString"/>.</remarks>
		/// <returns>An integer that represents the hashcode for this Command object</returns>
		public override int GetHashCode()
		{
			return mCommand.GetHashCode();
		}
		#endregion

		#region Properties

		/// <summary>The command string</summary>
		public string CommandString
		{
			get{ return mCommand; }
			set{ mCommand = value; }
		}

		/// <summary>The command type</summary>
		public CommandType CommandType
		{
			get{ return mCommandType; }
			set{ mCommandType = value; }
		}
		#endregion
	}
}
