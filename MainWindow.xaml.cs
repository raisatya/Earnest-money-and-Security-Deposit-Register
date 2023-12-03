using System;
using System.Windows;
using System.Windows.Controls;
using System.Data;
using System.Data.SqlClient;

namespace MSC_WPF_BETA
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            logInGrid1.Visibility = Visibility.Visible;
            logInTB1UserId.Focus();
        }

        //Sql connection string
        SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\Database1.mdf;Integrated Security=True");
        
        //Global variable declarations
        readonly string securityKey = "spiritualroute123";
        readonly string adminKey = "extensivesource321";
        public static string userId = "";
        public static string fullName = "";
        public static string accountType = "";
        SqlCommand cmd;
        SqlDataReader sqqr;
        //End of global variable declaration

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void usersDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        //Event triggered when users clicks on sign up button
        //Event to sign up new users
        private void SignupButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SignupButton.Visibility = Visibility.Hidden;
                exitButton2.Visibility = Visibility.Hidden;
                signingInLabel.Visibility = Visibility.Visible;
                errorLabel.Visibility = Visibility.Hidden;
                if (signUpFormDataIsValid())
                {                   
                    cmd = new SqlCommand("INSERT INTO users VALUES (@USERID, @FULLNAME, @PASSWORD, @ROLE, @STATUS, @REGISTERED_ON, @LAST_MODIFIED_ON)", con);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@USERID", signUpTBUserId.Text);
                    cmd.Parameters.AddWithValue("@FULLNAME", signUpTBName.Text);
                    cmd.Parameters.AddWithValue("@PASSWORD", signUpPBPassword.Password);
                    cmd.Parameters.AddWithValue("@ROLE", accountType);
                    cmd.Parameters.AddWithValue("@STATUS", "ENABLED");
                    cmd.Parameters.AddWithValue("@REGISTERED_ON", DateTime.Now);
                    cmd.Parameters.AddWithValue("@LAST_MODIFIED_ON", DateTime.Now);
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                    userId = signUpTBUserId.Text;
                    fullName = signUpTBName.Text;
                    Dashboard dashboard = new Dashboard();
                    dashboard.Show();
                    SignupButton.Visibility = Visibility.Visible;
                    exitButton2.Visibility = Visibility.Visible;
                    signingInLabel.Visibility = Visibility.Hidden;
                    this.Close();
                }
            } catch (Exception)
            {
                ResetSignUpForm();
                errorLabel.Content = "UserId already exists*";
                errorLabel.Visibility = Visibility.Visible;
                SignupButton.Visibility = Visibility.Visible;
                exitButton2.Visibility = Visibility.Visible;
                signingInLabel.Visibility = Visibility.Hidden;
                con.Close();
            }
        }

        //Function to check whether sign up form data is valid or not
        private bool signUpFormDataIsValid()
        {
            if(signUpTBUserId.Text == string.Empty || signUpTBName.Text == string.Empty || signUpPBPassword.Password == string.Empty)
            {
                errorLabel.Content = "Please fill in all the fields*";
                errorLabel.Visibility = Visibility.Visible;
                ResetSignUpForm();
                SignupButton.Visibility = Visibility.Visible;
                exitButton2.Visibility = Visibility.Visible;
                signingInLabel.Visibility = Visibility.Hidden;
                return false;
            }

            return true;
        }

        //Function to clear sign up form
        private void ResetSignUpForm()
        {
            signUpTBUserId.Clear();
            signUpTBName.Clear();
            signUpPBPassword.Clear();

            signUpTBUserId.Focus();
        }

        //Link Button Event: Event to redirect to login grid
        private void logInlinkButton2_Click(object sender, RoutedEventArgs e)
        {
            signUpGrid1.Visibility = Visibility.Hidden;
            signUpGrid2.Visibility = Visibility.Hidden;
            selectionGrid.Visibility = Visibility.Hidden;
            errorLabel.Visibility = Visibility.Hidden;
            errorLabel1.Visibility = Visibility.Hidden;
            errorLabel2.Visibility = Visibility.Hidden;
            logInGrid1.Visibility = Visibility.Visible;

            logInTB1UserId.Focus();
        }

        //Exit application event
        private void exitButton2_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        //Link Button Event: Link to sign up grid
        private void signInlinkButton_Click(object sender, RoutedEventArgs e)
        {
            selectionGrid.Visibility = Visibility.Visible;
            logInGrid1.Visibility = Visibility.Hidden;
            securityKeyPB.Clear();

            securityKeyPB.Focus();
        }

        //Event to check the security key or admin key prior to signup
        private void signUpButton1_Click(object sender, RoutedEventArgs e)
        {
            if (securityKeyPB.Password == string.Empty)
            {
                errorLabel2.Content = "Please provide the security key";
                errorLabel2.Visibility = Visibility.Visible;
                securityKeyPB.Clear();

                securityKeyPB.Focus();
            }
            else
            {
                switch(accountType)
                {
                    case "ADMIN":
                        {
                            if (securityKeyPB.Password == adminKey)
                            {
                                securityKeyOnCorrect();
                                break;
                            }
                            else
                            {
                                securityKeyOnError("Invalid Admin Key*");
                                break;
                            }
                        }
                    case "USER":
                        {
                            if (securityKeyPB.Password == securityKey)
                            {
                                securityKeyOnCorrect();
                                break;
                            }
                            else
                            {
                                securityKeyOnError("Invalid Security Key*");
                                break;
                            }
                        }
                    default:
                        {
                            signUpGrid1.Visibility = Visibility.Hidden;
                            logInGrid.Visibility = Visibility.Visible;
                            break;
                        }
                }
                
            }
        }

        //Security key validation function
        private void securityKeyOnCorrect()
        {
            signUpGrid2.Visibility = Visibility.Visible;
            signUpGrid1.Visibility = Visibility.Hidden;
            ResetSignUpForm();
            errorLabel2.Visibility = Visibility.Hidden;
        }

        //Error handling function on invalid security key
        private void securityKeyOnError(string errorMessage)
        {
            errorLabel2.Content = errorMessage;
            errorLabel2.Visibility = Visibility.Visible;
            securityKeyPB.Clear();

            securityKeyPB.Focus();
        }

        //Event triggered when user clicks on login button
        //Event to log in users
        private void logInButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                logInButton.Visibility = Visibility.Hidden;
                exitButton.Visibility = Visibility.Hidden;
                loggingInLabel.Visibility = Visibility.Visible;
                errorLabel1.Visibility = Visibility.Hidden;
                if(logInFormIsValid())
                {
                    
                    cmd = new SqlCommand("SELECT * FROM users WHERE USERID = @USERID AND PASSWORD = @PASSWORD", con);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@USERID", logInTB1UserId.Text);
                    cmd.Parameters.AddWithValue("@PASSWORD", logInPB1Password.Password);
                    con.Open();
                    sqqr = cmd.ExecuteReader();
                    if(sqqr.Read())
                    {
                        if (sqqr.GetString(4) == "DISABLED")
                        {
                            errorLabel1.Content = "User Disabled*";
                            errorLabel1.Visibility = Visibility.Visible;
                            ResetLogInForm();
                        } else
                        {
                            userId = logInTB1UserId.Text;
                            fullName = sqqr.GetString(1);
                            accountType = sqqr.GetString(3);
                            ResetLogInForm();
                            Dashboard dashboard = new Dashboard();
                            dashboard.Show();
                            logInButton.Visibility = Visibility.Visible;
                            exitButton.Visibility = Visibility.Visible;
                            loggingInLabel.Visibility = Visibility.Hidden;
                            this.Close();
                        }                      
                    } else
                    {
                        errorLabel1.Content = "Wrong UserId or password*";
                        errorLabel1.Visibility = Visibility.Visible;
                        logInButton.Visibility = Visibility.Visible;
                        exitButton.Visibility = Visibility.Visible;
                        loggingInLabel.Visibility = Visibility.Hidden;
                        ResetLogInForm();
                    }
                    con.Close();
                }

            } catch (Exception err)
            {
                ResetLogInForm();
                errorLabel1.Content = "Something went wrong*";
                errorLabel1.Visibility = Visibility.Visible;
                MessageBox.Show(err.Message, "Error");
            }
        }

        //Function to check whether login form data is valid or not
        private bool logInFormIsValid()
        {
            if(logInTB1UserId.Text == string.Empty || logInPB1Password.Password == string.Empty)
            {
                errorLabel1.Content = "Please fill in both the fields*";
                errorLabel1.Visibility = Visibility.Visible;
                logInButton.Visibility = Visibility.Visible;
                exitButton.Visibility = Visibility.Visible;
                loggingInLabel.Visibility = Visibility.Hidden;
                return false;
            }

            return true;
        }

        //Function to clear login form
        private void ResetLogInForm()
        {
            logInTB1UserId.Clear();
            logInPB1Password.Clear();

            logInTB1UserId.Focus();
        }

        //Function to select account type
        private void accountTypeButtonFunction(string accType)
        {
            accountType = accType;
            selectionGrid.Visibility = Visibility.Hidden;
            signUpGrid1.Visibility = Visibility.Visible;
        }

        //Event to set account type to ADMIN
        private void adminButton2_Click(object sender, RoutedEventArgs e)
        {
            accountTypeButtonFunction("ADMIN");
        }

        //Event to set account type to USER
        private void userButton2_Click(object sender, RoutedEventArgs e)
        {
            accountTypeButtonFunction("USER");
        }
    }
}
