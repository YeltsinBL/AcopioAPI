using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace AcopioAPIs.Models;

public partial class DbacopioContext : DbContext
{
    public DbacopioContext()
    {
    }

    public DbacopioContext(DbContextOptions<DbacopioContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AsignarTierra> AsignarTierras { get; set; }

    public virtual DbSet<AsignarTierraHistorial> AsignarTierraHistorials { get; set; }

    public virtual DbSet<Cosecha> Cosechas { get; set; }

    public virtual DbSet<CosechaTipo> CosechaTipos { get; set; }

    public virtual DbSet<Person> Persons { get; set; }

    public virtual DbSet<Proveedor> Proveedors { get; set; }

    public virtual DbSet<Ticket> Tickets { get; set; }

    public virtual DbSet<TicketEstado> TicketEstados { get; set; }

    public virtual DbSet<TicketHistorial> TicketHistorials { get; set; }

    public virtual DbSet<Tierra> Tierras { get; set; }

    public virtual DbSet<TypePerson> TypePeople { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AsignarTierra>(entity =>
        {
            entity.HasKey(e => e.AsignarTierraId).HasName("PK__AsignarT__9F4B1AF6331C27AD");

            entity.ToTable("AsignarTierra");

            entity.Property(e => e.AsignarTierraProveedor).HasColumnName("AsignarTierra_Proveedor");
            entity.Property(e => e.AsignarTierraTierra).HasColumnName("AsignarTierra_Tierra");
            entity.Property(e => e.UserCreatedName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.UserModifiedName)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(d => d.AsignarTierraProveedorNavigation).WithMany(p => p.AsignarTierras)
                .HasForeignKey(d => d.AsignarTierraProveedor)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__AsignarTi__Asign__34C8D9D1");

            entity.HasOne(d => d.AsignarTierraTierraNavigation).WithMany(p => p.AsignarTierras)
                .HasForeignKey(d => d.AsignarTierraTierra)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__AsignarTi__Asign__35BCFE0A");
        });

        modelBuilder.Entity<AsignarTierraHistorial>(entity =>
        {
            entity.HasKey(e => e.HistorialId).HasName("PK__AsignarT__9752068F181FAD42");

            entity.ToTable("AsignarTierraHistorial");

            entity.Property(e => e.UserModifiedAt).HasColumnType("datetime");
            entity.Property(e => e.UserModifiedName)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(d => d.Proveedor).WithMany(p => p.AsignarTierraHistorials)
                .HasForeignKey(d => d.ProveedorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__AsignarTi__Prove__4E88ABD4");

            entity.HasOne(d => d.Tierra).WithMany(p => p.AsignarTierraHistorials)
                .HasForeignKey(d => d.TierraId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__AsignarTi__Tierr__4D94879B");
        });

        modelBuilder.Entity<Cosecha>(entity =>
        {
            entity.HasKey(e => e.CosechaId).HasName("PK__Cosecha__BC3E89C2AC1F8B50");

            entity.ToTable("Cosecha");

            entity.Property(e => e.CosechaCosechaTipo).HasColumnName("Cosecha_CosechaTipo");
            entity.Property(e => e.CosechaHas)
                .HasColumnType("decimal(8, 2)")
                .HasColumnName("CosechaHAS");
            entity.Property(e => e.CosechaHumedad).HasColumnType("decimal(8, 2)");
            entity.Property(e => e.CosechaProveedor).HasColumnName("Cosecha_Proveedor");
            entity.Property(e => e.CosechaRed)
                .HasColumnType("decimal(8, 2)")
                .HasColumnName("CosechaRED");
            entity.Property(e => e.CosechaSac)
                .HasColumnType("decimal(8, 2)")
                .HasColumnName("CosechaSAC");
            entity.Property(e => e.CosechaSupervisor)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.CosechaTierra).HasColumnName("Cosecha_Tierra");
            entity.Property(e => e.UserCreatedName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.UserModifiedName)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(d => d.CosechaCosechaTipoNavigation).WithMany(p => p.Cosechas)
                .HasForeignKey(d => d.CosechaCosechaTipo)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Cosecha__Cosecha__3C69FB99");

            entity.HasOne(d => d.CosechaProveedorNavigation).WithMany(p => p.Cosechas)
                .HasForeignKey(d => d.CosechaProveedor)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Cosecha__Cosecha__3B75D760");

            entity.HasOne(d => d.CosechaTierraNavigation).WithMany(p => p.Cosechas)
                .HasForeignKey(d => d.CosechaTierra)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Cosecha__Cosecha__3A81B327");
        });

        modelBuilder.Entity<CosechaTipo>(entity =>
        {
            entity.HasKey(e => e.CosechaTipoId).HasName("PK__CosechaT__BD2C11AFAE6E865F");

            entity.ToTable("CosechaTipo");

            entity.Property(e => e.CosechaTipoDescripcion)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Person>(entity =>
        {
            entity.HasKey(e => e.PersonId).HasName("PK__Persons__AA2FFBE5D74AEBF7");

            entity.HasIndex(e => e.PersonDni, "UQ__Persons__47B8FF827B175767").IsUnique();

            entity.Property(e => e.PersonDni)
                .HasMaxLength(11)
                .IsUnicode(false)
                .HasColumnName("PersonDNI");
            entity.Property(e => e.PersonMaternalSurname)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.PersonName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.PersonPaternalSurname)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.PersonType).HasColumnName("Person_Type");

            entity.HasOne(d => d.PersonTypeNavigation).WithMany(p => p.People)
                .HasForeignKey(d => d.PersonType)
                .HasConstraintName("FK__Persons__Person___276EDEB3");
        });

        modelBuilder.Entity<Proveedor>(entity =>
        {
            entity.HasKey(e => e.ProveedorId).HasName("PK__Proveedo__61266A597EE06669");

            entity.ToTable("Proveedor");

            entity.HasIndex(e => e.ProveedorUt, "UQ__Proveedo__61264ADBB259A340").IsUnique();

            entity.Property(e => e.ProveedorPerson).HasColumnName("Proveedor_Person");
            entity.Property(e => e.ProveedorUt)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("ProveedorUT");
            entity.Property(e => e.UserCreatedName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.UserModifiedName)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(d => d.ProveedorPersonNavigation).WithMany(p => p.Proveedors)
                .HasForeignKey(d => d.ProveedorPerson)
                .HasConstraintName("FK__Proveedor__Prove__2F10007B");
        });

        modelBuilder.Entity<Ticket>(entity =>
        {
            entity.HasKey(e => e.TicketId).HasName("PK__Ticket__712CC607031AA049");

            entity.ToTable("Ticket");

            entity.Property(e => e.TicketCamion)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.TicketCamionPeso).HasColumnType("decimal(8, 3)");
            entity.Property(e => e.TicketChofer)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.TicketIngenio)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.TicketPesoBruto).HasColumnType("decimal(8, 3)");
            entity.Property(e => e.TicketTransportista)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.TicketUnidadPeso)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.TicketVehiculo)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.TicketVehiculoPeso).HasColumnType("decimal(8, 3)");
            entity.Property(e => e.TicketViaje)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.UserCreatedAt).HasColumnType("datetime");
            entity.Property(e => e.UserCreatedName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.UserModifiedAt).HasColumnType("datetime");
            entity.Property(e => e.UserModifiedName)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(d => d.TicketEstado).WithMany(p => p.Tickets)
                .HasForeignKey(d => d.TicketEstadoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Ticket__TicketEs__5EBF139D");
        });

        modelBuilder.Entity<TicketEstado>(entity =>
        {
            entity.HasKey(e => e.TicketEstadoId).HasName("PK__TicketEs__AECBF2FC6BB418D8");

            entity.ToTable("TicketEstado");

            entity.Property(e => e.TicketEstadoDescripcion)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TicketHistorial>(entity =>
        {
            entity.HasKey(e => e.HistorialId).HasName("PK__TicketHi__9752068F9F958281");

            entity.ToTable("TicketHistorial");

            entity.Property(e => e.TicketCamion)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.TicketCamionPeso).HasColumnType("decimal(8, 3)");
            entity.Property(e => e.TicketChofer)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.TicketIngenio)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.TicketPesoBruto).HasColumnType("decimal(8, 3)");
            entity.Property(e => e.TicketTransportista)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.TicketUnidadPeso)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.TicketVehiculo)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.TicketVehiculoPeso).HasColumnType("decimal(8, 3)");
            entity.Property(e => e.TicketViaje)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.UserModifiedAt).HasColumnType("datetime");
            entity.Property(e => e.UserModifiedName)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Tierra>(entity =>
        {
            entity.HasKey(e => e.TierraId).HasName("PK__Tierra__835790E56CCB9235");

            entity.ToTable("Tierra");

            entity.HasIndex(e => e.TierraUc, "UQ__Tierra__8368B4566263A9B7").IsUnique();

            entity.Property(e => e.TierraCampo)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.TierraHa)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("TierraHA");
            entity.Property(e => e.TierraSector)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.TierraUc)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("TierraUC");
            entity.Property(e => e.TierraValle)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.UserCreatedName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.UserModifiedName)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TypePerson>(entity =>
        {
            entity.HasKey(e => e.TypePesonId).HasName("PK__TypePers__48AF8CF93A302ADE");

            entity.ToTable("TypePerson");

            entity.Property(e => e.TypePesonName)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CC4C5EC9EAA3");

            entity.HasIndex(e => e.UserName, "UQ__Users__C9F28456D2A9FB79").IsUnique();

            entity.Property(e => e.UserCreatedName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.UserModifiedName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.UserName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.UserPassword)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.UserPersonId).HasColumnName("User_PersonId");

            entity.HasOne(d => d.UserPerson).WithMany(p => p.Users)
                .HasForeignKey(d => d.UserPersonId)
                .HasConstraintName("FK__Users__User_Pers__2B3F6F97");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
