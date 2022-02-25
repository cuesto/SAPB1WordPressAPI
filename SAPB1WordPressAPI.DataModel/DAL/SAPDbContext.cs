using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using SAPB1WordPressAPI.DataModel.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SAPB1WordPressAPI.DataModel.DAL
{
    public class SapDbContext : DbContext
    {
        public SapDbContext(DbContextOptions<SapDbContext> options) : base(options)
        {

        }

        public virtual DbSet<OCRD> OCRD { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                modelBuilder.Entity(entityType.ClrType).ToTable(entityType.ClrType.Name);

                entityType.GetForeignKeys()
                    .Where(fk => !fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.Cascade)
                    .ToList()
                    .ForEach(fk => fk.DeleteBehavior = DeleteBehavior.Restrict);
            }

            modelBuilder.Query<ISCustomerStatement>();

            base.OnModelCreating(modelBuilder);
        }

        /// <summary>  
        /// Get Customer Statement  
        /// </summary>  
        /// <param name="cardcode">Customer ID value parameter</param>  
        /// <param name="startdate">StartDate value parameter</param>  
        /// <param name="enddate">StartDate value parameter</param>  
        /// <returns>Returns - List of AR invoice with incoming payments</returns>  
        //[Obsolete]
        public async Task<List<ISCustomerStatement>> GetCustomerStatementAsync(string cardcode, DateTime startdate, DateTime enddate)
        {
            // Initialization.  
            List<ISCustomerStatement> lst = new List<ISCustomerStatement>();

            try
            {
                // Settings.  
                SqlParameter cardCodeParam = new SqlParameter("@cardcode", cardcode ?? (object)DBNull.Value);
                SqlParameter startDateParam = new SqlParameter("@startdate", startdate.ToString() ?? (object)DBNull.Value);
                SqlParameter endDateParam = new SqlParameter("@enddate", enddate.ToString() ?? (object)DBNull.Value);


                // Processing.  
                string sqlQuery = "EXEC [dbo].[ISCustomerStatement] " +
                                    "@cardcode, @startdate, @enddate";

                lst = await this.Query<ISCustomerStatement>().FromSqlRaw(sqlQuery, cardCodeParam, startDateParam, endDateParam).ToListAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            // Info.  
            return lst;
        }

    }
}
