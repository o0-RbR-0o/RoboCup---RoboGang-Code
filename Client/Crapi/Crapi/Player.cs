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
using TeamYaffa.CRaPI.Net;
using TeamYaffa.CRaPI.Utility;
using TeamYaffa.CRaPI.World;
using TeamYaffa.CRaPI.Info;

using System.Threading;

namespace TeamYaffa.CRaPI
{
	/// <summary>
	/// This class is the core of the CRaPI API and the main loop of a soccer player
	/// </summary>
	public class Player
	{
		#region Members and constructor
		/// <summary>The side [l|r]</summary>
		private Side mSide;
		/// <summary>The name of the team</summary>
		private string mTeamName;
		/// <summary>The number of the uniform</summary>
		private int mUniformNumber;
		/// <summary>The connection to the server</summary>
		private ServerConnection mConnection;
		/// <summary>If this Player is a goalie</summary>
		private bool mIsGoalie;
		/// <summary>The position on the field where this Player starts</summary>
		private Point2D mStartPoint;
		/// <summary>The WorldModel of the Player</summary>
		private WorldModel mWM;
		/// <summary>The last server error</summary>
		private string mLastError;
		/// <summary>The server parameters <see cref="TeamYaffa.CRaPI.Info.ServerParam"/></summary>
		private ServerParam mServerParam;
		/// <summary>The player parameters <see cref="TeamYaffa.CRaPI.Info.PlayerParam"/></summary>
		private PlayerParam mPlayerParam;
		/// <summary>The different types of players <see cref="TeamYaffa.CRaPI.Info.PlayerType"/></summary>
		private ArrayList mPlayerTypes;
		/// <summary>The current <see cref="TeamYaffa.CRaPI.PlayMode">PlayMode</see> of the game</summary>
		private PlayMode mPlayMode;
		/// <summary>The internal clock, to keep keep track of when it's time to send a reply to the server</summary>
		private Clock mClock;
		/// <summary>The current SenseBody</summary>
		private SenseBody mSenseBody;
		/// <summary>The previous SenseBody</summary>
		private SenseBody mPrevSenseBody;
		/// <summary>The current message from the server</summary>
		private string mCurrentServerMessage;
		/// <summary>The current queue of commands</summary>
		private CommandQueue mCommandQueue;
		/// <summary>The previous queue of commands</summary>
		private CommandQueue mPrevCommandQueue;
		/// <summary>The current score of my team</summary>
		private int mOwnScore;
		/// <summary>The current score of the opponent team</summary>
		private int mOpponentScore;

		/// <summary>Constructs a Player</summary>
		/// <remarks>The default <see cref="StartPoint"/> is set to (-20, 0).</remarks>
		/// <param name="pTeamName">Name of team</param>
		/// <param name="pIsGoalie">If player is goalie or not</param>
		public Player(string pTeamName, bool pIsGoalie)
		{
			mTeamName = pTeamName;
			mIsGoalie = pIsGoalie;
			mPlayerTypes = new ArrayList(7);
			mPlayMode = PlayMode.before_kick_off;
			mClock = new Clock();
			mCommandQueue = new CommandQueue();
			mPrevCommandQueue = new CommandQueue();
			mStartPoint = new Point2D(0,-33);
		}

		/// <summary>Constructs a Player</summary>
		/// <param name="pTeamName">Name of team</param>
		/// <param name="pIsGoalie">If player is goalie or not</param>
		/// <param name="pStartPosition">What position the player will start on when the game
		/// beginns or after goals.</param>
		public Player(string pTeamName, bool pIsGoalie, Point2D pStartPosition) : this(pTeamName, pIsGoalie)
		{
			mStartPoint = pStartPosition;
		}

