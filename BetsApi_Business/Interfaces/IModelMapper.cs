using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetsApi_Business.Interfaces {
    public interface IModelMapper<T, U> where T : class where U : class {
        /// <summary>
        /// Purpose: Return a converted Entity Framework Model to a View Model.
        /// </summary>
        /// <param name="ef"></param>
        /// <returns>ViewModel</returns>
        public U EFToView(T ef);
        /// <summary>
        /// Purpose: Return a View Model to an Entity Framework Model by searching the Database.
        /// </summary>
        /// <param name="view"></param>
        /// <returns>Entity Framework Model</returns>
        public Task<T> ViewToEF(U view);
    }
}
