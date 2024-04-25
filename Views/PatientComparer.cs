using DTCWaitingList.Models;
using System.Collections;

namespace DTCWaitingList.Views
{
    public enum SortCriteria
    {
        PatientDays,
        PatientTimes
    }

    // Custom sort for Collections PatientDays and PatientTimes
    public class PatientComparer : IComparer
    {
        private readonly SortCriteria _sortCriteria;

        public PatientComparer(SortCriteria sortCriteria)
        {
            _sortCriteria = sortCriteria;
        }

        public int Compare(object? x, object? y)
        {
            var patientX = (PatientView)x!;
            var patientY = (PatientView)y!;

            if (_sortCriteria == SortCriteria.PatientDays)
            {
                // Concatenate the PatientDays into a single string
                string xPatientDaysString = string.Join(", ", patientX.PatientDays!);
                string yPatientDaysString = string.Join(", ", patientY.PatientDays!);

                return string.Compare(xPatientDaysString, yPatientDaysString);
            }
            else if (_sortCriteria == SortCriteria.PatientTimes)
            {
                // Concatenate the PatientTimes into a single string
                string xPatientTimesString = string.Join(", ", patientX.PatientTimes!);
                string yPatientTimesString = string.Join(", ", patientY.PatientTimes!);

                return string.Compare(xPatientTimesString, yPatientTimesString);
            }
            else
            {
                throw new InvalidOperationException("Invalid sort criteria specified.");
            }
        }
    }
}