		/// <summary>
		/// Connects to the server and receives <c>player_type</c>, <c>player_param</c>,
		/// <c>server_param</c> from the server.
		/// </summary>
		/// <remarks>This should be used instead of using manually <see cref="Connect"/>
		/// because this method won't return until all messages from the server that is 
		/// necessary for the player is received.</remarks>
		/// <param name="pHost">The url or ip of the host</param>
		/// <param name="pPort">The port to connect to on the host</param>
		/// <param name="pTimeOut">The time before a receive command time outs</param>
		/// <returns>true if the start-up was successful</returns>
		public bool StartUp(string pHost, int pPort, int pTimeOut)
		{
			if(!Connect(pHost, pPort, pTimeOut))
				return false;

			int timeOuts = 0;
			bool done = false;

			while(timeOuts < 3 && !done)
			{
				try
				{
					mCurrentServerMessage = mConnection.Receive();
					if(mCurrentServerMessage.StartsWith("(see"))
					{
						handleSee();
						done = true;
					}
					else if(mCurrentServerMessage.StartsWith("(sense_body"))
					{
						handleSenseBody();
						done = true;
					}
					else if(mCurrentServerMessage.StartsWith("(hear"))
					{
						handleHear();
						done = true;
					}
					else if(mCurrentServerMessage.StartsWith("(player_type"))
					{
						handlePlayerType();
					}
					else if(mCurrentServerMessage.StartsWith("(player_param"))
					{
						handlePlayerParam();
					}
					else if(mCurrentServerMessage.StartsWith("(server_param"))
					{
						handleServerParam();
					}
				}
				catch(System.Net.Sockets.SocketException se)
				{// The connection timed out.
					timeOuts++;
					Console.WriteLine(se.ToString());
					return false;
				}
			}
			return true;
		}
		#endregion

		#region Main-loop
		/// <summary>
		/// The Thread entry point and main loop for the player.
		/// </summary>
		public void PlayGame()
		{
			Thread.CurrentThread.Name = "Player " + mUniformNumber.ToString();
			int timeOuts = 0;
			
            while(timeOuts<3){
				try
				{
					mCurrentServerMessage = mConnection.Receive();
					if(mCurrentServerMessage.StartsWith("(see"))
					{
						handleSee();
					}
					else if(mCurrentServerMessage.StartsWith("(sense_body"))
					{
						handleSenseBody();
					}
					else if(mCurrentServerMessage.StartsWith("(hear"))
					{
						handleHear();
					}
					else if(mCurrentServerMessage.StartsWith("(player_type"))
					{
						handlePlayerType();
					}
					else if(mCurrentServerMessage.StartsWith("(player_param"))
					{
						handlePlayerParam();
					}
					else if(mCurrentServerMessage.StartsWith("(server_param"))
					{
						handleServerParam();
					}
				}
				catch(System.Net.Sockets.SocketException se)
				{// The connection timed out.
					timeOuts++;
					Console.WriteLine(se.ToString());
				}
        }
		}
		#endregion

		#region Help methods to main loop

		/// <summary>
		/// Handles "see" -messages
		/// </summary>
		private void handleSee()
		{
			OnBeforeWorldModelUpdate();
			mWM.Update(mCurrentServerMessage);
			OnAfterWorldModelUpdate();
		}

		/// <summary>
		/// Handles "sense_body" -messages
		/// </summary>
		private void handleSenseBody()
		{
			OnBeforeNewCycle();
			mClock.Synchronize();
			mPrevSenseBody = mSenseBody;
			mSenseBody = Info.SenseBody.Create(mCurrentServerMessage);
						
			if(mSide == Side.Right && this.mSenseBody.TurnCount == 0)
			{
				mCommandQueue.Enqueue(BasicCommands.Turn(180));
				Send();
			}
			
			mWM.Update();
			OnAfterNewCycle(mSenseBody.Cycle);
		}

