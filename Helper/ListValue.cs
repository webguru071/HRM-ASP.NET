using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMSApp.Helper
{
    public class ListValue
    {
        public ListValue()
        {

        }
        public static Dictionary<string, char> Gender = new Dictionary<string, char>()
        {
            {"MALE", 'M'},
            {"FEMALE", 'F'},
            {"OTHER",'O' }
        };
        public static Dictionary<string, int> IsActive = new Dictionary<string, int>()
        {
            {"YES", 1},
            {"NO", 0},
        };
        public static Dictionary<string, string> Status = new Dictionary<string, string>()
        {
            {"ACTIVE", "a"},
            {"DEACTIVE", "d"},
        };

        public static Dictionary<string, char> Religion = new Dictionary<string, char>()
        {
            {"Islam", 'i'},
            {"Hindu", 'h'},
            {"Christian", 'c'},
            {"Buddhism", 'b'},
            {"Others", 'o'},
        };

        public static Dictionary<string, string> BloodGroup = new Dictionary<string, string>()
        {
            {"A+", "A+"},
            {"A-", "A-"},
            {"B+", "B+"},
            {"B-", "B-"},
            {"AB+","AB"},
            {"AB-","AB"},
            {"O+", "O+"},
            {"O-", "O-"},
        };
        public static Dictionary<int, int> ClassLevel = new Dictionary<int, int>()
        {
            {1, 1},
            {2, 2},
            {3, 3},
            {4, 4},
            {5,5},
            {6,6},
            {7, 7},
            {8, 8},
            {9, 9},
            {10, 10},
        };
        public static Dictionary<string, char> ExamType = new Dictionary<string, char>()
        {
            {"Mid Exam", 'm'},
            {"Final Exam", 'f'},
        };
        public static Dictionary<string, char> CourseType = new Dictionary<string, char>()
        {
            {"Main Subject", 'm'},
            {"Optional Subject", 'o'},
        };
        public static Dictionary<string, char> SubjectCategory = new Dictionary<string, char>()
        {
            {"General", 'g'},
            {"Science", 's'},
            {"Arts", 'a'},
            {"Commerce", 'c'},
        };
        public static Dictionary<string, string> Month = new Dictionary<string, string>()
        {
            {"January", "January"},
            {"February", "February"},
            {"March", "March"},
            {"April", "April"},
            {"May", "May"},
            {"June", "June"},
            {"July", "July"},
            {"August", "August"},
            {"September", "September"},
            {"October", "October"},
            {"November", "November"},
            {"December", "December"},
        };
        public static Dictionary<string, int> Year = new Dictionary<string, int>()
    {
        {"2001",2001 },
        {"2002",2002 },
        {"2003",2003 },
        {"2004",2004 },
        {"2005",2005 },
        {"2006",2006 },
        {"2007",2007 },
        {"2008",2008 },
        {"2009",2009 },
        {"2010",2010 },
        {"2011",2011 },
        {"2012",2012 },
        {"2013",2013 },
        {"2014",2014 },
        {"2015",2015 },
        {"2016",2016 },
        {"2017",2017 },
        {"2018",2018 },
        {"2019",2019 },
        {"2020",2020 },
        {"2021",2021 },
        {"2022",2022 },
        {"2023",2023 },
        {"2024",2024 },
        {"2025",2025 },
        {"2026",2026 },
        {"2027",2027 },
        {"2028",2028 },
        {"2029",2029 },
        {"2030",2030 },
        {"2031",2031 },
        {"2032",2032 },
        {"2033",2033 },
        {"2034",2034 },
        {"2035",2035 },
        {"2036",2036 },
        {"2037",2037 },
        {"2038",2038},
        {"2039",2039 },
        {"2040",2040 },

    };
    }
}