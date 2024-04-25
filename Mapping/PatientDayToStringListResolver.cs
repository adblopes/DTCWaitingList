using AutoMapper;
using DTCWaitingList.Database.Models;
using DTCWaitingList.Models;

namespace DTCWaitingList.Mapping
{
    public class PatientDayToStringListResolver : IValueResolver<Patient, PatientView, IList<string>?>
    {
        public IList<string>? Resolve(Patient source, PatientView destination, IList<string>? destMember, ResolutionContext context)
        {
            return source.PatientDays!.Select(pd => pd.Day!.NameOfDay).ToList()!;
        }
    }
}
