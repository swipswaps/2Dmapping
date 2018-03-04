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
	[PluginInfo(Name = "Area", Category = "2D.Geometry", Help = "Basic template with one value in/out", Tags = "")]
	#endregion PluginInfo
	public class C2D_GeometryAreaNode : IPluginEvaluate
	{
		#region fields & pins
		[Input("Path")]
		IDiffSpread<PathGeometry> FGeometry;


		[Output("Area")]
		ISpread<double> FArea;

		[Import()]
		ILogger FLogger;
		#endregion fields & pins

		//called when data for any output pin is requested
		public void Evaluate(int SpreadMax)
		{
			if (!FGeometry.IsChanged) return;

			FArea.SliceCount = FGeometry.SliceCount;

			for (int i = 0; i < FGeometry.SliceCount; i++) {
				try{
					FArea[i] = FGeometry[i].GetArea();
				} catch (Exception e) {
					FArea[i] = 0;
				}
				
			}
			//		FLogger.Log(LogType.Debug, myPathGeometry.GetOutlinedPathGeometry().Figures.Count.ToString());
		}
	}
}
