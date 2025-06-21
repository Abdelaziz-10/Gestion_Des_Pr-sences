using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace Gestion_Des_prèneces.Models
{
    public partial class Gestion_Des_PrésencesContext : DbContext
    {
        public Gestion_Des_PrésencesContext()
        {
        }

        public Gestion_Des_PrésencesContext(DbContextOptions<Gestion_Des_PrésencesContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Absence> Absences { get; set; }
        public virtual DbSet<Bulletin> Bulletins { get; set; }
        public virtual DbSet<Collaborateur> Collaborateurs { get; set; }
        public virtual DbSet<Congé> Congés { get; set; }
        public virtual DbSet<Disponibilité> Disponibilités { get; set; }
        public virtual DbSet<Mission> Missions { get; set; }
        public virtual DbSet<Respocollaborateur> Respocollaborateurs { get; set; }
        public virtual DbSet<Responsable> Responsables { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=.\\SQLExpress;Database=Gestion_Des_Présences;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Absence>(entity =>
            {
                entity.HasKey(e => e.IdAbsence)
                    .HasName("PK__Absence__60E131EFF7269516");

                entity.ToTable("Absence");

                entity.Property(e => e.IdAbsence).HasColumnName("Id_Absence");

                entity.Property(e => e.DateDebutAb)
                    .HasColumnType("date")
                    .HasColumnName("Date_Debut_Ab");

                entity.Property(e => e.DateFinAb)
                    .HasColumnType("date")
                    .HasColumnName("Date_Fin_Ab");

                entity.Property(e => e.NbHAb).HasColumnName("Nb_H_Ab");

                entity.HasOne(d => d.NumClNavigation)
                    .WithMany(p => p.Absences)
                    .HasForeignKey(d => d.NumCl)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("Fk_Col_Absence");
            });

            modelBuilder.Entity<Bulletin>(entity =>
            {
                entity.HasKey(e => e.IdBulletin)
                    .HasName("PK__Bulletin__B14091A2E338F29E");

                entity.ToTable("Bulletin");

                entity.Property(e => e.IdBulletin).HasColumnName("Id_Bulletin");

                entity.Property(e => e.DateBulletin)
                    .HasColumnType("date")
                    .HasColumnName("Date_Bulletin");

                entity.Property(e => e.NbHT).HasColumnName("Nb_H_t");

                entity.HasOne(d => d.NumClNavigation)
                    .WithMany(p => p.Bulletins)
                    .HasForeignKey(d => d.NumCl)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("Fk_Col_Bulletin");
            });

            modelBuilder.Entity<Collaborateur>(entity =>
            {
                entity.HasKey(e => e.NumCl)
                    .HasName("PK__Collabor__E32040A27367E6C0");

                entity.Property(e => e.Adresse).HasMaxLength(50);

                entity.Property(e => e.Email).HasMaxLength(50);

                entity.Property(e => e.IdMission).HasColumnName("Id_Mission");

                entity.Property(e => e.MotDePasse).HasMaxLength(50);

                entity.Property(e => e.NomCl).HasMaxLength(50);

                entity.Property(e => e.PrenomCl).HasMaxLength(50);

                entity.Property(e => e.Tel).HasMaxLength(50);

                entity.HasOne(d => d.IdMissionNavigation)
                    .WithMany(p => p.Collaborateurs)
                    .HasForeignKey(d => d.IdMission)
                    .HasConstraintName("FK__Collabora__Id_Mi__3A81B327");
            });

            modelBuilder.Entity<Congé>(entity =>
            {
                entity.HasKey(e => e.IdCongé)
                    .HasName("PK__Congé__F35DCB245955962B");

                entity.ToTable("Congé");

                entity.Property(e => e.IdCongé).HasColumnName("Id_Congé");

                entity.Property(e => e.Accord)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('pas encore vérifié')");

                entity.Property(e => e.DateDebutCongé)
                    .HasColumnType("date")
                    .HasColumnName("Date_Debut_Congé");

                entity.Property(e => e.DateFinCongé)
                    .HasColumnType("date")
                    .HasColumnName("Date_Fin_Congé");

                entity.Property(e => e.Types)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.NumClNavigation)
                    .WithMany(p => p.Congés)
                    .HasForeignKey(d => d.NumCl)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("Fk_Col_Congé");
            });

            modelBuilder.Entity<Disponibilité>(entity =>
            {
                entity.HasKey(e => e.IdDisponibilité)
                    .HasName("PK__Disponib__EB09817FE7DDC9E2");

                entity.ToTable("Disponibilité");

                entity.Property(e => e.IdDisponibilité).HasColumnName("Id_Disponibilité");

                entity.Property(e => e.DateHDebutDisponibilité)
                    .HasColumnType("datetime")
                    .HasColumnName("Date_H_Debut_Disponibilité");

                entity.Property(e => e.DateHFinDisponibilité)
                    .HasColumnType("datetime")
                    .HasColumnName("Date_H_Fin_Disponibilité");

                entity.Property(e => e.DateMiseEnDisponibilité)
                    .HasColumnType("date")
                    .HasColumnName("Date_MiseEnDisponibilité");

                entity.HasOne(d => d.NumClNavigation)
                    .WithMany(p => p.Disponibilités)
                    .HasForeignKey(d => d.NumCl)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("Fk_Col_Disponibilité");
            });

            modelBuilder.Entity<Mission>(entity =>
            {
                entity.HasKey(e => e.IdMission)
                    .HasName("PK__Mission__104417B12F52FC34");

                entity.ToTable("Mission");

                entity.Property(e => e.IdMission).HasColumnName("Id_Mission");

                entity.Property(e => e.NomMission)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("Nom_Mission");
            });

            modelBuilder.Entity<Respocollaborateur>(entity =>
            {
                entity.HasKey(e => new { e.NumRe, e.NumCl })
                    .HasName("PK__Respocol__479B6474EA8A66FF");

                entity.Property(e => e.NumRe).HasColumnName("Num_Re");

                entity.HasOne(d => d.NumClNavigation)
                    .WithMany(p => p.Respocollaborateurs)
                    .HasForeignKey(d => d.NumCl)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Fk_collaborateur");

                entity.HasOne(d => d.NumReNavigation)
                    .WithMany(p => p.Respocollaborateurs)
                    .HasForeignKey(d => d.NumRe)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Fk_Resp");
            });

            modelBuilder.Entity<Responsable>(entity =>
            {
                entity.HasKey(e => e.NumRe)
                    .HasName("PK__Responsa__79A9607E17D6A910");

                entity.ToTable("Responsable");

                entity.Property(e => e.NumRe).HasColumnName("Num_Re");

                entity.Property(e => e.Adresse)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Email)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.MotDePasse)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.NomRe)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("Nom_Re");

                entity.Property(e => e.PrenomRe)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("Prenom_Re");

                entity.Property(e => e.Tel)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
