using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace MinhaLoja.Pages.Clientes
{
    public class CriarModel : PageModel
    {
        public ClientInfo clientInfo = new ClientInfo();
        public String errMessage = "";
        public String sucessMessage = "";
        public void OnGet()
        {
        }

        public void OnPost()
        {
            clientInfo.name = Request.Form["name"];
            clientInfo.email = Request.Form["email"];
            clientInfo.phone = Request.Form["phone"];
            clientInfo.address = Request.Form["address"];

            if(clientInfo.name.Length == 0 || clientInfo.email.Length == 0 || clientInfo.phone.Length == 0 || clientInfo.address.Length == 0)
            {
                errMessage = "Preencha todos os campos!";
                return;
            }
            // salvar no db

            try
            {
                String connectonString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\Victor\\VSCode\\MinhaLoja\\db\\db.mdf;Integrated Security=True;Connect Timeout=30";
                using (SqlConnection connection = new SqlConnection(connectonString))
                {
                    connection.Open();
                    String sql = "INSERT INTO clientes " +
                        "(nome, email, telefone, endereco) VALUES " +
                        "(@name, @email, @phone, @address);";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@name", clientInfo.name);
                        command.Parameters.AddWithValue("@email", clientInfo.email);
                        command.Parameters.AddWithValue("@phone", clientInfo.phone);
                        command.Parameters.AddWithValue("@address", clientInfo.address);

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch(Exception ex)
            {
                errMessage = ex.Message;
                return;
            }

            clientInfo.name = ""; clientInfo.email = ""; clientInfo.phone = ""; clientInfo.address = "";
            sucessMessage = "Novo cliente adicionado";

            Response.Redirect("/Clientes/Index");
        }
    }
}
