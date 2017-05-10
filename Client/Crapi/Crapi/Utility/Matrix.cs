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
using TeamYaffa.CRaPI.World;

namespace TeamYaffa.CRaPI.Utility
{
	/// <summary>
	/// This class provides a matrix with rudimentary operations.
	/// </summary>
	public class Matrix : ICloneable
	{
		#region Members and Constructors
		/// <summary>The internal field</summary>
		private double[,] mField;
		/// <summary>The calculation types</summary>
		private enum Type{MIN, MAX, ADD, SUB, MUL, DIV}

		/// <summary>
		/// Constructs a matrix with given size
		/// </summary>
		/// <param name="pWidth">The width of the matrix</param>
		/// <param name="pHeight">The height of the matrix</param>
		public Matrix(int pWidth, int pHeight)
		{
			mField = new double[pWidth, pHeight];
		}
		#endregion

		#region Operations
		#region Superimpose

		/// <summary>
		/// Superimposes this matrix with the supplied, with respect to the "SuperimposeType" value of each cell
		/// </summary>
		/// <exception cref="System.ArgumentException">is thrown when supplied Matrix is superimposed out of bounds of the
		/// called Matrix</exception> 
		/// <param name="pType">Addition, subtraction, min or max used for superimposion</param>
		/// <param name="pMatrix">The matrix to superimpose</param>
		/// <param name="pXOffset">The X-offset where to superimpose the matrix</param>
		/// <param name="pYOffset">The Y-offset where to superimpose the matrix</param>
		private void Superimpose(Type pType, Matrix pMatrix, int pXOffset, int pYOffset)
		{
			int width = pMatrix.Width;
			int height = pMatrix.Height;

			if((pXOffset >= Width) || (pYOffset >= Height))
			{
				throw new ArgumentException("Tried to superimpose a matrix out of bounds");
			}

			int startY = pYOffset >= 0 ? pYOffset : 0;
			int endY = height + pYOffset <= Height ? height + pYOffset : Height;

			for(int y = startY; y < endY; y++)
			{
				int startX = pXOffset >= 0 ? pXOffset : 0;
				int endX = width + pXOffset <= Width ? width + pXOffset : Width;
				
				for(int x = startX; x < endX; x++)
				{
					int X = x - pXOffset;
					int Y = y - pYOffset;
					
					switch(pType)
					{
						case Type.MIN:
							mField[x,y] = Math.Min(mField[x,y], pMatrix[X,Y]);
							break;
						case Type.MAX:
							mField[x,y] = Math.Max(mField[x,y], pMatrix[X,Y]);
							break;
						case Type.ADD:
							mField[x,y] += pMatrix[X,Y];
							break;
						case Type.SUB:
							mField[x,y] -= pMatrix[X,Y];
							break;
						case Type.MUL:
							mField[x,y] *= pMatrix[X,Y];
							break;
						case Type.DIV:
							mField[x,y] /= pMatrix[X,Y];
							break;
					}
				}
			}
		}
 
		/// <summary>
		/// Superimposes this matrix with the supplied, with respect to the min value of each cell
		/// </summary>
		/// <remarks>The called matrix is altered</remarks>
		/// <exception cref="System.ArgumentException">is thrown when supplied Matrix is superimposed out of bounds of the
		/// called Matrix</exception> 
		/// <param name="pMatrix">The matrix to superimpose</param>
		/// <param name="pXOffset">The X-offset where to superimpose the matrix</param>
		/// <param name="pYOffset">The Y-offset where to superimpose the matrix</param>
		public void SuperimposeMin(Matrix pMatrix, int pXOffset, int pYOffset)
		{
			Superimpose(Type.MIN, pMatrix, pXOffset, pYOffset);
		}

