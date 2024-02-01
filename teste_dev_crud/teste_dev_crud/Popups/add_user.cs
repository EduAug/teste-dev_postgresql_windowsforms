using System;
using System.Windows.Forms;
using teste_dev_crud.Data;

namespace teste_dev_crud.Popups
{
    public partial class add_user : Form
    {
        private DataContr dbConnect;
        public add_user(DataContr _dbConnect)
        {
            InitializeComponent();
            this.dbConnect = _dbConnect;
            numericUpDown1.Controls[0].Visible = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try     // try catch devido à constraint do número, que deve ser único
            {
                dbConnect.InsertToDB(textBox1.Text, (int)numericUpDown1.Value);
                MessageBox.Show("Data inserted successfully!", "Success", MessageBoxButtons.OK);
            } catch (Npgsql.PostgresException ex) when (ex.SqlState == "23505") //Código 23505 aponta para constraints de unicidade no postgres
            {
                MessageBox.Show($"The number {numericUpDown1.Value} is already in use!", "Failed", MessageBoxButtons.OK);
            } catch (Npgsql.PostgresException ex) when (ex.SqlState == "23514") // Código 23514 aponta pra constraint do valor positivo
            {
                MessageBox.Show($"The number has to be greater than!\n\t({numericUpDown1.Value})", "Failed", MessageBoxButtons.OK);
            }
            catch (Exception ex) //E um catch de exceção genérico, vai saber
            {
                MessageBox.Show($"There was an error.\n{ex.Message}", "Error", MessageBoxButtons.OK);
            }
            this.Close();
        }
    }
}
