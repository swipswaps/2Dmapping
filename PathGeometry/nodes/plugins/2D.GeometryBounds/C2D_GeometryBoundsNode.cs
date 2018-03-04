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
	[PluginInfo(Name = "Bounds", Category = "2D.Geometry", Help = "Basic template with one value in/out", Tags = "")]
	#endregion PluginInfo
	public class C2D_GeometryBoundsNode : IPluginEvaluate
	{
		#region fields & pins
		[Input("Path")]
		IDiffSpread<PathGeometry> FGeometry;


		[Output("LeftTop")]
		ISpread<Vector2D> FLT;

		[Output("RightBottom")]
		ISpread<Vector2D> FRB;

		[Output("Center")]
		ISpread<Vector2D> FCenter;

		[Output("WidthHeight")]
		ISpread<Vector2D> FWidth;

		[Import()]
		ILogger FLogger;
		#endregion fields & pins

		//called when data for any output pin is requested
		public void Evaluate(int SpreadMax)
		{

			if (!FGeometry.IsChanged) return;
			
			FLT.SliceCount = FRB.SliceCount = FCenter.SliceCount = FWidth.SliceCount =  FGeometry.SliceCount;

			for (int i = 0; i < FGeometry.SliceCount; i++) {
				Rect b;
				try {
					b = FGeometry[i].Bounds;
				
					
				} catch (Exception e) {
					b = new Rect();
				}
				FLT[i] = new Vector2D(b.Left, b.Top);
				FRB[i] = new Vector2D(b.Right, b.Bottom);
				FCenter[i] = new Vector2D((b.Left-b.Right)/2, (b.Top-b.Bottom)/2);
				FWidth[i] = new Vector2D(b.Size.Width, b.Size.Height);
			}
			//		FLogger.Log(LogType.Debug, myPathGeometry.GetOutlinedPathGeometry().Figures.Count.ToString());
		}
	}
}
