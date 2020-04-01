using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace tuto2
{
    class Reader
    {
        private Boolean CheckStudent(string[] student)
        {
            if (student.Length != 9)
            {
                return false;
            }
            foreach (string field in student)
            {
                if (string.IsNullOrEmpty(field))
                {
                    return false;
                }
            }
            return true;
        }

        private Student CreateStudent(string[] student, string str)
        {
            DateTime birthDate;
            try
            {
                birthDate = DateTime.Parse(student[5]);
            }
            catch (Exception)
            {
                Logger.LogDataError(str, "birthDate");
                return null;
            }
            return new Student
            {
                Name = student[0],
                Surname = student[1],
                Studies = new Studies
                {
                    Name = student[2],
                    Mode = student[3]
                },
                Index = student[4],
                BirthDate = birthDate,
                Email = student[6],
                MothersName = student[7],
                FathersName = student[8]
            };
        }

        public University ReadFile(string path)
        {
            HashSet<Student> studentsSet;
            HashSet<Subject> activeStudies;
            try
            {
                using var stream = new StreamReader(File.OpenRead(path));

                studentsSet = new HashSet<Student>();
                activeStudies = new HashSet<Subject>();
                string line = null;
                while ((line = stream.ReadLine()) != null)
                {
                    string[] studentParts = line.Split(',');
                    if (!CheckStudent(studentParts))
                    {
                        Logger.LogMissingData(line);
                    }
                    else
                    {
                        Student student = CreateStudent(studentParts, line);
                        if (studentsSet.Contains(student))
                        {
                            Logger.LogAlreadyExists(student);
                        }
                        else
                        {
                            studentsSet.Add(student);

                            string studiesName = student.Studies.Name;
                            Subject subject = new Subject
                            {
                                Name = studiesName
                            };
                            activeStudies.TryGetValue(subject, out Subject present);
                            if (present != null)
                            {
                                present.NumberOfStudents += 1;
                            } else
                            {
                                subject.NumberOfStudents = 1;
                                activeStudies.Add(subject);
                            }
                        
                        }
                    }
                }
            }
            catch (FileNotFoundException)
            {
                Logger.LogFileNotFound(path);
                return null;
            }
            catch (ArgumentException)
            {
                Logger.LogIncorrectPath(path);
                return null;
            }
            return new University
            {
                Studetns = studentsSet,
                ActiveStudies = activeStudies
            };
        }
    }
}
