using System;
using System.Windows;
using System.Windows.Controls;
using System.Data;
using System.Data.SqlClient;


namespace MSC_WPF_BETA
{
    /// <summary>
    /// Interaction logic for Dashboard.xaml
    /// </summary>
    public partial class Dashboard : Window
    {
        //Initialializing code component
        public Dashboard()
        {
            InitializeComponent();
            fullNameLabel.Content = MainWindow.fullName + ",";
            dashboardGrid.Visibility = Visibility.Visible;
        }

        //Sql connection string where connection string is Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\Database1.mdf;Integrated Security=True
        SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\Database1.mdf;Integrated Security=True");
        
        //Global variables declaration section
        SqlCommand cmd;
        SqlDataReader dr;
        public static string userId = "";
        public static string userStatus = "";
        public static string userRole = "";
        public static string vendorName = "";
        public static string vendorId = "";
        public static string emId = "";
        public static string sdId = "";
        public static string payment_status = "";
        public static string tenderno = "";
        public static string billno = "";
        public static string natureofwork = "";
        public static string payment_date = "";
        public static string logsId = "";
        public static string vendorAddress = "";
        public static string vendorAccNo = "";
        public static string vendorBankName = "";
        public static string vendorBranchName = "";
        public static string estimatedCost = "";
        public static string earnestMoney = "";
        public static string securityDeposit = "";
        public static string remarks = "";
        public static string printFormNo = "";
        //End of Global variable declaration

        /*Function to set the visibility of all the grids to hidden
         *  This function is written for general purpose code reusability
         *  Whenever user switches from one tab to another the visible grid has to be hidden
         *  So instead of writing the same code on 10 different link buttons this function is created
         *  which gets invoked in each link buttons
         */
        private void SetVisibilityToHidden()
        {
            vendorFormGrid.Visibility = Visibility.Hidden;
            earnestMoneyFormGrid.Visibility = Visibility.Hidden;
            securityDepositFormGrid.Visibility = Visibility.Hidden;
            vendorRegisterGrid.Visibility = Visibility.Hidden;
            emRegisterGrid.Visibility = Visibility.Hidden;
            sdRegisterGrid.Visibility = Visibility.Hidden;
            earnestMoneyUpdateFormGrid.Visibility = Visibility.Hidden;
            securityDepositUpdateFormGrid.Visibility = Visibility.Hidden;
            successGrid.Visibility = Visibility.Hidden;
            vendorEditFormGrid.Visibility = Visibility.Hidden;
            logsGrid.Visibility = Visibility.Hidden;
            usersGrid.Visibility = Visibility.Hidden;
            vrferrorLabel.Visibility = Visibility.Hidden;
            emg2errorLabel.Visibility = Visibility.Hidden;
            sdg2errorLabel.Visibility = Visibility.Hidden;
            emug2errorLabel.Visibility = Visibility.Hidden;
            sdug2errorLabel.Visibility = Visibility.Hidden;
            veferrorLabel.Visibility = Visibility.Hidden;
        }

