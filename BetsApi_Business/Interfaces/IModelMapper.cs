using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetsApi_Business.Interfaces {
    public interface IModelMapper<T, U> where T : class where U : class {
        public U EFToView(T ef);
        public Task<T> ViewToEF(U view);
        public T ViewToEfModel(U v);
    }
}
