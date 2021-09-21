namespace SVS.Pages.Parcels
{
    using System;
    using System.Data;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.Data.SqlClient;
    using Model;

    public class DetailsModel : PageModel
    {
        public Parcel Parcel { get; set; }
        public string IsFragile { get; set; }
        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var queryString = "SELECT * FROM Parcels WHERE ID = @id";

            await using (var conn =
                new SqlConnection("Server=(localdb)\\mssqllocaldb;Database=SVS;MultipleActiveResultSets=true"))
            await using (var cmd = new SqlCommand(queryString, conn))
            {
                cmd.Parameters.Add("@id", SqlDbType.VarChar, 20).Value = id;
                conn.Open();

                await using var rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                if (!rdr.Read())
                {
                    return Page();
                }

                this.Parcel = new Parcel
                {
                    Weight  = (float) rdr["Weight"],
                    Length  = (float) rdr["Length"],
                    Width   = (float) rdr["Width"],
                    Height  = (float) rdr["Height"],
                    Fragile = (bool) rdr["Fragile"],
                    Status  = rdr["Status"] == DBNull.Value ? "" : (string) rdr["Status"]
                };
            }

            IsFragile = this.Parcel.Fragile ? "Yes" : "No";

            return Page();
        }
    }
}