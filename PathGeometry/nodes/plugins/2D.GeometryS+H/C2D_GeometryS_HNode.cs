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
using System.Collections.Generic;
#endregion usings

namespace VVVV.Nodes
{
	#region PluginInfo
	[PluginInfo(Name = "S+H", Category = "2D.Geometry", Help = "Basic template with one value in/out", Tags = "")]
	#endregion PluginInfo
	public class C2D_GeometryS_HNode : IPluginEvaluate
	{
		#region fields & pins
		[Input("Input Node")]
		ISpread<PathGeometry> FGeometryIn;

		[Input("Set")]
		ISpread<bool> FSet;

		[Output("Output")]
		ISpread<PathGeometry> FGeometryOut;

		[Import()] 
		ILogger FLogger;

		#endregion fields & pins

		//called when data for any output pin is requested
		public void Evaluate(int SpreadMax)
		{
			FGeometryOut.SliceCount = SpreadMax;
			for (int i = 0; i < SpreadMax; i++) {
				if (FSet[i]) {
					FGeometryOut[i] = FGeometryIn[i].Clone();
				}
			}
		}
	}
}
