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

namespace TeamYaffa.CRaPI.World
{
	/// <summary>
	/// This is a representation of the flags that is sent from the server
	/// in the <c>see</c> message.
	/// </summary>
	public struct Flag : IComparable
	{
		#region Members and constructors
		/// <summary>The distance between the player and the seen flag</summary>
		private double mDistance;
		/// <summary>The direction to the flag, relative the direction of the players view</summary>
		private double mDirection;
		/// <summary>The name of the flag</summary>
		private String mName;
		/// <summary>The static position of the flag</summary>
		private Point2D mPosition;

		/// <summary>
		/// Consructs a Flag
		/// </summary>
		/// <param name="pName">The name</param>
		/// <param name="pPosition">The position</param>
		/// <param name="pDistance">The distance to the flag</param>
		/// <param name="pDirection">The direction to the flag</param>
		public Flag(string pName, Point2D pPosition, double pDistance, double pDirection)
		{
			mName = pName;
			mPosition = pPosition;
			mDistance = pDistance;
			mDirection = pDirection;
		}

		#endregion

		#region Properties
		/// <summary>
		/// The distance between the player and the seen flag
		/// </summary>
		public double Distance
		{
			get { return mDistance; }
		}

		/// <summary>
		/// The direction to the flag, relative the direction of the players view
		/// </summary>
		public double Direction
		{
			get { return mDirection; }
		}

		/// <summary>
		/// The name of the flag
		/// </summary>
		public string Name
		{
			get { return mName; }
		}

		/// <summary>
		/// The static position of the flag
		/// </summary>
		public Point2D Position
		{
			get { return mPosition; }
		}
		#endregion

		#region Comparisons, IComparable implementation
		/// <summary>
		/// Compares the current instance with another flag.
		/// </summary>
		/// <param name="pFlag">The flag to compare with.</param>
		/// <returns>A 32-bit signed integer that indicates the relative order of the flags.
		/// The return value has these meanings:
		/// <list type="bullet">
		/// <item><description>Less than zero : The distance to this flag i less than to <c>pFlag</c>.</description></item>
		/// <item><description>Zero : The distance to the flags are equal.</description></item>
		/// <item><description>Greater than zero : The distance to this flag is greater than to <c>pFlag</c>.</description></item>
		/// </list></returns>
		/// <exception cref="System.ArgumentException"><c>pFlag</c> is not a <c>Flag</c>.</exception>
		int IComparable.CompareTo(object pFlag)
		{
			if((pFlag is Flag))
			{
				if(mDistance < ((Flag)pFlag).mDistance)
					return -1;
				if(mDistance > ((Flag)pFlag).mDistance)
					return 1;
				else
					return 0;
			}
			else
				throw new ArgumentException("The parameter is not a flag", "pFlag");
		}

		/// <summary>
		/// Compares the current instance with another flag.
		/// </summary>
		/// <param name="pFlag">The flag to compare with.</param>
		/// <returns>A 32-bit signed integer that indicates the relative order of the flags.
		/// The return value has these meanings:
		/// <list type="bullet">
		/// <item><description>Less than zero : The distance to this flag i less than to <c>pFlag</c>.</description></item>
		/// <item><description>Zero : The distance to the flags are equal.</description></item>
		/// <item><description>Greater than zero : The distance to this flag is greater than to <c>pFlag</c>.</description></item>
		/// </list></returns>
		public int CompareTo(Flag pFlag)
		{
			if(mDistance < ((Flag)pFlag).mDistance)
				return -1;
			if(mDistance > ((Flag)pFlag).mDistance)
				return 1;
			else
				return 0;
		}

		#endregion

		#region Misc. operations

		/// <summary>
		/// Creates a string representation of the Flag
		/// </summary>
		/// <returns>string representation</returns>
		public override string ToString()
		{
			return mName + ": " + mPosition + ", " + mDistance + ", " + mDirection;
		}
		#endregion
	}
}
