namespace ResourceAllocator
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea3 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend3 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series3 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabTask = new System.Windows.Forms.TabPage();
            this.tblTaskPanel = new System.Windows.Forms.TableLayoutPanel();
            this.btnCreateTask = new System.Windows.Forms.Button();
            this.tblTaskPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.txtTaskNumber = new System.Windows.Forms.TextBox();
            this.btnTaskNumber = new System.Windows.Forms.Button();
            this.lblTaskHeader = new System.Windows.Forms.Label();
            this.tabResource = new System.Windows.Forms.TabPage();
            this.tblResourcePanel = new System.Windows.Forms.TableLayoutPanel();
            this.btnResources = new System.Windows.Forms.Button();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.label2 = new System.Windows.Forms.Label();
            this.txtResourceNumber = new System.Windows.Forms.TextBox();
            this.btnResourceNumber = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.tabAllocate = new System.Windows.Forms.TabPage();
            this.btnAssignTaskResource = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.tblTaskResourcePanel = new System.Windows.Forms.TableLayoutPanel();
            this.tabResult = new System.Windows.Forms.TabPage();
            this.lblMessage = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.tblResultPanel = new System.Windows.Forms.TableLayoutPanel();
            this.tabSettings = new System.Windows.Forms.TabPage();
            this.tabPerformanceComparison = new System.Windows.Forms.TabPage();
            this.chartPerformance = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.tabControl1.SuspendLayout();
            this.tabTask.SuspendLayout();
            this.tabResource.SuspendLayout();
            this.tabAllocate.SuspendLayout();
            this.tabResult.SuspendLayout();
            this.tabPerformanceComparison.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chartPerformance)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabTask);
            this.tabControl1.Controls.Add(this.tabResource);
            this.tabControl1.Controls.Add(this.tabAllocate);
            this.tabControl1.Controls.Add(this.tabResult);
            this.tabControl1.Controls.Add(this.tabPerformanceComparison);
            this.tabControl1.Controls.Add(this.tabSettings);
            this.tabControl1.Location = new System.Drawing.Point(79, 29);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(742, 557);
            this.tabControl1.TabIndex = 0;
            // 
            // tabTask
            // 
            this.tabTask.AutoScroll = true;
            this.tabTask.Controls.Add(this.tblTaskPanel);
            this.tabTask.Controls.Add(this.btnCreateTask);
            this.tabTask.Controls.Add(this.tblTaskPanel1);
            this.tabTask.Controls.Add(this.label1);
            this.tabTask.Controls.Add(this.txtTaskNumber);
            this.tabTask.Controls.Add(this.btnTaskNumber);
            this.tabTask.Controls.Add(this.lblTaskHeader);
            this.tabTask.Location = new System.Drawing.Point(4, 22);
            this.tabTask.Name = "tabTask";
            this.tabTask.Padding = new System.Windows.Forms.Padding(3);
            this.tabTask.Size = new System.Drawing.Size(734, 531);
            this.tabTask.TabIndex = 0;
            this.tabTask.Text = "Task Details";
            this.tabTask.UseVisualStyleBackColor = true;
            // 
            // tblTaskPanel
            // 
            this.tblTaskPanel.AutoSize = true;
            this.tblTaskPanel.BackColor = System.Drawing.Color.Transparent;
            this.tblTaskPanel.ColumnCount = 2;
            this.tblTaskPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 47.02381F));
            this.tblTaskPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 52.97619F));
            this.tblTaskPanel.Location = new System.Drawing.Point(74, 99);
            this.tblTaskPanel.Name = "tblTaskPanel";
            this.tblTaskPanel.RowCount = 2;
            this.tblTaskPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tblTaskPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tblTaskPanel.Size = new System.Drawing.Size(336, 55);
            this.tblTaskPanel.TabIndex = 14;
            // 
            // btnCreateTask
            // 
            this.btnCreateTask.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.btnCreateTask.Location = new System.Drawing.Point(445, 99);
            this.btnCreateTask.Name = "btnCreateTask";
            this.btnCreateTask.Size = new System.Drawing.Size(75, 23);
            this.btnCreateTask.TabIndex = 20;
            this.btnCreateTask.Text = "Create Task";
            this.btnCreateTask.UseVisualStyleBackColor = false;
            this.btnCreateTask.Visible = false;
            this.btnCreateTask.Click += new System.EventHandler(this.BtnCreateTask_Click);
            // 
            // tblTaskPanel1
            // 
            this.tblTaskPanel1.AutoScroll = true;
            this.tblTaskPanel1.AutoSize = true;
            this.tblTaskPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tblTaskPanel1.BackColor = System.Drawing.Color.Gray;
            this.tblTaskPanel1.ColumnCount = 2;
            this.tblTaskPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tblTaskPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tblTaskPanel1.Location = new System.Drawing.Point(74, 114);
            this.tblTaskPanel1.Name = "tblTaskPanel1";
            this.tblTaskPanel1.RowCount = 2;
            this.tblTaskPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tblTaskPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tblTaskPanel1.Size = new System.Drawing.Size(0, 0);
            this.tblTaskPanel1.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(71, 62);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(116, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Enter Number Of Task:";
            // 
            // txtTaskNumber
            // 
            this.txtTaskNumber.Location = new System.Drawing.Point(258, 62);
            this.txtTaskNumber.Name = "txtTaskNumber";
            this.txtTaskNumber.Size = new System.Drawing.Size(100, 20);
            this.txtTaskNumber.TabIndex = 0;
            this.txtTaskNumber.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TxtTask_KeyPress);
            // 
            // btnTaskNumber
            // 
            this.btnTaskNumber.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.btnTaskNumber.Location = new System.Drawing.Point(386, 62);
            this.btnTaskNumber.Name = "btnTaskNumber";
            this.btnTaskNumber.Size = new System.Drawing.Size(75, 23);
            this.btnTaskNumber.TabIndex = 1;
            this.btnTaskNumber.Text = "Enter";
            this.btnTaskNumber.UseVisualStyleBackColor = false;
            this.btnTaskNumber.Click += new System.EventHandler(this.BtnTaskNumber_Click);
            // 
            // lblTaskHeader
            // 
            this.lblTaskHeader.AutoSize = true;
            this.lblTaskHeader.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTaskHeader.Location = new System.Drawing.Point(71, 25);
            this.lblTaskHeader.Name = "lblTaskHeader";
            this.lblTaskHeader.Size = new System.Drawing.Size(158, 13);
            this.lblTaskHeader.TabIndex = 0;
            this.lblTaskHeader.Text = "Please Enter Task Details:";
            // 
            // tabResource
            // 
            this.tabResource.AutoScroll = true;
            this.tabResource.Controls.Add(this.tblResourcePanel);
            this.tabResource.Controls.Add(this.btnResources);
            this.tabResource.Controls.Add(this.tableLayoutPanel2);
            this.tabResource.Controls.Add(this.label2);
            this.tabResource.Controls.Add(this.txtResourceNumber);
            this.tabResource.Controls.Add(this.btnResourceNumber);
            this.tabResource.Controls.Add(this.label3);
            this.tabResource.Location = new System.Drawing.Point(4, 22);
            this.tabResource.Name = "tabResource";
            this.tabResource.Padding = new System.Windows.Forms.Padding(3);
            this.tabResource.Size = new System.Drawing.Size(734, 531);
            this.tabResource.TabIndex = 1;
            this.tabResource.Text = "Resource Details";
            this.tabResource.UseVisualStyleBackColor = true;
            // 
            // tblResourcePanel
            // 
            this.tblResourcePanel.AutoSize = true;
            this.tblResourcePanel.ColumnCount = 2;
            this.tblResourcePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 44.04762F));
            this.tblResourcePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 55.95238F));
            this.tblResourcePanel.Location = new System.Drawing.Point(74, 97);
            this.tblResourcePanel.Name = "tblResourcePanel";
            this.tblResourcePanel.RowCount = 2;
            this.tblResourcePanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tblResourcePanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tblResourcePanel.Size = new System.Drawing.Size(336, 55);
            this.tblResourcePanel.TabIndex = 13;
            // 
            // btnResources
            // 
            this.btnResources.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.btnResources.Location = new System.Drawing.Point(538, 115);
            this.btnResources.Name = "btnResources";
            this.btnResources.Size = new System.Drawing.Size(75, 23);
            this.btnResources.TabIndex = 12;
            this.btnResources.Text = "Create Resources";
            this.btnResources.UseVisualStyleBackColor = false;
            this.btnResources.Visible = false;
            this.btnResources.Click += new System.EventHandler(this.BtnResources_Click);
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.AutoScroll = true;
            this.tableLayoutPanel2.AutoSize = true;
            this.tableLayoutPanel2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel2.BackColor = System.Drawing.Color.Gray;
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Location = new System.Drawing.Point(44, 143);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(0, 0);
            this.tableLayoutPanel2.TabIndex = 11;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(71, 62);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(143, 13);
            this.label2.TabIndex = 10;
            this.label2.Text = "Enter Number Of Resources:";
            // 
            // txtResourceNumber
            // 
            this.txtResourceNumber.Location = new System.Drawing.Point(239, 59);
            this.txtResourceNumber.Name = "txtResourceNumber";
            this.txtResourceNumber.Size = new System.Drawing.Size(100, 20);
            this.txtResourceNumber.TabIndex = 0;
            this.txtResourceNumber.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TxtTask_KeyPress);
            // 
            // btnResourceNumber
            // 
            this.btnResourceNumber.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.btnResourceNumber.Location = new System.Drawing.Point(406, 59);
            this.btnResourceNumber.Name = "btnResourceNumber";
            this.btnResourceNumber.Size = new System.Drawing.Size(75, 23);
            this.btnResourceNumber.TabIndex = 1;
            this.btnResourceNumber.Text = "Enter";
            this.btnResourceNumber.UseVisualStyleBackColor = false;
            this.btnResourceNumber.Click += new System.EventHandler(this.BtnResourceNumber_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(71, 25);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(184, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Please Enter Resource Details:";
            // 
            // tabAllocate
            // 
            this.tabAllocate.AutoScroll = true;
            this.tabAllocate.Controls.Add(this.btnAssignTaskResource);
            this.tabAllocate.Controls.Add(this.label4);
            this.tabAllocate.Controls.Add(this.tblTaskResourcePanel);
            this.tabAllocate.Location = new System.Drawing.Point(4, 22);
            this.tabAllocate.Name = "tabAllocate";
            this.tabAllocate.Size = new System.Drawing.Size(734, 531);
            this.tabAllocate.TabIndex = 3;
            this.tabAllocate.Text = "Allocate Task And Resources";
            this.tabAllocate.UseVisualStyleBackColor = true;
            // 
            // btnAssignTaskResource
            // 
            this.btnAssignTaskResource.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.btnAssignTaskResource.Location = new System.Drawing.Point(502, 53);
            this.btnAssignTaskResource.Name = "btnAssignTaskResource";
            this.btnAssignTaskResource.Size = new System.Drawing.Size(75, 23);
            this.btnAssignTaskResource.TabIndex = 16;
            this.btnAssignTaskResource.Text = "Create Resources";
            this.btnAssignTaskResource.UseVisualStyleBackColor = false;
            this.btnAssignTaskResource.Visible = false;
            this.btnAssignTaskResource.Click += new System.EventHandler(this.BtnAssignTaskResource_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(71, 25);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(184, 13);
            this.label4.TabIndex = 15;
            this.label4.Text = "Please Enter Resource Details:";
            // 
            // tblTaskResourcePanel
            // 
            this.tblTaskResourcePanel.AutoSize = true;
            this.tblTaskResourcePanel.ColumnCount = 2;
            this.tblTaskResourcePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.59524F));
            this.tblTaskResourcePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 49.40476F));
            this.tblTaskResourcePanel.Location = new System.Drawing.Point(74, 53);
            this.tblTaskResourcePanel.Name = "tblTaskResourcePanel";
            this.tblTaskResourcePanel.RowCount = 2;
            this.tblTaskResourcePanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tblTaskResourcePanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tblTaskResourcePanel.Size = new System.Drawing.Size(336, 55);
            this.tblTaskResourcePanel.TabIndex = 14;
            // 
            // tabResult
            // 
            this.tabResult.AutoScroll = true;
            this.tabResult.Controls.Add(this.lblMessage);
            this.tabResult.Controls.Add(this.label5);
            this.tabResult.Controls.Add(this.tblResultPanel);
            this.tabResult.Location = new System.Drawing.Point(4, 22);
            this.tabResult.Name = "tabResult";
            this.tabResult.Size = new System.Drawing.Size(734, 531);
            this.tabResult.TabIndex = 2;
            this.tabResult.Text = "Result";
            this.tabResult.UseVisualStyleBackColor = true;
            // 
            // lblMessage
            // 
            this.lblMessage.AutoSize = true;
            this.lblMessage.Location = new System.Drawing.Point(501, 57);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(50, 13);
            this.lblMessage.TabIndex = 18;
            this.lblMessage.Text = "Message";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(71, 25);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(143, 13);
            this.label5.TabIndex = 17;
            this.label5.Text = "Final Optimized Results:";
            // 
            // tblResultPanel
            // 
            this.tblResultPanel.AutoSize = true;
            this.tblResultPanel.ColumnCount = 2;
            this.tblResultPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.59524F));
            this.tblResultPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 49.40476F));
            this.tblResultPanel.Location = new System.Drawing.Point(74, 57);
            this.tblResultPanel.Name = "tblResultPanel";
            this.tblResultPanel.RowCount = 2;
            this.tblResultPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tblResultPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tblResultPanel.Size = new System.Drawing.Size(336, 55);
            this.tblResultPanel.TabIndex = 16;
            // 
            // tabSettings
            // 
            this.tabSettings.Location = new System.Drawing.Point(4, 22);
            this.tabSettings.Name = "tabSettings";
            this.tabSettings.Padding = new System.Windows.Forms.Padding(3);
            this.tabSettings.Size = new System.Drawing.Size(734, 531);
            this.tabSettings.TabIndex = 4;
            this.tabSettings.Text = "Settings";
            this.tabSettings.UseVisualStyleBackColor = true;
            // 
            // tabPerformanceComparison
            // 
            this.tabPerformanceComparison.Controls.Add(this.chartPerformance);
            this.tabPerformanceComparison.Location = new System.Drawing.Point(4, 22);
            this.tabPerformanceComparison.Name = "tabPerformanceComparison";
            this.tabPerformanceComparison.Padding = new System.Windows.Forms.Padding(3);
            this.tabPerformanceComparison.Size = new System.Drawing.Size(734, 531);
            this.tabPerformanceComparison.TabIndex = 5;
            this.tabPerformanceComparison.Text = "Performance";
            this.tabPerformanceComparison.UseVisualStyleBackColor = true;
            // 
            // chartPerformance
            // 
            chartArea3.Name = "ChartArea1";
            this.chartPerformance.ChartAreas.Add(chartArea3);
            legend3.Name = "Legend1";
            this.chartPerformance.Legends.Add(legend3);
            this.chartPerformance.Location = new System.Drawing.Point(88, 91);
            this.chartPerformance.Name = "chartPerformance";
            series3.ChartArea = "ChartArea1";
            series3.Legend = "Legend1";
            series3.Name = "Performance";
            this.chartPerformance.Series.Add(series3);
            this.chartPerformance.Size = new System.Drawing.Size(395, 300);
            this.chartPerformance.TabIndex = 0;
            this.chartPerformance.Text = "chart1";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1071, 713);
            this.Controls.Add(this.tabControl1);
            this.Name = "Form1";
            this.Text = "Task Resource Allocator";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabTask.ResumeLayout(false);
            this.tabTask.PerformLayout();
            this.tabResource.ResumeLayout(false);
            this.tabResource.PerformLayout();
            this.tabAllocate.ResumeLayout(false);
            this.tabAllocate.PerformLayout();
            this.tabResult.ResumeLayout(false);
            this.tabResult.PerformLayout();
            this.tabPerformanceComparison.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chartPerformance)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabTask;
        private System.Windows.Forms.TabPage tabResource;
        private System.Windows.Forms.Label lblTaskHeader;
        private System.Windows.Forms.TabPage tabResult;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtTaskNumber;
        private System.Windows.Forms.Button btnTaskNumber;
        private System.Windows.Forms.TableLayoutPanel tblTaskPanel1;
        private System.Windows.Forms.Button btnCreateTask;
        private System.Windows.Forms.TableLayoutPanel tblResourcePanel;
        private System.Windows.Forms.Button btnResources;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtResourceNumber;
        private System.Windows.Forms.Button btnResourceNumber;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TabPage tabAllocate;
        private System.Windows.Forms.TableLayoutPanel tblTaskResourcePanel;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TableLayoutPanel tblResultPanel;
        private System.Windows.Forms.Button btnAssignTaskResource;
        private System.Windows.Forms.TableLayoutPanel tblTaskPanel;
        private System.Windows.Forms.Label lblMessage;
        private System.Windows.Forms.TabPage tabSettings;
        private System.Windows.Forms.TabPage tabPerformanceComparison;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartPerformance;
    }
}

