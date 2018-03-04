#region usings
using System;
using System.ComponentModel.Composition;

using VVVV.PluginInterfaces.V1;
using VVVV.PluginInterfaces.V2;
using VVVV.Utils.VColor;
using VVVV.Utils.VMath;

using VVVV.Core.Logging;
using VVVV.Lib;
#endregion usings

namespace VVVV.Nodes
{
	#region PluginInfo
	[PluginInfo(Name = "TesselatePolygon", Category = "EX9.Geometry", Help = "Basic template with 2 dimensional spread with bin size input", Tags = "")]
	#endregion PluginInfo
	public class EX9_GeometryTesselatePolygonNode : IPluginEvaluate
	{
		#region fields & pins
		[Input("Vertices", DefaultValue = 1.0)]
		ISpread<ISpread<Vector2D>> FVerticesInput;

		[Input("Apply", DefaultValue = 1.0, IsBang = true)]
		ISpread<bool> FApply;

		[Output("Indices")]
		ISpread<ISpread<int>> FIndicesOutput;

		[Import()]
		ILogger Flogger;
		#endregion fields & pins

		//called when data for any output pin is requested
		public void Evaluate(int SpreadMax)
		{
			bool changeAll = false;
			if (FIndicesOutput.SliceCount != FVerticesInput.SliceCount) {
				FIndicesOutput.SliceCount = FVerticesInput.SliceCount;
				changeAll = true;
			}

			for (int i=0;i<FVerticesInput.SliceCount;i++) {
				if (changeAll || FApply[i]) {
					Triangulator t = new Triangulator(FVerticesInput[i]);

					int[] indices = t.Triangulate();
				
					FIndicesOutput[i].SliceCount = indices.Length;
					for (int j=0;j<indices.Length;j++) {
						FIndicesOutput[i][j] = indices[j];
					
					}
					
				}
			}
		}
	}
}
