using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.SqlClient;
using System.Data;
using System.Timers;

namespace DoorControlApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<Door> doorList = new List<Door>();
        static string connectionString = @"Server = DESKTOP\SQLEXPRESS; Database = Doors; Integrated Security=SSPI";
        SqlConnection sqlConnection = new SqlConnection(connectionString);

        //set a timer callback for 3 seconds
        private const short interval = 3;
        private Timer timer;

        public MainWindow()
        {
            InitializeComponent();

            timer = new Timer();
            timer.Interval = interval * 1000;
            timer.Elapsed += TimerListener;
            timer.Start();

            UpdateGrid();
            drawUI();
        }

        public void updateStatus(string iD, bool status)
        {
            //convert status to int
            int iStatus = status ? 1 : 0;

            //set the update query
            string updateQuery = @"update Doors
  set status = @status
  where id = @iD";
            sqlConnection.Open();
            SqlCommand sqlCommand = new SqlCommand(updateQuery, sqlConnection);
            sqlCommand.Parameters.AddWithValue("@status", iStatus);
            sqlCommand.Parameters.AddWithValue("@iD", iD);

            sqlCommand.ExecuteNonQuery();
            sqlConnection.Close();
            UpdateGrid();
        }

        Grid DynamicGrid = new Grid();
        List<RowDefinition> rows = new List<RowDefinition>();

        public void drawUI()
        {

            DynamicGrid.Width = 800;

            DynamicGrid.HorizontalAlignment = HorizontalAlignment.Left;

            DynamicGrid.VerticalAlignment = VerticalAlignment.Top;

            DynamicGrid.ShowGridLines = true;
            DynamicGrid.Background = new SolidColorBrush(Colors.Gray);
            ColumnDefinition numberColumn = new ColumnDefinition();
            ColumnDefinition iDColumn = new ColumnDefinition();
            ColumnDefinition labelColumn = new ColumnDefinition();
            ColumnDefinition statusColumn = new ColumnDefinition();

            numberColumn.Width = new GridLength(45);

            DynamicGrid.ColumnDefinitions.Add(numberColumn);
            DynamicGrid.ColumnDefinitions.Add(iDColumn);
            DynamicGrid.ColumnDefinitions.Add(labelColumn);
            DynamicGrid.ColumnDefinitions.Add(statusColumn);

            Window rootWindow = Window.GetWindow(this);
            rootWindow.Content = DynamicGrid;
        }

        public void TimerListener(object sender, ElapsedEventArgs e)
        {
            UpdateGrid();
        }

        public void UpdateGrid()
        {

            doorList.Clear();

            //set a select query to read from the database
            string selectQuery = @"SELECT [iD]
      ,[label]
      ,[status]
  FROM[Doors].[dbo].[doors]
";
            SqlCommand command = new SqlCommand(selectQuery, sqlConnection);

            sqlConnection.Open();

            using (SqlDataReader dataReader = command.ExecuteReader())
            {
                // Process the DataReader
                while (dataReader.Read())
                {
                    bool status = Convert.ToBoolean(dataReader["status"].ToString());
                    Door newDoor = new Door(dataReader["iD"].ToString(), dataReader["label"].ToString(), status);
                    doorList.Add(newDoor);
                }
            }
            sqlConnection.Close();

            //update the UI elements
            this.Dispatcher.Invoke(() =>
            {
                while (rows.Count > doorList.Count + 1)
                {
                    DynamicGrid.RowDefinitions.RemoveAt(rows.Count - 1);
                    rows.RemoveAt(rows.Count - 1);
                }

                int i = rows.Count;
                while (rows.Count < doorList.Count + 1)
                {
                    RowDefinition row = new RowDefinition();
                    row.Height = new GridLength(45);
                    rows.Add(row);
                    DynamicGrid.RowDefinitions.Add(rows[i]);
                    i++;
                }

                // Add first column header    
                TextBlock txtBlock1 = new TextBlock();
                txtBlock1.Text = "ID";
                txtBlock1.FontSize = 14;
                txtBlock1.FontWeight = FontWeights.Bold;
                txtBlock1.Foreground = new SolidColorBrush(Colors.GhostWhite);
                txtBlock1.VerticalAlignment = VerticalAlignment.Top;
                Grid.SetRow(txtBlock1, 0);
                Grid.SetColumn(txtBlock1, 1);

                // Add second column header    
                TextBlock txtBlock2 = new TextBlock();
                txtBlock2.Text = "Label";
                txtBlock2.FontSize = 14;
                txtBlock2.FontWeight = FontWeights.Bold;
                txtBlock2.Foreground = new SolidColorBrush(Colors.GhostWhite);
                txtBlock2.VerticalAlignment = VerticalAlignment.Top;
                Grid.SetRow(txtBlock2, 0);
                Grid.SetColumn(txtBlock2, 2);

                // Add third column header    
                TextBlock txtBlock3 = new TextBlock();
                txtBlock3.Text = "Status";
                txtBlock3.FontSize = 14;
                txtBlock3.FontWeight = FontWeights.Bold;
                txtBlock3.Foreground = new SolidColorBrush(Colors.GhostWhite);
                txtBlock3.VerticalAlignment = VerticalAlignment.Top;
                Grid.SetRow(txtBlock3, 0);
                Grid.SetColumn(txtBlock3, 3);

                DynamicGrid.Children.Clear();


                //// Add column headers to the Grid    
                DynamicGrid.Children.Add(txtBlock1);
                DynamicGrid.Children.Add(txtBlock2);
                DynamicGrid.Children.Add(txtBlock3);

                for (int d = 0; d < doorList.Count; d++)
                {
                    //populate number column
                    TextBlock numberText = new TextBlock();
                    numberText.Text = (d + 1).ToString();
                    numberText.FontSize = 12;
                    numberText.Foreground = new SolidColorBrush(Colors.GhostWhite);
                    numberText.FontWeight = FontWeights.Bold;
                    Grid.SetRow(numberText, d + 1);
                    Grid.SetColumn(numberText, 0);

                    // populate ID column    
                    TextBlock idText = new TextBlock();
                    idText.Text = doorList[d].ID;
                    idText.FontSize = 12;
                    idText.Foreground = new SolidColorBrush(Colors.GhostWhite);
                    idText.FontWeight = FontWeights.Bold;
                    Grid.SetRow(idText, d + 1);
                    Grid.SetColumn(idText, 1);

                    //populate label column
                    TextBlock labelText = new TextBlock();
                    labelText.Text = doorList[d].label;
                    labelText.FontSize = 12;
                    labelText.Foreground = new SolidColorBrush(Colors.GhostWhite);
                    labelText.FontWeight = FontWeights.Bold;
                    Grid.SetRow(labelText, d + 1);
                    Grid.SetColumn(labelText, 2);

                    //populate status column
                    TextBlock statusText = new TextBlock();
                    if (doorList[d].status) statusText.Text = "Open";
                    else statusText.Text = "Closed";
                    statusText.FontSize = 12;
                    statusText.Foreground = new SolidColorBrush(Colors.GhostWhite);
                    statusText.FontWeight = FontWeights.Bold;
                    Grid.SetRow(statusText, d + 1);
                    Grid.SetColumn(statusText, 3);

                    // Add first row to Grid   
                    DynamicGrid.Children.Add(numberText);
                    DynamicGrid.Children.Add(idText);
                    DynamicGrid.Children.Add(labelText);
                    DynamicGrid.Children.Add(statusText);
                }

                //dynamically resize the window
                this.SizeToContent = SizeToContent.Height;

            });
        }

        //mouse listener to convert the point location to a row and column value
        private void Window_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Point point = e.GetPosition(DynamicGrid);

            int row = 0;
            int col = 0;
            double accumulatedHeight = 0.0;
            double accumulatedWidth = 0.0;

            // calc row mouse was over
            foreach (var rowDefinition in DynamicGrid.RowDefinitions)
            {
                accumulatedHeight += rowDefinition.ActualHeight;
                if (accumulatedHeight >= point.Y)
                    break;
                row++;
            }

            // calc col mouse was over
            foreach (var columnDefinition in DynamicGrid.ColumnDefinitions)
            {
                accumulatedWidth += columnDefinition.ActualWidth;
                if (accumulatedWidth >= point.X)
                    break;
                col++;
            }

            //if the click lands in the status cell of any element, switch the bool between open and closed
            if (row > 0 && col == 3) updateStatus(doorList[row - 1].ID, !doorList[row - 1].status);
        }
    }
}
