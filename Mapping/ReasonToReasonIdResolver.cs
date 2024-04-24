using AutoMapper;
using DTCWaitingList.Interfaces;
using DTCWaitingList.Models;
using Microsoft.IdentityModel.Tokens;

namespace DTCWaitingList.Mapping
{
    public class ReasonToReasonIdResolver : IValueResolver<PatientView, Patient, int>
    {
        private readonly IDataAccessService _data;

        public ReasonToReasonIdResolver(IDataAccessService data)
        {
            _data = data;
        }

        public int Resolve(PatientView source, Patient destination, int destMember, ResolutionContext context)
        {
            var reasons = _data.GetReasons();
            var reasonVariants = _data.GetReasonVariants();
            int result = reasons.First().ReasonId;

            if (!source.FullReason.IsNullOrEmpty())
            {
                var data = reasonVariants.FirstOrDefault(r => source.FullReason!.Contains(r.Term!, StringComparison.CurrentCultureIgnoreCase));

                if (data != null)
                {
                    result = reasons.Find(r => r.ReasonId == data.ReasonId)!.ReasonId;
                }
            }

            return result;
        }
    }
}
