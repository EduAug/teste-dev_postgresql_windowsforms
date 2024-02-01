using System;
using System.Windows.Forms;
using teste_dev_crud.Data;

namespace teste_dev_crud.Popups
{
    public partial class update_user : Form
    {
        private DataContr dbConnect;
        public int original_number;
        public update_user(DataContr _dbConnect, string og_text, int og_numb)
        {
            InitializeComponent();
            this.dbConnect = _dbConnect;
            this.original_number = og_numb; //Mantendo uma cópia extra do número, que por ser UNIQUE, será usando no nosso WHERE

            textBox1.Text = og_text;
            numericUpDown1.Value = og_numb;
            numericUpDown1.Controls[0].Visible = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                dbConnect.UpdateOnDB(textBox1.Text, (int)numericUpDown1.Value, original_number);
                MessageBox.Show("Data updated successfully!", "Success", MessageBoxButtons.OK);
            }
            catch (Npgsql.PostgresException ex) when (ex.SqlState == "23505")
            {
                MessageBox.Show($"The number {numericUpDown1.Value} is already in use!\n(By other entry)", "Failed", MessageBoxButtons.OK);
            }
            catch (Npgsql.PostgresException ex) when (ex.SqlState == "23514")
            {
                MessageBox.Show($"The number has to be greater than!\n\t({numericUpDown1.Value})", "Failed", MessageBoxButtons.OK);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"There was an error.\n{ex.Message}", "Error", MessageBoxButtons.OK);
            }
            Close();
        }
    }
}
