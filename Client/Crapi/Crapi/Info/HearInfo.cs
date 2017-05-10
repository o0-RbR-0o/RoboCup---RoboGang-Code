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

namespace TeamYaffa.CRaPI.Info
{
	/// <summary>This class acts as a wrapper of hear information from the server.</summary>
	/// <remarks>Sent from the server each time a client or the coach has issued a "say"-command, or 
	/// when the referee is calling.</remarks>
	public class HearInfo : InfoBase
	{
		#region Members, Constructor and Create
		/// <summary>Holds the value of when the message was received.</summary>
		private int mCycle;
		/// <summary>Holds the value of who sent the message.</summary>
		private string mSender;
		/// <summary>Holds the message.</summary>
		private string mMessage;
		/// <summary>Holds the playmode if the message was from referee, otherwise it holds the
		/// value <see cref="TeamYaffa.CRaPI.PlayMode.none"/></summary>
		private PlayMode mCurrentPlayMode = PlayMode.none;
		/// <summary>Holds the current referee message if the message was from referee, otherwise it holds the
		/// value <see cref="TeamYaffa.CRaPI.RefereeMessage.none"/></summary>
		private RefereeMessage mCurrentRefereeMessage = RefereeMessage.none;

		/// <summary>Creates a new HearInfor message based on cycle, sender and message.</summary>
		/// <remarks>If the sender is <c>referee</c>, the playmode will be parsed and can then
		/// be retrieved by <see cref="CurrentPlayMode"/>.</remarks>
		/// <param name="pCycle">The cycle when the message was received.</param>
		/// <param name="pSender">Who sent the message.</param>
		/// <param name="pMessage">The message.</param>
		private HearInfo(int pCycle, string pSender, string pMessage)
		{
			mCycle = pCycle;
			mSender = pSender;
			mMessage = pMessage;
			if(mSender == "referee")
			{
				//play mode goal_"Side" has to be treated differently than
				//the other play modes since the server sends goal_"Side"_"Score", where _"Score" is
				//not part of the play mode
				if(mMessage.StartsWith("goal_l"))
				{
					mCurrentPlayMode = PlayMode.goal_l;
					return;
				}
				else if(mMessage.StartsWith("goal_r"))
				{
					mCurrentPlayMode = PlayMode.goal_r;
					return;
				}

				try
				{
					mCurrentPlayMode = (PlayMode)Enum.Parse(typeof(PlayMode), mMessage);
				}
				catch
				{
					// do not change playmode since it is most likely a regular referee message
					// if so, the next message from the referee will contain the playmode
					// should probably not do this -> mCurrentPlayMode = PlayMode.none;

					try
					{
						mCurrentRefereeMessage = (RefereeMessage)Enum.Parse(typeof(RefereeMessage), mMessage);
					}
					catch
					{
					}
				}
			}
		}

		/// <summary>Parses the <c>hear</c> message from the server</summary>
		/// <remarks><see cref="HearInfo"/> is able to parse <c>server_param</c> messages from robocup server 
		/// 9.0.4. Changes to the protocol could require changes to this class.</remarks>
		/// <param name="pServerData">The message from the server. Must begin with <c>(hear</c>.</param>
		/// <returns>A ServerParam object if it was a valid <c>hear</c> message. Null otherwise.</returns>
		public static HearInfo Create(string pServerData)
		{
			if(!pServerData.StartsWith("(hear"))
				return null;

			int space1 = pServerData.IndexOf(' ', 6);
			int cycle = Int32.Parse(pServerData.Substring(6, space1-6));
			int space2 = pServerData.IndexOf(' ', ++space1);
			string sender = pServerData.Substring(space1, space2-space1);
			string message = pServerData.Substring(++space2, pServerData.IndexOf(')', space2) - space2);
			return new HearInfo(cycle, sender, message);
		}
		#endregion

		#region Properties
		/// <summary>What cycle the hear message was received.</summary>
		public int Cycle
		{
			get { return mCycle; }
		}

		/// <summary>The message, i.e. what was heard.</summary>
		public string Message
		{
			get { return mMessage; }
		}

		/// <summary>The sender of the message.</summary>
		public string Sender
		{
			get { return mSender; }
		}

		/// <summary>The playmode if the message came from the referee.</summary>
		public PlayMode CurrentPlayMode
		{
			get { return mCurrentPlayMode; }
		}

		/// <summary>The referee message if the message came from the referee.</summary>
		public RefereeMessage CurrentRefereeMessage
		{
			get { return mCurrentRefereeMessage; }
		}

		/// <summary>Whether it is a message from the referee or not.</summary>
		/// <value>True if the message is from the referee, false otherwise.</value>
		public bool IsRefereeMessage
		{
			get { return mSender == "referee"; }
		}
		#endregion

		#region Misc. operations
		/// <summary>
		/// Creates a string representation of the HearInfo
		/// </summary>
		/// <returns>The string representation</returns>
		public override string ToString()
		{
			return "Cycle: " + mCycle + "\nSender: " + mSender + "\nMessage: " + mMessage;
		}
		#endregion
	}
}
