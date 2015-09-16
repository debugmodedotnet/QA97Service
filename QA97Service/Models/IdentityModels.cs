using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System.Data.Entity;
using QA97Service.Entities;
using System.Collections.Generic;

namespace QA97Service.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser()
        {
            Answers = new HashSet<Answer>();
            AnswerVotes = new HashSet<AnswerVote>();
            Questions = new HashSet<Question>();
            QuestionVotes = new HashSet<QuestionVote>();
            UserImages = new HashSet<UserImage>();
            //UserPoints = new HashSet<UserPoint>();
            UserImages = new HashSet<UserImage>();
            Comments = new HashSet<Comment>();
          
        }
        public virtual ICollection<Answer> Answers { get; set; }

        public virtual ICollection<AnswerVote> AnswerVotes { get; set; }

        public virtual ICollection<Question> Questions { get; set; }

        public virtual ICollection<QuestionVote> QuestionVotes { get; set; }

        public virtual ICollection<UserImage> UserImages { get; set; }
        public virtual UserPoint UserPoint { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }

        public int? SchoolId { get; set; }
        public virtual School School { get; set; }

        public string CityName { get; set; }
        public string FullName { get; set; }
  
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager, string authenticationType)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("name=QA97connectionstring")
        {
        }

        public virtual DbSet<Answer> Answers { get; set; }
        public virtual DbSet<AnswerVote> AnswerVotes { get; set; }
        public virtual DbSet<Class> Classes { get; set; }
        public virtual DbSet<Question> Questions { get; set; }
        public virtual DbSet<QuestionVote> QuestionVotes { get; set; }

        public virtual DbSet<School> Schools { get; set; }
        public virtual DbSet<Subject> Subjects { get; set; }

        public virtual DbSet<UserPoint> UserPoints { get; set; }

        public virtual DbSet<UserImage> UserImages { get; set; }

        public virtual DbSet<Comment> Comments { get; set; }

        public virtual DbSet<City> Cities { get; set; }


        //public virtual ICollection<UserImage> UserImages { get; set; }
        //public virtual DbSet<User> Users { get; set; }
        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Answer>()
               .HasMany(e => e.AnswerVotes)
               .WithRequired(e => e.Answer)
               .WillCascadeOnDelete(false);

            modelBuilder.Entity<Class>()
                .HasMany(e => e.Questions)
                .WithRequired(e => e.Class)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Question>()
                .HasMany(e => e.Answers)
                .WithRequired(e => e.Question)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Question>()
                .HasMany(e => e.QuestionVotes)
                .WithRequired(e => e.Question)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Subject>()
                .HasMany(e => e.Questions)
                .WithRequired(e => e.Subject)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ApplicationUser>()
                .HasMany(e => e.Answers)
                .WithRequired(e => e.User)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ApplicationUser>()
                .HasMany(e => e.AnswerVotes)
                .WithRequired(e => e.User)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ApplicationUser>()
                .HasMany(e => e.Questions)
                .WithRequired(e => e.User)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ApplicationUser>()
                .HasMany(e => e.QuestionVotes)
                .WithRequired(e => e.User)
                .WillCascadeOnDelete(false);
        }

        
    }
}