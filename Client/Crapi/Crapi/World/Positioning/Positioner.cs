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

namespace TeamYaffa.CRaPI.World.Positioning
{
	/// <summary>
	/// This class handles positioning of the player with the help of IntersectionPoints.
	/// </summary>
	public class Positioner
	{
		#region Members and constructors
		/// <summary>The resolution used for positioning</summary>
		private double mResolution = 0.2;
		/// <summary>The length of each simulator step</summary>
		private static double mSimulatorStep;
		/// <summary>The width of the play field</summary>
		private static double mFieldWidth;
		/// <summary>The height of the play field</summary>
		private static double mFieldHeight;
		/// <summary>The offset in X from origo on the play field</summary>
		private static double mXOffset;
		/// <summary>The offset in Y from origo on the play field</summary>
		private static double mYOffset;
		/// <summary>The PositionElements used for positioning</summary>
		private Hashtable mPositionElements;
		private WorldModel mWM;
	
		/// <summary>
		/// Creates a Positioner object
		/// </summary>
		/// <param name="pStep">The simulator step</param>
		/// <param name="pWM">The actual world-model of the agent</param>
		public Positioner(double pStep, WorldModel pWM)
		{
			mSimulatorStep = pStep;
			mPositionElements = new Hashtable();
			mWM = pWM;
		}

		static Positioner()
		{
			Hashtable temp = WorldModel.initStaticObjects(Side.Left);
			mFieldWidth = (Point2D)(temp["f l 0"]) - (Point2D)(temp["f r 0"]);
			mFieldHeight = (Point2D)(temp["f t 0"]) - (Point2D)(temp["f b 0"]);
			mXOffset = mFieldWidth / 2;
			mYOffset = mFieldHeight / 2;
		}

		#endregion

		#region Calculations
		/// <summary>
		/// Calculates the position of an object from the supplied intersections
		/// </summary>
		/// <param name="pIntersections">An ArrayList of intersections</param>
		/// <param name="pDRPos">The position calculated with dead reckogning, used for weighing intersections</param>
		/// <returns>The position of an object</returns>
		public Point2D CalculatePosition(Intersection[] pIntersections, Point2D pDRPos)
		{
			mPositionElements.Clear();
			foreach(Intersection intersection in pIntersections)
			{
				Point2D centerPoint = CalculateCenterPoint(intersection);
				PositionElement posEl;
			
				if(mPositionElements.Contains(centerPoint))
				{
					posEl = (PositionElement)(mPositionElements[centerPoint]);
					posEl.Increase();
				}
				else
				{
					double weight = 1 / (centerPoint - pDRPos);
					posEl = new PositionElement(centerPoint, weight);
					mPositionElements.Add(centerPoint, posEl);
				}
			}

			ICollection keys = mPositionElements.Keys;
			PositionElement bestGuess = new PositionElement(new Point2D(0,0), 0);
			foreach(Point2D key in keys)
			{
				PositionElement posEl = (PositionElement)(mPositionElements[key]);
				if(posEl.Value > bestGuess.Value)
				{
					bestGuess = posEl;
				}
			}
			
			Point2D position =  bestGuess.CenterPoint;

			if(!MathUtility.WithinPhysicalBoundary(mWM, position))
				position = CalculateClosestLegalPosition(position);

			return position;
		}

		/// <summary>
		/// Calculates the coordinates of the center point of the field element containing the supplied point
		/// </summary>
		/// <param name="pIntersection">Supplied point</param>
		/// <returns>Center point</returns>
		private Point2D CalculateCenterPoint(Intersection pIntersection)
		{
			int X = (int)(XToWorld(pIntersection.Point.X) / mResolution);
			int Y = (int)(YToWorld(pIntersection.Point.Y) / mResolution);

			double x = X * mResolution + (mResolution / 2.0);
			double y = Y * mResolution + (mResolution / 2.0);

			return new Point2D(x - mXOffset, y - mYOffset);
		}

		/// <summary>
		/// Calculate the coordinates of the position of the player with regard to previous location,
		/// direction and speed
		/// </summary>
		/// <param name="pOldPos">The position in the previous cycle</param>
		/// <param name="pDirection">The direction</param>
		/// <param name="pSpeed">The speed</param>
		/// <param name="pCycles">How many cycles ahead the position should be calculated for</param>
		/// <returns>The approximate location of the player</returns>
		public Point2D CalculateDeadReckoningPosition(Point2D pOldPos, double pDirection, double pSpeed, int pCycles)
		{
			Point2D position = new Point2D(pOldPos, pDirection, pSpeed * pCycles);
			
			if(!MathUtility.WithinPhysicalBoundary(mWM, position))
				position = CalculateClosestLegalPosition(position);

			return position;
		}
		#endregion

		#region Coordinate manipulation
		/// <summary>
		/// Converts a X-coordinate to the coordinate system of the play field
		/// </summary>
		/// <param name="x">X coordinate to convert</param>
		/// <returns>X coordinate</returns>
		public static float XToWorld(double x)
		{
			return (float)(x+mXOffset);
		}

		/// <summary>
		/// Converts a Y-coordinate to the coordinate system of the play field
		/// </summary>
		/// <param name="y">Y coordinate to convert</param>
		/// <returns>Y coordinate</returns>
		public static float YToWorld(double y)
		{
			return (float)(y+mYOffset);
		}

		/// <summary>
		/// Calculates the position within the play-field that is closest to a point
		/// </summary>
		/// <param name="pPosition">The point to be as close to as possible</param>
		/// <returns>The point closest to <paramref name="pPosition"/></returns>
		public Point2D CalculateClosestLegalPosition(Point2D pPosition)
		{
			if(MathUtility.WithinPhysicalBoundary(mWM, pPosition))
				return pPosition;

			double minY = ((Point2D)mWM.StaticObjects["f t 0"]).Y;
			double maxY = ((Point2D)mWM.StaticObjects["f b 0"]).Y;
			double minX = ((Point2D)mWM.StaticObjects["f l 0"]).X;
			double maxX = ((Point2D)mWM.StaticObjects["f r 0"]).X;

			if(mWM.Player.TeamSide == Side.Right)
			{
				minY *= -1;
				maxY *= -1;
				minX *= -1;
				maxX *= -1;
			}

			Point2D pos = new Point2D(pPosition);

			if(pos.X > maxX)
				pos.X = maxX;
			if(pos.X < minX)
				pos.X = minX;
			if(pos.Y > maxY)
				pos.Y = maxY;
			if(pos.Y < minY)
				pos.Y = minY;

			return pos;
		}

		#endregion

		#region Properties
		/// <summary>Gets and sets the resolution used for positioning</summary>
		public double Resolution
		{
			get{return mResolution;}
			set{mResolution = value;}
		}
		#endregion
	}
}
