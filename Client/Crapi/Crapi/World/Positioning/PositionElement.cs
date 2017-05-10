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

namespace TeamYaffa.CRaPI.World.Positioning
{
	/// <summary>Represent an element in the positioning algorithm.
	/// <seealso cref="Positioning"/></summary>
	/// <remarks>It contains a <see cref="CenterPoint"/>, which is the point in the center of
	/// the element where an <see cref="Intersection">intersection</see> of two observed flags has
	/// been calculated to be.</remarks>
	internal class PositionElement : IComparable
	{
		#region Members and Constructor
		/// <summary>The point in the center of the element</summary>
		private Point2D mCenterPoint;
		/// <summary>The number of times this element was "hit".</summary>
		private int mCount;
		/// <summary>The relative importance of this element</summary>
		private double mWeight;

		/// <summary>Creates a PositionElement.</summary>
		/// <remarks>A centerpoint should be provided when creating a PositionElement. Default
		/// <see cref="Count"/> is 1.</remarks>
		/// <param name="pCenterPoint">The center of this element</param>
		/// <param name="pWeight">The weight of this element</param>
		internal PositionElement(Point2D pCenterPoint, double pWeight)
		{
			mWeight = pWeight;
			mCenterPoint = pCenterPoint;
			mCount = 1;
		}
		
		#endregion

		#region Misc methods
		/// <summary>Increases the <see cref="Count"/> with 1.</summary>
		internal void Increase()
		{
			mCount++;
		}

		/// <summary>
		/// Creates a string representation of the PositionElement
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return mCenterPoint.ToString() + " : " + mCount;
		}
		#endregion

		#region Properties
		/// <summary>Gets how many times this element as been "hit".</summary>
		internal int Count
		{
			get { return mCount; }
		}

		/// <summary>Gets the point of the center of this element.</summary>
		internal Point2D CenterPoint
		{
			get { return mCenterPoint; }
		}

		/// <summary>Gets the weight of this element</summary>
		/// <remarks>PositionElements are weighted on their approximate validity</remarks>
		internal double Weight
		{
			get{return mWeight;}
			set{mWeight = value;}
		}

		/// <summary>The value of this element</summary>
		internal double Value
		{
			get{return (mCount * mWeight);}
		}
		#endregion

		#region IComparable implementation
		/// <summary>
		/// Compares this PositionElement to a another object
		/// </summary>
		/// <param name="pPositionElement">An object</param>
		/// <returns>0 if equal : 1 if this element is "better" than the other element 
		/// : -1 if this element is "worse" than the other element </returns>
		int IComparable.CompareTo(object pPositionElement)
		{
			if(pPositionElement is PositionElement)
			{
				if(Value < ((PositionElement)pPositionElement).Value)
					return -1;
				if(Value > ((PositionElement)pPositionElement).Value)
					return 1;
				else
					return 0;
			}
			else
				throw new ArgumentException("The parameter is not a flag", "pPositionElement");
		}

		/// <summary>
		/// Compares this PositionElement to a another PositionElement
		/// </summary>
		/// <param name="pPositionElement">a PositionElement</param>
		/// <returns>0 if equal : 1 if this element is "better" than the other element 
		/// : -1 if this element is "worse" than the other element</returns>
		internal int CompareTo(PositionElement pPositionElement)
		{
			if(Value < pPositionElement.Value)
				return -1;
			if(Value > pPositionElement.Value)
				return 1;
			else
				return 0;
		}
		#endregion
	}
}
