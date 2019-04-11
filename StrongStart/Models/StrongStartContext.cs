using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StrongStart.Models;

namespace StrongStart.Models
{
    public class StrongStartContext:DbContext
    {
        public StrongStartContext(DbContextOptions<StrongStartContext> options)
            : base(options)
        { }

        public DbSet<Region> Regions { get; set; }
        public DbSet<Training_Volunteer> Training_Volunteers { get; set; }
        public DbSet<Site> Sites { get; set; }
        public DbSet<Term> Terms { get; set; }
        public DbSet<Trainer> Trainers { get; set; }
        public DbSet<Training> Trainings { get; set; }
        public DbSet<Volunteer> Volunteers { get; set; }
        //public DbSet<Training_part> Training_parts { get; set; }
        public DbSet<Training_Trainer> Training_Trainers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Site>()
               .HasOne(s => s.region)
               .WithMany(r => r.Sites)
               .HasForeignKey(s => s.regionID)
               .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Training>()
                .HasOne(ti => ti.term)
                .WithMany(t => t.Trainings)
                .HasForeignKey(ti => ti.termID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Region>().HasData(
                new Region() { regionID = 1, regionCode = "WR", regionName = "Waterloo Region" },
                new Region() { regionID = 2, regionCode = "WL", regionName = "Wellington Region" },
                new Region() { regionID = 3, regionCode = "PR", regionName = "Perth Region" }
                );

            modelBuilder.Entity<Site>().HasData(
                new Site() { siteID=1, siteName= "Paisley Road Public School", Address= "406 Paisley Rd", Phone="5198220675",City= "Guelph", Province="ON",PostalCode= "N1H 2R3", regionID=2},
                new Site() { siteID = 2, siteName = "Conestoga College", Address = "299 Doon Valley", Phone = "987456213", City = "Kitchener", Province = "ON", PostalCode = "N2G 4M4", regionID = 1},
                new Site() { siteID = 3, siteName = "Centre Wellington District High School", Address = "905 Scotland St", Phone = "519843-2500", City = "Fergus", Province = "ON", PostalCode = "N1M 1Y7", regionID = 2 },
                new Site() { siteID = 4, siteName = "Stratford Central Secondary School", Address = "60 St Andrew St", Phone = "5192714500", City = "Stratford", Province = "ON", PostalCode = "N5A 1A3", regionID = 3 }

                );
            modelBuilder.Entity<Trainer>().HasData(
                new Trainer() { trainerID = 1, Title = Title.Trainer, firstName = "qiao", lastName = "wang", Address = "5 Rittenhouse Rd", Phone = "123456789", City = "Waterloo", Province = "ON", Country = "Canada", regionID = 1,Email="wangqiao921105@gmail.com" },
                new Trainer() { trainerID = 2, Title = Title.Trainee, firstName = "mao", lastName = "qiu", Address = "10 Rittenhouse Rd", Phone = "123456789", City = "Guelph", Province = "ON", Country = "Canada", regionID = 2,Email="1@1.com" },
                new Trainer() { trainerID = 3, Title = Title.Trainee, firstName = "Matthew", lastName = "Cioanca", Address = "10 macro Rd", Phone = "123456789", City = "Kitchener", Province = "ON", Country = "Canada", regionID = 2, Email = "Matthewgeorgecioanca.com" }
                );
            modelBuilder.Entity<Term>().HasData(
                new Term() { termID = 1, termName="F2019" },
                new Term() { termID = 2, termName = "S2019" },
                new Term() { termID = 3, termName = "F2020" }
                );
            modelBuilder.Entity<Training>().HasData(
               new Training()
               {
                   trainingID = 1,
                   siteID = 2,
                   termID = 1,
                   startTime = new DateTime(2019, 10, 5, 10, 0, 0),
                   endTime = new DateTime(2019, 10, 5, 12, 0, 0),
                   Date = new DateTime(2019, 10, 5),
                   permit = YesNO.Yes,
                   specInstructions = null,
                   part = Part.part1,
                   training_Progress_Status = Training_Progress_Status.Created,
                   Capacity = 30,
                   training_Status = Training_Status.Active
               },
                new Training()
                {
                    trainingID = 2,
                    siteID = 1,
                    termID = 2,
                    startTime = new DateTime(2019, 10, 12, 10, 0, 0),
                    endTime = new DateTime(2019, 10, 12, 12, 0, 0),
                    Date = new DateTime(2019, 10, 12),
                    permit = YesNO.Yes,
                    specInstructions = null,
                    part = Part.part2,
                    Capacity = 40,
                    training_Progress_Status = Training_Progress_Status.Approved,
                    training_Status = Training_Status.Active
                },
                new Training()
                {
                    trainingID = 3,
                    siteID = 3,
                    termID = 2,
                    startTime = new DateTime(2019, 10, 20, 8, 0, 0),
                    endTime = new DateTime(2019, 10, 20, 10, 0, 0),
                    Date = new DateTime(2019, 10, 20),
                    permit = YesNO.Yes,
                    specInstructions = null,
                    part = Part.part1,
                    training_Progress_Status = Training_Progress_Status.Published,
                    Capacity = 20,
                    training_Status = Training_Status.Canceled
                },
                new Training()
                {
                    trainingID = 4,
                    siteID = 4,
                    termID = 1,
                    startTime = new DateTime(2019, 10, 27, 13, 0, 0),
                    endTime = new DateTime(2019, 10, 27, 15, 0, 0),
                    Date = new DateTime(2019, 10, 27),
                    permit = YesNO.Yes,
                    specInstructions = null,
                    part = Part.part2,
                    training_Progress_Status = Training_Progress_Status.Published,
                    Capacity = 10,
                    training_Status = Training_Status.Active,
                    linkID = 3
                },
                new Training()
                {
                    trainingID = 5,
                    siteID = 3,
                    termID = 1,
                    startTime = new DateTime(2019, 10, 11, 15, 0, 0),
                    endTime = new DateTime(2019, 10, 11, 17, 0, 0),
                    Date = new DateTime(2019, 10, 11),
                    permit = YesNO.Yes,
                    specInstructions = null,
                    part = Part.part2,
                    Capacity = 15,
                    training_Progress_Status = Training_Progress_Status.Finished,
                    training_Status = Training_Status.Active
                }
                );
            
        }

        
    }
}
