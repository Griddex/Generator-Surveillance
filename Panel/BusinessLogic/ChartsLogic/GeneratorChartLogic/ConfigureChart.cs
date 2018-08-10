using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;

namespace Panel.BusinessLogic.ChartsLogic.GeneratorChartLogic
{
    public static class ConfigureChart
    {
        public static Tuple<Axis, Axis, List<Series>> ConfigureChartByChartType(
                                    string SeriesChartType, 
                                    List<string> lstBoxSelectedStringValues)
        {
            DefaultTooltip defaultTooltip = new DefaultTooltip();
            defaultTooltip.SelectionMode = TooltipSelectionMode.OnlySender;

            Axis AbscissaAxis = new Axis()
            {
                Separator = new Separator() { Step = 1, IsEnabled = false },
                Title = "Date",
                Labels = lstBoxSelectedStringValues,
                LabelsRotation = 20,
                FontSize = 16
            };

            Axis OrdinateAxis = new Axis()
            {
                Title = "Time Duration (hrs)",
                FontSize = 16,
                LabelFormatter = v => v.ToString("N0")
            };

            switch (SeriesChartType)
            {
                case "Column":
                    ColumnSeries columnSeries = new ColumnSeries()
                    {
                        Title = "GeneratorOn (hrs)",
                        DataLabels = true,
                        Values = new ChartValues<double>()
                    };
                    List<Series> columnSeriesList = new List<Series>();
                    columnSeriesList.Add(columnSeries);

                    return new Tuple<Axis, Axis, List<Series>>(
                        OrdinateAxis,
                        AbscissaAxis, 
                        columnSeriesList);

                case "Stacked Column":
                    StackedColumnSeries stackedColumnSeries = new StackedColumnSeries()
                    {
                        DataLabels = true,
                        Values = new ChartValues<double>()
                    };
                    List<Series> stackedColumnSeriesList = new List<Series>();
                    stackedColumnSeriesList.Add(stackedColumnSeries);

                    return new Tuple<Axis, Axis, List<Series>>(
                        OrdinateAxis,
                        AbscissaAxis, 
                        stackedColumnSeriesList);

                case "Stacked Area":
                    StackedAreaSeries stackedAreaSeries = new StackedAreaSeries()
                    {
                        DataLabels = true,
                        Values = new ChartValues<DateTimePoint>()
                    };
                    List<Series> stackedAreaSeriesList = new List<Series>();
                    stackedAreaSeriesList.Add(stackedAreaSeries);

                    return new Tuple<Axis, Axis, List<Series>>(
                        OrdinateAxis,
                        AbscissaAxis, 
                        stackedAreaSeriesList);

                case "Line":                    
                    LineSeries lineSeries = new LineSeries()
                    {
                        Title = "GeneratorOn (hrs)",
                        DataLabels = true,                        
                        Values = new ChartValues<double>()
                    };
                    List<Series> lineSeriesList = new List<Series>();
                    lineSeriesList.Add(lineSeries);

                    return new Tuple<Axis, Axis, List<Series>>(
                        OrdinateAxis,
                        AbscissaAxis, 
                        lineSeriesList);

                case "Pie":
                    PieSeries pieSeries = new PieSeries()
                    {
                        Title = "GeneratorOn (hrs)",
                        DataLabels = true,
                        Values = new ChartValues<double>()
                    };
                    List<Series> pieSeriesList = new List<Series>();
                    pieSeriesList.Add(pieSeries);

                    return new Tuple<Axis, Axis, List<Series>>(
                        OrdinateAxis,
                        AbscissaAxis, 
                        pieSeriesList);

                default:
                    break;
            }
            return null;
        }
    }
}
