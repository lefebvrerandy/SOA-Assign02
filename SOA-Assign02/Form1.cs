﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SOA_Assign02
{
    public partial class Form1 : Form
    {
        string fileName = "config.txt";
        FileManager file;                   


        public Form1()
        {
            InitializeComponent();
            txt_output.ScrollBars = ScrollBars.Both;
            cb_WebServiceList.DropDownStyle = ComboBoxStyle.DropDownList;
            this.Load += new EventHandler(Form1_Load);
        }


        /*
        *   METHOD        : Form1_Load
        *   DESCRIPTION   : Contains the methods executed during the onLoad phase of the forms creation
        *   PARAMETERS    : TODO
        *   RETURNS       : void : Has no return value 
        */
        private void Form1_Load(object sender, EventArgs e)
        {
            
            //Read in the config file, and populate the combobox with the web services
            file = new FileManager(fileName);
            foreach (var items in file.configList)
            {
                cb_WebServiceList.Items.Add(items.Item1);
            }


            #region DEBUGGING
            int i = 0;
            // Display all the information that is stored in the file.configList
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
            # endregion


            tb_param1.Enabled = false;
            tb_param2.Enabled = false;
        }


        /*
        *   METHOD        : cb_WebServiceList_SelectedIndexChanged
        *   DESCRIPTION   : Adjusts the parameter controls based on the users selected service
        *   PARAMETERS    : TODO
        *   RETURNS       : void : Has no return value 
        */
        private void cb_WebServiceList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cb_WebServiceList.SelectedIndex != -1)
            {
                // Determine how many parameters we are expecting
                switch (file.DetermineParamAmount(cb_WebServiceList.Text, file.configList))
                {
                    case 1:
                        tb_param1.Enabled = true;
                        tb_param2.Enabled = false;
                        break;
                    case 2:
                        tb_param1.Enabled = true;
                        tb_param2.Enabled = true;
                        break;
                }
            }
        }

        /*
        *   METHOD        : btn_Submit_Click
        *   DESCRIPTION   : Sends the users selected service, and parameters to the service provider
        *   PARAMETERS    : TODO
        *   RETURNS       : void : Has no return value 
        */
        private void btn_Submit_Click(object sender, EventArgs e)
        {
            // Make a call to parse the information from the selected web service method
            //  The returned value will contain an array of strings giving the:
            //   URL, ACTION, REQUEST, RESPONSE
            string[] paramArray = { tb_param1.Text, tb_param2.Text };
            string[] parseInformation = file.ParseWebService(cb_WebServiceList.Text, file.configList, paramArray);
            List<string> response = ServiceAdapter.CallWebService(parseInformation[0], parseInformation[1], parseInformation[2]);

            txt_output.Text = "";
            foreach (string line in response)
            {
                txt_output.Text += line + Environment.NewLine;
            }
        }


        /*
        *   METHOD        : btn_Clear_Click
        *   DESCRIPTION   : Clears the text from the forms controls
        *   PARAMETERS    : TODO
        *   RETURNS       : void : Has no return value 
        */
        private void btn_Clear_Click(object sender, EventArgs e)
        {
            // Clear the contents of combobox selection, param 1, param 2, textbox
            cb_WebServiceList.SelectedIndex = -1;
            tb_param1.Text = "";
            tb_param1.Enabled = false;
            tb_param2.Text = "";
            tb_param2.Enabled = false;
            txt_output.Text = "";
            cb_WebServiceList.Items.Clear();
            foreach (var items in file.configList)
            {
                cb_WebServiceList.Items.Add(items.Item1);
            }
            int i = 0;
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
    }
}
