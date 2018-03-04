#region usings
using System;
using System.Windows;
using System.Drawing;
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
	[PluginInfo(Name = "Change", Category = "2D.Geometry", Help = "Basic template with one value in/out", Tags = "")]
	#endregion PluginInfo
	public class C2D_GeometryChangeNode : IPluginEvaluate
	{
		#region fields & pins
		[Input("Input")]
		IDiffSpread<PathGeometry> FGeometry;

		[Output("OnChange")]
		ISpread<bool> FChange;

		[Import()]
		ILogger FLogger;
		#endregion fields & pins

		//called when data for any output pin is requested
		public void Evaluate(int SpreadMax)
		{
			FChange.SliceCount = 1;
			FChange[0] = (FGeometry.IsChanged);

			//		FLogger.Log(LogType.Debug, myPathGeometry.GetOutlinedPathGeometry().Figures.Count.ToString());
		}
	}
}
