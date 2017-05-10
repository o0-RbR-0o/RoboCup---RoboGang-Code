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

namespace TeamYaffa.CRaPI.World
{
	/// <summary>
	/// Event arguments for Flags
	/// </summary>
	/// <remarks>Used when an array of <see cref="Flag">flags</see> should be provided
	/// with the event.</remarks>
	public class FlagsEventArgs : EventArgs
	{
		/// <summary>The array of flags</summary>
		private Flag[] mFlags;
		/// <summary>The server cycle</summary>
		private int mServerCycle;

		/// <summary>
		/// Creates an FlagsEventArgs with an array of flags and a server cycle.
		/// </summary>
		/// <param name="pFlags">The array of flags</param>
		/// <param name="pServerCycle">The server cycle</param>
		public FlagsEventArgs(Flag[] pFlags, int pServerCycle)
		{
			mFlags = pFlags;
			mServerCycle = pServerCycle;
		}

		/// <summary>Gets the server cycle</summary>
		public int ServerCycle{ get { return mServerCycle; }}
		/// <summary>Gets the array of flags</summary>
		public Flag[] Flags { get { return mFlags; }}
	}

	/// <summary>
	/// Event arguments for Intersections
	/// </summary>
	/// <remarks>Used when an array of <see cref="Intersection">intersections</see> should be provided
	/// with the event.</remarks>
	public class IntersectionsEventArgs : EventArgs
	{
		/// <summary>The array of intersections</summary>
		private Intersection[] mIntersections;
		/// <summary>The server cycle</summary>
		private int mServerCycle;

		/// <summary>
		/// Creates an IntersectionsEventArgs with an array of intersections and a server cycle.
		/// </summary>
		/// <param name="pIntersections">The array of intersections</param>
		/// <param name="pServerCycle">The server cycle</param>
		public IntersectionsEventArgs(Intersection[] pIntersections, int pServerCycle)
		{
			mIntersections = pIntersections;
			mServerCycle = pServerCycle;
		}

		/// <summary>Gets the server cycle</summary>
		public int ServerCycle{ get { return mServerCycle; }}
		/// <summary>Gets the array of intersections</summary>
		public Intersection[] Intersections { get { return mIntersections; }}
	}

	/// <summary>
	/// Event arguments Positions
	/// </summary>
	/// <remarks>Used when a <see cref="TeamYaffa.CRaPI.Utility.Point2D">position</see> should be provided
	/// with the event.</remarks>
	public class PositionEventArgs : EventArgs
	{
		/// <summary>The position</summary>
		private Point2D mPosition;
		/// <summary>The server cycle</summary>
		private int mServerCycle;

		/// <summary>
		/// Creates an PositionEventArgs with a position and a server cycle.
		/// </summary>
		/// <param name="pPosition">The position</param>
		/// <param name="pServerCycle">The server cycle</param>
		public PositionEventArgs(Point2D pPosition, int pServerCycle)
		{
			mPosition = pPosition;
			mServerCycle = pServerCycle;
		}

		/// <summary>Gets the server cycle</summary>
		public int ServerCycle{ get { return mServerCycle; }}
		/// <summary>Gets the position</summary>
		public Point2D Position { get { return mPosition; }}
	}

	/// <summary>
	/// Event arguments FieldPlayers
	/// </summary>
	/// <remarks>Used when an ArrayList of <see cref="TeamYaffa.CRaPI.World.GameObjects.FieldPlayer">field players</see>
	/// should be provided with the event.</remarks>
	public class FieldPlayerEventArgs : EventArgs
	{
		/// <summary>The server cycle</summary>
		private int mServerCycle;
		/// <summary>The ArrayList of field players</summary>
		private ArrayList mPlayers;

		/// <summary>
		/// Creates an FieldPlayerEventArgs with an ArrayList of <see cref="TeamYaffa.CRaPI.World.GameObjects.FieldPlayer">field players</see>
		/// and a server cycle.
		/// </summary>
		/// <param name="pPlayers">The players</param>
		/// <param name="pServerCycle">The server cycle</param>
		public FieldPlayerEventArgs(ArrayList pPlayers, int pServerCycle)
		{
			mServerCycle = pServerCycle;
			mPlayers = pPlayers;
		}

		/// <summary>Gets the server cycle</summary>
		public int ServerCycle{ get { return mServerCycle; }}
		/// <summary>Gets the field players</summary>
		public ArrayList Players{ get {return mPlayers; }}
	}
}