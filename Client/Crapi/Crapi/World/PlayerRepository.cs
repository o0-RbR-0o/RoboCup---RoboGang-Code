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
using TeamYaffa.CRaPI.World.GameObjects;

namespace TeamYaffa.CRaPI.World
{
	/// <summary>
	/// This class acts as a repository for FieldPlayers, known, unknown and assumed
	/// </summary>
	public class PlayerRepository
	{
		#region Members and constructs
		/// <summary>The team name of the owner of the repository</summary>
		private string mTeamName;
		/// <summary>The team mates seen this cycle</summary>
		private ArrayList mSeenMates;
		/// <summary>All team mates</summary>
		private RepositoryElement[] mTeamMates;
		/// <summary>The opponents seen this cycle</summary>
		private ArrayList mSeenOpponents;
		/// <summary>All opponents</summary>
		private RepositoryElement[] mOpponents;
		/// <summary>The players whose team name cannot be seen</summary>
		private ArrayList mUnknownPlayers;


		/// <summary>
		/// Constructs a repository of FieldPlayers
		/// </summary>
		/// <param name="pTeamName">The name of my team</param>
		internal PlayerRepository(string pTeamName)
		{
			mTeamName = pTeamName;
			mSeenMates = new ArrayList();
			mTeamMates = new RepositoryElement[12];
			mSeenOpponents = new ArrayList();
			mOpponents = new RepositoryElement[12];
			mUnknownPlayers = new ArrayList();

			//from 1 through 11, i.e. all uniform numbers
			for(int i = 1; i < 12; i++)
			{
				//Mates
				FieldPlayer fp1 = new FieldPlayer();
				fp1.UniformNumber = i;
				fp1.Position = new Point2D(0,0);
				mTeamMates[i] = new RepositoryElement(fp1);

				//Opponents
				FieldPlayer fp2 = new FieldPlayer();
				fp2.UniformNumber = i;
				fp2.Position = new Point2D(0,0);
				mOpponents[i] = new RepositoryElement(fp2);
			}
		}
		#endregion

		#region Update
		/// <summary>
		/// Updates the data for the team mates
		/// </summary>
		/// <remarks><list type="number">
		/// <item>Mark all players as available (not taken)</item>
		/// <item>Go through all players with uniform number seen</item>
		/// <item>Loop all seen team mates</item>
		/// <item>For each player without number, figure out what it ought to be</item>
		/// <item>If best guess is more than 10 meters away, keep uniform number unknown</item>
		/// <item>Else set uniform number and the element as taken.</item>
		/// <item>End loop</item>
		/// </list></remarks>
		/// <param name="pMates">ArrayList of seen team mates</param>
		internal void UpdateMates(ArrayList pMates)
		{
			mSeenMates = pMates;

			//Mark all players as available (not taken)
			for(int i = 1; i < 12; i++)
			{
				mTeamMates[i].Taken = false;
			}
			
			//go through all players with uniform number seen
			foreach(FieldPlayer seen in mSeenMates)
			{
				if(seen.UniformNumber > 0)
				{
					mTeamMates[seen.UniformNumber].Taken = true;
					mTeamMates[seen.UniformNumber].FieldPlayer.SetData(seen);
				}
			}

			//loop all seen team mates
			foreach(FieldPlayer seen in mSeenMates)
			{
				//for each player without number, figure out what it ought to be
				if(seen.UniformNumber < 1)
				{
					double distance = double.MaxValue;
					RepositoryElement bestGuess = null;
					for(int i = 1; i < 12; i++)
					{
						RepositoryElement mate = mTeamMates[i];

						if(mate.Taken)
							continue;

						double temp = mate.FieldPlayer.Position - seen.Position;
						if(temp < distance)
						{
							distance = temp;
							bestGuess = mate;
						}
					}

					//if best guess is more than 10 meters away, keep uniform number unknown
					if(distance > 10)
					{
						seen.UniformNumber = 0;
					}
					//else set uniform number and the element as taken.
					else
					{
						seen.UniformNumber = bestGuess.FieldPlayer.UniformNumber;
						bestGuess.FieldPlayer.SetData(seen);
						bestGuess.Taken = true;
					}
				}
			}
		}

