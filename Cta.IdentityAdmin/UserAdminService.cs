﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Cta.IdentityAdmin
{
    public class UserAdminService
    {
        public AspNetUser GetUserById(string id)
        {
            using (var db = new AspNetIdentity())
            {
                var user = db.AspNetUsers
                    .Where(x => x.Id == id)
                    .Include(x => x.AspNetRoles)
                    .Include(x => x.AspNetUserClaims)
                    .FirstOrDefault();

                return user;
            }
        }

        public AspNetUser GetUserByUsername(string username)
        {
            using (var db = new AspNetIdentity())
            {
                var user = db.AspNetUsers
                    .Where(x => x.UserName == username)
                    .Include(x => x.AspNetRoles)
                    .Include(x => x.AspNetUserClaims)
                    .FirstOrDefault();

                return user;
            }
        }

        public UserAdminServiceResult AddUserToRole(string userId, string roleName)
        {
            using (var db = new AspNetIdentity())
            {
                var user = db.AspNetUsers.Where(x => x.Id == userId).Include(x => x.AspNetRoles).FirstOrDefault();
                if (user.IsInRole(roleName)) return UserAdminServiceResult.RoleChangeSuccessful;
                var role = db.AspNetRoles.FirstOrDefault(x => x.NormalizedName == roleName.ToLower());
                user.AspNetRoles.Add(role);
                db.SaveChanges();
            }
            return UserAdminServiceResult.RoleChangeSuccessful;
        }

        public UserAdminServiceResult RemoveUserFromRole(string userId, string roleName)
        {
            using (var db = new AspNetIdentity())
            {
                var user = db.AspNetUsers.Where(x => x.Id == userId).Include(x => x.AspNetRoles).FirstOrDefault();
                if (!user.IsInRole(roleName)) return UserAdminServiceResult.RoleChangeSuccessful;
                var role = db.AspNetRoles.FirstOrDefault(x => x.NormalizedName == roleName.ToLower());
                user.AspNetRoles.Remove(role);
                db.SaveChanges();
            }
            return UserAdminServiceResult.RoleChangeSuccessful;
        }

        public UserAdminServiceResult UpdateUsername(string userId, string newUsername)
        {
            using (var db = new AspNetIdentity())
            {
                var user = db.AspNetUsers.SingleOrDefault(u => u.Id == userId);
                if (user == null) return UserAdminServiceResult.UserDoesNotExist;
                var targetUser = db.AspNetUsers.FirstOrDefault(x => x.UserName == newUsername);
                if (targetUser != null) return UserAdminServiceResult.UsernameUnavailable;
                user.UserName = newUsername;
                user.NormalizedUserName = newUsername.ToLower();
                db.SaveChanges();
            }
            return UserAdminServiceResult.UsernameChangeSuccessful;
        }

        public UserAdminServiceResult UpdateUser(AspNetUser user)
        {
            using (var db = new AspNetIdentity())
            {
                var x = db.AspNetUsers.SingleOrDefault(u => u.Id == user.Id);

                if(x == null) return UserAdminServiceResult.UserDoesNotExist;

                x.FirstName = user.FirstName;
                x.LastName = user.LastName;
                x.Email = user.Email;
                x.NormalizedEmail = user.Email.ToLower();
                x.PhoneNumber = user.PhoneNumber;
                db.SaveChanges();
            }
            return UserAdminServiceResult.UserUpdateSuccessful;
        }

        public AspNetUser CreateUser(string username, string email, string firstName, string lastName, string phoneNumber)
        {
            using (var db = new AspNetIdentity())
            {
                var targetUser = db.AspNetUsers.FirstOrDefault(x => x.UserName == username);

                if (targetUser != null) return null;

                var newUser = new AspNetUser {
                    UserName = username,
                    NormalizedUserName = username.ToLower(),
                    Email = email,
                    NormalizedEmail = email.ToLower(),
                    FirstName = firstName,
                    LastName = lastName,
                    PhoneNumber = phoneNumber,
                    LockoutEnabled = true,
                    //a new security stamp is necessary for new users, where password hash 
                    //is still null, to allow these new users to reset their password
                    SecurityStamp = Guid.NewGuid().ToString() 
                };
                db.AspNetUsers.Add(newUser);
                db.SaveChanges();
                return newUser;
            }
        }

        public AspNetRole GetRoleByName(string roleName)
        {
            using (var db = new AspNetIdentity())
            {
                var role = db.AspNetRoles.FirstOrDefault(x => x.Name == roleName);
                return role;
            }
        }

        public List<AspNetUser> GetUsersByRole(string roleName)
        {
            using (var db = new AspNetIdentity())
            {
                if (string.IsNullOrWhiteSpace(roleName)) return null;

                var roles = db.AspNetRoles.Where(r => r.Name == roleName);

                if (!roles.Any()) return null;

                var roleId = roles.First().Id;

                return db.AspNetUsers
                    .Where(x => x.AspNetRoles.Any(r => r.Id == roleId))
                    .Include(x => x.AspNetRoles)
                    .Include(x => x.AspNetUserClaims)
                    .ToList();
            }
        }

        public enum UserAdminServiceResult
        {
            UserDoesNotExist,
            UsernameUnavailable,
            UsernameChangeSuccessful,
            UserUpdateSuccessful,
            RoleChangeSuccessful
        }
    }
}
