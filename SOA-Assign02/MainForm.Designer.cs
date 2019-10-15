namespace SOA_Assign02
{
    partial class ClientForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.cb_WebServiceList = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tb_param1 = new System.Windows.Forms.TextBox();
            this.txt_output = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btn_Submit = new System.Windows.Forms.Button();
            this.btn_Clear = new System.Windows.Forms.Button();
            this.paramLblToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.tb_param1ToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.cb_SelectedMethod = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.hModeBtn = new System.Windows.Forms.Button();
            this.a03_ServiceCmbBox = new System.Windows.Forms.ComboBox();
            this.a03_ParamPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.a03_ServiceLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // cb_WebServiceList
            // 
            this.cb_WebServiceList.FormattingEnabled = true;
            this.cb_WebServiceList.Location = new System.Drawing.Point(187, 31);
            this.cb_WebServiceList.Margin = new System.Windows.Forms.Padding(2);
            this.cb_WebServiceList.Name = "cb_WebServiceList";
            this.cb_WebServiceList.Size = new System.Drawing.Size(226, 21);
            this.cb_WebServiceList.TabIndex = 0;
            this.cb_WebServiceList.Text = "Select your web service.";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(184, 12);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(193, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Select a web service from the list below";
            // 
            // tb_param1
            // 
            this.tb_param1.Location = new System.Drawing.Point(11, 80);
            this.tb_param1.Margin = new System.Windows.Forms.Padding(2);
            this.tb_param1.Name = "tb_param1";
            this.tb_param1.Size = new System.Drawing.Size(962, 20);
            this.tb_param1.TabIndex = 2;
            // 
            // txt_output
            // 
            this.txt_output.AcceptsReturn = true;
            this.txt_output.AcceptsTab = true;
            this.txt_output.BackColor = System.Drawing.Color.White;
            this.txt_output.Location = new System.Drawing.Point(11, 383);
            this.txt_output.Margin = new System.Windows.Forms.Padding(2);
            this.txt_output.Multiline = true;
            this.txt_output.Name = "txt_output";
            this.txt_output.ReadOnly = true;
            this.txt_output.Size = new System.Drawing.Size(962, 567);
            this.txt_output.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(451, 65);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(60, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Parameters";
            this.paramLblToolTip.SetToolTip(this.label2, "Seperate parameters using a semicolon\r\n1 Parameter:  10\r\n2 parameters: 10;2\r\n");
            // 
            // btn_Submit
            // 
            this.btn_Submit.Location = new System.Drawing.Point(392, 347);
            this.btn_Submit.Margin = new System.Windows.Forms.Padding(2);
            this.btn_Submit.Name = "btn_Submit";
            this.btn_Submit.Size = new System.Drawing.Size(86, 32);
            this.btn_Submit.TabIndex = 7;
            this.btn_Submit.Text = "Submit";
            this.btn_Submit.UseVisualStyleBackColor = true;
            this.btn_Submit.Click += new System.EventHandler(this.btn_Submit_Click);
            // 
            // btn_Clear
            // 
            this.btn_Clear.Location = new System.Drawing.Point(488, 347);
            this.btn_Clear.Margin = new System.Windows.Forms.Padding(2);
            this.btn_Clear.Name = "btn_Clear";
            this.btn_Clear.Size = new System.Drawing.Size(86, 32);
            this.btn_Clear.TabIndex = 8;
            this.btn_Clear.Text = "Clear";
            this.btn_Clear.UseVisualStyleBackColor = true;
            this.btn_Clear.Click += new System.EventHandler(this.btn_Clear_Click);
            // 
            // paramLblToolTip
            // 
            this.paramLblToolTip.ToolTipTitle = "Instructions";
            // 
            // tb_param1ToolTip
            // 
            this.tb_param1ToolTip.ToolTipTitle = "Instructions";
            // 
            // cb_SelectedMethod
            // 
            this.cb_SelectedMethod.FormattingEnabled = true;
            this.cb_SelectedMethod.Location = new System.Drawing.Point(559, 30);
            this.cb_SelectedMethod.Name = "cb_SelectedMethod";
            this.cb_SelectedMethod.Size = new System.Drawing.Size(226, 21);
            this.cb_SelectedMethod.TabIndex = 9;
            this.cb_SelectedMethod.Text = "Select your method";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(556, 11);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(171, 13);
            this.label3.TabIndex = 10;
            this.label3.Text = "Select a method from the list below";
            // 
            // hModeBtn
            // 
            this.hModeBtn.Location = new System.Drawing.Point(974, 953);
            this.hModeBtn.Name = "hModeBtn";
            this.hModeBtn.Size = new System.Drawing.Size(10, 10);
            this.hModeBtn.TabIndex = 11;
            this.hModeBtn.UseVisualStyleBackColor = true;
            this.hModeBtn.Click += new System.EventHandler(this.A03Btn_Click);
            // 
            // a03_ServiceCmbBox
            // 
            this.a03_ServiceCmbBox.FormattingEnabled = true;
            this.a03_ServiceCmbBox.Location = new System.Drawing.Point(392, 54);
            this.a03_ServiceCmbBox.Name = "a03_ServiceCmbBox";
            this.a03_ServiceCmbBox.Size = new System.Drawing.Size(182, 21);
            this.a03_ServiceCmbBox.TabIndex = 12;
            // 
            // a03_ParamPanel
            // 
            this.a03_ParamPanel.Location = new System.Drawing.Point(11, 105);
            this.a03_ParamPanel.Name = "a03_ParamPanel";
            this.a03_ParamPanel.Size = new System.Drawing.Size(961, 237);
            this.a03_ParamPanel.TabIndex = 13;
            // 
            // a03_ServiceLabel
            // 
            this.a03_ServiceLabel.AutoSize = true;
            this.a03_ServiceLabel.Location = new System.Drawing.Point(389, 38);
            this.a03_ServiceLabel.Name = "a03_ServiceLabel";
            this.a03_ServiceLabel.Size = new System.Drawing.Size(174, 13);
            this.a03_ServiceLabel.TabIndex = 14;
            this.a03_ServiceLabel.Text = "Select a Web Service From the List";
            // 
            // ClientForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(984, 961);
            this.Controls.Add(this.a03_ServiceLabel);
            this.Controls.Add(this.a03_ParamPanel);
            this.Controls.Add(this.a03_ServiceCmbBox);
            this.Controls.Add(this.hModeBtn);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cb_SelectedMethod);
            this.Controls.Add(this.btn_Clear);
            this.Controls.Add(this.btn_Submit);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txt_output);
            this.Controls.Add(this.tb_param1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cb_WebServiceList);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "ClientForm";
            this.Text = "SOAP Client";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cb_WebServiceList;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tb_param1;
        private System.Windows.Forms.TextBox txt_output;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btn_Submit;
        private System.Windows.Forms.Button btn_Clear;
        private System.Windows.Forms.ToolTip paramLblToolTip;
        private System.Windows.Forms.ToolTip tb_param1ToolTip;
        private System.Windows.Forms.ComboBox cb_SelectedMethod;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button hModeBtn;
        private System.Windows.Forms.ComboBox a03_ServiceCmbBox;
        private System.Windows.Forms.FlowLayoutPanel a03_ParamPanel;
        private System.Windows.Forms.Label a03_ServiceLabel;
    }
}

