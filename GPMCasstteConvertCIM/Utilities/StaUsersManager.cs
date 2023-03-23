﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace GPMCasstteConvertCIM.Utilities
{
    internal class StaUsersManager
    {
        internal enum USER_GROUP
        {
            VISITOR,
            USER_ENG,
            GPM_ENG,
            GPM_RD
        }
        internal class User
        {
            public USER_GROUP Group { get; set; }
            public string Name { get; set; }
            public string Password { get; set; }
        }


        internal static User CurrentUser { get; private set; } = new User()
        {
            Group = USER_GROUP.VISITOR,
        };

        internal static Dictionary<USER_GROUP, List<User>> Users = new Dictionary<USER_GROUP, List<User>>()
        {
            {
                USER_GROUP.USER_ENG, new List<User>() { new User()} },
            {
                USER_GROUP.GPM_ENG, new List<User>() { new User()} },
            {
                USER_GROUP.GPM_RD, new List<User>() {
                    new User(){
                         Group = USER_GROUP.GPM_RD,
                         Name = "GPM",
                         Password ="33838628"
                    }
                }
            },
        };



        internal static bool TryLogin(string name, string pw, out User user)
        {
            user = Users.Values.SelectMany(v => v).FirstOrDefault(user => user.Name == name && user.Password == pw);
            bool success = user != null;
            CurrentUser = success ? user : new User()
            {
                Group = USER_GROUP.VISITOR,
            };

            return success;
        }
    }
}