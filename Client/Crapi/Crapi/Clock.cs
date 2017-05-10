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
using System.Timers;

namespace TeamYaffa.CRaPI
{
	/// <summary>This class is used as an internal clock of CRaPI.</summary>
	/// <remarks>It is used to keep track of how long time it is left before next cycle,
	/// i.e. when we must send a command to the server so that it will be performed in this cycle.</remarks>
	public class Clock
	{
		#region Members and constructors
		/// <summary>The Internal system clock</summary>
		private Timer mTimer;
		/// <summary>The cycle duration</summary>
		private int mCycleDuration;
		/// <summary>The time when the current cycle will end</summary>
		private long mEndTimeForCycle;
		/// <summary>The time when the clock was stopped</summary>
		private long mStopTime;
		/// <summary>If the game state is running or stopped</summary>
		private bool mGameOn;
		/// <summary>How many ticks there should be left of the cycle to raise the event <see cref="TickOnCycleSoonEnding"/></summary>
		private int mTicksLeftOnCycle;
		/// <summary>Which tick to raise the event <see cref="TickOnCycleSoonEnding"/></summary>
		private int mTickOnRaiseEvent;
		/// <summary>The current tick of the cycle</summary>
		private int mCurrentTick;
		
		/// <summary>Constructs a new clock.</summary>
		/// <remarks>
		/// Default value for time interval is 10 milliseconds, cycle duration is 100 milliseconds
		/// and tick to send event is 0 and that means no <see cref="TickOnCycleSoonEnding"/> will be raised.
		/// </remarks>
		public Clock()
		{
			mTimer = new System.Timers.Timer();
			mTimer.AutoReset = true;
			mTimer.Interval = 10;
			mTimer.Elapsed += new ElapsedEventHandler(this.OnTick);
			mCycleDuration = 100;
			mEndTimeForCycle = 0;
			mCurrentTick = 1;
			mTicksLeftOnCycle = 0;
			mTickOnRaiseEvent = 0;
			mGameOn = false;
			mStopTime = 0;
			mTimer.Start();
		}

		#endregion

		#region Start and Stop

		/// <summary>This method starts the clock.</summary>
		internal void Start()
		{	
			mGameOn = true;
			mCurrentTick = 1;
			mEndTimeForCycle = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond + mCycleDuration;
		}
		/// <summary>This method stops the clock</summary>
		internal void Stop()
		{
			mGameOn = false;
			mStopTime = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
		}

		#endregion
		
		#region Properties

		/// <summary>
		/// This method sets the time inteval to get a notify message about how much time
		/// has passed after the game clock was stopped or how much time that is left on the current cycle.
		/// </summary>
		/// <value>The time interval</value>
		/// <remarks>The time is in milliseconds and you only get a notify message if you have subscribed to any of the events
		/// Conditions: Time interval has to be greater than zero, smaller or equal to cycle duration and possible ticks has to be greater than zero (cycle duration / time interval - ticks left on cycle).</remarks>
		/// <exception cref="ArgumentOutOfRangeException"> If the conditions in the remark are not fulfilled this exception will be raised</exception>
		internal double TimeInterval
		{
			set 
			{ 
				bool valid = true;
				string debug = "";
				if(value < 0)
				{
					valid = false;
					debug += "Time interval has to be greater than zero";
				}
				if(value > mCycleDuration)
				{
					valid = false;
					if(debug.Length > 0)
						debug += "\nand smaller or equal to cycle duration";
					else
						debug += "Time interval has to be smaller or equal to cycle duration";
				}
				if(mTicksLeftOnCycle != 0)
				{
					if(((int)((mCycleDuration / value) - mTicksLeftOnCycle)) <= 0)
					{
						valid = false;
						if(debug.Length > 0)
							debug += "\nand possible ticks has to be greater than zero ((cycle duration / time interval) - ticks left on cycle)";
						else
							debug += "Possible ticks has to be greater than zero ((cycle duration / time interval) - ticks left on cycle)";
					}
					else
					{
						mTickOnRaiseEvent = ((int)(mCycleDuration / value)) - mTicksLeftOnCycle;
					}
				}
				if(!valid)	
					throw new ArgumentOutOfRangeException("value", value, debug);
				else
					mTimer.Interval = value;	
			}
		}

