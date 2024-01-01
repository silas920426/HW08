using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using static System.Net.WebRequestMethods;

namespace HW08
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string defaultURL = "https://data.moenv.gov.tw/api/v2/aqx_p_432?api_key=e8dd42e6-9b8b-43f8-991e-b3dee723a52d&limit=1000&sort=ImportDate desc&format=JSON";
        AQIdata aqidata = new AQIdata();
        List<Field> fields = new List<Field>();
        List<Record> records = new List<Record>();
        public MainWindow()
        {
            InitializeComponent();
            UrlTextBox.Text = defaultURL;
        }

        private async void FetchDataButton_Click(object sender, RoutedEventArgs e)
        {
            ContentTextBox.Text = "抓取網路資料中...";

            string jsonData = await FetchContentAsync(defaultURL);
            ContentTextBox.Text = jsonData;
            aqidata = JsonSerializer.Deserialize<AQIdata>(jsonData);
            fields = aqidata.fields.ToList();
            records = aqidata.records.ToList();
            StatusTextBlock.Text = $"共有{records.Count}筆資料";
            DisplayAQIData();
        }

        private void DisplayAQIData()
        {
            RecordDataGrid.ItemsSource = records;
        }

        private async Task<string> FetchContentAsync(string url)
        {
            try 
            {
                using (HttpClient client = new HttpClient())
                {
                    return await client.GetStringAsync(url);
                }
            }
            catch (Exception ex) 
            {
                return $"{ex.Message}";
            }
        }
    }
}
