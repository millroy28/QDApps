using QDApps.Models;
using QDApps.Models.ViewModels;

namespace QDApps.Context
{
    public class ModelHelper
    {
        internal readonly QdappsContext _context;
        public ModelHelper(QdappsContext context) {_context = context;}

        #region Users
        public bool IsUserNew(string aspNetUserId)
        {
            return !_context.Users.Where(x => x.AspNetUserId == aspNetUserId).Any();
        }
        public List<QDApps.Models.TimeZone> GetTimeZones()
        {
            return _context.TimeZones.OrderBy(x => x.Utcoffset).ToList();
        }
        public bool EditUser(EditUser editUser)
        {
            if (editUser.UserId == 0)
            {
                QDApps.Models.User user = new()
                {
                    AspNetUserId = editUser.AspNetUserId,
                    UserName = editUser.UserName,
                    TimeZoneId = editUser.TimeZoneId
                };

                try
                {
                    _context.Users.Add(user);
                    _context.SaveChanges();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
            else
            {
                try
                {
                    User user = _context.Users.Single(x => x.UserId == editUser.UserId);
                    user.UserName = editUser.UserName;
                    user.TimeZoneId = editUser.TimeZoneId;
                    _context.SaveChanges();
                    return true;
                }
                catch
                {
                    return false;
                }
            }


        }
        public int GetUserId(string aspNetUserId)
        {
            int userId = 0;
            if(_context.Users.Where(x=>x.AspNetUserId == aspNetUserId).Any())
            {
                userId = _context.Users.Single(x => x.AspNetUserId == aspNetUserId).UserId;
            }
            return userId;
        }
        public string GetUserName(int userId) 
        {
            string userName = (_context.Users?.Single(x => x.UserId == userId).UserName) ?? "";

            return userName;
        }
        public User GetUser(int userId)
        {
            return _context.Users.Single(x => x.UserId == userId);
        }


        
        #endregion
    }
}
