using System;
using System.Windows.Forms;
using teste_dev_crud.Data;
using teste_dev_crud.Popups;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace teste_dev_crud
{
    public partial class Form1 : Form
    {
        private DataContr dbConnect;
        public Form1()
        {
            InitializeComponent();
            dbConnect = new DataContr("Server=127.0.0.1;Port=5445;Database=teste_dev_db;User Id=postgres;Password=test;");

            //Permite que a fileira toda seja selecionada, atribuindo o valor junto do texto quando clica em um ou outro
            listView1.FullRowSelect = true;
            listView1.GridLines = true;
            //Linhas só pra dar um estilo de separar e deixar pouco mais legível

            listView1.Columns[0].Width = Convert.ToInt32(listView1.Width * 0.75);
            listView1.Columns[1].Width = Convert.ToInt32(listView1.Width * 0.25);

            button2.Enabled = false;
            button3.Enabled = false;
        }

        private void DisplayData()
        {
            var dataList = dbConnect.GetAllData();

            listView1.Items.Clear();
            foreach (var items in dataList)
            {
                //Debug.WriteLine(text,numb);
                ListViewItem item = new ListViewItem(items.text);
                item.SubItems.Add(items.number.ToString());
                listView1.Items.Add(item);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            DisplayData();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            add_user add_user_window = new add_user(dbConnect);
            add_user_window.ShowDialog();
            DisplayData();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            update_user update_user_window = new update_user(dbConnect, listView1.SelectedItems[0].SubItems[0].Text, Convert.ToInt32(listView1.SelectedItems[0].SubItems[1].Text));
            update_user_window.ShowDialog();
            DisplayData();
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(listView1.SelectedItems.Count > 0)
            {
                button2.Enabled=true;
                button3.Enabled=true;
            }
            else
            {
                button2.Enabled=false;
                button3.Enabled=false;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try     
            {
                dbConnect.DeleteOnDB(listView1.SelectedItems[0].SubItems[0].Text, Convert.ToInt32(listView1.SelectedItems[0].SubItems[1].Text));
                MessageBox.Show("Data deleted successfully!", "Success", MessageBoxButtons.OK);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"There was an error.\n{ex.Message}", "Error", MessageBoxButtons.OK);
            }

            DisplayData();
        }
    }
}
