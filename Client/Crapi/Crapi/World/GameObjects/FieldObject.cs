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

namespace TeamYaffa.CRaPI.World.GameObjects
{
	/// <summary>
	/// This class represents a FieldObject.
	/// </summary>
	public class FieldObject : GameObject
	{
		#region Members and Constructors

		/// <summary>The position of the FieldObject</summary>
		private Point2D mPosition;

		/// <summary>
		/// Constructs a FieldObject
		/// </summary>
		public FieldObject()
		{
			mPosition = new Point2D();
		}

		/// <summary>
		/// Constructs a FieldObject with a designated position
		/// </summary>
		/// <param name="pPosition">The position</param>
		public FieldObject(Point2D pPosition)
		{
			mPosition = pPosition;
		}

		/// <summary>
		/// Constructs a FieldObject with a designated position, distance, direction and name
		/// </summary>
		/// <param name="pPosition">The position</param>
		/// <param name="pDistance">The distance</param>
		/// <param name="pDirection">The direction</param>
		/// <param name="pName">The name</param>
		public FieldObject(Point2D pPosition, double pDistance, double pDirection, string pName)
			: base(pDistance, pDirection, pName)
		{
			mPosition = pPosition;
		}

		#endregion

		#region Properties

		/// <summary>The position of the FieldObject</summary>
		public Point2D Position
		{
			get { return mPosition; }
			set { mPosition = value; }
		}

		#endregion

		#region Misc. operations

		/// <summary>
		/// Returns a string representation of the FieldObject
		/// </summary>
		/// <returns>string representation</returns>
		public override String ToString() 
		{
			return "(Position: " + mPosition + ", " + base.ToString() + ")";
		}

		#endregion
	}
}
