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
using TeamYaffa.CRaPI.World.GameObjects;
using System.Collections;

namespace TeamYaffa.CRaPI.Utility
{
	/// <summary>
	/// This class provides a set of frequently used mathematical functions
	/// </summary>
	public class MathUtility
	{
		#region static utility methods
		/// <summary>The number of degrees for 1 radian</summary>
		private static readonly double mToDegreesConstant = 180/System.Math.PI;

		/// <summary>
		/// Transforms an angle from radians to degrees
		/// </summary>
		/// <param name="pRadians">The angle to transform</param>
		/// <returns>The angle in degrees</returns>
		public static double ToDegrees(double pRadians)
		{
			return mToDegreesConstant * pRadians;
		}

		/// <summary>
		/// Transforms an angle from degrees to radians
		/// </summary>
		/// <param name="pDegrees">The angle to transform</param>
		/// <returns>The angle in radians</returns>
		public static double ToRadians(double pDegrees)
		{
			return pDegrees / mToDegreesConstant;
		}

		/// <summary>
		/// Normalizes an angle to fit into the range -180 ~ 180
		/// </summary>
		/// <param name="pAngle">Anglo to normalize</param>
		/// <returns>Normalized angle, a value between -180 and 180</returns>
		public static double NormalizeAngle(double pAngle)
		{
			while(pAngle > 180)
			{
				pAngle -= 360;
			}
			while(pAngle < -180)
			{
				pAngle += 360;
			}

			return pAngle;
		}
		
//		/// <summary>
//		/// Calculates the intersection point between the player and a MobileObject
//		/// </summary>
//		/// <param name="pSelf">The player</param>
//		/// <param name="pIntersectObject">The MobileObject to intersect</param>
//		/// <param name="pIntersectionPoint">The intersection point</param>
//		/// <returns>true if intersection found, otherwise false</returns>
//		public static bool IntersectionPoint(Player pSelf, MobileObject pIntersectObject, out Point2D pIntersectionPoint)
//		{
//			if(pIntersectObject.SpeedAmount < 0.1)
//			{
//				pIntersectionPoint = pIntersectObject.Position;
//				return true;
//			}
//
//			FieldPlayer self = pSelf.World.Self;
//			int simulatorStep = pSelf.ServerParam.SimulatorStep;
//			float intersectObjCycleDist = (float)(pIntersectObject.SpeedAmount); 
//			float selfCycleDist = (float)(pSelf.ServerParam.PlayerSpeedMax);
//
//			double decay = 1;
//
//			//Check for reasonable speeds.
//			if(pIntersectObject is Ball)
//			{
//				decay = pSelf.ServerParam.BallDecay;
//
//				if(intersectObjCycleDist > pSelf.ServerParam.BallSpeedMax)
//					intersectObjCycleDist = (float)pSelf.ServerParam.BallSpeedMax;
//			}
//			else if(pIntersectObject is FieldPlayer && intersectObjCycleDist > pSelf.ServerParam.PlayerSpeedMax)
//			{
//				intersectObjCycleDist = (float)pSelf.ServerParam.PlayerSpeedMax;
//			}
//			
//			int step = 0;
//			int stepFactor = 0;
//			pIntersectionPoint = pIntersectObject.Position;
//			double intersectLength = intersectObjCycleDist;
//	
//			while(step < 200)
//			{
//				step++;
//
//				//pIntersectionPoint = new Point2D(new Point2D(pIntersectObject.Position.X+1, pIntersectObject.Position.Y), pIntersectObject.SpeedDirection, intersectLength);
//				pIntersectionPoint = new Point2D(pIntersectObject.Position, pIntersectObject.SpeedDirection, intersectLength);
//
//				if(!WithinPhysicalBoundary(pSelf.World, pIntersectionPoint))
//				{
//					return false;
//				}
//				
//				if(self.Position - pIntersectObject.Position > 2)
//				{
//					stepFactor = 6;
//				}
//				else
//					stepFactor = 2;
//
//				float distanceToIntersect = (float)(pSelf.World.MyPosition - pIntersectionPoint);
//				float myMaxDistance = (float)((step - stepFactor) * selfCycleDist);
//
//				if(myMaxDistance >= distanceToIntersect)
//				{
////					if(self.Position - pIntersectObject.Position < 2.5)
////					{
////						pIntersectionPoint = pIntersectionPoint.MidwayTo(pIntersectObject.Position);
////					}
//					return true;
//				}
//				
//				intersectLength += intersectObjCycleDist * Math.Pow(decay, step);
//			}
//			return false;
//		}

