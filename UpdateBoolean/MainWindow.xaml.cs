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
using NewEventLogDLL;
using WeeklyBulkToolInspectionDLL;

namespace UpdateBoolean
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //setting up the clases
        WPFMessagesClass TheMessagesClass = new WPFMessagesClass();
        EventLogClass TheEventLogClass = new EventLogClass();
        WeeklyBulkToolInspectionClass TheWeeklyBulkToolInspectionClass = new WeeklyBulkToolInspectionClass();

        //setting up the data
        WeeklyBulkToolInspectionDataSet TheWeeklyToolInspectionDataSet = new WeeklyBulkToolInspectionDataSet();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            TheWeeklyToolInspectionDataSet = TheWeeklyBulkToolInspectionClass.GetWeeklyBulktoolInspectionInfo();

            dgrResults.ItemsSource = TheWeeklyToolInspectionDataSet.weeklybulktoolinspection;
        }

        private void btnProcess_Click(object sender, RoutedEventArgs e)
        {
            int intCounter;
            int intNumberOfRecords;
            bool blnItemContains;

            try
            {
                intNumberOfRecords = TheWeeklyToolInspectionDataSet.weeklybulktoolinspection.Rows.Count - 1;

                for(intCounter = 0; intCounter <= intNumberOfRecords; intCounter++)
                {
                    if (TheWeeklyToolInspectionDataSet.weeklybulktoolinspection[intCounter].IsConesCorrectNull() == true)
                    {
                        blnItemContains = TheWeeklyToolInspectionDataSet.weeklybulktoolinspection[intCounter].InspectionNotes.Contains("CONES");

                        if(blnItemContains == true)
                        {
                            TheWeeklyToolInspectionDataSet.weeklybulktoolinspection[intCounter].ConesCorrect = false;
                        }
                        else
                        {
                            TheWeeklyToolInspectionDataSet.weeklybulktoolinspection[intCounter].ConesCorrect = true;
                        }
                    }
                    if(TheWeeklyToolInspectionDataSet.weeklybulktoolinspection[intCounter].IsSignsCorrectNull() == true)
                    {
                        blnItemContains = TheWeeklyToolInspectionDataSet.weeklybulktoolinspection[intCounter].InspectionNotes.Contains("SIGN");

                        if (blnItemContains == true)
                        {
                            TheWeeklyToolInspectionDataSet.weeklybulktoolinspection[intCounter].SignsCorrect = false;
                        }
                        else
                        {
                            TheWeeklyToolInspectionDataSet.weeklybulktoolinspection[intCounter].SignsCorrect = true;
                        }
                    }
                    if (TheWeeklyToolInspectionDataSet.weeklybulktoolinspection[intCounter].IsFirstAidCorrectNull() == true)
                    {
                        blnItemContains = TheWeeklyToolInspectionDataSet.weeklybulktoolinspection[intCounter].InspectionNotes.Contains("FIRST");

                        if (blnItemContains == true)
                        {
                            TheWeeklyToolInspectionDataSet.weeklybulktoolinspection[intCounter].FirstAidCorrect = false;
                        }
                        else
                        {
                            TheWeeklyToolInspectionDataSet.weeklybulktoolinspection[intCounter].FirstAidCorrect = true;
                        }
                    }

                    if (TheWeeklyToolInspectionDataSet.weeklybulktoolinspection[intCounter].IsFireExtingisherNull() == true)
                    {
                        blnItemContains = TheWeeklyToolInspectionDataSet.weeklybulktoolinspection[intCounter].InspectionNotes.Contains("FIRE");

                        if (blnItemContains == true)
                        {
                            TheWeeklyToolInspectionDataSet.weeklybulktoolinspection[intCounter].FireExtingisher = false;
                        }
                        else
                        {
                            TheWeeklyToolInspectionDataSet.weeklybulktoolinspection[intCounter].FireExtingisher = true;
                        }
                    }
                }

                dgrResults.ItemsSource = TheWeeklyToolInspectionDataSet.weeklybulktoolinspection;

                TheWeeklyBulkToolInspectionClass.UpdateWeeklyBulkToolInspectionDB(TheWeeklyToolInspectionDataSet);
            }
            catch (Exception Ex)
            {
                TheEventLogClass.InsertEventLogEntry(DateTime.Now, "Update Boolean // Process Button " + Ex.Message);

                TheMessagesClass.ErrorMessage(Ex.ToString());
            }
        }
    }
}
