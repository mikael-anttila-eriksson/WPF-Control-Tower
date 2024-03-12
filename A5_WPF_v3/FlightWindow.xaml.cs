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
using System.Windows.Shapes;
using System.Text.RegularExpressions; // import Regex
using System.Media;                   // import 5 simple sounds  

namespace A5_WPF_v3
{
    /// <summary>
    /// A FlightWindow for a specific flight. Trigger events to Start and Land the plane, aswell as change its Destination country.
    /// </summary>
    public partial class FlightWindow : Window
    {
        //Fields
        string flightNr = string.Empty;
        BitmapImage planeImage = null;
        string myUri = string.Empty;
        
        //---------------------------------------------------------------
        #region Constructor
        public FlightWindow(string code)
        {
            InitializeComponent();
            // Set ComboBox data source
            cmbxRoute.ItemsSource = Enum.GetValues(typeof(Countries));

            // Set default
            //cmbxRoute.SelectedIndex = (int)Countries.Afghanistan;
            // Default is set in XAML

            // Choose image
            flightNr = code;
            ChooseImages(flightNr);

            //Enable buttons
            btnStart.IsEnabled = true;
            cmbxRoute.IsEnabled = false;
            btnLand.IsEnabled = false;
            
        }
        
        #endregion
        //---------------------------------------------------------------
        #region Properties // none
        
        #endregion
        //---------------------------------------------------------------
        #region Methods
        // **************************************************************
        #region Image
        /// <summary>
        /// Decide imgage of plane based on some rules on the Flight number
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        private bool ChooseImages(string code)
        {
            bool test1 = char.IsLetter(code, 0);

            // Check first charachter
            if(test1 == true)
            {
                // First: Is letter
                code = code.ToUpper();
                char[] isdD = { 'D' };
                

                // Test A - First letter is D
                bool testA = code.IndexOfAny(isdD) == 0;
                if (testA)      // D as in Killer Drone
                {
                    // first letter is D
                    //MessageBox.Show("A");
                    SetImage("KillerDrone.jpg");
                    return true; // Stop the code
                }

                // Test B - Contains WXYZ
                char[] isWXYZ = { 'W', 'X', 'Y', 'Z' };
                bool testB = code.IndexOfAny(isWXYZ) != -1;
                if (testB) // Only for Jet fighter
                {
                    // Contains at least one of following: WXYZ
                    //MessageBox.Show("B");
                    SetImage("JetPlane.jpg");
                    return true; // Stop the code
                }

                // Test C - Has form of: ABC123 (3 letters and 3 numbers)
                if(code.Length == 6) // i.e. must be 6 characters long
                {
                    string strC = code.Substring(0, 3);
                    bool testC1 = Regex.IsMatch(strC, @"^[a-zA-Z]+$");
                    strC = code.Substring(3, 3);
                    bool testC2 = int.TryParse(strC, out _);
                    if (testC1 && testC2)  // SAS
                    {
                        //MessageBox.Show("C");
                        SetImage("SAS.jpg");
                        return true; // Stop the code
                    }
                }
                else  // Default Take off    
                {
                    //MessageBox.Show("D");
                    SetImage("TakeOff.jpg");
                    return true; // Stop the code
                }

            }
            else // Finnair starts with number
            {
                // First: Is number
                //MessageBox.Show("Finnair");
                SetImage("Finnair.jpg");
            }
            return true; // End of code anyway

        }
        /// <summary>
        /// Set image of plane on FlightWindow
        /// </summary>
        /// <param name="path"></param>
        private void SetImage(string path)
        {
            // Select image
            //myImage = "JetPlane.jpg";
            myUri = string.Format("Image/{0}", path);
            #region Alternatives 1 o 2
            // Connect image to XMAL
            // ALT 1
            //imgPlane.Source = new BitmapImage(new Uri(myUri, UriKind.Relative));
            // ALT 2
            // Create a BitmapSource
            //BitmapImage bit = new BitmapImage();
            //bit.BeginInit();
            //bit.UriSource = new Uri(myUri, UriKind.Relative);
            //bit.EndInit();
            // Set XMAL-image = Bitmap
            //imgPlane.Source = bit;
            #endregion Alt 1 o 2
            // ALT 3
            // Initiate BitmapImage
            planeImage = new BitmapImage();
            planeImage.BeginInit();
            planeImage.UriSource = new Uri(myUri, UriKind.Relative);
            planeImage.EndInit();
            // Set XMAL-image = Bitmap
            imgPlane.Source = planeImage;
        }
        #endregion image
        // **************************************************************
        #region Buttons // are under Events
        
