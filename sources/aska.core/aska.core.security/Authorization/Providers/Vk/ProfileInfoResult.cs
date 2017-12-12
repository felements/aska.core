using System.Collections.Generic;

namespace aska.core.security.Authorization.Providers.Vk
{
    public class ProfileInfoResult
    {
        public ProfileInfoResult()
        {
            
        }

        public List<responseData> response { get; set; }
        public errorData error { get; set; }

        public struct location
        {
            public int id { get; set; }
            public string title { get; set; }
        }
        public class errorData
        {
            public int error_code { get; set; }
            public string error_msg { get; set; }
        }

        public class responseData
        {
            public responseData()
            {
                
            }

            public string id { get; set; }

            public string first_name { get; set; }
            public string last_name { get; set; }
            //public string screen_name { get; set; }

            /// <summary>
            ///   1 — женский;
            ///   2 — мужской;
            ///   0 — пол не указан. 
            /// </summary>
            public int sex { get; set; }
            public string bdate { get; set; }
            //public string home_town { get; set; }
            //public location country { get; set; }
            //public location city { get; set; }
            //public string status { get; set; }
            public string phone { get; set; }
            public string photo_50 { get; set; }

            /// <summary>
            /// 1 — не женат/не замужем;
            /// 2 — есть друг/есть подруга;
            /// 3 — помолвлен/помолвлена;
            /// 4 — женат/замужем;
            /// 5 — всё сложно;
            /// 6 — в активном поиске;
            /// 7 — влюблён/влюблена;
            /// 8 — в гражданском браке;
            /// 0 — не указано.
            /// </summary>
            //public int relation { get; set; }
        }

    }

    
}