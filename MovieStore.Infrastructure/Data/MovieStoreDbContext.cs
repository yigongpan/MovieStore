//add this
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Linq;
using MovieStore.Core.Entities;

namespace MovieStore.Infrastructure.Data
{
    //07/13
    //Install all the EF Core libraries using Nuget package Manager
    //Create a class that inherits from DbContext Class
    //DbContext represents your Database
    //Create a connection string which EF is gonna use to create/access the Database,should include server name, Database Name amd amu credentials
    //Create an Entity class: Genre
    //inside DbContext class, Make sure to add Entity class as a DbSet property
    //in EF we have thing called Migrations, which is used to create our Database
    //we need to add Migration commands to create the tables, database etc
    // Add-Migration MigrationName  ;  make sure migration names are meaningful
    //when running Migration commands, make sure the "Default project" selected is the one that has DbContext class
    //we need to tell MVC project that we are using DbContext
    //Make sure add reference for library that has DbContext to startup.cs, in this case MVC
    //Tell MVC project that we are using EF in startup.cs
    //Add DbContext options as contructor parameter for our DbContext
    // Make sure you have Migrations folder created, and check the created migration file
    // After Creating Migration file and verifying it we need to use update-database command

    //07/14
    //Whenever you change your model/entity always make sure you add mew Migration
    //With CF approach never change the Database directly, always change the entities using Dataanonotations or Fluent API and add new migration
    //    then update database

    //In EF we have 2 ways to create our entities and model our database using Code-First approach
    //   1.Data Annotations which is nothing but bunch of C# atrributes that we can use
    //   2.Fluent API is more syntax and more powerful and usually uses lambdas
    //    Combine both DataAnnotatios and Fluent API
    public class MovieStoreDbContext :DbContext
    {
        //multiple Dbsets, all the DbSets you create will reside in DbContext class
        // This DbSet will represent our Table based on Entity class which is Genre in this case

        public MovieStoreDbContext(DbContextOptions<MovieStoreDbContext> options) : base(options)
        {
        }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Trailer> Trailers { get; set; }
        public DbSet<MovieGenre> MovieGenres { get; set; }
        public DbSet<Cast> Casts { get; set; }
        public DbSet<MovieCast> MovieCasts { get; set; }
        public DbSet<Purchase> Purchases { get; set; }
        public DbSet<Favorite> Favorites { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<Review> Reviews { get; set; }

        //2.Fluent API
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Movie>(ConfigureMovie);
            modelBuilder.Entity<Trailer>(ConfigureTrailer);
            modelBuilder.Entity<MovieGenre>(ConfigureMovieGenre);

            modelBuilder.Entity<MovieCast>(ConfigureMovieCast);
            modelBuilder.Entity<Purchase>(ConfigurePurchase);
            modelBuilder.Entity<User>(ConfigureUser);
            modelBuilder.Entity<UserRole>(ConfigureUserRole);
            modelBuilder.Entity<Review>(ConfigureReview);
        }

        private void ConfigureReview(EntityTypeBuilder<Review> modelBuilder)
        {
            modelBuilder.ToTable("Review");
            modelBuilder.HasKey(rw => new { rw.MovieId, rw.UserId });
            modelBuilder.HasOne(rw => rw.Movie).WithMany(m => m.Reviews).HasForeignKey(rw => rw.MovieId);
            modelBuilder.HasOne(rw => rw.User).WithMany(u => u.Reviews).HasForeignKey(rw => rw.UserId);
            modelBuilder.Property(rw => rw.Rating).HasColumnType("decimal(3,2)");
            //------->  how to set Max Length??????
            //modelBuilder.Property(rw => rw.ReviewText).HasMaxLength(256);
        }

        private void ConfigureUserRole(EntityTypeBuilder<UserRole> modelBuilder)
        {
            modelBuilder.ToTable("UserRole");
            modelBuilder.HasKey(ur => new { ur.UserId, ur.RoleId });
            modelBuilder.HasOne(ur => ur.Role).WithMany(r => r.UserRoles).HasForeignKey(ur => ur.RoleId);
            modelBuilder.HasOne(ur => ur.User).WithMany(u => u.UserRoles).HasForeignKey(ur => ur.UserId);
        }

        private void ConfigureUser(EntityTypeBuilder<User> modelBuilder)
        {
            modelBuilder.ToTable("User");
            modelBuilder.HasKey(u => u.Id);
            modelBuilder.Property(u => u.FirstName).HasMaxLength(128);
            modelBuilder.Property(u => u.LastName).HasMaxLength(128);
            modelBuilder.Property(u => u.DateOfBirth).HasColumnType("datetime2(7)");
            modelBuilder.Property(u => u.Email).HasMaxLength(256);
            modelBuilder.Property(u => u.HashedPassword).HasMaxLength(256);
            modelBuilder.Property(u => u.Salt).HasMaxLength(256);
            modelBuilder.Property(u => u.PhoneNumber).HasMaxLength(256);
            modelBuilder.Property(u => u.TwoFactorEnabled).HasColumnType("bit");
            modelBuilder.Property(u => u.LockoutEndDate).HasColumnType("datetime2(7)");
            modelBuilder.Property(u => u.LastLoginDateTime).HasColumnType("datetime2(7)");
            modelBuilder.Property(u => u.IsLocked).HasColumnType("bit");
            modelBuilder.Property(u => u.AccessFailedCount).HasColumnType("int");
        }

