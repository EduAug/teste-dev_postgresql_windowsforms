using Npgsql;
using System;
using System.Diagnostics;
using System.Collections.Generic;

namespace teste_dev_crud.Data
{
    public class DataContr
    {
        private string ConnectionString;

        public DataContr(string _connectionString)
        {
            this.ConnectionString = _connectionString;
        }

        private NpgsqlConnection GetConn()                  //Tornando funções de abrir conexão, etc. privadas
        {
            Debug.WriteLine("Got connection");
            return new NpgsqlConnection(ConnectionString);
        }

        private void OpenConn(NpgsqlConnection conn)        //Pois não faz sentido poder fazer isso fora daqui, não deve pelo menos
        {
            try 
            {
                conn.Open();
                Debug.WriteLine("Connectiong open");
            }
            catch 
            {
                Debug.WriteLine("Something went wrong when openning the connection");
            }
        }

        private void CloseConn(NpgsqlConnection conn) 
        {
            conn.Close();
            Debug.WriteLine("Connection closed");
        }

        private NpgsqlDataReader ExecuteQuery(string command, NpgsqlConnection conn)
        {
            using(NpgsqlCommand cmd = new NpgsqlCommand(command, conn)) 
            {
                return cmd.ExecuteReader();     //  Função extra criada só pra testar o primeiro get all, depreciada nas outras
            }
        }


        //Deixando apenas os "métodos" ou queries, públicas aos outros... componentes?

        public List<(string text, int number)> GetAllData()
        {
            List<(string resText, int resNum)> result = new List<(string resText, int resNum)>();

            using(NpgsqlConnection conn = GetConn()) //Abre conexão, pegando uma "nova"
            {
                OpenConn(conn); //Abre a conexão "nova"

                string queryToGetAllData = "SELECT texto, numero FROM user_table"; //query
                using(NpgsqlDataReader rdr = ExecuteQuery(queryToGetAllData, conn)) //Executa a query, com a conexão, por assim dizer
                {
                    while (rdr.Read()) //Meio que um "foreach", pros resultados da query
                    {
                        string text = rdr["texto"].ToString();
                        
                        int numb = Convert.ToInt32(rdr["numero"]);

                        result.Add((text, numb)); //Atribui cada par à tupla
                    }
                }
                CloseConn(conn); //Fecha conexão, segurança, pá
            }
            //foreach(var items in result)
                //Debug.WriteLine((items.resText, items.resNum));
            return result; //Retorna a tupla com o resultado esperado da query, o texto e o valor
        }

        public void InsertToDB(string text, int numb)
        {
            var dbConnection = GetConn();
            OpenConn(dbConnection);
            
            string queryToInsert = "INSERT INTO user_table (texto, numero) VALUES (@text, @nume)";

            using (NpgsqlCommand cmd = new NpgsqlCommand(queryToInsert, dbConnection))
            {
                cmd.Parameters.AddWithValue("@text", text);
                cmd.Parameters.AddWithValue("@nume", numb);
                cmd.ExecuteNonQuery();
            }
            CloseConn(dbConnection);
        }

        public void UpdateOnDB(string newText, int newNumb, int ogNumb)
        {
            var dbConnection = GetConn();
            OpenConn(dbConnection);

            string queryToUpdate = "UPDATE user_table SET texto = @newText, numero = @newNumb WHERE numero = @oldNumb";

            using(NpgsqlCommand cmd = new NpgsqlCommand(queryToUpdate, dbConnection))
            {
                cmd.Parameters.AddWithValue("@newText", newText);
                cmd.Parameters.AddWithValue("@newNumb", newNumb);
                cmd.Parameters.AddWithValue("@oldNumb", ogNumb);
                cmd.ExecuteNonQuery();
            }

            CloseConn(dbConnection);
        }

        public void DeleteOnDB(string text, int numb)
        {
            var dbConnection = GetConn();
            OpenConn(dbConnection);

            string queryToDelete = "DELETE FROM user_table WHERE texto = @text AND numero = @numb";
            //  Usando tanto o número quando o texto para ter certeza que só irá retirar um
            //  Devido a constraint de unique no número, já é uma certeza que tirará apenas um, mas por vier das dúvidas, não machuca

            using(NpgsqlCommand cmd = new NpgsqlCommand(queryToDelete, dbConnection))
            {
                cmd.Parameters.AddWithValue("@text", text);
                cmd.Parameters.AddWithValue("@numb", numb);
                cmd.ExecuteNonQuery();
            }
            CloseConn(dbConnection);
        }
    }
}
