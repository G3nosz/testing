namespace SVS.Areas.Identity.Pages.Account.Manage.Storage
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Text;
    using System.Threading.Tasks;
    using Areas.Storage.Data;
    using Data;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.Data.SqlClient;
    using Microsoft.EntityFrameworkCore;
    using Model;

    [Authorize(Roles = "Admin, Employee")]
    public class CreateModel : PageModel
    {
        private readonly ParcelContext        _context;
        private readonly UserManager<SVSUser> _userManager;

        public CreateModel(ParcelContext context, UserManager<SVSUser> userManager)
        {
            _context     = context;
            _userManager = userManager;
        }

        [BindProperty]
        public Parcel Parcel { get; set; }

        public IList<Parcel> Parcels { get; set; }

        public IActionResult OnGet()
        {
            return Page();
        }

        public string UniqueIDGenerator()
        {
            var con = new SqlConnection("Server=(localdb)\\mssqllocaldb;Database=SVS;MultipleActiveResultSets=true");
            con.Open();
            var cmd   = new SqlCommand("SELECT MAX(Number) FROM Parcels", con).ExecuteScalar();
            var count = cmd == DBNull.Value ? 0 : Convert.ToInt32(cmd);
            var now   = DateTime.Now.ToUniversalTime();

            this.Parcel.Number = count + 1;
            var primaryKey = count.ToString();
            primaryKey += now.DayOfYear.ToString("D3")   +
                          now.Millisecond.ToString("D3") +
                          now.Second.ToString("D2")      +
                          now.Minute.ToString("D2");
            HashAlgorithm algorithm = SHA1.Create();
            var           hash      = algorithm.ComputeHash(Encoding.UTF8.GetBytes(primaryKey));
            var           sb        = new StringBuilder();
            sb.Append("SVS");
            for (var i = 0; i < 6; ++i)
            {
                sb.Append(hash[i].ToString("X2"));
            }

            return sb.ToString();
        }

        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!this.ModelState.IsValid)
            {
                return Page();
            }

            this.Parcel.ID     = this.Parcel.Sender.ID = this.Parcel.Receiver.ID = UniqueIDGenerator();
            this.Parcel.Status = "Warehouse";

            _context.Parcels.Add(this.Parcel);

            await _context.SaveChangesAsync();

            this.Parcels = await _context.Parcels.ToListAsync();
            var user = await _userManager.GetUserAsync(this.User);
            if (user.TwoFactorEnabled)
            {
                return Redirect(this.Request.Headers["Referer"].ToString());
            }

            var con = new SqlConnection("Server=(localdb)\\mssqllocaldb;Database=SVS;MultipleActiveResultSets=true");
            con.Open();
            var cmd = new SqlCommand(
                "SELECT UserId FROM AspNetUserRoles WHERE RoleId = '087a0af3-1aa9-46ae-a317-cc8501d6e210' ",
                con);

            var DriverIDList = new List<DriverInfo>();
            await using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    var userID = reader["UserId"].ToString();

                    var line = "SELECT COUNT(ID) FROM Parcels WHERE Driver = '" +
                               reader["UserId"]                                 +
                               "' AND NOT Status='Warehouse'";
                    var cm    = new SqlCommand(line, con);
                    var count = Convert.ToInt32(cm.ExecuteScalar());

                    var cmdUser =
                        (bool) new SqlCommand("SELECT TwoFactorEnabled FROM AspNetUsers WHERE Id = '" + userID + "'",
                                              con).
                            ExecuteScalar();
                    Debug.WriteLine(cmdUser);
                    if (cmdUser)
                    {
                        continue;
                    }

                    DriverIDList.Add(new DriverInfo
                    {
                        ID    = userID,
                        Count = count
                    });
                }
            }

            var parcelLimit = 10;
            foreach (var parcel in this.Parcels.Where(x => x.Status == "Warehouse").OrderBy(x => x.Number))
            {
                DriverIDList = DriverIDList.OrderBy(x => x.Count).ToList();
                foreach (var t in DriverIDList)
                {
                    var ID    = t.ID;
                    var Count = t.Count;
                    if (Count >= parcelLimit)
                    {
                        continue;
                    }

                    parcel.Driver = ID;
                    parcel.Status = "Shipped";
                    t.Count++;

                    await _context.SaveChangesAsync();

                    break;
                }
            }

            return RedirectToPage("./Added", new {this.Parcel.ID});
        }

        private class DriverInfo
        {
            public string ID    { get; set; }
            public int    Count { get; set; }
        }
    }
}