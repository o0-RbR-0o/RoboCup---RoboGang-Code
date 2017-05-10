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
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace TeamYaffa.CRaPI.Net
{
	/// <summary>
	/// This class takes care of all communication with the server.
	/// </summary>
	/// <remarks>This class makes it possible and simplifies to <see cref="Connect"/>,
	/// <see cref="Reconnect"/> and <see cref="Send"/> to the server, and
	/// <see cref="Receive"/> from the server.
	/// <para>The ServerConnection class should later also be able to handle
	/// the connection between the server and a monitor.</para></remarks>
	public class ServerConnection
	{
		#region Members and constructors
		
		/// <summary>The Socket providing the low-level connection to the server</summary>
		private Socket mSocket;
		/// <summary>The wrapped ip information of the server</summary>
		private IPEndPoint mEndPoint;
		/// <summary>The ip-address of the server</summary>
		private readonly string mServerAddress;
		/// <summary>The port on the server to connect to</summary>
		private readonly int mServerPort;
		/// <summary>The data buffer to receive data from server in</summary>
		private Byte[] mReceiveBytes = new Byte[4096];

		/// <summary>
		/// Creates a ServerConnection
		/// </summary>
		/// <remarks>A number of exception can occur when creating the ServerConnection.
		/// You could take a closer look at <see cref="IPEndPoint"/>, <see cref="Dns"/>
		/// and <see cref="System.Net.IPAddress"/>.</remarks>
		/// <param name="pServerAddress">The address of the server</param>
		/// <param name="pServerPort">The port of the server to connect to</param>
		/// <param name="pTimeOut">The time to wait for a receive before timing out</param>
		public ServerConnection(string pServerAddress, int pServerPort, int pTimeOut)
		{
			mServerAddress = pServerAddress;
			mServerPort = pServerPort;
			mEndPoint = new IPEndPoint(Dns.GetHostByName(pServerAddress).AddressList[0], pServerPort);
			mSocket = new Socket(mEndPoint.AddressFamily, SocketType.Dgram, ProtocolType.Udp);
			mSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout, pTimeOut);
		}
		#endregion

		#region Connect and Reconnect
		/// <summary>
		/// Connects to the server by sending an init-command.
		/// </summary>
		/// <returns>The response from the server. Can be one of two things:
		/// <example><list type="bullet">
		/// <item><description>(init Side Unum PlayMode)</description></item>
		/// <item><description>(error no_more_team_or_player_or_goalie)</description></item>
		/// </list></example></returns>
		/// <remarks><list type="bullet">
		/// <item><description>Side == l | r</description></item>
		/// <item><description>Unum == the player number</description></item>
		/// <item><description>PlayMode == <see cref="TeamYaffa.CRaPI.PlayMode"/></description></item>
		/// </list></remarks>
		/// <param name="pTeamName">The name of the player's team</param>
		/// <param name="pIsGoalie">Whether the player is a goalie or not</param>
		/// <exception cref="System.Net.Sockets.SocketException">Thrown when the socket times-out</exception>
		public String Connect(string pTeamName, bool pIsGoalie)
		{
			return this.Connect(pTeamName, pIsGoalie, 15);
		}

		/// <summary>
		/// Connects to the server by sending an init-command.
		/// </summary>
		/// <returns>The response from the server. Can be one of two things:
		/// <example><list type="bullet">
		/// <item><description>(init Side Unum PlayMode)</description></item>
		/// <item><description>(error no_more_team_or_player_or_goalie)</description></item>
		/// </list></example></returns>
		/// <remarks><list type="bullet">
		/// <item><description>Side == l | r</description></item>
		/// <item><description>Unum == the player number</description></item>
		/// <item><description>PlayMode == <see cref="TeamYaffa.CRaPI.PlayMode"/></description></item>
		/// </list></remarks>
		/// <param name="pTeamName">The name of the player's team</param>
		/// <param name="pIsGoalie">Whether the player is a goalie or not</param>
		/// <param name="pVersion">The version of the player</param>
		/// <exception cref="System.Net.Sockets.SocketException">Thrown when the socket times-out</exception>
		public string Connect(string pTeamName, bool pIsGoalie, int pVersion)
		{
			//Send init data to server
			string sendString = "(init " + pTeamName + " (version " + pVersion + ")" + (pIsGoalie ? " (goalie))" : ")");
			Byte[] sendBytes = Encoding.ASCII.GetBytes(sendString);
			mSocket.SendTo(sendBytes, sendBytes.Length, SocketFlags.None, (EndPoint)mEndPoint);

			mEndPoint.Address = IPAddress.Any;
			mEndPoint.Port = 0;
			return Receive();
		}

		/// <summary>
		/// Reconnects to a server
		/// </summary>
		/// <returns>The response from the server. Can be one of three things:
		/// <example><list type="bullet">
		/// <item><description>(init Side Unum PlayMode)</description></item>
		/// <item><description>(error no_more_team_or_player)</description></item>
		/// <item><description>(error reconnect)</description></item>
		/// </list></example>
		/// </returns>
		/// <remarks><list type="bullet">
		/// <item><description>Side == l | r</description></item>
		/// <item><description>Unum == 1 ~ 11</description></item>
		/// <item><description>PlayMode == <see cref="TeamYaffa.CRaPI.PlayMode"/></description></item>
		/// </list></remarks>
		/// <param name="pTeamName">The name of the player's team</param>
		/// <param name="pPlayerNumber">The player's jersey number</param>
		/// <exception cref="System.Net.Sockets.SocketException">Thrown when the socket times-out</exception>
		public string Reconnect(string pTeamName, int pPlayerNumber)
		{
			string sendString = "reconnect " + pTeamName + " " + pPlayerNumber + ")";
			Byte[] sendBytes = Encoding.ASCII.GetBytes(sendString);
			mSocket.SendTo(sendBytes, sendBytes.Length, SocketFlags.None, (EndPoint)mEndPoint);
			mEndPoint.Address = IPAddress.Any;
			mEndPoint.Port = 0;
			return Receive();
		}

		/// <summary>
		/// Closes the socket to the server.
		/// </summary>
		public void CloseConnection()
		{
			mSocket.Close();
		}
		#endregion

		#region Send and receive
		/// <summary>Retrieves a messages from the Robocup server.</summary>
		/// <returns>The message from the server.</returns>
		/// <exception cref="System.Net.Sockets.SocketException">Thrown when the socket times-out</exception>
		public string Receive()
		{
			EndPoint tmpEndPoint = (EndPoint)mEndPoint;
			mSocket.ReceiveFrom(mReceiveBytes, mReceiveBytes.Length, SocketFlags.None, ref tmpEndPoint);
			int endIndex = Array.IndexOf(mReceiveBytes, (byte)0);
			endIndex = (mReceiveBytes[endIndex-1] == (byte)10 ? endIndex-1 : endIndex);
			mEndPoint.Address = ((IPEndPoint)tmpEndPoint).Address;
			mEndPoint.Port = ((IPEndPoint)tmpEndPoint).Port;
			return Encoding.ASCII.GetString(mReceiveBytes, 0, endIndex);
		}

		/// <summary>Sends a message to the Robocup server.</summary>
		/// <param name="pMessage">The message to send to the server.</param>
		/// <remarks>Since <c>ServerConnection</c> uses the udp-protocol, no 
		/// evaluation is made whether the message was received by the server or not.</remarks>
		/// <returns>Number of bytes sent to the server.</returns>
		public int Send(string pMessage)
		{
			EndPoint tmpEndPoint = (EndPoint)mEndPoint;
			Byte[] sendBytes = Encoding.ASCII.GetBytes(pMessage +  "\0");
			return mSocket.SendTo(sendBytes, sendBytes.Length, SocketFlags.None, tmpEndPoint);
		}
		#endregion

		#region Properties
		/// <summary>The port which the server is listening on.</summary>
		/// <remarks>The port number is obtained from the last message from the server.</remarks>
		/// <value>The servers port number for the current player</value>
		public int Port
		{
			get { return mEndPoint.Port; }
		}

		/// <summary>The adress where the server resides.</summary>
		/// <remarks>Since the adress of the server most likely won't change during a game,
		/// the <c>Adress</c> is read-only.</remarks>
		/// <value>The ip-adress to the server, and the adress family it belongs to.</value>
		public string Adress
		{
			get { return mEndPoint.Address + "(" + mEndPoint.AddressFamily + ")"; }
		}
		#endregion
	}
}
