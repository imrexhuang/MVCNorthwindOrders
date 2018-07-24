using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

/* 提供給Service層使用 */

namespace NorthwindOrdersEntity.Models
{
    public class ModelContext : DbContext
    {
        public ModelContext() : base("name=NorthwindEntities")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}