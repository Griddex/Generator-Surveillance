using LiveCharts;
using LiveCharts.Wpf;
using LiveCharts.Wpf.Charts.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Panel.BusinessLogic.ChartsLogic.GeneratorChartLogic
{
    public static class AssembleChart
    {
        public static Func<ChartPoint,string> PointLabel { get; set; }
        public static void ShowPlot(string SelectedChartType, List<List<double>> AllOrdinateSeriesInHours,
                                    Tuple<Axis, List<Series>> AxisSeriesTuple, Chart Chart, List<string> lstBoxSelectedStringValues)
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
                        StackedColumnSeries.Add
                        (
                            new StackedColumnSeries
                            {
                                Values = new ChartValues<double>(),
                                StackMode = StackMode.Values,
                                DataLabels = true
                            }
                        );
                        for (int j = 0; j < AllOrdinateSeriesInHours[i].Count; j++)
                        {
                            
                            StackedColumnSeries[i].Values.Add(AllOrdinateSeriesInHours[i][j]);
                        }
                        Chart.Series.Add(StackedColumnSeries[i]);
                    }
                    Chart.AxisX.Add(AxisSeriesTuple.Item1);
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
                    MessageBox.Show("Implementation coming soon", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;

                case "Line":
                    List<LineSeries> LineSeries = AxisSeriesTuple.Item2.Cast<LineSeries>().ToList();
                    for (int i = 0; i < AllOrdinateSeriesInHours[0].Count; i++)
                    {
                        LineSeries[0].Values.Add(AllOrdinateSeriesInHours[0][i]);
                    }
                    Chart.Series.Add(LineSeries[0]);
                    Chart.AxisX.Add(AxisSeriesTuple.Item1);
                    break;

                case "Pie":
                    PointLabel = chtPt => string.Format($"{chtPt.Y.ToString("N")} hours");
                    List<Series> SeriesCollection = AxisSeriesTuple.Item2;
                    for (int i = 0; i < AllOrdinateSeriesInHours[0].Count; i++)
                    {
                        Chart.Series.Add
                        (
                            new PieSeries
                            {
                                Title = lstBoxSelectedStringValues[i],
                                DataLabels = true,
                                LabelPoint = PointLabel,
                                Values = new ChartValues<double>() { AllOrdinateSeriesInHours[0][i] }
                            }
                        );
                    }
                    Chart.AxisX.Add(AxisSeriesTuple.Item1);
                    Chart.LegendLocation = LegendLocation.Bottom;
                    break;

                default:
                    break;
            }            
        }
    }
}
