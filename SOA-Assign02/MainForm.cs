/*
*  FILE          : MainForm.cs
*  PROGRAMMER    : Randy Lefebvre 2256 and Bence Karner5307
*  DESCRIPTION   : Contains the back end of the code required to run the form. It acts 
*                  as a controller by interpreting the users input, and directing the programs execution.
*/

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
#pragma warning disable CS0105 
using System.Linq;  //Required during control disposal
#pragma warning restore CS0105
using EventLogger;

namespace SOA_Assign02
{
    /*
    *   NAME    :   Form1
    *   PURPOSE :   Contains the back-end of the form. Responsible for interacting 
    *               with the forms controls, and performing input validation.
    */
    public partial class ClientForm : Form
    {
        private FileManager file;
        private bool        a03;


        /*
        *   METHOD        : ClientForm
        *   DESCRIPTION   : Constructor used to init events, controls, and set their properties
        *   PARAMETERS    : void : Has no parameters
        *   RETURNS       : void : Has no return value 
        */
        public ClientForm()
        {
            InitializeComponent();
            file = new FileManager();
            this.Load += new EventHandler(Form1_Load);
            cb_SelectedMethod.SelectedIndexChanged += new EventHandler(Cb_SelectedMethod_SelectedIndexChanged);
            cb_WebServiceList.SelectedIndexChanged += new EventHandler(Cb_WebServiceList_SelectedIndexChanged);
            txt_output.ScrollBars = ScrollBars.Both;
            cb_SelectedMethod.DropDownStyle = ComboBoxStyle.DropDownList;
            cb_WebServiceList.DropDownStyle = ComboBoxStyle.DropDownList;
            tb_param1.Text = Constants.DEFAULT_PARAMETERS;
            tb_param1.ForeColor = Color.Gray;
            a03_ParamPanel.FlowDirection = FlowDirection.TopDown;
            a03_ParamPanel.Hide();
            a03_ServiceCmbBox.Hide();
            a03_ServiceLabel.Hide();
            a03_ServiceCmbBox.SelectedIndexChanged += new EventHandler(A03_ServiceCmbBox_IndexChanged);
            a03 = false;
            hModeBtn.Location = new Point(0, 0);//DEBUG REMOVE BEFORE SUBMISSION AND MOVE THE BUTTON TO THE TOP LEFT
            //DEBUG CHANGE WINDOW SIZE TO FIT 1080P screens
        }


        /*
        *   METHOD        : Form1_Load
        *   DESCRIPTION   : Contains the methods executed during the onLoad phase of the forms creation
        *   PARAMETERS    : object sender: object that triggered the event
        *                   EventArgs e  : captured data related to the event
        *   RETURNS       : void : Has no return value 
        */
        private void Form1_Load(object sender, EventArgs e)
        {
            Populate_Web_Service_List();
            print_Config_Into_Text_Box();
        }


        #region A02


        /*
        *   METHOD        : cb_WebServiceList_SelectedIndexChanged
        *   DESCRIPTION   : This event is trigged if the user selects something from the web service combobox.
        *                   We need to populate the method combobox given the web service that is selected.
        *   PARAMETERS    : object sender: object that triggered the event
        *                   EventArgs e  : captured data related to the event
        *   RETURNS       : void : Has no return value 
        */
        private void Cb_WebServiceList_SelectedIndexChanged(object sender, EventArgs e)
        {
            print_Config_Into_Text_Box();
            cb_SelectedMethod.Items.Clear();
            // Populate the list for the methods
            List<string> methodList = new List<string>();

            foreach (var items in file.configList)
            {
                methodList.Add(items.Item1);
            }
            foreach (var item in methodList)
            {
                if (item.StartsWith(cb_WebServiceList.Text))
                {
                    cb_SelectedMethod.Items.Add(item);
                }
            }
        }


