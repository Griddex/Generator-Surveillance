using LiveCharts.Wpf;
using Panel.BusinessLogic.ChartsLogic.GeneratorChartLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Panel.BusinessLogic.ChartsLogic.GeneratorChartLogic
{
    public static class GeneratorUsageLogic
    {
        public static void PlotChart(string SelectedGeneratorName, string SelectedChartType, 
                                    string DurationPerspective, List<string> lstBoxSelectedStringValues, 
                                    int lstBoxSelectedValuesCount, StackPanel chtStackPanel)
        {
            foreach (var control in chtStackPanel.Children)
            {
                if (control is CartesianChart)
                {
                    chtStackPanel.Children.Remove((CartesianChart)control);
                    break;
                }                    
                else if (control is PieChart)
                {
                    chtStackPanel.Children.Remove((PieChart)control);
                    break;
                }                   
            }

            switch (SelectedChartType)
            {
                case "Column":
                case "Stacked Column":
                case "Stacked Area":
                case "Line":
                    CartesianChart cartesianChart = new CartesianChart();
                    chtStackPanel.Children.Add(cartesianChart);
                    List<List<double>> AllCartesianOrdinateSeriesInHours = GetOrdinateData.GetData(SelectedGeneratorName, SelectedChartType,
                                                                                            DurationPerspective,lstBoxSelectedStringValues);
                    Tuple<Axis, List<Series>> AxisCartesianSeriesTuple = ConfigureChart.ConfigureChartByChartType(SelectedChartType, 
                                                                                                    lstBoxSelectedStringValues);
                    AssembleChart.ShowPlot(SelectedChartType, AllCartesianOrdinateSeriesInHours, AxisCartesianSeriesTuple, cartesianChart);
                    
                    break;

                case "Pie":
                    PieChart pieChart = new PieChart();
                    chtStackPanel.Children.Add(pieChart);
                    List<List<double>> AllPieOrdinateSeriesInHours = GetOrdinateData.GetData(SelectedGeneratorName, SelectedChartType,
                                                                                           DurationPerspective, lstBoxSelectedStringValues);
                    Tuple<Axis, List<Series>> AxisPieSeriesTuple = ConfigureChart.ConfigureChartByChartType(SelectedChartType, 
                                                                                                    lstBoxSelectedStringValues);
                    AssembleChart.ShowPlot(SelectedChartType, AllPieOrdinateSeriesInHours, AxisPieSeriesTuple, pieChart);

                    break;
                default:
                    break;
            }
        }
    }
}
