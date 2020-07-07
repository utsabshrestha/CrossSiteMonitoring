using System;
using System.Collections.Generic;
using System.Text;

namespace Csm.Dto.Entities
{
    public class FullReportDto<T, U, R> where T : InitialsDetails where U : ConstructionObservationDetail where R : FilesDetail
    {
        private IEnumerable<T> initial;
        private IEnumerable<U> constructionObservation;
        private IEnumerable<R> files;

        public IEnumerable<T> GetInitial { get => initial; set => initial = value; }
        public IEnumerable<R> GetFiles { get => files; set => files = value; }
        public IEnumerable<U> GetConstruction { get => constructionObservation; set => constructionObservation = value; }
    }
}
