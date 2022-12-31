using AI_Hospital.exceptions;
using AI_Hospital.model;
using AI_Hospital.sort;
using AI_Hospital.generics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AI_Hospital
{
    class Program
    {

        private static List<County> countiesList = new List<County>();
        private static List<Symptom> symptomsList = new List<Symptom>();
        private static List<Disease> diseasesList = new List<Disease>();
        private static List<Person> personsList = new List<Person>();
        private static Dictionary<Disease, List<Person>> sufferingFromDiseaseDictionary = new Dictionary<Disease, List<Person>>();

        static void Main(string[] args)
        {
            EnterCounty();
            EnterSymptom();
            EnterDisease();
            EnterPerson();

            PrintAllInfo();

            FindSuffersFromDisease();
            PrintSuffersFromDisease();

            MostAffectedCounty();

            ClinicForInfectedDiseases<Virus, Person> clinic = CreateClinicForInfectedDiseases();
            if(clinic.VirusList.Count > 0) { clinic.SortVirus(); clinic.PrintViruses(); }

            FindPeopleBySurname();

            FindNumberOfSymptomsOfDisease();

        }

        /// <summary>
        /// The method in which county data is entered
        /// </summary>
        private static void EnterCounty()
        {
            int numOfCounties = 0;
            bool isValid = false;
            NumberOfEntities(ref numOfCounties, ref isValid, "counties");

            Console.WriteLine("Please insert information about {0} counties:", numOfCounties);
            for (int i = 0; i < numOfCounties; i++)
            {
                Console.WriteLine("Please insert information about {0}. county:", i + 1);
                Console.Write("Enter the county name: ");
                string countyName = Console.ReadLine();

                int countyPopulation = 0;
                isValid = false;
                do
                {
                    try
                    {
                        Console.Write("Enter the county population: ");
                        countyPopulation = Int32.Parse(Console.ReadLine());
                        isValid = true;
                    }
                    catch (FormatException ex)
                    {
                        Console.WriteLine("Wrong input: {0}\nPlease enter integer.", ex.Message);
                    }
                } while (!isValid);

                int countyAffectedPopulation = 0;
                isValid = false;
                do
                {
                    try
                    {
                        Console.Write("Enter the affected population of the county: ");
                        countyAffectedPopulation = Int32.Parse(Console.ReadLine());
                        isValid = true;
                    }
                    catch (FormatException ex)
                    {
                        Console.WriteLine("Wrong input: {0}\nPlease enter integer.", ex.Message);
                    }
                } while (!isValid);

                countiesList.Add(new County(countyName, countyPopulation, countyAffectedPopulation));
            }
        }



        /// <summary>
        /// The method in which symptom data is entered
        /// </summary>
        private static void EnterSymptom()
        {
            int numOfSymptoms = 0;
            bool isValid = false;
            NumberOfEntities(ref numOfSymptoms, ref isValid, "symptoms");

            Console.WriteLine("Please insert information about {0} symptoms:", numOfSymptoms);
            for (int i = 0; i < numOfSymptoms; i++)
            {
                Console.WriteLine("Please insert information about {0}. symptom:", i + 1);
                Console.Write("Enter the name of the symptom: ");
                string symptomName = Console.ReadLine();
                Console.Write("Enter the value of the symptom (RARE, MEDIUM, OFTEN): ");
                string symptomValue = Console.ReadLine();

                symptomsList.Add(new Symptom(symptomName, symptomValue));
            }
        }

        /// <summary>
        /// The method in which disease data is entered
        /// </summary>
        private static void EnterDisease()
        {
            int numOfDiseases = 0;
            bool isValid = false;
            NumberOfEntities(ref numOfDiseases, ref isValid, "diseases");

            Console.WriteLine("Please insert information about {0} diseases:", numOfDiseases);
            for(int i = 0; i < numOfDiseases; i++)
            {
                int inputType = 0;
                isValid = false;
                do {
                    try
                    {
                        Console.Write("Do you want to enter a disease or virus?\n1) DISEASE\n2) VIRUS\nSelection: ");
                        inputType = Int32.Parse(Console.ReadLine());
                        CheckCorrectInput(inputType, 2, 1);
                        isValid = true;
                    }
                    catch(FormatException ex)
                    {
                        Console.WriteLine("Wrong input: {0}\nPlease enter integer.", ex.Message);
                    }
                    catch(InputGreaterThanOptionsException ex2)
                    {
                        Console.WriteLine("Wrong input: {0}\nPlease try again.", ex2.Message);
                    }
                } while (!isValid);

                Console.WriteLine("Please insert information about {0}. disease:", i + 1);
                Console.Write("Enter the name of the disease: ");
                string diseaseName = Console.ReadLine();

                int numberOfSymptoms = 0;
                isValid = false;
                do {
                    try
                    {
                        Console.Write("Enter the number of symptoms: ");
                        numberOfSymptoms = Int32.Parse(Console.ReadLine());
                        CheckCorrectInput(numberOfSymptoms, symptomsList.Count, 1);
                        isValid = true;
                    }
                    catch(FormatException ex)
                    {
                        Console.WriteLine("Wrong input: {0}\nPlease enter integer.", ex.Message);
                    }
                    catch (InputGreaterThanOptionsException ex2)
                    {
                        Console.WriteLine("Wrong input: {0} Max number of symptoms is {1}, min is 1.\nPlease try again.", ex2.Message, symptomsList.Count);
                    }
                } while (!isValid);

                List < Symptom > symptomsOfDisease = new List<Symptom>();
                for(int j = 0; j < numberOfSymptoms; j++)
                {
                    isValid = false;
                    do
                    {
                        try
                        {
                            Console.WriteLine("Select {0}. symptom: ", j + 1);
                            for (int k = 0; k < symptomsList.Count; k++)
                            {
                                Console.WriteLine("{0}. {1} - {2}", k + 1, symptomsList[k].Name, symptomsList[k].Value);
                            }

                            Console.Write("Selection: ");
                            int symptomSelection = Int32.Parse(Console.ReadLine());
                            CheckCorrectInput(symptomSelection, symptomsList.Count, 1);
                            symptomsOfDisease.Add(symptomsList[symptomSelection - 1]);
                            isValid = true;
                        }
                        catch(FormatException ex)
                        {
                            Console.WriteLine("Wrong input: {0}\nPlease enter integer.", ex.Message);
                        }
                        catch (InputGreaterThanOptionsException ex2)
                        {
                            Console.WriteLine("Wrong input: {0}\nPlease try again.", ex2.Message);
                        }
                    } while (!isValid);
                }

                if(inputType == 1)
                    diseasesList.Add(new Disease(diseaseName, symptomsOfDisease));
                else if(inputType == 2)
                    diseasesList.Add(new Virus(diseaseName, symptomsOfDisease));
            }
        }

        /// <summary>
        /// The method in which data about person is entered
        /// </summary>
        private static void EnterPerson()
        {
            int numOfPeople = 0;
            bool isValid = false;
            NumberOfEntities(ref numOfPeople, ref isValid, "people");

            Console.WriteLine("Please insert information about {0} people:", numOfPeople);
            for(int i = 0; i < numOfPeople; i++)
            {
                Console.WriteLine("Please insert information about {0}. person:", i + 1);
                Console.Write("Enter the person's first name: ");
                string personFirstName = Console.ReadLine();
                Console.Write("Enter the person's last name: ");
                string personLastName = Console.ReadLine();

                int personAge = 0;
                isValid = false;
                do
                {
                    try
                    {
                        Console.Write("Enter the person's age: ");
                        personAge = Int32.Parse(Console.ReadLine());
                        isValid = true;
                    }
                    catch(FormatException ex)
                    {
                        Console.WriteLine("Wrong input: {0}\nPlease enter integer.", ex.Message);
                    }
                } while (!isValid);

                int countySelection = 0;
                isValid = false;
                do
                {
                    try
                    {
                        Console.WriteLine("Select the county of the person:");
                        for (int j = 0; j < countiesList.Count; j++)
                        {
                            Console.WriteLine("{0}. {1}", j + 1, countiesList[j].Name);
                        }
                        Console.Write("Selection: ");
                        countySelection = Int32.Parse(Console.ReadLine());
                        CheckCorrectInput(countySelection, countiesList.Count, 1);
                        isValid = true;
                    }
                    catch(FormatException ex)
                    {
                        Console.WriteLine("Wrong input: {0}\nPlease enter integer.", ex.Message);
                    }
                    catch (InputGreaterThanOptionsException ex2)
                    {
                        Console.WriteLine("Wrong input: {0}\nPlease try again.", ex2.Message);
                    }
                } while (!isValid);

                int diseaseSelection = 0;
                isValid = false;
                do
                {
                    try
                    {
                        Console.WriteLine("Select a person's disease:");
                        for (int j = 0; j < diseasesList.Count; j++)
                        {
                            Console.WriteLine("{0}. {1}", j + 1, diseasesList[j].Name);
                        }
                        Console.Write("Selection: ");
                        diseaseSelection = Int32.Parse(Console.ReadLine());
                        CheckCorrectInput(diseaseSelection, diseasesList.Count, 1);
                        isValid = true;
                    }
                    catch(FormatException ex)
                    {
                        Console.WriteLine("Wrong input: {0}\nPlease enter integer.", ex.Message);
                    }
                    catch (InputGreaterThanOptionsException ex2)
                    {
                        Console.WriteLine("Wrong input: {0}\nPlease try again.", ex2.Message);
                    }
                } while (!isValid);

                if (i != 0)
                {
                    isValid = false;
                    do
                    {
                        try
                        {
                            List<Person> contactedPersonsList = new List<Person>();
                            Console.Write("Enter the number of people this person has been in contact with: ");
                            int numberOfContacts = Int32.Parse(Console.ReadLine());
                            CheckCorrectInput(numberOfContacts, i, 0);
                            for (int j = 0; j < numberOfContacts; j++)
                            {
                                Console.WriteLine("Select {0}. person:", j + 1);
                                for (int k = 0; k < personsList.Count; k++)
                                {
                                    Console.WriteLine("{0}. {1} {2}", k + 1, personsList[k].FirstName, personsList[k].LastName);
                                }
                                Console.Write("Selection: ");
                                int contactedPersonSelection = Int32.Parse(Console.ReadLine());
                                CheckCorrectInput(contactedPersonSelection, personsList.Count, 1);

                                CheckDuplicateContactedPerson(personsList, contactedPersonsList, contactedPersonSelection);
                                contactedPersonsList.Add(personsList[contactedPersonSelection - 1]);
                            }

                            personsList.Add(new Person.PersonBuilder(personFirstName, personLastName)
                                                                            .HasAge(personAge)
                                                                            .HasCounty(countiesList[countySelection - 1])
                                                                            .HasDisease(diseasesList[diseaseSelection - 1])
                                                                            .HasContactedPersonsList(contactedPersonsList)
                                                                            .Build());
                            isValid = true;
                        }
                        catch(FormatException ex)
                        {
                            Console.WriteLine("Wrong input: {0}\nPlease enter integer.", ex.Message);
                        }
                        catch(ContactedPersonDuplicateException ex2)
                        {
                            Console.WriteLine("Wrong input: {0}\nPlease try again.", ex2.Message);
                        }
                        catch (InputGreaterThanOptionsException ex3)
                        {
                            Console.WriteLine("Wrong input: {0} Max number of contacted people is {1}.\nPlease try again.", ex3.Message, i);
                        }
                    } while (!isValid);
                }
                else
                {
                    personsList.Add(new Person.PersonBuilder(personFirstName, personLastName)
                                                .HasAge(personAge)
                                                .HasCounty(countiesList[countySelection - 1])
                                                .HasDisease(diseasesList[diseaseSelection - 1])
                                                .Build());
                }
            }
        }

        /// <summary>
        /// A method that creates a number of entities of a particular class
        /// </summary>
        /// <param name="numOfEntities">Number of entities user wants to create</param>
        /// <param name="isValid">Boolean flag to check if input is correct</param>
        /// <param name="entity">Particular entity</param>
        private static void NumberOfEntities(ref int numOfEntities, ref bool isValid, string entity)
        {
            do
            {
                try
                {
                    Console.Write("Please enter how many {0} you would like to enter: ", entity);
                    numOfEntities = Int32.Parse(Console.ReadLine());
                    CheckIfZero(numOfEntities);
                    isValid = true;
                }
                catch (FormatException ex)
                {
                    Console.WriteLine("Wrong input: {0}\nPlease enter integer.", ex.Message);
                }
                catch(ZeroNumberOfEntitiesException ex2)
                {
                    Console.WriteLine("Wrong input: {0}\nPlease try again.", ex2.Message);
                }
            } while (!isValid);
        }

        /// <summary>
        /// A method in which all entered data is displayed inside of the console
        /// </summary>
        private static void PrintAllInfo()
        {
            Console.WriteLine("A LIST OF PEOPLE WITH ALL UPDATED INFORMATIONS:");
            for(int i = 0; i < personsList.Count; i++)
            {
                Console.WriteLine("Full name: {0} {1}", personsList[i].FirstName, personsList[i].LastName);
                Console.WriteLine("Age: {0}", personsList[i].Age);
                Console.WriteLine("County of residence: {0}", personsList[i].County.Name);
                Console.WriteLine("Infected with the {0}: {1}", personsList[i].Disease is Virus ? "virus" : "disease", personsList[i].Disease.Name);
                Console.WriteLine("List of contacted people: ");
                if(i == 0 || personsList[i].ContactedPersonsList.Count == 0)
                {
                    Console.WriteLine("No people contacted.");
                }
                else
                {
                    for(int j = 0; j < personsList[i].ContactedPersonsList.Count; j++)
                    {
                        Console.WriteLine("{0}. {1} {2}", j+1, personsList[i].ContactedPersonsList[j].FirstName, personsList[i].ContactedPersonsList[j].LastName);
                    }
                }
                Console.WriteLine();
            }
        }

        /// <summary>
        /// A method that adds disease and all of the people suffering from that disease to dictionary
        /// </summary>
        private static void FindSuffersFromDisease()
        {
            foreach (Person person in personsList)
            {
                if(sufferingFromDiseaseDictionary.ContainsKey(person.Disease))
                {
                    sufferingFromDiseaseDictionary[person.Disease].Add(person); 
                }
                else
                {
                    sufferingFromDiseaseDictionary.Add(person.Disease, new List<Person> { person });
                }
            }
        }

        /// <summary>
        /// Prints disease and all of the people suffering from that disease to the console
        /// </summary>
        private static void PrintSuffersFromDisease()
        {
            foreach(KeyValuePair<Disease,List<Person>> entry in sufferingFromDiseaseDictionary)
            {
                string tmpDisease = string.Empty;
                if (entry.Key is Virus)
                    tmpDisease = "virus";
                else
                    tmpDisease = "disease";

                Console.Write("{0} {1} affects people: ", entry.Key.Name, tmpDisease);
                for(int i = 0; i < entry.Value.Count; i++)
                {
                    Console.Write("{0} {1}", entry.Value[i].FirstName, entry.Value[i].LastName);
                    if (i != entry.Value.Count - 1)
                        Console.Write(", ");                        
                }
                Console.WriteLine();
            }
        }

        /// <summary>
        /// A method that finds the county in which the most infected population lives
        /// </summary>
        private static void MostAffectedCounty()
        {
            countiesList.Sort(new MostAffectedInCounty());
            Console.WriteLine("The most infected people are in the county: {0} {1}%.", countiesList[0].Name, countiesList[0].PercentageOfAffectedPopulation);
        }

        /// <summary>
        /// Method that creates clinic that stores all viruses entered in the application and people infected by those viruses
        /// </summary>
        /// <returns>Clinic</returns>
        private static ClinicForInfectedDiseases<Virus, Person> CreateClinicForInfectedDiseases()
        {
            var virusesList = (from disease in diseasesList
                               where disease is Virus
                               select disease).ToList();
            ClinicForInfectedDiseases<Virus, Person> clinic = new ClinicForInfectedDiseases<Virus, Person>();
            foreach (Virus vir in virusesList)
            {
                clinic.AddVirus(vir);
            }
            foreach (Person person in personsList)
            {
                if (person.Disease is Virus)
                {
                    clinic.AddPerson(person);
                }
            }
            return clinic;
        }

        /// <summary>
        /// Method that allows user to search for a specific person by last name
        /// </summary>
        private static void FindPeopleBySurname()
        {
            Console.Write("Please enter the last name (or part of the last name) by which you would like to search: ");
            string input = Console.ReadLine();
            var tmpList = (from person in personsList
                           where person.LastName.Contains(input)
                           select person).ToList();
            Console.WriteLine("A list of people matching the search (\"{0}\"):", input);
            if (tmpList.Count > 0)
            {
                for (int i = 0; i < tmpList.Count; i++)
                {
                    Console.WriteLine("{0}. {1} {2}", i + 1, tmpList[i].FirstName, tmpList[i].LastName);
                }
            }
            else
                Console.WriteLine("No person matched the search.");
        }

        /// <summary>
        /// Method that finds number of symptoms of current disease
        /// </summary>
        private static void FindNumberOfSymptomsOfDisease()
        {
            diseasesList.ForEach(disease =>
            {
                Console.WriteLine("{0} has {1} symptoms.", disease.Name, disease.SymptomsList.Count);
            });
        }

        /// <summary>
        /// A method that throws an exception in the case of entering duplicates of the contacted person
        /// </summary>
        /// <param name="personsList">A list that includes all the people entered in the application</param>
        /// <param name="tmpPersonsList">A temporary list covering all people with whom the current person has been in contact</param>
        /// <param name="selection">User selection</param>
        private static void CheckDuplicateContactedPerson(List<Person> personsList, List<Person> tmpPersonsList, int selection)
        {
            for(int i = 0; i < tmpPersonsList.Count; i++)
            {
                if(personsList[selection-1].FirstName.Equals(tmpPersonsList[i].FirstName) && personsList[selection-1].LastName.Equals(tmpPersonsList[i].LastName))
                {
                    throw new ContactedPersonDuplicateException("This person has already been added to the list of contacted people.");
                }
            }
        }

        /// <summary>
        /// A method that throws an exception in the case of entering wrong input
        /// </summary>
        /// <param name="input">Entered input by the user</param>
        /// <param name="options">Current options user can choose from</param>
        /// <param name="min">Minimal option user could select</param>
        private static void CheckCorrectInput(int input, int options, int min)
        {
            if (input > options)
                throw new InputGreaterThanOptionsException("Your selection (input) is greater than the number of options.");
            else if(input < min)
                throw new InputGreaterThanOptionsException("Your selection (input) is lesser than the number of options.");
        }

        private static void CheckIfZero(int entityNum)
        {
            if (entityNum <= 0)
                throw new ZeroNumberOfEntitiesException("You must enter at least 1 entity.");
        }

    }
}
