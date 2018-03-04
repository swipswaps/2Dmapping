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
	[PluginInfo(Name = "Unify", Category = "2D.Geometry", Help = "Basic template with one value in/out", Tags = "")]
	#endregion PluginInfo
	public class C2D_GeometryUnifyNode : IPluginEvaluate
	{
		#region fields & pins
		[Input("Input 1")]
		IDiffSpread<PathGeometry> FGeometryIn;

		[Input("Input 2")]
		IDiffSpread<PathGeometry> FGeometryIn2;

		[Output("Output")]
		ISpread<PathGeometry> FGeometryOut;

		[Import()]
		ILogger FLogger;
		#endregion fields & pins

		//called when data for any output pin is requested
		public void Evaluate(int SpreadMax)
		{
			if (!(FGeometryIn.IsChanged || FGeometryIn2.IsChanged)) return;

			FGeometryOut.SliceCount = 0;
			for (int i=0;i<SpreadMax;i++) {
				try {

					PathGeometry geom =  PathGeometry.Combine(FGeometryIn[i], FGeometryIn2[i], GeometryCombineMode.Union, null );
					
					if (geom.Figures == null || geom.Figures.Count == 0) {
						FGeometryOut[i] = new PathGeometry();
					}
					
					for (int j = 0;j<geom.Figures.Count;j++) {
						PathGeometry g = new PathGeometry();
						g.Figures = new PathFigureCollection();						
						g.Figures.Add(geom.Figures[j]);
						FGeometryOut.Add(g);
//						FLogger.Log(LogType.Debug, "uni count" + i.ToString() + " " +j.ToString());
					}				
					
				} catch (Exception e) {
					FLogger.Log(LogType.Error, e.ToString());
				}
			}			
		}
	}
}
