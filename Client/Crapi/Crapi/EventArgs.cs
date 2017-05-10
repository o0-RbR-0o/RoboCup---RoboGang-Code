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
using TeamYaffa.CRaPI.Info;

namespace TeamYaffa.CRaPI
{
	/// <summary>
	/// This class is used by events fired and time is concered.
	/// </summary>
	/// <remarks>Use this when fireing events where e.g. time should be sent to the
	/// receivers. <see cref="Time"/> can be time left on cycle, time of current cycle or
	/// current cycle number, depending on the event.</remarks>
	public class TimeEventArgs : EventArgs
	{
		/// <summary>The time set by the event.</summary>
		private int mTime;

		/// <summary>
		/// Creates a new TimeEventArgs with the specified time.
		/// </summary>
		/// <param name="pTime">The time set by the event.</param>
		public TimeEventArgs(int pTime)
		{
			mTime = pTime;
		}

		/// <summary>The time set by the event.</summary>
		public int Time{ get { return mTime; }}
	}

	/// <summary>
	/// This class is used when an event is fired because the player has received a hear message.
	/// </summary>
	public class HearEventArgs : EventArgs
	{
		/// <summary>The message</summary>
		private HearInfo mMessage;

		/// <summary>Creates a new HearEventArgs with the heard message.</summary>
		/// <param name="pMessage">The message</param>
		public HearEventArgs(HearInfo pMessage)
		{
			mMessage = pMessage;
		}

		/// <summary>The message</summary>
		public HearInfo Message{ get { return mMessage; }}
	}
}