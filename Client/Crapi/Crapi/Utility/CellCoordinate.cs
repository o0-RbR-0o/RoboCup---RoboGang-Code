using System;

namespace TeamYaffa.CRaPI.Utility
{
	/// <summary>
	/// This struct represents a coordinate in a <see cref="Matrix"/>.
	/// </summary>
	public struct CellCoordinate
	{
		#region Members
		/// <summary>The horizontal cell</summary>
		private int mHCell;
		/// <summary>The vertical cell</summary>
		private int mVCell;
		#endregion

		/// <summary>
		/// Creates the CellCoordinate with the horizontal and the vertical cell number, that together
		/// makes up the cellcoordinate.
		/// </summary>
		/// <param name="pHCell">The horizontal cell</param>
		/// <param name="pVCell">The vertical cell</param>
		/// <exception cref="ArgumentOutOfRangeException">If either of the parameters are less
		/// than zero this exception will be thrown.</exception>
		public CellCoordinate(int pHCell, int pVCell)
		{
			if(pHCell < 0)
				throw new ArgumentOutOfRangeException("pHCell", pHCell, "CellCoordinate can not be " +
					"initialized with a negative value.");
			if(pVCell < 0)
				throw new ArgumentOutOfRangeException("pVCell", pVCell, "CellCoordinate can not be " +
					"initialized with a negative value.");

			mHCell = pHCell;
			mVCell = pVCell;
		}

		/// <summary>The horizontal cell</summary>
		/// <exception cref="ArgumentOutOfRangeException">If the value are less
		/// than zero this exception will be thrown.</exception>
		public int HorizontalCell
		{
			get { return mHCell; }
			set
			{
				if(mHCell < 0)
					throw new ArgumentOutOfRangeException("HorizontalCell", value,
						"CellCoordinate can not have a negative value.");
				mHCell = value;
			}
		}

		/// <summary>The vertical cell</summary>
		/// <exception cref="ArgumentOutOfRangeException">If the value are less
		/// than zero this exception will be thrown.</exception>
		public int VerticalCell
		{
			get { return mVCell; }
			set
			{
				if(mVCell < 0)
					throw new ArgumentOutOfRangeException("VerticalCell", value,
						"CellCoordinate can not have a negative value.");
				mVCell = value;
			}
		}
	}
}
