using WebApplication1.Model;

namespace WebApplication1
{
    public static class ItemsMapper
    {
        public static List<PersonalInformation> MapFromRangeData(IList<IList<object>> values)
        {
            var items = new List<PersonalInformation>();
            int count = 0;
            foreach (var value in values)
            {
                if(count == 0)
                {
                    count++;
                    continue;
                }
                PersonalInformation item = new()
                {
                    Name = value[0].ToString(),
                    DateOfBirth = value[1].ToString(),
                    ResidentialAddress = value[2].ToString(),
                    PermanentAddress = value[3].ToString(),
                    MobileNumber = value[4].ToString(),
                    EmailAddress = value[5].ToString(),
                    MaritalStatus = value[6].ToString(),
                    Gender = value[7].ToString(),
                    Occupation = value[8].ToString(),
                    AadharCardNumber = value[9].ToString(),
                    PANNumber = value[10].ToString(),
                };
                items.Add(item);
            }

            return items;
        }

        public static IList<IList<object>> MapToRangeData(PersonalInformation item)
        {
            var objectList = new List<object>() { item.Name, item.DateOfBirth, item.ResidentialAddress, item.PermanentAddress, item.MobileNumber, item.EmailAddress, item.MaritalStatus, item.Gender, item.Occupation, item.AadharCardNumber, item.PANNumber };
            var rangeData = new List<IList<object>> { objectList };
            return rangeData;
        }
    }
}