        /*
        *   METHOD        : cb_SelectedMethod_SelectedIndexChanged
        *   DESCRIPTION   : This event is triggered when the user selects something from the method combobox.
        *                   We will find out the information about the parameters that we are requiring, and give them instructions on what they can enter.
        *   PARAMETERS    : object sender: object that triggered the event
        *                   EventArgs e  : captured data related to the event
        *   RETURNS       : void : Has no return value 
        */
        private void Cb_SelectedMethod_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cb_SelectedMethod.SelectedIndex != -1)
            {
                // Determine how many parameters we are expecting
                Tuple<int, string, string> paramaterInformation = file.DetermineParamAmount(cb_SelectedMethod.Text, file.configList);
                txt_output.Text = "Parameters Required: " + paramaterInformation.Item1 + Environment.NewLine + "Parameter Type: " + paramaterInformation.Item2
                    + Environment.NewLine + "Example Format: " + paramaterInformation.Item3;
            }
        }


        /*
        *   METHOD        : paramValidations
        *   DESCRIPTION   : Checks if the user entered the correct number and type of parameters
        *   PARAMETERS    : void : Has no arguments
        *   RETURNS       : void : Has no return value 
        */
        private bool paramValidation()
        {

            //Ensure they selected a service
            if (cb_WebServiceList.SelectedIndex == -1)
            {
                txt_output.Text = "Please select from the list of services...";
                return false;
            }


            //Compare their input string to the regex pattern found in the validation file
            try
            {
                string[] validationPatterns = file.ReadAllLines(Constants.VALIDATION_FILEPATH);
                int indexOfMethod = GetConfigIndex();

                string regexPattern = validationPatterns.ElementAt(GetConfigIndex());
                RegexStringValidator stringValidator = new RegexStringValidator(@regexPattern);         //Light weight variant of regex class
                stringValidator.Validate(tb_param1.Text);                                               //Throws exception if regex fails
            }
            catch (Exception error)
            {
                txt_output.Text += Environment.NewLine + "ERROR: Please enter the correct amount and type of parameters...";
                Logger.RecordError(error.Message);
                return false;
            }

            return true;
        }


        /*
        *   METHOD        : GetConfigIndex
        *   DESCRIPTION   : Gets the index of the selected method in the config list
        *   PARAMETERS    : void : Has no arguments
        *   RETURNS       : void : Has no return value 
        */
        private int GetConfigIndex()
        {
            for (int i = 0; i < file.configList.Count; i++)
            {
                if (file.configList[i].Item1.Contains(cb_SelectedMethod.Text))
                {
                    return i;
                }
            }
            return -1;
        }


        /*
        *   METHOD        : extractParameters
        *   DESCRIPTION   : Parse the users input string for the real values
        *   PARAMETERS    : void : Has no arguments
        *   RETURNS       : string[] : Values of their input
        */
        private string[] extractParameters()
        {
            //Set the regex pattern from the validations file
            string[] validationPatterns = file.ReadAllLines(Constants.VALIDATION_FILEPATH);
            string regexPattern = validationPatterns.ElementAt(GetConfigIndex());
            Regex rx = new Regex(@regexPattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);


            //Count the number of matches according to the regex pattern
            var match = rx.Match(tb_param1.Text);
            List<string> parameterValues = new List<string> { };
            for (int i = 1; i < match.Groups.Count; i++)
            {
                parameterValues.Add(match.Groups[i].ToString());
            }

            return parameterValues.ToArray();
        }


        /*
        *   METHOD        : btn_Clear_Click
        *   DESCRIPTION   : Clears the contents of all controls on the form
        *   PARAMETERS    : object sender: object that triggered the event
        *                   EventArgs e  : captured data related to the event
        *   RETURNS       : void : Has no return value 
        */
        private void btn_Clear_Click(object sender, EventArgs e)
        {
            cb_WebServiceList.SelectedIndex = -1;
            tb_param1.Text = string.Empty;
            txt_output.Text = string.Empty; ;
            cb_WebServiceList.Items.Clear();
            cb_SelectedMethod.Items.Clear();
            Populate_Web_Service_List();
            print_Config_Into_Text_Box();
        }


        /*
        *   METHOD        : Populate_Web_Service_List
        *   DESCRIPTION   : Fills in the combo box with the available services defined in the
        *                   config.txt file
        *   PARAMETERS    : void : Has no parameters
        *   RETURNS       : void : Has no return value 
        */
        private void Populate_Web_Service_List()
        {
            List<string> webServiceNames = new List<string>();

            //Read the config file, and populate the combobox with the web services
            foreach (var items in file.configList)
            {
                string[] tempName = items.Item1.Split(' ');
                webServiceNames.Add(tempName[0]);
            }

            for (int i = 0; (i + 1) < webServiceNames.Count; i++)
            {
                if (string.Equals(webServiceNames[i], webServiceNames[i + 1]))
                {
                    webServiceNames.RemoveAt(i);
                    i--;
                }
            }

            foreach (var item in webServiceNames)
            {
                cb_WebServiceList.Items.Add(item);
            }

            string[] services = new string[] { "TextService", "VinniesLoanService", "ResolveIP" };
            a03_ServiceCmbBox.Items.AddRange(services);
        }


        /*
        *   METHOD        : print_Config_Into_Text_Box
        *   DESCRIPTION   : Prints the contents of the config file to the main display
        *   PARAMETERS    : void : Has no arguments
        *   RETURNS       : void : Has no return value 
        */
        private void print_Config_Into_Text_Box()
        {
            int i = 0;
            txt_output.Text = "";
            foreach (var items in file.configList)
            {
                txt_output.Text += string.Format("*******************************************" +
                                   Environment.NewLine +
                                   "            WEB SERVICE [{0}]" +
                                    Environment.NewLine +
                                   "*******************************************" +
                                   Environment.NewLine, i++);
                txt_output.Text += items.Item1 + Environment.NewLine + Environment.NewLine +
                    items.Item2 + Environment.NewLine + Environment.NewLine +
                    items.Item3 + Environment.NewLine;
            }
        }


        /*
        *   METHOD        : PrintList
        *   DESCRIPTION   : Prints the response from the web service
        *   PARAMETERS    : List<Tuple<string, string>> webServiceResponse : Response from the webs service
        *   RETURNS       : void : Has no return value 
        */
        private void PrintList(List<Tuple<string, string>> webServiceResponse)
        {
            txt_output.Text = string.Empty;
            if (webServiceResponse.Count > 0)
            {
                foreach (var line in webServiceResponse)
                {
                    txt_output.Text += line.Item1 + "\t\t" + line.Item2 + Environment.NewLine;
                }
            }
            else
            {
                txt_output.Text = "Unable to retrieve a response from the web service. Double check you entered the parameters correctly.";
            }
        }


        #endregion
        #region A03


        /*
        *   METHOD        : a03Btn_Click
        *   DESCRIPTION   : Switches operating modes to better work with the published services in A03
        *   PARAMETERS    : object sender : Object that triggered the event
        *                   EventArgs e   : Event related data
        *   RETURNS       : void : Has no return value 
        */
        private void A03Btn_Click(object sender, EventArgs e)
        {
            if (a03)
            {
                foreach (Control control in Controls)
                {
                    switch (control.Name)
                    {
                        case "label1":
                        case "label2":
                        case "label3":
                        case "cb_WebServiceList":
                        case "cb_SelectedMethod":
                        case "tb_param1":
                            control.Show();
                            break;
                    }
                    switch (control.Name)
                    {
                        case "a03_ParamPanel":
                        case "a03_ServiceCmbBox":
                        case "a03_ServiceLabel":
                            control.Hide();
                            break;
                    }
                }
                a03 = false;
            }
            else
            {
                foreach (Control control in Controls)
                {
                    switch (control.Name)
                    {
                        case "label1":
                        case "label2":
                        case "label3":
                        case "cb_WebServiceList":
                        case "cb_SelectedMethod":
                        case "tb_param1":
                            control.Hide();
                            break;
                    }
                    switch (control.Name)
                    {
                        case "a03_ParamPanel":
                        case "a03_ServiceCmbBox":
                        case "a03_ServiceLabel":
                            control.Show();
                            break;
                    }
                }
                a03 = true;
            }
        }


        /*
        *   METHOD        : A03_ServiceCmbBox_IndexChanged
        *   DESCRIPTION   : Dynamically add the required controls to the a03_ParamPanel 
        *                   based on the selected web service
        *   PARAMETERS    : object sender : Object that triggered the event
        *                   EventArgs e   : Event related data
        *   RETURNS       : void : Has no return
        */
        private void A03_ServiceCmbBox_IndexChanged(object sender, EventArgs e)
        {

            //Clear the control panel, and add the new controls
            DisploseControls();
            switch (a03_ServiceCmbBox.SelectedIndex)
            {
                case 0: //TextService
                    Label messageLabel = new Label();
                    Label flagLabel = new Label();
                    messageLabel.Text = "String to Convert";
                    flagLabel.Text = "Convert To (Uppercase: 1 | Lowercase: 2)";
                    messageLabel.AutoSize = true;
                    flagLabel.AutoSize = true;

                    //Input area for service parameters
                    TextBox messageInput = new TextBox();
                    TextBox flagInput = new TextBox();
                    messageInput.Name = "messageTxtBox";
                    flagInput.Name = "flagTxtbox";

                    //Add new controls to the panel
                    a03_ParamPanel.Controls.Add(messageLabel);
                    a03_ParamPanel.Controls.Add(messageInput);
                    a03_ParamPanel.Controls.Add(flagLabel);
                    a03_ParamPanel.Controls.Add(flagInput);
                    break;


                case 1: //VinniesLoanService
                    Label principleLabel = new Label();
                    Label rateLabel = new Label();
                    Label paymentsLabel = new Label();
                    principleLabel.Text = "Principle of the Loan in Dollars($)";
                    rateLabel.Text = "Interest Rate as a Percent (%)";
                    paymentsLabel.Text = "Loan Duration in Years";
                    principleLabel.AutoSize = true;
                    rateLabel.AutoSize = true;
                    paymentsLabel.AutoSize = true;

                    //Input area for service parameters
                    TextBox principleInput = new TextBox();
                    TextBox rateInput = new TextBox();
                    TextBox paymentsInput = new TextBox();
                    principleInput.Name = "principleTxtBox";
                    rateInput.Name = "rateTxtBox";
                    paymentsInput.Name = "paymentsTxtBox";

                    //Add new controls to the panel
                    a03_ParamPanel.Controls.Add(principleLabel);
                    a03_ParamPanel.Controls.Add(principleInput);
                    a03_ParamPanel.Controls.Add(rateLabel);
                    a03_ParamPanel.Controls.Add(rateInput);
                    a03_ParamPanel.Controls.Add(paymentsLabel);
                    a03_ParamPanel.Controls.Add(paymentsInput);
                    break;


                case 2: //ResolveIP
                    break;


                default:
                    break;
            }
        }


        /*
        *   METHOD        : DisploseControls
        *   DESCRIPTION   : Removes the controls from the FlowLayoutPanel, and calls their dispose() method
        *   PARAMETERS    : void : Has no arguments
        *   RETURNS       : void : Has no return value 
        */
        private void DisploseControls()
        {
            /*
             * As per the recommendation on stackoverflow, and MSDN, "Calling the Clear method [of the FlowLayoutPanel] 
             * does not remove control handles from memory. You must explicitly call the Dispose method to avoid memory leaks"
             * 
             * Microsoft. (N.D.). Control.ControlCollection.Clear Method. Retrieved on October 12, 2019, from: 
             *      https://docs.microsoft.com/en-us/dotnet/api/system.windows.forms.control.controlcollection.clear?redirectedfrom=MSDN&view=netframework-4.8#System_Windows_Forms_Control_ControlCollection_Clear
             * Yoinazek. (2012). Remove all controls in a flowlayoutpanel in C#. Retrieved on October 12, 2019, from: 
             *      https://stackoverflow.com/questions/12667304/remove-all-controls-in-a-flowlayoutpanel-in-c-sharp
             */
            List<Control> listControls = a03_ParamPanel.Controls.Cast<Control>().ToList();
            foreach (Control control in listControls)
            {
                a03_ParamPanel.Controls.Remove(control);
                control.Dispose();
            }
            a03_ParamPanel.Controls.Clear();
        }


        /*
        *   METHOD        : btn_Submit_Click
        *   DESCRIPTION   : Sends the users selected service, and parameters to the service provider
        *   PARAMETERS    : object sender: object that triggered the event
        *                   EventArgs e  : captured data related to the event
        *   RETURNS       : void : Has no return value 
        */
        private void btn_Submit_Click(object sender, EventArgs e)
        {
            if (!a03)
            {
                //Ensure the users input matches the data type and number of expected arguments
                if (!paramValidation()) { return; }


                /* 
                * Make a call to parse the information from the selected web service method
                * The returned value will contain an array of strings giving the: URL, ACTION, REQUEST, RESPONSE
                */
                string[] paramArray = extractParameters();
                string[] parseInformation = file.ParseWebService(cb_SelectedMethod.Text, file.configList, paramArray);
                var response = ServiceAdapter.CallWebService(parseInformation[0], parseInformation[1], parseInformation[2]);


                txt_output.Text = string.Empty;
                file.RemoveUselessElements(response);
                PrintList(response);
            }
            else
            {
                txt_output.Text = string.Empty;
                switch (a03_ServiceCmbBox.SelectedIndex)
                {
                    case 0: //TextService
                        bool messageValid = ChecKMessageFormat();
                        //bool flagValid = CheckFlagFormat();
 
                        ////Continue only if all the input fields were correct
                        //if (messageValid && flagValid)
                        //{

                        //    //Package the arguments, and call the web service

                        //}
                        break;


                    case 1: //VinniesLoanService
                        bool principleValid = CheckPrincipleFormat();
                        //bool rateValid = CheckRateFormat();
                        //bool paymentValid = CheckPaymentsFormat();

                        //Continue only if all the input fields were correct
                        //if(principleValid && rateValid && paymentValid)
                        //{

                        //    //Package the arguments, and call the web service
                        //}
                        break;


                    case 2: //ResolveIP
                        break;


                    default:
                        txt_output.Text = "Please choose a web service from the list above";
                        break;
                }
            }
        }


        /*
        *   METHOD        : ChecKMessageFormat
        *   DESCRIPTION   : Checks if the format of the messageTxtBox string is valid
        *   PARAMETERS    : void : Has no parameters
        *   RETURNS       : bool : True if no errors detected
        */
        private bool ChecKMessageFormat()
        {
            bool messageValid = false;
            var messageString = a03_ParamPanel.Controls["messageTxtBox"].Text;

            if (String.IsNullOrWhiteSpace(messageString))
            {
                //DEBUG SET LABEL TO THE RIGHT AS RED AND PRINT THE ERROR
            }
            else
            {
                try
                {
                    //Throw exception if they input any character except those found in English alphabet, and whitespace
                    RegexStringValidator stringValidator = new RegexStringValidator(@"^[a-zA-Z\s]+$");
                    stringValidator.Validate(messageString);

                    messageValid = true;
                }
                catch (Exception argEx)
                {
                    //DEBUG SET THE LABEL TO THE RIGHT OF THE TXT BOX TO INDICATE ERROR
                    Logger.RecordError(argEx.Message);
                }
            }

            return messageValid;
        }


        /*
        *   METHOD        : CheckFlagFormat
        *   DESCRIPTION   : Checks the format of the principle string, and filters the dollar value if required
        *   PARAMETERS    : void : Has no parameters
        *   RETURNS       : bool : True if no errors detected
        */
        private bool CheckFlagFormat()
        {
            bool flagValid = false;
            var flagString = a03_ParamPanel.Controls["flagTxtBox"].Text;
            try
            {
                //Throw exception if they input any character except a single flag value of 1 or 2
                RegexStringValidator stringValidator = new RegexStringValidator(@"^[1-2]$");
                stringValidator.Validate(flagString);

                flagValid = true;
            }
            catch (Exception argEx)
            {
                //DEBUG SET THE LABEL TO THE RIGHT OF THE TXT BOX TO INDICATE ERROR
                Logger.RecordError(argEx.Message);
            }
            return flagValid;
        }


        /*
        *   METHOD        : CheckPrincipleFormat
        *   DESCRIPTION   : Checks the format of the principle string, and filters the dollar value if required
        *   PARAMETERS    : void : Has no parameters
        *   RETURNS       : bool : True if no errors detected
        */
        private bool CheckPrincipleFormat()
        {
            bool principleValid = false;
            var principleString = a03_ParamPanel.Controls["principleTxtBox"].Text;
            try
            {

                if (String.IsNullOrWhiteSpace(principleString))
                {
                    //DEBUG SET LABEL TO THE RIGHT AS RED AND PRINT THE ERROR
                    throw new ArgumentNullException("principleString IsNullOrWhiteSpace");
                }

                var cleanedString = StringSanitizer.RemoveSpecialCharacters(principleString);
                cleanedString = StringSanitizer.RemoveLetters(cleanedString);

                //Find the locale of the 
                if()
                {

                }


                //No errors -> set return as valid
                principleValid = true;
            }
            catch (Exception argEx)
            {
                Logger.RecordError(argEx.Message);
            }

            return principleValid;
        }


        /*
        *   METHOD        : DEBUG
        *   DESCRIPTION   : DEBUG
        *   PARAMETERS    : DEBUG
        *   RETURNS       : bool : True if no errors detected
        */
        private bool CheckRateFormat()
        {
            var rateString = a03_ParamPanel.Controls["rateTxtBox"].Text;
            throw new NotImplementedException();
        }


        /*
        *   METHOD        : DEBUG
        *   DESCRIPTION   : DEBUG
        *   PARAMETERS    : DEBUG
        *   RETURNS       : bool : True if no errors detected
        */
        private bool CheckPaymentsFormat()
        {
            var paymentString = a03_ParamPanel.Controls["paymentsTxtBox"].Text;
            throw new NotImplementedException();
        }


        #endregion
    }//class
}//namespace

