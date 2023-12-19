using LiveCharts;
using MyShop.Model;
using MyShop.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
namespace MyShop.Screen
{
    /// <summary>
    /// Interaction logic for Page1.xaml
    /// </summary>
    public partial class StatisticPage : Page
    {
        public SeriesCollection SeriesCollection { get; set; }
        public List<string> Labels { get; set; }
        public SeriesCollection SeriesCollectionProduct { get; set; }

        public List<string> LabelsProduct { get; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public Func<double, string> Formatter { get; set; }

        public StatisticPage()
        {
            InitializeComponent();
            SeriesCollection = new SeriesCollection();
            Labels = new List<string>();
            LabelsProduct = new List<string>();
            SeriesCollectionProduct = new SeriesCollection();


            //Formatter = value => value + " sản phẩm";
            StartDate = DateTime.Now.AddDays(-6); // Set a default start date (7 days ago)
            EndDate = DateTime.Now;
            // Formatter = value => "d" + value;
            // Thêm dữ liệu mẫu (bạn sẽ cần thay thế bằng dữ liệu thực tế của mình)
            viewModeComboBox.SelectedIndex = 0;
            DataContext = this;
        }

        private void ViewModeComboBox_SelectionChanged(object sender, RoutedEventArgs e)
        {
            var selectedMode = ((ComboBoxItem)viewModeComboBox.SelectedItem).Content.ToString();

            UpdateChartDataByMode(selectedMode);
            chart.Update(true, true);
        }
        private void ApplyDateRange_Click(object sender, RoutedEventArgs e)
        {
            // Apply date range logic          
            //UpdateChartDataByMode(((ComboBoxItem)viewModeComboBox.SelectedItem).Content.ToString());
            //chart.Update(true, true);
            StartDate = startDatePicker.SelectedDate.Value;
            EndDate = endDatePicker.SelectedDate.Value;
            UpdateChartDataByMode(((ComboBoxItem)viewModeComboBox.SelectedItem).Content.ToString());
            chart.Update(true, true);
        }

        private async void UpdateChartDataByMode(string mode)
        {
            datePickerStack.Visibility = (mode == "Date") ? Visibility.Visible : Visibility.Collapsed;

            Random random = new Random();
            SeriesCollection.Clear();
            Labels.Clear();

            string token = "";
            using (var reader = new StreamReader("data.json"))
            {
                // Đọc dữ liệu từ file
                token = reader.ReadToEnd();
            }

            var revenueData = await StatisticService.GetStatisticRevenue(mode, token, StartDate, EndDate);
            var statisticSold = await StatisticService.GetStatisticSold(mode, token, StartDate, EndDate);

            var statisticData1 = new Statistic
                (
                    "Profit Statistics",
                    new ChartValues<double>(revenueData.Revenue),
                    new ChartValues<double>(revenueData.Profit),
                    new List<string>(revenueData.Labels)
                );

            var statisticData2 = new Statistic
                (
                    "Product Statistics",
                    new ChartValues<double>(statisticSold.SoldQuantity), // Thay bằng dữ liệu từ API tương ứng
                    new ChartValues<double>(statisticSold.RemainingQuantity),   // Thay bằng dữ liệu từ API tương ứng
                    new List<string>(statisticSold.ProductNames)           // Thay bằng dữ liệu từ API tương ứng
                );

            if (mode == "Date")
            {
                statisticData1.show(SeriesCollection, Labels);
                statisticData2.showStatisticProduct(SeriesCollectionProduct, LabelsProduct);

            }
            else if (mode == "Week")
            {
                statisticData1.show(SeriesCollection, Labels);
                statisticData2.showStatisticProduct(SeriesCollectionProduct, LabelsProduct);


            }
            else if (mode == "Month")
            {
                statisticData1.show(SeriesCollection, Labels);
                statisticData2.showStatisticProduct(SeriesCollectionProduct, LabelsProduct);
            }
            else if (mode == "Year")
            {
                statisticData1.show(SeriesCollection, Labels);
                statisticData2.showStatisticProduct(SeriesCollectionProduct, LabelsProduct);

            }

        }

        private void CartesianChart_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
