using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace WatchMovies.Models
{
    public class WatchMoviesContext:DbContext
    {
        public WatchMoviesContext():base("name=ChuoiKN")
        {
            
        }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Phim> Phims { get; set; }
        public virtual DbSet<YeuThich> YeuThichs { get; set; }
        public virtual DbSet<TheLoai> TheLoais { get; set; }
        public virtual DbSet<LichSuXem> LichSuXems { get; set; }
        public virtual DbSet<ChatLuong> ChatLuongs { get; set; }
        public virtual DbSet<Comment> Comments { get; set; }
        public virtual DbSet<BaiDang> BaiDangs { get; set; }
        public virtual DbSet<Vote> Votes { get; set; }
        public virtual DbSet<Quyen> Quyens { get; set; }
        public virtual DbSet<Report> Reports { get; set; }
        public virtual DbSet<Phan> Phans { get; set; }
        public virtual DbSet<Tap> Taps { get; set; }
        public virtual DbSet<TheLoai_Phim> TheLoai_Phims { get; set; }
        public virtual DbSet<Quyen_User> Quyen_Users { get; set; }
        public virtual DbSet<ThongBao> ThongBaos { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //ChatLuong
            modelBuilder.Entity<ChatLuong>()
                .HasMany(e => e.phims)
                .WithRequired(e => e.ChatLuong)
                .WillCascadeOnDelete(false);
            //Phim
            modelBuilder.Entity<Phim>()
                .HasMany(e => e.comments)
                .WithRequired(e => e.Phim)
                .WillCascadeOnDelete(false);
            modelBuilder.Entity<Phim>()
                .HasMany(e => e.Phans)
                .WithRequired(e => e.Phim)
                .WillCascadeOnDelete(false);
            modelBuilder.Entity<Phim>()
                .HasMany(e => e.votes)
                .WithRequired(e => e.Phim)
                .WillCascadeOnDelete(false);
            modelBuilder.Entity<Phim>()
                .HasMany(e => e.yeuThichs)
                .WithRequired(e => e.Phim)
                .WillCascadeOnDelete(false);
            modelBuilder.Entity<Phim>()
                .HasMany(e => e.lichSuXems)
                .WithRequired(e => e.Phim)
                .WillCascadeOnDelete(false);
            modelBuilder.Entity<Phim>()
                .HasMany(e => e.TheLoai_Phims)
                .WithRequired(e => e.Phim)
                .WillCascadeOnDelete(false);
            //TheLoai
            modelBuilder.Entity<TheLoai>()
                .HasMany(e => e.TheLoai_Phims)
                .WithRequired(e => e.TheLoai)
                .WillCascadeOnDelete(false);
            //User
            modelBuilder.Entity<User>()
                .HasMany(e => e.yeuThichs)
                .WithRequired(e => e.User)
                .WillCascadeOnDelete(false);
            modelBuilder.Entity<User>()
                .HasMany(e => e.lichSuXems)
                .WithRequired(e => e.User)
                .WillCascadeOnDelete(false);
            modelBuilder.Entity<User>()
                .HasMany(e => e.comments)
                .WithRequired(e => e.User)
                .WillCascadeOnDelete(false);
            modelBuilder.Entity<User>()
                .HasMany(e => e.votes)
                .WithRequired(e => e.User)
                .WillCascadeOnDelete(false);
            modelBuilder.Entity<User>()
                .HasMany(e => e.BaiDangsTao)
                .WithRequired(e => e.UserTao)
                .WillCascadeOnDelete(false);
            modelBuilder.Entity<User>()
                .HasMany(e => e.Quyen_Users)
                .WithRequired(e => e.User)
                .WillCascadeOnDelete(false);
            modelBuilder.Entity<User>()
                .HasMany(e => e.reports)
                .WithRequired(e => e.User)
                .WillCascadeOnDelete(false);
            modelBuilder.Entity<User>()
                .HasMany(e => e.ThongBaos)
                .WithRequired(e => e.User)
                .WillCascadeOnDelete(false);
            //Quyen
            modelBuilder.Entity<Quyen>()
                .HasMany(e => e.Quyen_Users)
                .WithRequired(e => e.Quyen)
                .WillCascadeOnDelete(false);
            //Comment
            modelBuilder.Entity<Comment>()
                .HasMany(e => e.reports)
                .WithRequired(e => e.Comment)
                .WillCascadeOnDelete(false);
            //Phan
            modelBuilder.Entity<Phan>()
                .HasMany(e => e.Taps)
                .WithRequired(e => e.Phan)
                .WillCascadeOnDelete(false);
            //Tap
            modelBuilder.Entity<Tap>()
                .HasMany(e => e.Comments)
                .WithOptional(e => e.Tap)
                .WillCascadeOnDelete(false);
        }
    }
}