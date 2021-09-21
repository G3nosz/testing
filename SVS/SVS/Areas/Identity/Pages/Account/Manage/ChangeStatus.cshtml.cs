namespace SVS.Areas.Identity.Pages.Account.Manage.Storage
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Areas.Storage.Data;
    using Data;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.Data.SqlClient;
    using Microsoft.EntityFrameworkCore;
    using Model;

    public class ChangeStatusModel : PageModel
    {
        private readonly ParcelContext        _context;
        private readonly UserManager<SVSUser> _userManager;

        public ChangeStatusModel(
            UserManager<SVSUser> userManager,
            ParcelContext        context)
        {
            _userManager = userManager;
            _context     = context;
        }

        public IList<Parcel> Parcel { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(this.User);
            if (user == null)
            {
                return Redirect(this.Request.Headers["Referer"].ToString());
            }

            await _userManager.SetTwoFactorEnabledAsync(user, !user.TwoFactorEnabled);
            await _userManager.UpdateAsync(user);
            this.Parcel = await _context.Parcels.ToListAsync();
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
            foreach (var parcel in this.Parcel.Where(x => x.Status == "Warehouse").OrderBy(x => x.Number))
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

            return Redirect(this.Request.Headers["Referer"].ToString());
        }

        private class DriverInfo
        {
            public string ID    { get; set; }
            public int    Count { get; set; }
        }
    }
}