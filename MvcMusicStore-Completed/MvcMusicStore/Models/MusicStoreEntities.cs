﻿using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;

using System.Data.Entity.ModelConfiguration.Conventions;

namespace MvcMusicStore.Models
{
    public class MusicStoreEntities : DbContext
    {
        public DbSet<Album> Albums { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Artist> Artists { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<AlbumReview> AlbumReview { get; set; }
        public DbSet<UserDetail> UserDetails { get; set; }
        public DbSet<Comment> Comments { get; set; }

        protected override void OnModelCreating(System.Data.Entity.DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}