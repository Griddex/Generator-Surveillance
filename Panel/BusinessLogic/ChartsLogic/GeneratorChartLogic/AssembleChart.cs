using LiveCharts.Wpf;
using LiveCharts.Wpf.Charts.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Panel.BusinessLogic.ChartsLogic.GeneratorChartLogic
{
    public static class AssembleChart
    {
        public static void ShowPlot(string SelectedChartType, List<List<double>> AllOrdinateSeriesInHours,
                                    Tuple<Axis, List<Series>> AxisSeriesTuple, Chart Chart)
        {
            Chart.Height = 690;
            Chart.Width = 1150;

            switch (SelectedChartType)
            {
                case "Column":
                    List<ColumnSeries> ColumnSeries = AxisSeriesTuple.Item2.Cast<ColumnSeries>().ToList();
                    for (int i = 0; i < AllOrdinateSeriesInHours[0].Count; i++)
                    {
                        ColumnSeries[0].Values.Add(AllOrdinateSeriesInHours[0][i]);
                    }
                    Chart.Series.Add(ColumnSeries[0]);                   
                    Chart.AxisX.Add(AxisSeriesTuple.Item1);
                    break;

                case "Stacked Column":
                    List<StackedColumnSeries> StackedColumnSeries = AxisSeriesTuple.Item2.Cast<StackedColumnSeries>().ToList();
                    for (int i = 0; i < AllOrdinateSeriesInHours.Count(); i++)
                    {
                        for (int j = 0; j < AllOrdinateSeriesInHours[i].Count; j++)
                        {
                            StackedColumnSeries[i].Values.Add(AllOrdinateSeriesInHours[i][j]);
                        }
                        Chart.Series.Add(StackedColumnSeries[i]);
                    }
                    Chart.AxisX.Add(AxisSeriesTuple.Item1);
                    break;

                //case "Stacked Area":
                //    StackedAreaSeries StackedAreaSeries = (StackedAreaSeries)AxisSeriesTuple.Item2;
                //    for (int i = 0; i < AllOrdinateSeriesInHours.Count(); i++)
                //    {
                //        foreach (var seriesValue in AllOrdinateSeriesInHours[i])
                //        {
                //            StackedAreaSeries.Values.Add(seriesValue);
                //        }
                //        Chart.Series.Add(StackedAreaSeries);
                //    }
                //    Chart.AxisX.Add(AxisSeriesTuple.Item1);
                //    break;

                //case "Line":
                //    LineSeries LineSeries = (LineSeries)AxisSeriesTuple.Item2;
                //    for (int i = 0; i < AllOrdinateSeriesInHours.Count(); i++)
                //    {
                //        foreach (var seriesValue in AllOrdinateSeriesInHours[i])
                //        {
                //            LineSeries.Values.Add(seriesValue);
                //        }
                //        Chart.Series.Add(LineSeries);
                //    }
                //    Chart.AxisX.Add(AxisSeriesTuple.Item1);
                //    break;

                //case "Pie":
                //    PieSeries PieSeries = (PieSeries)AxisSeriesTuple.Item2;
                //    for (int i = 0; i < AllOrdinateSeriesInHours.Count(); i++)
                //    {
                //        foreach (var seriesValue in AllOrdinateSeriesInHours[i])
                //        {
                //            PieSeries.Values.Add(seriesValue);
                //        }
                //        Chart.Series.Add(PieSeries);
                //    }
                //    Chart.AxisX.Add(AxisSeriesTuple.Item1);
                //    break;

                default:
                    break;
            }            
        }
    }
}
