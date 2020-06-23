using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLibrary.Models
{
    public class GenericReport<T, U, R> where T : Inital where U : ConstructionObservation where R : Files
    {
        private IEnumerable<T> initial;
        private IEnumerable<U> constructionObservation;
        private IEnumerable<R> files;

        public IEnumerable<T> GetInitial { get => initial; set => initial = value; }
        public IEnumerable<R> GetFiles { get => files; set => files = value; }
        public IEnumerable<U> GetConstruction { get => constructionObservation; set => constructionObservation = value; }
    }
}
