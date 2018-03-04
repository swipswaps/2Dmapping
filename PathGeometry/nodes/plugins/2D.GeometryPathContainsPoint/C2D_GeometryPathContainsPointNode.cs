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
	[PluginInfo(Name = "PathContainsPoint", Category = "2D.Geometry", Help = "Basic template with one value in/out", Tags = "")]
	#endregion PluginInfo
	public class C2D_GeometryPathContainsPointNode : IPluginEvaluate
	{
		#region fields & pins
		[Input("Input")]
		IDiffSpread<PathGeometry> FGeometryIn;

		[Input("Point")]
		IDiffSpread<Vector2D> FPoint;

		[Input("Tolerance", DefaultValue = 0.0)]
		IDiffSpread<double> FTolerance;

		[Output("Output")]
		ISpread<bool> FContains;

		[Import()]
		ILogger FLogger;
		#endregion fields & pins

		//called when data for any output pin is requested
		public void Evaluate(int SpreadMax)
		{
			if (!(FGeometryIn.IsChanged || FPoint.IsChanged || FTolerance.IsChanged)) return;
			
			FContains.SliceCount = SpreadMax;

			Pen p = new Pen();
			p.Thickness = 0.01;
			
			for (int i = 0; i < SpreadMax; i++) {
				try {
					FContains[i] = FGeometryIn[i].StrokeContains(p, new Point(FPoint[i].x, FPoint[i].y), FTolerance[0], ToleranceType.Absolute);
				} catch (Exception e) {
					FContains[i] = false;
				}
			}
		}
	}
}