		/// <summary>
		/// Handles "hear" -messages
		/// </summary>
		private void handleHear()
		{
			HearInfo tmp = HearInfo.Create(mCurrentServerMessage);
			if(tmp != null)
			{
				//Coaches
				if(tmp.Sender.Equals("online_coach_left") 
					|| tmp.Sender.Equals("online_coach_right") 
					|| tmp.Sender.Equals("coach"))
				{
					OnBeforeCoachMessage();
					OnAfterCoachMessage(tmp);
				}
					//Referee
				else if(tmp.Sender.Equals("referee"))
				{
					OnBeforeRefereeMessage();
					PlayMode prevPlayMode = mPlayMode;
					mPlayMode = tmp.CurrentPlayMode;
								
					//update clock
					if((prevPlayMode == PlayMode.before_kick_off) && (mPlayMode != PlayMode.before_kick_off))
					{
						mClock.Start();
					}
					else if((prevPlayMode != PlayMode.before_kick_off) && (mPlayMode == PlayMode.before_kick_off))
					{
						mClock.Stop();
					}

					if(mPlayMode == PlayMode.goal_l || mPlayMode == PlayMode.goal_r)
					{    
						int start = mCurrentServerMessage.IndexOf("goal");
						start += 7;
						int end = mCurrentServerMessage.IndexOf(")", start);
						string sval = mCurrentServerMessage.Substring(start, end-start);
						int val = Convert.ToInt32(sval);
						
						if((mPlayMode == PlayMode.goal_l && TeamSide == Side.Left) ||
							(mPlayMode == PlayMode.goal_r && TeamSide == Side.Right))
						{
							mOwnScore = val;
						}
						else 
						{
							mOpponentScore = val;
						}
					}
							
					OnAfterRefereeMessage(tmp);
				}
				//Not a coach and not a referee, then it must be a player ;-)
				else// if(tmp.Sender.Equals("self") || tmp.Sender.Equals("Direction"))
				{
					OnBeforePlayerMessage();
					OnAfterPlayerMessage(tmp);
				}
			}
		}

		/// <summary>
		/// Handles "player_type" -messages
		/// </summary>
		private void handlePlayerType()
		{
			PlayerType tmp = PlayerType.Create(mCurrentServerMessage);
			if(tmp != null)
				mPlayerTypes.Add(tmp);
		}

		/// <summary>
		/// Handles "player_param" -messages
		/// </summary>
		private void handlePlayerParam()
		{
			mPlayerParam = PlayerParam.Create(mCurrentServerMessage);
			OnPlayerParamReceived();
		}

		/// <summary>
		/// Handles "server_param" -messages
		/// </summary>
		private void handleServerParam()
		{
			mServerParam = ServerParam.Create(mCurrentServerMessage);
			mWM = new WorldModel(this);
			OnServerParamReceived();
		}

		#endregion

		#region Communication
		/// <summary>
		/// Connects the player to a RoboCup server
		/// </summary>
		/// <remarks>The default client version is set to 8.</remarks>
		/// <param name="pHost">The url or ip of the host</param>
		/// <param name="pPort">The port to connect to on the host</param>
		/// <param name="pTimeOut">The time before a receive command time outs</param>
		/// <returns>true if connect was successful</returns>
		public bool Connect(string pHost, int pPort, int pTimeOut)
		{
			return this.Connect(pHost, pPort, pTimeOut, 8);
		}

