using System;

namespace TeamYaffa.CRaPI.World.GameObjects
{
	/// <summary>
	/// A model of objects that are seen on the playfield, but where only the
	/// direction to the object is known.
	/// </summary>
	public class VagueObject
	{
		/// <summary>The type of object, i.e. player, ball or flag</summary>
		public VagueType mType;
		/// <summary>Direction to the GameObject</summary>
		private double mDirection;
		/// <summary>The server cycle in which this object was last seen</summary>
		private int mLastSeen;

		/// <summary>
		/// Creates the VagueObject with a type, direction and what cycle it was seen.
		/// </summary>
		/// <param name="pType">What type of object that is seen.</param>
		/// <param name="pDirection">In what direction the object was seen.</param>
		/// <param name="pCycle">What cycle it was seen.</param>
		public VagueObject(VagueType pType, double pDirection, int pCycle)
		{
			mType = pType;
			mDirection = pDirection;
			mLastSeen = pCycle;
		}

		/// <summary>The type of the vague object</summary>
		public VagueType Type
		{
			get{ return mType; }
		}

		/// <summary>The direction to the object</summary>
		public double Direction
		{
			get{ return mDirection; }
		}

		/// <summary>What cycle the object was seen</summary>
		public int LastSeen
		{
			get{ return mLastSeen; }
		}
	}

	/// <summary>
	/// What different VagueObjects CRaPI can handle.
	/// </summary>
	public enum VagueType
	{
		/// <summary>A player</summary>
		player,
		/// <summary>The ball</summary>
		ball,
		/// <summary>A flag</summary>
		flag
	}
}
