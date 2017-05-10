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
namespace TeamYaffa.CRaPI
{
	#region PlayMode
	/// <summary>
	/// A type-safe representation of the different play modes.
	/// </summary>
	/// <remarks>By giving the different playmodes the same name as the server uses, it is
	/// possible to do a string-comparison.</remarks>
	public enum PlayMode
	{
		/// <summary>At the beginning of a half. Subsequent play mode (0 cycles):
		/// <c>kick_off_l</c> | <c>kick_off_r</c></summary>
		before_kick_off,
		/// <summary>Left team has a corner kick</summary>
		corner_kick_l,
		/// <summary>Right team has a corner kick</summary>
		corner_kick_r,
		/// <summary>Subsequent play mode (0 cycles): <c>play_on</c></summary>
		drop_ball,
		/// <summary>Left team has a free kick</summary>
		free_kick_l,
		/// <summary>Right team has a free kick</summary>
		free_kick_r,
		/// <summary>Play mode changes once the ball leaves the penalty area.
		/// Subsequent play mode: <c>play_on</c></summary>
		goal_kick_l,
		/// <summary>Play mode changes once the ball leaves the penalty area.
		/// Subsequent play mode: <c>play_on</c></summary>
		goal_kick_r,
		/// <summary>Goal for left team.
		/// Subsequent play mode (50 cycles): <c>kick_off_r</c></summary>
		goal_l,
		/// <summary>Goal for right team.
		/// Subsequent play mode (50 cycles): <c>kick_off_l</c></summary>
		goal_r,
		/// <summary>Left team has a kick in</summary>
		kick_in_l,
		/// <summary>Right team has a kick in</summary>
		kick_in_r,
		/// <summary>Announce start of play where left team have the kick off
		/// (after pressing the <c>Kick Off</c> button in the monitor)</summary>
		kick_off_l,
		/// <summary>Announce start of play where right team have the kick off
		/// (after pressing the <c>Kick Off</c> button in the monitor)</summary>
		kick_off_r,
		/// <summary>The playmode is unknown, not set, or could not be parsed from the last referee message.</summary>
		none,
		/// <summary>Subsequent play mode (30 cycles): <c>free_kick_r</c></summary>
		offside_l,
		/// <summary>Subsequent play mode (30 cycles): <c>free_kick_l</c></summary>
		offside_r,
		/// <summary>During normal play</summary>
		play_on,
		/// <summary>End of game</summary>
		time_over

/*
		/// <summary>Foul left team.
		/// Subsequent play mode (0 cycles): <c>free_kick_r</c></summary>
		foul_l,
		/// <summary>Foul right team.
		/// Subsequent play mode (0 cycles): <c>free_kick_l</c></summary>
		foul_r,
		/// <summary>The goalie of the left team has caught the ball.
		/// Subsequent play mode (0 cycles): <c>free_kick_r</c>
		/// (seems odd, but says so in the manual)</summary>
		goalie_catch_ball_l,
		/// <summary>The goalie of the right team has caught the ball.
		/// Subsequent play mode (0 cycles): <c>free_kick_l</c>
		/// (seems odd, but says so in the manual)</summary>
		goalie_catch_ball_r,
		/// <summary>sent if there was no opponent until
		/// the end of the second half.
		/// Subsequent play mode: <c>time_over</c></summary>
		time_up_without_a_team,
		/// <summary>sent once the game is over
		/// (if the time is after second half and
		/// the scores for each team are different).
		/// Subsequent play mode: <c>time_over</c></summary>
		time_up,
		/// <summary>Half time.
		/// Subsequent play mode: <c>before_kick_off</c></summary>
		half_time,
		/// <summary>End of normal play-time.
		/// Subsequent play mode: <c>before_kick_off</c></summary>
		time_extended,
		/// <summary>Left team performed an illegal free kick</summary>
		free_kick_fault_l,
		/// <summary>Right team performed an illegal free kick</summary>
		free_kick_fault_r,
		/// <summary>Left team did a back-pass to the goalie</summary>
		back_pass_l,
		/// <summary>Right team did a back-pass to the goalie</summary>
		back_pass_r,
		*/
	}
	#endregion

