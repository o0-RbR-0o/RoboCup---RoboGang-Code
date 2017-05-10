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

namespace TeamYaffa.CRaPI.Commands
{
	/// <summary>
	/// A wrapper for a change_view command.
	/// </summary>
	public class ChangeView : Command
	{
		#region Members and constructor

		/// <summary>The width of the view</summary>
		ViewWidth mViewWidth;
		/// <summary>The quality of the view</summary>
		ViewQuality mViewQuality;

		/// <summary>
		/// Constructs a ChangeView command object
		/// </summary>
		/// <remarks>The command will of the form:
		/// <code>(change_view pWidth pQuality)</code></remarks>
		/// <param name="pWidth">The width of the view</param>
		/// <param name="pQuality">The quality of the view</param>
		public ChangeView(ViewWidth pWidth, ViewQuality pQuality) : base(CommandType.Change_View)
		{
			mViewWidth = pWidth;
			mViewQuality = pQuality;

			mCommand = mCommand.Substring(0, mCommand.Length - 1); // remove ')' from end of command string
			mCommand += " " + mViewWidth.ToString().ToLower() + " " + mViewQuality.ToString().ToLower() + ")";
		}
		#endregion

		#region Properties

		/// <summary>
		/// The quality of the view
		/// </summary>
		public ViewQuality ViewQuality
		{
			get{ return mViewQuality; }
			set{ mViewQuality = value; }
		}

		/// <summary>
		/// The width of the view
		/// </summary>
		public ViewWidth ViewWidth
		{
			get{ return mViewWidth; }
			set{ mViewWidth = value; }
		}
		#endregion
	}
}
