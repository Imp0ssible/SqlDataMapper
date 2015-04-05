using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace SqlDataMapper
{
    class Program
    {
        static void Main(string[] args)
        {
            var list = new List<Entity>();

            using (var connection = new SqlConnection("Server=.;Database=test;Trusted_Connection=True;"))
            {
                using (var cmd = new SqlCommand("select * from entity", connection))
                {
                    connection.Open();
                    list.AddRange(cmd.ExecuteReader().MapToList<Entity>());
                }
            }

            Console.Read();
        }
    }
}
