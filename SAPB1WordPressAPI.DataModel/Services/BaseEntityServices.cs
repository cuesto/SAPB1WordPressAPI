using SAPB1WordPressAPI.DataModel.Bases;
using SAPB1WordPressAPI.DataModel.Entities;
using System;

namespace SAPB1WordPressAPI.DataModel.Services
{
    public static class BaseEntityServices
    {
        /// <summary>
        ///     Sets the values.
        /// </summary>
        /// <typeparam name="T">The type of the entity.</typeparam>
        /// <param name="entityBase">The entity base.</param>
        public static void SetValues<T>(this T entityBase) where T : BaseEntity
        {
            if (string.IsNullOrEmpty(entityBase.ModifiedBy))
            {
                entityBase.Created = DateTime.Now;
            }
            else
            {
                entityBase.Modified = DateTime.Now;
            }
        }
    }
}
