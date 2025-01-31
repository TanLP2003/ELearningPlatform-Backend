using Microsoft.AspNetCore.DataProtection.KeyManagement;
using PaymentService.API.Applications.Services;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace PaymentService.API.InterbankSubsystem.VNPay
{
    public class VNPayService(IConfiguration configuration) : IPaymentService
    {
        public string GeneratePaymentUrl(int amount, string orderInfo)
        {
            string vnp_Version = "2.1.0";
            string vnp_Command = "pay";
            string orderType = "other";
            int vnpAmount = amount * 100;
            string vnpReturnUrl = configuration["VNPay:ReturnUrl"];
            //Console.WriteLine($"VNP Return URL: {vnpReturnUrl}");
            string vnp_TxnRef = DateTime.Now.Ticks.ToString();
            string vnp_IpAddr = configuration["VNPay:IpAddr"];
            //Console.WriteLine($"VNP IP Addr: {vnp_IpAddr}");
            string vnp_TmnCode = configuration["VNPay:TmnCode"];
            //Console.WriteLine($"TMN Code: {vnp_TmnCode}");
            string secretKey = configuration["VNPay:HashSecret"];
            string vnpPayUrl = configuration["VNPay:BaseUrl"];

            var vnp_Params = new Dictionary<string, string>
            {
                { "vnp_Version", vnp_Version },
                { "vnp_Command", vnp_Command },
                { "vnp_TmnCode", vnp_TmnCode },
                { "vnp_Amount", vnpAmount.ToString() },
                { "vnp_CurrCode", "VND" },
                { "vnp_BankCode", "ncb" },
                { "vnp_TxnRef", vnp_TxnRef },
                { "vnp_OrderInfo", orderInfo },
                { "vnp_OrderType", orderType },
                { "vnp_Locale", "vn" },
                { "vnp_ReturnUrl", vnpReturnUrl },
                { "vnp_IpAddr", vnp_IpAddr }
            };

            var timeZoneById = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
            var timeNow = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timeZoneById);
            var vnp_CreateDate = timeNow.ToString("yyyyMMddHHmmss");
            vnp_Params.Add("vnp_CreateDate", vnp_CreateDate);

            var expireDate = timeNow.AddMinutes(15);
            var vnp_ExpireDate = expireDate.ToString("yyyyMMddHHmmss");
            vnp_Params.Add("vnp_ExpireDate", vnp_ExpireDate);

            var fieldNames = vnp_Params.Keys.ToList();
            fieldNames.Sort();

            var hashData = new StringBuilder();
            var query = new StringBuilder();

            foreach (var fieldName in fieldNames)
            {
                var fieldValue = vnp_Params[fieldName];
                if (!string.IsNullOrEmpty(fieldValue))
                {
                    // Build hash data
                    hashData.Append(fieldName);
                    hashData.Append('=');
                    hashData.Append(WebUtility.UrlEncode(fieldValue));

                    // Build query
                    query.Append(WebUtility.UrlEncode(fieldName));
                    query.Append('=');
                    query.Append(WebUtility.UrlEncode(fieldValue));

                    if (fieldName != fieldNames.Last())
                    {
                        query.Append('&');
                        hashData.Append('&');
                    }
                }
            }

            string queryUrl = query.ToString();
            string vnp_SecureHash = HmacSHA512(secretKey, hashData.ToString());
            queryUrl += "&vnp_SecureHash=" + vnp_SecureHash;
            return vnpPayUrl + "?" + queryUrl;
        }

        private string HmacSHA512(string key, string data)
        {
            try
            {
                if (key == null || data == null)
                {
                    throw new ArgumentNullException();
                }

                byte[] keyBytes = Encoding.UTF8.GetBytes(key);
                byte[] dataBytes = Encoding.UTF8.GetBytes(data);

                using (var hmac = new HMACSHA512(keyBytes))
                {
                    byte[] result = hmac.ComputeHash(dataBytes);

                    var sb = new StringBuilder();
                    foreach (byte b in result)
                    {
                        sb.Append(b.ToString("x2")); // "x2" sẽ in số hex với 2 chữ số
                    }

                    return sb.ToString();
                }
            }
            catch (Exception)
            {
                return "";
            }
        }
    }
}
