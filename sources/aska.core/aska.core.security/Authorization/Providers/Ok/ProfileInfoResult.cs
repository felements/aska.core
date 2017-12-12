namespace aska.core.security.Authorization.Providers.Ok
{
    public class ProfileInfoResult
    {
        /*"{\"error_code\":101,\"error_msg\":\"PARAM_API_KEY : No application key\",\"error_data\":null}"*/

        public string error_code { get; set; }
        public string error_msg { get; set; }
        public string error_data { get; set; }



        //{"uid":"574098034399","birthday":"1987-01-01","name":"ksc manager","locale":"en","gender":"male","pic50x50":"https://i.mycdn.me/res/stub_50x50.gif","first_name":"ksc","last_name":"manager"}

        public string uid { get; set; }
        public string birthday { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string name { get; set; }
        public string locale { get; set; }
        public string gender { get; set; }
        public string pic50x50 { get; set; }
    }
}