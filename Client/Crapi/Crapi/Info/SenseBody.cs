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
	/// This class acts as a wrapper of player characteristics received from the server.
	/// </summary>
	/// <remarks>Sent from the server each sense_body_step, currently each 100 ms. What value
	/// is really used is reasived in the <see cref="ServerParam"/> message.</remarks>
	/// <seealso cref="ServerParam"/>
	public class SenseBody : InfoBase
	{
		#region Members, constructor and Create
		/// <summary>The cycle when the message was sent.</summary>
		private int mCycle;

		/// <summary>Creates a new SenseBody with a fresh Hashtable for the values in the sense_body message.</summary>
		private SenseBody()
		{
			mValues = new Hashtable();
		}

		/// <summary>Parses the <c>sense_body</c> message from the server</summary>
		/// <remarks><see cref="SenseBody"/> is able to parse <c>server_param</c> messages from robocup server 
		/// 9.0.4. Changes to the protocol could require changes to this class.
		/// <para>Currently are <c>arm, target, focus, tackle</c> not supported.</para></remarks>
		/// <param name="pServerData">The message from the server. Must begin with <c>(sense_body</c>.</param>
		/// <returns>A ServerParam object if it was a valid <c>sense_body</c> message. Null otherwise.</returns>
		public static SenseBody Create(string pServerData)
		{
			if(!pServerData.StartsWith("(sense_body"))
				return null;

			SenseBody result = new SenseBody();
			int startIndex = pServerData.IndexOf(' ', 11) + 1;
			result.mCycle = Int32.Parse(pServerData.Substring(startIndex, pServerData.IndexOf(' ', startIndex+1)-startIndex));
			startIndex = pServerData.IndexOf('(', startIndex+1) + 1;
			while(startIndex != 0)
			{
				int endIndex = pServerData.IndexOf(')', startIndex);
				int splitIndex = pServerData.IndexOf(' ', startIndex);
				string key = pServerData.Substring(startIndex, splitIndex-startIndex);
				if(key == "arm") // TODO: SenseBody should support arm, focus and tackle.
					break;
				result.mValues[key] = pServerData.Substring(++splitIndex, endIndex-splitIndex);
				startIndex = pServerData.IndexOf('(', endIndex+1) + 1;
			}

			return result;
		}
		#endregion

		#region Properties
		/// <summary>The cycle the message was sent from the server.</summary>
		public int Cycle
		{
			get { return mCycle; }
		}

		/// <summary>The view quality the player is using.</summary>
		public ViewQuality ViewQuality
		{
			get
			{
				string viewString = (string)mValues["view_mode"];
				string qualityString = viewString.Substring(0, viewString.IndexOf(' '));
				return (ViewQuality)Enum.Parse(typeof(ViewQuality), qualityString);
			}
		}

		/// <summary>The width of the view that the player is using.</summary>
		public ViewWidth ViewWidth
		{
			get
			{
				string viewString = (string)mValues["view_mode"];
				string widthString = viewString.Substring(viewString.IndexOf(' ')+1);
				return (ViewWidth)Enum.Parse(typeof(ViewWidth), widthString);
			}
		}

		/// <summary>The Players stamina.</summary>
		public double Stamina
		{
			get { return getDouble("stamina", true); }
		}

		/// <summary>The stamina effort</summary>
		public double StaminaEffort
		{
			get { return getDouble("stamina", false); }
		}

		/// <summary>The amount of speed</summary>
		public double SpeedAmount
		{
			get { return getDouble("speed", true); }
		}

		/// <summary>The direction of speed</summary>
		public double SpeedDirection
		{
			get { return getDouble("speed", false); }
		}

		/// <summary>The angle of the head</summary>
		public int HeadAngle
		{
			get { return getInt("head_angle"); }
		}

		/// <summary>Number of times kick has been sent and acknowledge by the server</summary>
		public int KickCount
		{
			get { return getInt("kick"); }
		}

		/// <summary>Number of times dash has been sent and acknowledge by the server</summary>
		public int DashCount
		{
			get { return getInt("dash"); }
		}

		/// <summary>Number of times turn has been sent and acknowledge by the server</summary>
		public int TurnCount
		{
			get { return getInt("turn"); }
		}

		/// <summary>Number of times say has been sent and acknowledge by the server</summary>
		public int SayCount
		{
			get { return getInt("say"); }
		}

		/// <summary>Number of times turn_neck has been sent and acknowledge by the server</summary>
		public int TurnNeckCount
		{
			get { return getInt("turn_neck"); }
		}

		/// <summary>Number of times catch has been sent and acknowledge by the server</summary>
		public int CatchCount
		{
			get { return getInt("catch"); }
		}

		/// <summary>Number of times move has been sent and acknowledge by the server</summary>
		public int MoveCount
		{
			get { return getInt("move"); }
		}

		/// <summary>Number of times change_view has been sent and acknowledge by the server</summary>
		public int ChangeViewCount
		{
			get { return getInt("change_view"); }
		}
		#endregion

		#region Misc. operations
		/// <summary>
		/// The string representation of the class.
		/// </summary>
		/// <returns><see cref="InfoBase.ToString">base.ToString()</see> and the cycle the 
		/// sensebody was sent from the server.</returns>
		public override string ToString()
		{
			string result = base.ToString();
			result += "cycle:\t" + mCycle + "\n";
			return result;
		}
		#endregion
	}
}