		/// <summary>This method sets the time duration of a cycle.</summary>
		/// <value>The cycle duration</value>
		/// <remarks>The time is in milliseconds. Conditions: Cycle duration has to be greater than zero,
		/// cycle duration has to greater than time interval and possible ticks has to be 
		/// greater than zero (cycle duration / time interval - ticks left on cycle),
		/// and cycle duration must be evenly dividable with timer interval</remarks>
		/// <exception cref="ArgumentOutOfRangeException">If the conditions in the remark are not fulfilled this exception will be raised.</exception>
		internal int CycleDuration
		{
			set
			{
				bool valid = true;
				string debug = "";

				if(value < 0)
				{
					valid = false;
					debug += "Cycle duration has to be greater than zero";
				}
				if(value < mTimer.Interval)
				{
					valid = false;
					if(debug.Length > 0)
						debug += "\nand cycle duration has to greater than time interval";
					else
						debug +="Cycle duration has to greater than time interval";
				}
				if(mTicksLeftOnCycle != 0)
				{
					if(((int)(value / mTimer.Interval)) - mTicksLeftOnCycle <= 0)
					{
						valid = false;
						if(debug.Length > 0)
							debug += "\nand possible ticks has to be greater than zero (cycle duration / time interval - ticks left on cycle)";
						else
							debug += "Possible ticks has to be greater than zero (cycle duration / time interval - ticks left to send event)";
					}
					else
						mTickOnRaiseEvent = (int)(value / mTimer.Interval) - mTicksLeftOnCycle;
				}
				if(value % mTimer.Interval != 0)
				{
					valid = false;

					if(debug.Length > 0)
						debug += "\nand cycle duration must be evenly dividable with timer interval";
					else
						debug += "Cycle duration must be evenly dividable with timer interval";

				}
				if(!valid)
					throw new ArgumentOutOfRangeException("value", value, debug);
				else
					mCycleDuration = value;
			}
		}
		/// <summary>
		/// This method sets how many ticks there should be left on the cycle to raise the event <see cref="TickOnCycleSoonEnding"/>
		/// </summary>
		/// <value>How many ticks there should be left of the cycle to raise the event <see cref="TickOnCycleSoonEnding"/></value>
		/// <remarks>Cycle duration / time intervals, gives you how many tick there could be on a cycle. Conditions:
		/// How many tick left on cycle has to be greater than zero and 
		/// possible ticks left on cycle has to be greater than zero = (cycle duration / time interval >= ticks left on cycle"/>)</remarks>
		/// <exception cref="ArgumentOutOfRangeException">If the conditions in the remark are not fulfilled this exception will be raised.</exception>
		internal int TicksLeftToSendEvent
		{
			set
			{
				bool valid = true;
				string debug = "";
				if(value < 0)
				{
					valid = false;
					debug += "How many tick left on cycle has to be greater than zero";
				}
				if( value > ((int)mCycleDuration / mTimer.Interval))
				{
					valid = false;
					if(debug.Length > 0)
						debug += "\nand possible ticks left on cycle has to be greater than zero(cycle duration / time interval >= ticks left)";
					else
						debug += "Possible ticks left on cycle has to be greater than zero(cycle duration / time interval >= ticks left)";

				}
				if(!valid)
					throw new ArgumentOutOfRangeException("value", value, debug);
				else
				{
					mTicksLeftOnCycle = value;
					mTickOnRaiseEvent = (int)(mCycleDuration / mTimer.Interval) - value;		
				}
			}
		}
		
		/// <summary>This method resets the cycle time.</summary>
		/// <remarks>This method shall be called to syncronize the agents clock with the server clock.</remarks>
		internal void Synchronize()
		{	
			mCurrentTick = 1;
			mEndTimeForCycle = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond + mCycleDuration;		
		}

