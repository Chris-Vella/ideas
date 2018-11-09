using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System;
using BrightIdeas.Models;


namespace BrightIdeas.Models
{
    using BrightIdeas.Models;
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options) : base(options) { }
        public DbSet<User> Users {get;set;}
        public DbSet<Post> Posts {get;set;}
        public DbSet<Like> Likes {get;set;}
    }
}