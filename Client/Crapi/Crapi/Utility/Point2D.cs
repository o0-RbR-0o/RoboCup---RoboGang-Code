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
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Xml.Serialization;

namespace TeamYaffa.CRaPI.Utility
{
	/// <summary>This class represents a point of double precision on a 2 dimensional plane.</summary>
	[TypeConverter(typeof(Point2DConverter))]
	public struct Point2D
	{
		#region Members and constructors

		/// <summary>X-coordinate</summary>
		private double mX;
		/// <summary>Y-coordinate</summary>
		private double mY;

		/// <summary>Constructs a Point2D of two doubles</summary>
		/// <param name="pX">The X position</param>
		/// <param name="pY">The Y position</param>
		public Point2D(double pX, double pY)
		{
			mX = pX;
			mY = pY;
		}

		/// <summary>
		/// Constructs a Point2D of a Point2D and a direction and distance from that point
		/// </summary>
		/// <param name="pPosition">The starting point</param>
		/// <param name="pDirection">The direction of the new point in relation to the starting point</param>
		/// <param name="pDistance">The distance to the new point in relation to the starting point</param>
		public Point2D(Point2D pPosition, double pDirection, double pDistance)
		{
			double direction = MathUtility.ToRadians(pDirection);
			mX = pPosition.X + pDistance * Math.Cos(direction);
			mY = pPosition.Y + pDistance * Math.Sin(direction);
		}

		/// <summary>
		/// Creates a new Point2D from an existing Point2D
		/// </summary>
		/// <param name="pPosition">The point to copy</param>
		public Point2D(Point2D pPosition)
		{
			mX = pPosition.X;
			mY = pPosition.Y;
		}

		#endregion

		#region Geometrical operations
		/// <summary>Checks if two Point2D are equal</summary>
		/// <remarks>Two points are equal if (P1.X == P2.X) and (P1.Y == P2.Y)</remarks>
		/// <param name="pP1">Point 1</param>
		/// <param name="pP2">Point 2</param>
		/// <returns>true if the points are equal</returns>
		public static bool operator ==(Point2D pP1, Point2D pP2)
		{
			if((pP1.X == pP2.X) && (pP1.Y == pP2.Y))
				return true;

			return false;
		}

		/// <summary>Checks if two Point2D are inequal<seealso cref="operator=="/></summary>
		/// <param name="pP1">Point 1</param>
		/// <param name="pP2">Point 2</param>
		/// <returns>true if not equal</returns>
		public static bool operator !=(Point2D pP1, Point2D pP2)
		{
			return !(pP1 == pP2);
		}

		/// <summary>
		/// Calculates the distance between two points <seealso cref="Distance"/>
		/// </summary>
		/// <remarks>The distance between the two points are calculated with
		/// <code>dist = Sqrt(xDist^2 + yDist^2)</code>
		/// where <c>xDist</c> is the X-difference and <c>yDist</c> is the Y-difference for the
		/// two points.
		/// </remarks>
		/// <param name="pP1">Point 1</param>
		/// <param name="pP2">Point 2</param>
		/// <returns>The distance between the two points.</returns>
		public static double operator -(Point2D pP1, Point2D pP2)
		{
			double xDist = pP2.X - pP1.X;
			double yDist = pP2.Y - pP1.Y;
			return Math.Sqrt(Math.Pow(xDist, 2) + Math.Pow(yDist, 2));
		}

		/// <summary>
		/// Calculates the distance between two points <seealso cref="operator-"/>
		/// </summary>
		/// <remarks>The distance between the two points are calculated with
		/// <code>dist = Sqrt(xDist^2 + yDist^2)</code>
		/// where <c>xDist</c> is the X-difference and <c>yDist</c> is the Y-difference for the
		/// two points.
		/// </remarks>
		/// <param name="pP1">Point 1</param>
		/// <param name="pP2">Point 2</param>
		/// <returns>The distance between the two points.</returns>
		public static double Distance(Point2D pP1, Point2D pP2)
		{
			return pP1-pP2;
		}

		/// <summary>The absolute value of the x distance to a point</summary>
		/// <param name="pPoint">The point</param>
		/// <returns>The distance</returns>
		public double XDistanceTo(Point2D pPoint)
		{
			return Math.Abs(pPoint.X - mX);
		}

		/// <summary>The absolute value of the y distance to a point</summary>
		/// <param name="pPoint">The point</param>
		/// <returns>The distance</returns>
		public double YDistanceTo(Point2D pPoint)
		{
			return Math.Abs(pPoint.Y - mY);
		}

		/// <summary>The x distance to a point</summary>
		/// <param name="pPoint">The point</param>
		/// <returns>The distance</returns>
		public double SignedXDistanceTo(Point2D pPoint)
		{
			return pPoint.X - mX;
		}

		/// <summary>The y distance to a point</summary>
		/// <param name="pPoint">The point</param>
		/// <returns>The distance</returns>
		public double SignedYDistanceTo(Point2D pPoint)
		{
			return pPoint.Y - mY;
		}

		/// <summary>The angle from this point to another point</summary>
		/// <param name="pPoint">The point to calculate angle to</param>
		/// <returns>The angle to the point</returns>
		public double AngleTo(Point2D pPoint)
		{
			double xDist = SignedXDistanceTo(pPoint);
			double yDist = SignedYDistanceTo(pPoint);
			double angle = MathUtility.ToDegrees(Math.Atan(yDist/xDist));

			if(xDist < 0)
			{
				if(yDist < 0)
				{
					angle -= 180;
				}
				else
				{
					angle += 180;
				}
			}
			return MathUtility.NormalizeAngle(angle);
		}

