using Microsoft.EntityFrameworkCore;
using MvcActor.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;

namespace MvcActor.Data
{
    public class MvcActorContext : DbContext
    {
        public MvcActorContext (DbContextOptions<MvcActorContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder){
            JArray actorsObject = new JArray();
            for(int i=1; i<20; i++){
                const  string URL = "postgres://ldetimjpwraxvx:8dd3d702392b159b4e1503c1b86ed15ea1834349f8c0826fa8262cb214923f50@ec2-54-155-87-214.eu-west-1.compute.amazonaws.com:5432/d2jv7d6pmkrc6j";
                string urlParameters = $"?api_key={Environment.GetEnvironmentVariable("API")}&language=en-US&page={i}";
                var seriesReponse = HTTP.Response.returnResponse(URL, urlParameters);
                actorsObject.Merge((JArray)seriesReponse["results"]);
            }
            int counter = 1;
            foreach(var item in actorsObject.Children()){
                string[] names = ((string)item["name"]).Split(" ");
                modelBuilder.Entity<Actor>().HasData(
                    new Actor{
                        Id = counter,
                        LastName = names[names.Length -1],
                        FirstName = String.Join(" ", names.Take(names.Length-1)),
                    }
                );
                counter += 1;
            }
        }
        public DbSet<Actor> Actor { get; set; }

    }
}