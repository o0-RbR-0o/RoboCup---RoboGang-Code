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
using TeamYaffa.CRaPI.Info;
using TeamYaffa.CRaPI.World.GameObjects;

namespace TeamYaffa.CRaPI.World
{
	/// <summary>
	/// This class acts a wrapper of FieldPlayers with additional information for Repository handling
	/// </summary>
	internal class RepositoryElement
	{
		#region Members and constructs
		/// <summary>The wrapped FieldPlayer</summary>
		private FieldPlayer mFieldPlayer;
		/// <summary>If this element is taken or not</summary>
		private bool mTaken;

		/// <summary>
		/// Constructs a RepositoryElement
		/// </summary>
		/// <param name="pFieldPlayer">A FieldPlayer</param>
		internal RepositoryElement(FieldPlayer pFieldPlayer)
		{
			mFieldPlayer = pFieldPlayer;
			mTaken = false;
		}
		#endregion

		#region Properties
		/// <summary>
		/// The FieldPlayer connected to this RepositoryElement
		/// </summary>
		internal FieldPlayer FieldPlayer
		{
			get{return mFieldPlayer;}
			set{mFieldPlayer = value;}
		}

		/// <summary>
		/// Indicates if this RepositoryElement is already taken or not
		/// </summary>
		internal bool Taken
		{
			get{return mTaken;}
			set{mTaken = value;}
		}
		#endregion
	}
}
