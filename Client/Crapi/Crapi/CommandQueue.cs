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
using TeamYaffa.CRaPI.Commands;

namespace TeamYaffa.CRaPI
{
	/// <summary>
	/// Queue of Command objects.
	/// </summary>
	public class CommandQueue
	{
		#region Members and Constructor
		/// <summary>The internal queue</summary>
		protected Queue mQueue;

		/// <summary>
		/// Constructs a CommandQueue
		/// </summary>
		public CommandQueue()
		{
			mQueue = new Queue();
		}
		#endregion

		#region Queue manipulation

		/// <summary>
		/// Adds a Command to the queue
		/// </summary>
		/// <param name="pCommand">The command to add</param>
		public void Enqueue(Command pCommand)
		{
			if(pCommand != null)
			{
				mQueue.Enqueue(pCommand);
			}
		}

		/// <summary>
		/// Adds an array of Commands to the queue
		/// </summary>
		/// <param name="pCommands">An array of Commands</param>
		public void Enqueue(Command[] pCommands)
		{
			if(pCommands != null)
			{
				foreach(Command com in pCommands)
				{
					Enqueue(com);
				}
			}
		}

		/// <summary>
		/// Adds an ArrayList of Commands to the queue
		/// </summary>
		/// <param name="pCommands">An ArrayList of Commands</param>
		public void Enqueue(ArrayList pCommands)
		{
			if(pCommands != null)
			{
				foreach(Command com in pCommands)
				{
					Enqueue(com);
				}
			}
		}

		/// <summary>
		/// Clears all commands in the internal queue
		/// </summary>
		public void Clear()
		{
			mQueue.Clear();
		}

		/// <summary>
		/// Returns all commands in the internal queue
		/// </summary>
		/// <returns>A Queue of commands</returns>
		public Queue GetCommands()
		{
			return new Queue(mQueue);
		}

		/// <summary>
		/// Returns the first Command object in the queue with
		/// the CommandType pType
		/// </summary>
		/// <param name="pType">The CommandType</param>
		/// <returns>A Command</returns>
		public Command GetCommand(CommandType pType)
		{
			Command command = null;

			foreach(Command com in mQueue)
			{
				if(com.CommandType == pType)
				{
					command = com;
				}
			}
			return command;
		}

		/// <summary>
		/// Dequeues and returns the first command in the queue
		/// </summary>
		/// <returns>The first command in the queue</returns>
		public Command Dequeue()
		{
			return (Command)mQueue.Dequeue();
		}

		/// <summary>
		/// Returns the first command in the queue
		/// </summary>
		/// <returns>The first command in the queue</returns>
		public Command Peek()
		{
			return (Command)mQueue.Peek();
		}

		/// <summary>
		/// Returns the concatenated command string for all Command objects
		/// in the queue
		/// </summary>
		/// <returns>A command string</returns>
		public string GetCommandString()
		{
			string command = "";

			foreach(Command com in mQueue)
			{
				command += com.CommandString;
			}
			return command;
		}

		#endregion
	}
}