		/// <summary>
		/// Calculates the intersection point between the player and a MobileObject
		/// </summary>
		/// <param name="pSelf">The player</param>
		/// <param name="pIntersectObject">The MobileObject to intersect</param>
		/// <param name="pIntersectionPoint">The intersection point</param>
		/// <returns>true if intersection found, otherwise false</returns>
		public static bool IntersectionPoint(Player pSelf, MobileObject pIntersectObject, out Point2D pIntersectionPoint)
		{
			if(pIntersectObject.SpeedAmount == 0)
			{
				pIntersectionPoint = pIntersectObject.Position;
				return true;
			}

			FieldPlayer self = pSelf.World.Self;
			int simulatorStep = pSelf.ServerParam.SimulatorStep;
			float intersectObjCycleDist = (float)(pIntersectObject.SpeedAmount); 
			float selfCycleDist = (float)(pSelf.ServerParam.PlayerSpeedMax * (pSelf.SenseBody.Stamina/pSelf.ServerParam.MaxStamina));

			double decay = 1;


			//Check for reasonable speeds.
			if(pIntersectObject is Ball)
			{
				decay = pSelf.ServerParam.BallDecay;

				if(intersectObjCycleDist > pSelf.ServerParam.BallSpeedMax)
					intersectObjCycleDist = (float)pSelf.ServerParam.BallSpeedMax;
			}
			else if(pIntersectObject is FieldPlayer && intersectObjCycleDist > pSelf.ServerParam.PlayerSpeedMax)
			{
				intersectObjCycleDist = (float)pSelf.ServerParam.PlayerSpeedMax;
			}
			
			int step = 0;
			pIntersectionPoint = pIntersectObject.Position;
			double intersectLength = intersectObjCycleDist;
	
			while(step < 200)
			{
				step++;
				double offset = 0.8D;
				pIntersectionPoint = new Point2D(new Point2D(pIntersectObject.Position.X+offset, pIntersectObject.Position.Y), pIntersectObject.SpeedDirection, intersectLength);
				
				if(!WithinPhysicalBoundary(pSelf.World, pIntersectionPoint))
				{
					return false;
				}

				float distanceToIntersect = (float)(pSelf.World.MyPosition - pIntersectionPoint);
				float myMaxDistance = (float)((step - stepFactor(pIntersectObject, pSelf)) * selfCycleDist);

				if(myMaxDistance >= distanceToIntersect)
				{
					return true;
				}
				intersectLength += intersectObjCycleDist * Math.Pow(decay, step);
			}
			return false;
		}

		private static double stepFactor(MobileObject pMO, Player pSelf)
		{
			double speedFactor = (pMO.SpeedAmount - pSelf.SenseBody.SpeedAmount) * 1.5;
			
			if(speedFactor < 0)
				speedFactor = 0;

			double factor = speedFactor;
			return factor;
		}


		/// <summary>
		/// Checks if a position is within the physical boundary for the agent.
		/// </summary>
		/// <param name="pWm">The agents world-model</param>
		/// <param name="pPosition">The position to check</param>
		/// <returns>True if the point is within the physical boundary, false otherwise</returns>
		public static bool WithinPhysicalBoundary(World.WorldModel pWm, Point2D pPosition)
		{
			Hashtable staticObj = pWm.StaticObjects;
			double minY = ((Point2D)staticObj["f t 0"]).Y;
			double maxY = ((Point2D)staticObj["f b 0"]).Y;
			double minX = ((Point2D)staticObj["f l 0"]).X;
			double maxX = ((Point2D)staticObj["f r 0"]).X;

			if(pWm.Player.TeamSide == Side.Right)
			{
				minY *= -1;
				maxY *= -1;
				minX *= -1;
				maxX *= -1;
			}

			if(pPosition.Y > maxY || pPosition.Y < minY || pPosition.X > maxX || pPosition.X < minX)
			{
				return false;
			}
			return true;
		}

		/// <summary>
		/// Checks if a position is within the boundaries of the playfield.
		/// </summary>
		/// <param name="pWm">The agents world-model</param>
		/// <param name="pPosition">The position to check</param>
		/// <returns>True if the point is within boundaries of the playfield, false otherwise</returns>
		public static bool WithinPlayFieldBoundary(World.WorldModel pWm, Point2D pPosition)
		{
			Hashtable staticObj = pWm.StaticObjects;
			double minY = ((Point2D)staticObj["f c t"]).Y;
			double maxY = ((Point2D)staticObj["f c b"]).Y;
			double minX = ((Point2D)staticObj["g l"]).X;
			double maxX = ((Point2D)staticObj["g r"]).X;

			if(pWm.Player.TeamSide == Side.Right)
			{
				minY *= -1;
				maxY *= -1;
				minX *= -1;
				maxX *= -1;
			}

			if(pPosition.Y > maxY || pPosition.Y < minY || pPosition.X > maxX || pPosition.X < minX)
			{
				return false;
			}
			return true;
		}

//		public static float AngleDifference(float pAngle1, float pAngle2)
//		{
//			if(pAngle1 < 0)
//			{
//				pAngle1 += 360;
//			}
//
//			if(pAngle2 < 0)
//			{
//				pAngle2 += 360;
//			}
//
//			if(pAngle1 < 90 && pAngle2 > 270)
//			{
//				return pAngle1 + (360 - pAngle2);
//			}
//			else if(pAngle2 < 90 && pAngle1 > 270)
//			{
//				return pAngle2 + (360 - pAngle1);
//			}
//
//			else if(pAngle1 > pAngle2)
//			{
//				float diff = pAngle1 - pAngle2;
//				if(diff > 180)
//				{
//					return 360 - diff;
//				}
//				else
//				{
//					return diff;
//				}
//			}
//			else if(pAngle1 < pAngle2)
//			{
//				float diff = pAngle2 - pAngle1;
//				if(diff > 180)
//				{
//					return 360 - diff;
//				}
//				else
//				{
//					return diff;
//				}
//			}
//			else
//			{
//				return 0;
//			}
//		}
		#endregion
	}
}
