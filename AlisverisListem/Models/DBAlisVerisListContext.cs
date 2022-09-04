using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace AlisverisListem.Models
{
    public partial class DBAlisVerisListContext : DbContext
    {
        public DBAlisVerisListContext()
        {
        }

        public DBAlisVerisListContext(DbContextOptions<DBAlisVerisListContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Kategoriler> Kategorilers { get; set; } = null!;
        public virtual DbSet<Kullanicilar> Kullanicilars { get; set; } = null!;
        public virtual DbSet<ListeEleman> ListeElemen { get; set; } = null!;
        public virtual DbSet<Listeler> Listelers { get; set; } = null!;
        public virtual DbSet<Urunler> Urunlers { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {

                optionsBuilder.UseSqlServer("Server=.;Database=DBAlisVerisList;Trusted_Connection=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Kategoriler>(entity =>
            {
                entity.HasKey(e => e.KategoriId);

                entity.ToTable("Kategoriler");

                entity.Property(e => e.KategoriId).HasColumnName("KategoriID");

                entity.Property(e => e.KategoriAd).HasMaxLength(50);
            });

            modelBuilder.Entity<Kullanicilar>(entity =>
            {
                entity.HasKey(e => e.KullaniciId);

                entity.ToTable("Kullanicilar");

                entity.Property(e => e.KullaniciId).HasColumnName("KullaniciID");

                entity.Property(e => e.KullaniciAd)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.KullaniciMail)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.KullaniciSifre)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.KullaniciSoyad)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<ListeEleman>(entity =>
            {
                entity.ToTable("ListeEleman");

                entity.Property(e => e.ListeElemanId).HasColumnName("ListeElemanID");

                entity.Property(e => e.ListeId).HasColumnName("ListeID");

                entity.Property(e => e.UrunId).HasColumnName("UrunID");

                entity.Property(e => e.UrunNot).IsUnicode(false);

                entity.HasOne(d => d.Liste)
                    .WithMany(p => p.ListeElemen)
                    .HasForeignKey(d => d.ListeId)
                    .HasConstraintName("FK_ListeDetay_Listeler");

                entity.HasOne(d => d.Urun)
                    .WithMany(p => p.ListeElemen)
                    .HasForeignKey(d => d.UrunId)
                    .HasConstraintName("FK_ListeDetay_Urunler");
            });

            modelBuilder.Entity<Listeler>(entity =>
            {
                entity.HasKey(e => e.ListeId);

                entity.ToTable("Listeler");

                entity.Property(e => e.ListeId).HasColumnName("ListeID");

                entity.Property(e => e.KullaniciId).HasColumnName("KullaniciID");

                entity.Property(e => e.ListeAd)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.HasOne(d => d.Kullanici)
                    .WithMany(p => p.Listelers)
                    .HasForeignKey(d => d.KullaniciId)
                    .HasConstraintName("FK_Listeler_Kullanicilar");
            });

            modelBuilder.Entity<Urunler>(entity =>
            {
                entity.HasKey(e => e.UrunId);

                entity.ToTable("Urunler");

                entity.Property(e => e.UrunId).HasColumnName("UrunID");

                entity.Property(e => e.KategoriId).HasColumnName("KategoriID");

                entity.Property(e => e.UrunAd)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UrunResim).IsUnicode(false);

                entity.HasOne(d => d.Kategori)
                    .WithMany(p => p.Urunlers)
                    .HasForeignKey(d => d.KategoriId)
                    .HasConstraintName("FK_Urunler_Kategoriler");
            });
            modelBuilder.Entity<Kullanicilar>().HasData(new Kullanicilar
            {
                KullaniciId = 1,
                KullaniciAd = "admin",
                KullaniciSoyad = "admin",
                KullaniciMail = "admin@mail.com",
                KullaniciSifre = "Sifre1234"

            });

            OnModelCreatingPartial(modelBuilder);
        }


        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
