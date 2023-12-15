// See https://aka.ms/new-console-template for more information

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Reflection.Metadata.Ecma335;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
using Microsoft.Extensions.Logging;
using System.Linq;
using Azure.Core;

namespace SQLtest;
class Program
{
    static async Task Main(string[] args)
    {
        /* Connect to SQL Server */
        await using var db = new PartsDbContext();


        Part newPart = new Part { PART_KEY = 0004, PART_NAME = "test" };
        /* (C) Create record in the database */
        if (db.Part.Single(p => p.PART_KEY == 0004) == null)
        {
            try
            {
               
                db.Add(newPart);
                db.SaveChanges();
            
            }
            catch (Exception exc)
            {
                if (exc.InnerException != null)
                    Console.WriteLine("Create: " + exc.InnerException.Message);
            }
        }


        /* (U) Update a record */
        try
        {
            var partToUpdate = db.Part.Single(p => p.PART_KEY == 789);
            if ( partToUpdate != null)
            {
                partToUpdate.PART_NAME = "F O R D  F 1 5 0";
                db.SaveChanges();
            }
        }
        catch (Exception exu)
        {
            if (exu.InnerException != null)
                Console.WriteLine("Update: " + exu.InnerException.Message);
        }

        /* (D) Delete a record */
        try
        {
            var partToDelete = db.Part.Single(p => p.PART_KEY == 1);
            db.Part.Remove(partToDelete);
            db.SaveChanges();
            Console.WriteLine("AFTER REMOVE SAVE");
        } 
        catch (Exception exd) {
            if (exd.InnerException != null)
                Console.WriteLine("Create: " + exd.InnerException.Message);
        }

        /* (R) Read the database */
        db.printSet();
    }
}

public class PartsDbContext : DbContext
{
    public DbSet<Part> Part {  get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder
            .UseSqlServer(@"Server=MTX0588\SQLEXPRESS;Database=test;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True;User Id=MTXDOM\pcannell;Password=purpleunicornsdancearoundtheOasis12#");
            //.LogTo(Console.WriteLine, LogLevel.Information);
    
    public void printSet()
    {
        var list = this.Part.ToList();
        Console.WriteLine("LIST COUNT= " + list.Count);
        foreach (var pt in list)
        {
            Console.WriteLine("" + pt.PART_NAME);
        }
    }

}

public class Part
{
    [Key]
    public int PART_KEY {  get; set; }
    public String PART_NAME { get; set; }
}


