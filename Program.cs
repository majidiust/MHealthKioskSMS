using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SMSJobHandler
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                try
                {
                    DatabaseDataContext db = new DatabaseDataContext();
                    foreach (var x in db.VisitDescribtions)
                    {
                       
                        if (x.SendSMS == null || x.SendSMS == false)
                        {
                            Console.WriteLine("new job : " + x.NationalityCode + ":" + x.MobileNumber);
                            String msg = "فشارخون و درصد اکسیژن خون شما اندازه گیری شده است، خواهشمند است کد زیر را از طریق پیامک کوتاه به شماره 30002666000220 ارسال نمایید.";
                            msg += "کد شما : " + x.GeneratedCode + "است";
                            SendSmsEvent(new string[] { x.MobileNumber }, msg);
                            x.SendSMS = true;
                            db.SubmitChanges();
                        }
                    }
                    System.Threading.Thread.Sleep(5000);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        public static void SendSmsEvent(string[] to, string message)
        {
            message += " همراه اول سلامت";
            SMSService.ArrayOfLong rec = new SMSService.ArrayOfLong();
            byte[] status = null;
            SMSService.SendSoapClient SMSClient = new SMSService.SendSoapClient();
            SMSService.ArrayOfString tos = new SMSService.ArrayOfString();
            foreach (var str in to)
                tos.Add(str);
            SMSClient.SendSms("msadeghi", "76049270", tos, "30002666000220", message, false, "", ref rec, ref status);
        }
    }
}
