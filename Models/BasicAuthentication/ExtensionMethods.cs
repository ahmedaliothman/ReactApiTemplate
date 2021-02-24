using System.Collections.Generic;
using System.Linq;
using Api.Models.DB;

namespace Api.Models.BasicAuthentication

{
    public static class ExtensionMethods
    {
       
        public static IEnumerable<SystemUser> WithoutPasswords(this IEnumerable<SystemUser> users) {
            return users.Select(x => x.WithoutPassword());
        }

        public static SystemUser WithoutPassword(this SystemUser user) {
            user.Password = null;
            return user;
        }
    }
}