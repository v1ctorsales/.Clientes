using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace MinhaLoja.Pages.Clientes
{
    public class EditarModel : PageModel
    {
        public ClientInfo clientInfo = new ClientInfo();
        public String errMessage = "";
        public String sucessMessage = "";

        public void OnGet()
        {
            String id = Request.Query["id"];

            try
            {
                String connString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\Victor\\VSCode\\MinhaLoja\\db\\db.mdf;Integrated Security=True;Connect Timeout=30";
                using (SqlConnection connection = new SqlConnection(connString))
                {
                    connection.Open();
                    String sql = "SELECT * FROM clientes WHERE id=@id";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                clientInfo.id = "" + reader.GetInt32(0);
                                clientInfo.name =  reader.GetString(1);
                                clientInfo.email = reader.GetString(2);
                                clientInfo.phone = reader.GetString(3);
                                clientInfo.address = reader.GetString(4);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
            }
        }
        
        public void OnPost()
        {
            clientInfo.id = Request.Form["id"];
            clientInfo.name = Request.Form["name"];
            clientInfo.email = Request.Form["email"];
            clientInfo.phone = Request.Form["phone"];
            clientInfo.address = Request.Form["address"];

            if (clientInfo.id.Length == 0 || clientInfo.name.Length == 0 ||
                clientInfo.email.Length == 0 || clientInfo.phone.Length == 0 ||
                clientInfo.address.Length == 0)
            {
                errMessage = "Preencha todos os campos!";
                return;
            }

            try
            {
                String connectonString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\Victor\\VSCode\\MinhaLoja\\db\\db.mdf;Integrated Security=True;Connect Timeout=30";
                using (SqlConnection connection = new SqlConnection(connectonString))
                {
                    connection.Open();
                    String sql = "UPDATE clientes " +
                                "SET nome=@name, email=@email, telefone=@phone, endereco=@address " +
                                "WHERE id=@id";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@name", clientInfo.name);
                        command.Parameters.AddWithValue("@email", clientInfo.email);
                        command.Parameters.AddWithValue("@phone", clientInfo.phone);
                        command.Parameters.AddWithValue("@address", clientInfo.address);
                        command.Parameters.AddWithValue("@id", clientInfo.id);

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex )
            {
                errMessage = ex.Message;
                return;
            }

            Response.Redirect("/Clientes/Index");
        }
    }
}
