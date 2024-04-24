using AutoMapper;
using DTCWaitingList.Database;
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

        public List<PatientView> GetPatients()
        {
            List<Patient> patient = _dbContext.GetPatients();

            List<PatientView> patientView = _mapper.Map<List<Patient>, List<PatientView>>(patient);

            return patientView;
        }

        public List<Day> GetDays()
        {
            return _dbContext.GetDays();
        }
        
        public List<Time> GetTimes()
        {
            return _dbContext.GetTimes();
        }

        //public async Task<List<Patient>> GetPatientsAsync(string[]? args)
        //{
        //    //parse search parameters
        //    return await _dbContext.GetPatientsAsync();
        //}


        public List<Reason> GetReasons()
        {
            return _dbContext.GetReasons();
        }

        public List<ReasonVariant> GetReasonVariants()
        {
            return _dbContext.GetReasonVariants();
        }

        public async Task RemovePatientAsync(int id)
        {
            var patient = await _dbContext.GetPatientByIdAsync(id);
            if (patient != null)
            {
                var patientHistory = _mapper.Map<Patient, PatientHistory>(patient);
                await _dbContext.RemovePatientAsync(patient, patientHistory);
            }
        }

        public async Task AddPatientAsync(PatientView patientView)
        {
            try
            {
                Patient patient = _mapper.Map<PatientView, Patient>(patientView);

                await _dbContext.AddPatientAsync(patient);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
