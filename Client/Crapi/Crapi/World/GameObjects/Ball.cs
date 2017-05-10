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

namespace TeamYaffa.CRaPI.World.GameObjects
{
	/// <summary>
	/// This class represents a Ball.
	/// </summary>
	public class Ball : MobileObject
	{	
		#region Misc. operations

		/// <summary>
		/// Returns a string representation of the ball.
		/// </summary>
		/// <returns>string representation</returns>
		public override String ToString() 
		{
			return "(Ball " + base.ToString() + ")";
		}

		#endregion
	}
}