/*
*  FILE          : Form1.cs
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
using EventLogger;

namespace SOA_Assign02
{

    /*
    *   NAME    :   Form1
    *   PURPOSE :   Contains the back-end of the form. Here the chart is dynamically created, and the database queries are controlled. 
    */
    public partial class Form1 : Form
    {
        FileManager file;

        public Form1()
        {
            InitializeComponent();
            file = new FileManager();
            this.Load += new EventHandler(Form1_Load);
            cb_SelectedMethod.SelectedIndexChanged += new EventHandler(cb_SelectedMethod_SelectedIndexChanged);
            cb_WebServiceList.SelectedIndexChanged += new EventHandler(cb_WebServiceList_SelectedIndexChanged);
            tb_param1.GotFocus += new EventHandler(tb_param1_Enter);
            tb_param1.LostFocus += new EventHandler(tb_param1_Leave);
            txt_output.ScrollBars = ScrollBars.Both;
            cb_SelectedMethod.DropDownStyle = ComboBoxStyle.DropDownList;
            cb_WebServiceList.DropDownStyle = ComboBoxStyle.DropDownList;
            tb_param1.Text = Constants.DEFAULT_PARAMETERS;
            tb_param1.ForeColor = Color.Gray;
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


        /*
        *   METHOD        : cb_WebServiceList_SelectedIndexChanged
        *   DESCRIPTION   : This event is trigged if the user selects something from the web service combobox.
        *                   We need to populate the method combobox given the web service that is selected.
        *   PARAMETERS    : object sender: object that triggered the event
        *                   EventArgs e  : captured data related to the event
        *   RETURNS       : void : Has no return value 
        */
        private void cb_WebServiceList_SelectedIndexChanged(object sender, EventArgs e)
        {
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
        private void cb_SelectedMethod_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cb_SelectedMethod.SelectedIndex != -1)
            {
                // Determine how many parameters we are expecting
                Tuple<int, string> paramaterInformation = file.DetermineParamAmount(cb_SelectedMethod.Text, file.configList);
                txt_output.Text = "Parameters Required: " + paramaterInformation.Item1 + Environment.NewLine + "Parameter Type: " + paramaterInformation.Item2; //DEBUG FIND WAY TO QUERY XML FILE FOR PARAM TYPES
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
                string regexPattern = validationPatterns.ElementAt(cb_WebServiceList.SelectedIndex);
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
        *   METHOD        : btn_Submit_Click
        *   DESCRIPTION   : Sends the users selected service, and parameters to the service provider
        *   PARAMETERS    : object sender: object that triggered the event
        *                   EventArgs e  : captured data related to the event
        *   RETURNS       : void : Has no return value 
        */
        private void btn_Submit_Click(object sender, EventArgs e)
        {
            //Ensure the user has selected both a web service and method
            if (cb_SelectedMethod.SelectedIndex < 1) { return; }
            //Ensure the users input matches the data type and number of expected arguments
            if (!paramValidation()) { return; }


            /* 
             * Make a call to parse the information from the selected web service method
            *  The returned value will contain an array of strings giving the: URL, ACTION, REQUEST, RESPONSE
            */
            string[] paramArray = extractParameters();
            string[] parseInformation = file.ParseWebService(cb_SelectedMethod.Text, file.configList, paramArray);
            var response = ServiceAdapter.CallWebService(parseInformation[0], parseInformation[1], parseInformation[2]);


            txt_output.Text = "";
            file.RemoveUselessElements(response);
            PrintList(response);
            //foreach (Tuple<string,string> line in response)
            //{
            //    txt_output.Text += line.Item1 + line.Item2 + Environment.NewLine;
            //}
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
            string regexPattern = validationPatterns.ElementAt(cb_WebServiceList.SelectedIndex);
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
            tb_param1.Text = Constants.DEFAULT_PARAMETERS;
            txt_output.Text = "";
            cb_WebServiceList.Items.Clear();
            cb_SelectedMethod.Items.Clear();
            Populate_Web_Service_List();
            print_Config_Into_Text_Box();
        }


        /*
        *   METHOD        : Populate_Web_Service_List
        *   DESCRIPTION   : X
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
        *   METHOD        : tb_param1_Enter
        *   DESCRIPTION   : Sets the tooltip and placeholder text to clear when the txt box has focus
        *   PARAMETERS    : object sender: object that triggered the event
        *                   EventArgs e  : captured data related to the event
        *   RETURNS       : void : Has no return value 
        *   NOTE          : This method was taken from ExceptionLimeCat. (2012). Adding Placeholder Text To TextBox. From: https://stackoverflow.com/questions/11873378/adding-placeholder-text-to-textbox
        */
        private void tb_param1_Enter(object sender, EventArgs e)
        {
            tb_param1ToolTip.ToolTipTitle = "Instructions";
            tb_param1ToolTip.Show("Example parameters: John Smith; Marry Citizen", tb_param1);
            if (tb_param1.Text == Constants.DEFAULT_PARAMETERS)
            {
                tb_param1.Text = "";
            }
            tb_param1.ForeColor = Color.Black;
        }


        /*
        *   METHOD        : tb_param1_Leave
        *   DESCRIPTION   : Sets the placeholder text when the txt box loses focus
        *   PARAMETERS    : object sender: object that triggered the event
        *                   EventArgs e  : captured data related to the event
        *   RETURNS       : void : Has no return value 
        *   NOTE          : This method was taken from ExceptionLimeCat. (2012). Adding Placeholder Text To TextBox. From: https://stackoverflow.com/questions/11873378/adding-placeholder-text-to-textbox
        */
        private void tb_param1_Leave(object sender, EventArgs e)
        {
            if (tb_param1.Text == "")
            {
                tb_param1.Text = Constants.DEFAULT_PARAMETERS;
                tb_param1.ForeColor = Color.Gray;
            }
        }

        private void PrintList(List<Tuple<string, string>> list)
        {
            txt_output.Text = "";
            foreach (var line in list)
            {
                txt_output.Text += line.Item1 + "\t\t" + line.Item2 + Environment.NewLine;
            }
        }
    }
}

