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

    public virtual DbSet<Action> Actions { get; set; }

    public virtual DbSet<AsignarTierra> AsignarTierras { get; set; }

    public virtual DbSet<AsignarTierraHistorial> AsignarTierraHistorials { get; set; }

    public virtual DbSet<Carguillo> Carguillos { get; set; }

    public virtual DbSet<CarguilloDetalle> CarguilloDetalles { get; set; }

    public virtual DbSet<CarguilloTipo> CarguilloTipos { get; set; }

    public virtual DbSet<Corte> Cortes { get; set; }

    public virtual DbSet<CorteDetalle> CorteDetalles { get; set; }

    public virtual DbSet<CorteEstado> CorteEstados { get; set; }

    public virtual DbSet<CorteHistorial> CorteHistorials { get; set; }

    public virtual DbSet<Cosecha> Cosechas { get; set; }

    public virtual DbSet<CosechaTipo> CosechaTipos { get; set; }

    public virtual DbSet<HistorialRefreshToken> HistorialRefreshTokens { get; set; }

    public virtual DbSet<Liquidacion> Liquidacions { get; set; }

    public virtual DbSet<LiquidacionAdicional> LiquidacionAdicionals { get; set; }

    public virtual DbSet<LiquidacionEstado> LiquidacionEstados { get; set; }

    public virtual DbSet<LiquidacionFinanciamiento> LiquidacionFinanciamientos { get; set; }

    public virtual DbSet<LiquidacionTicket> LiquidacionTickets { get; set; }

    public virtual DbSet<Module> Modules { get; set; }

    public virtual DbSet<Person> Persons { get; set; }

    public virtual DbSet<Proveedor> Proveedors { get; set; }

    public virtual DbSet<ProveedorPerson> ProveedorPeople { get; set; }

    public virtual DbSet<Recojo> Recojos { get; set; }

    public virtual DbSet<RecojoEstado> RecojoEstados { get; set; }

    public virtual DbSet<ServicioTransporte> ServicioTransportes { get; set; }

    public virtual DbSet<ServicioTransporteDetalle> ServicioTransporteDetalles { get; set; }

    public virtual DbSet<ServicioTransporteEstado> ServicioTransporteEstados { get; set; }

    public virtual DbSet<Tesorerium> Tesoreria { get; set; }

    public virtual DbSet<Ticket> Tickets { get; set; }

    public virtual DbSet<TicketEstado> TicketEstados { get; set; }

    public virtual DbSet<TicketHistorial> TicketHistorials { get; set; }

    public virtual DbSet<Tierra> Tierras { get; set; }

    public virtual DbSet<TypePerson> TypePeople { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserPermission> UserPermissions { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Action>(entity =>
        {
            entity.HasKey(e => e.ActionId).HasName("PK__Actions__FFE3F4D9B2F3287F");

            entity.Property(e => e.ActionName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UserCreatedAt).HasColumnType("datetime");
            entity.Property(e => e.UserCreatedName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.UserModifiedAt).HasColumnType("datetime");
            entity.Property(e => e.UserModifiedName)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(d => d.Module).WithMany(p => p.Actions)
                .HasForeignKey(d => d.ModuleId)
                .HasConstraintName("FK__Actions__ModuleI__79FD19BE");
        });

        modelBuilder.Entity<AsignarTierra>(entity =>
        {
            entity.HasKey(e => e.AsignarTierraId).HasName("PK__AsignarT__9F4B1AF6331C27AD");

            entity.ToTable("AsignarTierra");

            entity.Property(e => e.AsignarTierraProveedor).HasColumnName("AsignarTierra_Proveedor");
            entity.Property(e => e.AsignarTierraTierra).HasColumnName("AsignarTierra_Tierra");
            entity.Property(e => e.UserCreatedAt).HasColumnType("datetime");
            entity.Property(e => e.UserCreatedName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.UserModifiedAt).HasColumnType("datetime");
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

        modelBuilder.Entity<Carguillo>(entity =>
        {
            entity.HasKey(e => e.CarguilloId).HasName("PK__Carguill__A3EF0B6DB7B6B83E");

            entity.ToTable("Carguillo");

            entity.Property(e => e.CarguilloTitular)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.UserCreatedAt).HasColumnType("datetime");
            entity.Property(e => e.UserCreatedName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.UserModifiedAt).HasColumnType("datetime");
            entity.Property(e => e.UserModifiedName)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(d => d.CarguilloTipo).WithMany(p => p.Carguillos)
                .HasForeignKey(d => d.CarguilloTipoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Carguillo__Cargu__236943A5");
        });

        modelBuilder.Entity<CarguilloDetalle>(entity =>
        {
            entity.HasKey(e => e.CarguilloDetalleId).HasName("PK__Carguill__210C0FF1FBD456EB");

            entity.ToTable("CarguilloDetalle");

            entity.Property(e => e.CarguilloDetallePlaca)
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

            entity.HasOne(d => d.Carguillo).WithMany(p => p.CarguilloDetalles)
                .HasForeignKey(d => d.CarguilloId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Carguillo__Cargu__2A164134");

            entity.HasOne(d => d.CarguilloTipo).WithMany(p => p.CarguilloDetalles)
                .HasForeignKey(d => d.CarguilloTipoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Carguillo__Cargu__2B0A656D");
        });

        modelBuilder.Entity<CarguilloTipo>(entity =>
        {
            entity.HasKey(e => e.CarguilloTipoId).HasName("PK__Carguill__CAB54414CB52BA9E");

            entity.ToTable("CarguilloTipo");

            entity.Property(e => e.CarguilloTipoDescripcion)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.IsCarguillo).HasColumnName("isCarguillo");
        });

        modelBuilder.Entity<Corte>(entity =>
        {
            entity.HasKey(e => e.CorteId).HasName("PK__Corte__90507851CD85BA0B");

            entity.ToTable("Corte");

            entity.Property(e => e.CortePesoBrutoTotal).HasColumnType("decimal(8, 3)");
            entity.Property(e => e.CortePrecio).HasColumnType("decimal(8, 3)");
            entity.Property(e => e.CorteTotal).HasColumnType("decimal(8, 3)");
            entity.Property(e => e.UserCreatedAt).HasColumnType("datetime");
            entity.Property(e => e.UserCreatedName)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(d => d.CorteEstado).WithMany(p => p.Cortes)
                .HasForeignKey(d => d.CorteEstadoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Corte__CorteEsta__72C60C4A");

            entity.HasOne(d => d.Tierra).WithMany(p => p.Cortes)
                .HasForeignKey(d => d.TierraId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Corte__TierraId__71D1E811");
        });

        modelBuilder.Entity<CorteDetalle>(entity =>
        {
            entity.HasKey(e => e.CorteDetalleId).HasName("PK__CorteDet__6C90B4D01CAA1121");

            entity.ToTable("CorteDetalle");

            entity.Property(e => e.UserCreatedAt).HasColumnType("datetime");
            entity.Property(e => e.UserCreatedName)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(d => d.Corte).WithMany(p => p.CorteDetalles)
                .HasForeignKey(d => d.CorteId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CorteDeta__Corte__3587F3E0");

            entity.HasOne(d => d.Ticket).WithMany(p => p.CorteDetalles)
                .HasForeignKey(d => d.TicketId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CorteDeta__Ticke__367C1819");
        });

        modelBuilder.Entity<CorteEstado>(entity =>
        {
            entity.HasKey(e => e.CorteEstadoId).HasName("PK__CorteEst__FD51CACD906DD953");

            entity.ToTable("CorteEstado");

            entity.Property(e => e.CorteEstadoDescripcion)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<CorteHistorial>(entity =>
        {
            entity.HasKey(e => e.CorteHistorialId).HasName("PK__CorteHis__011FF964B69E64B6");

            entity.ToTable("CorteHistorial");

            entity.Property(e => e.CarguilloPrecio).HasColumnType("decimal(8, 3)");
            entity.Property(e => e.CortePesoBrutoTotal).HasColumnType("decimal(8, 3)");
            entity.Property(e => e.CortePrecio).HasColumnType("decimal(8, 3)");
            entity.Property(e => e.CorteTotal).HasColumnType("decimal(8, 3)");
            entity.Property(e => e.UserModifiedAt).HasColumnType("datetime");
            entity.Property(e => e.UserModifiedName)
                .HasMaxLength(100)
                .IsUnicode(false);
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

        modelBuilder.Entity<HistorialRefreshToken>(entity =>
        {
            entity.HasKey(e => e.HistorialTokenId).HasName("PK__Historia__03DC48A58F302C9A");

            entity.ToTable("HistorialRefreshToken");

            entity.Property(e => e.EsActivo).HasComputedColumnSql("(case when [FechaExpiracion]<getdate() then CONVERT([bit],(0)) else CONVERT([bit],(1)) end)", false);
            entity.Property(e => e.FechaCreacion).HasColumnType("datetime");
            entity.Property(e => e.FechaExpiracion).HasColumnType("datetime");
            entity.Property(e => e.RefreshToken)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Token).IsUnicode(false);

            entity.HasOne(d => d.User).WithMany(p => p.HistorialRefreshTokens)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Historial__IdUsu__267ABA7A");
        });

        modelBuilder.Entity<Liquidacion>(entity =>
        {
            entity.HasKey(e => e.LiquidacionId).HasName("PK__Liquidac__94C125CC17387160");

            entity.ToTable("Liquidacion");

            entity.Property(e => e.LiquidacionAdicionalTotal).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.LiquidacionFinanciamientoAcuenta)
                .HasColumnType("decimal(16, 2)")
                .HasColumnName("LiquidacionFinanciamientoACuenta");
            entity.Property(e => e.LiquidacionPagar).HasColumnType("decimal(16, 2)");
            entity.Property(e => e.LiquidacionPesoBruto).HasColumnType("decimal(8, 3)");
            entity.Property(e => e.LiquidacionPesoNeto).HasColumnType("decimal(8, 3)");
            entity.Property(e => e.LiquidacionToneladaPrecioCompra).HasColumnType("decimal(8, 2)");
            entity.Property(e => e.LiquidacionToneladaTotal).HasColumnType("decimal(16, 2)");
            entity.Property(e => e.UserCreatedAt).HasColumnType("datetime");
            entity.Property(e => e.UserCreatedName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.UserModifiedAt).HasColumnType("datetime");
            entity.Property(e => e.UserModifiedName)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(d => d.LiquidacionEstado).WithMany(p => p.Liquidacions)
                .HasForeignKey(d => d.LiquidacionEstadoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Liquidaci__Liqui__05A3D694");

            entity.HasOne(d => d.Persona).WithMany(p => p.Liquidacions)
                .HasForeignKey(d => d.PersonaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Liquidacion_Persons");

            entity.HasOne(d => d.Proveedor).WithMany(p => p.Liquidacions)
                .HasForeignKey(d => d.ProveedorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Liquidaci__Prove__04AFB25B");

            entity.HasOne(d => d.Tierra).WithMany(p => p.Liquidacions)
                .HasForeignKey(d => d.TierraId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Liquidaci__Tierr__03BB8E22");
        });

        modelBuilder.Entity<LiquidacionAdicional>(entity =>
        {
            entity.HasKey(e => e.LiquidacionAdicionalId).HasName("PK__Liquidac__A9126BEF38C0D31C");

            entity.ToTable("LiquidacionAdicional");

            entity.Property(e => e.LiquidacionAdicionalMotivo)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.LiquidacionAdicionalTotal).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.UserCreatedAt).HasColumnType("datetime");
            entity.Property(e => e.UserCreatedName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.UserModifiedAt).HasColumnType("datetime");
            entity.Property(e => e.UserModifiedName)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(d => d.Liquidacion).WithMany(p => p.LiquidacionAdicionals)
                .HasForeignKey(d => d.LiquidacionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Liquidaci__UserM__74444068");
        });

        modelBuilder.Entity<LiquidacionEstado>(entity =>
        {
            entity.HasKey(e => e.LiquidacionEstadoId).HasName("PK__Liquidac__B30B5044048AC30C");

            entity.ToTable("LiquidacionEstado");

            entity.Property(e => e.LiquidacionEstadoDescripcion)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<LiquidacionFinanciamiento>(entity =>
        {
            entity.HasKey(e => e.LiquidacionFinanciamientoId).HasName("PK__Liquidac__230AD20CC56ECA24");

            entity.ToTable("LiquidacionFinanciamiento");

            entity.Property(e => e.LiquidacionFinanciamientoAcuenta)
                .HasColumnType("decimal(8, 2)")
                .HasColumnName("LiquidacionFinanciamientoACuenta");
            entity.Property(e => e.LiquidacionFinanciamientoInteres).HasColumnType("decimal(8, 2)");
            entity.Property(e => e.LiquidacionFinanciamientoInteresMes).HasColumnType("decimal(8, 2)");
            entity.Property(e => e.LiquidacionFinanciamientoTotal).HasColumnType("decimal(16, 2)");
            entity.Property(e => e.UserCreatedAt).HasColumnType("datetime");
            entity.Property(e => e.UserCreatedName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.UserModifiedAt).HasColumnType("datetime");
            entity.Property(e => e.UserModifiedName)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(d => d.Liquidacion).WithMany(p => p.LiquidacionFinanciamientos)
                .HasForeignKey(d => d.LiquidacionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Liquidaci__Liqui__0880433F");
        });

        modelBuilder.Entity<LiquidacionTicket>(entity =>
        {
            entity.HasKey(e => e.LiquidacionTicketId).HasName("PK__Liquidac__B6F4A008F1CC7051");

            entity.ToTable("LiquidacionTicket");

            entity.Property(e => e.UserCreatedAt).HasColumnType("datetime");
            entity.Property(e => e.UserCreatedName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.UserModifiedAt).HasColumnType("datetime");
            entity.Property(e => e.UserModifiedName)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(d => d.Liquidacion).WithMany(p => p.LiquidacionTickets)
                .HasForeignKey(d => d.LiquidacionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Liquidaci__Liqui__7073AF84");

            entity.HasOne(d => d.Ticket).WithMany(p => p.LiquidacionTickets)
                .HasForeignKey(d => d.TicketId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Liquidaci__Ticke__7167D3BD");
        });

        modelBuilder.Entity<Module>(entity =>
        {
            entity.HasKey(e => e.ModuleId).HasName("PK__Module__2B7477A73223D99F");

            entity.ToTable("Module");

            entity.Property(e => e.ModuleColor)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ModuleIcon)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ModuleName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ModuleRoute)
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

            entity.HasOne(d => d.ModulePrimary).WithMany(p => p.InverseModulePrimary)
                .HasForeignKey(d => d.ModulePrimaryId)
                .HasConstraintName("FK_Module_Module");
        });

        modelBuilder.Entity<Person>(entity =>
        {
            entity.HasKey(e => e.PersonId).HasName("PK__Persons__AA2FFBE5D74AEBF7");

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
            entity.Property(e => e.UserCreatedAt).HasColumnType("datetime");
            entity.Property(e => e.UserCreatedName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.UserModifiedAt).HasColumnType("datetime");
            entity.Property(e => e.UserModifiedName)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(d => d.PersonTypeNavigation).WithMany(p => p.People)
                .HasForeignKey(d => d.PersonType)
                .HasConstraintName("FK__Persons__Person___276EDEB3");
        });

        modelBuilder.Entity<Proveedor>(entity =>
        {
            entity.HasKey(e => e.ProveedorId).HasName("PK__Proveedo__61266A597EE06669");

            entity.ToTable("Proveedor");

            entity.HasIndex(e => e.ProveedorUt, "UQ__Proveedo__61264ADBB259A340").IsUnique();

            entity.Property(e => e.ProveedorUt)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("ProveedorUT");
            entity.Property(e => e.UserCreatedAt).HasColumnType("datetime");
            entity.Property(e => e.UserCreatedName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.UserModifiedAt).HasColumnType("datetime");
            entity.Property(e => e.UserModifiedName)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<ProveedorPerson>(entity =>
        {
            entity.HasKey(e => e.ProveedorPersonId).HasName("PK__Proveedo__E9244A43F3BD74B6");

            entity.ToTable("ProveedorPerson");

            entity.Property(e => e.UserCreatedAt).HasColumnType("datetime");
            entity.Property(e => e.UserCreatedName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.UserModifiedAt).HasColumnType("datetime");
            entity.Property(e => e.UserModifiedName)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(d => d.Person).WithMany(p => p.ProveedorPeople)
                .HasForeignKey(d => d.PersonId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Proveedor__Perso__2EA5EC27");

            entity.HasOne(d => d.Proveedor).WithMany(p => p.ProveedorPeople)
                .HasForeignKey(d => d.ProveedorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Proveedor__Prove__2F9A1060");
        });

        modelBuilder.Entity<Recojo>(entity =>
        {
            entity.HasKey(e => e.RecojoId).HasName("PK__Recojo__C0038E371163959D");

            entity.ToTable("Recojo");

            entity.Property(e => e.RecojoCamionesPrecio).HasColumnType("decimal(8, 2)");
            entity.Property(e => e.RecojoCampo)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.RecojoDiasPrecio).HasColumnType("decimal(8, 2)");
            entity.Property(e => e.RecojoTotalPrecio).HasColumnType("decimal(8, 2)");
            entity.Property(e => e.UserCreatedAt).HasColumnType("datetime");
            entity.Property(e => e.UserCreatedName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.UserModifiedAt).HasColumnType("datetime");
            entity.Property(e => e.UserModifiedName)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(d => d.RecojoEstado).WithMany(p => p.Recojos)
                .HasForeignKey(d => d.RecojoEstadoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Recojo__RecojoEs__51300E55");
        });

        modelBuilder.Entity<RecojoEstado>(entity =>
        {
            entity.HasKey(e => e.RecojoEstadoId).HasName("PK__RecojoEs__364844B583E0E031");

            entity.ToTable("RecojoEstado");

            entity.Property(e => e.RecojoEstadoDescripcion)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<ServicioTransporte>(entity =>
        {
            entity.HasKey(e => e.ServicioTransporteId).HasName("PK__Servicio__720345606DD63230");

            entity.ToTable("ServicioTransporte");

            entity.Property(e => e.CarguilloPaleroPrecio).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.ServicioTransportePrecio).HasColumnType("decimal(8, 2)");
            entity.Property(e => e.ServicioTransporteTotal).HasColumnType("decimal(8, 2)");
            entity.Property(e => e.UserCreatedAt).HasColumnType("datetime");
            entity.Property(e => e.UserCreatedName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.UserModifiedAt).HasColumnType("datetime");
            entity.Property(e => e.UserModifiedName)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(d => d.Carguillo).WithMany(p => p.ServicioTransporteCarguillos)
                .HasForeignKey(d => d.CarguilloId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ServicioT__Cargu__6442E2C9");

            entity.HasOne(d => d.CarguilloIdPaleroNavigation).WithMany(p => p.ServicioTransporteCarguilloIdPaleroNavigations)
                .HasForeignKey(d => d.CarguilloIdPalero)
                .HasConstraintName("FK_ServicioTransporte_Carguillo");

            entity.HasOne(d => d.ServicioTransporteEstado).WithMany(p => p.ServicioTransportes)
                .HasForeignKey(d => d.ServicioTransporteEstadoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ServicioT__Servi__65370702");
        });

        modelBuilder.Entity<ServicioTransporteDetalle>(entity =>
        {
            entity.HasKey(e => e.ServicioTransporteDetalleId).HasName("PK__Servicio__6D9351E6348C6BF5");

            entity.ToTable("ServicioTransporteDetalle");

            entity.Property(e => e.UserCreatedAt).HasColumnType("datetime");
            entity.Property(e => e.UserCreatedName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.UserModifiedAt).HasColumnType("datetime");
            entity.Property(e => e.UserModifiedName)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(d => d.ServicioTransporte).WithMany(p => p.ServicioTransporteDetalles)
                .HasForeignKey(d => d.ServicioTransporteId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ServicioT__Servi__13F1F5EB");

            entity.HasOne(d => d.Ticket).WithMany(p => p.ServicioTransporteDetalles)
                .HasForeignKey(d => d.TicketId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ServicioT__Ticke__14E61A24");
        });

        modelBuilder.Entity<ServicioTransporteEstado>(entity =>
        {
            entity.HasKey(e => e.ServicioTransporteEstadoId).HasName("PK__Servicio__D7262336A0B0792B");

            entity.ToTable("ServicioTransporteEstado");

            entity.Property(e => e.ServicioTransporteEstadoDescripcion)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Tesorerium>(entity =>
        {
            entity.HasKey(e => e.TesoreriaId).HasName("PK__Tesoreri__93C71F9F7C4496C3");

            entity.Property(e => e.TesoreriaBanco)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.TesoreriaCtaCte)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.TesoreriaFecha).HasColumnType("datetime");
            entity.Property(e => e.TesoreriaMonto).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.UserCreatedAt).HasColumnType("datetime");
            entity.Property(e => e.UserCreatedName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.UserModifiedAt).HasColumnType("datetime");
            entity.Property(e => e.UserModifiedName)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(d => d.Liquidacion).WithMany(p => p.Tesoreria)
                .HasForeignKey(d => d.LiquidacionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Tesoreria__UserM__473C8FC7");
        });

        modelBuilder.Entity<Ticket>(entity =>
        {
            entity.HasKey(e => e.TicketId).HasName("PK__Ticket__712CC607316DBA75");

            entity.ToTable("Ticket");

            entity.Property(e => e.TicketCamionPeso).HasColumnType("decimal(8, 3)");
            entity.Property(e => e.TicketCampo)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.TicketChofer)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.TicketIngenio)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.TicketPesoBruto).HasColumnType("decimal(8, 3)");
            entity.Property(e => e.TicketUnidadPeso)
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

            entity.HasOne(d => d.CarguilloDetalleCamion).WithMany(p => p.TicketCarguilloDetalleCamions)
                .HasForeignKey(d => d.CarguilloDetalleCamionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Ticket__Carguill__2FCF1A8A");

            entity.HasOne(d => d.CarguilloDetalleVehiculo).WithMany(p => p.TicketCarguilloDetalleVehiculos)
                .HasForeignKey(d => d.CarguilloDetalleVehiculoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Ticket__Carguill__30C33EC3");

            entity.HasOne(d => d.Carguillo).WithMany(p => p.Tickets)
                .HasForeignKey(d => d.CarguilloId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Ticket__Carguill__2EDAF651");

            entity.HasOne(d => d.TicketEstado).WithMany(p => p.Tickets)
                .HasForeignKey(d => d.TicketEstadoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Ticket__TicketEs__2DE6D218");
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
            entity.HasKey(e => e.HistorialId).HasName("PK__TicketHi__9752068F6B483D76");

            entity.ToTable("TicketHistorial");

            entity.Property(e => e.TicketCamionPeso).HasColumnType("decimal(8, 3)");
            entity.Property(e => e.TicketCampo)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.TicketChofer)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.TicketIngenio)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.TicketPesoBruto).HasColumnType("decimal(8, 3)");
            entity.Property(e => e.TicketUnidadPeso)
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
            entity.Property(e => e.UserCreatedAt).HasColumnType("datetime");
            entity.Property(e => e.UserCreatedName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.UserModifiedAt).HasColumnType("datetime");
            entity.Property(e => e.UserModifiedName)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CC4C5EC9EAA3");

            entity.HasIndex(e => e.UserName, "UQ__Users__C9F28456D2A9FB79").IsUnique();

            entity.Property(e => e.UserCreatedAt).HasColumnType("datetime");
            entity.Property(e => e.UserCreatedName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.UserModifiedAt).HasColumnType("datetime");
            entity.Property(e => e.UserModifiedName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.UserName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.UserPersonId).HasColumnName("User_PersonId");
            entity.Property(e => e.VerificarToken)
                .HasMaxLength(500)
                .IsUnicode(false);

            entity.HasOne(d => d.UserPerson).WithMany(p => p.Users)
                .HasForeignKey(d => d.UserPersonId)
                .HasConstraintName("FK__Users__User_Pers__2B3F6F97");
        });

        modelBuilder.Entity<UserPermission>(entity =>
        {
            entity.HasKey(e => e.UserPermissionId).HasName("PK__UserPerm__A90F88B204006614");

            entity.ToTable("UserPermission");

            entity.Property(e => e.UserCreatedAt).HasColumnType("datetime");
            entity.Property(e => e.UserCreatedName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.UserModifiedAt).HasColumnType("datetime");
            entity.Property(e => e.UserModifiedName)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(d => d.Module).WithMany(p => p.UserPermissions)
                .HasForeignKey(d => d.ModuleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__UserPermi__Modul__7DCDAAA2");

            entity.HasOne(d => d.User).WithMany(p => p.UserPermissions)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__UserPermi__UserI__7CD98669");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
