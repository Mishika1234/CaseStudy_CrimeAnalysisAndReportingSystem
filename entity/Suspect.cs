using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrimeAnalysisAndReportingSystem.entity
{
    public class Suspect
    {
      
        private int _suspectID;
        private string _firstName;
        private string _lastName;
        private DateTime _dateOfBirth;
        private char _gender;
        private string _contactInformation;

        public Suspect(int suspectID, string firstName, string lastName, DateTime dateOfBirth, char gender, string contactInformation)
        {
            _suspectID = suspectID;
            _firstName = firstName;
            _lastName = lastName;
            _dateOfBirth = dateOfBirth;
            _gender = gender;
            _contactInformation = contactInformation;
        }

        public int SuspectID
        {
            get { return _suspectID; }
            set { _suspectID = value; }
        }

        public string FirstName
        {
            get { return _firstName; }
            set { _firstName = value; }
        }

        public string LastName
        {
            get { return _lastName; }
            set { _lastName = value; }
        }

        public DateTime DateOfBirth
        {
            get { return _dateOfBirth; }
            set { _dateOfBirth = value; }
        }

        public char Gender
        {
            get { return _gender; }
            set { _gender = value; }
        }

        public string ContactInformation
        {
            get { return _contactInformation; }
            set { _contactInformation = value; }
        }
    }
}