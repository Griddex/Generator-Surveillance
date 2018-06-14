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
    //AbscissaAxis
    public static class ConfigureChart
    {
        public static Tuple<Axis, Series> ConfigureChartByChartType(string SeriesChartType, List<string> lstBoxSelectedStringValues)
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
                    return new Tuple<Axis, Series>(AbscissaAxis, columnSeries);

                case "Stacked Column":
                    StackedColumnSeries stackedColumnSeries = new StackedColumnSeries()
                    {
                        DataLabels = false,
                        Values = new ChartValues<double>()
                    };
                    return new Tuple<Axis, Series>(AbscissaAxis, stackedColumnSeries);

                case "Stacked Area":
                    StackedAreaSeries stackedAreaSeries = new StackedAreaSeries()
                    {
                        DataLabels = false,
                        Values = new ChartValues<DateTimePoint>()
                    };
                    return new Tuple<Axis, Series>(AbscissaAxis, stackedAreaSeries);

                case "Line":                    
                    LineSeries lineSeries = new LineSeries()
                    {
                        DataLabels = false,                        
                        Values = new ChartValues<double>()
                    };
                    return new Tuple<Axis, Series>(AbscissaAxis, lineSeries);

                case "Pie":
                    PieSeries pieSeries = new PieSeries()
                    {
                        DataLabels = false,
                        Values = new ChartValues<double>()
                    };
                    return new Tuple<Axis, Series>(AbscissaAxis, pieSeries);

                default:
                    break;
            }
            return null;
        }
    }
}