		/// <summary>
		/// Connects the player to a RoboCup server
		/// </summary>
		/// <param name="pHost">The url or ip of the host</param>
		/// <param name="pPort">The port to connect to on the host</param>
		/// <param name="pTimeOut">The time before a receive command time outs</param>
		/// <param name="pClientVersion">The version of the client.</param>
		/// <returns>true if connect was successful</returns>
		public bool Connect(string pHost, int pPort, int pTimeOut, int pClientVersion)
		{
			mConnection = new ServerConnection(pHost, pPort, pTimeOut);
			string response;
			try
			{
				response = mConnection.Connect(mTeamName, mIsGoalie, pClientVersion);
			}
			catch(System.Net.Sockets.SocketException pSE)
			{
				mLastError = "ERROR: Time-out (" + pSE.ErrorCode + ")";
				return false;
			}

			if(response == "(error no_more_team_or_player_or_goalie)")
			{
				mLastError = "ERROR: No more team or player or goalie";
				return false;
			}
			if(parseInitMessage(response))
			{
				mCommandQueue.Enqueue(BasicCommands.Move((int)mStartPoint.X, (int)mStartPoint.Y));
				if(Send())
				{
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// Reconnects the player to the previous server
		/// </summary>
		/// <returns>true if reconnect was successful, otherwise false</returns>
		public bool ReConnect()
		{
			if(mConnection == null)
				return false;

			string response = mConnection.Reconnect(mTeamName, mUniformNumber);
			if(response == "(error no_more_team_or_player)" || response == "(error reconnect)")
			{
				mLastError = "Could not reconnect to server : " + response;
				return false;
			}

			if(parseInitMessage(response))
			{
				mCommandQueue.Enqueue(BasicCommands.Move((int)mStartPoint.X, (int)mStartPoint.Y));//new Command("(move " + mStartPoint.X + " " + mStartPoint.Y + ")", CommandType.Move));
				if(Send())
				{
					return true;
				}
			}
			return false;

		//	return (parseInitMessage(response) && Send(new Command("(move " + mStartPoint.X + " " + mStartPoint.Y + ")", CommandType.Move)));
		}

		/// <summary>
		/// Disconnects the Player from the server
		/// </summary>
		public void Disconnect()
		{
			mCommandQueue.Enqueue(BasicCommands.Bye());
			Send();
			mConnection.CloseConnection();
			mConnection = null;
		}

		/// <summary>
		/// Parses an init message
		/// </summary>
		/// <param name="pMessage">The message to parse</param>
		/// <returns>true if message could be parsed, otherwise false</returns>
		private bool parseInitMessage(string pMessage)
		{
			//(init Side Unum PlayMode)
			string[] initString = pMessage.Substring(1, pMessage.LastIndexOf(')') - 1).Split(' ');
			if(initString[0] == "init")
			{
				mSide = (initString[1] == "l" ? Side.Left : Side.Right);
				try
				{
					mUniformNumber = int.Parse(initString[2]);
					mPlayMode = (PlayMode)Enum.Parse(typeof(PlayMode), initString[3]);
				}
				catch(ArgumentException pAE)
				{
					mLastError = pAE.ToString();
					return false;
				}
				catch(SystemException pSE)
				{
					mLastError = "Unable to parse uniform-number:\n" + pSE.ToString();
					return false;
				}
				return true;
			}
			else
			{
				mLastError = "Unknown message: " + pMessage;
				return false;
			}
		}

		/// <summary>
		/// Sends the commands stored in the command queue
		/// </summary>
		/// <returns>true if the command could be sent, otherwise false</returns>
		public bool Send()
		{
			try
			{
				string command = mCommandQueue.GetCommandString();
				mConnection.Send(command);
			}
			catch(Exception e)
			{
				mLastError = e.ToString();
				return false;
			}

			CommandQueue tmp = mPrevCommandQueue;
			mPrevCommandQueue = mCommandQueue;
			mCommandQueue = tmp;
			mCommandQueue.Clear();

			return true;
		}
		#endregion

		#region Misc. methods
		/// <summary>
		/// Creates a string representation of the Player
		/// </summary>
		/// <returns>The string representation</returns>
		public override string ToString()
		{
			return "Player " + mUniformNumber;
		}

		/// <summary>
		/// The finalizer disconnects the client from the server. 
		/// </summary>
		~Player()
		{
			Disconnect();
		}
		#endregion

		#region Properties
		/// <summary>The last error that has occured.</summary>
		/// <remarks>If any error occurs in the player (e.g. connection with the server) this is
		/// where the last error-value can be retrieved.</remarks>
		/// <value>A string value representing the last error that has occured in the player.</value>
		public string LastError
		{
			get { return mLastError; }
		}

		/// <summary>The players starting point.</summary>
		/// <remarks>This point is used when the player does a move. E.g. at the beginning of each
		/// half-time, or after a goal.</remarks>
		/// <value>If the point is set to be on the opponents side (X > 0), the point is set to X * (-1).
		/// The is no check of the Y-value, since the player will be placed at the long-side of the field
		/// by the server in case the value is outside the field.</value>
		public Point2D StartPoint
		{
			get { return mStartPoint; }
			set
			{
				if(value.X > 0)
					value.X *= -1;
				mStartPoint = value;
			}
		}

		/// <summary>
		/// Goalie or not
		/// </summary>
		public bool IsGoalie
		{
			get { return mIsGoalie; }
		}

		/// <summary>
		/// Team name
		/// </summary>
		public string TeamName
		{
			get { return mTeamName; }
		}

		/// <summary>
		/// Uniform number
		/// </summary>
		public int UniformNumber
		{
			get { return mUniformNumber; }
			set { mUniformNumber = value; }
		}

		/// <summary>
		/// Side [Left|Right]
		/// </summary>
		public Side TeamSide
		{
			get { return mSide; }
			set { mSide = value; }
		}

		/// <summary>
		/// The WorldModel of this Player
		/// </summary>
		public WorldModel World
		{
			get { return mWM; }
		}

		/// <summary>
		/// The internal clock
		/// </summary>
		public Clock Clock
		{
			get{ return mClock; }
		}

		/// <summary>
		/// The server parameters
		/// </summary>
		public ServerParam ServerParam
		{
			get{ return mServerParam; }
		}

		/// <summary>
		/// The SenseBody
		/// </summary>
		public SenseBody SenseBody
		{
			get{ return mSenseBody; }
		}

		/// <summary>
		/// The previous SenseBody
		/// </summary>
		public SenseBody PreviousSenseBody
		{
			get{ return mPrevSenseBody; }
		}

		/// <summary>
		/// The current PlayMode
		/// </summary>
		public PlayMode PlayMode
		{
			get{ return mPlayMode; }
		}

		/// <summary>
		/// The current CommandQueue
		/// </summary>
		public CommandQueue CommandQueue
		{
			get{return mCommandQueue;}
		}

		/// <summary>
		/// The previous CommandQueue
		/// </summary>
		public CommandQueue PreviousCommandQueue
		{
			get{return mPrevCommandQueue;}
		}

		/// <summary>
		/// Determines if the ball is catchable.
		/// </summary>
		/// <value>true if the player is a goalie, the ball is seen in this cycle and
		/// the ball is within catchable length according to the server param.</value>
		public bool BallIsCatchable
		{
			get
			{
				return (mIsGoalie && mWM.TheBall.LastSeen == mWM.UpdatedFrame &&
					mWM.TheBall.Distance <= mServerParam.CatchableAreaLength);
			}
		}

		/// <summary>
		/// Determines if the ball is kickable.
		/// </summary>
		/// <value>true if the ball is seen in this cycle and
		/// the ball is within Kickable margin according to the server param.</value>
		public bool BallIsKickable
		{
			get
			{
				return (mWM.TheBall.SeenThisCycle && mWM.TheBall.Distance <= mServerParam.KickableMargin);
			}
		}

		/// <summary>
		/// The player types
		/// </summary>
		public ArrayList PlayerTypes
		{
			get{return mPlayerTypes;}
		}

		/// <summary>The current score of my team</summary>
		public int OwnScore
		{
			get{return mOwnScore;}
		}

		/// <summary>The current score of the opponent team</summary>
		public int OpponentScore
		{
			get{return mOpponentScore;}
		}

		/// <summary>The differance between own score and opponent score</summary>
		public int ScoreDifferance
		{
			get{return mOwnScore - mOpponentScore;}
		}

		#endregion
		
		#region Events
		/// <summary>Event indicating that a new cycle is coming once the clock has been syncronized</summary>
		public event EventHandler BeforeNewCycle;
		/// <summary>Event indicating that a new cycle has started</summary>
		public event TimeEventHandler AfterNewCycle;
		/// <summary>Event indicating that the WorldModel is to be updated</summary>
		public event EventHandler BeforeWorldModelUpdate;
		/// <summary>Event indicating that the WorldModel is updated</summary>
		public event EventHandler AfterWorldModelUpdate;
		/// <summary>Event indicating that a RefereeMessage is to be handled</summary>
		public event EventHandler BeforeRefereeMessage;
		/// <summary>Event indicating that a RefereeMessage has been handled</summary>
		public event MessageEventHandler AfterRefereeMessage;
		/// <summary>Event indicating that a CoachMessage is to be handled </summary>
		public event EventHandler BeforeCoachMessage;
		/// <summary>Event indicating that a CoachMessage has been handled </summary>
		public event MessageEventHandler AfterCoachMessage;
		/// <summary>Event indicating that a PlayerMessage is to handled </summary>
		public event EventHandler BeforePlayerMessage;
		/// <summary>Event indicating that a PlayerMessage has been handled</summary>
		public event MessageEventHandler AfterPlayerMessage;
		/// <summary>Event indicating that a ServerParam has been handled, and the worldmodel is updated.</summary>
		public event EventHandler ServerParamReceived;
		/// <summary>Event indicating that a PlayerParam has been handled</summary>
		public event EventHandler PlayerParamReceived;

		/// <summary>
		/// Generates a <see cref="BeforeNewCycle"/> event
		/// </summary>
		protected void OnBeforeNewCycle()
		{
			if(BeforeNewCycle != null)
			{
				BeforeNewCycle(this, EventArgs.Empty);
			}
		}

		/// <summary>
		/// Generates an AfterNewCycle event
		/// </summary>
		/// <param name="args">The current cycle</param>
		protected void OnAfterNewCycle(int args)
		{
			if(AfterNewCycle != null)
			{
				AfterNewCycle(this, new TimeEventArgs(args));
			}
		}

		/// <summary>
		/// Generates a BeforeWorldModelUpdate event
		/// </summary>
		protected void OnBeforeWorldModelUpdate()
		{
			if(BeforeWorldModelUpdate != null)
			{
				BeforeWorldModelUpdate(this, EventArgs.Empty);
			}
		}

		/// <summary>
		/// Generates an AfterWorldModelUpdate event
		/// </summary>
		protected void OnAfterWorldModelUpdate()
		{
			if(AfterWorldModelUpdate != null)
			{
				AfterWorldModelUpdate(this, EventArgs.Empty);
			}
		}

		/// <summary>
		/// Generates a BeforeRefereeMessage event
		/// </summary>
		protected void OnBeforeRefereeMessage()
		{
			if(BeforeRefereeMessage != null)
			{
				BeforeRefereeMessage(this, EventArgs.Empty);
			}
		}

		/// <summary>
		/// Generates an AfterRefereeMessage event
		/// </summary>
		/// <param name="arg">The message</param>
		protected void OnAfterRefereeMessage(HearInfo arg)
		{
			if(AfterRefereeMessage != null)
			{
				AfterRefereeMessage(this, new HearEventArgs(arg));
			}
		}

		/// <summary>
		/// Generates a BeforeCoachMessage event
		/// </summary>
		protected void OnBeforeCoachMessage()
		{
			if(BeforeCoachMessage != null)
			{
				BeforeCoachMessage(this, EventArgs.Empty);
			}
		}

		/// <summary>
		/// Generates an AfterCoachMessage event
		/// </summary>
		/// <param name="arg">The message</param>
		protected void OnAfterCoachMessage(HearInfo arg)
		{
			if(AfterCoachMessage != null)
			{
				AfterCoachMessage(this, new HearEventArgs(arg));
			}
		}

		/// <summary>
		/// Generates a BeforePlayerMessage event
		/// </summary>
		protected void OnBeforePlayerMessage()
		{
			if(BeforePlayerMessage != null)
			{
				BeforePlayerMessage(this, EventArgs.Empty);
			}
		}

		/// <summary>
		/// Generates an AfterPlayerMessage event
		/// </summary>
		/// <param name="arg">The message</param>
		protected void OnAfterPlayerMessage(HearInfo arg)
		{
			if(AfterPlayerMessage != null)
			{
				AfterPlayerMessage(this, new HearEventArgs(arg));
			}
		}

		/// <summary>
		/// Generates a ServerParamReceived event
		/// </summary>
		protected void OnServerParamReceived()
		{
			if(ServerParamReceived != null)
			{
				ServerParamReceived(this, EventArgs.Empty);
			}
		}

		/// <summary>
		/// Generates a PlayerParamReceived event
		/// </summary>
		protected void OnPlayerParamReceived()
		{
			if(PlayerParamReceived != null)
			{
				PlayerParamReceived(this, EventArgs.Empty);
			}
		}
		#endregion
	}
}
