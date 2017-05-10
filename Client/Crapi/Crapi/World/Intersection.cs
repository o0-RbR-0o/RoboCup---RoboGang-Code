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
	/// A point where the imagined circles of two flag-distances intersect.
	/// </summary>
	/// <remarks>If a circle is imagined around each flag, where the circle represent the distance to
	/// the flag, then this class is a representation of an intersection between two flags. The order
	/// of the flag can be of any order.</remarks>
	public struct Intersection
	{
		#region Members and Constructors

		/// <summary>The position</summary>
		private Point2D mPoint;
		/// <summary>The first flag</summary>
		private Flag mFlagOne;
		/// <summary>The second flag</summary>
		private Flag mFlagTwo;

		/// <summary>
		/// Constructs an Intersection, with a point and 2 flags.
		/// </summary>
		/// <param name="pPoint">The position</param>
		/// <param name="pFlagOne">The first flag</param>
		/// <param name="pFlagTwo">The second flag</param>
		public Intersection(Point2D pPoint, Flag pFlagOne, Flag pFlagTwo)
		{
			mPoint = pPoint;
			mFlagOne = pFlagOne;
			mFlagTwo = pFlagTwo;
		}
		#endregion

		#region Properties
		/// <summary>The position</summary>
		public Point2D Point
		{
			get { return mPoint; }
		}

		/// <summary>The first flag</summary>
		public Flag FlagOne
		{
			get { return mFlagOne; }
		}

		/// <summary>The second flag</summary>
		public Flag FlagTwo
		{
			get { return mFlagTwo; }
		}
		#endregion
	}
}
