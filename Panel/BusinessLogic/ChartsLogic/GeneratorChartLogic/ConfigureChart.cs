using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Definitions.Series;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Panel.BusinessLogic.ChartsLogic.GeneratorChartLogic
{
    public static class ConfigureChart
    {
        public static Tuple<Axis, List<Series>> ConfigureChartByChartType(string SeriesChartType, List<string> lstBoxSelectedStringValues)
        {
            
            Axis AbscissaAxis = new Axis()
            {
                Separator = new LiveCharts.Wpf.Separator() { Step = 1, IsEnabled = false },
                Title = "Date",
                Labels = lstBoxSelectedStringValues,
                LabelsRotation = 20
            };
            //YFormatter = v => v.ToString("N") + " hrs";
            switch (SeriesChartType)
            {
                case "Column":
                    ColumnSeries columnSeries = new ColumnSeries()
                    {
                        DataLabels = false,
                        Values = new ChartValues<double>()
                    };
                    List<Series> columnSeriesList = new List<Series>();
                    columnSeriesList.Add(columnSeries);
                    return new Tuple<Axis, List<Series>>(AbscissaAxis, columnSeriesList);

                case "Stacked Column":
                    StackedColumnSeries stackedColumnSeries = new StackedColumnSeries()
                    {
                        DataLabels = false,
                        Values = new ChartValues<double>()
                    };
                    List<Series> stackedColumnSeriesList = new List<Series>();
                    stackedColumnSeriesList.Add(stackedColumnSeries);
                    return new Tuple<Axis, List<Series>>(AbscissaAxis, stackedColumnSeriesList);

                case "Stacked Area":
                    StackedAreaSeries stackedAreaSeries = new StackedAreaSeries()
                    {
                        DataLabels = false,
                        Values = new ChartValues<DateTimePoint>()
                    };
                    List<Series> stackedAreaSeriesList = new List<Series>();
                    stackedAreaSeriesList.Add(stackedAreaSeries);
                    return new Tuple<Axis, List<Series>>(AbscissaAxis, stackedAreaSeriesList);

                case "Line":                    
                    LineSeries lineSeries = new LineSeries()
                    {
                        DataLabels = false,                        
                        Values = new ChartValues<double>()
                    };
                    List<Series> lineSeriesList = new List<Series>();
                    lineSeriesList.Add(lineSeries);
                    return new Tuple<Axis, List<Series>>(AbscissaAxis, lineSeriesList);

                case "Pie":
                    PieSeries pieSeries = new PieSeries()
                    {
                        DataLabels = false,
                        Values = new ChartValues<double>()
                    };
                    List<Series> pieSeriesList = new List<Series>();
                    pieSeriesList.Add(pieSeries);
                    return new Tuple<Axis, List<Series>>(AbscissaAxis, pieSeriesList);

                default:
                    break;
            }
            return null;
        }
    }
}
