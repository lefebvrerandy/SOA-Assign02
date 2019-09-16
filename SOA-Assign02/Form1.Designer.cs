namespace SOA_Assign02
{
    partial class Form1
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
            this.cb_WebServiceList = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tb_param1 = new System.Windows.Forms.TextBox();
            this.tb_param2 = new System.Windows.Forms.TextBox();
            this.txt_output = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // cb_WebServiceList
            // 
            this.cb_WebServiceList.FormattingEnabled = true;
            this.cb_WebServiceList.Location = new System.Drawing.Point(512, 81);
            this.cb_WebServiceList.Name = "cb_WebServiceList";
            this.cb_WebServiceList.Size = new System.Drawing.Size(449, 33);
            this.cb_WebServiceList.TabIndex = 0;
            this.cb_WebServiceList.Text = "Select your web service.";
            this.cb_WebServiceList.SelectedIndexChanged += new System.EventHandler(this.cb_WebServiceList_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(512, 50);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(389, 25);
            this.label1.TabIndex = 1;
            this.label1.Text = "Select a web service from the list below";
            // 
            // tb_param1
            // 
            this.tb_param1.Location = new System.Drawing.Point(184, 198);
            this.tb_param1.Name = "tb_param1";
            this.tb_param1.Size = new System.Drawing.Size(360, 31);
            this.tb_param1.TabIndex = 2;
            // 
            // tb_param2
            // 
            this.tb_param2.Location = new System.Drawing.Point(915, 198);
            this.tb_param2.Name = "tb_param2";
            this.tb_param2.Size = new System.Drawing.Size(360, 31);
            this.tb_param2.TabIndex = 3;
            // 
            // txt_output
            // 
            this.txt_output.AcceptsReturn = true;
            this.txt_output.AcceptsTab = true;
            this.txt_output.Location = new System.Drawing.Point(33, 338);
            this.txt_output.Multiline = true;
            this.txt_output.Name = "txt_output";
            this.txt_output.ReadOnly = true;
            this.txt_output.Size = new System.Drawing.Size(1416, 630);
            this.txt_output.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(215, 167);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(129, 25);
            this.label2.TabIndex = 5;
            this.label2.Text = "Parameter 1";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(951, 170);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(129, 25);
            this.label3.TabIndex = 6;
            this.label3.Text = "Parameter 2";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(562, 249);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(172, 62);
            this.button1.TabIndex = 7;
            this.button1.Text = "Submit";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(754, 249);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(172, 62);
            this.button2.TabIndex = 8;
            this.button2.Text = "Clear";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1493, 1006);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txt_output);
            this.Controls.Add(this.tb_param2);
            this.Controls.Add(this.tb_param1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cb_WebServiceList);
            this.Name = "Form1";
            this.Text = "SOAP - Assignment 02";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cb_WebServiceList;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tb_param1;
        private System.Windows.Forms.TextBox tb_param2;
        private System.Windows.Forms.TextBox txt_output;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
    }
}

