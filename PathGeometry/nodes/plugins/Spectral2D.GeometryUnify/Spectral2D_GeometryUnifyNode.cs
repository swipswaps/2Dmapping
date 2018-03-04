#region usings
using System;
using System.Windows;
using System.ComponentModel.Composition;

using VVVV.PluginInterfaces.V1;
using VVVV.PluginInterfaces.V2;
using VVVV.Utils.VColor;
using VVVV.Utils.VMath;

using VVVV.Core.Logging;

using System.Windows.Media;
#endregion usings

namespace VVVV.Nodes
{
	#region PluginInfo
	[PluginInfo(Name = "Unify", 
				Category = "2D.Geometry", 
				Version = "Spectral", 
				Help = "", 
				Tags = "")]
	#endregion PluginInfo
	public class Spectral2D_GeometryUnifyNode : IPluginEvaluate
	{
		#region fields & pins
		[Input("Input")]
		public IDiffSpread<ISpread<PathGeometry>> FGeometryIn;

		[Output("Output")]
		public ISpread<PathGeometry> FGeometryOut;

		[Import()]
		ILogger FLogger;
		#endregion fields & pins

		//called when data for any output pin is requested
		public void Evaluate(int SpreadMax)
		{
			if (!FGeometryIn.IsChanged)
				return;

			FGeometryOut.SliceCount = FGeometryIn.SliceCount;
			for (int i = 0; i < FGeometryIn.SliceCount; i++) 
			{
				var geom = new PathGeometry();
				
				for (int j = 0; j < FGeometryIn[i].SliceCount; j++)
				{
					try 
					{
						geom = PathGeometry.Combine(geom, FGeometryIn[i][j], GeometryCombineMode.Union, null);
	
						if (geom.Figures == null || geom.Figures.Count == 0) 
						{
							FGeometryOut[i] = new PathGeometry();
						}
	
						var g = new PathGeometry();
						for (int k = 0; k < geom.Figures.Count; k++) 
						{
							g.Figures = new PathFigureCollection();
							g.Figures.Add(geom.Figures[k]);
//							FLogger.Log(LogType.Debug, "uni count" + i.ToString() + " " +j.ToString());
						}
						FGeometryOut[i] = g;
					} 
					catch (Exception e) 
					{
						FLogger.Log(LogType.Error, e.ToString());
					}
				}
			}
		}
	}
}
