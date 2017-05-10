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
using System.Globalization;

namespace TeamYaffa.CRaPI.Info
{
	/// <summary>This is the base class for other info objects.</summary>
	/// <remarks>This class provides a hashtable where keys-value pairs from messages from the 
	/// server can be stored, e.g. server parameters, player parameters or sensebody information.
	/// This class also provides functionality to parse doubles and ints from the values in the
	/// hashtable, and if it is the first or the second value that is of interest, i.e. one key
	/// can have one or two values connected to it, it depends on the message.</remarks>
	public abstract class InfoBase
	{
		#region Members and constructors
		/// <summary>The hashtable containing the keys and values parsed from the message.</summary>
		protected Hashtable mValues;
		/// <summary>The numberformatter is needed to parse the messages from the server that contains
		/// floating point numbers. The server uses a . (dot) as a decimal separator, but when the .NET
		/// uses localisation dependant separator.</summary>
		private NumberFormatInfo mNumberFormatter;

		/// <summary>Makes sure the decimal separator is a . (dot).</summary>
		protected InfoBase()
		{
			mNumberFormatter = new NumberFormatInfo();
			mNumberFormatter.NumberDecimalSeparator = ".";
		}
		#endregion

		#region Get methods
		/// <summary>Gets a value from the message, mapping <paramref name="pKey"/>
		/// to an attribute, and converts the value to a double.</summary>
		/// <param name="pKey">Which attribute that is wanted.</param>
		/// <returns>The value, or -1 if there is no attribute mapping to <paramref name="pKey"/>.</returns>
		/// <seealso cref="getDouble(string, bool)"/>
		/// <ssealso cref="getInt(string)"/>
		public double getDouble(string pKey)
		{
			string sResult = (string)mValues[pKey];
			if(sResult == null) return -1;
			return Double.Parse(sResult, mNumberFormatter);
		}

		/// <summary>Divides a two value attribute, and returns the requested value.</summary>
		/// <remarks>When an attribute has two values, this method can used to divide
		/// the values in two parts, as the values are stored as one string in the hashtable.</remarks>
		/// <param name="pKey">Which attribute that is wanted.</param>
		/// <param name="pFirst">If it is the first or the second value.</param>
		/// <returns>The value, or -1 if there is no attribute mapping to <paramref name="pKey"/>.</returns>
		/// <seealso cref="getDouble(string)"/>
		/// <ssealso cref="getInt(string)"/>
		public double getDouble(string pKey, bool pFirst)
		{
			string sResult = (string)mValues[pKey];
			if(sResult == null) return -1;
			if(pFirst)
				return Double.Parse(sResult.Substring(0, sResult.IndexOf(' ')), mNumberFormatter);
			else
				return Double.Parse(sResult.Substring(sResult.IndexOf(' ') + 1), mNumberFormatter);
		}

		/// <summary>Gets a value from the message, mapping <paramref name="pKey"/>
		/// to an attribute, and converts the value to an int.</summary>
		/// <param name="pKey">Which attribute that is wanted.</param>
		/// <returns>The value, or -1 if there is no attribute mapping to <paramref name="pKey"/>.</returns>
		/// <seealso cref="getDouble(string, bool)"/>
		/// <seealso cref="getDouble(string)"/>
		public int getInt(string pKey)
		{
			string sResult = (string)mValues[pKey];
			if(sResult == null) return -1;
			return Int32.Parse(sResult);
		}
		#endregion

		#region Properties
		/// <summary>The parameter from the server</summary>
		/// <param name="pKey">The key that matches the requested server parameter.</param>
		/// <value>A string representation of the server parameter associated with the specified key.</value>
		/// <exception cref="System.ArgumentNullException">see <see cref="System.Collections.Hashtable.this[object]">
		/// System.Collections.Hashtable</see> for details.</exception>
		/// <exception cref="System.NotSupportedException">see <see cref="System.Collections.Hashtable.this[object]">
		/// System.Collections.Hashtable</see> for details.</exception>
		public string this[string pKey]
		{
			get { return (string)mValues[pKey]; }
		}
		#endregion

		#region Misc. operations
		/// <summary>The string representation of the class.</summary>
		/// <returns>The type of the class, and all key-value pair in the hashtable.</returns>
		public override string ToString()
		{
			string result = base.ToString() + "\n";
			foreach(string key in mValues.Keys)
			{
				result += key.ToString() + ":\t" + mValues[key] + "\n";
			}
			return result;
		}
		#endregion
	}
}