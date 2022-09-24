﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SoccerStatResourceServer.Repository;

namespace SoccerStatResourceServer.Migrations
{
    [DbContext(typeof(ResourceDbContext))]
    [Migration("20220815112201_AddedImageUrlProperty")]
    partial class AddedImageUrlProperty
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseIdentityColumns()
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.0");

            modelBuilder.Entity("SoccerStatResourceServer.Models.League", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ImageUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Leagues");
                });

            modelBuilder.Entity("SoccerStatResourceServer.Models.Match", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AwayTeamExtraTime")
                        .HasColumnType("int");

                    b.Property<int>("AwayTeamFullTime")
                        .HasColumnType("int");

                    b.Property<string>("AwayTeamId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AwayTeamPenalties")
                        .HasColumnType("int");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<int>("HomeTeamExtraTime")
                        .HasColumnType("int");

                    b.Property<int>("HomeTeamFullTime")
                        .HasColumnType("int");

                    b.Property<string>("HomeTeamId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("HomeTeamPenalties")
                        .HasColumnType("int");

                    b.Property<string>("LeagueId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("AwayTeamId");

                    b.HasIndex("HomeTeamId");

                    b.HasIndex("LeagueId");

                    b.ToTable("Matches");
                });

            modelBuilder.Entity("SoccerStatResourceServer.Models.Player", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TeamId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("TeamId");

                    b.ToTable("Players");
                });

            modelBuilder.Entity("SoccerStatResourceServer.Models.Team", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LeagueId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("LeagueId");

                    b.ToTable("Teams");
                });

            modelBuilder.Entity("SoccerStatResourceServer.Models.Match", b =>
                {
                    b.HasOne("SoccerStatResourceServer.Models.Team", "AwayTeam")
                        .WithMany()
                        .HasForeignKey("AwayTeamId");

                    b.HasOne("SoccerStatResourceServer.Models.Team", "HomeTeam")
                        .WithMany()
                        .HasForeignKey("HomeTeamId");

                    b.HasOne("SoccerStatResourceServer.Models.League", null)
                        .WithMany("Matches")
                        .HasForeignKey("LeagueId");

                    b.Navigation("AwayTeam");

                    b.Navigation("HomeTeam");
                });

            modelBuilder.Entity("SoccerStatResourceServer.Models.Player", b =>
                {
                    b.HasOne("SoccerStatResourceServer.Models.Team", "Team")
                        .WithMany("Players")
                        .HasForeignKey("TeamId");

                    b.Navigation("Team");
                });

            modelBuilder.Entity("SoccerStatResourceServer.Models.Team", b =>
                {
                    b.HasOne("SoccerStatResourceServer.Models.League", "League")
                        .WithMany("Teams")
                        .HasForeignKey("LeagueId");

                    b.Navigation("League");
                });

            modelBuilder.Entity("SoccerStatResourceServer.Models.League", b =>
                {
                    b.Navigation("Matches");

                    b.Navigation("Teams");
                });

            modelBuilder.Entity("SoccerStatResourceServer.Models.Team", b =>
                {
                    b.Navigation("Players");
                });
#pragma warning restore 612, 618
        }
    }
}
