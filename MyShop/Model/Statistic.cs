using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;

namespace MyShop.Model
{
    internal class Statistic
    {
        public string Name { get; set; }
        public ChartValues<double> RevenueData { get; set; }
        public ChartValues<double> ProfitData { get; set; }
        public List<string> Labels { get; set; }

        public Statistic(string name, ChartValues<double> revenueData, ChartValues<double> profitData, List<string> labels)
        {
            Name = name;
            RevenueData = revenueData ?? throw new ArgumentNullException(nameof(revenueData));
            ProfitData = profitData ?? throw new ArgumentNullException(nameof(profitData));
            Labels = labels ?? throw new ArgumentNullException(nameof(labels));

        }

        public void showStatisticProduct(SeriesCollection series, List<string> labels)
        {
            series.Clear();

            labels.Clear();
            for (int i = 0; i < Labels.Count; i++)
            {
                labels.Add(Labels[i]);
            }
            series.Add(

                new StackedColumnSeries
                {
                    Title = "Còn lại",
                    Values = new ChartValues<double>(this.ProfitData),
                    StackMode = StackMode.Values, // this is not necessary, values is the default stack mode
                                                  //DataLabels = true,

                }
                );
            series.Add(

                new StackedColumnSeries
                {
                    Title = "Đã bán",
                    Values = new ChartValues<double>(this.RevenueData),
                    StackMode = StackMode.Values, // this is not necessary, values is the default stack mode
                    //DataLabels = true
                }
                );

            //adding series updates and animates the chart
            //series.Add(new StackedColumnSeries
            //{
            //    Values = new ChartValues<double> { 6, 2, 7 },
            //    StackMode = StackMode.Values
            //});

            //adding values also updates and animates
            //SeriesCollectionProduct[2].Values.Add(4d);





        }
        public void show(SeriesCollection series, List<string> labels)
        {

            series.Clear();
            labels.Clear();
            series.Add(new LineSeries
            {
                Title = "Revenue",
                Values = this.RevenueData
            });

            series.Add(new LineSeries
            {
                Title = "Profit",
                Values = this.ProfitData
            });
            labels.AddRange(this.Labels);

        }
    }
}
