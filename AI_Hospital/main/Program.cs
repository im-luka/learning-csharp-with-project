using AI_Hospital.exceptions;
using AI_Hospital.model;
using AI_Hospital.sort;
using AI_Hospital.generics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Newtonsoft.Json;

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
            ReadCounties();
            ReadSymptoms();
            ReadDisease();
            ReadViruses();
            ReadPersons();

            PrintAllInfo();

            FindSuffersFromDisease();
            PrintSuffersFromDisease();

            MostAffectedCounty();

            ClinicForInfectedDiseases<Virus, Person> clinic = CreateClinicForInfectedDiseases();
            if (clinic.VirusList.Count > 0) { clinic.SortVirus(); clinic.PrintViruses(); }

            FindPeopleBySurname();

            FindNumberOfSymptomsOfDisease();

            SerializationOfCounties();
            DeserializationOfCounties();
        }


        /// <summary>
        /// A method that writes all stored information about counties within a file to a program
        /// </summary>
        private static void ReadCounties()
        {
            Console.WriteLine("Loading county data...");
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\files\");
            string fileName = "counties.txt";
            if (File.Exists(path + fileName))
            {
                using (StreamReader reader = File.OpenText(path + fileName))
                {
                    string line;
                    while((line = reader.ReadLine()) != null)
                    {
                        long id = long.Parse(line);
                        string name = reader.ReadLine();
                        int population = Int32.Parse(reader.ReadLine());
                        int affectedPopulation = Int32.Parse(reader.ReadLine());

                        countiesList.Add(new County(id, name, population, affectedPopulation));
                    }
                }
            }
            else
                Console.WriteLine($"File {fileName} doesn't exist.");
        }

        /// <summary>
        /// A method that writes all stored information about symptoms within a file to a program
        /// </summary>
        private static void ReadSymptoms()
        {
            Console.WriteLine("Loading symptom data...");
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\files\");
            string fileName = "symptoms.txt";
            if(File.Exists(path + fileName))
            {
                using (StreamReader reader = File.OpenText(path + fileName))
                {
                    string line;
                    while((line = reader.ReadLine()) != null)
                    {
                        long id = long.Parse(line);
                        string name = reader.ReadLine();
                        string value = reader.ReadLine();

                        symptomsList.Add(new Symptom(id, name, value));
                    }
                }
            }
            else
                Console.WriteLine($"File {fileName} doesn't exist.");
        }

        /// <summary>
        /// A method that writes all stored information about diseases within a file to a program
        /// </summary>
        private static void ReadDisease()
        {
            Console.WriteLine("Loading disease data...");
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\files\");
            string fileName = "diseases.txt";
            if (File.Exists(path + fileName))
            {
                using (StreamReader reader = File.OpenText(path + fileName))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        long id = long.Parse(line);
                        string name = reader.ReadLine();

                        List<Symptom> tmpSymptomsList = new List<Symptom>();
                        string values = reader.ReadLine();
                        string[] delimiter = values.Split(',');
                        foreach (string val in delimiter)
                        {
                            tmpSymptomsList.Add(symptomsList.Find(symptom => (symptom.Id == long.Parse(val))));
                        }

                        diseasesList.Add(new Disease(id, name, tmpSymptomsList));
                    }
                }
            }
            else
                Console.WriteLine($"File {fileName} doesn't exist.");
        }

        /// <summary>
        /// A method that writes all stored information about viruses within a file to a program
        /// </summary>
        private static void ReadViruses()
        {
            Console.WriteLine("Loading viruses data...");
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\files\");
            string fileName = "viruses.txt";
            if (File.Exists(path + fileName))
            {
                using (StreamReader reader = File.OpenText(path + fileName))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        long id = long.Parse(line);
                        string name = reader.ReadLine();

                        List<Symptom> tmpSymptomsList = new List<Symptom>();
                        string values = reader.ReadLine();
                        string[] delimiter = values.Split(',');
                        foreach (string val in delimiter)
                        {
                            tmpSymptomsList.Add(symptomsList.Find(symptom => (symptom.Id == long.Parse(val))));
                        }

                        diseasesList.Add(new Virus(id, name, tmpSymptomsList));
                    }
                }
            }
            else
                Console.WriteLine($"File {fileName} doesn't exist.");
        }

        /// <summary>
        /// A method that writes all stored information about people within a file to a program
        /// </summary>
        private static void ReadPersons()
        {
            Console.WriteLine("Loading people data...");
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\files\");
            string fileName = "persons.txt";
            if(File.Exists(path + fileName))
            {
                using(StreamReader reader = File.OpenText(path + fileName))
                {
                    string line;
                    while((line = reader.ReadLine()) != null)
                    {
                        long id = long.Parse(line);
                        string firstName = reader.ReadLine();
                        string lastName = reader.ReadLine();
                        int age = Int32.Parse(reader.ReadLine());
                        long countyID = long.Parse(reader.ReadLine());
                        long diseaseID = long.Parse(reader.ReadLine());

                        string value = reader.ReadLine();
                        string[] delimiter = value.Split(',');

                        if(delimiter[0].Equals("0"))
                        {
                            personsList.Add(new Person.PersonBuilder(id, firstName, lastName)
                                                            .HasAge(age)
                                                            .HasCounty(countiesList.Find(county => (county.Id == countyID)))
                                                            .HasDisease(diseasesList.Find(disease => (disease.Id == diseaseID)))
                                                            .Build());
                        }
                        else
                        {
                            List<Person> tmpPersonList = new List<Person>();
                            foreach (string val in delimiter)
                            {
                                tmpPersonList.Add(personsList.Find(person => (person.Id == long.Parse(val))));
                            }
                            personsList.Add(new Person.PersonBuilder(id, firstName, lastName)
                                                            .HasAge(age)
                                                            .HasCounty(countiesList.Find(county => (county.Id == countyID)))
                                                            .HasDisease(diseasesList.Find(disease => (disease.Id == diseaseID)))
                                                            .HasContactedPersonsList(tmpPersonList)
                                                            .Build());
                        }
                    }
                }
            }
            else
                Console.WriteLine($"File {fileName} doesn't exist.");
        }

        /// <summary>
        /// A method for serialization of county objects with more than 0.5% affected population
        /// </summary>
        private static void SerializationOfCounties()
        {
            List<County> filterCountiesList = (from county in countiesList
                                               where county.PercentageOfAffectedPopulation > 0.5
                                               select county).ToList();
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\files\countySerialization.json");
            string jsonString = JsonConvert.SerializeObject(filterCountiesList);
            File.WriteAllText(path, jsonString);
        }

        /// <summary>
        /// A method for deserialization of county objects with more than 0.5% affected population
        /// </summary>
        private static void DeserializationOfCounties()
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\files\countySerialization.json");
            string json = File.ReadAllText(path);
            List<County> filteredList = JsonConvert.DeserializeObject<List<County>>(json);
            Console.WriteLine("List of counties with more than 0.5% affected population: ");
            filteredList.ForEach(county =>
            {
                Console.WriteLine(county.Id + " " + county.Name + " -> " + county.PercentageOfAffectedPopulation + "%.");
            });
        }

        /// <summary>
        /// A method in which all entered data is displayed inside of the console
        /// </summary>
        private static void PrintAllInfo()
        {
            Console.WriteLine("\nA LIST OF PEOPLE WITH ALL UPDATED INFORMATIONS:");
            for(int i = 0; i < personsList.Count; i++)
            {
                Console.WriteLine("Full name: {0} {1}", personsList[i].FirstName, personsList[i].LastName);
                Console.WriteLine("Age: {0}", personsList[i].Age);
                Console.WriteLine("County of residence: {0}", personsList[i].County.Name);
                Console.WriteLine("Infected with the {0}: {1}", personsList[i].Disease is Virus ? "virus" : "disease", personsList[i].Disease.Name);
                Console.WriteLine("List of contacted people: ");
                if(i == 0 || personsList[i].ContactedPersonsList == null)
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
                           where person.LastName.ToLower().Contains(input)
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
                if(personsList[selection-1].Equals(tmpPersonsList[i]))
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

        /// <summary>
        /// A method that prohibits an empty input
        /// </summary>
        /// <param name="entityNum">Number of entities user is trying to enter</param>
        private static void CheckIfZero(int entityNum)
        {
            if (entityNum <= 0)
                throw new ZeroNumberOfEntitiesException("You must enter at least 1 entity.");
        }

    }
}
