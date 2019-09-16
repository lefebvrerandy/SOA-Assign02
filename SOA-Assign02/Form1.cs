using System;
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
        FileManager file;
        string fileName = "config.txt";

        public Form1()
        {
            InitializeComponent();
            txt_output.ScrollBars = ScrollBars.Both;
            cb_WebServiceList.DropDownStyle = ComboBoxStyle.DropDownList;
            file = new FileManager(fileName);

            // List all the web services in the combobox
            foreach (var items in file.configList)
            {
                cb_WebServiceList.Items.Add(items.Item1);
            }

            /// DEBUGGING PURPOSES
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
            /// END DEBUGGING PURPOSES

            // Make a call to parse the information from the selected web service method
            //  The returned value will contain an array of strings giving the:
            //   URL, ACTION, REQUEST, RESPONSE

        }

        private void cb_WebServiceList_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void btn_Submit_Click(object sender, EventArgs e)
        {
            string[] parseInformation = file.ParseWebService(cb_WebServiceList.Text, file.configList, tb_param1.Text, tb_param2.Text);
            //string expectedResults = file.ParseExpectedResults(cb_WebServiceList.Text, file.configList);
            string actualResponse = ServiceAdapter.CallWebService(parseInformation[0], parseInformation[1], parseInformation[2]);

            txt_output.Text = actualResponse;

        }
    }
}
