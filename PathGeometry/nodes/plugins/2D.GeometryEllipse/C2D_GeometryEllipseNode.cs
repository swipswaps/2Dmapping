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
	[PluginInfo(Name = "Ellipse", Category = "2D.Geometry", Help = "Basic template with one value in/out", Tags = "")]

	#endregion PluginInfo
	public class C2D_GeometryEllipseNode : IPluginEvaluate
	{
		#region fields & pins

		[Input("Center")]
		IDiffSpread<Vector2D> FCenter; 

		[Input("Radius 1", DefaultValue = 0.5)]
		IDiffSpread<double> FRadius1;

		[Input("Radius 2", DefaultValue = 0.5)]
		IDiffSpread<double> FRadius2;

		[Input("Resolution", DefaultValue = 0.5)]
		IDiffSpread<double> FResolution;
		
		[Output("Path")]
		ISpread<PathGeometry> FGeometry;

		[Import()]
		ILogger FLogger;
		#endregion fields & pins

		//called when data for any output pin is requested
		public void Evaluate(int SpreadMax)
		{
			
			if (FCenter.IsChanged || FRadius1.IsChanged || FRadius2.IsChanged || FResolution.IsChanged) {
				FGeometry.SliceCount = SpreadMax;
				for (int i=0;i<SpreadMax;i++)			
						FGeometry[i] = new EllipseGeometry(new Point(FCenter[i].x, FCenter[i].y), FRadius1[i],FRadius2[i]).GetFlattenedPathGeometry(0.001/FResolution[i], ToleranceType.Relative);

			//		FLogger.Log(LogType.Debug, myPathGeometry.GetOutlinedPathGeometry().Figures.Count.ToString());
			}
			
		}
	}
}