	#region RefereeMessage
	/// <summary>
	/// A type-safe representation of the different referee messages
	/// </summary>
	public enum RefereeMessage
	{
		/// <summary>Left team did a back-pass to the goalie (this message is undocumented in
		/// the manual).</summary>
		back_pass_l,
		/// <summary>Right team did a back-pass to the goalie (this message is undocumented in
		/// the manual).</summary>
		back_pass_r,
		/// <summary>Left team performed an illegal free kick (this message is undocumented in
		/// the manual).</summary>
		free_kick_fault_l,
		/// <summary>Right team performed an illegal free kick (this message is undocumented in
		/// the manual).</summary>
		free_kick_fault_r,
		/// <summary>Announce that left team has made a foul. Subsequent play 
		/// mode (0 cycles): <c>free_kick_r</c></summary>
		foul_l,
		/// <summary>Announce that right team has made a foul. Subsequent play 
		/// mode (0 cycles): <c>free_kick_l</c></summary>
		foul_r,
		/// <summary>Subsequent play mode (0 cycles): <c>free_kick_r</c> (seems odd?)</summary>
		goalie_catch_ball_l,
		#warning: Is it really free_kick_r
		/// <summary>Subsequent play mode (0 cycles): <c>free_kick_l</c> (seems odd?)</summary>
		goalie_catch_ball_r,
		#warning: Is it really free_kick_l
		/// <summary>Not complete! Server sends <c>goal_l_n</c>, which announces 
		/// the n:th goal for left team. Subsequent play mode (50 cycles): 
		/// <c>kick_off_r</c></summary>
		goal_l_,
		/// <summary>Not complete! Server sends <c>goal_r_n</c>, which announces 
		/// the n:th goal for right team. Subsequent play mode (50 cycles): 
		/// <c>kick_off_l</c></summary>
		goal_r_,
		/// <summary>Subsequent play mode (0 cycles): <c>before_kick_off</c></summary>
		half_time,
		/// <summary>Subsequent play mode (0 cycles): <c>before_kick_off</c></summary>
		time_extended,
		/// <summary>Sent once the game is over (if the time >= second half and the
		/// scores for each team are different). Subsequent play mode (0 cycles): 
		/// <c>time_over</c></summary>
		time_up,
		/// <summary>Sent if there was no opponent until the end of the second half.
		/// Subsequent play mode (0 cycles): <c>time_over</c></summary>
		time_up_without_a_team,
		/// <summary>Used if the Referee-message could not be recognized.</summary>
		none
	}
	#endregion

	#region Side
	/// <summary>
	/// Describes what team-side the player is playing with, i.e. what side of the
	/// field the player is starting on.
	/// </summary>
	public enum Side
	{
		/// <summary>The player starts at the left side of the play field.
		/// <para>Left is implemented as the character l, which means that it can be 
		/// used with comparisons whith messages from the server.</para></summary>
		Left = 'l',
		/// <summary>The player starts at the right side of the play field.
		/// <para>Right is implemented as the character r, which means that it can be 
		/// used with comparisons whith messages from the server.</para></summary>
		Right = 'r'
	}
	#endregion

	#region ViewModes
	/// <summary>
	/// Describes the view quality of the player
	/// </summary>
	/// <remarks>The player can influence the frequency and quality of the visual information by changing
	/// <c>ViewQuality</c> and <see cref="ViewWidth"/>.
	/// <para>For a more detailed description how the view quality affects the player, you are encouraged
	/// to read <c>Vision sensor model</c> of the server manual.</para>
	/// <para>If the view quality can not be determined, it should be set to unknown.</para></remarks>
	public enum ViewQuality
	{
		/// <summary>High view quality.</summary>
		high,
		/// <summary>Low view quality.</summary>
		low,
		/// <summary>The quality is unknown, not set, or could not be parsed from the sense-body message.</summary>
		unknown
	}

	/// <summary>
	/// Describes the width of the "view cone" of the player
	/// </summary>
	/// <remarks>The player can influence the frequency and quality of the visual information by changing
	/// <c>ViewWidth</c> and <see cref="ViewQuality"/>.
	/// <para>For a more detailed description how the view quality affects the player, you are encouraged
	/// to read <c>Vision sensor model</c> of the server manual.</para>
	/// <para>If the view width can not be determined, it should be set to unknown.</para></remarks>
	public enum ViewWidth
	{
		/// <summary>Narrow view width.</summary>
		narrow,
		/// <summary>Normal view width.</summary>
		normal,
		/// <summary>Wide view width.</summary>
		wide,
		/// <summary>The width is unknown, not set, or could not be parsed from the sense-body message.</summary>
		unknown
	}
	#endregion
}