		/// <summary>Checks if this point have the same coordinate as a second point
		/// <seealso cref="operator=="/></summary>
		/// <param name="o">The second point</param>
		/// <returns>true if equal, otherwise false</returns>
		public override bool Equals(Object o)
		{
			if(o is Point2D)
				return this == (Point2D)o;
			return false;
		}

		/// <summary>The hash code of this object</summary>
		/// <remarks>Calculated by:
		/// <code>
		/// <see cref="X"/> XOR (<see cref="Y"/> (left shift operator) 1)
		/// </code></remarks>
		/// <returns>The hash code of this object</returns>
		public override int GetHashCode()
		{
			return mX.GetHashCode() | (mY.GetHashCode() << 1);
		}

		/// <summary>
		/// Calculates the position on the middle of the imagined line
		/// between two points.
		/// </summary>
		/// <param name="pPoint">The other point</param>
		/// <returns>A point representing the position on the middle of the imagined line
		/// between the two points.</returns>
		public Point2D MidwayTo(Point2D pPoint)
		{
			return new Point2D(this, this.AngleTo(pPoint), (this - pPoint) / 2);
		}

		#endregion

		#region Misc. operations
		/// <summary>Sets the location of this Point2D to the location supplied.</summary>
		/// <param name="pX">The X position of the new location</param>
		/// <param name="pY">The Y position of the new location</param>
		public void SetLocation(double pX, double pY)
		{
			mX = pX;
			mY = pY;
		}

		/// <summary>A string representation of the point.</summary>
		/// <remarks>The X and Y values are round to precision of 4.</remarks>
		/// <returns>X; Y</returns>
		public override string ToString()
		{
			return String.Format("{0}; {1}", Math.Round(mX, 4), Math.Round(mY, 4));
		}

		/// <summary>
		/// Creates a <see cref="System.Drawing.Point">Point</see> from this Point2D.
		/// </summary>
		/// <returns>A <see cref="System.Drawing.Point">Point</see> based on this
		/// Point2D where the x- and y-value is rounded off.</returns>
		public Point ToPoint()
		{
			return new Point((int)this.mX, (int)this.mY);
		}
		#endregion

		#region Properties
		/// <summary>The X-coordinate of the point</summary>
		/// <value><see cref="mX"/></value>
		[XmlAttribute]
		public double X
		{
			get { return mX; }
			set { mX = value; }
		}

		/// <summary>The Y-coordinate of the point</summary>
		/// <value><see cref="mY"/></value>
		[XmlAttribute]
		public double Y
		{
			get { return mY; }
			set { mY = value; }
		}
		#endregion
	}

	#region Internal TypeConverter-class
	/// <summary>
	/// This class used to convert a <see cref="Point2D">Point2D</see> so it can be used in a property table.
	/// </summary>
	/// <remarks>The string format of the point must be "x; y". The formatter works both ways: string -> Point2D
	/// and Point2D -> string</remarks>
	internal class Point2DConverter : ExpandableObjectConverter
	{
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type t)
		{
			if (t == typeof(string))
			{
				return true;
			}
			return base.CanConvertFrom(context, t);
		}

		/// <summary>
		/// Converts a string to a Point2D.
		/// </summary>
		/// <remarks>This method is called from the Property table when a conversion is needed.</remarks>
		/// <param name="context">Not used in this method.</param>
		/// <param name="info">Not used in this method.</param>
		/// <param name="value">The value that should be converted. If it is not a string, base.ConvertFrom(...) will
		/// be called.</param>
		/// <returns>The Point2D that is represented by the string in <paramref name="value">value</paramref>.</returns>
		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo info, object value)
		{
			if (value is string)
			{
				try
				{
					string s = (string) value;
					// parse the format "X; Y"
					int semicolon = s.IndexOf(';');
					if (semicolon != -1)
					{
						// now that we have the semicolon, get
						// the x-position and y-position.
						double xPos = double.Parse(s.Substring(0, semicolon).Trim());
						double yPos = double.Parse(s.Substring(semicolon + 1, s.Length - (semicolon + 1)));
						Point2D p = new Point2D(xPos, yPos);
						return p;
					}
				}
				catch {}
				// if we got this far, complain that we
				// couldn't parse the string
				throw new ArgumentException("Can not convert '" + (string)value + "' to type Point2D");
			}
			return base.ConvertFrom(context, info, value);
		}

		/// <summary>
		/// Converts a Point2D to a string.
		/// </summary>
		/// <remarks>It checks if <paramref name="destType"/> is a string, and <paramref name="value"/> is
		/// a Point2D.</remarks>
		/// <param name="context">Not used in this method.</param>
		/// <param name="culture">Not used in this method.</param>
		/// <param name="value">The Point2D to convert.</param>
		/// <param name="destType">The type the result should result in.</param>
		/// <returns>A string on the form "x; y".</returns>
		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destType)
		{
			if (destType == typeof(string) && value is Point2D)
			{
				Point2D p = (Point2D)value;
				// simply build the string as "X; Y"
				// return p.X + "; " + p.Y;
				return value.ToString();
			}
			return base.ConvertTo(context, culture, value, destType);
		}
	}
	#endregion
}