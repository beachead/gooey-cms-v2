using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Gooeycms.Data.Model.Subscription
{
    public class RegistrationDao : BaseDao
    {
        public Registration FindByGuid(String guid)
        {
            String hql = "select registration from RegistrationDto registration where registration.Guid = :guid";
            RegistrationDto dto = base.NewHqlQuery(hql).SetString("guid", guid).UniqueResult<RegistrationDto>();

            Registration result = null;

            if (dto != null)
            {
                MemoryStream stream = new MemoryStream(dto.Data);
                BinaryFormatter bf = new BinaryFormatter();
                stream.Position = 0;
                using (stream)
                {
                    result = (Registration)bf.Deserialize(stream);
                }
            }

            return result;
        }

        public void Save(Registration item)
        {
            MemoryStream stream = new MemoryStream();
            BinaryFormatter bf = new BinaryFormatter();
            try
            {
                bf.Serialize(stream, item);
                byte[] data = stream.ToArray();

                RegistrationDto dto = new RegistrationDto();
                dto.Guid = item.Guid;
                dto.Email = item.Email;
                dto.Created = item.Created;
                dto.IsComplete = item.IsComplete;
                dto.Data = data;

                SaveObject(dto);
            }
            finally
            {
                stream.Close();
            }
        }
    }
}
