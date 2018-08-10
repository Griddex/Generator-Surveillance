using LiveCharts;
using LiveCharts.Wpf;
using LiveCharts.Wpf.Charts.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace Panel.BusinessLogic.ChartsLogic.GeneratorChartLogic
{
    public static class AssembleChart
    {
        public static Func<ChartPoint,string> PointLabel { get; set; }
        public static void ShowPlot(string SelectedChartType, 
                                    List<List<double>> AllOrdinateSeriesInHours,
                                    Tuple<Axis, Axis, List<Series>> AxisSeriesTuple, 
                                    Chart Chart, List<string> lstBoxSelectedStringValues)
        {
            Chart.Height = 690;
            Chart.Width = 1150;

            //PointLabel = chtPt => string.Format($"Duration: " +
            //                                    $"{lstBoxSelectedStringValues[Convert.ToInt32(chtPt.X)]}\n" +
            //                                    $"Time:" +
            //                                    $"[{chtPt.Y.ToString("N0")} hours]");

            PointLabel = chtPt => string.Format($"{chtPt.Y.ToString("N0")} hours");

            switch (SelectedChartType)
            {
                case "Column":
                    List<ColumnSeries> ColumnSeries = AxisSeriesTuple
                                                        .Item3
                                                        .Cast<ColumnSeries>()
                                                        .ToList();

                    for (int i = 0; i < AllOrdinateSeriesInHours[0].Count; i++)
                    {
                        ColumnSeries[0].Values.Add(AllOrdinateSeriesInHours[0][i]);
                    }

                    Chart.Series.Add(ColumnSeries[0]);
                    Chart.AxisY.Add(AxisSeriesTuple.Item1);
                    Chart.AxisX.Add(AxisSeriesTuple.Item2);                    
                    Chart.LegendLocation = LegendLocation.Right;

                    break;

                case "Stacked Column":
                    List<StackedColumnSeries> StackedColumnSeries = AxisSeriesTuple
                                                                        .Item3
                                                                        .Cast<StackedColumnSeries>()
                                                                        .ToList();

                    for (int i = 0; i < AllOrdinateSeriesInHours.Count(); i++)
                    {
                        
                        StackedColumnSeries.Add
                        (
                            new StackedColumnSeries
                            {
                                Values = new ChartValues<double>(),                                
                                StackMode = StackMode.Values,
                                FontSize = 14,
                                DataLabels = true,
                                LabelPoint = PointLabel
                            }
                        );
                        for (int j = 0; j < AllOrdinateSeriesInHours[i].Count; j++)
                        {
                            
                            StackedColumnSeries[i].Values.Add(
                                                    AllOrdinateSeriesInHours[i][j]);
                        }

                        if (i == 0)
                            StackedColumnSeries[i].Title = "GeneratorOn (hours)";
                        else
                            StackedColumnSeries[i].Title = "PowerOn (hours)";

                        Chart.Series.Add(StackedColumnSeries[i]);
                    }

                    Chart.AxisY.Add(AxisSeriesTuple.Item1);
                    Chart.AxisX.Add(AxisSeriesTuple.Item2);
                    Chart.LegendLocation = LegendLocation.Right;

                    break;

                case "Stacked Area":
                    //StackedAreaSeries StackedAreaSeries = (StackedAreaSeries)AxisSeriesTuple.Item2;
                    //for (int i = 0; i < AllOrdinateSeriesInHours.Count(); i++)
                    //{
                    //    foreach (var seriesValue in AllOrdinateSeriesInHours[i])
                    //    {
                    //        StackedAreaSeries.Values.Add(seriesValue);
                    //    }
                    //    Chart.Series.Add(StackedAreaSeries);
                    //}
                    //Chart.AxisX.Add(AxisSeriesTuple.Item1);
                    MessageBox.Show("Implementation coming soon", 
                                    "Information", 
                                    MessageBoxButton.OK, 
                                    MessageBoxImage.Information);
                    return;

                case "Line":
                    List<LineSeries> LineSeries = AxisSeriesTuple
                                                        .Item3
                                                        .Cast<LineSeries>()
                                                        .ToList();

                    for (int i = 0; i < AllOrdinateSeriesInHours[0].Count; i++)
                    {
                        LineSeries[0].Values.Add(AllOrdinateSeriesInHours[0][i]);
                    }

                    Chart.Series.Add(LineSeries[0]);
                    Chart.AxisY.Add(AxisSeriesTuple.Item1);
                    Chart.AxisX.Add(AxisSeriesTuple.Item2);
                    Chart.LegendLocation = LegendLocation.Right;

                    break;

                case "Pie":

                    List<Series> SeriesCollection = AxisSeriesTuple.Item3;

                    for (int i = 0; i < AllOrdinateSeriesInHours[0].Count; i++)
                    {
                        Chart.Series.Add
                        (
                            new PieSeries
                            {
                                Title = lstBoxSelectedStringValues[i],
                                DataLabels = true,
                                LabelPoint = PointLabel,
                                FontSize = 14,
                                Values = new ChartValues<double>() {
                                                AllOrdinateSeriesInHours[0][i] }
                            }
                        );
                    }

                    Chart.AxisY.Add(AxisSeriesTuple.Item1);
                    Chart.AxisX.Add(AxisSeriesTuple.Item2);
                    Chart.LegendLocation = LegendLocation.Right;

                    break;

                default:
                    break;
            }            
        }
    }
}