		/// <summary>
		/// Updates the data for the opponents
		/// </summary>
		/// <remarks><list type="number">
		/// <item>Mark all players as available (not taken)</item>
		/// <item>Go through all players with uniform number seen</item>
		/// <item>Loop all seen opponents</item>
		/// <item>For each player without number, figure out what it ought to be</item>
		/// <item>If best guess is more than 10 meters away, keep uniform number unknown</item>
		/// <item>Else set uniform number and the element as taken.</item>
		/// <item>End loop</item>
		/// </list></remarks>
		/// <param name="pOpponents">An ArrayList of seen opponents</param>
		internal void UpdateOpponents(ArrayList pOpponents)
		{
			mSeenOpponents = pOpponents;
			
			//Mark all players as available (not taken)
			for(int i = 1; i < 12; i++)
			{
				mOpponents[i].Taken = false;
			}
			
			//go through all players with uniform number seen
			foreach(FieldPlayer seen in mSeenOpponents)
			{
				if(seen.UniformNumber > 0)
				{
					mOpponents[seen.UniformNumber].Taken = true;;
					mOpponents[seen.UniformNumber].FieldPlayer.SetData(seen);
				}
			}

			//loop all seen opponents
			foreach(FieldPlayer seen in mSeenOpponents)
			{
				//for each player without number, figure out what it ought to be
				if(seen.UniformNumber < 1)
				{
					double distance = double.MaxValue;
					RepositoryElement bestGuess = null;
					for(int i = 1; i < 12; i++)
					{
						RepositoryElement opponent = mOpponents[i];

						if(opponent.Taken)
							continue;

						double temp = opponent.FieldPlayer.Position - seen.Position;
						if(temp < distance)
						{
							distance = temp;
							bestGuess = opponent;
						}
					}

					//if best guess is more than 10 meters away, keep uniform number unknown
					if(distance > 10)
					{
						seen.UniformNumber = 0;
					}
					//else set uniform number and the element as taken.
					else
					{	
						seen.UniformNumber = bestGuess.FieldPlayer.UniformNumber;
						bestGuess.FieldPlayer.SetData(seen);
						bestGuess.Taken = true;
					}
				}
			}
		}

		#endregion

		#region Properties
		/// <summary>
		/// Removes all the unknown players from the repository
		/// </summary>
		internal void ClearUnknownPlayers()
		{
			mUnknownPlayers.Clear();
		}

		/// <summary>
		/// Adds an unknown player to the repository
		/// </summary>
		/// <param name="pFP">The player to add.</param>
		internal void AddUnknownPlayer(FieldPlayer pFP)
		{
			mUnknownPlayers.Add(pFP);
		}

		/// <summary>
		/// The unknown players
		/// </summary>
		internal ArrayList UnknownPlayers
		{
			get{ return mUnknownPlayers; }
		}

		/// <summary>
		/// The team mates
		/// </summary>
		internal ArrayList TeamMates
		{
			get{ return mSeenMates; }
		}

		/// <summary>
		/// The opponents
		/// </summary>
		internal ArrayList Opponents
		{
			get{ return mSeenOpponents; }
		}

		internal FieldPlayer TeamMate(int pUniform)
		{
			foreach(FieldPlayer mate in TeamMates)
			{
				if(mate.UniformNumber == pUniform)
					return mate;
			}

			return null;
		}

		internal FieldPlayer Opponent(int pUniform)
		{
			foreach(FieldPlayer opponent in Opponents)
			{
				if(opponent.UniformNumber == pUniform)
					return opponent;
			}

			return null;
		}

		internal FieldPlayer[] AllMates
		{
			get
			{
				FieldPlayer[] fpArr = new FieldPlayer[mTeamMates.Length-1];
				for(int i = 1; i < mTeamMates.Length; i++)
				{
					fpArr[i-1] = mTeamMates[i].FieldPlayer;
				}
				return fpArr;
			}
		}

		internal FieldPlayer[] AllOpponents
		{
			get
			{
				FieldPlayer[] fpArr = new FieldPlayer[mOpponents.Length-1];
				for(int i = 1; i < mOpponents.Length; i++)
				{
					fpArr[i-1] = mOpponents[i].FieldPlayer;
				}
				return fpArr;
			}
		}
		#endregion
	}
}