        /* Code to insert vendor data into vendors table which will be performed
         * whenever user click on the vendorRegisterButton
         */
        private void vendorRegisterButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                vrferrorLabel.Visibility = Visibility.Hidden;
                if(VendorFormIsValid())
                {
                    cmd = new SqlCommand("INSERT INTO vendors VALUES (@VENDORNAME, @CONTACTNUMBER, @ADDRESS, @ACCOUNTNO, @BANKNAME, @BRANCH, @DETAILS, @REGISTERED_ON, @USERID, @FULLNAME, @LAST_MODIFIED_BY, @LAST_MODIFIED_ON)", con);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@VENDORNAME", vrftbname.Text);
                    cmd.Parameters.AddWithValue("@CONTACTNUMBER", vrftbContact.Text);
                    cmd.Parameters.AddWithValue("@ADDRESS", vrftbAddress.Text);
                    cmd.Parameters.AddWithValue("@BANKNAME", vrftbBankName.Text);
                    cmd.Parameters.AddWithValue("@BRANCH", vrftbBranchName.Text);
                    cmd.Parameters.AddWithValue("@ACCOUNTNO", vrftbAccNo.Text);
                    cmd.Parameters.AddWithValue("@DETAILS", vrftbDetails.Text);
                    cmd.Parameters.AddWithValue("@REGISTERED_ON", DateTime.Now);
                    cmd.Parameters.AddWithValue("@USERID", MainWindow.userId);
                    cmd.Parameters.AddWithValue("@FULLNAME", MainWindow.fullName);
                    cmd.Parameters.AddWithValue("@LAST_MODIFIED_BY", MainWindow.fullName + ", " + MainWindow.userId);
                    cmd.Parameters.AddWithValue("@LAST_MODIFIED_ON", DateTime.Now);
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                    successGridTB.Text = vrftbname.Text + " successfully registered by " + MainWindow.fullName + " on " + DateTime.Now + "\nContact Number: " + vrftbContact.Text + "\nAddress: " + vrftbAddress.Text + "\nBank: " + vrftbBankName.Text + "," + vrftbBranchName.Text + "\nAccount Number: " + vrftbAccNo.Text + "\nOther Details:\n" + vrftbDetails.Text;
                    successGrid.Visibility = Visibility.Visible;
                    vendorName = vrftbname.Text;
                    successButtonGrid(1);
                    Resetvrf();
                }
            } catch (Exception)
            {   
                //Invalid. Data maybe too long: This exception will get triggered when user exceeds the value
                // of the designated data types
                vrferrorLabel.Content = "Invalid. Data maybe too long*";
                vrferrorLabel.Visibility = Visibility.Visible;
                Resetvrf();
            }
        }

        //Validation function to check whether the vendor form is valid or not
        //If valid returns true else false
        private bool VendorFormIsValid()
        {
            Boolean IsValid = true;
            if (vrftbname.Text == string.Empty || vrftbAccNo.Text == string.Empty || vrftbBankName.Text == string.Empty || vrftbBranchName.Text == string.Empty)
            {
                vrferrorLabel.Content = "Please fill in all the required fields*";
                vrferrorLabel.Visibility = Visibility.Visible;
                return false;
            }
            cmd = new SqlCommand("SELECT VENDORNAME FROM vendors", con);
            con.Open();
            dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                if (dr[0].ToString() == vrftbname.Text)
                {
                    vrferrorLabel.Content = "Vendor Name already exists*";
                    vrferrorLabel.Visibility = Visibility.Visible;
                    veftbname.Focus();
                    IsValid = false;
                }
            }
            dr.Close();
            con.Close();
            if (IsValid == false)
                return false;
            return true;
        }

        //Function to clear all the fields of vendor form
        private void Resetvrf()
        {
            vrftbname.Clear();
            vrftbContact.Clear();
            vrftbAddress.Clear();
            vrftbAccNo.Clear();
            vrftbBankName.Clear();
            vrftbBranchName.Clear();
            vrftbDetails.Clear();

            vrftbname.Focus();
        }

        private void vrfClearButton_Click(object sender, RoutedEventArgs e)
        {
            Resetvrf();
        }

        /* This event gets triggered whenever user clicks on security deposit register button
         * This code is used to insert all the values of security deposit details in securitydeposit table
         */
        private void sdfGridButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                sdg2errorLabel.Visibility = Visibility.Hidden;
                if (sdfIsValid())
                {
                    string sdDate = sdDatePicker.SelectedDate.ToString();
                    cmd = new SqlCommand("INSERT INTO securitydeposit VALUES (@DATE, @NATUREOFWORK, @BILL_NO, @GROSS_AMOUNT, @SECURITY_DEPOSIT, @INCOME_TAX, @SALES_TAX, @NET_AMOUNT, @FUND_NAME, @REGISTERED_BY, @REMARKS, @PAYMENT_STATUS, @VENDOR_NAME, @LAST_MODIFIED_BY, @LAST_MODIFIED_ON, @PAYMENT_DATE)", con);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@DATE", sdDate);
                    cmd.Parameters.AddWithValue("@NATUREOFWORK", sdtb1.Text);
                    cmd.Parameters.AddWithValue("@BILL_NO", sdtb2.Text);
                    cmd.Parameters.AddWithValue("@GROSS_AMOUNT", sdtb3.Text);
                    cmd.Parameters.AddWithValue("@SECURITY_DEPOSIT", sdtb4.Text);
                    cmd.Parameters.AddWithValue("@INCOME_TAX", sdtb5.Text);
                    cmd.Parameters.AddWithValue("@SALES_TAX", sdtb6.Text);
                    cmd.Parameters.AddWithValue("@NET_AMOUNT", sdtb7.Text);
                    cmd.Parameters.AddWithValue("@FUND_NAME", sdtb8.Text);
                    cmd.Parameters.AddWithValue("@REGISTERED_BY", MainWindow.userId);
                    cmd.Parameters.AddWithValue("@REMARKS", sdtb9.Text);
                    cmd.Parameters.AddWithValue("@PAYMENT_STATUS", "UNPAID");
                    cmd.Parameters.AddWithValue("@VENDOR_NAME", vendorName);
                    cmd.Parameters.AddWithValue("@LAST_MODIFIED_BY", MainWindow.fullName + ", " + MainWindow.userId);
                    cmd.Parameters.AddWithValue("@LAST_MODIFIED_ON", DateTime.Now);
                    cmd.Parameters.AddWithValue("@PAYMENT_DATE", DateTime.Now);
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                    successGridTB.Text = vendorName + "\nDate: " + sdDatePicker.Text + "\nNature of Work: " + sdtb1.Text + "\nBill Number: " + sdtb2.Text + "\nGross Amount: ₹" + sdtb3.Text + "\nSecurity Deposit: ₹" + sdtb4.Text + "\nIncome Tax: ₹" + sdtb5.Text + "\nSales Tax: ₹" + sdtb6.Text + "\nNet Amount: ₹" + sdtb7.Text + "\nFund Name: " + sdtb8.Text + "\nRemarks: " + sdtb9.Text + "\nRefund Status: Not Refunded\nRegistered by " + MainWindow.fullName + " on " + DateTime.Now;
                    successGrid.Visibility = Visibility.Visible;
                    successButtonGrid(3);
                    Resetsdf();
                }
            } catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
                sdg2errorLabel.Content = "Something went wrong";
                sdg2errorLabel.Visibility = Visibility.Visible;
                Resetsdf();
            }
        }

        //Function to check whether security deposit form is valid or not
        private bool sdfIsValid()
        {
            if(sdDatePicker.SelectedDate == null || sdtb1.Text == string.Empty || sdtb2.Text == string.Empty || sdtb4.Text == string.Empty || sdtb7.Text == string.Empty || sdtb8.Text == string.Empty)
            {
                sdg2errorLabel.Content = "Please fill all the required fields*";
                sdg2errorLabel.Visibility = Visibility.Visible;
                return false;
            }
            return true;
        }

        //Code to continue on the security deposit form through
        private void sdcbGridButton_Click(object sender, RoutedEventArgs e)
        {
            sdId = vendorId;
            sdg2head.Content = vendorName;
            vendorNameLabel.Content = "";
            sdGrid1.Visibility = Visibility.Hidden;
            sdGrid2.Visibility = Visibility.Visible;
            sdGrid1Button.Visibility = Visibility.Hidden;
        }

        //Link Button Event: Event to redirect to vendor Registration Form
        private void VendorFormButtonLink_Click(object sender, RoutedEventArgs e)
        {
            Resetvrf();
            SetVisibilityToHidden();
            vendorFormGrid.Visibility = Visibility.Visible;
        }

        //Link Button Event: Event to redirect to security deposit registration form
        private void SecurityFormButtonLink_Click(object sender, RoutedEventArgs e)
        {
            Resetsdf();
            GetVendorsRecord(2);
            SetVisibilityToHidden();
            searchtb1.Clear();
            searchtb1.Focus();
            securityDepositFormGrid.Visibility = Visibility.Visible;
            sdGrid1.Visibility = Visibility.Visible;
            sdGrid2.Visibility = Visibility.Hidden;
            sdGrid1Button.Visibility = Visibility.Hidden;
            vendorNameLabel.Content = "";
            vendorName = "";
            vendorId = "";
            sdId = "";
        }

        //Code to reset security deposit registration form
        private void Resetsdf()
        {
            sdDatePicker.SelectedDate = null;
            sdtb1.Clear();
            sdtb2.Clear();
            sdtb3.Clear();
            sdtb4.Clear();
            sdtb5.Clear();
            sdtb6.Clear();
            sdtb7.Clear();
            sdtb8.Clear();
            sdtb9.Clear();

            sdDatePicker.Focus();
            sdg2errorLabel.Visibility = Visibility.Hidden;
        }

        //Link Button Event: Event to redirect to earnest money registration form
        private void EarnestFormButtonLink_Click(object sender, RoutedEventArgs e)
        {
            Resetemf();
            GetVendorsRecord(1);
            SetVisibilityToHidden();
            searchtb2.Clear();
            searchtb2.Focus();
            earnestMoneyFormGrid.Visibility = Visibility.Visible;
            emGrid1.Visibility = Visibility.Visible;
            emGrid2.Visibility = Visibility.Hidden;
            emGrid1Button.Visibility = Visibility.Hidden;
            vendorNameLabel2.Content = "";
            vendorName = "";
            vendorId = "";
            emId = "";
        }

        private void sdfGridButton2_Click(object sender, RoutedEventArgs e)
        {
            Resetsdf();
        }

        //Button event to continue to earnest money registration form after selecting a vendor
        private void emcbGridButton_Click(object sender, RoutedEventArgs e)
        {
                emId = vendorName;
                emg2head.Content = vendorName;
                emGrid1.Visibility = Visibility.Hidden;
                emGrid2.Visibility = Visibility.Visible;
        }

        //Button event to insert earnest money data into earnestmoney table
        private void emfGridButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                emg2errorLabel.Visibility = Visibility.Hidden;
                if (emfIsValid())
                {
                    string emDate = emDatePicker.SelectedDate.ToString();
                    cmd = new SqlCommand("INSERT INTO earnestmoney VALUES (@DATE, @NATUREOFWORK, @TENDER_NO, @ESTIMATED_COST, @EARNEST_MONEY, @FUND_NAME, @REGISTERED_BY, @REMARKS, @PAYMENT_STATUS, @VENDOR_NAME, @LAST_MODIFIED_BY, @LAST_MODIFIED_ON, @PAYMENT_DATE)", con);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@DATE", emDate);
                    cmd.Parameters.AddWithValue("@NATUREOFWORK", emtb1.Text);
                    cmd.Parameters.AddWithValue("@TENDER_NO", emtb2.Text);
                    cmd.Parameters.AddWithValue("@ESTIMATED_COST", emtb3.Text);
                    cmd.Parameters.AddWithValue("@EARNEST_MONEY", emtb4.Text);
                    cmd.Parameters.AddWithValue("@FUND_NAME", emtb5.Text);
                    cmd.Parameters.AddWithValue("@REGISTERED_BY", MainWindow.userId);
                    cmd.Parameters.AddWithValue("@REMARKS", emtb6.Text);
                    cmd.Parameters.AddWithValue("@PAYMENT_STATUS", "UNPAID");
                    cmd.Parameters.AddWithValue("@VENDOR_NAME", vendorName);
                    cmd.Parameters.AddWithValue("@LAST_MODIFIED_BY", MainWindow.fullName + ", " + MainWindow.userId);
                    cmd.Parameters.AddWithValue("@LAST_MODIFIED_ON", DateTime.Now);
                    cmd.Parameters.AddWithValue("@PAYMENT_DATE", DateTime.Now);
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                    successGridTB.Text = vendorName + "\nDate: " + emDatePicker.Text + "\nNature of Work: " + emtb1.Text + "\nTender Number: " + emtb2.Text + "\nEstimated Cost: ₹" + emtb3.Text + "\nEarnest Money: ₹" + emtb4.Text + "\nFund Name: " + emtb5.Text + "\nRemarks: " + emtb6.Text + "\nRefund Status: Not Refunded\nRegistered by " + MainWindow.fullName + " on " + DateTime.Now;
                    successGrid.Visibility = Visibility.Visible;
                    successButtonGrid(2);
                    Resetemf();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
                sdg2errorLabel.Content = "Something went wrong";
                sdg2errorLabel.Visibility = Visibility.Visible;
                Resetsdf();
            }
        }

        //Function to check whether earnest money form is valid or not
        private bool emfIsValid()
        {
            if (emDatePicker.SelectedDate == null || emtb1.Text == string.Empty || emtb2.Text == string.Empty || emtb4.Text == string.Empty || emtb5.Text == string.Empty)
            {
                emg2errorLabel.Content = "Please fill all the required fields*";
                emg2errorLabel.Visibility = Visibility.Visible;
                return false;
            }
            return true;
        }

        //Code to clear all the values of earnest money registration form
        private void Resetemf()
        {
            emDatePicker.SelectedDate = null;
            emtb1.Clear();
            emtb2.Clear();
            emtb3.Clear();
            emtb4.Clear();
            emtb5.Clear();
            emtb6.Clear();

            emDatePicker.Focus();
            emg2errorLabel.Visibility = Visibility.Hidden;
        }

        private void emfGridButton2_Click(object sender, RoutedEventArgs e)
        {
            Resetemf();
        }

        //Function to show the specific buttons on the floor of the registration success acknowledgement grid
        private void successButtonGrid(int i)
        {
            int ch = i;
            switch(ch)
            {
                case 1: //Success acknowledgement of vendor registration
                    {
                        vrbl.Visibility = Visibility.Visible;
                        vrbm.Visibility = Visibility.Visible;
                        vrbr.Visibility = Visibility.Visible;
                        embl.Visibility = Visibility.Hidden;
                        embr.Visibility = Visibility.Hidden;
                        sdbl.Visibility = Visibility.Hidden;
                        sdbr.Visibility = Visibility.Hidden;
                        break;
                    }
                case 2: //Success acknowledgement of earnest money registration
                    {
                        vrbl.Visibility = Visibility.Hidden;
                        vrbm.Visibility = Visibility.Hidden;
                        vrbr.Visibility = Visibility.Hidden;
                        embl.Visibility = Visibility.Visible;
                        embr.Visibility = Visibility.Visible;
                        sdbl.Visibility = Visibility.Hidden;
                        sdbr.Visibility = Visibility.Hidden;
                        break;
                    }
                case 3: //Success acknowledgement of security deposit registration
                    {
                        vrbl.Visibility = Visibility.Hidden;
                        vrbm.Visibility = Visibility.Hidden;
                        vrbr.Visibility = Visibility.Hidden;
                        embl.Visibility = Visibility.Hidden;
                        embr.Visibility = Visibility.Hidden;
                        sdbl.Visibility = Visibility.Visible;
                        sdbr.Visibility = Visibility.Visible;
                        break;
                    }
                default:
                    {
                        MessageBox.Show("Something went wrong", "Error");
                        break;
                    }
            }
        }

        //Event of the left button (Go to previous form button) of case 1 of success acknowledgement grid
        private void vrbl_Click(object sender, RoutedEventArgs e)
        {
            Resetvrf();
            vendorName = "";
            successGrid.Visibility = Visibility.Hidden;
            vendorFormGrid.Visibility = Visibility.Visible;
        }

        //Event of the center button (Fill security deposit) of case 1 of success acknowledgement grid
        private void vrbm_Click(object sender, RoutedEventArgs e)
        {
            successGrid.Visibility = Visibility.Hidden;
            sdg2head.Content = vendorName;
            securityDepositFormGrid.Visibility = Visibility.Visible;
            sdGrid1.Visibility = Visibility.Hidden;
            sdGrid2.Visibility = Visibility.Visible;
        }

        //Event of the right button (Fill earnest moeny) of case 1 of success acknowledgement grid
        private void vrbr_Click(object sender, RoutedEventArgs e)
        {
            successGrid.Visibility = Visibility.Hidden;
            emg2head.Content = vendorName;
            earnestMoneyFormGrid.Visibility = Visibility.Visible;
            emGrid1.Visibility = Visibility.Hidden;
            emGrid2.Visibility = Visibility.Visible;
        }

        //Event of the left button (Go to previous form) of case 2 of success acknowledgement grid
        private void embl_Click(object sender, RoutedEventArgs e)
        {
            Resetemf();
            emg2head.Content = vendorName;
            successGrid.Visibility = Visibility.Hidden;
            emGrid1.Visibility = Visibility.Hidden;
            emGrid2.Visibility = Visibility.Visible;
        }

        //Event of the right button (Go to earnest money register) of case 2 of success acknowledgement grid
        private void embr_Click(object sender, RoutedEventArgs e)
        {
            successGrid.Visibility = Visibility.Hidden;
            GetEarnestMoneyRecord();
            emRegisterGrid.Visibility = Visibility.Visible;
        }

        //Event of the left button (Go to previous form) of case 3 of success acknowledgement grid
        private void sdbl_Click(object sender, RoutedEventArgs e)
        {
            Resetsdf();
            sdg2head.Content = vendorName;
            successGrid.Visibility = Visibility.Hidden;
            sdGrid1.Visibility = Visibility.Hidden;
            sdGrid2.Visibility = Visibility.Visible;
        }

        //Event of the right button (Go to security deposit register) of case 3 of success acknowledgement grid
        private void sdbr_Click(object sender, RoutedEventArgs e)
        {
            successGrid.Visibility = Visibility.Hidden;
            GetSecurityDepositRecord();
            sdRegisterGrid.Visibility = Visibility.Visible;
        }

        //Function to load vendor details in
        //choice 1: Vendor Data grid of earnest money
        //choice 2: Vendor Data grid of security deposit
        //choice 3: Vendor Data grid of vendor register
        private void GetVendorsRecord(int choice)
        {
            cmd = new SqlCommand("SELECT VENDORID AS ID, VENDORNAME AS [Vendor Name], CONTACTNUMBER AS [Contact No], ADDRESS AS Address, BANKNAME + ', ' + BRANCH AS Bank, ACCOUNTNO AS [Account No], DETAILS AS Details, FULLNAME + ', ' + USERID AS [Registered By], REGISTERED_ON AS [Registered On], LAST_MODIFIED_BY AS [Last Modified By], LAST_MODIFIED_ON AS [Last Modified On] FROM vendors", con);
            DataTable dt = new DataTable();
            con.Open();
            
            dr = cmd.ExecuteReader();
            dt.Load(dr);

            con.Close();
            if (choice == 1)
                vendorDataGrid1.ItemsSource = dt.DefaultView;
            else if (choice == 2)
                vendorDataGrid2.ItemsSource = dt.DefaultView;
            else if (choice == 3)
                vendorDataGrid3.ItemsSource = dt.DefaultView;
            else
                vendorDataGrid3.ItemsSource = dt.DefaultView;


        }

        //Function to get earnest money details in Earnest Money Data grid
        private void GetEarnestMoneyRecord()
        {
            cmd = new SqlCommand("SELECT EMID AS ID, DATE, NATUREOFWORK AS [Nature of Work],TENDER_NO AS [Tender No], ESTIMATED_COST AS [Estimated Cost], EARNEST_MONEY AS [Earnest money], FUND_NAME AS [Name of the fund], REMARKS AS Remarks, VENDOR_NAME, REGISTERED_BY AS [Registered By], PAYMENT_STATUS AS [Payment Status], LAST_MODIFIED_BY AS [Last Modified By], LAST_MODIFIED_ON AS [Last Modified On] FROM earnestmoney ORDER BY DATE DESC", con);
            DataTable dt = new DataTable();
            con.Open();

            dr = cmd.ExecuteReader();
            dt.Load(dr);

            con.Close();
            EMDataGrid.ItemsSource = dt.DefaultView;

        }

        //Function to get security deposit details in Security Deposit Data grid
        private void GetSecurityDepositRecord()
        {
            cmd = new SqlCommand("SELECT SDID AS ID, DATE, NATUREOFWORK AS [Nature of Work], BILL_NO AS [Bill No], GROSS_AMOUNT AS [Gross Amount], SECURITY_DEPOSIT AS [Security Deposit], INCOME_TAX AS [Income Tax], SALES_TAX AS [Sales Tax], NET_AMOUNT AS [Net Amount], FUND_NAME AS [Name of the fund], REMARKS AS Remarks, VENDOR_NAME, REGISTERED_BY AS [Registered By], PAYMENT_STATUS AS [Payment Status], LAST_MODIFIED_BY AS [Last Modified By], LAST_MODIFIED_ON AS [Last Modified On] FROM securitydeposit ORDER BY DATE DESC", con);
            DataTable dt = new DataTable();
            con.Open();

            dr = cmd.ExecuteReader();
            dt.Load(dr);

            con.Close();
            SDDataGrid.ItemsSource = dt.DefaultView;

        }

        //Link Button Event: Event to redirect to earnest money register grid
        private void EarnestRegisterButtonLink_Click(object sender, RoutedEventArgs e)
        {
            SetVisibilityToHidden();
            GetEarnestMoneyRecord();
            emRegisterGrid.Visibility = Visibility.Visible;
            searchtb4.Clear();
            searchtb4.Focus();
            emDataSelectedLabel.Content = "";
            emDataSelectedLabel2.Content = "";
            emDataSelectedLabel3.Content = "";
            emDeleteButton.Visibility = Visibility.Hidden;
            emEditButton.Visibility = Visibility.Hidden;
            emsubDeleteButtonYes.Visibility = Visibility.Hidden;
            emsubDeleteButtonNo.Visibility = Visibility.Hidden;
            emPaymentButton.Visibility = Visibility.Hidden;
            vendorName = "";
            vendorId = "";
            emId = "";
            tenderno = "";
            natureofwork = "";
        }

        //Link Button Event: Event to redirect to vendor register grid
        private void VendorRegisterButtonLink_Click(object sender, RoutedEventArgs e)
        {          
            searchtb3.Clear();
            searchtb3.Focus();
            SetVisibilityToHidden();
            GetVendorsRecord(3);
            vendorRegisterGrid.Visibility = Visibility.Visible;
            vendorDataSelectedVendorName.Content = "";
            vendorDeleteButton.Visibility = Visibility.Hidden;
            vendorEditButton.Visibility = Visibility.Hidden;
            vendorsubDeleteButtonYes.Visibility = Visibility.Hidden;
            vendorsubDeleteButtonNo.Visibility = Visibility.Hidden;
            vendorName = "";
            vendorId = "";
        }

        //Event to update vendor details triggered when users clicks on edit button of through vendor update grid
        private void vendorEditButton_Click(object sender, RoutedEventArgs e)
        {
            cmd = new SqlCommand("SELECT VENDORID, VENDORNAME, CONTACTNUMBER, ADDRESS, ACCOUNTNO, BANKNAME, BRANCH, DETAILS FROM vendors WHERE VENDORID = @VENDORID", con);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@VENDORID", vendorId);
            con.Open();

            dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                vendorId = dr[0].ToString();
                veftbname.Text = dr[1].ToString();
                veftbContact.Text = dr[2].ToString();
                veftbAddress.Text = dr[3].ToString();
                veftbAccNo.Text = dr[4].ToString();
                veftbBankName.Text = dr[5].ToString();
                veftbBranchName.Text = dr[6].ToString();
                veftbDetails.Text = dr[7].ToString();
            }
            dr.Close();
            con.Close();
            vendorRegisterGrid.Visibility = Visibility.Hidden;
            vendorEditFormGrid.Visibility = Visibility.Visible;
        }

        //Event to delete vendor
        private void vendorDeleteButton_Click(object sender, RoutedEventArgs e)
        {
            SqlCommand cmd = new SqlCommand("DELETE FROM vendors WHERE VENDORID = @VENDORID", con);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@VENDORID", vendorId);
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
            vendorDataSelectedVendorName.Content = vendorName + " deleted successfully.";
            GetVendorsRecord(3);
            vendorsubDeleteButtonYes.Visibility = Visibility.Hidden;
            vendorsubDeleteButtonNo.Visibility = Visibility.Hidden;
        }

        //Link Button Event: Event to redirect tot security deposit register
        private void SecurityDepositRegisterButtonLink_Click(object sender, RoutedEventArgs e)
        {
            SetVisibilityToHidden();
            GetSecurityDepositRecord();
            sdRegisterGrid.Visibility = Visibility.Visible;
            searchtb5.Clear();
            searchtb5.Focus();
            sdDataSelectedLabel.Content = "";
            sdDataSelectedLabel2.Content = "";
            sdDataSelectedLabel3.Content = "";
            sdDeleteButton.Visibility = Visibility.Hidden;
            sdEditButton.Visibility = Visibility.Hidden;
            sdsubDeleteButtonYes.Visibility = Visibility.Hidden;
            sdsubDeleteButtonNo.Visibility = Visibility.Hidden;
            sdPaymentButton.Visibility = Visibility.Hidden;
            vendorName = "";
            vendorId = "";
            sdId = "";
            billno = "";
            natureofwork = "";
        }

        //Event to redirect to security deposit update form of selected row
        private void sdEditButton_Click(object sender, RoutedEventArgs e)
        {
                    cmd = new SqlCommand("SELECT SDID, VENDOR_NAME, DATE, NATUREOFWORK, BILL_NO, GROSS_AMOUNT, SECURITY_DEPOSIT, INCOME_TAX, SALES_TAX, NET_AMOUNT, FUND_NAME, REMARKS, PAYMENT_STATUS, PAYMENT_DATE FROM securitydeposit WHERE SDID = @SDID", con);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@SDID", sdId);
                    con.Open();

                    dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        sdId = dr[0].ToString();
                        sdug2head.Content = dr[1].ToString();
                        sduDateLabel.Content = dr[2].ToString();
                        sdutb1.Text = dr[3].ToString();
                        sdutb2.Text = dr[4].ToString();
                        sdutb3.Text = dr[5].ToString();
                        sdutb4.Text = dr[6].ToString();
                        sdutb5.Text = dr[7].ToString();
                        sdutb6.Text = dr[8].ToString();
                        sdutb7.Text = dr[9].ToString();
                        sdutb8.Text = dr[10].ToString();
                        sdutb9.Text = dr[11].ToString();                       

                        payment_status = dr[12].ToString();
                        if (dr[12].ToString() == "UNPAID")
                        {
                            sdPSL.Content = dr[12].ToString();
                        }
                        else
                        {
                            sdPSL.Content = "Paid On " + dr[13].ToString();
                            if (MainWindow.accountType == "ADMIN")
                                sdPSB.Visibility = Visibility.Visible;                            
                        }
                    }
                    dr.Close();
                    con.Close();
                    sdRegisterGrid.Visibility = Visibility.Hidden;
                    securityDepositUpdateFormGrid.Visibility = Visibility.Visible;
        }

        //Event to delete selected security deposit data
        private void sdDeleteButton_Click(object sender, RoutedEventArgs e)
        {
            SqlCommand cmd = new SqlCommand("DELETE FROM securitydeposit WHERE SDID = @SDID", con);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@SDID", sdId);
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
            GetSecurityDepositRecord();
            sdDataSelectedLabel.Content = "Deleted successfully.";
            sdDataSelectedLabel2.Content = "";
            sdId = "";
            sdsubDeleteButtonYes.Visibility = Visibility.Hidden;
            sdsubDeleteButtonNo.Visibility = Visibility.Hidden;
            sdDataSelectedLabel3.Content = "";
            sdPaymentButton.Visibility = Visibility.Hidden;
        }

        //Event to redirect to earnest money details grid of selected row
        private void emEditButton_Click(object sender, RoutedEventArgs e)
        {
            cmd = new SqlCommand("SELECT EMID, VENDOR_NAME, DATE, NATUREOFWORK, TENDER_NO, ESTIMATED_COST, EARNEST_MONEY, FUND_NAME, REMARKS, PAYMENT_STATUS, PAYMENT_DATE FROM earnestmoney WHERE EMID = @EMID", con);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@EMID", emId);
            con.Open();

            dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                emId = dr[0].ToString();
                emug2head.Content = dr[1].ToString();
                emuDateLabel.Content = dr[2].ToString();
                emutb1.Text = dr[3].ToString();
                emutb2.Text = dr[4].ToString();
                emutb3.Text = dr[5].ToString();
                emutb4.Text = dr[6].ToString();
                emutb5.Text = dr[7].ToString();
                emutb6.Text = dr[8].ToString();
                        
                payment_status = dr[9].ToString();
                if(dr[9].ToString() == "UNPAID")
                {
                    emPSL.Content = dr[9].ToString();
                } else
                {
                     emPSL.Content = "Paid On " + dr[10].ToString();
                        if (MainWindow.accountType == "ADMIN")
                            emPSB.Visibility = Visibility.Visible;
                }
            }
            dr.Close();
            con.Close();
            emRegisterGrid.Visibility = Visibility.Hidden;
            earnestMoneyUpdateFormGrid.Visibility = Visibility.Visible;
        }

        //Event to delete earnest money data of selected row
        private void emDeleteButton_Click(object sender, RoutedEventArgs e)
        {
            SqlCommand cmd = new SqlCommand("DELETE FROM earnestmoney WHERE EMID = @EMID", con);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@EMID", emId);
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
            GetEarnestMoneyRecord();
            emDataSelectedLabel.Content = "Deleted successfully.";
            emDataSelectedLabel2.Content = "";
            emId = "";
            emsubDeleteButtonYes.Visibility = Visibility.Hidden;
            emsubDeleteButtonNo.Visibility = Visibility.Hidden;
            emDataSelectedLabel3.Content = "";
            emPaymentButton.Visibility = Visibility.Hidden;
        }

        //Event to update vendor details of selected vendor
        private void vendorUpdateButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (veftbname.Text == string.Empty || veftbBankName.Text == string.Empty || veftbBranchName.Text == string.Empty || veftbAccNo.Text == string.Empty)
                {
                    veferrorLabel.Visibility = Visibility.Visible;
                }
                else
                {
                    cmd = new SqlCommand("UPDATE vendors SET VENDORNAME = @VENDORNAME, CONTACTNUMBER = @CONTACTNUMBER, ADDRESS = @ADDRESS, ACCOUNTNO = @ACCOUNTNO, BANKNAME = @BANKNAME, BRANCH = @BRANCH, DETAILS = @DETAILS, LAST_MODIFIED_BY = @LAST_MODIFIED_BY, LAST_MODIFIED_ON = @LAST_MODIFIED_ON WHERE VENDORID = @VENDORID", con);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@VENDORID", vendorId);
                    cmd.Parameters.AddWithValue("@VENDORNAME", veftbname.Text);
                    cmd.Parameters.AddWithValue("@CONTACTNUMBER", veftbContact.Text);
                    cmd.Parameters.AddWithValue("@ADDRESS", veftbAddress.Text);
                    cmd.Parameters.AddWithValue("@ACCOUNTNO", veftbAccNo.Text);
                    cmd.Parameters.AddWithValue("@BANKNAME", veftbBankName.Text);
                    cmd.Parameters.AddWithValue("@BRANCH", veftbBranchName.Text);
                    cmd.Parameters.AddWithValue("@DETAILS", veftbDetails.Text);
                    cmd.Parameters.AddWithValue("@LAST_MODIFIED_BY", MainWindow.fullName + ", " + MainWindow.userId);
                    cmd.Parameters.AddWithValue("@LAST_MODIFIED_ON", DateTime.Now);
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                    MessageBox.Show("Data Updated Successfully.", "Saved");
                    veferrorLabel.Visibility = Visibility.Hidden;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Something went wrong", "Error");
                veftbname.Focus();
            }
        }

        //Link Button Event: Event to redirect to initial screen viz. Dashboard
        private void DashboardButtonLink_Click(object sender, RoutedEventArgs e)
        {
            secondDashboardGrid.Visibility = Visibility.Hidden;
            dashboardGrid.Visibility = Visibility.Visible;
        }

        //Link Button Event: Event to redirect to second screen viz. secondDashboard
        private void secondDashboardButtonLink_Click(object sender, RoutedEventArgs e)
        {
            SetVisibilityToHidden();
            dashboardGrid.Visibility = Visibility.Hidden;
            secondDashboardGrid.Visibility = Visibility.Visible;
            vendorFormGrid.Visibility = Visibility.Visible;
        }

        //Event to close or exit the application
        private void exitButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        //Event to update earnest money data of provided earnest money id
        private void emufGridButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (emutb1.Text == string.Empty || emutb2.Text == string.Empty || emutb4.Text == string.Empty || emutb5.Text == string.Empty)
                {
                    emug2errorLabel.Content = "Please fill all the required fields*";
                    emug2errorLabel.Visibility = Visibility.Visible;
                }
                else
                {
                    cmd = new SqlCommand("UPDATE earnestmoney SET NATUREOFWORK = @NATUREOFWORK, TENDER_NO = @TENDER_NO, ESTIMATED_COST = @ESTIMATED_COST, EARNEST_MONEY = @EARNEST_MONEY, FUND_NAME = @FUND_NAME, REMARKS = @REMARKS, LAST_MODIFIED_BY = @LAST_MODIFIED_BY, LAST_MODIFIED_ON = @LAST_MODIFIED_ON WHERE EMID = @EMID", con);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@EMID", emId);
                    cmd.Parameters.AddWithValue("@NATUREOFWORK", emutb1.Text);
                    cmd.Parameters.AddWithValue("@TENDER_NO", emutb2.Text);
                    cmd.Parameters.AddWithValue("@ESTIMATED_COST", emutb3.Text);
                    cmd.Parameters.AddWithValue("@EARNEST_MONEY", emutb4.Text);
                    cmd.Parameters.AddWithValue("@FUND_NAME", emutb5.Text);
                    cmd.Parameters.AddWithValue("@REMARKS", emutb6.Text);
                    cmd.Parameters.AddWithValue("@LAST_MODIFIED_BY", MainWindow.fullName + ", " + MainWindow.userId);
                    cmd.Parameters.AddWithValue("@LAST_MODIFIED_ON", DateTime.Now);
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                    MessageBox.Show("Data Updated Successfully.", "Saved");
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Something went wrong", "Error");
                emutb1.Focus();
            }
        }

        //Event to undo payment status to Unpaid (Only Admin)
        private void emPSB_Click(object sender, RoutedEventArgs e)
        {
            cmd = new SqlCommand("UPDATE earnestmoney SET PAYMENT_STATUS = @PAYMENT_STATUS, LAST_MODIFIED_BY = @LAST_MODIFIED_BY, LAST_MODIFIED_ON = @LAST_MODIFIED_ON, PAYMENT_DATE = @PAYMENT_DATE WHERE EMID = @EMID", con);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@EMID", emId);
            cmd.Parameters.AddWithValue("@PAYMENT_STATUS", "UNPAID");
            cmd.Parameters.AddWithValue("@LAST_MODIFIED_BY", MainWindow.fullName);
            cmd.Parameters.AddWithValue("@LAST_MODIFIED_ON", DateTime.Now);
            cmd.Parameters.AddWithValue("@PAYMENT_DATE", DateTime.Now);
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
            emPSL.Content = "UNPAID";
            MessageBox.Show("Payment status set to UNPAID", "Saved");
            emPSB.Visibility = Visibility.Hidden;
        }

        //Event to update security deposit details of provided security deposit id
        private void sdufGridButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (sdutb1.Text == string.Empty || sdutb2.Text == string.Empty || sdutb4.Text == string.Empty || sdutb7.Text == string.Empty || sdutb8.Text == string.Empty)
                {
                    sdug2errorLabel.Content = "Please fill all the required fields*";
                    sdug2errorLabel.Visibility = Visibility.Visible;
                }
                else
                {
                    cmd = new SqlCommand("UPDATE securitydeposit SET NATUREOFWORK = @NATUREOFWORK, BILL_NO = @BILL_NO, GROSS_AMOUNT = @GROSS_AMOUNT, SECURITY_DEPOSIT = @SECURITY_DEPOSIT, INCOME_TAX = @INCOME_TAX, SALES_TAX = @SALES_TAX, NET_AMOUNT = @NET_AMOUNT, FUND_NAME = @FUND_NAME, REMARKS = @REMARKS, LAST_MODIFIED_BY = @LAST_MODIFIED_BY, LAST_MODIFIED_ON = @LAST_MODIFIED_ON WHERE SDID = @SDID", con);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@SDID", sdId);
                    cmd.Parameters.AddWithValue("@NATUREOFWORK", sdutb1.Text);
                    cmd.Parameters.AddWithValue("@BILL_NO", sdutb2.Text);
                    cmd.Parameters.AddWithValue("@GROSS_AMOUNT", sdutb3.Text);
                    cmd.Parameters.AddWithValue("@SECURITY_DEPOSIT", sdutb4.Text);
                    cmd.Parameters.AddWithValue("@INCOME_TAX", sdutb5.Text);
                    cmd.Parameters.AddWithValue("@SALES_TAX", sdutb6.Text);
                    cmd.Parameters.AddWithValue("@NET_AMOUNT", sdutb7.Text);
                    cmd.Parameters.AddWithValue("@FUND_NAME", sdutb8.Text);
                    cmd.Parameters.AddWithValue("@REMARKS", sdutb9.Text);
                    cmd.Parameters.AddWithValue("@LAST_MODIFIED_BY", MainWindow.fullName + ", " + MainWindow.userId);
                    cmd.Parameters.AddWithValue("@LAST_MODIFIED_ON", DateTime.Now);
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                    MessageBox.Show("Data Updated Successfully.", "Saved");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
                sdutb1.Focus();
            }
        }

        //Event to set the payment status to unpaid (Only Admin)
        private void sdPSB_Click(object sender, RoutedEventArgs e)
        {
            cmd = new SqlCommand("UPDATE securitydeposit SET PAYMENT_STATUS = @PAYMENT_STATUS, LAST_MODIFIED_BY = @LAST_MODIFIED_BY, LAST_MODIFIED_ON = @LAST_MODIFIED_ON, PAYMENT_DATE = @PAYMENT_DATE WHERE SDID = @SDID", con);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@SDID", sdId);
            cmd.Parameters.AddWithValue("@PAYMENT_STATUS", "UNPAID");
            cmd.Parameters.AddWithValue("@LAST_MODIFIED_BY", MainWindow.fullName);
            cmd.Parameters.AddWithValue("@LAST_MODIFIED_ON", DateTime.Now);
            cmd.Parameters.AddWithValue("@PAYMENT_DATE", DateTime.Now);
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
            sdPSL.Content = "UNPAID";
            MessageBox.Show("Payment status set to UNPAID", "Saved");
            sdPSB.Visibility = Visibility.Hidden;
        }

        //Text changed event of search text box 1 - Search box of earnest money registration grid
        private void searchtb1_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchparams = searchtb1.Text;
            GetSearchData(searchparams, 2);
        }

        //Text changed event of search text box 2 - Search box of security deposit registration grid
        private void searchtb2_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchparams = searchtb2.Text;
            GetSearchData(searchparams, 1);
        }

        //Function to update data in the data grid according to provided parameters in search text box
        private void GetSearchData(string str1, int ch)
        {
            cmd = new SqlCommand("SELECT VENDORID AS ID, VENDORNAME AS [Vendor Name], CONTACTNUMBER AS [Contact No], ADDRESS AS Address, BANKNAME + ', ' + BRANCH AS Bank, ACCOUNTNO AS [Account No], DETAILS AS Details, FULLNAME + ', ' + USERID AS [Registered By], REGISTERED_ON AS [Registered On], LAST_MODIFIED_BY AS [Last Modified By], LAST_MODIFIED_ON AS [Last Modified On] FROM vendors WHERE VENDORNAME LIKE @VENDORNAME + '%' OR VENDORID LIKE @VENDORID + '%'", con);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@VENDORID", str1);
            cmd.Parameters.AddWithValue("@VENDORNAME", str1);

            DataTable dt = new DataTable();
            con.Open();

            dr = cmd.ExecuteReader();
            dt.Load(dr);

            con.Close();

            switch (ch)
            {
                case 1:
                    vendorDataGrid1.ItemsSource = dt.DefaultView;
                    break;
                case 2:
                    vendorDataGrid2.ItemsSource = dt.DefaultView;
                    break;
                case 3:
                    vendorDataGrid3.ItemsSource = dt.DefaultView;
                    break;
                default:
                    MessageBox.Show("Something went wrong", "OOPS");
                    break;
            }
            
        }

        //Selection Changed Event of vendor Data grid 2 - Gets triggered when user selects a row from the Data grid
        private void vendorDataGrid2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            object item = vendorDataGrid2.SelectedItem;
            if(item != null)
            {
                vendorName = (vendorDataGrid2.SelectedCells[1].Column.GetCellContent(item) as TextBlock).Text;
                vendorId = (vendorDataGrid2.SelectedCells[0].Column.GetCellContent(item) as TextBlock).Text;
                vendorNameLabel.Content = "Vendor name: " + vendorName;
                sdGrid1Button.Visibility = Visibility.Visible;
            }
        }

        //Selection Changed Event of vendor Data grid 1 - Gets triggered when user selects a row from the Data grid
        private void vendorDataGrid1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            object item = vendorDataGrid1.SelectedItem;
            if (item != null)
            {
                vendorName = (vendorDataGrid1.SelectedCells[1].Column.GetCellContent(item) as TextBlock).Text;
                vendorId = (vendorDataGrid1.SelectedCells[0].Column.GetCellContent(item) as TextBlock).Text;
                vendorNameLabel2.Content = "Vendor name: " + vendorName;
                emGrid1Button.Visibility = Visibility.Visible;
            }
        }

        //Selection Changed Event of vendor Data grid 3 - Gets triggered when user selects a row from the Data grid
        private void vendorDataGrid3_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            object item = vendorDataGrid3.SelectedItem;
            if (item != null)
            {
                vendorName = (vendorDataGrid3.SelectedCells[1].Column.GetCellContent(item) as TextBlock).Text;
                vendorId = (vendorDataGrid3.SelectedCells[0].Column.GetCellContent(item) as TextBlock).Text;
                vendorDataSelectedVendorName.Content = "Vendor name: " + vendorName;
                if (MainWindow.accountType == "ADMIN")
                    vendorDeleteButton.Visibility = Visibility.Visible;
                vendorEditButton.Visibility = Visibility.Visible;
            }
        }

        //Text Changed Event of search text box 3
        private void searchtb3_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchparams = searchtb3.Text;
            GetSearchData(searchparams, 3);
        }

        //Event to delete vendors
        private void vendorDeleteButton1_Click(object sender, RoutedEventArgs e)
        {
            vendorDataSelectedVendorName.Content = "Delete " + vendorName + "?";
            vendorDeleteButton.Visibility = Visibility.Hidden;
            vendorEditButton.Visibility = Visibility.Hidden;
            vendorsubDeleteButtonYes.Visibility = Visibility.Visible;
            vendorsubDeleteButtonNo.Visibility = Visibility.Visible;
        }

        //Event to cancel deletion of vendors
        private void vendorDeleteCancelButton_Click(object sender, RoutedEventArgs e)
        {
            vendorsubDeleteButtonNo.Visibility = Visibility.Hidden;
            vendorsubDeleteButtonYes.Visibility = Visibility.Hidden;
            vendorDataSelectedVendorName.Content = "Vendor name: " + vendorName;
            if (MainWindow.accountType == "ADMIN")
                vendorDeleteButton.Visibility = Visibility.Visible;
            vendorEditButton.Visibility = Visibility.Visible;
        }

        //Text Changed Event of search text box 4
        private void searchtb4_TextChanged(object sender, TextChangedEventArgs e)
        {
            cmd = new SqlCommand("SELECT EMID AS ID, DATE, NATUREOFWORK AS [Nature of Work],TENDER_NO AS [Tender No], ESTIMATED_COST AS [Estimated Cost], EARNEST_MONEY AS [Earnest money], FUND_NAME AS [Name of the fund], REMARKS AS Remarks, VENDOR_NAME, REGISTERED_BY AS [Registered By], PAYMENT_STATUS AS [Payment Status], LAST_MODIFIED_BY AS [Last Modified By], LAST_MODIFIED_ON AS [Last Modified On] FROM earnestmoney WHERE VENDOR_NAME LIKE @VENDORNAME + '%' OR EMID LIKE @EMID + '%' OR TENDER_NO LIKE @TENDERNO + '%' OR NATUREOFWORK LIKE @NATUREOFWORK + '%'", con);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@EMID", searchtb4.Text);
            cmd.Parameters.AddWithValue("@VENDORNAME", searchtb4.Text);
            cmd.Parameters.AddWithValue("@TENDERNO", searchtb4.Text);
            cmd.Parameters.AddWithValue("@NATUREOFWORK", searchtb4.Text);

            DataTable dt = new DataTable();
            con.Open();

            dr = cmd.ExecuteReader();
            dt.Load(dr);

            con.Close();
            EMDataGrid.ItemsSource = dt.DefaultView;
        }

        //Selection Changed Event of earnest money Data grid - Gets triggered when user selects a row from the Data grid
        private void EMDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            object item = EMDataGrid.SelectedItem;
            if (item != null)
            {
                vendorName = (EMDataGrid.SelectedCells[8].Column.GetCellContent(item) as TextBlock).Text;
                tenderno = (EMDataGrid.SelectedCells[3].Column.GetCellContent(item) as TextBlock).Text;
                natureofwork = (EMDataGrid.SelectedCells[2].Column.GetCellContent(item) as TextBlock).Text;
                emId = (EMDataGrid.SelectedCells[0].Column.GetCellContent(item) as TextBlock).Text;
                payment_status = (EMDataGrid.SelectedCells[10].Column.GetCellContent(item) as TextBlock).Text;
                if (payment_status == "PAID")
                    receiptButton1.Visibility = Visibility.Visible;
                cmd = new SqlCommand("SELECT PAYMENT_DATE FROM earnestmoney WHERE EMID = @EMID", con);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@EMID", emId);
                con.Open();
                dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    payment_date = dr[0].ToString();
                }              
                con.Close();                
                emDataSelectedLabel.Content = "Tender No: " + tenderno + ", " + natureofwork;
                emDataSelectedLabel2.Content = "Vendor: " + vendorName;
                if (payment_status == "UNPAID")
                {
                    emDataSelectedLabel3.Content = "Payment status:  UNPAID";
                    emPaymentButton.Visibility = Visibility.Visible;
                } else
                {
                    emPaymentButton.Visibility = Visibility.Hidden;
                    emDataSelectedLabel3.Content = "Payment status: Refunded on " + payment_date;
                }               
                if (MainWindow.accountType == "ADMIN")
                    emDeleteButton.Visibility = Visibility.Visible;
                emEditButton.Visibility = Visibility.Visible;
            }
        }

        //Event to delete earnest money details of given id
        private void emDeleteButton1_Click(object sender, RoutedEventArgs e)
        {
            emDataSelectedLabel.Content = "";
            emDataSelectedLabel2.Content = "Delete the selected item?";
            emDeleteButton.Visibility = Visibility.Hidden;
            emEditButton.Visibility = Visibility.Hidden;
            emsubDeleteButtonYes.Visibility = Visibility.Visible;
            emsubDeleteButtonNo.Visibility = Visibility.Visible;
        }

        //Event to cancel delete event of earnest money
        private void emDeleteCancelButton_Click(object sender, RoutedEventArgs e)
        {
            emsubDeleteButtonNo.Visibility = Visibility.Hidden;
            emsubDeleteButtonYes.Visibility = Visibility.Hidden;
            emDataSelectedLabel.Content = "Tender No: " + tenderno + ", " + natureofwork;
            emDataSelectedLabel2.Content = "Vendor: " + vendorName;
            if (MainWindow.accountType == "ADMIN")
                emDeleteButton.Visibility = Visibility.Visible;
            emEditButton.Visibility = Visibility.Visible;
        }

        //Event to set the payment status to paid of earnest money
        private void emPaymentButton_Click(object sender, RoutedEventArgs e)
        {
            cmd = new SqlCommand("UPDATE earnestmoney SET PAYMENT_STATUS = @PAYMENT_STATUS, PAYMENT_DATE = @PAYMENT_DATE, LAST_MODIFIED_BY = @LAST_MODIFIED_BY, LAST_MODIFIED_ON = @LAST_MODIFIED_ON WHERE EMID = @EMID", con);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@EMID", emId);
            cmd.Parameters.AddWithValue("@PAYMENT_STATUS", "PAID");
            cmd.Parameters.AddWithValue("@PAYMENT_DATE", DateTime.Now);
            cmd.Parameters.AddWithValue("@LAST_MODIFIED_BY", MainWindow.fullName + ", " + MainWindow.userId);
            cmd.Parameters.AddWithValue("@LAST_MODIFIED_ON", DateTime.Now);
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
            GetEarnestMoneyRecord();
            emDataSelectedLabel3.Content = "Payment status: Refunded on " + DateTime.Now;
            emPaymentButton.Visibility = Visibility.Hidden;
            receiptButton1.Visibility = Visibility.Visible;
        }

        //Text changed event of search text box 5
        private void searchtb5_TextChanged(object sender, TextChangedEventArgs e)
        {
            cmd = new SqlCommand("SELECT SDID AS ID, DATE, NATUREOFWORK AS [Nature of Work], BILL_NO AS [Bill No], GROSS_AMOUNT AS [Gross Amount], SECURITY_DEPOSIT AS [Security Deposit], INCOME_TAX AS [Income Tax], SALES_TAX AS [Sales Tax], NET_AMOUNT AS [Net Amount], FUND_NAME AS [Name of the fund], REMARKS AS Remarks, VENDOR_NAME, REGISTERED_BY AS [Registered By], PAYMENT_STATUS AS [Payment Status], LAST_MODIFIED_BY AS [Last Modified By], LAST_MODIFIED_ON AS [Last Modified On] FROM securitydeposit WHERE VENDOR_NAME LIKE @VENDORNAME + '%' OR SDID LIKE @SDID + '%' OR BILL_NO LIKE @BILLNO + '%' OR NATUREOFWORK LIKE @NATUREOFWORK + '%'", con);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@SDID", searchtb5.Text);
            cmd.Parameters.AddWithValue("@VENDORNAME", searchtb5.Text);
            cmd.Parameters.AddWithValue("@BILLNO", searchtb5.Text);
            cmd.Parameters.AddWithValue("@NATUREOFWORK", searchtb5.Text);

            DataTable dt = new DataTable();
            con.Open();

            dr = cmd.ExecuteReader();
            dt.Load(dr);

            con.Close();
            SDDataGrid.ItemsSource = dt.DefaultView;
        }

        //Selection Changed Event of security deposit Data grid - Gets triggered when user selects a row from the Data grid
        private void SDDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            object item = SDDataGrid.SelectedItem;
            if (item != null)
            {
                vendorName = (SDDataGrid.SelectedCells[11].Column.GetCellContent(item) as TextBlock).Text;
                billno = (SDDataGrid.SelectedCells[3].Column.GetCellContent(item) as TextBlock).Text;
                natureofwork = (SDDataGrid.SelectedCells[2].Column.GetCellContent(item) as TextBlock).Text;
                sdId = (SDDataGrid.SelectedCells[0].Column.GetCellContent(item) as TextBlock).Text;
                payment_status = (SDDataGrid.SelectedCells[13].Column.GetCellContent(item) as TextBlock).Text;
                if (payment_status == "PAID")
                    receiptButton2.Visibility = Visibility.Visible;
                cmd = new SqlCommand("SELECT PAYMENT_DATE FROM securitydeposit WHERE SDID = @SDID", con);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@SDID", sdId);
                con.Open();
                dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    payment_date = dr[0].ToString();
                }
                con.Close();
                sdDataSelectedLabel.Content = "Bill No: " + billno + ", " + natureofwork;
                sdDataSelectedLabel2.Content = "Vendor: " + vendorName;
                if (payment_status == "UNPAID")
                {
                    sdDataSelectedLabel3.Content = "Payment status:  UNPAID";
                    sdPaymentButton.Visibility = Visibility.Visible;
                }
                else
                {
                    sdPaymentButton.Visibility = Visibility.Hidden;
                    sdDataSelectedLabel3.Content = "Payment status: Refunded on " + payment_date;
                }
                if (MainWindow.accountType == "ADMIN")
                    sdDeleteButton.Visibility = Visibility.Visible;
                sdEditButton.Visibility = Visibility.Visible;
            }
        }
    
        //Event to delete security deposit details of given id
        private void sdDeleteButton1_Click(object sender, RoutedEventArgs e)
        {
            sdDataSelectedLabel.Content = "";
            sdDataSelectedLabel2.Content = "Delete the selected item?";
            sdDeleteButton.Visibility = Visibility.Hidden;
            sdEditButton.Visibility = Visibility.Hidden;
            sdsubDeleteButtonYes.Visibility = Visibility.Visible;
            sdsubDeleteButtonNo.Visibility = Visibility.Visible;
        }

        //Event to set the payment status to paid of security deposit
        private void sdPaymentButton_Click(object sender, RoutedEventArgs e)
        {
            cmd = new SqlCommand("UPDATE securitydeposit SET PAYMENT_STATUS = @PAYMENT_STATUS, PAYMENT_DATE = @PAYMENT_DATE, LAST_MODIFIED_BY = @LAST_MODIFIED_BY, LAST_MODIFIED_ON = @LAST_MODIFIED_ON WHERE SDID = @SDID", con);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@SDID", sdId);
            cmd.Parameters.AddWithValue("@PAYMENT_STATUS", "PAID");
            cmd.Parameters.AddWithValue("@PAYMENT_DATE", DateTime.Now);
            cmd.Parameters.AddWithValue("@LAST_MODIFIED_BY", MainWindow.fullName + ", " + MainWindow.userId);
            cmd.Parameters.AddWithValue("@LAST_MODIFIED_ON", DateTime.Now);
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
            GetSecurityDepositRecord();
            sdDataSelectedLabel3.Content = "Payment status: Refunded on " + DateTime.Now;
            sdPaymentButton.Visibility = Visibility.Hidden;
            receiptButton2.Visibility = Visibility.Visible;
        }

        //Event to cancel delete operation of security deposit
        private void sdDeleteCancelButton_Click(object sender, RoutedEventArgs e)
        {
            sdsubDeleteButtonNo.Visibility = Visibility.Hidden;
            sdsubDeleteButtonYes.Visibility = Visibility.Hidden;
            sdDataSelectedLabel.Content = "Bill No: " + billno + ", " + natureofwork;
            sdDataSelectedLabel2.Content = "Vendor: " + vendorName;
            if (MainWindow.accountType == "ADMIN")
                sdDeleteButton.Visibility = Visibility.Visible;
            sdEditButton.Visibility = Visibility.Visible;
        }

        //Event to add logs
        private void logsAddButton_Click(object sender, RoutedEventArgs e)
        {
            cmd = new SqlCommand("INSERT INTO logs VALUES (@log, @written_by, @date)", con);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@log", logstb.Text);
            cmd.Parameters.AddWithValue("@written_by", MainWindow.userId + ", " + MainWindow.fullName);
            cmd.Parameters.AddWithValue("@date", DateTime.Now);
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
            GetLogsRecord();
        }

        //Function to get all logs
        private void GetLogsRecord()
        {
            cmd = new SqlCommand("SELECT * FROM logs ORDER BY date DESC", con);
            DataTable dt = new DataTable();
            con.Open();

            dr = cmd.ExecuteReader();
            dt.Load(dr);

            con.Close();
            logsDataGrid.ItemsSource = dt.DefaultView;
        }

        //Selection Changed event of logs data grid
        private void logsDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            object item = logsDataGrid.SelectedItem;
            if (item != null)
            {
                logsId = (logsDataGrid.SelectedCells[0].Column.GetCellContent(item) as TextBlock).Text;
                logsDataSelectedLabel.Content = "Log Number: " + logsId;
                logsDeleteButton.Visibility = Visibility.Visible;
            }
        }

        //Event to delete logs
        private void logsDeleteButton_Click(object sender, RoutedEventArgs e)
        {
            cmd = new SqlCommand("DELETE FROM logs WHERE Id = @Id", con);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@Id", logsId);
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
            logsDataSelectedLabel.Content = "Log Number " + logsId + " deleted successfully.";
            GetLogsRecord();
            logsDeleteButton.Visibility = Visibility.Hidden;
            logsId = "";
        }

        //Link Button Event: Event to redirect to logs grid
        private void LogsButtonLink_Click(object sender, RoutedEventArgs e)
        {
            SetVisibilityToHidden();
            GetLogsRecord();
            logsId = "";
            logstb.Clear();
            logstb.Focus();
            logsDataSelectedLabel.Content = "";
            logsDeleteButton.Visibility = Visibility.Hidden;
            logsGrid.Visibility = Visibility.Visible;
        }

        //Event to generate security deposit receipt
        private void receiptButton2_Click(object sender, RoutedEventArgs e)
        {
            cmd = new SqlCommand("SELECT SECURITY_DEPOSIT, REMARKS FROM securitydeposit WHERE SDID = @SDID", con);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@SDID", sdId);
            con.Open();
            dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                securityDeposit = dr[0].ToString();
                remarks = dr[1].ToString();
            }
            dr.Close();
            con.Close();
            GetVendorsInfo();
            printFormNo = "2";
            PrintWindow printWindow = new PrintWindow();
            printWindow.Show();
        }

        //Event to generate earnest money receipt
        private void receiptButton1_Click(object sender, RoutedEventArgs e)
        {
            cmd = new SqlCommand("SELECT ESTIMATED_COST, EARNEST_MONEY, REMARKS FROM earnestmoney WHERE EMID = @EMID", con);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@EMID", emId);
            con.Open();
            dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                estimatedCost = dr[0].ToString();
                earnestMoney = dr[1].ToString();
                remarks = dr[2].ToString();
            }
            dr.Close();
            con.Close();
            GetVendorsInfo();
            printFormNo = "1";
            PrintWindow printWindow = new PrintWindow();
            printWindow.Show();
        }

        //Event to get vendor details of specific vendor
        private void GetVendorsInfo()
        {
            cmd = new SqlCommand("SELECT ADDRESS, ACCOUNTNO, BANKNAME, BRANCH FROM vendors WHERE VENDORNAME = @VENDORNAME", con);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@VENDORNAME", vendorName);
            con.Open();
            dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                vendorAddress = dr[0].ToString();
                vendorAccNo = dr[1].ToString();
                vendorBankName = dr[2].ToString();
                vendorBranchName = dr[3].ToString();
            }
            dr.Close();
            con.Close();
        }

        //Event to check password to change password
        private void oldpasswordCheckerButton_Click(object sender, RoutedEventArgs e)
        {
            string oldpassword = "";
            if (passwordBox1.Password == string.Empty)
                passwordErrorLabel.Content = "Please provide current password*";
            else
            {
                cmd = new SqlCommand("SELECT PASSWORD FROM users WHERE USERID = @USERID", con);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@USERID", MainWindow.userId);
                con.Open();
                dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    oldpassword = dr[0].ToString();
                }
                dr.Close();
                con.Close();
                if(passwordBox1.Password == oldpassword)
                {
                    passwordBox1.Clear();
                    passwordBox2.Visibility = Visibility.Visible;
                    passwordLabel1.Content = "New password*";
                    passwordLabel2.Content = "Confirm once*";
                    passwordErrorLabel.Content = "";
                    passwordBox1.Focus();
                    oldpasswordCheckerButton.Visibility = Visibility.Hidden;
                    passwordUpdateButton.Visibility = Visibility.Visible;
                } else
                {
                    passwordErrorLabel.Content = "Invalid password";
                    passwordBox1.Clear();
                    passwordBox1.Focus();
                }
            }
        }

        //Selection changed event of users data grid
        private void usersDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            object item = usersDataGrid.SelectedItem;
            if (item != null)
            {
                userId = (usersDataGrid.SelectedCells[0].Column.GetCellContent(item) as TextBlock).Text;
                usersDataSelectedLabel2.Content = "Name: " + (usersDataGrid.SelectedCells[1].Column.GetCellContent(item) as TextBlock).Text;
                userRole = (usersDataGrid.SelectedCells[2].Column.GetCellContent(item) as TextBlock).Text;
                userStatus = (usersDataGrid.SelectedCells[3].Column.GetCellContent(item) as TextBlock).Text;
                usersDataSelectedLabel1.Content = "User Id: " + userId;
                usersDataSelectedLabel3.Content = "Role: " + userRole + ", Account: " + userStatus;
                
                if (MainWindow.accountType == "ADMIN" && userStatus == "ENABLED")
                    usersDisableButton.Visibility = Visibility.Visible;
                if (MainWindow.accountType == "ADMIN" && userStatus == "DISABLED")
                    usersEnableButton.Visibility = Visibility.Visible;
                if (MainWindow.userId == userId)
                {
                    usersDisableButton.Visibility = Visibility.Hidden;
                    usersEnableButton.Visibility = Visibility.Hidden;
                }
            }
        }
        
        //Function to set all the buttons and textboxes including variables to hidden and ""
        private void usersPasswordsMessClear()
        {
            passwordBox1.Clear();
            passwordBox2.Clear();
            passwordBox1.Visibility = Visibility.Hidden;
            passwordBox2.Visibility = Visibility.Hidden;
            passwordLabel1.Content = "";
            passwordLabel2.Content = "";
            passwordErrorLabel.Content = "";
            cancelButton.Visibility = Visibility.Hidden;
            oldpasswordCheckerButton.Visibility = Visibility.Hidden;
            passwordUpdateButton.Visibility = Visibility.Hidden;
            userPasswordChangeButton.Visibility = Visibility.Visible;
        }

        //Invoking usersPasswordMessClear()
        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            usersPasswordsMessClear();
        }

        //Event to disable user (Only admin)
        private void usersDisableButton_Click(object sender, RoutedEventArgs e)
        {
            cmd = new SqlCommand("UPDATE users SET STATUS = @STATUS, LAST_MODIFIED_ON = @LAST_MODIFIED_ON WHERE USERID = @USERID", con);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@STATUS", "DISABLED");
            cmd.Parameters.AddWithValue("@USERID", userId);
            cmd.Parameters.AddWithValue("@LAST_MODIFIED_ON", DateTime.Now);
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
            MessageBox.Show("User Disabled", "Done");
            userStatus = "DISABLED";
            usersDataSelectedLabel3.Content = "Role: " + userRole + ", Account: " + userStatus;
            usersDisableButton.Visibility = Visibility.Hidden;
            usersEnableButton.Visibility = Visibility.Visible;
            GetUsersRecord();
        }

        //Event ot update password
        private void passwordUpdateButton_Click(object sender, RoutedEventArgs e)
        {
            if (passwordBox1.Password == string.Empty || passwordBox1.Password == string.Empty)
            {
                passwordErrorLabel.Content = "Please provide passwords in both fields*";
            } else if (passwordBox1.Password != passwordBox2.Password)
            {
                passwordErrorLabel.Content = "Password not matching";
            } else
            {
                cmd = new SqlCommand("UPDATE users SET PASSWORD = @PASSWORD, LAST_MODIFIED_ON = @LAST_MODIFIED_ON WHERE USERID = @USERID", con);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@PASSWORD", passwordBox1.Password);
                cmd.Parameters.AddWithValue("@USERID", MainWindow.userId);
                cmd.Parameters.AddWithValue("@LAST_MODIFIED_ON", DateTime.Now);
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
                MessageBox.Show("Password changed successfully", "Done");
                usersPasswordsMessClear();
                GetUsersRecord();
            }
        }

        //Event to show password boxes and password update button
        private void userPasswordChangeButton_Click(object sender, RoutedEventArgs e)
        {
            passwordBox1.Visibility = Visibility.Visible;
            passwordLabel1.Content = "Old password*";
            cancelButton.Visibility = Visibility.Visible;
            oldpasswordCheckerButton.Visibility = Visibility.Visible;
            passwordBox1.Focus();
            userPasswordChangeButton.Visibility = Visibility.Hidden;
        }

        //Event to enable user (Only admin)
        private void usersEnableButton_Click(object sender, RoutedEventArgs e)
        {
            cmd = new SqlCommand("UPDATE users SET STATUS = @STATUS, LAST_MODIFIED_ON = @LAST_MODIFIED_ON WHERE USERID = @USERID", con);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@STATUS", "ENABLED");
            cmd.Parameters.AddWithValue("@USERID", userId);
            cmd.Parameters.AddWithValue("@LAST_MODIFIED_ON", DateTime.Now);
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
            MessageBox.Show("User enabled", "Done");
            userStatus = "ENABLED";
            usersDataSelectedLabel3.Content = "Role: " + userRole + ", Account: " + userStatus;
            usersDisableButton.Visibility = Visibility.Visible;
            usersEnableButton.Visibility = Visibility.Hidden;
            GetUsersRecord();
        }

        //Function to get user details
        private void GetUsersRecord()
        {
            cmd = new SqlCommand("SELECT USERID, FULLNAME, ROLE, STATUS, REGISTERED_ON, LAST_MODIFIED_ON FROM users ORDER BY REGISTERED_ON DESC", con);
            DataTable dt = new DataTable();
            con.Open();

            dr = cmd.ExecuteReader();
            dt.Load(dr);

            con.Close();
            usersDataGrid.ItemsSource = dt.DefaultView;
        }

        //Link Button Event: Event to redirect to user settings grid
        private void settingsButtonLink_Click(object sender, RoutedEventArgs e)
        {
            SetVisibilityToHidden();
            usersPasswordsMessClear();
            usersDataSelectedLabel1.Content = "";
            usersDataSelectedLabel2.Content = "";
            usersDataSelectedLabel3.Content = "";
            usersDisableButton.Visibility = Visibility.Hidden;
            usersEnableButton.Visibility = Visibility.Hidden;
            GetUsersRecord();
            usersGrid.Visibility = Visibility.Visible;
        }

        //Link Button Event: Event to redirect to security deposit registration form grid through dashboard
        private void securityDepositLink_Click(object sender, RoutedEventArgs e)
        {
            dashboardGrid.Visibility = Visibility.Hidden;
            secondDashboardGrid.Visibility = Visibility.Visible;
            Resetsdf();
            GetVendorsRecord(2);
            SetVisibilityToHidden();
            searchtb1.Clear();
            searchtb1.Focus();
            securityDepositFormGrid.Visibility = Visibility.Visible;
            sdGrid1.Visibility = Visibility.Visible;
            sdGrid2.Visibility = Visibility.Hidden;
            sdGrid1Button.Visibility = Visibility.Hidden;
            vendorNameLabel.Content = "";
            vendorName = "";
            vendorId = "";
            sdId = "";
        }

        //Link Button Event: Event to redirect to earnest money registration form grid through dashboard
        private void EarnestMoneyLink_Click(object sender, RoutedEventArgs e)
        {
            dashboardGrid.Visibility = Visibility.Hidden;
            secondDashboardGrid.Visibility = Visibility.Visible;
            Resetemf();
            GetVendorsRecord(1);
            SetVisibilityToHidden();
            searchtb2.Clear();
            searchtb2.Focus();
            earnestMoneyFormGrid.Visibility = Visibility.Visible;
            emGrid1.Visibility = Visibility.Visible;
            emGrid2.Visibility = Visibility.Hidden;
            emGrid1Button.Visibility = Visibility.Hidden;
            vendorNameLabel2.Content = "";
            vendorName = "";
            vendorId = "";
            emId = "";
        }

        //Link Button Event: Event to redirect to vendors registration form grid through dashboard
        private void RegisterVendorsLink_Click(object sender, RoutedEventArgs e)
        {
            Resetvrf();
            SetVisibilityToHidden();
            dashboardGrid.Visibility = Visibility.Hidden;
            secondDashboardGrid.Visibility = Visibility.Visible;
            vendorFormGrid.Visibility = Visibility.Visible;
        }
    }
}
