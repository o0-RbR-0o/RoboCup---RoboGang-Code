using System;
using TeamYaffa.CRaPI.Utility;

namespace TeamYaffa.CRaPI.World.Fuzzy
{
	/// <summary>
	/// Summary description for PositioningMatrix.
	/// </summary>
	public class PositioningMatrix : Matrix
	{
		#region Members and Constructors
		private Intersection mIntersection;
		
		/// <summary>
		/// Constructs a PositioningMatrix with given size
		/// </summary>
		/// <param name="pWidth">The width</param>
		/// <param name="pHeight">The height</param>
		/// <param name="pIntersection">The intersection that corresponds to this matrix</param>
		public PositioningMatrix(int pWidth, int pHeight, Intersection pIntersection) : base(pWidth, pHeight)
		{
			//base(pWidth, pHeight);
			mIntersection = pIntersection;
		}

		/// <summary>
		/// Constructs a PositioningMatrix from a generic Matrix
		/// </summary>
		/// <param name="pMatrix">The matrix to build this matrix from</param>
		public PositioningMatrix(Matrix pMatrix) : base(pMatrix.Width, pMatrix.Height)
		{
			Field = pMatrix.Field;
		}
		#endregion

		#region Properties
		public Intersection Intersection
		{
			get{return mIntersection;}
			set{mIntersection = value;}
		}
		#endregion
	}
}
