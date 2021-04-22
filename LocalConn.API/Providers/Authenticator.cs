﻿using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using LocalConn.API.Models;

namespace LocalConn.API.Providers
{
    public class Authenticator : IDisposable
    {
        private readonly ApplicationDbContext _ctx;

        public readonly UserManager<ApplicationUser> _userManager;
        public Authenticator()
        {
            _ctx = new ApplicationDbContext();
            _userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(_ctx));
        }

        public async Task<ApplicationUser> FindUser(string userName, string password)
        {
            ApplicationUser user = await _userManager.FindAsync(userName, password);
            if (user == null)
            {
                user = new ApplicationUser() { Email = null,RoleName = "Customer", ProfileName = null, UserName = userName, PhoneNumber = userName, IsActive = true };
                IdentityResult result = await _userManager.CreateAsync(user,password);
            }
            return user;
        }

        public void Dispose()
        {
            _ctx.Dispose();
            _userManager.Dispose();

        }
    }
}