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
	/// This class represents a line of double precision on a 2 dimensional plane.
	/// </summary>
	/// <remarks>The line is represented by a <see cref="Start">start point</see> and an
	/// <see cref="End">end point</see>.</remarks>
	public struct Line2D
	{
		#region Members and Constructors
		/// <summary>Start position of line.<seealso cref="Start"/></summary>
		private Point2D mStart;
		/// <summary>End position of line.<seealso cref="End"/></summary>
		private Point2D mEnd;

		/// <summary>
		/// Constructs a Line2D
		/// </summary>
		/// <remarks>The line is created with two points, which are the lines start and end points.</remarks>
		/// <param name="pStart">Start position of line</param>
		/// <param name="pEnd">End position of line</param>
		public Line2D(Point2D pStart, Point2D pEnd)
		{
			mStart = pStart;
			mEnd = pEnd;
		}
		#endregion

		#region Operations
		/// <summary>
		/// Checks if this Line2D intersects another Line2D
		/// </summary>
		/// <remarks>See <see href="http://astronomy.swin.edu.au/~pbourke/geometry/lineline2d/"/> for details</remarks>
		/// <param name="pLine">Line2D to check for intersection</param>
		/// <returns>true if intersection is found, otherwise false</returns>
		public bool Intersects(Line2D pLine)
		{
			double x1 = mStart.X;
			double x2 = mEnd.X;
			double x3 = pLine.Start.X;
			double x4 = pLine.End.X;

			double y1 = mStart.Y;
			double y2 = mEnd.Y;
			double y3 = pLine.Start.Y;
			double y4 = pLine.End.Y;

			double ua = ((x4-x3)*(y1-y3)-(y4-y3)*(x1-x3)) / ((y4-y3)*(x2-x1)-(x4-x3)*(y2-y1));
			
			//no intersection
			if(ua > 1 || ua < 0)
			{
				return false;
			}
			return true;
		}

		/// <summary>
		/// Calculates where two lines intersect.
		/// </summary>
		/// <remarks>See <see href="http://astronomy.swin.edu.au/~pbourke/geometry/lineline2d/"/> for details</remarks>
		/// <param name="pLine">Line2D to check for intersection.</param>
		/// <param name="intersection">Intersection point.</param>
		/// <returns>true if intersection is found, otherwise false</returns>
		public bool Intersection(Line2D pLine, out Point2D intersection)
		{
			double x1 = mStart.X;
			double x2 = mEnd.X;
			double x3 = pLine.Start.X;
			double x4 = pLine.End.X;

			double y1 = mStart.Y;
			double y2 = mEnd.Y;
			double y3 = pLine.Start.Y;
			double y4 = pLine.End.Y;

			double ua = ((x4-x3)*(y1-y3)-(y4-y3)*(x1-x3)) / ((y4-y3)*(x2-x1)-(x4-x3)*(y2-y1));
			
			if(!Intersects(pLine))
			{
				intersection = new Point2D(0,0);
				return false;
			}

			double X = x1 + ua*(x2-x1);
			double Y = y1 + ua*(y2-y1);
			intersection = new Point2D(X,Y);
			return true;
		}

		/// <summary>
		/// Checks if a Point2D is on this Line2D
		/// </summary>
		/// <param name="pPoint">The point</param>
		/// <returns>true if the point is on line, otherwise false</returns>
		public bool OnLine(Point2D pPoint)
		{
			double xOff = mStart.XDistanceTo(pPoint);
			double yOff = mStart.YDistanceTo(pPoint);

			//out of bounds
			if(pPoint.X < mStart.X || pPoint.X > mEnd.X)
			{
				return false;
			}
			else if(pPoint.Y < mStart.Y || pPoint.Y > mEnd.Y)
			{
				return false;
			}

			//calculation based on uniformity of triangles
			if((xOff/yOff)==(XLength/YLength))
			{
				return true;
			}
			return false;
		}
		#endregion

		#region Properties
		/// <summary>Start position of line</summary>
		/// <value>A Point2D as a representation of the point where the line starts.</value>
		public Point2D Start
		{
			get{ return mStart; }
			set{ mStart = value; }
		}

		/// <summary>End position of line</summary>
		/// <value>A Point2D as a representation of the point where the line ends.</value>
		public Point2D End
		{
			get{ return mEnd; }
			set{ mEnd = value; }
		}

		/// <summary>Length in X of Line</summary>
		/// <remarks>The XLength of the line is the same as the
		/// <see cref="Point2D.XDistanceTo">XDistanceTo</see> between <see cref="Start"/> and
		/// <see cref="End"/>.</remarks>
		public double XLength
		{
			get{ return mStart.XDistanceTo(mEnd); }
		}

		/// <summary>Length in Y of Line</summary>
		/// <remarks>The YLength of the line is the same as the
		/// <see cref="Point2D.YDistanceTo">YDistanceTo</see> between <see cref="Start"/> and
		/// <see cref="End"/>.</remarks>
		public double YLength
		{
			get{ return mStart.YDistanceTo(mEnd); }
		}

		/// <summary>Length of Line.
		/// <seealso cref="Point2D.operator-">Point2D substraction</seealso></summary>
		/// <remarks>The Length of the line is the distance between the <see cref="Start"/> point and
		/// the <see cref="End"/> point.</remarks>
		public double Length
		{
			get{ return mStart-mEnd; }
		}
		#endregion
	}
}
