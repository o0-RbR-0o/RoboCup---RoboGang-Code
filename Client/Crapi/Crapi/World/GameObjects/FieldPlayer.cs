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
	/// This class represents a FieldPlayer.
	/// </summary>
	public class FieldPlayer : MobileObject
	{
		#region Members and Constructors
		
		/// <summary>The number</summary>
		private int mUniformNumber;
		/// <summary>The team name</summary>
		private string mTeamName;
		/// <summary>Direction of the body</summary>
		private double mBodyDirection;
		/// <summary>Direction of the face</summary>
		private double mFaceDirection;
		/// <summary>If this player is a goalie or not</summary>
		private bool mGoalie;
		/// <summary>The direction of the body relative the opponent goal</summary>
		private double mAbsoluteBodyDirection;
		/// <summary>The direction of the face relative the opponent goal (absolute body direction + head angle)</summary>
		private double mAbsoluteFaceDirection;
		/// <summary>The angle of the head</summary>
		private double mHeadAngle;

		/// <summary>
		/// Default constructor
		/// </summary>
		public FieldPlayer()
		{
			mUniformNumber = -1;
		}

		/// <summary>
		/// Constructs a FieldPlayer
		/// </summary>
		/// <param name="pUniformNumber">The number</param>
		public FieldPlayer(int pUniformNumber)
		{
			mUniformNumber = pUniformNumber;
		}

		/// <summary>
		/// Constructs a FieldPlayer
		/// </summary>
		/// <param name="pUniformNumber">The number</param>
		/// <param name="pGoalie">Toggle if this player is a goalie</param>
		public FieldPlayer(int pUniformNumber, bool pGoalie)
		{
			mUniformNumber = pUniformNumber;
			mGoalie = pGoalie;
		}

		/// <summary>
		/// Consructs a FieldPlayer
		/// </summary>
		/// <param name="pTeamName">The team name</param>
		/// <param name="pUniformNumber">The number</param>
		/// <param name="pGoalie">Toggle if this player is a goalie</param>
		/// <param name="pDirectionChange">The change in direction</param>
		/// <param name="pDistanceChange">The change in distance</param>
		/// <param name="pLastSeen">When this player was last seen</param>
		/// <param name="pPosition">The position</param>
		public FieldPlayer(string pTeamName, int pUniformNumber, bool pGoalie, double pDirectionChange, double pDistanceChange, int pLastSeen, Point2D pPosition)
			: base(pDirectionChange, pDistanceChange, pLastSeen, pPosition)
		{
			mTeamName = pTeamName;
			mUniformNumber = pUniformNumber;
			mGoalie = pGoalie;
		}

		/// <summary>
		/// Sets the data of the FieldPlayer
		/// </summary>
		/// <remarks>Used in some cases to not have to construct a new FieldPlayer</remarks>
		/// <param name="pFP">The FieldPlayer to copy data from</param>
		public void SetData(FieldPlayer pFP)
		{
			mTeamName = pFP.TeamName;
			mUniformNumber = pFP.UniformNumber;
			mGoalie = pFP.Goalie;
			DirectionChange = pFP.DirectionChange;
			DistanceChange = pFP.DistanceChange;
			AbsoluteBodyDirection = pFP.AbsoluteBodyDirection;
			AbsoluteFaceDirection = pFP.AbsoluteFaceDirection;
			BodyDirection = pFP.BodyDirection;
			FaceDirection = pFP.FaceDirection;
			LastSeen = pFP.LastSeen;
			Position = pFP.Position;
			Name = pFP.Name;
			Direction = pFP.Direction;
			Distance = pFP.Distance;
		}

		#endregion

		#region Properties
		
		/// <summary>The team name</summary>
		public string TeamName
		{
			get{return mTeamName;}
			set{mTeamName = value;}
		}

		/// <summary>The number</summary>
		public int UniformNumber
		{
			get { return mUniformNumber; }
			set { mUniformNumber = value; }
		}

		/// <summary>Direction of the body</summary>
		public double BodyDirection
		{
			get { return mBodyDirection; }
			set { mBodyDirection = value; }
		}

			/// <summary>Direction of the face</summary>
		public double FaceDirection
		{
			get { return mFaceDirection; }
			set { mFaceDirection = value; }
		}

		/// <summary>If this player is a goalie or not</summary>
		public bool Goalie
		{
			get { return mGoalie; }
			set { mGoalie = value; }
		}

		/// <summary>The direction of the body relative the opponent goal</summary>
		public double AbsoluteBodyDirection
		{
			get{return mAbsoluteBodyDirection;}
			set{mAbsoluteBodyDirection = value;}
		}

		/// <summary>The direction of the face relative the opponent goal (absolute body direction + head angle)</summary>
		public double AbsoluteFaceDirection
		{
			get{return mAbsoluteFaceDirection;}
			set{mAbsoluteFaceDirection = value;}
		}

		/// <summary>The angle of the head</summary>
		public double HeadAngle
		{
			get{return mHeadAngle;}
			set{mHeadAngle = value;}
		}

		#endregion

		#region Misc. operations

		/// <summary>
		/// Returns a string representation of the FieldPlayer
		/// </summary>
		/// <returns>string representation</returns>
		public override String ToString() 
		{
			return "(Player " + base.ToString() + ", BodyDir: " + mBodyDirection + ", FaceDir: " +
				mFaceDirection + ", Goalie: " + mGoalie + ", Number: " + mUniformNumber + ")";
		}

		#endregion
	}
}
