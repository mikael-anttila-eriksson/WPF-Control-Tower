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
using System.Text.RegularExpressions; // import Regex

namespace A5_WPF_v3
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// Subscriber class- It determines what 
    ///  action is taken in response to the event
    /// </summary>
    public partial class MainWindow : Window
    {
        //Fields
        List<FlightInfo> rows;

        //---------------------------------------------------------------
        #region Constructor
        /// <summary>
        /// Initiate main window aka Control Tower
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            // Initiate rows
            rows = new List<FlightInfo>();
            lstvwGeneral.Items.Clear();
            lstvwGeneral.ItemsSource = rows;

            txtFlightCode.Text = string.Empty;

            // For test
            //Testing();
        }



        #endregion
        //---------------------------------------------------------------
        #region Properties //none
        #endregion
        //---------------------------------------------------------------
        #region Methods
        // **************************************************************
        #region Coding/Debugging
        private void Testing()
        {
            // Create Flight information
            DateTime td = DateTime.Now;
            UpdateFlightInfo("Hej :D", "Are flying away", td);
            UpdateFlightInfo("ABC123", "New route to New York", td);

            // Create FlightWindow
            FlightWindow obj = new FlightWindow("dKJ443");   // Create form
            obj.ShowDialog();                                // open form

            // Enter a Flight code
            txtFlightCode.Text = "123456789";
            //ValidateFlightNr();

        }


        /// <summary>
        /// Indicate test successful! Used for Testing/Debugging
        /// </summary>
        private void Passed()
        {
            MessageBox.Show("Check pass!", "Test", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        /// <summary>
        /// Check content of rows. Used for Coding/Debugging
        /// </summary>
        /// <returns></returns>
        private string listToString()
        {
            string str = string.Empty;
            for (int i = 0; i < rows.Count; i++)
            {
                str += rows[i].ToString();
            }
            return str;
        }
        #endregion code/debug
        // **************************************************************
        #region Messages

        /// <summary>
        /// Simple error message
        /// </summary>
        /// <param name="msg"></param>
        private void Message(string msg)
        {
            MessageBoxButton button = MessageBoxButton.OK;
            MessageBoxImage err = MessageBoxImage.Error;

            MessageBox.Show(msg, "Error", button, err);
        }
        /// <summary>
        /// Message with custom Title
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="title"></param>
        private void Message(string msg, string title)
        {
            MessageBox.Show(msg, title);
        }
        #endregion messages
        // **************************************************************
        /// <summary>
        /// Validate Flight code
        /// </summary>
        /// <returns></returns>
        private bool ValidateFlightNr()
        {
            string flightCode = txtFlightCode.Text;

            if (string.IsNullOrEmpty(flightCode))
            {
                Message("Flight number is empty!");
                return false;
            }

            string alphaNumeric = @"^[a-zA-Z0-9]+$";
            if (!Regex.IsMatch(flightCode,alphaNumeric))
            {
                Message("Flight number contains invalid characters!");
                return false;
            }

            if(flightCode.Length >= 10)
            {
                Message("Flight number is too long!\nMax 9 characters.");
                return false;
            }

            //Passed();
            return true;

        }

        
        // **************************************************************
        #region Listview
        /// <summary>
        /// Update information about flights
        /// </summary>
        /// <param name="code"></param>
        /// <param name="status"></param>
        /// <param name="time"></param>
        private void UpdateFlightInfo(string code, string status, DateTime time)
        {
            //lstvwGeneral.Items.Clear();
            
            // 0. As I did on Forms
            //ListViewItem lvi = new ListViewItem();
            //lvi.Name = code;
            //lstvwFlightCode.Items.Add(code);

            // 1. WPF 
            rows.Add(new FlightInfo()
            {
                FlightCode = code,
                Status = status,
                Time = time.ToShortTimeString()
            });
            // Reset item source
            lstvwGeneral.ClearValue(ListView.ItemsSourceProperty);
            lstvwGeneral.ItemsSource = rows;

            // 1.1
            #region // For test
            if (2 == 3) 
            {
                List<FlightInfo> item = new List<FlightInfo>();
                item.Add(new FlightInfo()
                {
                    FlightCode = "Acode",
                    Status = "Sent to runway Sent to runway Sent to r", // max längd
                    Time = DateTime.Now.ToShortTimeString()
                });
                lstvwGeneral.ItemsSource = item;
                item.Add(new FlightInfo()
                {
                    FlightCode = "moracode",
                    Status = "Sent to runway",
                    Time = DateTime.Now.ToShortTimeString()
                });

                lstvwGeneral.ItemsSource = item;
            }
            #endregion for test
        }
        #endregion listview
        // **************************************************************

        /////////////////// EVENTS //////////////////////////////////////////
        #region Events
        /// <summary>
        /// Send Plane to Runway
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Send_Click(object sender, EventArgs e)     // My Start Event Method
        {
            
            string flightNr = txtFlightCode.Text;
            if(ValidateFlightNr() ==true)
            {
                FlightWindow plane = new FlightWindow(flightNr);
                // Update flight info
                UpdateFlightInfo(flightNr, "Sent to runway", DateTime.Now);
                // Subscribe to events
                plane.TakingOff += StartPlane; // Om den är före ShowDialog så funkar det :D
                plane.NewRoute += ChangeRoute;
                plane.Landing += Landed;

                // Create FlightWindow
                //plane.ShowDialog(); //Must close before you can access any other window
                plane.Show();       // Can open multiple windows

                //Reset Flight code box
                txtFlightCode.Text = String.Empty;

                //plane.SendToRunway();
            }

            
        }

        
        

        //###############################################################################
        ///  FROM THIS LINE ON - no information available on the event5 that is just created.
        ///  Here is the power of delegates - who is the bidder - don't know,
        ///  the sender tells us at run time
        ///  Eventhandler for the OnEvent5Created event 
        ///  - what to do when the event is 
        ///  fired by the publisher object (a bidder class, see above)

        private void StartPlane(object sender, FlightEventInfo e)
        {
            
            string status = "started";
            
            //Message("Plane is leaving " + e.FlightID);
            UpdateFlightInfo(e.FlightID, status, DateTime.Now);
        }

        private void ChangeRoute(object sender, FlightEventInfo e)
        {
            string status = "Now heading for " + e.Destination;
            UpdateFlightInfo(e.FlightID, status, DateTime.Now);
        }

        private void Landed(object sender, FlightEventInfo e)
        {
            string status = "Has landed";
            UpdateFlightInfo(e.FlightID, status, DateTime.Now);

            // Unsubscribe from events when plane has landed
            if (sender != null)
            {
                FlightWindow plane = (FlightWindow)sender; // get correct window

                // Unsubscribe
                plane.TakingOff -= StartPlane;
                plane.NewRoute -= ChangeRoute;
                plane.Landing -= Landed;
                //Message("Unsubscribed!", "Notice");
            }

        }
        #endregion events
        // **************************************************************
        #endregion
        //---------------------------------------------------------------
    }
}
