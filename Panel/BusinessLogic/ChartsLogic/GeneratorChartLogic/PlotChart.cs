using LiveCharts.Wpf;
using LiveCharts.Wpf.Charts.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Panel.BusinessLogic.ChartsLogic.GeneratorChartLogic
{
    public static class PlotChart
    {
        public static void ShowPlot(string SelectedChartType, List<List<double>> AllOrdinateSeriesInHours,
                                    Tuple<Axis, Series> AxisSeriesTuple, Chart Chart)
        {
            switch (SelectedChartType)
            {
                case "Column":
                    ColumnSeries ColumnSeries = (ColumnSeries)AxisSeriesTuple.Item2;                    
                    for (int i = 0; i < AllOrdinateSeriesInHours.Count(); i++)
                    {
                        foreach (var seriesValue in AllOrdinateSeriesInHours[i])
                        {
                            ColumnSeries.Values.Add(seriesValue);
                        }
                        Chart.Series.Add(ColumnSeries);
                    }
                    Chart.AxisX.Add(AxisSeriesTuple.Item1);
                    break;

                case "Stacked Column":
                    StackedColumnSeries StackedColumnSeries = (StackedColumnSeries)AxisSeriesTuple.Item2;
                    for (int i = 0; i < AllOrdinateSeriesInHours.Count(); i++)
                    {
                        foreach (var seriesValue in AllOrdinateSeriesInHours[i])
                        {
                            StackedColumnSeries.Values.Add(seriesValue);
                        }
                        Chart.Series.Add(StackedColumnSeries);
                    }
                    Chart.AxisX.Add(AxisSeriesTuple.Item1);
                    break;

                case "Stacked Area":
                    StackedAreaSeries StackedAreaSeries = (StackedAreaSeries)AxisSeriesTuple.Item2;
                    for (int i = 0; i < AllOrdinateSeriesInHours.Count(); i++)
                    {
                        foreach (var seriesValue in AllOrdinateSeriesInHours[i])
                        {
                            StackedAreaSeries.Values.Add(seriesValue);
                        }
                        Chart.Series.Add(StackedAreaSeries);
                    }
                    Chart.AxisX.Add(AxisSeriesTuple.Item1);
                    break;

                case "Line":
                    LineSeries LineSeries = (LineSeries)AxisSeriesTuple.Item2;
                    for (int i = 0; i < AllOrdinateSeriesInHours.Count(); i++)
                    {
                        foreach (var seriesValue in AllOrdinateSeriesInHours[i])
                        {
                            LineSeries.Values.Add(seriesValue);
                        }
                        Chart.Series.Add(LineSeries);
                    }
                    Chart.AxisX.Add(AxisSeriesTuple.Item1);
                    break;

                case "Pie":
                    PieSeries PieSeries = (PieSeries)AxisSeriesTuple.Item2;
                    for (int i = 0; i < AllOrdinateSeriesInHours.Count(); i++)
                    {
                        foreach (var seriesValue in AllOrdinateSeriesInHours[i])
                        {
                            PieSeries.Values.Add(seriesValue);
                        }
                        Chart.Series.Add(PieSeries);
                    }
                    Chart.AxisX.Add(AxisSeriesTuple.Item1);
                    break;

                default:
                    break;
            }            
        }
    }
}