		/// <summary>
		/// Superimposes this matrix with the supplied, with respect to the max value of each cell
		/// </summary>
		/// <remarks>The called matrix is altered</remarks>
		/// <exception cref="System.ArgumentException">is thrown when supplied Matrix is superimposed out of bounds of the
		/// called Matrix</exception> 
		/// <param name="pMatrix">The matrix to superimpose</param>
		/// <param name="pXOffset">The X-offset where to superimpose the matrix</param>
		/// <param name="pYOffset">The Y-offset where to superimpose the matrix</param>
		public void SuperimposeMax(Matrix pMatrix, int pXOffset, int pYOffset)
		{
			Superimpose(Type.MAX, pMatrix, pXOffset, pYOffset);
		}

		/// <summary>
		/// Superimposes this matrix with the supplied, with respect to the added value of each cell
		/// </summary>
		/// <remarks>The called matrix is altered</remarks>
		/// <exception cref="System.ArgumentException">is thrown when supplied Matrix is superimposed out of bounds of the
		/// called Matrix</exception> 
		/// <param name="pMatrix">he matrix to superimpose</param>
		/// <param name="pXOffset">The X-offset where to superimpose the matrix</param>
		/// <param name="pYOffset">The Y-offset where to superimpose the matrix</param>
		public void SuperimposeAdd(Matrix pMatrix, int pXOffset, int pYOffset)
		{
			Superimpose(Type.ADD, pMatrix, pXOffset, pYOffset);
		}

//		public void SuperimposeAdd(Matrix pMatrix, int pXOffset, int pYOffset, double pFitnessFactor)
//		{
//			Superimpose(Type.ADD, pMatrix, pXOffset, pYOffset, pFitnessFactor);
//		}

		/// <summary>
		/// Superimposes this matrix with the supplied, with respect to the subtracted value of each cell
		/// </summary>
		/// <remarks>The called matrix is altered</remarks>
		/// <exception cref="System.ArgumentException">is thrown when supplied Matrix is superimposed out of bounds of the
		/// called Matrix</exception> 
		/// <param name="pMatrix">he matrix to superimpose</param>
		/// <param name="pXOffset">The X-offset where to superimpose the matrix</param>
		/// <param name="pYOffset">The Y-offset where to superimpose the matrix</param>
		public void SuperimposeSub(Matrix pMatrix, int pXOffset, int pYOffset)
		{
			Superimpose(Type.SUB, pMatrix, pXOffset, pYOffset);
		}

		/// <summary>
		/// Superimposes this matrix with the supplied, with respect to the mulitplicated value of each cell
		/// </summary>
		/// <remarks>The called matrix is altered</remarks>
		/// <exception cref="System.ArgumentException">is thrown when supplied Matrix is superimposed out of bounds of the
		/// called Matrix</exception> 
		/// <param name="pMatrix">he matrix to superimpose</param>
		/// <param name="pXOffset">The X-offset where to superimpose the matrix</param>
		/// <param name="pYOffset">The Y-offset where to superimpose the matrix</param>
		public void SuperimposeMUL(Matrix pMatrix, int pXOffset, int pYOffset)
		{
			Superimpose(Type.MUL, pMatrix, pXOffset, pYOffset);
		}

		/// <summary>
		/// Superimposes this matrix with the supplied, with respect to the divided value of each cell
		/// </summary>
		/// <remarks>The called matrix is altered</remarks>
		/// <exception cref="System.ArgumentException">is thrown when supplied Matrix is superimposed out of bounds of the
		/// called Matrix</exception> 
		/// <param name="pMatrix">he matrix to superimpose</param>
		/// <param name="pXOffset">The X-offset where to superimpose the matrix</param>
		/// <param name="pYOffset">The Y-offset where to superimpose the matrix</param>
		public void SuperimposeDIV(Matrix pMatrix, int pXOffset, int pYOffset)
		{
			Superimpose(Type.DIV, pMatrix, pXOffset, pYOffset);
		}

		#endregion

