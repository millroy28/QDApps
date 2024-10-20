using QDApps.Models.ViewModels;

namespace QDApps.Context
{
    public class ModelHelper
    {
        private readonly QdappsContext _context;
        public ModelHelper(QdappsContext context) {_context = context;}

        #region Users
        public bool IsUserNew(string aspUserId)
        {
            return !_context.Users.Where(x => x.AspNetUserId == aspUserId).Any();
        }
        public List<QDApps.Models.TimeZone> GetTimeZones()
        {
            return _context.TimeZones.OrderBy(x => x.Utcoffset).ToList();
        }
        public bool CreateNewUser(EditUser newUser)
        {
            QDApps.Models.User user = new()
            {
                AspNetUserId = newUser.AspNetUserId,
                UserName = newUser.UserName,
                TimeZoneId = newUser.TimeZoneId
                
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
        
        #endregion
    }
}
