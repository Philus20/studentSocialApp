using FinalProject.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;


namespace FinalProject.services
{
        public class ChatServices
        {
            private readonly AppDb _appDb;
        public string userName;
        public ChatServices()
            {
                _appDb = new AppDb();
            }

            // ... existing methods ...

            public bool AddUserTOList(string userToAdd)
            {
                // Check if the user already exists in the database
                if ( _appDb.Users.Any(u => u.UserName.ToLower() == userToAdd.ToLower()))
                {
                
                    return true; // User already exists
                }

                // Add the user to the database
                _appDb.Users.Add(new User { UserName = userToAdd });
                _appDb.SaveChanges();

                return true;
            }

            public void AddUserConnectionId(string user, string connectionId)
            {
                var userEntity = _appDb.Users.FirstOrDefault(u => u.UserName == user);

                if (userEntity != null)
                {
                    userEntity.ConnectionId = connectionId;
                    _appDb.SaveChanges();
                }
            }
        public string GetUserByConnectionId(string connectionId)
        {
        
               var userEntity = _appDb.Users.FirstOrDefault(x=> x.ConnectionId == connectionId);
            return userEntity?.UserName;
            
        }

        public string GetConnectionIdByUser(string user)
            {
                var userEntity = _appDb.Users.FirstOrDefault(u => u.UserName == user);
                return userEntity?.ConnectionId;
            }

            public string RemoveUserFromList(string user)
            {
                var userEntity = _appDb.Users.FirstOrDefault(u => u.UserName == user);

                if (userEntity != null)
                {
                    _appDb.Users.Remove(userEntity);
                    _appDb.SaveChanges();
                return "successfull remove";
                }
            return "error occured while removing user";
            }
        public string[] GetOnlineUsers()
        {
            

               
                return _appDb.Users.OrderBy(x => x.UserName).Select(X => X.UserName).ToArray();
            
        }
    }
    

}