        #endregion buttons 
        // **************************************************************
        #region EVENTS 
        // Steps from "BidHouseWPF"
        //########################################################################################
        //0. Create a class for holding info for en event (can be one each event)
        //  Here: One class called (FlightEventInfo) 

        //1. Declare an event (of the generic type - the easiest way)
        public event EventHandler<FlightEventInfo> TakingOff;
        public event EventHandler<FlightEventInfo> NewRoute;
        public event EventHandler<FlightEventInfo> Landing;

        //2.  Determine WHEN TO RAISE THE EVENT. 
        //Note: The arguments here, neither the sender, nor the e is of any interest for us,
        //as these two belong to the button-click event.
        //We create a bidInfo  (correspondng to e) object and the sender is "this".

        // ---------- When to Raise event -------------
        /// <summary>
        /// Click to let the plane start
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnStart_Click(object sender, EventArgs e)
        {
            // Change enabled
            btnStart.IsEnabled = false;
            cmbxRoute.IsEnabled = true;
            btnLand.IsEnabled = true;

            FlightEventInfo start = new FlightEventInfo(this.flightNr);
            OnStart(start);
            //OnStart(sender, start);
        }

        /// <summary>
        /// Select country in combobox to change the destination
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbxRoute_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = cmbxRoute.SelectedIndex;
            if(index >= 0)
            {
                Countries dest = (Countries)index;
                FlightEventInfo destination = new FlightEventInfo(this.flightNr, dest);
                OncmbxChange(destination);
            }
            
        }
        /// <summary>
        /// Click to let the plane land
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLand_Click(object sender, EventArgs e)
        {
            FlightEventInfo land = new FlightEventInfo(this.flightNr);
            OnLand(land);

            // Play sound when Window closes
            SystemSounds.Hand.Play();
            this.Close();             // Close current Window
            //App.Current.Shutdown();   // Close ALL WindowSS!
        }

        //3. Raise events
        /// <summary>
        /// Trigger event that starts the plane. Send information in e.
        /// </summary>
        /// <param name="e"></param>
        public void OnStart(FlightEventInfo e)
        {
            //TakingOff?.Invoke(this, e);
            TakingOff(this, e);
            //if(TakingOff != null)
        }

        /// <summary>
        /// Trigger event that changes the plane´s destionation route. Send information in e.
        /// </summary>
        /// <param name="e"></param>
        public void OncmbxChange(FlightEventInfo e)
        {
            NewRoute(this, e);
        }

        /// <summary>
        /// Trigger event that makes the plane land. Send information in e.
        /// </summary>
        /// <param name="e"></param>
        public void OnLand(FlightEventInfo e)
        {
            Landing(this, e);
        }

        // **************************************************************
        // When to Raise event
        #region Code Not Used
        public void SendToRunway() //FlightWindow Created
        {
            var data = new FlightEventInfo("wrong");
            MessageBox.Show("Starting event");
            try
            {
                data.FlightID = flightNr;
                // ON-method
                OnTakingOff(data);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Problem when taking off");
                
            }

        }

        protected virtual void OnTakingOff(FlightEventInfo e)
        {
            TakingOff?.Invoke(this, e);
        }
        #endregion not used
        // **************************************************************
        #endregion events
        // **************************************************************
        #endregion methods
        //---------------------------------------------------------------

    }
}
