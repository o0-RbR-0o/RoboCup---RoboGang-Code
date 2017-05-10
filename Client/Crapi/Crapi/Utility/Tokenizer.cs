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

namespace TeamYaffa.CRaPI.Utility
{
	/// <summary>
	/// This class handles tokenizing of strings.
	/// </summary>
	public class Tokenizer: IEnumerable
	{
		#region Members and constructors

		/// <summary>The elements of the tokenized string</summary>
		private string[] mElements;

		/// <summary>
		/// Creates a Tokenizer of a string and and array of delimiters.
		/// </summary>
		/// <param name="pSource">The string to tokenize</param>
		/// <param name="pDelimiters">The delimiters to tokenize on</param>
		public Tokenizer(string pSource, params char[] pDelimiters)
		{
			mElements = pSource.Split(pDelimiters);
		}

		#endregion

		#region Misc. operations

		/// <summary>
		/// Returns a TokenEnumerator of the tokenized string
		/// </summary>
		/// <returns>TokenEnumerator</returns>
		public TokenEnumerator GetEnumerator() // non-IEnumerable version
		{
			return new TokenEnumerator(this);
		}

		/// <summary>
		/// Returns a IEnumerator of the tokenized string
		/// </summary>
		/// <returns>IEnumerator</returns>
		IEnumerator IEnumerable.GetEnumerator() // IEnumerable version
		{
			return (IEnumerator) new TokenEnumerator(this);
		}

		#endregion

		#region Inner class TokenEnumerator

		/// <summary>
		/// Inner class implements IEnumerator interface:
		/// </summary>
		public class TokenEnumerator: IEnumerator
		{
			private int mPosition = -1;
			private Tokenizer mTokenizer;

			/// <summary>
			/// Creates a TokenEnumerator of a Tokenizer
			/// </summary>
			/// <param name="pTokenizer">Tokenizer</param>
			public TokenEnumerator(Tokenizer pTokenizer)
			{
				this.mTokenizer = pTokenizer;
			}

			/// <summary>
			/// Moves the position to the next token in the TokenEnumerator
			/// </summary>
			/// <returns></returns>
			public bool MoveNext()
			{
				if (mPosition < mTokenizer.mElements.Length - 1)
				{
					mPosition++;
					return true;
				}
				else
				{
					return false;
				}
			}

			/// <summary>
			/// Resets the position in the TokenEnumerator
			/// </summary>
			public void Reset()
			{
				mPosition = -1;
			}

			/// <summary>
			/// The current token in the TokenEnumerator
			/// </summary>
			public string Current // non-IEnumerator version: type-safe
			{
				get
				{
					return mTokenizer.mElements[mPosition];
				}
			}

			/// <summary>
			/// IEnumerator version of the current token
			/// </summary>
			object IEnumerator.Current
			{
				get
				{
					return mTokenizer.mElements[mPosition];
				}
			}

			/// <summary>
			/// Counts the tokens in the TokenEnumerator
			/// </summary>
			/// <returns>number of tokens in the TokenEnumerator</returns>
			public int CountTokens()
			{
				return mTokenizer.mElements.Length;
			}
		}
		#endregion
	}
}
