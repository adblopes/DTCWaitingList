using AutoMapper;
using DTCWaitingList.Database.Models;
using DTCWaitingList.Interfaces;
using DTCWaitingList.Models;

namespace DTCWaitingList.Mapping
{
    public class StringToPatientDayListResolver : IValueResolver<PatientView, Patient, IList<PatientDay>>
    {
        private readonly IDataAccessService _data;

        public StringToPatientDayListResolver(IDataAccessService data)
        {
            _data = data;
        }

        public IList<PatientDay> Resolve(PatientView source, Patient destination, IList<PatientDay> destMember, ResolutionContext context)
        {
            var dayNames = source.PatientDays;
            var patientDays = new List<PatientDay>();
            var days = _data.GetDays();

            foreach (var dayName in dayNames)
            {
                var patientDay = new PatientDay
                {
                    Day = days.First(d => d.NameOfDay == dayName)
                };

                patientDays.Add(patientDay);
            }

            return patientDays;
        }
    }
}
