﻿// <auto-generated />
using System;
using MassTransitUi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace MassTransitUi.Migrations
{
    [DbContext(typeof(MassTransitUiContext))]
    partial class MassTransitUiContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "5.0.0");

            modelBuilder.Entity("MassTransitUi.Models.FailedMessage", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<byte[]>("Content")
                        .HasColumnType("BLOB");

                    b.Property<string>("ErrorMessage")
                        .HasColumnType("TEXT");

                    b.Property<string>("MessageId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Properties")
                        .HasColumnType("TEXT");

                    b.Property<string>("Queue")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("RecievedTsUtc")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("FailedMessages");
                });

            modelBuilder.Entity("MassTransitUi.Models.FailedMessageHeader", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<long>("FailedMessageId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Key")
                        .HasColumnType("TEXT");

                    b.Property<string>("Value")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("FailedMessageId");

                    b.ToTable("FailedMessageHeader");
                });

            modelBuilder.Entity("MassTransitUi.Models.FailedMessageHeader", b =>
                {
                    b.HasOne("MassTransitUi.Models.FailedMessage", null)
                        .WithMany("Headers")
                        .HasForeignKey("FailedMessageId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("MassTransitUi.Models.FailedMessage", b =>
                {
                    b.Navigation("Headers");
                });
#pragma warning restore 612, 618
        }
    }
}
