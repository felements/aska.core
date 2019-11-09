﻿// <auto-generated />
using System;
using Aska.Core.EntityStorage.DemoApp;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Aska.Core.EntityStorage.DemoApp.Migrations.DemoMariaDb
{
    [DbContext(typeof(DemoMariaDbContext))]
    partial class DemoMariaDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("Aska.Core.EntityStorage.DemoApp.MariaDbEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("char(36)");

                    b.Property<string>("Name")
                        .HasColumnType("longtext");

                    b.ToTable("MariaDbEntity");
                });
#pragma warning restore 612, 618
        }
    }
}