		/// <summary>
		/// Calculates (MIN|MAX|ADD|SUB|MUL|DIV) of each cell, according to pType
		/// </summary>
		/// <remarks>A new Matrix is produced</remarks>
		/// <exception cref="System.ArgumentException">is thrown when the two matrices are of different sizes</exception>
		/// <param name="pType">The type of calculation</param>
		/// <param name="pMatrix1">The first matrix</param>
		/// <param name="pMatrix2">The second matrix</param>
		/// <returns>The result matrix</returns>
		private static Matrix calculate(Type pType, Matrix pMatrix1, Matrix pMatrix2)
		{
			int width = pMatrix1.Width;
			int height = pMatrix1.Height;
			if(!((width == pMatrix2.Width) && (height == pMatrix2.Height)))
				throw new ArgumentException("Matrices have different sizes");

			double[,] field = new double[width, height];

			for(int y = 0; y < height; y++)
			{
				for(int x = 0; x < width; x++)
				{
					switch(pType)
					{
						case Type.MIN:
							field[x,y] = Math.Min(pMatrix1[x,y], pMatrix2[x,y]);
							break;
						case Type.MAX:
							field[x,y] = Math.Max(pMatrix1[x,y], pMatrix2[x,y]);
							break;
						case Type.ADD:
							field[x,y] = pMatrix1[x,y] + pMatrix2[x,y];
							break;
						case Type.SUB:
							field[x,y] = pMatrix1[x,y] - pMatrix2[x,y];
							break;
						case Type.MUL:
							field[x,y] = pMatrix1[x,y] * pMatrix2[x,y];
							break;
						case Type.DIV:
							field[x,y] = pMatrix1[x,y] / pMatrix2[x,y];
							break;
					}
				}
			}
			Matrix m = new Matrix(width, height);
			m.Field = field;
			return m;
		}

		/// <summary>
		/// Calculate the MIN of each cell of the two matrices
		/// </summary>
		/// <remarks>A new Matrix is produced</remarks>
		/// <exception cref="System.ArgumentException">is thrown when the two matrices are of different sizes</exception>
		/// <param name="pMatrix1">First matrix</param>
		/// <param name="pMatrix2">Second matrix</param>
		/// <returns>The result matrix</returns>
		public Matrix CalculateMIN(Matrix pMatrix1, Matrix pMatrix2)
		{
			return calculate(Type.MIN, pMatrix1, pMatrix2);
		}

		/// <summary>
		/// Calculates the MAX of each cell of the two matrices
		/// </summary>
		/// <remarks>A new Matrix is produced</remarks>
		/// <exception cref="System.ArgumentException">is thrown when the two matrices are of different sizes</exception>
		/// <param name="pMatrix1">First matrix</param>
		/// <param name="pMatrix2">Second matrix</param>
		/// <returns>The result matrix</returns>
		public Matrix CalculateMAX(Matrix pMatrix1, Matrix pMatrix2)
		{
			return calculate(Type.MAX, pMatrix1, pMatrix2);
		}

		/// <summary>
		/// Calculates the subtracted value of each cell of two matrices
		/// </summary>
		/// <remarks>A new Matrix is produced</remarks>
		/// <exception cref="System.ArgumentException">is thrown when the two matrices are of different sizes</exception>
		/// <param name="pMatrix1">First matrix</param>
		/// <param name="pMatrix2">Second matrix</param>
		/// <returns>The result matrix</returns>
		public static Matrix operator-(Matrix pMatrix1, Matrix pMatrix2)
		{
			return calculate(Type.SUB, pMatrix1, pMatrix2);
		}

		/// <summary>
		/// Calculates the added value of each cell of two matrices
		/// </summary>
		/// <remarks>A new Matrix is produced</remarks>
		/// <exception cref="System.ArgumentException">is thrown when the two matrices are of different sizes</exception>
		/// <param name="pMatrix1">First matrix</param>
		/// <param name="pMatrix2">Second matrix</param>
		/// <returns>The result matrix</returns>
		public static Matrix operator+(Matrix pMatrix1, Matrix pMatrix2)
		{
			return calculate(Type.ADD, pMatrix1, pMatrix2);
		}

