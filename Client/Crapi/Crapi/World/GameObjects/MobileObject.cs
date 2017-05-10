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
using TeamYaffa.CRaPI.Utility;

namespace TeamYaffa.CRaPI.World.GameObjects
{
	/// <summary>
	/// This class represents a MobileObject.
	/// </summary>
	public class MobileObject : FieldObject
	{
		#region Members and Constructors
		/// <summary>The direction change</summary>
		private double mDirectionChange;
		/// <summary>The distance change</summary>
		private double mDistanceChange;
		/// <summary>The server cycle in which this MobileObject was last seen</summary>
		private int mLastSeen;
		/// <summary>The current approximate speed of the MobileObject (distance per cycle)</summary>
		private double mSpeedAmount;
		/// <summary>The current approximate direction of the speed of the MobileObject</summary>
		private double mSpeedDirection;
		/// <summary>A collection of forecasted positions, where the element no. 1 is one cycle into the future and so on</summary>
		private Point2D[] mForecasts = new Point2D[2];
		/// <summary>Has this MobileObject been seen this cycle</summary>
		private bool mSeenThisCycle;

		/// <summary>
		/// Default constructor
		/// </summary>
		/// <remarks>Sets last seen to <c>-1</c></remarks>
		public MobileObject()
		{
			mLastSeen = -1;
		}

		/// <summary>
		/// Constructs a MobileObject
		/// </summary>
		/// <param name="pDirectionChange">The direction change</param>
		/// <param name="pDistanceChange">The distance change</param>
		/// <param name="pLastSeen">The server cycle in which this MobileObject was last seen</param>
		/// <param name="pPosition">The position</param>
		/// <param name="pDistance">The distance to this MobileObject</param>
		/// <param name="pDirection">The direction to this MobileObject</param>
		/// <param name="pName">The name of this MobileObject</param>
		public MobileObject(double pDirectionChange, double pDistanceChange, int pLastSeen,
			Point2D pPosition, double pDistance, double pDirection, string pName)
			: base(pPosition, pDistance, pDirection, pName)
		{
			mDirectionChange = pDirectionChange;
			mDistanceChange = pDirectionChange;
			mLastSeen = pLastSeen;
		}

		/// <summary>
		/// Constructs a MobileObject
		/// </summary>
		/// <param name="pDirectionChange">The direction change</param>
		/// <param name="pDistanceChange">The distance change</param>
		/// <param name="pLastSeen">The server cycle in which this MobileObject was last seen</param>
		/// <param name="pPosition">The position</param>
		public MobileObject(double pDirectionChange, double pDistanceChange, int pLastSeen, Point2D pPosition)
			: base(pPosition)
		{
			mDirectionChange = pDirectionChange;
			mDistanceChange = pDirectionChange;
			mLastSeen = pLastSeen;
		}

		/// <summary>
		/// Constructs a MobileObject
		/// </summary>
		/// <param name="pLastSeen">The server cycle in which this MobileObject was last seen</param>
		/// <param name="pPosition">The position</param>
		public MobileObject(int pLastSeen, Point2D pPosition)
			: base(pPosition)
		{
			mLastSeen = pLastSeen;
		}
		
		#endregion
		
		#region Properties
		
		/// <summary>The distance change</summary>
		public double DistanceChange
		{
			get { return mDistanceChange; }
			set { mDistanceChange = value; }
		}

		/// <summary>The server cycle in which this MobileObject was last seen</summary>
		public int LastSeen
		{
			get { return mLastSeen; }
			set { mLastSeen = value; }
		}

		/// <summary>Gets and sets if the object has been seen this cycle.</summary>
		public bool SeenThisCycle
		{
			get{return mSeenThisCycle;}
			set{mSeenThisCycle = value;}
		}

		/// <summary>The direction change</summary>
		public double DirectionChange
		{
			get { return mDirectionChange; }
			set { mDirectionChange = value; }
		}

		/// <summary>The current approximate speed of the MobileObject (distance per cycle)</summary>
		public double SpeedAmount
		{
			get{ return mSpeedAmount; }
			set{ mSpeedAmount = value; }
		}

