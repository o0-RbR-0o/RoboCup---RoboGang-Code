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

namespace TeamYaffa.CRaPI.Utility
{
	/// <summary>
	/// This class represents a rectangle in a 2 dimensional plane.
	/// </summary>
	public struct Rectangle2D
	{
		#region Members and constructors
		/// <summary>Lower left corner.</summary>
		private Point2D mLowerLeft;
		/// <summary>Lower right corner.</summary>
		private Point2D mLowerRight;
		/// <summary>Upper left corner.</summary>
		private Point2D mUpperLeft;
		/// <summary>Upper right corner.</summary>
		private Point2D mUpperRight;
		/// <summary>The central point of this rectangle.</summary>
		private Point2D mCentralPoint;

		/// <summary>
		/// Creates a Rectangle2D of the upper left corner coordinate and a width and an height.
		/// </summary>
		/// <param name="pUpperLeftX">The X position of the upper left corner</param>
		/// <param name="pUpperLeftY">The Y position of the upper left corner</param>
		/// <param name="pWidth">The width of the rectangle</param>
		/// <param name="pHeight">The height of the rectangle</param>
		public Rectangle2D(double pUpperLeftX, double pUpperLeftY, double pWidth, double pHeight)
		{
			mUpperLeft = new Point2D(pUpperLeftX, pUpperLeftY);
			mLowerLeft = new Point2D(pUpperLeftX, pUpperLeftY + pHeight);
			mLowerRight = new Point2D(pUpperLeftX + pWidth, pUpperLeftY + pHeight);
			mUpperRight = new Point2D(pUpperLeftX + pWidth, pUpperLeftY);
			mCentralPoint = new Point2D(mUpperLeft.X + (mUpperLeft.XDistanceTo(mUpperRight) / 2),
				mUpperLeft.Y + (mUpperLeft.YDistanceTo(mLowerLeft) / 2));
		}

		/// <summary>
		/// Create a Rectangle2D of four Point2D, representing the corners.
		/// </summary>
		/// <param name="pLowerLeft">Lower left corner</param>
		/// <param name="pLowerRight">Lower right corner</param>
		/// <param name="pUpperLeft">Upper left corner</param>
		/// <param name="pUpperRight">Upper right corner</param>
		public Rectangle2D(Point2D pLowerLeft, Point2D pLowerRight, Point2D pUpperLeft, Point2D pUpperRight)
		{
			mLowerLeft = pLowerLeft;
			mLowerRight = pLowerRight;
			mUpperLeft = pUpperLeft;
			mUpperRight = pUpperRight;
			mCentralPoint = new Point2D(mUpperLeft.X + (mUpperLeft.XDistanceTo(mUpperRight) / 2),
				mUpperLeft.Y + (mUpperLeft.YDistanceTo(mLowerLeft) / 2));
		}
		#endregion

		#region Geometrical operations
		/// <summary>
		/// Checks if a Point2D is contained by this rectangle.
		/// </summary>
		/// <param name="pPoint">The Point2D to make the check on.</param>
		/// <returns>true if the point is contained in this rectangle, otherwise false</returns>
		public bool Contains(Point2D pPoint)
		{
			bool xOk = false;
			bool yOk = false;

			//Check X is in range
			if((pPoint.X >= mLowerLeft.X) && (pPoint.X <= mLowerRight.X))
			{
				xOk = true;
			}

			//Check Y is in range
			if((pPoint.Y >= mUpperLeft.Y) && (pPoint.Y <= mLowerLeft.Y))
			{
				yOk = true;
			}

			return xOk && yOk;
		}
		#endregion

		#region Properties
		/// <summary>Lower left corner of the Rectangle.</summary>
		public Point2D LowerLeft
		{
			get{ return mLowerLeft; }
			set
			{
				mLowerLeft = value;
				mCentralPoint = new Point2D(mUpperLeft.X + (mUpperLeft.XDistanceTo(mUpperRight) / 2),
					mUpperLeft.Y + (mUpperLeft.YDistanceTo(mLowerLeft) / 2));
			}
		}

		/// <summary>Lower right corner of the Rectangle.</summary>
		public Point2D LowerRight
		{
			get{ return mLowerRight; }
			set
			{
				mLowerRight = value;
				mCentralPoint = new Point2D(mUpperLeft.X + (mUpperLeft.XDistanceTo(mUpperRight) / 2),
					mUpperLeft.Y + (mUpperLeft.YDistanceTo(mLowerLeft) / 2));
			}
		}

		/// <summary>Upper left corner of the Rectangle.</summary>
		public Point2D UpperLeft
		{
			get{ return mUpperLeft; }
			set
			{
				mUpperLeft = value;
				mCentralPoint = new Point2D(mUpperLeft.X + (mUpperLeft.XDistanceTo(mUpperRight) / 2),
					mUpperLeft.Y + (mUpperLeft.YDistanceTo(mLowerLeft) / 2));
			}
		}

		/// <summary>Upper right corner of the Rectangle.</summary>
		public Point2D UpperRight
		{
			get{ return mUpperRight; }
			set
			{
				mUpperRight = value;
				mCentralPoint = new Point2D(mUpperLeft.X + (mUpperLeft.XDistanceTo(mUpperRight) / 2),
					mUpperLeft.Y + (mUpperLeft.YDistanceTo(mLowerLeft) / 2));
			}
		}

		/// <summary>Gets the central point of the rectangle.</summary>
		public Point2D CentralPoint
		{
			get{ return mCentralPoint; }
		}
		#endregion

		#region Misc.
		/// <summary>
		/// The string representation of the rectangle.
		/// </summary>
		/// <returns>[upper left point][upper right point][lower left point][lower right point]</returns>
		public override string ToString()
		{
			return String.Format("[{0}][{1}][{2}][{3}]", mUpperLeft, mUpperRight, mLowerLeft, mLowerRight);
		}
		#endregion
	}
}
