using System;
using System.Collections;
using TeamYaffa.CRaPI.Utility;

namespace TeamYaffa.CRaPI.World.Fuzzy
{
	/// <summary>
	/// This class handles positioning of dynamic objects.
	/// </summary>
	public class Positioning
	{
		//remove if no good use found
		public Positioning()
		{
		}

		/// <summary>
		/// Calculates the position of an object from the supplied intersections
		/// </summary>
		/// <param name="pIntersections">An ArrayList of intersections</param>
		/// <returns>The position of an object</returns>
		public Point2D CalculatePosition(ArrayList pIntersections)
		{
			//1. Creates 2 matrices for each intersection point
			ArrayList matrices = new ArrayList();
			foreach(Intersection isp in pIntersections)
			{
				Flag f1 = isp.FlagOne;
				Flag f2 = isp.FlagTwo;
                double resolution = WorldModel.RESOLUTION;
				double noise = WorldModel.NOISE;
			//	double maxDistance = Math.Sqrt(Math.Pow(115, 2) + Math.Pow(68, 2));
			//	int width = (int)((2 * noise * maxDistance) / resolution);
			
				double distance = Math.Max(f1.Distance, f2.Distance);
				int width = (int)((2 * noise * distance) / resolution);

				//if even, add 1
				if((width % 2) == 0)
				{
					width++;
				}

				int height = width;
				//Console.WriteLine("width: " + width);
				//Console.WriteLine("height: " + height);
				Console.ReadLine();
				Matrix m1 = new Matrix(width, height);
				Matrix m2 = new Matrix(width, height);
				
				//create cones
				CalculateCone(m1, f1.Distance);
				CalculateCone(m2, f2.Distance);
				
				/////////
				Console.WriteLine("---------Matrices-------");
				Console.WriteLine(m1);
				Console.WriteLine(m2);
				/////////

				//2. Calculate max of the pairs
				PositioningMatrix res = new PositioningMatrix(m1 + m2);
			//	Console.WriteLine("width: " + res.Width);
			//	Console.WriteLine("height: " + res.Height);
				res.Intersection = isp;

				/////////
				Console.WriteLine("\n---------Result matrix-------");
				Console.WriteLine(res);
				/////////
				
				matrices.Add(res);
			}
			//3. Take the point corresponding to the cell with the highest peak
			Matrix resultMatrix = createResultMatrix(matrices);
			Point2D point = getIntersection(resultMatrix);
			return point;
		}

		/// <summary>
		/// Returns the point corresponding to the cell with the highest value in the supplied matrix
		/// </summary>
		/// <param name="pMatrix">A matrix</param>
		/// <returns>a point</returns>
		private Point2D getIntersection(Matrix pMatrix)
		{ //TODO: start here and enjoy life
			return new Point2D(0,0);
		}

		/// <summary>
		/// Creates a matrix as the result of superimposing all sub-matrices
		/// </summary>
		/// <param name="pMatrices">An ArrayList of matrices</param>
		/// <returns>the resulting matrix</returns>
		private Matrix createResultMatrix(ArrayList pMatrices)
		{
			//1. Calculate size of result matrix, based on top left and lower right matrix
			int topX = int.MaxValue;
			int topY = int.MaxValue;
			int bottomX = int.MinValue;
			int bottomY = int.MinValue;
			PositioningMatrix top = null;
			PositioningMatrix bottom = null;

			foreach(PositioningMatrix pm in pMatrices)
			{
				double curX = pm.Intersection.Point.X;
				double curY = pm.Intersection.Point.Y;

				if((curX <= topX) && (curY <= topY))
					top = pm;
				
				if((curX >= bottomX) && (curY >= bottomY))
					bottom = pm;
			}
			int width = (int)((top.Intersection.Point.XDistanceTo(bottom.Intersection.Point)) / WorldModel.RESOLUTION);
			int height = (int)((top.Intersection.Point.YDistanceTo(bottom.Intersection.Point)) / WorldModel.RESOLUTION);
			width += (int)((top.Width/2.0) + (bottom.Width/2.0));
			height += (int)((top.Height/2.0) + (bottom.Height/2.0));
			Matrix resmat = new Matrix(width, height);
			
			//2. Superimpose all matrices on the result matrix
			foreach(PositioningMatrix pm in pMatrices)
			{
				int xOff = (int)((pm.Intersection.Point.XDistanceTo(top.Intersection.Point)) / WorldModel.RESOLUTION);
				int yOff = (int)((pm.Intersection.Point.YDistanceTo(top.Intersection.Point)) / WorldModel.RESOLUTION);
				resmat.SuperimposeMin(pm, xOff, yOff);
			}
			Console.WriteLine("------------- SUPERIMPOSED-------------\n" + resmat);
			return resmat;
		}

		/// <summary>
		/// Calculates values for each cell according to a cone distribution
		/// </summary>
		/// <param name="pMatrix">The matrix to build the cone in</param>
		/// <param name="pDistance">The distance to the flag corresponding to the matrix</param>
		private void CalculateCone(Matrix pMatrix, double pDistance)
		{
			Matrix m = pMatrix;
			double noise = pDistance * WorldModel.NOISE;
			double resolution = WorldModel.RESOLUTION;
			double peak = 1 / noise;
			int centerX = (int)(Math.Ceiling(((double)m.Width)/2.0)) - 1;
			int centerY = (int)(Math.Ceiling(((double)m.Height)/2.0)) - 1;

			for(int y = 0; y < m.Height; y++)
			{
				for(int x = 0; x < m.Width; x++)
				{
					int xDist = centerX - x;
					int yDist = centerY - y;
					double distance = (Math.Sqrt(Math.Pow(xDist, 2) + Math.Pow(yDist, 2))) * resolution;
					double val = (peak *(noise-distance)) / noise;
					if(val < 0)
					{
						val = 0;
					}
					m[x,y] = val;
				}
			}
		}
	}
}
