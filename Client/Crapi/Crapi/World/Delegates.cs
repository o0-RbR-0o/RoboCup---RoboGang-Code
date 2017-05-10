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

namespace TeamYaffa.CRaPI.World
{
	/// <summary>Handling event fired when Flags have been parsed</summary>
	public delegate void FlagsEventHandler(object sender, FlagsEventArgs e);
	
	/// <summary>Handling event fired when Intersections have been calculated</summary>
	public delegate void IntersectionsEventHandler(object sender, IntersectionsEventArgs e);
	
	/// <summary>Handling event fired when player position has been calculated</summary>
	public delegate void PositionEventHandler(object sender, PositionEventArgs e);
	
	/// <summary>Handling event fired when team mates have been updated</summary>
	public delegate void TeamMatesEventHandler(object sender, FieldPlayerEventArgs e);
	
	/// <summary>Handling event fired when opponents have been updated</summary>
	public delegate void OpponentsEventHandler(object sender, FieldPlayerEventArgs e);
	
	/// <summary>Handling event fired Player has been updated</summary>
	public delegate void PlayerCalculatedEventHandler(object sender, FieldPlayerEventArgs e);

	/// <summary>Handling event fired when position of one self is unknown</summary>
	public delegate void PositionUnknownEventHandler(object sender, EventArgs e);
}