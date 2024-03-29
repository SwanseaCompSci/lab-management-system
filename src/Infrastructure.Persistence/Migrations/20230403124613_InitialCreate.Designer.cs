﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SwanseaCompSci.LabManagementSystem.Infrastructure.Persistence.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20230403124613_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.15")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("SwanseaCompSci.LabManagementSystem.Core.Domain.Entities.Lab", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("CreatedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Day")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<DateTime>("EndTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("MaxNumberOfStaff")
                        .HasColumnType("int");

                    b.Property<int>("MinNumberOfStaff")
                        .HasColumnType("int");

                    b.Property<Guid>("ModuleId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<DateTime>("PeriodEnd")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("datetime2")
                        .HasColumnName("PeriodEnd");

                    b.Property<DateTime>("PeriodStart")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("datetime2")
                        .HasColumnName("PeriodStart");

                    b.Property<DateTime>("StartTime")
                        .HasColumnType("datetime2");

                    b.Property<Guid?>("UpdatedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("UtcCreated")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("UtcUpdated")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("ModuleId");

                    b.ToTable("Labs", (string)null);

                    b.ToTable(tb => tb.IsTemporal(ttb =>
                        {
                            ttb
                                .HasPeriodStart("PeriodStart")
                                .HasColumnName("PeriodStart");
                            ttb
                                .HasPeriodEnd("PeriodEnd")
                                .HasColumnName("PeriodEnd");
                        }
                    ));
                });

            modelBuilder.Entity("SwanseaCompSci.LabManagementSystem.Core.Domain.Entities.LabSchedule", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("CreatedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("End")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("LabId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("PeriodEnd")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("datetime2")
                        .HasColumnName("PeriodEnd");

                    b.Property<DateTime>("PeriodStart")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("datetime2")
                        .HasColumnName("PeriodStart");

                    b.Property<DateTime>("Start")
                        .HasColumnType("datetime2");

                    b.Property<Guid?>("UpdatedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("UtcCreated")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("UtcUpdated")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("LabId");

                    b.ToTable("LabSchedules", (string)null);

                    b.ToTable(tb => tb.IsTemporal(ttb =>
                        {
                            ttb
                                .HasPeriodStart("PeriodStart")
                                .HasColumnName("PeriodStart");
                            ttb
                                .HasPeriodEnd("PeriodEnd")
                                .HasColumnName("PeriodEnd");
                        }
                    ));
                });

            modelBuilder.Entity("SwanseaCompSci.LabManagementSystem.Core.Domain.Entities.Module", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<Guid?>("CreatedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Level")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<DateTime>("PeriodEnd")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("datetime2")
                        .HasColumnName("PeriodEnd");

                    b.Property<DateTime>("PeriodStart")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("datetime2")
                        .HasColumnName("PeriodStart");

                    b.Property<Guid?>("UpdatedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("UtcCreated")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("UtcUpdated")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("Modules", (string)null);

                    b.ToTable(tb => tb.IsTemporal(ttb =>
                        {
                            ttb
                                .HasPeriodStart("PeriodStart")
                                .HasColumnName("PeriodStart");
                            ttb
                                .HasPeriodEnd("PeriodEnd")
                                .HasColumnName("PeriodEnd");
                        }
                    ));
                });

            modelBuilder.Entity("SwanseaCompSci.LabManagementSystem.Core.Domain.Entities.ModulePreference", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ModuleId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("CreatedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("PeriodEnd")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("datetime2")
                        .HasColumnName("PeriodEnd");

                    b.Property<DateTime>("PeriodStart")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("datetime2")
                        .HasColumnName("PeriodStart");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<Guid?>("UpdatedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("UtcCreated")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("UtcUpdated")
                        .HasColumnType("datetime2");

                    b.HasKey("UserId", "ModuleId");

                    b.HasIndex("ModuleId");

                    b.ToTable("ModulePreferences", (string)null);

                    b.ToTable(tb => tb.IsTemporal(ttb =>
                        {
                            ttb
                                .HasPeriodStart("PeriodStart")
                                .HasColumnName("PeriodStart");
                            ttb
                                .HasPeriodEnd("PeriodEnd")
                                .HasColumnName("PeriodEnd");
                        }
                    ));
                });

            modelBuilder.Entity("SwanseaCompSci.LabManagementSystem.Core.Domain.Entities.TimeAvailability", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("CreatedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Day")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<DateTime>("EndTime")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsAllocated")
                        .HasColumnType("bit");

                    b.Property<DateTime>("PeriodEnd")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("datetime2")
                        .HasColumnName("PeriodEnd");

                    b.Property<DateTime>("PeriodStart")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("datetime2")
                        .HasColumnName("PeriodStart");

                    b.Property<DateTime>("StartTime")
                        .HasColumnType("datetime2");

                    b.Property<Guid?>("UpdatedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("UtcCreated")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("UtcUpdated")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("TimeAvailabilities", (string)null);

                    b.ToTable(tb => tb.IsTemporal(ttb =>
                        {
                            ttb
                                .HasPeriodStart("PeriodStart")
                                .HasColumnName("PeriodStart");
                            ttb
                                .HasPeriodEnd("PeriodEnd")
                                .HasColumnName("PeriodEnd");
                        }
                    ));
                });

            modelBuilder.Entity("SwanseaCompSci.LabManagementSystem.Core.Domain.Entities.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("AchievedLevel")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<Guid?>("CreatedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int>("MaxWeeklyWorkHours")
                        .HasColumnType("int");

                    b.Property<DateTime>("PeriodEnd")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("datetime2")
                        .HasColumnName("PeriodEnd");

                    b.Property<DateTime>("PeriodStart")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("datetime2")
                        .HasColumnName("PeriodStart");

                    b.Property<Guid?>("QuestionnaireToken")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Surname")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<Guid?>("UpdatedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("UtcCreated")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("UtcUpdated")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("Users", (string)null);

                    b.ToTable(tb => tb.IsTemporal(ttb =>
                        {
                            ttb
                                .HasPeriodStart("PeriodStart")
                                .HasColumnName("PeriodStart");
                            ttb
                                .HasPeriodEnd("PeriodEnd")
                                .HasColumnName("PeriodEnd");
                        }
                    ));
                });

            modelBuilder.Entity("SwanseaCompSci.LabManagementSystem.Core.Domain.Entities.UserLab", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("LabId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("CreatedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("PeriodEnd")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("datetime2")
                        .HasColumnName("PeriodEnd");

                    b.Property<DateTime>("PeriodStart")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("datetime2")
                        .HasColumnName("PeriodStart");

                    b.Property<Guid?>("UpdatedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("UtcCreated")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("UtcUpdated")
                        .HasColumnType("datetime2");

                    b.HasKey("UserId", "LabId");

                    b.HasIndex("LabId");

                    b.ToTable("UserLabs", (string)null);

                    b.ToTable(tb => tb.IsTemporal(ttb =>
                        {
                            ttb
                                .HasPeriodStart("PeriodStart")
                                .HasColumnName("PeriodStart");
                            ttb
                                .HasPeriodEnd("PeriodEnd")
                                .HasColumnName("PeriodEnd");
                        }
                    ));
                });

            modelBuilder.Entity("SwanseaCompSci.LabManagementSystem.Core.Domain.Entities.UserLabSchedule", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("LabScheduleId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("CreatedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("PeriodEnd")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("datetime2")
                        .HasColumnName("PeriodEnd");

                    b.Property<DateTime>("PeriodStart")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("datetime2")
                        .HasColumnName("PeriodStart");

                    b.Property<Guid?>("UpdatedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("UtcCreated")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("UtcUpdated")
                        .HasColumnType("datetime2");

                    b.HasKey("UserId", "LabScheduleId");

                    b.HasIndex("LabScheduleId");

                    b.ToTable("UserLabSchedules", (string)null);

                    b.ToTable(tb => tb.IsTemporal(ttb =>
                        {
                            ttb
                                .HasPeriodStart("PeriodStart")
                                .HasColumnName("PeriodStart");
                            ttb
                                .HasPeriodEnd("PeriodEnd")
                                .HasColumnName("PeriodEnd");
                        }
                    ));
                });

            modelBuilder.Entity("SwanseaCompSci.LabManagementSystem.Core.Domain.Entities.UserModule", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ModuleId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("CreatedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("PeriodEnd")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("datetime2")
                        .HasColumnName("PeriodEnd");

                    b.Property<DateTime>("PeriodStart")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("datetime2")
                        .HasColumnName("PeriodStart");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<Guid?>("UpdatedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("UtcCreated")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("UtcUpdated")
                        .HasColumnType("datetime2");

                    b.HasKey("UserId", "ModuleId");

                    b.HasIndex("ModuleId");

                    b.ToTable("UserModules", (string)null);

                    b.ToTable(tb => tb.IsTemporal(ttb =>
                        {
                            ttb
                                .HasPeriodStart("PeriodStart")
                                .HasColumnName("PeriodStart");
                            ttb
                                .HasPeriodEnd("PeriodEnd")
                                .HasColumnName("PeriodEnd");
                        }
                    ));
                });

            modelBuilder.Entity("SwanseaCompSci.LabManagementSystem.Core.Domain.Entities.Lab", b =>
                {
                    b.HasOne("SwanseaCompSci.LabManagementSystem.Core.Domain.Entities.Module", "Module")
                        .WithMany("Labs")
                        .HasForeignKey("ModuleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Module");
                });

            modelBuilder.Entity("SwanseaCompSci.LabManagementSystem.Core.Domain.Entities.LabSchedule", b =>
                {
                    b.HasOne("SwanseaCompSci.LabManagementSystem.Core.Domain.Entities.Lab", "Lab")
                        .WithMany("LabSchedules")
                        .HasForeignKey("LabId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Lab");
                });

            modelBuilder.Entity("SwanseaCompSci.LabManagementSystem.Core.Domain.Entities.ModulePreference", b =>
                {
                    b.HasOne("SwanseaCompSci.LabManagementSystem.Core.Domain.Entities.Module", "Module")
                        .WithMany("ModulePreferences")
                        .HasForeignKey("ModuleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SwanseaCompSci.LabManagementSystem.Core.Domain.Entities.User", "User")
                        .WithMany("ModulePreferences")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Module");

                    b.Navigation("User");
                });

            modelBuilder.Entity("SwanseaCompSci.LabManagementSystem.Core.Domain.Entities.TimeAvailability", b =>
                {
                    b.HasOne("SwanseaCompSci.LabManagementSystem.Core.Domain.Entities.User", "User")
                        .WithMany("TimeAvailabilities")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("SwanseaCompSci.LabManagementSystem.Core.Domain.Entities.UserLab", b =>
                {
                    b.HasOne("SwanseaCompSci.LabManagementSystem.Core.Domain.Entities.Lab", "Lab")
                        .WithMany("UserLabs")
                        .HasForeignKey("LabId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SwanseaCompSci.LabManagementSystem.Core.Domain.Entities.User", "User")
                        .WithMany("UserLabs")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Lab");

                    b.Navigation("User");
                });

            modelBuilder.Entity("SwanseaCompSci.LabManagementSystem.Core.Domain.Entities.UserLabSchedule", b =>
                {
                    b.HasOne("SwanseaCompSci.LabManagementSystem.Core.Domain.Entities.LabSchedule", "LabSchedule")
                        .WithMany("UserLabSchedules")
                        .HasForeignKey("LabScheduleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SwanseaCompSci.LabManagementSystem.Core.Domain.Entities.User", "User")
                        .WithMany("UserLabSchedules")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("LabSchedule");

                    b.Navigation("User");
                });

            modelBuilder.Entity("SwanseaCompSci.LabManagementSystem.Core.Domain.Entities.UserModule", b =>
                {
                    b.HasOne("SwanseaCompSci.LabManagementSystem.Core.Domain.Entities.Module", "Module")
                        .WithMany("UserModules")
                        .HasForeignKey("ModuleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SwanseaCompSci.LabManagementSystem.Core.Domain.Entities.User", "User")
                        .WithMany("UserModules")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Module");

                    b.Navigation("User");
                });

            modelBuilder.Entity("SwanseaCompSci.LabManagementSystem.Core.Domain.Entities.Lab", b =>
                {
                    b.Navigation("LabSchedules");

                    b.Navigation("UserLabs");
                });

            modelBuilder.Entity("SwanseaCompSci.LabManagementSystem.Core.Domain.Entities.LabSchedule", b =>
                {
                    b.Navigation("UserLabSchedules");
                });

            modelBuilder.Entity("SwanseaCompSci.LabManagementSystem.Core.Domain.Entities.Module", b =>
                {
                    b.Navigation("Labs");

                    b.Navigation("ModulePreferences");

                    b.Navigation("UserModules");
                });

            modelBuilder.Entity("SwanseaCompSci.LabManagementSystem.Core.Domain.Entities.User", b =>
                {
                    b.Navigation("ModulePreferences");

                    b.Navigation("TimeAvailabilities");

                    b.Navigation("UserLabSchedules");

                    b.Navigation("UserLabs");

                    b.Navigation("UserModules");
                });
#pragma warning restore 612, 618
        }
    }
}
