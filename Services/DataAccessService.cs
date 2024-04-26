using AutoMapper;
using DTCWaitingList.Database;
using DTCWaitingList.Database.Models;
using DTCWaitingList.Interfaces;
using DTCWaitingList.Models;

namespace DTCWaitingList.Services
{
    public class DataAccessService : IDataAccessService
    {
        private readonly WaitingListDb _dbContext;
        private readonly IMapper _mapper;

        public DataAccessService(WaitingListDb dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public List<PatientView>? GetPatients()
        {
            List<Patient> patients = _dbContext.GetPatients();
            List<PatientView> patientViews = _mapper.Map<List<Patient>, List<PatientView>>(patients);
            return patientViews;
        }

        public List<Day> GetDays()
        {
            return _dbContext.GetDays();
        }
        
        public List<Time> GetTimes()
        {
            return _dbContext.GetTimes();
        }

        public List<PatientView>? SearchPatients(Dictionary<string, object> conditions)
        {
            var patients = _dbContext.SearchPatients(conditions);
            List<PatientView> patientViews = _mapper.Map<List<Patient>, List<PatientView>>(patients);
            return patientViews;
        }

        public List<Reason> GetReasons()
        {
            return _dbContext.GetReasons();
        }

        public List<ReasonVariant> GetReasonVariants()
        {
            return _dbContext.GetReasonVariants();
        }

        public void RemovePatient(int id)
        {
            var patient = _dbContext.GetPatientById(id);
            if (patient != null)
            {
                var patientHistory = _mapper.Map<Patient, PatientHistory>(patient);
                _dbContext.AddPatientHistory(patientHistory);
                _dbContext.RemovePatient(patient);
            }
        }

        public void AddPatient(PatientView patientView)
        {
            try
            {
                Patient patient = _mapper.Map<PatientView, Patient>(patientView);
                _dbContext.AddPatient(patient);
            }
            catch (Exception ex)
            {
                throw new AutoMapperMappingException(ex.Message);
            }
        }
    }
}
