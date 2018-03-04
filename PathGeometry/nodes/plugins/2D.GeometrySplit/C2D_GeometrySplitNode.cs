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
	[PluginInfo(Name = "Split", Category = "2D.Geometry", Help = "Basic template with one value in/out", Tags = "")]
	#endregion PluginInfo
	public class C2D_GeometrySplitNode : IPluginEvaluate
	{
		#region fields & pins
		[Input("Path")]
		IDiffSpread<PathGeometry> FGeometry;

		
		[Output("Vertices")]
		ISpread<ISpread<Vector2D>> FVertices;

		[Output("Closed")]
		ISpread<bool> FClosed;

		[Output("FillRule")]
		ISpread<FillRule> FFillRuleEnum;	

		[Import()]
		ILogger FLogger;
		#endregion fields & pins

		//called when data for any output pin is requested
		public void Evaluate(int SpreadMax)
		{
			
			if (!FGeometry.IsChanged) return;
			int currentCount = 0;
			try {	
				for (int i = 0; i < FGeometry.SliceCount; i++) {
					PathGeometry myPathGeometry = FGeometry[i];
					PathFigureCollection myPathFigureCollection = myPathGeometry.Figures;
					
					if (myPathFigureCollection == null || myPathFigureCollection.Count == 0) {
						currentCount++;
					}
					else {
						for (int j = 0;j<myPathFigureCollection.Count;j++) {
							currentCount++;
						}
					}
				}
			} catch (Exception e) {
//				FLogger.Log(LogType.Debug, "Error "+e.ToString());
				
			}
				
//				FLogger.Log(LogType.Debug, "count "+currentCount.ToString());
				FClosed.SliceCount = FVertices.SliceCount = FFillRuleEnum.SliceCount = currentCount;
				 
				currentCount = 0;
				for (int i = 0; i < FVertices.SliceCount; i++) {
					PathGeometry myPathGeometry = FGeometry[i];
					PathFigureCollection myPathFigureCollection = myPathGeometry.Figures;
					for (int j = 0;j<myPathFigureCollection.Count;j++) {
						PathFigure myPathFigure = myPathFigureCollection[j];

						PathSegmentCollection myPathSegmentCollection = myPathFigure.Segments;

						// Startpoint
						FVertices[currentCount].SliceCount = 1;
						FVertices[currentCount][0] = new Vector2D(myPathFigure.StartPoint.X, myPathFigure.StartPoint.Y);
					
						for (int k = 0; k < myPathSegmentCollection.Count; k++) {
//						FLogger.Log(LogType.Debug, myPathSegment.GetType().ToString());
						
							if (myPathSegmentCollection[k].GetType().ToString() == "System.Windows.Media.PolyLineSegment") {
								PolyLineSegment myPathSegment = (PolyLineSegment)myPathSegmentCollection[k];

								for (int l=0;l<myPathSegment.Points.Count;l++) {
									FVertices[currentCount].Add(new Vector2D(myPathSegment.Points[l].X, myPathSegment.Points[l].Y));
								}
							}
							if (myPathSegmentCollection[k].GetType().ToString() == "System.Windows.Media.LineSegment") {
								LineSegment myPathSegment = (LineSegment)myPathSegmentCollection[k];
								FVertices[currentCount].Add(new Vector2D(myPathSegment.Point.X, myPathSegment.Point.Y));
							}
						}
							
						FClosed[currentCount] = myPathFigure.IsClosed;
						FFillRuleEnum[currentCount] = (myPathGeometry.FillRule);
						currentCount++;					
						
					}
				}
			
			//		
		}
	}
}