		/// <summary>
		/// Calculates the multiplicated value of each cell of the two matrices
		/// </summary>
		/// <remarks>A new Matrix is produced</remarks>
		/// <exception cref="System.ArgumentException">is thrown when the two matrices are of different sizes</exception>
		/// <param name="pMatrix1">First matrix</param>
		/// <param name="pMatrix2">Second matrix</param>
		/// <returns>The result matrix</returns>
		public static Matrix operator*(Matrix pMatrix1, Matrix pMatrix2)
		{
			return calculate(Type.MUL, pMatrix1, pMatrix2);
		}

		/// <summary>
		/// Calculates the divided value of each cell of two matrices
		/// </summary>
		/// <remarks>A new Matrix is produced</remarks>
		/// <exception cref="System.ArgumentException">is thrown when the two matrices are of different sizes</exception>
		/// <param name="pMatrix1">First matrix</param>
		/// <param name="pMatrix2">Second matrix</param>
		/// <returns>The result matrix</returns>
		public static Matrix operator/(Matrix pMatrix1, Matrix pMatrix2)
		{
			return calculate(Type.DIV, pMatrix1, pMatrix2);
		}

		#endregion

		#region Misc. operations

		/// <summary>A string representation of the matrix.</summary>
		/// <returns>A string</returns>
		public override string ToString()
		{
			System.Text.StringBuilder sb = new System.Text.StringBuilder(Width*Height*5);
			for(int y = 0; y < Height; y++)
			{
				for(int x = 0; x < Width; x++)
				{
					sb.Append('[');
					sb.Append(Math.Round(mField[x,y], 1));
					sb.Append(']');
					if(x == (Width-1))
					{
						sb.Append('\n');
					}
				}
			}
			return sb.ToString();
		}

		/// <summary>
		/// Creates a deep copy of the <c>Matrix</c>.
		/// </summary>
		/// <remarks>The deep copy creates a new <c>Matrix</c> and copies the values from the origin to the
		/// new <c>Matrix</c>.
		/// <para>The clone is of type <see cref="Matrix"/>.</para></remarks>
		/// <returns>A copy of the <see cref="Matrix"/>.</returns>
		object ICloneable.Clone()
		{
			return this.Clone();
		}

		/// <summary>
		/// Creates a deep copy of the <c>Matrix</c>.
		/// </summary>
		/// <remarks>The deep copy creates a new <c>Matrix</c> and copies the values from the origin to the
		/// new <c>Matrix</c>.</remarks>
		/// <returns>A copy of the <see cref="Matrix"/>.</returns>
		public Matrix Clone()
		{
			Matrix temp = new Matrix(this.Width, this.Height);
			for(int i = 0; i < this.Width; i++)
			{
				for(int j = 0; j < this.Height; j++)
				{
					temp[i,j] = this[i,j];
				}
			}
			return temp;
		}

		#endregion

		#region Properties

		/// <summary>
		/// The number of cells of the matrix in X-dimension
		/// </summary>
		public int Width
		{
			get{return mField.GetLength(0);}
		}
		
		/// <summary>
		/// The number of cells of the matrix in Y-dimension
		/// </summary>
		public int Height
		{
			get{return mField.GetLength(1);}
		}

		/// <summary>
		/// The number of cells of the matrix in both dimensions
		/// </summary>
		public int Size
		{
			get{return Width * Height;}
		}

		/// <summary>
		/// The internal representation
		/// </summary>
		public double[,] Field
		{
			set{mField = value;}
			get{return mField;}
		}

		/// <summary>
		/// This Matrix
		/// </summary>
		public double this[int x, int y]
		{
			set{mField[x,y] = value;}
			get{return mField[x,y];}
		}
		#endregion
	}
}
