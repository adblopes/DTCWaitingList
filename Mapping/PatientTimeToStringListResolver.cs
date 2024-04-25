using AutoMapper;
using DTCWaitingList.Database.Models;
using DTCWaitingList.Interfaces;
using DTCWaitingList.Models;

namespace DTCWaitingList.Mapping
{
    public class PatientTimeToStringListResolver : IValueResolver<Patient, PatientView, IList<string>?>
    {
        public IList<string>? Resolve(Patient source, PatientView destination, IList<string>? destMember, ResolutionContext context)
        {
            return source.PatientTimes!.Select(pd => pd.Time!.TimeOfDay).ToList()!;
        }
    }
}
