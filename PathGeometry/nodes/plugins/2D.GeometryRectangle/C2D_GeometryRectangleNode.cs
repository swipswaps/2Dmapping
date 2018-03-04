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
	[PluginInfo(Name = "Rectangle", Category = "2D.Geometry", Help = "Basic template with one value in/out", Tags = "")]

	#endregion PluginInfo
	public class C2D_GeometryRectangleNode : IPluginEvaluate
	{
		#region fields & pins

		[Input("Center")]
		IDiffSpread<Vector2D> FCenter;

		[Input("Width", DefaultValue = 0.5)]
		IDiffSpread<double> FWidth;

		[Input("Height", DefaultValue = 0.5)]
		IDiffSpread<double> FHeight;
		
		[Input("Radius 1", DefaultValue = 0.0)]
		IDiffSpread<double> FRadius1;

		[Input("Radius 2", DefaultValue = 0.0)]
		IDiffSpread<double> FRadius2;


		[Input("Resolution", DefaultValue = 0.0001)]
		IDiffSpread<double> FResolution;

		[Output("Path")]
		ISpread<PathGeometry> FGeometry;

		[Import()]
		ILogger FLogger;
		#endregion fields & pins

		//called when data for any output pin is requested
		public void Evaluate(int SpreadMax)
		{
			if (FCenter.IsChanged || FWidth.IsChanged || FHeight.IsChanged || FRadius1.IsChanged || FRadius2.IsChanged || FResolution.IsChanged) {
				FGeometry.SliceCount = SpreadMax;
				for (int i = 0; i < SpreadMax; i++)
					FGeometry[i] = new RectangleGeometry(new Rect(FCenter[i].x-FWidth[i], FCenter[i].y-FHeight[i], FWidth[i]*2, FHeight[i]*2), FRadius1[i], FRadius2[i]).GetFlattenedPathGeometry(0.001 / FResolution[i], ToleranceType.Absolute);
	
			//		FLogger.Log(LogType.Debug, myPathGeometry.GetOutlinedPathGeometry().Figures.Count.ToString());
			}
			
		}
	}
}
