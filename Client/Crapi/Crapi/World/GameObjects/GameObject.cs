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

namespace TeamYaffa.CRaPI.World.GameObjects
{
	/// <summary>
	/// This class represents a GameObject.
	/// </summary>
	public class GameObject
	{
		#region Members and Constructors

		/// <summary>Distance to the GameObject</summary>
		private double mDistance;
		/// <summary>Direction to the GameObject</summary>
		private double mDirection;
		/// <summary>The name of the GameObject</summary>
		private String mName;

		/// <summary>
		/// Constructs a GameObject
		/// </summary>
		public GameObject()
		{
			mName = "";
		}

		/// <summary>
		/// Constructs a GameObject
		/// </summary>
		/// <param name="pDistance">Distance to the GameObject</param>
		/// <param name="pDirection">Direction to the GameObject</param>
		/// <param name="pName">The name</param>
		public GameObject(double pDistance, double pDirection, string pName)
		{
			mDistance = pDistance;
			mDirection = pDirection;
			mName = pName;
		}

		#endregion

		#region Properties

		/// <summary>Distance to the GameObject</summary>
		public double Distance
		{
			get { return mDistance; }
			set { mDistance = value; }
		}

		/// <summary>Direction to the GameObject</summary>
		public double Direction
		{
			get { return mDirection; }
			set { mDirection = value; }
		}

		/// <summary>The name</summary>
		public String Name
		{
			get { return mName; }
			set { mName = value; }
		}

		#endregion

		#region Misc. operations
		/// <summary>
		/// Returns a string representation of the GameObject
		/// </summary>
		/// <returns>string representation</returns>
		public override String ToString() 
		{
			return "(Distance: " + mDistance + ", Direction: " + mDirection + ", Name: " + mName + ")";
		}
		#endregion
	}
}