		/// <summary>The current approximate direction of the speed of the MobileObject</summary>
		public double SpeedDirection
		{
			get{ return mSpeedDirection; }
			set{ mSpeedDirection = value; }
		}

		/// <summary>The forecasted positions of the MobileObject</summary>
		public Point2D[] Forecasts
		{
			get{ return mForecasts; }
		}

		#endregion

		#region Misc. operations

		/// <summary>
		/// Calculates the speed direction and amount of this MobileObject
		/// </summary>
		/// <param name="pSelf">This player</param>
		public void CalculateSpeed(FieldPlayer pSelf)
		{
			double prevSpeedDir = SpeedDirection;
			bool isKicker = DistanceChange > 0 ? true : false;

			if(Distance == 0)
			{
				SpeedAmount = 0;
				SpeedDirection = 0;
				return;
			}

			double Pry = Position.SignedYDistanceTo(pSelf.Position);
			double Prx = Position.SignedXDistanceTo(pSelf.Position);
			double ery = Pry / Distance;
			double erx = Prx / Distance;

			double Vry = ((mDistanceChange * ery) + ((mDirectionChange * Distance * erx) / (180 / Math.PI))) / (erx*erx + ery*ery);
			double Vrx = (mDistanceChange - (Vry*ery)) / erx;
			
			double ourVx = pSelf.SpeedAmount * Math.Cos(MathUtility.ToRadians(pSelf.SpeedDirection));
			double ourVy = pSelf.SpeedAmount * Math.Sin(MathUtility.ToRadians(pSelf.SpeedDirection));
			
			double totalVx = ourVx + Vrx;
			double totalVy = ourVy + Vry;

			SpeedAmount = Math.Sqrt(totalVy*totalVy + totalVx*totalVx);		
			SpeedDirection = MathUtility.NormalizeAngle(MathUtility.ToDegrees(Math.Atan(totalVy / totalVx)));
				

			//
			// Its flipped
			//
			if(totalVx > 0 && totalVy < 0) //down left
			{
				SpeedDirection = (180-SpeedDirection) * -1;
			}
			else if(totalVx < 0 && totalVy < 0) //down right
			{
			}
			else if(totalVx > 0 && totalVy > 0) //up left
			{
				SpeedDirection -= 180;
			}
			else if(totalVx < 0 && totalVy > 0) //up right
			{
			}
			
			SpeedDirection = MathUtility.NormalizeAngle(SpeedDirection);

			if(SpeedAmount == 0 || totalVx == 0)
				SpeedDirection = 0;
		}


		/// <summary>
		/// Returns a string representation of the MobileObject
		/// </summary>
		/// <returns>string representation</returns>
		public override String ToString() 
		{
			return "(DistanceChange: " + mDistanceChange + ", DirectionChange: " + mDirectionChange +
				", LastSeen: " + mLastSeen + ", " + base.ToString() + ")";
		}
		#endregion

		#region Forecasts

		/// <summary>
		/// Forecast the position of this MobileObject a certain cycles into the future
		/// </summary>
		/// <param name="pCycles">The number of cycles for forecast</param>
		/// <param name="pPositioner">The positioner to use when calculating the position</param>
		/// <returns>A forecasted position</returns>
		public Point2D ForecastPosition(int pCycles, Positioning.Positioner pPositioner)
		{
			//if array is too small, resize it
			if(mForecasts.Length <= pCycles)
			{
				Point2D[] temp = new Point2D[pCycles + 1];
				mForecasts.CopyTo(temp, 0);
				mForecasts = temp;
			}

			mForecasts[pCycles] = pPositioner.CalculateDeadReckoningPosition(this.Position, mSpeedDirection, mSpeedAmount, pCycles);
			return mForecasts[pCycles];
		}

		/// <summary>
		/// Clears the forecasts
		/// </summary>
		public void ClearForecasts()
		{
			mForecasts = new Point2D[2];
		}
		#endregion
	}
}
