using API.Uteis;
using Microsoft.Data.SqlClient;
using System;
using System.Data;
using System.Threading.Tasks;

namespace API.Infrastructure
{
    public class DataAccess
    {
        private readonly string _connectionString;
        private SqlConnection _connection;
        private static int _timeoutCmd = 20;

        public static int TimeoutComando { get { return _timeoutCmd; } set { _timeoutCmd = value; } }

        public DataAccess(string conn)
        {
            _connectionString = new Criptografia().Decriptografar(conn);
        }


        private bool OpenConnection()
        {
            bool resp = true;
            try
            {
                if (_connection == null) _connection = new(_connectionString);
                if (_connection.State != ConnectionState.Open) _connection.Open();
            }
            catch (Exception)
            {
                throw;
            }
            return resp;
        }

        public void CloseConnection()
        {
            if (_connection.State == ConnectionState.Open) _connection.Close();
        }

        public async Task<SqlDataReader> ExecuteReader(SqlCommand command)
        {
            SqlDataReader reader;
            try
            {
                OpenConnection();
                command.Connection = _connection;
                reader = await command.ExecuteReaderAsync();
            }
            catch (Exception)
            {
                throw;
            }
            return reader;
        }

        public Boolean ExecuteNonQuery(string commandText)
        {
            try
            {
                OpenConnection();
                SqlCommand command = new SqlCommand(commandText, _connection);
                command.Connection = _connection;
                command.ExecuteNonQuery();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public int ExecuteInsert(SqlCommand command)
        {
            int id;
            try
            {
                command.CommandText += ";SELECT SCOPE_IDENTITY()";
                OpenConnection();
                command.Connection = _connection;
                object resp = command.ExecuteScalarAsync();
                id = int.Parse(resp.ToString());
            }
            catch (Exception)
            {
                throw;
            }
            return id;
        }


        public void Dispose()
        {
            if (_connection != null)
            {
                if (_connection.State == ConnectionState.Open) _connection.Close();
                _connection = null;
            }
        }
    }
}