        private void ConfigurePurchase(EntityTypeBuilder<Purchase> modelBuilder)
        {
            modelBuilder.ToTable("Purchase");
            modelBuilder.HasKey(p => p.Id);
            modelBuilder.Property(p => p.UserId);
            modelBuilder.HasIndex(p => p.PurchaseNumber).IsUnique();
            modelBuilder.Property(p => p.TotalPrice).HasColumnType("decimal(5, 2)");
            modelBuilder.Property(p => p.PurchaseDateTime).HasColumnType("datetime2(7)");
            //modelBuilder.HasForeignKey()

        }

        private void ConfigureMovieCast(EntityTypeBuilder<MovieCast> modelBuilder)
        {
            modelBuilder.ToTable("MovieCast");
            modelBuilder.HasKey(mc => new { mc.MovieId, mc.CastId,mc.Character});
            modelBuilder.HasOne(mc => mc.Movie).WithMany(g => g.MovieCasts).HasForeignKey(mg => mg.MovieId);
            modelBuilder.HasOne(mc => mc.Cast).WithMany(g => g.MovieCasts).HasForeignKey(mg => mg.CastId);
            modelBuilder.Property(mc => mc.Character).HasMaxLength(450);

        }

        private void ConfigureMovieGenre(EntityTypeBuilder<MovieGenre> modelBuilder)
        {
            modelBuilder.ToTable("MovieGenre");
            modelBuilder.HasKey(mg => new { mg.MovieId, mg.GenreId });
            modelBuilder.HasOne(mg => mg.Movie).WithMany(g => g.MovieGenres).HasForeignKey(mg => mg.MovieId);
            modelBuilder.HasOne(mg => mg.Genre).WithMany(g => g.MovieGenres).HasForeignKey(mg => mg.GenreId);
            
        }

        private void ConfigureTrailer(EntityTypeBuilder<Trailer> modelBuilder)
        {
            modelBuilder.ToTable("Trailer");
            modelBuilder.HasKey(t => t.Id);
            modelBuilder.Property(t => t.Name).HasMaxLength(2084);
            modelBuilder.Property(t => t.TrailerUrl).HasMaxLength(2084);
        }

        private void ConfigureMovie(EntityTypeBuilder<Movie> modelBuilder)
        {
            //we can use Fluent API configurations to model our tables
            modelBuilder.ToTable("Movie");    //set table name as Movie
            modelBuilder.HasKey(m => m.Id);   //set Id as primary key
            modelBuilder.Property(m => m.Title).IsRequired().HasMaxLength(256);    //set not null and Maxlength
            modelBuilder.Property(m => m.Overview).HasMaxLength(4096);
            modelBuilder.Property(m => m.Tagline).HasMaxLength(512);
            modelBuilder.Property(m => m.ImdbUrl).HasMaxLength(2084);
            modelBuilder.Property(m => m.TmdbUrl).HasMaxLength(2084);
            modelBuilder.Property(m => m.PosterUrl).HasMaxLength(2084);
            modelBuilder.Property(m => m.BackdropUrl).HasMaxLength(2084);
            modelBuilder.Property(m => m.OriginalLanguage).HasMaxLength(64);
            modelBuilder.Property(m => m.Price).HasColumnType("decimal(5, 2)").HasDefaultValue(9.9m);
            modelBuilder.Property(m => m.CreatedDate).HasDefaultValueSql("getdate()");

            //we don't want to create Rating Column but want C# rating property in our Entity so we can show Movie rating in the UI
            modelBuilder.Ignore(m => m.Rating);

        }

        //07/15
        /* 
      IEnumerable<T> VS IQueryable<T>      
       -all collection (like regular Lists,Dictionaries,..) implements IEnumerable, so LINQ methos will point to IEnumerable extension
	  (which work with in-memory source);
       -Entities (like DbSets,database,SQL server) normally implement IQuerable, they point to IQuerable methods 
	  (which usually work with out-memory source)

      In LINQ, WHERE() method is extended by both IEnumerable and IQueryable; one of the extension method will be used depends on the source
         */
        /*public void Test()
        {
            //in memory source
            var ll = new List<int>();
            //Where() extension method points to IEnumerable
            //parameter is Func<TSource, bool> predicate
            ll.Where(x => x > 3);
            //out memory source
            //Where() extension method points to IQueryable
            //parameter is Expression<Func<TSource, bool>> predicate
            Genres.Where(global => global.Id == 2);
        }
        */
    }
}
