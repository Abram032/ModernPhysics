using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ModernPhysics.Web.Data
{
    public class WebIdentityDbContext : IdentityDbContext
    {
        public WebIdentityDbContext(DbContextOptions<WebIdentityDbContext> options)
            : base(options)
        {
        }
    }
}