		/// <summary>This method returns the time that is left on the current cycle.</summary>
		/// <value>The time left on the current cycle</value>
		/// <remarks>The time is in milliseconds. If the time have passed the cycle duration the returned time is less then 0.</remarks>
		internal int TimeLeftOnCycle
		{
			get { return (int)(mEndTimeForCycle - (DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond)); }
		}

		/// <summary>This method returns the state of the clock.</summary>
		/// <returns>Cycle duration, time interval, and time left on the current cycle</returns>
		public override string ToString()
		{
			return "CycleDuration = " + mCycleDuration + 
					", Interval = " + mTimer.Interval + "\n" +
					", Time Left On Cycle = " + ((mGameOn) ? (TimeLeftOnCycle).ToString() : "Clock not started");
		}
		#endregion

		#region Events
		/// <summary>Raised every <see cref="TimeInterval">specified time interval</see>, if the games state is play on.</summary>
		/// <remarks>Subscibe this event if you want to be notifed how much time there is left on the current cycle.</remarks>
		public event TimeEventHandler TickPlayOn;
		/// <summary>Raised every  <see cref="TimeInterval">specified time interval</see>, if the game state is play off.</summary>
		/// <remarks>Subscibe this event if you want to be notifed how much time have passed since the clock had been stopped.</remarks>
		public event TimeEventHandler TickPlayOff;
		/// <summary>Raised due to the <see cref="TicksLeftToSendEvent">specified ticks</see> there should be left on the current cycle.</summary>
		/// <remarks>Subscibe this event if you want to be notifed about cycle is soon ending.</remarks>
		public event TimeEventHandler TickOnCycleSoonEnding;
		/// <summary>How much time has passed a cycle duration</summary>
		/// <remarks>Subscibe this event if you want to be notified about how much time has passed of a cycle duration</remarks>
		public event TimeEventHandler TickOverTimeOnCycle;

		/// <summary>
		/// When the internal CRaPI clock ticks the subsribed events will be raised due to its specified raise condtions <see cref="TickPlayOn"/> 
		/// </summary>
		/// <remarks>To get these events you have to subscribe them</remarks>
		/// <example>This sample shows how to subscribe events in your code. 
		/// <code>
		/// Clock mClock = new Clock();
		/// mClock.TickPlayOn += new TimeEventHandler(OnTick);
		/// mClock.TickPlayOff += new TimeEventHandler(OnTick);
		/// 
		/// public void OnTick(object sender, TimeEventArgs e)
		/// {
		///		Do something...
		/// }
		/// </code>
		/// </example>
		/// <param name="o">The subscribed object</param>
		/// <param name="e">The elapsed event argument</param>
		/// <event cref="TickPlayOn">Raised every specified time interval if the games state is play on</event>
		/// <event cref="TickPlayOff">Raised every specified time interval if the game state is play off</event>
		/// <event cref="TickOnCycleSoonEnding">Raised when cycle is soon ending</event>
		/// <event cref="TickOverTimeOnCycle">Raised when time has passed a cycle duration</event>
		protected void OnTick(Object o, ElapsedEventArgs e)
		{
			if(mGameOn)
			{
				int timeLeftOnCycle = TimeLeftOnCycle;

				if(TickPlayOn != null)
					TickPlayOn(this, new TimeEventArgs(timeLeftOnCycle));
				if(TickOnCycleSoonEnding != null)
				{
					if(mCurrentTick == mTickOnRaiseEvent)
					{
						TickOnCycleSoonEnding(this, new TimeEventArgs(timeLeftOnCycle));
					}
				}
				if(TickOverTimeOnCycle != null)
				{
					if(timeLeftOnCycle < 0)
						TickOverTimeOnCycle(this, new TimeEventArgs(timeLeftOnCycle * -1));
				}
				mCurrentTick++;
			}		
			else
			{
				int timeAfterStop = (int)(DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond - mStopTime);
				if(TickPlayOff != null)
					TickPlayOff(this, new TimeEventArgs(timeAfterStop));
			}
		}
		#endregion
	}
}
