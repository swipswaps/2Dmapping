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
	[PluginInfo(Name = "Group", Category = "2D.Geometry", Help = "Basic template with one value in/out", Tags = "")]
	#endregion PluginInfo
	public class C2D_GeometryGroupNode : IPluginEvaluate
	{
		#region fields & pins
		[Input("Input", IsPinGroup = true)]
		ISpread<ISpread<PathGeometry>> FGeometryIn;

		[Output("Output")]
		ISpread<PathGeometry> FGeometryOut;

		[Import()]
		ILogger FLogger;
		
		int PinCount = 0;
		int Count = 0;
		#endregion fields & pins

		//called when data for any output pin is requested
		public void Evaluate(int SpreadMax)
		{
			bool changed = false;
			if (PinCount == 0) changed = true;
			if (PinCount != FGeometryIn.SliceCount) {
				PinCount = FGeometryIn.SliceCount;
				changed=true;
			}
			int count = 0;
			for (int i = 0; i < PinCount; i++) {
				for (int j = 0; j < FGeometryIn[i].SliceCount; j++) {
					if (FGeometryIn[i].SliceCount > 0 && FGeometryIn[i][0] != null) {
						try {
							if (!FGeometryIn[i][j].Equals(FGeometryOut[count])) changed = true;
						}catch (Exception e) {
							changed = true;
						}
						count++;
					}
				}
			}
			if (count != Count) {
				Count = count;
				changed = true;
			}
			
			if (changed) {
				FGeometryOut.SliceCount = 0;
				for (int i = 0; i < FGeometryIn.SliceCount; i++) {
					for (int j = 0; j < FGeometryIn[i].SliceCount; j++) {
						try {
							if (FGeometryIn[i][j]!=null) FGeometryOut.Add(FGeometryIn[i][j]);
						} catch (Exception e) {
						}
					}
				}
				
			}
		}
	}
}
