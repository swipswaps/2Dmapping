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
	[PluginInfo(Name = "Contains", Category = "2D.Geometry", Help = "Basic template with one value in/out", Tags = "")]
	#endregion PluginInfo
	public class C2D_GeometryContainsNode : IPluginEvaluate
	{
		#region fields & pins
		[Input("Input 1")]
		IDiffSpread<PathGeometry> FGeometryIn;

		[Input("Input 2")]
		IDiffSpread<PathGeometry> FGeometryIn2;

		[Input("Tolerance", DefaultValue = 0.0)]
		IDiffSpread<double> FTolerance;

		[Output("FullyInside")]
		ISpread<bool> FFullyInside;

		[Output("FullyContained")]
		ISpread<bool> FFullyContained;

		[Output("Intersects")]
		ISpread<bool> FIntersects;


		[Import()]
		ILogger FLogger;
		#endregion fields & pins

		//called when data for any output pin is requested
		public void Evaluate(int SpreadMax)
		{
			if (!(FGeometryIn.IsChanged || FGeometryIn2.IsChanged || FTolerance.IsChanged)) return;

			FFullyInside.SliceCount = FFullyContained.SliceCount = FIntersects.SliceCount = SpreadMax;

			for (int i = 0; i < SpreadMax; i++) {
				try {
					IntersectionDetail detail = FGeometryIn[i].FillContainsWithDetail(FGeometryIn2[i], FTolerance[i], ToleranceType.Absolute);
					
					
					FFullyInside[i] = (detail == IntersectionDetail.FullyInside);
					FFullyContained[i] = (detail == IntersectionDetail.FullyContains);
					FIntersects[i] = (detail == IntersectionDetail.Intersects);
				} catch (Exception e) {
					FFullyInside[i] = false;
					FFullyContained[i] = false;
					FIntersects[i] = false;
				}	
			}
		}
	}
}
