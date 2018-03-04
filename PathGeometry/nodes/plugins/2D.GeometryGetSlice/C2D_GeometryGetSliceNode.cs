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
	[PluginInfo(Name = "GetSlice", Category = "2D.Geometry", Help = "Basic template with one value in/out", Tags = "")]
	#endregion PluginInfo
	public class C2D_GeometryGetSliceNode : IPluginEvaluate
	{
		#region fields & pins
		[Input("Input Node")]
		IDiffSpread<PathGeometry> FGeometryIn;

		[Input("Index")]
		IDiffSpread<int> FIndex;

		[Output("Output")]
		ISpread<PathGeometry> FGeometryOut;

		[Import()]
		ILogger FLogger;
		#endregion fields & pins

		//called when data for any output pin is requested
		public void Evaluate(int SpreadMax)
		{
			if (!(FGeometryIn.IsChanged || FIndex.IsChanged)) return;
			
			FGeometryOut.SliceCount = 0;
			for (int i = 0; i < FIndex.SliceCount; i++) {
				try {
					FGeometryOut.Add(FGeometryIn[FIndex[i]]);
				} catch (Exception e) {

				}
			}
		}
	}
}
