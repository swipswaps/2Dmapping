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
	[PluginInfo(Name = "Path", Category = "2D.Geometry", Help = "Basic template with one value in/out", Tags = "")]
	
	#endregion PluginInfo
	public class C2D_GeometryPathNode : IPluginEvaluate
	{
		#region fields & pins
		[Input("Vertices")]
		ISpread<ISpread<Vector2D>> FVertices;

		[Input("Closed")]
		ISpread<bool> FClosed;

		[Input("FillRule", DefaultEnumEntry = "NonZero")]
		IDiffSpread<FillRule> FFillRuleEnum;		
		
		[Input("Apply", DefaultValue = 1.0, IsSingle = true) ]
		ISpread<bool> FApply;

		[Output("Path")]
		ISpread<PathGeometry> FGeometry;

		[Import()]
		ILogger FLogger;
		#endregion fields & pins

		//called when data for any output pin is requested
		public void Evaluate(int SpreadMax)
		{
			if (FApply[0]) {
				FGeometry.SliceCount = 0;
				
				for (int i=0;i<FVertices.SliceCount;i++) {
					if (FVertices[i].SliceCount > 2)
					{
						PathGeometry myPathGeometry = new PathGeometry();
					
						myPathGeometry.FillRule = FFillRuleEnum[i];
						PathFigureCollection myPathFigureCollection = new PathFigureCollection();
						myPathGeometry.Figures = myPathFigureCollection;

						PathFigure myPathFigure = new PathFigure();
						myPathFigureCollection.Add(myPathFigure);

						PathSegmentCollection myPathSegmentCollection = new PathSegmentCollection();
						myPathFigure.Segments = myPathSegmentCollection;
					
						myPathFigure.StartPoint = new Point(FVertices[i][0].x, FVertices[i][0].y);

						for (int j = 1;j<FVertices[i].SliceCount;j++) {
							LineSegment myLineSegment = new LineSegment();
							myLineSegment.Point = new Point(FVertices[i][j].x, FVertices[i][j].y);
							myPathSegmentCollection.Add(myLineSegment);						
						}
	
						myPathFigure.IsClosed = FClosed[i];

						if (FClosed[i]) {
							LineSegment myLineSegment = new LineSegment();
							myLineSegment.Point = new Point(FVertices[i][0].x, FVertices[i][0].y);
							myPathSegmentCollection.Add(myLineSegment);						
						}
					FGeometry.Add(myPathGeometry);
					}
					
				}
			}
		}
	}
}
