#region usings
using System;
using System.Windows;
using System.ComponentModel.Composition;
using System.Collections.Generic;

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
	[PluginInfo(Name = "Queue", Category = "2D.Geometry", Help = "Basic template with one value in/out", Tags = "")]
	#endregion PluginInfo
	public class C2D_GeometryQueueNode : IPluginEvaluate
	{
		#region fields & pins
		[Input("Input")]
		ISpread<PathGeometry> FGeometryIn;

		[Input("doInsert", DefaultValue = 0.0, IsSingle = true, IsBang = true)]
		ISpread<bool> FInsert;

		[Input("Frame Count", DefaultValue = 1.0, IsSingle = true)]
		IDiffSpread<int> FCount;

		[Output("Output")]
		ISpread<PathGeometry> FGeometryOut;

		[Output("BinSize")]
		ISpread<int> FBinSize;

		[Import()]
		ILogger FLogger;
		
		List<ISpread<PathGeometry>> buffer = new List<ISpread<PathGeometry>>();
		int ringPointer = 0;
		#endregion fields & pins

		//called when data for any output pin is requested
		public void Evaluate(int SpreadMax)
		{
			if (FCount.IsChanged) {
				ringPointer = 0;
				buffer.Clear();
				for (int i=0;i<FCount[0];i++) buffer.Add(new Spread<PathGeometry>(0));
			}
			 
			if (FInsert[0]) {
				buffer[ringPointer] = FGeometryIn.Clone();
			
				for (int i=0;i<FGeometryIn.SliceCount;i++) {
					buffer[ringPointer].SliceCount = 0;
					buffer[ringPointer].Add(FGeometryIn[i].Clone());
				}
				ringPointer = (ringPointer+1)%FCount[0];
				
				FGeometryOut.SliceCount = FBinSize.SliceCount = 0;
				
				for (int i=0;i<FCount[0];i++) {
					for (int j=0;j<buffer[(ringPointer+i)%FCount[0]].SliceCount;j++) 
					{
						FGeometryOut.Add(buffer[(ringPointer+i)%FCount[0]][j]);
					}
					FBinSize.Add(buffer[(ringPointer+i)%FCount[0]].SliceCount);
				}
			}
		}
	}
}
