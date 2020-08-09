using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ResourceAllocator
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        public void ClearPanelControl(TableLayoutPanel tableLayout)
        {

            //tableLayout.Controls.Cast<Control>().ToList();
           foreach (var cntrol in tableLayout.Controls.Cast<Control>().ToList())
            {
               if( cntrol.GetType() == typeof(TextBox))
                {
                    TextBox txtCntrol = (TextBox)cntrol;
                    tableLayout.Controls.Remove(txtCntrol);
                }
                else if (cntrol.GetType() == typeof(Label))
                {
                    Label lblCntrol = (Label)cntrol;
                    tableLayout.Controls.Remove(lblCntrol);
                }
                //if (cntrol.GetType() == typeof(TextBox) || cntrol.GetType() == typeof(Label) || cntrol.GetType() == typeof(Button))
                //{
                //    tableLayout.Controls.Remove(cntrol);
                //    //TextBox txtCntrol = (TextBox)cntrol;
                //    //lstTask.Add(txtCntrol.Text.Trim());
                //}
            }
            tblTaskPanel.RowCount = 2;
        }
        private void BtnTaskNumber_Click(object sender, EventArgs e)
        {
            ClearPanelControl(tblTaskPanel);
            btnCreateTask.Visible = true;
            int TaskCount = int.Parse(txtTaskNumber.Text.Trim());
            tblTaskPanel.Visible = false;
            for (int i = 0; i < TaskCount; i++)
            {
                tblTaskPanel .RowStyles.Add(new RowStyle(SizeType.AutoSize, 40F));
                RowStyle temp = tblTaskPanel.RowStyles[tblTaskPanel.RowCount - 1];
                Label lblTask = new Label();
                lblTask.Dock = DockStyle.Bottom;
                lblTask.Text = "Name For Task " + (i + 1);
                lblTask.Width = 100;
                tblTaskPanel.Controls.Add(lblTask, 0, i + 2);

                TextBox txtTask = new TextBox();
                txtTask.Text = "";
                txtTask.MinimumSize = new Size(200, 20);
                txtTask.TabIndex = btnTaskNumber.TabIndex + i + 1;
                tblTaskPanel.Controls.Add(txtTask, 1, i + 2);


                tblTaskPanel.RowCount = tblTaskPanel.RowCount + 1;
              //  tblTaskPanel.RowStyles.Add(new RowStyle(temp.SizeType, temp.Height));

            }
            tblTaskPanel.RowCount = tblTaskPanel.RowCount + 1;
            tblTaskPanel.Controls.Add(btnCreateTask, 1, tblTaskPanel.RowCount);
            //tblTaskPanel.Controls.Add(panel);
            tblTaskPanel.Visible = true;
             
        }

        private void TxtTask_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Verify that the pressed key isn't CTRL or any non-numeric digit
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
                MessageBox.Show("Please enter only numbers.");
            }

            // If you want, you can allow decimal (float) numbers
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
            
        }

        private void BtnCreateTask_Click(object sender, EventArgs e)
        {
            lstTask = new List<string>();
            foreach (var cntrol in tblTaskPanel.Controls)
            {
                if (cntrol.GetType() == typeof(TextBox))
                {
                    TextBox txtCntrol = (TextBox)cntrol;
                    lstTask.Add(txtCntrol.Text.Trim()); 
                }
            }
            tabControl1.SelectedTab = tabResource;

        }
        public List<string> lstTask = new List<string>();
        public List<string> lstResources = new List<string>();
        private void BtnResources_Click(object sender, EventArgs e)
        {
            List<Control> lstControl = tblTaskResourcePanel.Controls.Cast<Control>().Where(obj => obj.GetType() == typeof(TextBox) || obj.GetType() == typeof(Label)).ToList();
            foreach (var cntrol in lstControl)
            {
                if (cntrol.GetType() == typeof(TextBox) || cntrol.GetType() == typeof(Label))
                {
                    tblTaskResourcePanel.Controls.Remove((Control)cntrol);
                }
            }
            //int 
            //for (int i= 0;i < ;i++)
            //{
            //    if (tblTaskResourcePanel.Controls[i].GetType() == typeof(TextBox) || tblTaskResourcePanel.Controls[i].GetType() == typeof(Label))
            //    {
            //        tblTaskResourcePanel.Controls.Remove(tblTaskResourcePanel.Controls[i]);                 
            //    }
            //}
            tblTaskResourcePanel.RowCount = 2;
            //  List<string> lstContorls = new List<string>();
            lstResources = new List<string>();
            foreach (var cntrol in tblResourcePanel.Controls)
            {
                if (cntrol.GetType() == typeof(TextBox))
                {
                    TextBox txtCntrol = (TextBox)cntrol;
                    lstResources.Add(txtCntrol.Text.Trim());
                }
            }

            //Arrange Task and Resource Allocation window controls
            btnAssignTaskResource.Visible = true;
            tblTaskResourcePanel.ColumnCount = 0;
            tblTaskResourcePanel.RowStyles.Add(new RowStyle(SizeType.AutoSize, 40F));

            //RowStyle temp = tblTaskResourcePanel.RowStyles[tblTaskResourcePanel.RowCount - 1];
           // tblTaskResourcePanel.RowStyles.Add(new RowStyle(temp.SizeType, temp.Height));
            for (int i = 0; i < lstResources.Count; i++)
            {
                

                Label lblResource = new Label();
                lblResource.Dock = DockStyle.Bottom;
                lblResource.Text = lstResources[i];
                tblTaskResourcePanel.Controls.Add(lblResource,i+1 ,0);
                tblTaskResourcePanel.ColumnCount = tblTaskResourcePanel.ColumnCount + 1;
                //tblTaskResourcePanel.Controls.Add(txtResource, 1, i + 1);


                //tblTaskResourcePanel.RowCount = tblTaskResourcePanel.RowCount + 1;
              
            }
            int tabCounter = 0;
            for (int i = 0; i < lstTask.Count; i++)
            {

                //RowStyle tempStyle = tblTaskResourcePanel.RowStyles[tblTaskResourcePanel.RowCount - 2];
                Label lblResource = new Label();
                lblResource.Dock = DockStyle.Bottom;
                lblResource.Text = lstTask[i];
                tblTaskResourcePanel.Controls.Add(lblResource,0,i+1);
                tblTaskResourcePanel.ColumnCount = tblTaskResourcePanel.ColumnCount + 1;                 
                for (int j = 0; j < lstResources.Count; j++)
                {
                    TextBox txtResource = new TextBox();
                    txtResource.Text = "";
                    txtResource.Name = i + "_" + j;
                    txtResource.MinimumSize = new Size(50, 20);
                    txtResource.KeyPress += TxtTask_KeyPress;
                    txtResource.TabIndex = tabCounter++;// i +j;
                    tblTaskResourcePanel.Controls.Add(txtResource,j+1, i + 1);
                     
                  //  tblTaskResourcePanel.RowStyles.Add(new RowStyle(tempStyle.SizeType, tempStyle.Height));
                }
                tblTaskResourcePanel.RowCount = tblTaskResourcePanel.RowCount + 1;
               // tblTaskResourcePanel.RowStyles.Add(new RowStyle(tempStyle.SizeType, tempStyle.Height));
            }
            //    for (int i = 0; i < lstTask.Count; i++)
            //{
            //    RowStyle temp = tblResourcePanel.RowStyles[tblTaskPanel1.RowCount - 2];
            //    Label lblResource = new Label();
            //    lblResource.Dock = DockStyle.Bottom;
            //    lblResource.Text = "Name For Resources " + (i + 1);
            //    tblResourcePanel.Controls.Add(lblResource, 0, i + 1);

            //    TextBox txtResource = new TextBox();
            //    txtResource.Text = "";
            //    txtResource.MinimumSize = new Size(200, 20);
            //    tblResourcePanel.Controls.Add(txtResource, 1, i + 1);


            //    tblResourcePanel.RowCount = tblResourcePanel.RowCount + 1;
            //    tblResourcePanel.RowStyles.Add(new RowStyle(temp.SizeType, temp.Height));
            //}
            //tblTaskResourcePanel.RowCount = tblTaskResourcePanel.RowCount + 1;
            btnAssignTaskResource.TabIndex = tabCounter++;
            tblTaskResourcePanel.Controls.Add(btnAssignTaskResource, 1, tblTaskResourcePanel.RowCount);

            tabControl1.SelectedTab = tabAllocate;



        }

        private void BtnResourceNumber_Click(object sender, EventArgs e)
        {
            ClearPanelControl(tblResourcePanel);
            btnResources.Visible = true;
            tblResourcePanel.RowCount = 2;
            int TaskCount = int.Parse(txtResourceNumber.Text.Trim());
            tblResourcePanel.RowStyles.Add(new RowStyle(SizeType.AutoSize, 40F));
            tblResourcePanel.Visible = false;
            for (int i = 0; i < TaskCount; i++)
            {
                // RowStyle temp = tblResourcePanel.RowStyles[tblResourcePanel.RowCount - 2];
   
                Label lblResource = new Label();
                lblResource.Dock = DockStyle.Bottom;
                lblResource.Text = "Name For Resources " + (i + 1);
                tblResourcePanel.Controls.Add(lblResource, 0, i + 2);

                TextBox txtResource = new TextBox();
                txtResource.Text = "";
                txtResource.MinimumSize = new Size(200, 20);
                txtResource.TabIndex = btnResourceNumber.TabIndex + i + 1;
                tblResourcePanel.Controls.Add(txtResource, 1, i + 2);

                tblResourcePanel.RowCount = tblResourcePanel.RowCount + 1;
                //tblResourcePanel.RowStyles.Add(new RowStyle(temp.SizeType, temp.Height));
            }
            tblResourcePanel.RowCount = tblResourcePanel.RowCount + 1;
            btnResources.TabIndex = btnResourceNumber.TabIndex + TaskCount + 1;
            tblResourcePanel.Controls.Add(btnResources, 1, tblResourcePanel.RowCount);
            tblResourcePanel.Visible = true;
        }

        private void BtnAssignTaskResource_Click(object sender, EventArgs e)
        {
            List<Control> lstControl = tblResultPanel.Controls.Cast<Control>().Where(obj => obj.GetType() == typeof(TextBox) || obj.GetType() == typeof(Label)).ToList();
            foreach (var cntrol in lstControl)
            {
                if (cntrol.GetType() == typeof(TextBox) || cntrol.GetType() == typeof(Label))
                {
                    tblResultPanel.Controls.Remove((Control)cntrol);
                }
            }

            int MaxCount = (lstResources.Count > lstTask.Count) ? lstResources.Count : lstTask.Count;
            arrMatrixTaskResource = new int[MaxCount,MaxCount] ;
            foreach (var cntrol in tblTaskResourcePanel.Controls)
            {
                if (cntrol.GetType() == typeof(TextBox))
                {
                    TextBox txtCntrol = (TextBox)cntrol;
                    string strID = txtCntrol.Name;
                    string[] arrSTRID = strID.Split(new string[] { "_" }, StringSplitOptions.None);
                    arrMatrixTaskResource[int.Parse(arrSTRID[0]), int.Parse(arrSTRID[1])] =int.Parse( txtCntrol.Text.Trim());

                }
            }
            ///Start Arrange Result Window Controls
            tblResultPanel.Visible = false;


            tblResultPanel.ColumnCount = 0;
            tblResultPanel.RowStyles.Add(new RowStyle(SizeType.AutoSize, 40F));

            for (int i = 0; i < lstResources.Count; i++)
            {
                Label lblResource = new Label();
                lblResource.Dock = DockStyle.Bottom;
                lblResource.Text = lstResources[i];
                tblResultPanel.Controls.Add(lblResource, i + 1, 0);
                tblResultPanel.ColumnCount = tblResultPanel.ColumnCount + 1;
            }
            for (int i = 0; i < lstTask.Count; i++)
            {
                Label lblResource = new Label();
                lblResource.Dock = DockStyle.Bottom;
                lblResource.Text = lstTask[i];
                tblResultPanel.Controls.Add(lblResource, 0, i + 1);
                tblResultPanel.ColumnCount = tblResultPanel.ColumnCount + 1;
                for (int j = 0; j < lstResources.Count; j++)
                {
                    TextBox txtResource = new TextBox();
                    txtResource.Text = arrMatrixTaskResource[i, j].ToString();
                    txtResource.Name = i + "_" + j;
                    txtResource.MinimumSize = new Size(50, 20);
                   // txtResource.ReadOnly = true;
                    tblResultPanel.Controls.Add(txtResource, j + 1, i + 1);
                }
                tblResultPanel.RowCount = tblResultPanel.RowCount + 1;
            }
            tblResultPanel.Controls.Add(lblMessage, 0, tblResultPanel.RowCount);
            lblMessage.Text = "Starting";
            tabControl1.SelectedTab = tabResult;
            //SetText( lblMessage,"Starting");
            this.Refresh();



            DateTime dateParallelStart = DateTime.Now;
            var algorithm = new HungarianParallelSEMAD(arrMatrixTaskResource);//, UpdateLabel);            
              var result = algorithm.Run();
            DateTime dateParallelEnd = DateTime.Now;
            TimeSpan spanParalel = dateParallelEnd - dateParallelStart;

            for (int i = 0; i < result.Count(); i++)
            {
                if (tblResultPanel.Controls.Cast<Control>().ToList().Where(obj => obj.Name == i + "_" + result[i]).Count()>0)
                {
                    var optimalResult = tblResultPanel.Controls.Cast<Control>().ToList().Where(obj => obj.Name == i + "_" + result[i]).First();
                    optimalResult.ForeColor = Color.Red;
                    optimalResult.Font = new Font(optimalResult.Font, FontStyle.Bold);
                   
                }           
 
            }
            lblMessage.Text = "Completed";
            tblResultPanel.Visible = true;
            this.Refresh();
            DateTime dateSerialStart = DateTime.Now;
            var algorithmSerial = new clsHungarianAlgorithm(arrMatrixTaskResource);//,UpdateLabel);
            var resultSerial = algorithmSerial.Run();
            DateTime dateSerialEnd = DateTime.Now;
            TimeSpan spanSerial = dateSerialEnd - dateSerialStart;

            chartPerformance.Series["Performance"].Points.Clear();
            chartPerformance.Series["Performance"].Points.AddXY("Serial", spanSerial.TotalMilliseconds);
            chartPerformance.Series["Performance"].Points.AddXY("Parallel", spanParalel.TotalMilliseconds);
            chartPerformance.Series["Performance"].IsValueShownAsLabel = true;
            chartPerformance.ChartAreas[0].AxisY.Title = "Time Taken In Milli Seconds";
            //Thread.Sleep(5 * 1000);
            // SetText(lblMessage, "Completed");
            ///End Arrange Result Window Controls


        }
        private void UpdateLabel(string newText)
        {
            this.Invoke(new MethodInvoker(() => lblMessage.Text = newText));
            this.Refresh();
        }

        public void SetText(object sender, string text)
        {
            //Type typeControl = sender.GetType();

            //if (typeControl == typeof(TextBox))
            //{
            //    TextBox control = (TextBox)sender;
            //    control.Invoke(new Action(() => control.Text = text));
            //}
            //else if (typeControl == typeof(Label))
            //{
            //    Label control = (Label)sender;
            //    control.Invoke(new Action(() => control.Text = text));
            //}
            

            //Invoke(new MethodInvoker(control=>control.Text = text),);
        }
        public int[,] arrMatrixTaskResource;

        private void Form1_Load(object sender, EventArgs e)
        {
           
        }
    }
}