using AutoMapper;
using DTCWaitingList.Database.Models;
using DTCWaitingList.Interfaces;
using DTCWaitingList.Models;

namespace DTCWaitingList.Mapping
{
    public class StringToPatientTimeListResolver : IValueResolver<PatientView, Patient, IList<PatientTime>?>
    {
        private readonly IDataAccessService _data;

        public StringToPatientTimeListResolver(IDataAccessService data)
        {
            _data = data;
        }

        public IList<PatientTime>? Resolve(PatientView source, Patient destination, IList<PatientTime>? destMember, ResolutionContext context)
        {
            var timeNames = source.PatientTimes;
            var patientTimes = new List<PatientTime>();
            var times = _data.GetTimes();

            foreach (var timeName in timeNames!)
            {
                var patientTime = new PatientTime
                {
                    Time = times.First(d => d.TimeOfDay == timeName)
                };

                patientTimes.Add(patientTime);
            }

            return patientTimes;
        }
    }
}
