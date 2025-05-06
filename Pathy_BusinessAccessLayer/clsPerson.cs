using Pathy_DataAccessLayer;
using Pathy_DataAccessLayer.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pathy_BusinessAccessLayer
{
    public class clsPerson
    {
        public enum enMode
        {
            AddNew = 1, Update = 2, Delete = 3
        }

        public enum enGender
        {
            Male = 1, Female = 2
        }

        public int personID { get; set; }
        public string firstName { get; set; }
        public string secondName { get; set; }
        public string thirdName { get; set; }
        public string lastName { get; set; }

        public string fullname
        {
            get
            {
                return this.firstName + " " + this.secondName + " " + this.thirdName + " " + this.lastName;
            }
        }

        public string email { get; set; }
        public string phoneNumber { get; set; }
        public int gender { get; set; }
        public enMode mode { get; set; }



        public clsPerson()
        {
            this.personID = -1;
            this.firstName = "";
            this.secondName = "";
            this.thirdName = "";
            this.lastName = "";
            this.email = "";
            this.phoneNumber = "";
            this.gender = -1;
            this.mode = enMode.AddNew;
        }

        private clsPerson(int personID, string firstName, string secondName, string thirdName, string lastName, string email, string phoneNumber, short gender)
        {
            this.personID = personID;
            this.firstName = firstName;
            this.secondName = secondName;
            this.thirdName = thirdName;
            this.lastName = lastName;
            this.email = email;
            this.phoneNumber = phoneNumber;
            this.gender = gender;
            this.mode = enMode.Update;

        }

        public bool save()
        {
            switch (this.mode)
            {
                case enMode.AddNew:
                    {
                        this.personID = addNewPerson(new clsPersonDTO {firstName=this.firstName,secondName=this.secondName,thirdName=this.thirdName,lastName=this.lastName,email=this.email,phoneNumber=this.phoneNumber,gender=this.gender});
                        return this.personID != -1;
                    }

                case enMode.Update:
                    {
                        return updatePerson(new clsPersonDTO {personID=this.personID,firstName = this.firstName, secondName = this.secondName, thirdName = this.thirdName, lastName = this.lastName, email = this.email, phoneNumber = this.phoneNumber, gender = this.gender });
                    }

                case enMode.Delete:
                    {
                        return false;
                    }

            }

            return false;
        }


        // Static Methods

        public static clsPerson getPersonByPersonID(int personID)
        {
            clsPersonDTO personDTO = new clsPersonDTO();

            if (clsPersonDataAccess.GetPersonByPersonID(personID, personDTO))
            {
                return new clsPerson
                {
                    personID = personDTO.personID,
                    firstName = personDTO.firstName,
                    secondName = personDTO.secondName,
                    thirdName = personDTO.thirdName,
                    lastName = personDTO.lastName,
                    email = personDTO.email,
                    phoneNumber = personDTO.phoneNumber,
                    gender = personDTO.gender

                };
            }

            return null;
        }

        public static bool doesEmailExist(string email)
        {
            return clsPersonDataAccess.doesEmailExist(email);
        }

        public static int addNewPerson(clsPersonDTO personDTO)
        {
            return clsPersonDataAccess.addNewPerson(personDTO);
        }

        public static bool updatePerson(clsPersonDTO personDTO)
        {
            return clsPersonDataAccess.updatePerson(personDTO);
        }
    }
}
